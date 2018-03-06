using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace iSketch
{
    public class Member //: IEquatable<Member>
    {
        public int ID { get; set; } // Für Sockets als int -> ID wird vom Socket zugewiesen!
        public string Username { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public bool Guessed_Correctly { get; set; }
    }
    // Bei Add -> Daten müssen auch an den andern geschickt werden
}
