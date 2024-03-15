using eSya.ConfigBusiness.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.IF
{
    public interface ILicenseRepository
    {
        #region Business Entity
        Task<List<DO_BusinessEntity>> GetBusinessEntities();
        Task<DO_BusinessEntity> GetBusinessEntityInfo(int BusinessId);
        Task<DO_ReturnParameter> InsertBusinessEntity(DO_BusinessEntity obj);
        Task<DO_ReturnParameter> UpdateBusinessEntity(DO_BusinessEntity obj);
        Task<DO_ReturnParameter> DeleteBusinessEntity(int BusinessEntityId);
        Task<List<DO_BusinessEntity>> GetActiveBusinessEntities();
        Task<List<DO_EntityPreferredLanguage>> GetPreferredLanguagebyBusinessKey(int BusinessId);
        #endregion

        #region  Business Location
        Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessId(int BusinessId);

        Task<DO_ReturnParameter> InsertBusinessLocation(DO_BusinessLocation obj);

        Task<DO_ReturnParameter> UpdateBusinessLocation(DO_BusinessLocation obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveBusinessLocation(bool status, int BusinessId, int locationId);

        Task<List<DO_BusienssSegmentCurrency>> GetCurrencybyBusinessKey(int BusinessKey);

        Task<DO_BusinessEntity> GetBusinessUnitType(int businessId);

        Task<List<DO_eSyaParameter>> GetLocationParametersbyBusinessKey(int BusinessKey);

        Task<List<DO_LocationPreferredLanguage>> GetLocationPreferredLanguagebyBusinessKey(int BusinessID, int BusinessKey);

        #endregion

        #region Location Financial Info
         Task<DO_ReturnParameter> InsertOrUpdateLocationFinancialInfo(DO_LocationFinancialInfo obj);

       Task<DO_LocationFinancialInfo> GetLocationFinancialInfo(int BusinessKey);
        Task<List<DO_BusinessLocation>> GetActiveLocationsAsSegments();

        #endregion

        #region Location Tax Info

        Task<DO_ReturnParameter> InsertOrUpdateLocationTaxInfo(DO_LocationTaxInfo obj);
        Task<DO_LocationTaxInfo> GetLocationLocationTaxInfo(int BusinessKey);
        Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode);

        Task<List<DO_CountryCodes>> GetCurrencyListbyIsdCode(int IsdCode);


        Task<List<DO_Cities>> GetCityListbyISDCode(int isdCode);

        Task<DO_TaxIdentification> GetStateCodeByISDCode(int isdCode, int TaxIdentificationId);


        #endregion

        #region Define Menu Link to Location
        Task<DO_ConfigureMenu> GetLocationMenuLinkbyBusinessKey(int businesskey);
        Task<DO_ReturnParameter> InsertOrUpdateLocationMenuLink(List<DO_LocationMenuLink> obj);
        #endregion
    }
}
