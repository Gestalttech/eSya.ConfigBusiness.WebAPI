using eSya.ConfigBusiness.DO;
using eSya.ConfigBusiness.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigBusiness.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalendarControlController : ControllerBase
    {
        private readonly ICalendarControlRepository _calendarControlRepository;
        public CalendarControlController(ICalendarControlRepository calendarControlRepository)
        {
            _calendarControlRepository = calendarControlRepository;
        }
        #region Business Calendar
        [HttpGet]
        public async Task<IActionResult> GetCalendarHeaders(int businesskey)
        {
            var ds = await _calendarControlRepository.GetCalendarHeaders(businesskey);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj)
        {
            var msg = await _calendarControlRepository.InsertBusinessKeyIntoCalendar(obj);
            return Ok(msg);

        }
        #endregion
    }
}
