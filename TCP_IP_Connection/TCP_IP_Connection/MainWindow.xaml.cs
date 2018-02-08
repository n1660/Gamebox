using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace TCP_IP_Connection
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Server tcpsv = new Server();
            new Client().StartClient();
        }

        private void Btn_send_Click(object sender, RoutedEventArgs e)
        {
            byte[] toSend = new byte[1024];
            for (int i = 0; Txtbl_input.Text[i-1] != '\0'; i++)
            {
                toSend[i] = (byte)Txtbl_input.Text[i];
            }
        }
    }
}
