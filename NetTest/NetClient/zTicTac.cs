using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace NetClient
{
    class zTicTac
    {
        public static bool first = false;
        public static int turn = 2;
        public static int lastturn = 0;
        public static bool canmov = true;
        private static DispatcherTimer newt;
        public static void TicTacReset(string winner)
        {
            canmov = false;
            Application.Current.Dispatcher.Invoke(() => {


                if (winner == "Quit")
                {
                    MainWindow.mainWindow.Height = 320;
                    MainWindow.maingrid.Height = 312;
                    MainWindow.maingrid.Width = 439;
                    MainWindow.mainWindow.Width = 446;
                    MainWindow.startas.Visibility = Visibility.Hidden;
                    MainWindow.ticshow = Visibility.Hidden;
                    MainWindow.quitb.Visibility = Visibility.Hidden;
                    MainWindow.quitb.IsEnabled = false;
                    MainWindow.SendLoop("!tictacquit:" + MainWindow.against);
                    WhosTurn("Quit");
                    first = false;
                    turn = 2;
                    lastturn = 0;
                    foreach (Image img in MainWindow.TTG)
                    {
                        img.Source = MainWindow.TBlank.Source;
                        MainWindow.TPas.Source = MainWindow.TBlank.Source;

                    }
                }
                if (winner == "Quitz")
                {
                    MainWindow.mainWindow.Height = 320;
                    MainWindow.maingrid.Height = 312;
                    MainWindow.maingrid.Width = 439;
                    MainWindow.mainWindow.Width = 446;
                    MainWindow.startas.Visibility = Visibility.Hidden;
                    MainWindow.ticshow = Visibility.Hidden;
                    MainWindow.quitb.Visibility = Visibility.Hidden;
                    MainWindow.quitb.IsEnabled = false;
                    WhosTurn("Quit");
                    first = false;
                    turn = 2;
                    lastturn = 0;
                    foreach (Image img in MainWindow.TTG)
                    {
                        img.Source = MainWindow.TBlank.Source;
                        MainWindow.TPas.Source = MainWindow.TBlank.Source;

                    }
                }


            });
            if (winner != "Quit" && winner != "Quitz")
            {
                if (winner.Contains("es"))
                {
                    MainWindow.SendLoop("The Winner is: " + winner);
                    MainWindow.outp2 = "The Winner is: " + winner;
                }
                
                if(winner == "Crosses")
                {
                    WhosTurn("Cros");
                }
                if(winner == "Circles")
                {
                    WhosTurn("Circ");
                }
                newt = new DispatcherTimer();
                newt.Interval = new TimeSpan(0, 0, 0, 1);
                newt.Tick += new EventHandler(timertick);
                newt.Start();

            }
            
            
        }

        private static void timertick(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => 
            { 
            for(int i = 0; i < 4; i++)
            {

                if(i == 2)
                {
                    
                    foreach (Image img in MainWindow.TTG)
                    {
                        img.Source = MainWindow.TBlank.Source;
                        MainWindow.TPas.Source = MainWindow.TBlank.Source;

                    }
                        
                }
                if(i == 3)
                {
                        
                        TicTacHandler("Enabled");
                        canmov = true;
                        WhosTurn("");
                        newt.Stop();
                }
            }
            });
        }

        private static void OnUpdate()
        {

            Application.Current.Dispatcher.Invoke(() => {

                int i = 0;
                WhosTurn("");

                if (MainWindow.TTG[i].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 1].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 2].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[i].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 4].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 8].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[i].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 3].Source.ToString().Contains("Cross") && MainWindow.TTG[i + 6].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[3].Source.ToString().Contains("Cross") && MainWindow.TTG[4].Source.ToString().Contains("Cross") && MainWindow.TTG[5].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[6].Source.ToString().Contains("Cross") && MainWindow.TTG[7].Source.ToString().Contains("Cross") && MainWindow.TTG[8].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[1].Source.ToString().Contains("Cross") && MainWindow.TTG[4].Source.ToString().Contains("Cross") && MainWindow.TTG[7].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[2].Source.ToString().Contains("Cross") && MainWindow.TTG[5].Source.ToString().Contains("Cross") && MainWindow.TTG[8].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[6].Source.ToString().Contains("Cross") && MainWindow.TTG[4].Source.ToString().Contains("Cross") && MainWindow.TTG[2].Source.ToString().Contains("Cross"))
                {
                    TicTacReset("Crosses");
                }
                if (MainWindow.TTG[i].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 1].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 2].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[i].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 4].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 8].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[i].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 3].Source.ToString().Contains("Circle") && MainWindow.TTG[i + 6].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[3].Source.ToString().Contains("Circle") && MainWindow.TTG[4].Source.ToString().Contains("Circle") && MainWindow.TTG[5].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[6].Source.ToString().Contains("Circle") && MainWindow.TTG[7].Source.ToString().Contains("Circle") && MainWindow.TTG[8].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[1].Source.ToString().Contains("Circle") && MainWindow.TTG[4].Source.ToString().Contains("Circle") && MainWindow.TTG[7].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[2].Source.ToString().Contains("Circle") && MainWindow.TTG[5].Source.ToString().Contains("Circle") && MainWindow.TTG[8].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (MainWindow.TTG[6].Source.ToString().Contains("Circle") && MainWindow.TTG[4].Source.ToString().Contains("Circle") && MainWindow.TTG[2].Source.ToString().Contains("Circle"))
                {
                    TicTacReset("Circles");
                }
                if (!MainWindow.TTG[0].Source.ToString().Contains("Blank") 
                  && !MainWindow.TTG[1].Source.ToString().Contains("Blank") 
                   && !MainWindow.TTG[2].Source.ToString().Contains("Blank") 
                    && !MainWindow.TTG[3].Source.ToString().Contains("Blank") 
                     && !MainWindow.TTG[4].Source.ToString().Contains("Blank") 
                      && !MainWindow.TTG[5].Source.ToString().Contains("Blank") 
                       && !MainWindow.TTG[6].Source.ToString().Contains("Blank") 
                        && !MainWindow.TTG[7].Source.ToString().Contains("Blank") 
                && !MainWindow.TTG[8].Source.ToString().Contains("Blank"))
                {
                    TicTacReset("");
                }

            });

        }

        private static void WhosTurn(string com)
        {
            

            
            Application.Current.Dispatcher.Invoke(() =>
            {


                if (com == "Cros")
                {
                    MainWindow.currturn.Content = "Crosses has won!";
                }
                if (com == "Circ")
                {
                    MainWindow.currturn.Content = "Circles has won!";
                }

                if (com == "" || com == null)
                {
                    MainWindow.mainWindow.Height = 458;
                    MainWindow.maingrid.Height = 448;
                    MainWindow.maingrid.Width = 790;
                    MainWindow.mainWindow.Width = 800;
                if (first == true && (turn - 1) % 2 == 1)
                {
                    MainWindow.currturn.Content = "It is your turn, choose wisely!";
                }
                if (first == false && (turn) % 2 == 1)
                {
                    MainWindow.currturn.Content = "It is your turn, choose wisely!";
                }
                if (first == true && (turn - 1) % 2 == 0)
                {
                    MainWindow.currturn.Content = "Wait for your opponent to make a move!";
                }
                if (first == false && (turn) % 2 == 0)
                {
                    MainWindow.currturn.Content = "Wait for your opponent to make a move!";
                }
             }
                if (com == "Quit")
                {
                   
                        MainWindow.currturn.Content = "";

                }
            });
        }
        public static void TicTacHandler(String Recv)
        {

            if (Recv == "Enabled")
            {

                if (first)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        MainWindow.startas.Visibility = Visibility.Visible;
                        MainWindow.TPas.Source = MainWindow.TCros.Source;
                        MainWindow.quitb.Visibility = Visibility.Visible;
                        MainWindow.quitb.IsEnabled = true;
                        WhosTurn("");


                    });

                }
                if (!first)
                {
                    Application.Current.Dispatcher.Invoke(() => {
                        MainWindow.startas.Visibility = Visibility.Visible;
                        MainWindow.TPas.Source = MainWindow.TCirc.Source;
                        MainWindow.quitb.Visibility = Visibility.Visible;
                        MainWindow.quitb.IsEnabled = true;
                        WhosTurn("");
                    });

                }
            }
            if (Recv.StartsWith("D") && canmov)
            {
                for (int i = 0; i < MainWindow.TTG.Count; i++)
                {

                    if (MainWindow.TTG[i].Name == Recv && first == true && (turn - 1) % 2 == 1 && MainWindow.TTG[i].Source.ToString().Contains("Blank"))
                    {
                        MainWindow.TTG[i].Source = new BitmapImage(new Uri("Cross.png", UriKind.Relative));
                        turn++;
                        MainWindow.SendLoop("!ttset:" + MainWindow.TTG[i].Name + ":" + turn);

                        OnUpdate();

                    }
                    if (MainWindow.TTG[i].Name == Recv && first == false && turn % 2 == 1 && MainWindow.TTG[i].Source.ToString().Contains("Blank"))
                    {
                        MainWindow.TTG[i].Source = new BitmapImage(new Uri("Circle.png", UriKind.Relative));
                        turn++;
                        MainWindow.SendLoop("!ttset:" + MainWindow.TTG[i].Name + ":" + turn);
                        OnUpdate();

                    }

                }
            }

        }

        public static void RecMov(string Recv)
        {

            if (Recv.StartsWith("TT:"))
            {

                string tempz = Recv.Replace("TT:", "");
                string[] tempsplit = tempz.Split(':', ' ');
                for (int i = 0; i < MainWindow.TTG.Count; i++)
                {
                    string imgz = "";
                    Image imgt = null;
                    Application.Current.Dispatcher.Invoke(() => {
                        imgz = MainWindow.TTG[i].Name;
                        imgt = MainWindow.TTG[i];
                    });
                    if (imgz.ToLower() == tempsplit[0] && first == false)
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            imgt.Source = new BitmapImage(new Uri("Cross.png", UriKind.Relative));
                        });

                        turn = Convert.ToInt32(tempsplit[3]);
                    }
                    if (imgz.ToLower() == tempsplit[0] && first == true)
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            imgt.Source = new BitmapImage(new Uri("Circle.png", UriKind.Relative));

                        });
                        turn = Convert.ToInt32(tempsplit[3]);
                    }

                }
                OnUpdate();

            }
        }

    }
}