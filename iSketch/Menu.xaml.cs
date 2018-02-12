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
    /// Interaktionslogik für Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
               InitializeComponent();
        }

        private void Button_Click_Menu(object sender, RoutedEventArgs e)
        {
            if (sender == this.iSketch)
            {
                
                MainWindow.win.Content = new Artist();
            }
            else if(sender == this.Snake)
            {
                Console.Write("Start Snake ~");
            }
            else if(sender == this.Hangman)
            {
                Console.Write("Start Hangman X");
            }
        }
    }
}
