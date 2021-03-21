using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net;
using System.Net.Sockets;
using System.Windows.Threading;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Controls;
using NetTest;

namespace NetClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static Socket _ClientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private Dictionary<string,
        RadioButton> rblist = new Dictionary<string,
        RadioButton>();
        private List<Image> ticGrid = new List<Image>();
        private static string outp = "";
        public static string outp2 = "";

        private static string username = "";
        public static string against = "";

        private static bool check = false;
        private static bool tried = false;
        private static bool firststart = false;
        private static bool nospam = false;
        private static byte[] _buff = new byte[1024];
        private int count = 0;
        private static int SendDelay = 0;
        private static string splitusrs = "";
        public static Visibility ticshow = Visibility.Hidden;
        public static List<Image> TTG = new List<Image>();

        private static Button acceptb;
        public static Button quitb;

        public static Label startas = null;
        public static Label currturn = null;


        public static Grid maingrid = null;
        public static Window mainWindow = null;

        public static Image TCirc = null;
        public static Image TCros = null;
        public static Image TBlank = null;
        public static Image TPas = null;
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer zetimer = new DispatcherTimer();
            zetimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            zetimer.Tick += new EventHandler(onUpdate);
            zetimer.Start();

            //ticgrid = TicTac;
            TCirc = TempCircle;
            TCros = TempCross;
            TBlank = TempBlank;
            TPas = TempCross_Copy;
            quitb = qbutton;
            acceptb = invacpt;
            startas = playas;
            currturn = turnlabel;
            maingrid = deGrid;
            mainWindow = MainWind;
            mainWindow.Height = 126;
            deGrid.Height = 338;
            mainWindow.Width = 336;
            deGrid.Width = 333;


        }

        private void onUpdate(object sender, EventArgs e)
        {
            if (users.SelectedItem != null && users.SelectedItem.ToString() != username && users.SelectedItem.ToString() != "")
            {
                invtic.IsEnabled = true;

            }
            if (users.SelectedItem == null || users.SelectedItem.ToString() == username)
            {
                invtic.IsEnabled = false;
            }
            if (nospam == true)
            {
                SendDelay++;
                if (SendDelay >= 30)
                {

                    SendDelay = 0;
                    nospam = false;
                }
            }
            if (TicTac.Visibility == Visibility.Visible && !users.Items.Contains(against))
            {
                zTicTac.TicTacReset("Quitz");
            }
            if (_ClientSock.Connected)
            {
                ipadr.Visibility = Visibility.Hidden;
                button.Visibility = Visibility.Hidden;
                invng.Visibility = Visibility.Visible;
                invtic.Visibility = Visibility.Visible;
                if (!firststart)
                {
                    firststart = true;

                    if (outp2 != "")
                    {
                        textBox.Text += outp2 + "\n";
                        outp2 = "";
                    }

                    textBox.ScrollToEnd();
                    /*
                              string[] temphold = textBox.Text.Split('\n');
                              if(temphold.Length == 10)
                              {
                                  for(int i = 5; i < 10; i++)
                                  {
                                      //textBox.Text = temphold[i] + "\n";
                                  }
                              }
                          */
                }
                if (users.Items.Count != splitusrs.Split(':').Length)
                {
                    if (users.Items.Count != 0)
                    {
                        users.Items.Clear();
                    }
                    if (splitusrs.Length > 0)
                    {

                        string[] zspitz = splitusrs.Split(':');
                        foreach (string dis in zspitz)
                        {
                            users.Items.Add(dis);
                        }

                    }

                }
                if (count >= 30)
                {
                    tried = false;
                    SendLoop("!lupdate");
                    count = 0;
                }
                TicTac.Visibility = ticshow;
                count++;
            }
            try
            {
                if (_ClientSock.Poll(500, SelectMode.SelectRead) && check)
                {
                    mainWindow.Width = 336;
                    deGrid.Width = 333;
                    mainWindow.Height = 126;
                    deGrid.Height = 338;
                    if (tried)
                    {
                        tboks.Content = "Username already in use!";
                        usern.IsEnabled = true;
                        invng.Visibility = Visibility.Hidden;
                        invtic.Visibility = Visibility.Hidden;
                        ipadr.Visibility = Visibility.Visible;
                        button.Visibility = Visibility.Visible;
                        if (username != usern.Text)
                        {
                            button.IsEnabled = true;

                            tried = false;
                            check = false;
                        }

                    }
                    if (!_ClientSock.Connected && !tried && !usern.IsEnabled && !button.IsEnabled)
                    {

                        invng.Visibility = Visibility.Hidden;
                        invtic.Visibility = Visibility.Hidden;
                        ipadr.Visibility = Visibility.Visible;
                        button.Visibility = Visibility.Visible;
                        tboks.Content = "Error: Couldnt connect to ipaddress.";
                        usern.IsEnabled = true;
                        button.IsEnabled = true;
                        check = false;
                    }
                    users.Items.Clear();
                    _ClientSock.Dispose();
                    _ClientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                }
            }
            catch (ObjectDisposedException){}
            if (TicTac.Visibility == Visibility.Visible && TTG.Count == 0)
            {
                foreach (UIElement img in TicTac.Children)
                {
                    if (img.GetType() == typeof(Image))
                    {
                        Image dimg = (Image)img;
                        if (dimg.Name.StartsWith("D"))
                        {
                            TTG.Add(dimg);
                        }
                    }

                }
            }
        }
        private static string ConnectToHost(string ipadr)
        {
            check = true;
            int attempts = 0;
            while (!_ClientSock.Connected)
            {
                try
                {
                    attempts++;
                    _ClientSock.Connect(ipadr, 16666);
                }
                catch (SocketException)
                {
                    outp = "Attempting to Connect: " + attempts.ToString();
                }
            }
            if (_ClientSock.Connected)
            {
                mainWindow.Width = 446;
                maingrid.Width = 436;
                mainWindow.Height = 320;
                maingrid.Height = 312;
                outp = "Connected";
                tried = true;
            }
            return outp;

        }
        private void UpdateTicTac(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Image))
            {
                Image img = (Image)sender;
                zTicTac.TicTacHandler(img.Name);
            }

        }
        public static void SendLoop(string input)
        {
            Task f = Task.Factory.StartNew(() => {

                if (input.Contains("@"))
                {
                    input = username + input;
                    outp2 = "MSG:" + input;
                }
                if (input.StartsWith("!accept"))
                {
                    ticshow = Visibility.Visible;
                    zTicTac.first = true;
                    zTicTac.TicTacHandler("Enabled");
                }
                if (input.StartsWith("!end"))
                {
                    against = "";
                    ticshow = Visibility.Hidden;
                }
                if (input.StartsWith("!ttset:"))
                {
                    input += ":" + against;
                }
                if (input.StartsWith("!tictac:"))
                {
                    string stemp = input.Replace("!tictac:", "");
                    string[] spltemp = stemp.Split('@', ':');
                    against = spltemp[1];
                }
                byte[] buffer = Encoding.ASCII.GetBytes(input);
                _ClientSock.Send(buffer);
                while (true && _ClientSock.Connected)
                {
                    byte[] recvbuff = new byte[1024];
                    int rec = _ClientSock.Receive(recvbuff);
                    byte[] data = new byte[rec];
                    Array.Copy(recvbuff, data, rec);
                    string datemp = Encoding.ASCII.GetString(data);

                    firststart = false;
                    if (datemp.StartsWith("!tictacinv:"))
                    {
                        string remcom = datemp.Replace("!tictacinv:", "");
                        string[] username = remcom.Split('>', ':');
                        outp2 = "Invited By: " + username[0] + " to Play EggsNCrosses.";
                        Application.Current.Dispatcher.Invoke(() => {
                            acceptb.Visibility = Visibility.Visible;
                            acceptb.IsEnabled = true;
                        });

                        if (against != username[0])
                        {
                            against = username[0];
                        }
                    }
                    if (datemp.StartsWith("!lusrlist"))
                    {
                        string ripcom = datemp.Replace("!lusrlist:", "");
                        splitusrs = ripcom;

                    }
                    if (datemp.StartsWith("!NGB:"))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            NewGame.ThisGameFunction(datemp);
                        });
                    }
                    if (datemp.Contains("!tictacquit:"))
                    {
                        outp2 = "EggsnCrosses Game Closed.";
                        zTicTac.TicTacReset("Quitz");
                    }

                    if (datemp.StartsWith("!accepted"))
                    {
                        outp2 = "EggsnCrosses Invite Accepted.";
                        ticshow = Visibility.Visible;
                        zTicTac.TicTacHandler("Enabled");
                    }
                    if (datemp.StartsWith("!ttset:"))
                    {
                        string remcom = datemp.Replace("!ttset:", "");
                        string[] tdurn = remcom.Split(':');
                        zTicTac.RecMov("TT:" + tdurn[0] + ":" + tdurn[1]);
                    }
                    if (datemp.Contains("MSG"))
                    {
                        outp2 = Encoding.ASCII.GetString(data);
                    }
                    if (datemp.StartsWith("@"))
                    {
                        outp2 = "MSG" + Encoding.ASCII.GetString(data);
                    }
                    if (!datemp.Contains("!lusrlist") && !datemp.Contains("!accepted") && !datemp.Contains("!tictacinv:") && !datemp.Contains("!ttset:") && !datemp.Contains("!tictacquit:") && !datemp.Contains("!NGB:") && !datemp.Contains("MSG") && datemp != ("") && !datemp.StartsWith("@"))
                    {
                        outp2 = "SRV: " + Encoding.ASCII.GetString(data);
                    }

                }

            });

        }
        /*
                private static void RecvLoop(IAsyncResult ares)
                {


                    Socket socket = (Socket)ares.AsyncState;
                    int received = socket.EndReceive(ares);

                    byte[] databuff = new byte[received];
                    Array.Copy(_buff, databuff, received);

                    string ByteConv = Encoding.ASCII.GetString(databuff);
                    socket.BeginReceive(_buff, 0, _buff.Length, SocketFlags.None, new AsyncCallback(RecvLoop), _ClientSock);

                }
            */
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (ipadr.Text != "" && ipadr.Text != "IPadress")
            {
                tboks.Content = ConnectToHost(ipadr.Text);
                button.IsEnabled = false;
                usern.IsEnabled = false;
                SendLoop("usr:" + usern.Text);
                username = usern.Text;
            }
            if (ipadr.Text == "IPadress")
            {
                tboks.Content = ConnectToHost("127.0.0.1");
                button.IsEnabled = false;
                usern.IsEnabled = false;
                SendLoop("usr:" + usern.Text);
                username = usern.Text;
            }
        }

        private void button_Click1(object sender, RoutedEventArgs e)
        {
            if (!nospam)
            {
                nospam = true;
                SendLoop(IO.Text);
            }

        }
        private void buttonenable(object sender, RoutedEventArgs e)
        {
            if (!_ClientSock.Connected && !usern.Text.Contains("name"))
            {
                button.IsEnabled = true;
            }

        }

        private void SetupTicTac(object sender, RoutedEventArgs e)
        {
            TicTac.Visibility = Visibility.Visible;
        }

        private void setu1_checked(object sender, RoutedEventArgs e)
        {
            if (users.SelectedItem != null && users.SelectedItem.ToString() != "")
            {
                IO.Text = "@" + users.SelectedItem.ToString() + ":";
                users.SelectedItem = null;
            }
        }

        private void invtic_Click(object sender, RoutedEventArgs e)
        {
            if (users.SelectedItem != null && users.SelectedItem.ToString() != "" && users.SelectedItem.ToString() != username && !nospam)
            {
                nospam = true;
                SendLoop("!tictac:" + username + ":" + users.SelectedItem.ToString() + ":");
                textBox.Text += "Invited: " + users.SelectedItem.ToString() + " to play a game of Eggs N Crosses" + "\n";
                users.SelectedItem = null;
            }
        }

        private void invacpt_Click(object sender, RoutedEventArgs e)
        {
            if (!nospam)
            {

                nospam = true;
                SendLoop("!accept");
                textBox.Text += "Accepted Invite." + "\n";
                invacpt.Visibility = Visibility.Hidden;
                invacpt.IsEnabled = false;
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            outp2 = "Eggs n Crosses Game Quit.";
            qbutton.Visibility = Visibility.Hidden;
            zTicTac.TicTacReset("Quit");

        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void invitenewgame_Click(object sender, RoutedEventArgs e)
        {
            NewGame newgame = new NewGame();
            newgame.Show();
        }
    }
}