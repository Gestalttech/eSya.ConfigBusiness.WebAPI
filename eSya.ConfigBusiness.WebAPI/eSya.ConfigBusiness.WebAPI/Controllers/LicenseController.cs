using eSya.ConfigBusiness.DO;
using eSya.ConfigBusiness.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigBusiness.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LicenseController : ControllerBase
    {

        private readonly ILicenseRepository _licenseRepository;
        public LicenseController(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }
        #region Business Entity
        /// <summary>
        /// Getting  Business Entity List.
        /// UI Reffered - Business Entity Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessEntities()
        {
            var b_entities = await _licenseRepository.GetBusinessEntities();
            return Ok(b_entities);

        }

        /// <summary>
        /// Getting  Business Entity Info.
        /// UI Reffered - Business Entity Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessEntityInfo(int BusinessId)
        {
            var b_entities = await _licenseRepository.GetBusinessEntityInfo(BusinessId);
            return Ok(b_entities);

        }

        /// <summary>
        /// Insert Or Update Business Entity .
        /// UI Reffered -Business Entity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessEntity(DO_BusinessEntity obj)
        {
            var msg = await _licenseRepository.InsertBusinessEntity(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Insert Business Entity .
        /// UI Reffered -Business Entity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessEntity(DO_BusinessEntity obj)
        {
            var msg = await _licenseRepository.UpdateBusinessEntity(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Delete Business Entity
        /// UI Reffered - Business Entity
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteBusinessEntity(int BusinessEntityId)
        {
            var msg = await _licenseRepository.DeleteBusinessEntity(BusinessEntityId);
            return Ok(msg);
        }

        /// <summary>
        /// Getting  Business Entity List for dropdown.
        /// UI Reffered - Business Segment & Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveBusinessEntities()
        {
            var b_entities = await _licenseRepository.GetActiveBusinessEntities();
            return Ok(b_entities);

        }


        /// <summary>
        /// Getting  Business Entity Preferred Language.
        /// UI Reffered - Business Entity Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPreferredLanguagebyBusinessKey(int BusinessId)
        {
            var lang = await _licenseRepository.GetPreferredLanguagebyBusinessKey(BusinessId);
            return Ok(lang);

        }

        #endregion  Business Entity

        #region  Business Location
        /// <summary>
        /// Get BusinessLocationByBusinessId .
        /// UI Reffered - Business Location By Business Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationByBusinessId(int BusinessId)
        {
            var locs = await _licenseRepository.GetBusinessLocationByBusinessId(BusinessId);
            return Ok(locs);
        }
        /// <summary>
        /// Get Date Format by ISD Code
        /// UI Reffered - Business Location By Business Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDateFormatbyISDCode(int isdCode)
        {
            var locs = await _licenseRepository.GetDateFormatbyISDCode(isdCode);
            return Ok(locs);
        }
      
        /// <summary>
        /// Insert Business Location .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessLocation(DO_BusinessLocation location)
        {
            var msg = await _licenseRepository.InsertBusinessLocation(location);
            return Ok(msg);

        }
        /// <summary>
        /// Insert  Business Location .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessLocation(DO_BusinessLocation location)
        {
            var msg = await _licenseRepository.UpdateBusinessLocation(location);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De BusinessLocation.
        /// UI Reffered - BusinessLocation
        /// </summary>
        /// <param name="status-code_type"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveBusinessLocation(bool status, int BusinessId, int locationId)
        {
            var res = await _licenseRepository.ActiveOrDeActiveBusinessLocation(status, BusinessId, locationId);
            return Ok(res);
        }

        /// <summary>
        /// Get Currency by BusinessKey.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencybyBusinessKey(int BusinessKey)
        {
            var state = await _licenseRepository.GetCurrencybyBusinessKey(BusinessKey);
            return Ok(state);
        }

        /// <summary>
        /// Get Business Unit Type by EntityId .
        /// UI Reffered - Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitType(int businessId)
        {
            var taxidentifications = await _licenseRepository.GetBusinessUnitType(businessId);
            return Ok(taxidentifications);
        }
        /// <summary>
        /// Get Parameters by BusinessKey.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocationParametersbyBusinessKey(int BusinessKey)
        {
            var param = await _licenseRepository.GetLocationParametersbyBusinessKey(BusinessKey);
            return Ok(param);
        }
        /// <summary>
        /// Get Location Preferred Languages by BusinessKey.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocationPreferredLanguagebyBusinessKey(int BusinessID, int BusinessKey)
        {
            var plang = await _licenseRepository.GetLocationPreferredLanguagebyBusinessKey(BusinessID, BusinessKey);
            return Ok(plang);
        }
        #endregion

        #region Location Financial Info
        /// <summary>
        /// Insert Location Financial Info .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocationFinancialInfo(DO_LocationFinancialInfo obj)
        {
            var msg = await _licenseRepository.InsertOrUpdateLocationFinancialInfo(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Get Location Financial Info by BusinessKey.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocationFinancialInfo(int BusinessKey)
        {
            var fininfo = await _licenseRepository.GetLocationFinancialInfo(BusinessKey);
            return Ok(fininfo);
        }
        /// <summary>
        /// Get Existing Location as Segment if IsBookofAccount is checked .
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveLocationsAsSegments()
        {
            var segments = await _licenseRepository.GetActiveLocationsAsSegments();
            return Ok(segments);
        }
        #endregion

        #region Location Tax Info
        /// <summary>
        /// Insert Location Tax Info.
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocationTaxInfo(DO_LocationTaxInfo obj)
        {
            var msg = await _licenseRepository.InsertOrUpdateLocationTaxInfo(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Get Location Tax Info.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocationLocationTaxInfo(int BusinessKey)
        {
            var taxinfo = await _licenseRepository.GetLocationLocationTaxInfo(BusinessKey);
            return Ok(taxinfo);
        }
        /// <summary>
        /// Get Tax Idendification by ISD Code .
        /// UI Reffered - Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxIdentificationByISDCode(int IsdCode)
        {
            var taxidentifications = await _licenseRepository.GetTaxIdentificationByISDCode(IsdCode);
            return Ok(taxidentifications);
        }

        /// <summary>
        /// Get Currency List by ISD Code .
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCurrencyListbyIsdCode(int IsdCode)
        {
            var currencies = await _licenseRepository.GetCurrencyListbyIsdCode(IsdCode);
            return Ok(currencies);
        }

        /// <summary>
        /// Get Cities List by ISD Code .
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCityListbyISDCode(int isdCode)
        {
            var cities = await _licenseRepository.GetCityListbyISDCode(isdCode);
            return Ok(cities);
        }
        /// <summary>
        /// Get State Code By ISD Code and Tax Identification.
        /// UI Reffered -Business Location 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetStateCodeByISDCode(int isdCode, int TaxIdentificationId)
        {
            var state = await _licenseRepository.GetStateCodeByISDCode(isdCode, TaxIdentificationId);
            return Ok(state);
        }
        #endregion

        #region Payment Method Business Link
        /// <summary>
      
        /// Getting  Payment Method Interface.
        /// UI Reffered - Payment Method Interface Tab
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodInterfacebyISDCode(int ISDCode, int BusinessKey)
        {
            var payments = await _licenseRepository.GetPaymentMethodInterfacebyISDCode(ISDCode, BusinessKey);
            return Ok(payments);

        }
        /// <summary>
        /// Insert Or Update  Payment Method Interface Required
        /// UI Reffered -Payment Method Interface Tab
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePaymentMethodInterfaceBusinessLink(List<DO_PaymentMethodBusinessLink> obj)
        {
            var msg = await _licenseRepository.InsertOrUpdatePaymentMethodInterfaceBusinessLink(obj);
            return Ok(msg);
        }
        #endregion

        #region Define Menu Link to Location
        /// <summary>
        /// Getting  All Menu .
        /// UI Reffered - Define Menu Link to Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetLocationMenuLinkbyBusinessKey(int businesskey)
        {
            var menu = await _licenseRepository.GetLocationMenuLinkbyBusinessKey(businesskey);
            return Ok(menu);

        }
        /// <summary>
        /// Insert Or Update  LocationMenuLink .
        /// UI Reffered -LocationMenuLink
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateLocationMenuLink(List<DO_LocationMenuLink> obj)
        {
            var msg = await _licenseRepository.InsertOrUpdateLocationMenuLink(obj);
            return Ok(msg);
        }
        #endregion
    }
}
