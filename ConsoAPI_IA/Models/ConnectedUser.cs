using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoAPI_IA.Models
{
    internal class ConnectedUser
    {
        public int MemberId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Pseudo { get; set; }
    }
}
