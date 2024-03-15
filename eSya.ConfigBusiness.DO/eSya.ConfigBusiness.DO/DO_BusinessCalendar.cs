using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DO
{
    public class DO_BusinessCalendar
    {
        public int BusinessKey { get; set; }
        public int DocumentId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public string CalendarType { get; set; } = null!;
        public DateTime? EffectiveTill { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormID { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
        public string? DocumentDesc { get; set; }
        public string? CalendarTypeDesc { get; set; }
    }
}
