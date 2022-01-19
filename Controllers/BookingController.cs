using Microsoft.AspNetCore.Mvc;
using Caderly.Models;
using Caderly.Services;
using System.Collections.Generic;

namespace Caderly.Controllers
{
    [Produces("application/json")]
    [Route("api/Booking")]
    public class BookingController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        // GET: api/Car
        [HttpGet]
        public string Get()
        {
            //return _bookingService.JSONBookList();
            return _bookingService.JSONBookListDB();
        }

    }
}
