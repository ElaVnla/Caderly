using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Caderly.Models
{
    public class BookInfo
    {
        [Required, Key]
        public string bookId { get; set; }
        public int booktype { get; set; }
        public string booktitle { get; set; }
        public int bookvisitors { get; set; }
        public int bookyear { get; set; }
        public int bookmonth { get; set; }
        public int bookday { get; set; }
        public string booktime { get; set; }
        public string bookstatus { get; set; }
        public string bookduration { get; set; }
    }
}
