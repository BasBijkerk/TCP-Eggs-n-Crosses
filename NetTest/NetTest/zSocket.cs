using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Threading;

namespace NetTest
{
    class zSocket
    {

        private static byte[] _buff = new byte[1024];
        private static Dictionary<string,
        Socket> _ClientSocks = new Dictionary<string,
        Socket>();
        private static Dictionary<int,
        string> ActivTicTac = new Dictionary<int,
        string>();
        private static Socket _ServerSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static bool once = false;
        public static string conio = "";
        private static int lastcount = 0;

        public static Socket zSender;
        public static Socket zReceiver;
        public static void ServerSetup()
        {
            Console.WriteLine("Server Setting up.");
            conio += "Server Setting up." + "\n";
            _ServerSock.Bind(new IPEndPoint(IPAddress.Any, 16666));
            _ServerSock.Listen(5);
            _ServerSock.BeginAccept(new AsyncCallback(CallbackAccept), null);

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 200);
            dt.Tick += new EventHandler(OnUpdate);
            dt.Start();

        }
        public static void ServerCmds(string recv)
        {
            if (recv.StartsWith("kick"))
            {
                string username = recv.Replace("kick ", "");
                Socket holder;
                _ClientSocks.TryGetValue(username, out holder);
                holder.Shutdown(SocketShutdown.Both);
                holder.Disconnect(true);
            }
            if (recv.StartsWith("say"))
            {
                string msg = recv.Replace("say ", "");
                byte[] data = Encoding.ASCII.GetBytes(msg);
                foreach(KeyValuePair<string,Socket> kp in _ClientSocks)
                {
                    kp.Value.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), kp.Value);
                }
            }
            if (recv.StartsWith("help"))
            {                                  
                Console.WriteLine("--------------Commands--------------");
                conio += "--------------Commands--------------" + "\n";
                Console.WriteLine("kick playername (to kick player)");
                conio += "kick playername (to kick player)" + "\n";
                Console.WriteLine("say message (message every connected client)");
                conio += "say message (message every connected client)" + "\n";
            }
        }

        private static void OnUpdate(object sender, EventArgs e)
        {
            if (once)
            {
                once = false;
                Dictionary<string,
                Socket> d = _ClientSocks;
                if (d.ToArray().Length > 0)
                {

                    try
                    {

                        foreach (KeyValuePair<string, Socket> s in d)
                        {

                            if (s.Value != null)
                            {
                                Socket z = (Socket)s.Value;

                                if (!z.Connected)
                                {

                                    _ClientSocks.Remove(s.Key);

                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Info: " + "Disconnected user Removed.");
                        conio += "Info: " + "Disconnected user Removed." + "\n";
                    }
                }
            }
            if (!_ServerSock.Connected)
            {
                MainWindow.ButtonHost();
            }
        }

        private static void CallbackAccept(IAsyncResult ares)
        {
            Socket socket = _ServerSock.EndAccept(ares);
            socket.BeginReceive(_buff, 0, _buff.Length, SocketFlags.None, new AsyncCallback(CallbackReceive), socket);
            Console.WriteLine("Client Connected: " + socket.Handle.ToString());
            conio += "Client Connected: " + socket.Handle.ToString() + "\n";
            _ServerSock.BeginAccept(new AsyncCallback(CallbackAccept), socket);
        }

        private static void CallbackReceive(IAsyncResult ares)
        {
            try
            {
                once = true;

                Socket socket = (Socket)ares.AsyncState;
                int received = socket.EndReceive(ares);

                byte[] databuff = new byte[received];
                Array.Copy(_buff, databuff, received);

                string ByteConv = Encoding.ASCII.GetString(databuff);

                if (!ByteConv.ToLower().Contains("!lupdate"))
                {
                    Console.WriteLine("MSG RECV: " + ByteConv);
                    conio += "MSG RECV: " + ByteConv + "\n";
                }

                string response = string.Empty;

                string contents = ByteConv.ToLower();
                if (contents.StartsWith("usr:"))
                {
                    string username = contents.Replace("usr:", "");
                    if (_ClientSocks.ContainsKey(username) == false)
                    {

                        _ClientSocks.Add(username, socket);

                        response = "username assigned: " + username;

                        byte[] data = Encoding.ASCII.GetBytes(response);

                        socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), socket);
                    }
                    else
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Disconnect(true);
                        if (!socket.Connected)
                        {
                            
                        }
                    }
                }
                if (contents.StartsWith("me:"))
                {
                    string username = contents.Replace("me:", "");
                    Socket DictLoc;
                    _ClientSocks.TryGetValue(username, out DictLoc);
                    conio += username + "Requested identity validation." + "\n";
                    if (socket == DictLoc)
                    {
                        response = "your name: " + username;
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), socket);
                    }
                }
                if (contents.Contains("!ngc:"))
                {
                    //string username = contents.Replace("me:", "");
                    //Socket DictLoc;
                    //_ClientSocks.TryGetValue(username, out DictLoc);
                    conio += "NGC RECEIVED." + "\n";
                    
                        response = "!NGB:";
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), socket);
                   
                }
                if (contents.Contains('@') && contents.Contains(':'))
                {
                    string[] username = contents.Split('@', ':');
                    Socket DictLoc;
                    Console.WriteLine("Trying to Forward Message" + username[0] + ">" + username[1] + "@" + username[2]);
                    conio += "MSG:" + username[0] + ">" + username[1] + "@" + username[2] + "\n";
                    _ClientSocks.TryGetValue(username[1], out DictLoc);
                    if (_ClientSocks.ContainsKey(username[0]) && _ClientSocks.ContainsKey(username[1]))
                    {
                        response = "MSG:" + username[0] + ">" + username[1] + "@" + username[2];
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        DictLoc.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), DictLoc);
                    }
                }
                if (contents.Contains("@") && !contents.Contains(':'))
                {
                    string[] Message = contents.Split('@');
                    Console.WriteLine("Trying to Forward Global Message" + Message[1]);
                    conio += "Trying to Forward Global Message" + Message[1] + "\n";
                   
                    foreach(KeyValuePair<string,Socket> kp in _ClientSocks)
                    {
                        
                        if(kp.Value != socket)
                        {
                            response = "@Global " + Message[0] + ":" + Message[1];
                            byte[] data = Encoding.ASCII.GetBytes(response);
                            kp.Value.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), kp.Value);
                        }
                        
                    }
                }
                if (contents.StartsWith("!tictac:"))
                {
                    string remcom = contents.Replace("!tictac:", "");
                    string[] username = remcom.Split('@', ':');
                    Socket sender;

                    _ClientSocks.TryGetValue(username[0], out sender);
                    zSender = sender;
                    Socket receiver;
                    _ClientSocks.TryGetValue(username[1], out receiver);
                    zReceiver = receiver;

                    ActivTicTac.Add(ActivTicTac.Count, username[0] + ":" + username[1]);

                    Console.WriteLine("Sending TicTac Invite: " + username[0] + ">" + username[1]);
                    conio += "Sending TicTac Invite: " + username[0] + ">" + username[1] + "\n";
                    if (_ClientSocks.ContainsKey(username[0]) && _ClientSocks.ContainsKey(username[1]))
                    {
                        response = "!tictacinv:" + username[0] + ">" + username[1];
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        zReceiver.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), zReceiver);
                    }
                }
                if (contents.StartsWith("!accept"))
                {

                    Console.WriteLine("Accepting TicTact Invite: ");
                    conio += "Accepting TicTact Invite: " + "\n";
                    if (zSender.Connected && zReceiver.Connected)
                    {
                        response = "!accepted";
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        zSender.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), zSender);
                        zSender = null;
                        zReceiver = null;
                    }
                }
                if (contents.StartsWith("!ttset:"))
                {
                    string remcom = contents.Replace("!ttset:", "");
                    string[] tdurn = remcom.Split(':');

                    Console.WriteLine("TicTacMOVE: " + tdurn[0] + " : " + tdurn[1] + " : " + tdurn[2]);
                    conio += "TicTacMOVE: " + tdurn[0] + " : " + tdurn[1] + " : " + tdurn[2] + "\n";
                    Socket tempsock;
                    _ClientSocks.TryGetValue(tdurn[2], out tempsock);
                    if (tempsock.Connected)
                    {
                        response = "!ttset:" + tdurn[0] + " : " + tdurn[1];
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        tempsock.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), tempsock);

                    }

                }
                if (contents.Contains("!tictacquit:"))
                {
                    string remcom = contents.Replace("!tictacquit:", "");

                    Console.WriteLine("TicTacQuit: " + remcom);
                    conio += "TicTacQuit: " + remcom + "\n";
                    Socket tempsock;
                    _ClientSocks.TryGetValue(remcom, out tempsock);
                    if (tempsock.Connected)
                    {
                        Console.WriteLine("SEND MSG QUIT");
                        response = "!tictacquit:";
                        byte[] data = Encoding.ASCII.GetBytes(response);
                        tempsock.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), tempsock);

                    }

                }

                if (lastcount < _ClientSocks.Count || contents.StartsWith("!lupdate"))
                {
                    //Console.WriteLine("True");
                    response = "";
                    lastcount = _ClientSocks.Count;
                    foreach (KeyValuePair<string, Socket> kp in _ClientSocks)
                    {
                        if (kp.Value.Connected && kp.Value != null)
                        {
                            response += kp.Key + ":";
                        }

                    }
                    response = "!lusrlist:" + response;
                    byte[] data = Encoding.ASCII.GetBytes(response);
                    foreach (KeyValuePair<string, Socket> kp in _ClientSocks)
                    {
                        if (kp.Value.Connected && kp.Value != null)
                        {
                            kp.Value.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(CallbackSend), kp.Value);
                        }

                    }
                }
                socket.BeginReceive(_buff, 0, _buff.Length, SocketFlags.None, new AsyncCallback(CallbackReceive), socket);

            }
            catch (ObjectDisposedException)
            {

            }
            catch (SocketException)
            {
                Console.WriteLine("Error: " + "Client Disconnected or Force Quit");
                conio += "Error: " + "Client Disconnected or Force Quit" + "\n";
            }

        }

        private static void CallbackSend(IAsyncResult ares)
        {
            try
            {
                Socket socket = (Socket)ares.AsyncState;
                socket.EndSend(ares);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }

    }
}