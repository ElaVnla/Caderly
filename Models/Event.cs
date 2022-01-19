using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caderly.Models
{
    public class Event
    {
        public int booktype { get; set; }
        public string occasion { get; set; }
        public int invited_count { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public string stime { get; set; }
        public string bookstatus { get; set; }
    }
}
