using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DO
{
    public class DO_BusinessLocation
    {
        public int BusinessId { get; set; }
        public int LocationId { get; set; }
        public int BusinessKey { get; set; }
        public string ShortDesc { get; set; } = null!;
        public string LocationDescription { get; set; } = null!;
        public string BusinessName { get; set; } = null!;
        public int Isdcode { get; set; }
        public int CityCode { get; set; }
        public string CurrencyCode { get; set; } = null!;
        //public bool? TolocalCurrency { get; set; }
        //public bool TocurrConversion { get; set; }
        //public bool TorealCurrency { get; set; }
        public bool Lstatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? CurrencyName { get; set; }
        public int SegmentId { get; set; }
        public List<DO_BusienssSegmentCurrency> l_BSCurrency { get; set; }
        public List<DO_eSyaParameter>? l_FormParameter { get; set; }
        public List<DO_LocationPreferredLanguage>? l_Preferredlanguage { get; set; }
        public bool SMSIntegration { get; set; }
        public bool EmailIntegration { get; set; }
        public bool SecurityQuestionIntegration { get; set; }
        public string? DateFormat { get; set; } 
        public string? ShortDateFormat { get; set; } 
    }
    public class DO_BusienssSegmentCurrency
    {
        public int BusinessId { get; set; }
        public int SegmentId { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsTransacting { get; set; }
        public bool IsReal { get; set; }
        public string? CurrencyName { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }
    }
    public class DO_LocationPreferredLanguage
    {
        public int BusinessKey { get; set; }
        public string PreferredLanguage { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }
        public string? CultureDesc { get; set; }
        public string? Pldescription { get; set; }
    }
    public class DO_LocationFinancialInfo
    {
        public int BusinessKey { get; set; }
        public bool IsBookOfAccounts { get; set; }
        public int BusinessSegmentId { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }
    }
    public class DO_LocationLicenseInfo
    {
        public int BusinessKey { get; set; }
        public byte[] EBusinessKey { get; set; } = null!;
        public string ESyaLicenseType { get; set; } = null!;
        public int EUserLicenses { get; set; }
        public byte[] EActiveUsers { get; set; } = null!;
        public int ENoOfBeds { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }



    }
    public class DO_LocationTaxInfo
    {
        public int BusinessKey { get; set; }
        public int TaxIdentificationId { get; set; }
        public bool ActiveStatus { get; set; }
        public int UserID { get; set; }
        public string FormID { get; set; }
        public string TerminalId { get; set; }

    }
    public class DO_eSyaParameter
    {
        public int ParameterID { get; set; }
        public bool ParmAction { get; set; }
        public decimal? ParmValue { get; set; }
        public decimal? ParmPerct { get; set; }
        public string? ParmDesc { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class DO_TaxIdentification
    {
        public int Isdcode { get; set; }
        public int TaxIdentificationId { get; set; }
        public string TaxIdentificationDesc { get; set; }
        public string StateCode { get; set; }
        public bool ActiveStatus { get; set; }
    }
    public class DO_CountryCodes
    {
        public int Isdcode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; } 
        public string CurrencyName { get; set; } 

    }
    public class DO_Cities
    {
        public int CityCode { get; set; }
        public string CityDesc { get; set; }
       
    }
    public class DO_CurrencyMaster
    {
        public string CurrencyCode { get; set; } = null!;
        public string CurrencyName { get; set; } = null!;
        
    }
}
