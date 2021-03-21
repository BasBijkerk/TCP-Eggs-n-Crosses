using NetClient;
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
    public partial class NewGame : Window
    {
        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();


        private static bool onoff = true;


        public NewGame()
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
            


        }
        
        public static void ThisGameFunction(string RecvCall)
        {
            Console.WriteLine("Hello");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("?");
            MainWindow.SendLoop("!NGC:");
        }
    }
}
