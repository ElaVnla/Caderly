using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Caderly.Models;
using Caderly.Services;

namespace Caderly.Pages
{
    public class lovedones_bookingconfirmationModel : PageModel
    {
        private readonly IBookingService _svc;

        public lovedones_bookingconfirmationModel(IBookingService service)
        {
            _svc = service;
        }

        public void OnGet()
        {
            string[] arrMonth = {"January","February","March","April","May","June",
                        "July","August","September","October","November","December"};

            BookInfo bookInfo = new BookInfo();
            bookInfo.bookId = HttpContext.Session.GetString("SesBookId");
            bookInfo.booktype = Convert.ToInt16(HttpContext.Session.GetString("SesBookType"));
            bookInfo.booktitle = HttpContext.Session.GetString("SesBookTitle");
            bookInfo.bookvisitors = Convert.ToInt16(HttpContext.Session.GetString("SesBookVisitors"));
            bookInfo.bookyear = Convert.ToInt16(HttpContext.Session.GetString("SesBookYear"));
            bookInfo.bookmonth = Convert.ToInt16(HttpContext.Session.GetString("SesBookMonth"));
            bookInfo.bookday = Convert.ToInt16(HttpContext.Session.GetString("SesBookDay"));
            bookInfo.booktime = HttpContext.Session.GetString("SesBookTime");
            bookInfo.bookstatus = HttpContext.Session.GetString("SesBookStatus");
            bookInfo.bookduration = HttpContext.Session.GetString("SesBookDuration");
            bookInfo.bookdate = Convert.ToDateTime(HttpContext.Session.GetString("sesBookDate"));

            if (bookInfo.booktype == 1)
            {
                ViewData["BookType"] = "Face to Face";
            }
            else
            {
                ViewData["BookType"] = "Video Call";
            }
            ViewData["BookTitle"] = bookInfo.booktitle;
            ViewData["BookVisitors"] = bookInfo.bookvisitors;
            ViewData["BookYear"] = bookInfo.bookyear;
            ViewData["BookSMonth"] = arrMonth[bookInfo.bookmonth - 1];
            ViewData["BookDay"] = bookInfo.bookday;
            if ((Convert.ToInt16(bookInfo.booktime) > 1230))
            {
                ViewData["BookTime"] = bookInfo.booktime + "PM";
            }
            else
            {
                ViewData["BookTime"] = bookInfo.booktime + "AM";
            }
            ViewData["BookDuration"] = bookInfo.bookduration;
            ViewData["BookStatus"] = bookInfo.bookstatus;

            if (_svc.BookListDBAdd(bookInfo))
            {
                ViewData["BookMessage"] = "Yay! You're booked!";
                // Create session

            }
            else
            {
                ViewData["BookMessage"] = "Sorry Got Error!";

                //MyMessage = "Employee Id already exist!";
                //return Page();
            }
            //  will put the code in saving the record.

        }
    }
}
