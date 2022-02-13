using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Caderly.Models;
using Caderly.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Caderly.Pages.lovedones
{
    public class lovedones_editbookingModel : PageModel
    {
        [BindProperty]

        public BookInfo bookInfo { get; set; }

        private readonly IBookingService _svc;

        string bookId;

        public lovedones_editbookingModel(IBookingService service)
        {
            _svc = service;
        }
        public void OnGet()
        {

            bookId = HttpContext.Request.Query["bookId"];
            bookInfo = _svc.BookListGetInfoDB(bookId);
        }
        public void OnPost()
        {
            bookId = HttpContext.Request.Query["bookId"];
            bookInfo = _svc.BookListGetInfoDB(bookId);
            doUpdateRecord();
        }
        public void doUpdateRecord()
        {
            var bkid = Request.Form["txtBookId"].ToString();
            BookInfo bookInfox = _svc.BookListGetInfoDB(bkid);
            bookInfox.bookvisitors = Convert.ToInt16(Request.Form["txtNumber"].ToString());
            bookInfox.booktitle = Request.Form["txtTitle"].ToString();
            bookInfox.bookduration = Request.Form["txtDuration"].ToString();


            if (_svc.BookListDBUpdate(bookInfox))
            {
                doSendEmailHTML(bookInfox);
                Response.Redirect("lovedones_schedules");
            }
            else
            {
                Response.Redirect("lovedones_schedules");
            }
        }
        private void doSendEmailHTML(BookInfo bookInfo)
        {
            string[] strMonth = {"January","February","March","April","May","June",
                                                                         "July","August","September","October","November","December"};
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("caderlytest@gmail.com"));
            email.To.Add(MailboxAddress.Parse("erika.sampang.c@gmail.com"));
            email.Subject = "Booking has been Updated.";
            if (bookInfo.booktype == 1)
            {
                email.Body = new TextPart(TextFormat.Html) { Text = "Your booking has been Updated:<br/> " + @"
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
                email.Body = new TextPart(TextFormat.Html) { Text = "Your booking has been Updated:<br/> " + @"
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
    }
}
