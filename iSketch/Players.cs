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
        public IPAddress ID { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public int Moves { get; set; }
        public bool Guessed_Correctly { get; set; }
    }



}
