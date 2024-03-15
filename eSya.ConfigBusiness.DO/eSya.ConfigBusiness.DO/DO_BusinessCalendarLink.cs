using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DO
{
    public class DO_BusinessCalendarLink
    {
        public int BusinessKey { get; set; }
        public string CalenderType { get; set; }
        public decimal Year { get; set; }
        public string? CalenderKey { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public bool YearEndStatus { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public bool Alreadylinked { get; set; }
    }
}
