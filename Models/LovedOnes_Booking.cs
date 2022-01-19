using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Caderly.Models
{
    public class LovedOnes_Booking
    {
        public int Booking_Id { get; set; }
        public int Booking_Type { get; set; }
        public int Book_Year { get; set; }
        public int Book_Month { get; set; }
        public int Book_Day { get; set; }
        public String Book_Time { get; set; }
        public String Book_Status { get; set; }
        public String Book_Title { get; set; }
        public String Book_Duration { get; set; }
        public int Book_Visitors { get; set; }

    }
}
