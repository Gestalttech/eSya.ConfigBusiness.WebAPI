using eSya.ConfigBusiness.DL.Repository;
using eSya.ConfigBusiness.DO;
using eSya.ConfigBusiness.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigBusiness.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessCalendarController : ControllerBase
    {
        private readonly IBusinessCalendarRepository _businessCalendarRepository;
        public BusinessCalendarController(IBusinessCalendarRepository businessCalendarRepository)
        {
            _businessCalendarRepository = businessCalendarRepository;
        }
        #region Business Calendar
        [HttpGet]
        public async Task<IActionResult> GetBusinessCalendarBYBusinessKey(int businessKey)
        {
            var ds = await _businessCalendarRepository.GetBusinessCalendarBYBusinessKey(businessKey);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj)
        {
            var msg = await _businessCalendarRepository.InsertOrUpdateBusinessCalendar(obj);
            return Ok(msg);

        }
        #endregion
    }
}
