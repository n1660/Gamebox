using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace iSketch.Connection
{
    class PacketUtil
    {
        public static void HandlePacket(Artist owner, String packet, StreamWriter writer)
        {
            String[] arr = packet.Split(';');
            if (arr.Length == 0) return;

            if (arr[0] == "SCORE")
            {
                String[] score = new String[2];

                foreach (String str in arr)
                {
                    if (str == arr[0] || str == "")
                        continue;

                    Console.WriteLine("HANDLING SCORE PACKET: '" + str + "'");
                    score = str.Split('=');
                    String username = score[0];
                    int scoreValue = Int32.Parse(score[1]);

                    bool scoreUpdated = false;

                    foreach (Member member in iSketch.Menu.MemberList[iSketch.Menu.Host])
                    {
                        if (member.Username == username)
                        {
                            Console.WriteLine("Updated score for " + member.Username + " -> " + scoreValue);
                            member.Score = scoreValue;
                            scoreUpdated = true;
                            break;
                        }
                    }
                    if (!scoreUpdated)
                    {
                        Console.WriteLine("Adding new member: " + username + " -> " + scoreValue);
                        // The user is not in the memberlist yet, so we add them to the list
                        Member member = new Member(username, true)
                        {
                            Score = scoreValue
                        }; // Set host to true so it won't connect (shitty)
                        iSketch.Menu.MemberList[iSketch.Menu.Host].Add(member);
                    }
                }
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    owner.ShowScores();
                }));
            }
            else if (arr[0] == "CLEAR")
            {
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    owner.MyCanvas.Children.Clear();
                }));
            }
            else if (arr[0] == "CORRECT")
            {
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    owner.ShowCorrectWord();
                }));
            }
            else if (arr[0] == "INCORRECT")
            {
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    owner.ShowCorrectWord();
                }));
            }
            else if (arr[1] == "LINE")
            {
                int senderId = Int32.Parse(arr[0]);
                Console.WriteLine("RECEIVED LINE " + senderId + " -> " + arr[2]);
                if (senderId == Menu.member.ID) return; // The sender already drew the line on his canvas
                String[] lineCoords = arr[2].Split('_');
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Point start = new Point
                    {
                        X = double.Parse(lineCoords[0]),
                        Y = double.Parse(lineCoords[1])
                    };
                    Point end = new Point
                    {
                        X = double.Parse(lineCoords[2]),
                        Y = double.Parse(lineCoords[3])
                    };

                    Line newLine = new Line
                    {
                        StrokeStartLineCap = PenLineCap.Round,
                        StrokeEndLineCap = PenLineCap.Round,
                        X1 = start.X,
                        Y1 = start.Y,
                        X2 = end.X,
                        Y2 = end.Y,
                        Stroke = (SolidColorBrush)(new BrushConverter()).ConvertFromString(lineCoords[4]),
                        StrokeThickness = double.Parse(lineCoords[5]),
                    };
               
                    owner.MyCanvas.Children.Add(newLine);
                }));               
            } else if (arr[1] == "START")
            {
                long startTime = long.Parse(arr[2]);
                owner.Dispatcher.BeginInvoke(new Action(() =>
                {
                    owner.startCountdownTime = startTime;
                    owner.Countdown.Text = owner.counter.ToString();
                    owner.countdown2.Start();
                }));
            }          
        }
    }
}
