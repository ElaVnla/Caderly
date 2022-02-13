using Microsoft.Extensions.Caching.Memory;
using Caderly.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Caderly.Services
{
    public interface IBookingService
    {
        string JSONBookList();
        string JSONBookListDB();
        bool BookListDBAdd(BookInfo bookInfo);
        List<BookInfo> BookListDB();
        BookInfo BookListGetInfoDB(string bookId);
        bool BookListDBUpdate(BookInfo bookInfo);
        void doSetMissed();
    }
    public class BookingService : IBookingService
    {
        private CaderlyDbContext _context;
        public BookingService(CaderlyDbContext context)
        {
            _context = context;
        }

        public string JSONBookList()
        {
            List<BookInfo> bookInfos = new List<BookInfo>();

            BookInfo bookInfo = new BookInfo();
            bookInfo.booktype = 1;
            bookInfo.booktitle = "Testing Event1";
            bookInfo.bookvisitors = 10;
            bookInfo.bookyear = 2022;
            bookInfo.bookmonth = 1;
            bookInfo.bookday = 20;
            bookInfo.booktime = "1000";
            bookInfo.bookstatus = "OnGoing";
            bookInfo.bookduration = "15mins";
            bookInfos.Add(bookInfo);

            bookInfo = new BookInfo();
            bookInfo.booktype = 1;
            bookInfo.booktitle = "Testing Event2";
            bookInfo.bookvisitors = 10;
            bookInfo.bookyear = 2022;
            bookInfo.bookmonth = 1;
            bookInfo.bookday = 25;
            bookInfo.booktime = "1300";
            bookInfo.bookstatus = "OnGoing";
            bookInfo.bookduration = "15mins";
            bookInfos.Add(bookInfo);

            bookInfo = new BookInfo();
            bookInfo.booktype = 1;
            bookInfo.booktitle = "Vaccine Day";
            bookInfo.bookvisitors = 2;
            bookInfo.bookyear = 2022;
            bookInfo.bookmonth = 2;
            bookInfo.bookday = 25;
            bookInfo.booktime = "1230";
            bookInfo.bookstatus = "OnGoing";
            bookInfo.bookduration = "1hr";
            bookInfos.Add(bookInfo);


            bookInfo = new BookInfo();
            bookInfo.booktype = 2;
            bookInfo.booktitle = "Lola Videocall";
            bookInfo.bookvisitors = 10;
            bookInfo.bookyear = 2022;
            bookInfo.bookmonth = 1;
            bookInfo.bookday = 25;
            bookInfo.booktime = "1300";
            bookInfo.bookstatus = "OnGoing";
            bookInfo.bookduration = "2hrs";
            bookInfos.Add(bookInfo);



            var options = new JsonSerializerOptions { WriteIndented = true };
            string JSONData = JsonSerializer.Serialize(bookInfos, options);
            return JSONData;
        }

        public string JSONBookListDB()
        {
            List<BookInfo> bookInfos = new List<BookInfo>();

            bookInfos = GetAllBookInfo();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string JSONData = JsonSerializer.Serialize(bookInfos, options);
            return JSONData;
        }
        public List<BookInfo> GetAllBookInfo()
        {
            List<BookInfo> AllBookInfo = new List<BookInfo>();
            AllBookInfo = _context.BookInfo.ToList();
            AllBookInfo.Sort((d1, d2) => System.DateTime.Compare(d1.bookdate, d2.bookdate));
            //var enum1 = AllBookInfo.OrderBy(d => d.bookdate);
            //AllBookInfo.Clear();
            //foreach (var bookinfo in enum1)
            //{
            //    AllBookInfo.Add(bookinfo);
            //}
            //objLis.Sort((d1, d2) => System.DateTime.Compare(d1.bookdate, d2.bookdate));
            return AllBookInfo;
        }

        public bool BookListDBAdd(BookInfo bookInfo)
        {
            AddBookInfo(bookInfo);
            return true;
        }
        public List<BookInfo> BookListDB()
        {
            List<BookInfo> bookInfos = new List<BookInfo>();
            bookInfos = GetAllBookInfo();
            return bookInfos;
        }
        public bool AddBookInfo(BookInfo newbookinfo)
        {
            // if (EmployeeExists(newemployee.Id))
            // {
            //     return false;
            // }
            _context.Add(newbookinfo);
            _context.SaveChanges();
            return true;
        }
        private bool BookInfoExists(string bookId)
        {
            return _context.BookInfo.Any(e => e.bookId == bookId);
        }
        public BookInfo BookListGetInfoDB(string bookId)
        {
            return GetBookInfoById(bookId);
        }
        public BookInfo GetBookInfoById(string bookId)
        {
            BookInfo bookInfo = _context.BookInfo.Where(e => e.bookId == bookId).FirstOrDefault();
            return bookInfo;
        }
        public bool BookListDBUpdate(BookInfo bookInfo)
        {
            return UpdateBookInfo(bookInfo);

        }
        public void doSetMissed()
        {
            List<BookInfo> lstBookInfo = new List<BookInfo>();
            lstBookInfo = BookListDB();
            foreach (var bookInfo in lstBookInfo)
            {
                if (bookInfo.bookdate < System.DateTime.Now && bookInfo.bookstatus == "OnGoing")
                {
                    bookInfo.bookstatus = "Missed";
                    bool retval = BookListDBUpdate(bookInfo);
                    doSendEmailHTML(bookInfo);

                }
            }

        }
        private void doSendEmailHTML(BookInfo bookInfo)
        {
            string[] strMonth = {"January","February","March","April","May","June",
                                                                         "July","August","September","October","November","December"};
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("caderlytest@gmail.com"));
            email.To.Add(MailboxAddress.Parse("erika.sampang.c@gmail.com"));
            email.Subject = "You've missed your booking!";
            if (bookInfo.booktype == 1)
            {
                email.Body = new TextPart(TextFormat.Html) { Text = "You have unfortunetly missed the following booking:<br/> " + @"
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
                            <h5>49 Upper Thomson Rd, Singapore 574325</h5>
                                        
   " };
            }
            else
            {
                email.Body = new TextPart(TextFormat.Html) { Text = "You have unfortunetly missed the following booking:<br/> " + @"
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
                            <h5>Via zoom</h5>                    
   " };
            }


            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp-relay.sendinblue.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("caderlytest@gmail.com", "x56msVOYnUSv0fzd");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public bool UpdateBookInfo(BookInfo bookInfo)
        {
            bool updated = true;
            _context.Attach(bookInfo).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
                updated = true;

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookInfoExists(bookInfo.bookId))
                {
                    updated = false;
                }
                else
                {
                    throw;
                }
            }
            return updated;


        }

    }
}
