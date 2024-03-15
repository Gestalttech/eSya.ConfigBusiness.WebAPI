using System;
using System.Collections.Generic;

namespace eSya.ConfigBusiness.DL.Entities
{
    public partial class GtDncnbc
    {
        public int BusinessKey { get; set; }
        public int DocumentId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public string CalendarType { get; set; } = null!;
        public DateTime? EffectiveTill { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
