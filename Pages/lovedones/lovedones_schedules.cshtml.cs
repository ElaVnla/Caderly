using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Caderly.Models;
using Caderly.Services;

namespace Caderly.Pages.lovedones
{
    public class lovedones_schedulesModel : PageModel
    {
        [BindProperty]
        public List<BookInfo> BookInfoList { get; set; }


        private readonly IBookingService _svc;

        public lovedones_schedulesModel(IBookingService service)
        {
            _svc = service;
        }

        public void OnGet()
        {
            BookInfoList = getBookListDB();
            //ViewData["BookInfoList"] = BookInfoList;

            //  will put the code in saving the record.

        }
        public List<BookInfo> getBookListDB()
        {
            List<BookInfo> bookInfos = new List<BookInfo>();

            bookInfos = _svc.BookListDB();

            return bookInfos;
        }
        public void OnPost()
        {
            doCancel();

        }
        public void doCancel()
        {
            string bookId = Request.Form["txtBookId"].ToString();
            BookInfo bookInfo = _svc.BookListGetInfoDB(bookId);
            bookInfo.bookstatus = "Cancelled";

            if (_svc.BookListDBUpdate(bookInfo))
            {

                Response.Redirect("lovedones_schedules");
            }
            else
            {
                Response.Redirect("lovedones_schedules");
            }
        }

    }
}
