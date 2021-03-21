using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NetTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();


        private static bool onoff = true;


        public MainWindow()
        {
            InitializeComponent();
            AllocConsole();

            

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dt.Tick += new EventHandler(OnUpdate);
            dt.Start();
            
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            Host.IsEnabled = onoff;
            try
            {

            
            ConO.Text = zSocket.conio;
            ConO.ScrollToEnd();
            string[] temphold = zSocket.conio.Split('\n');
                if (temphold.Length >= 10)
            {
                for (int i = 0; i <= 5; i++)
                {
                    ConO.Text = temphold[temphold.Length - i] + "\n";
                }
            }
            }
            catch(Exception)
            {
            }


        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!onoff)
            {
                
            }
            else
            {
                
            }
                
                
                onoff = !onoff;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            zSocket.ServerSetup();
            Host.IsEnabled = false;
        }
        public static void ButtonHost()
        {
            onoff = true;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            zSocket.ServerCmds(ConI.Text);
        }

    }
}
