using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
//using Caderly.Pages.Models;
using Caderly.Models;


namespace Caderly.Pages
{
    public class lovedones_bookingModel : PageModel
    {
        public void OnGet()
        {
        }
        public int genBookingID()
        {
            Random rand = new Random();
            int number = rand.Next(1, 27);
            return number;
        }

        public string setTimeFormat(string strTime)
        {
            string sTime = strTime.Substring(0, 2) + strTime.Substring(3, 2);
            if (sTime == "0100") sTime = "1300";
            else if (sTime == "0130") sTime = "1330";
            else if (sTime == "0200") sTime = "1400";
            else if (sTime == "0230") sTime = "1430";
            else if (sTime == "0300") sTime = "1500";
            return sTime;
        }
        public void createBooking()
        {
            string sLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string sLetter2 = "ZYXWVUTSRQPOMNLKJIHGFEDCBA";
            string sLetter3 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string sID1 = Convert.ToString(sLetter[genBookingID() - 1]);
            string sID2 = Convert.ToString(sLetter2[genBookingID() - 1]);
            string sID3 = Convert.ToString(sLetter3[genBookingID() - 1]);
            int sID4 = genBookingID();

            BookInfo objBooking = new BookInfo();
            objBooking.bookId = sID1 + sID2 + sID3 + sID4;
            objBooking.booktype = 1;
            objBooking.booktitle = Request.Form["txtTitle"].ToString();
            objBooking.bookvisitors = Convert.ToInt16(Request.Form["txtNumber"].ToString());
            objBooking.bookyear = Convert.ToInt16(Request.Form["txtYear"].ToString());
            objBooking.bookmonth = Convert.ToInt16(Request.Form["txtMonth"].ToString());
            objBooking.bookday = Convert.ToInt16(Request.Form["txtDay"].ToString());
            objBooking.booktime = setTimeFormat(Request.Form["txtTiming"].ToString());
            objBooking.bookstatus = "OnGoing";
            objBooking.bookduration = Request.Form["txtDuration"].ToString();

            HttpContext.Session.SetString("SesBookId", objBooking.bookId);
            HttpContext.Session.SetString("SesBookType", objBooking.booktype.ToString());
            HttpContext.Session.SetString("SesBookTitle", objBooking.booktitle);
            HttpContext.Session.SetString("SesBookVisitors", objBooking.bookvisitors.ToString());
            HttpContext.Session.SetString("SesBookYear", objBooking.bookyear.ToString());
            HttpContext.Session.SetString("SesBookMonth", objBooking.bookmonth.ToString());
            HttpContext.Session.SetString("SesBookDay", objBooking.bookday.ToString());
            HttpContext.Session.SetString("SesBookTime", objBooking.booktime);
            HttpContext.Session.SetString("SesBookStatus", objBooking.bookstatus);
            HttpContext.Session.SetString("SesBookDuration", objBooking.bookduration);
            Response.Redirect("lovedones_bookingconfirmation");

        }
        public IActionResult OnPost()
        {
            createBooking();
            return Page();
        }
    }
}
