using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DO
{
    public class DO_BusinessEntity
    {
        public int BusinessId { get; set; }
        public string BusinessDesc { get; set; }
        public bool IsMultiSegmentApplicable { get; set; }
        public string BusinessUnitType { get; set; }
        public int NoOfUnits { get; set; }
        public int ActiveNoOfUnits { get; set; }
        public bool UsageStatus { get; set; }
        public bool ActiveStatus { get; set; }
        //public string FormId { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }

        public List<DO_EntityPreferredLanguage>? l_Preferredlang { get; set; }
    }
    public class DO_EntityPreferredLanguage
    {
        public int BusinessId { get; set; }
        //public string PreferredLanguage { get; set; } = null!;
        public string CultureCode { get; set; }
        public string? CultureDesc { get; set; }
        public string Pldesc { get; set; }
        public bool DefaultLanguage { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }


    }
}
