using eSya.ConfigBusiness.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.IF
{
    public interface ICalendarControlRepository
    {
        Task<List<DO_BusinessCalendarLink>> GetCalendarHeaders(int businesskey);
        Task<DO_ReturnParameter> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj);
    }
}
