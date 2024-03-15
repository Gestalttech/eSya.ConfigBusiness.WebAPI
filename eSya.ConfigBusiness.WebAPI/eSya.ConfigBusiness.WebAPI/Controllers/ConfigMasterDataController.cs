using eSya.ConfigBusiness.DL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigBusiness.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigMasterDataController : ControllerBase
    {
        #region Common Methods
       
        /// <summary>
        /// Get Business key.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await new CommonMethod().GetBusinessKey();
            return Ok(ds);
        }

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetISDCodes()
        {
            var ds = await new CommonMethod().GetISDCodes();
            return Ok(ds);
        }

      

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIndiaISDCodes()
        {
            var ds = await new CommonMethod().GetIndiaISDCodes();
            return Ok(ds);
        }

        /// <summary>
        /// Get Active Currencies for dropdown.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCurrencyCodes()
        {
            var ds = await new CommonMethod().GetActiveCurrencyCodes();
            return Ok(ds);
        }
        #endregion
    }
}
