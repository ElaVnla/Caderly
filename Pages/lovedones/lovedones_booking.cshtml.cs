using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
//using Caderly.Pages.Models;
using Caderly.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;


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
            if (sTime == "1000") sTime = "1000";
            else if (sTime == "1030") sTime = "1030";
            else if (sTime == "1100") sTime = "1100";
            else if (sTime == "1130") sTime = "1130";
            else if (sTime == "1200") sTime = "1230";
            else if (sTime == "0100") sTime = "1300";
            else if (sTime == "0130") sTime = "1330";
            else if (sTime == "0200") sTime = "1400";
            else if (sTime == "0230") sTime = "1430";
            else if (sTime == "0300") sTime = "1500";
            return sTime;
        }
        public string setTimeFormat2(string strTime)
        {
            string sTime = strTime.Substring(0, 2) + strTime.Substring(3, 2);
            if (sTime == "1000") sTime = "10:00";
            else if (sTime == "1030") sTime = "10:30";
            else if (sTime == "1100") sTime = "11:00";
            else if (sTime == "1130") sTime = "11:30";
            else if (sTime == "1200") sTime = "12:30";
            else if (sTime == "0100") sTime = "13:00";
            else if (sTime == "0130") sTime = "13:30";
            else if (sTime == "0200") sTime = "14:00";
            else if (sTime == "0230") sTime = "14:30";
            else if (sTime == "0300") sTime = "15:00";
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
            var booktime2 = setTimeFormat2(Request.Form["txtTiming"].ToString());
            objBooking.bookstatus = "OnGoing";
            objBooking.bookduration = Request.Form["txtDuration"].ToString();
            var bookdate = objBooking.bookyear + "-" + objBooking.bookmonth + "-" + objBooking.bookday + " " + booktime2;
            objBooking.bookdate = Convert.ToDateTime(bookdate);

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
            HttpContext.Session.SetString("sesBookDate", objBooking.bookdate.ToString());
            doSendEmailHTML(objBooking);
            Response.Redirect("lovedones_bookingconfirmation");

        }
        private void doSendEmailHTML(BookInfo bookInfo)
        {
            string[] strMonth = {"January","February","March","April","May","June",
                                                                         "July","August","September","October","November","December"};
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("caderlytest@gmail.com"));
            email.To.Add(MailboxAddress.Parse("erika.sampang.c@gmail.com"));
            email.Subject = "Booking has been confirmed";
            if (bookInfo.booktype == 1)
            {
                email.Body = new TextPart(TextFormat.Html) { Text = "Your booking has been successful. Your following booking:<br/> " + @"
                    <label><strong>Day and time:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookday + "&nbsp;" + bookInfo.booktime + @" &nbsp; visitors</h5></br>
                        <label><strong>Month of booking:&nbsp;</strong></label>
                                <h5>" + strMonth[bookInfo.bookmonth - 1] + @" &nbsp; visitors</h5><br/>
                        <label><strong>Year of booking:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookyear + @" &nbsp; visitors</h5><br/>
                        <label><strong>Title of booking:&nbsp;</strong></label>
                            <h5><strong></strong>" + bookInfo.booktitle + @"</h5><br/>
                        <label><strong>No.of visitors:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookvisitors + @" &nbsp; visitors</h5><br/>
                        <div class='col row'>
                                <label><strong>Duration of stay:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookduration + @"</h5><br/>
                            </div>
                        <label><strong>location:&nbsp;</strong></label>
                            <h5>49 Upper Thomson Rd, Singapore 574325</h5>" };
            }
            else
            {
                email.Body = new TextPart(TextFormat.Html) { Text = "Your booking has been successful. Your following booking:<br/> " + @"
                    <label><strong>Day and time:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookday + "&nbsp;" + bookInfo.booktime + @" &nbsp; visitors</h5></br>
                        <label><strong>Month of booking:&nbsp;</strong></label>
                                <h5>" + strMonth[bookInfo.bookmonth - 1] + @" &nbsp; visitors</h5><br/>
                        <label><strong>Year of booking:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookyear + @" &nbsp; visitors</h5><br/>
                        <label><strong>Title of booking:&nbsp;</strong></label>
                            <h5><strong></strong>" + bookInfo.booktitle + @"</h5><br/>
                        <label><strong>No.of visitors:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookvisitors + @" &nbsp; visitors</h5><br/>
                        <div class='col row'>
                                <label><strong>Duration of stay:&nbsp;</strong></label>
                                <h5>" + bookInfo.bookduration + @"</h5><br/>
                            </div>
                        <label><strong>Platform:&nbsp;</strong></label>
                            <h5>Via zoom</h5>" };
            }


            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp-relay.sendinblue.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("caderlytest@gmail.com", "x56msVOYnUSv0fzd");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public IActionResult OnPost()
        {
            createBooking();
            return Page();
        }
    }
}
