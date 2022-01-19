using Microsoft.Extensions.Caching.Memory;
using Caderly.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
