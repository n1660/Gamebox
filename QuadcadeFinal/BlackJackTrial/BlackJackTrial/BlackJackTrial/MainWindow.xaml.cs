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
using System.Web;
namespace BlackJackTrial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Random random = new Random();
        int counter = 0; //counting how many times i hit the button 
        int points_player = 0;
        int points_computer = 0;

        private void button_Click(object sender, RoutedEventArgs e) //hit button to pull cards
        {
            counter++;
            if (counter == 1) //if i pushed hit once allready, first 2 cards should come randomly
            {
                int c1; //my first card
                int c2;// second card
                int sum; //worth sum

                c1 = random.Next(1, 11); // 1-11 ;becase K,J,Q have all the worth 10
                c2 = random.Next(1, 11);
                sum = c1 + c2;

                label1.Content = c1.ToString();
                label2.Content = c2.ToString();
                label_sum_content.Content = sum.ToString();
            }

            if(counter == 2) // second time should just the 3rd one change
            {
                int c3;
                int sum; // had to convert the variables it bc it didnt work //not getting the other cards in the other if//no idea why.

                c3 = random.Next(1, 11);
                label3.Content = c3.ToString();

                sum = Convert.ToInt32(label1.Content) + Convert.ToInt32(label2.Content)+ c3; //converted them to integers bc they had to be algorithms
                label_sum_content.Content = sum.ToString();
                //Converts the specified string representation of a number to an equivalent 32-bit signed integer.Thx google
            }

            if(counter == 3) // i shouldnt be able to pull more than 4 cards so it stops here.
            {
                int c4;
                int sum;

                c4 = random.Next(1, 11);
                label4.Content = c4.ToString();

                sum = Convert.ToInt32(label1.Content) + Convert.ToInt32(label2.Content) + Convert.ToInt32(label3.Content) + c4;
                label_sum_content.Content = sum.ToString();
            }

        }

        private void button_comp_Click(object sender, RoutedEventArgs e)
        {
            int c1_comp;
            int c2_comp;
            int sum_comp;

            c1_comp = random.Next(1, 11);
            c2_comp = random.Next(1, 11);
            sum_comp = c1_comp + c2_comp;

            label5.Content = c1_comp.ToString();
            label6.Content = c2_comp.ToString();
            label_sum_computer_content.Content = sum_comp.ToString();

            if(sum_comp <=17)
            {
                int c3_comp;
                c3_comp = random.Next(1, 11);
                label7.Content = c3_comp.ToString();

                sum_comp = sum_comp + c3_comp;
                label_sum_computer_content.Content = label_sum_computer_content.ToString();
            }
            
            if(sum_comp <=17) //if its still smaller than 17 after pulling a card.
            {
                int c4_comp;
                c4_comp = random.Next(1, 11);
                label8.Content = c4_comp.ToString();

                sum_comp = sum_comp + c4_comp;
                label_sum_computer_content.Content = label_sum_computer_content.Content.ToString();

            }

            if (sum_comp < 17) //if its still smaller than 17 after pulling a card.
            {
                int c5_comp;
                c5_comp = random.Next(1, 11);
                label8.Content = c5_comp.ToString();

                sum_comp = sum_comp + c5_comp;
                label_sum_computer_content.Content = label_sum_computer_content.Content.ToString();

            }

        }

        private void buttonResult_Click(object sender, RoutedEventArgs e)
        {
            //int c1, c2, c3, c4, playerSum;
            //c1 = Convert.ToInt32(label1.Content);
            //c2 = Convert.ToInt32(label2.Content);
            //c3 = Convert.ToInt32(label3.Content);
            //c4 = Convert.ToInt32(label4.Content);
            //playerSum = c1 + c2 + c3 + c4;

            //shorter version

            buttonResult.IsEnabled = false;
            buttonNext.IsEnabled = true;

            int playerSum, computerSum;

            playerSum = Convert.ToInt32(label_sum_content.Content);
            computerSum = Convert.ToInt32(label_sum_computer_content.Content);

  

            if(playerSum > computerSum && playerSum <=21)

            {

                  points_player = points_player + 5;
                  scorePlayer_content.Content = points_player.ToString();
              }

            if( computerSum > playerSum && computerSum <=21)

            {
                points_computer = points_computer + 5;
                scoreComputer_content.Content = points_computer.ToString();
            }

            if(computerSum > 21 && playerSum > 21)
            {
                MessageBox.Show(" No winner in this game !");
            }

            if (computerSum == playerSum && playerSum <=21 && computerSum <=21)
            {
                points_player = points_player + 5;
                scorePlayer_content.Content = points_player.ToString();
                points_computer = points_computer + 5;
                scoreComputer_content.Content = points_computer.ToString();
            }

            if(playerSum == 50)
            {
                MessageBox.Show("Congrats you Won !");
            }

            if (computerSum == 50)
            {
                MessageBox.Show("Sorry ! You Lost !");
            }

            if(computerSum <=21 && playerSum >21)
            {
                points_computer = points_computer + 5;
                scoreComputer_content.Content = points_computer.ToString();
            }

            if (playerSum <= 21 && computerSum > 21)
            {
                points_computer = points_computer + 5;
                scoreComputer_content.Content = points_computer.ToString();
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {

            buttonResult.IsEnabled = true;
            buttonNext.IsEnabled = false;

            counter = 0;
            label1.Content = 0;
            label2.Content = 0;
            label3.Content = 0;
            label4.Content = 0;
            label5.Content = 0;
            label6.Content = 0;
            label7.Content = 0;
            label8.Content = 0;

            label_sum_computer_content.Content= 0;
            label_sum_content.Content = 0;
            
        }
    }
}
