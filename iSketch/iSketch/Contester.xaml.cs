using System;
using System.Collections.Generic;
using System.Linq;
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

namespace iSketch
{
    /// <summary>
    /// Interaktionslogik für Contester.xaml
    /// </summary>
    public partial class Contester : Page
    {
        public Contester()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (sender == this.BTN_Clear)
                this.MyCanvas.Children.Clear();

            else if (sender == this.BTN_Submit)
            {
                if (this.Your_Word.Text == this.Chat_Window.Text)
                {
                    Stop_All();
                    Console.Write("yay\n");
                    this.Chat_Window.Clear();
                    this.MyCanvas.Children.Clear();
                    this.Your_Word.Text = Get_Random_Word();
                    Start_All();
                }
                else
                {
                    Console.Write("No Way\n");
                    this.Chat_Window.Clear();
                }
            }
            else if (sender == this.BTN_HOME)
            {
                MainWindow.win.Content = new Menu();
            }
        }
    }
}
