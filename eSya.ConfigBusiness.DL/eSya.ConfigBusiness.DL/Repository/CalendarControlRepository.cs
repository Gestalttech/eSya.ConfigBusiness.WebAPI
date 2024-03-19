using eSya.ConfigBusiness.DL.Entities;
using eSya.ConfigBusiness.DO;
using eSya.ConfigBusiness.IF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DL.Repository
{
    public class CalendarControlRepository: ICalendarControlRepository
    {
        private readonly IStringLocalizer<LicenseRepository> _localizer;
        public CalendarControlRepository(IStringLocalizer<LicenseRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Calendar Business Link
        public async Task<List<DO_BusinessCalendarLink>> GetCalendarHeaders(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var result = await db.GtEcclcos

                                 .Select(c => new DO_BusinessCalendarLink
                                 {
                                     CalenderType = c.CalenderType,
                                     StartMonth=c.StartMonth,
                                     CalenderKey = c.CalenderKey,
                                     YearEndStatus = c.YearEndStatus,
                                     ActiveStatus = c.ActiveStatus
                                 }).ToListAsync();
                    List<DO_BusinessCalendarLink> lstheader = new List<DO_BusinessCalendarLink>();

                    foreach (var link in result)
                    {
                        var exists = db.GtEcblcls.Where(x => x.CalenderKey == link.CalenderKey && x.BusinessKey== businesskey).FirstOrDefault();
                        if (exists != null)
                        {
                            link.Alreadylinked = true;
                        }
                        else
                        {
                            link.Alreadylinked = false;
                        }
                        DO_BusinessCalendarLink model = new DO_BusinessCalendarLink()
                        {
                            CalenderType = link.CalenderType,
                            StartMonth=link.StartMonth,
                            CalenderKey = link.CalenderKey,
                            YearEndStatus = link.YearEndStatus,
                            ActiveStatus = link.ActiveStatus,
                            Alreadylinked = link.Alreadylinked
                        };
                        lstheader.Add(model);
                    }
                    return lstheader;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertBusinessKeyIntoCalendar(DO_BusinessCalendarLink obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {

                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        
                        var cblink = db.GtEcblcls.Where(x => x.BusinessKey == obj.BusinessKey && x.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cblink != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W00168", Message = string.Format(_localizer[name: "W00168"]) };
                        }
                        GtEcblcl calblink = new GtEcblcl()
                        {
                            BusinessKey = obj.BusinessKey,
                            CalenderKey = obj.CalenderKey,
                            YearEndStatus = obj.YearEndStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcblcls.Add(calblink);
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                    }

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }

            }
        }
        #endregion
    }
}
