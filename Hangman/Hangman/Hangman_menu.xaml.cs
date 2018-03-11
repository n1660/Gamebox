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

namespace Hangman
{
    public partial class Hangman_menu : Page
    {
        public Hangman_menu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string content = btn.Content.ToString();
            switch (content)
            {
                case "New Game":
                    Content = new GamepageHangman();
                    break;
                case "Settings":
                    Content = new Settings();
                    break;
                case "Quit":
                    System.Windows.Application.Current.Shutdown();
                    break;
                default:
                    break;
            }
        }
    }
}
