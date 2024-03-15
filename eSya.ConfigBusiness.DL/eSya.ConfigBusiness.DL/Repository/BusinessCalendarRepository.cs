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
    public class BusinessCalendarRepository: IBusinessCalendarRepository
    {
        private readonly IStringLocalizer<BusinessCalendarRepository> _localizer;
        public BusinessCalendarRepository(IStringLocalizer<BusinessCalendarRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Business Calendar
        public async Task<List<DO_BusinessCalendar>> GetBusinessCalendarBYBusinessKey(int businessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var defaultDate = DateTime.Now.Date;
                    var result = await db.GtDccnsts.Where(x => x.ActiveStatus)
                        .GroupJoin(db.GtDncnbcs.Where(w => w.BusinessKey == businessKey),
                        d => d.DocumentId,
                        bc => bc.DocumentId,
                        (d, bc) => new { d, bc })
                        .SelectMany(z => z.bc.DefaultIfEmpty(),
                                 (a, b) => new DO_BusinessCalendar
                                 {
                                     BusinessKey = businessKey,
                                     DocumentId = a.d.DocumentId,
                                     DocumentDesc = a.d.DocumentDesc,
                                     CalendarType = b != null ? b.CalendarType : "",
                                     EffectiveFrom = b != null ? b.EffectiveFrom : defaultDate,
                                     ActiveStatus = b != null ? b.ActiveStatus : false,

                                 }
                        ).ToListAsync();

                    foreach (var dc in result)
                    {
                        if (dc.CalendarType != null)
                        {
                            dc.CalendarTypeDesc = (dc.CalendarType == "CY") ? "Calendar Year" : (dc.CalendarType == "FY") ? "Financial Year" :
                                 (dc.CalendarType == "NA") ? "Not Applicable" : "";
                        }
                        else
                        {
                            dc.CalendarTypeDesc = string.Empty;
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessCalendar(DO_BusinessCalendar obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var bcExist = db.GtDncnbcs.Where(w => w.DocumentId == obj.DocumentId && w.BusinessKey == obj.BusinessKey && w.EffectiveTill == null).FirstOrDefault();
                        if (bcExist != null)
                        {
                            if (obj.EffectiveFrom != bcExist.EffectiveFrom)
                            {
                                if (obj.EffectiveFrom < bcExist.EffectiveFrom)
                                {
                                    return new DO_ReturnParameter() { Status = false, StatusCode = "W0183", Message = string.Format(_localizer[name: "W0183"]) };
                                }
                                bcExist.EffectiveTill = obj.EffectiveFrom.AddDays(-1);
                                bcExist.ModifiedBy = obj.UserID;
                                bcExist.ModifiedOn = DateTime.Now;
                                bcExist.ModifiedTerminal = obj.TerminalID;
                                bcExist.ActiveStatus = false;

                                var bc = new GtDncnbc
                                {
                                    BusinessKey = obj.BusinessKey,
                                    DocumentId = obj.DocumentId,
                                    CalendarType = obj.CalendarType,
                                    EffectiveFrom = obj.EffectiveFrom,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtDncnbcs.Add(bc);


                            }
                            else
                            {
                                bcExist.CalendarType = obj.CalendarType;
                                bcExist.ActiveStatus = obj.ActiveStatus;
                                bcExist.ModifiedBy = obj.UserID;
                                bcExist.ModifiedOn = DateTime.Now;
                                bcExist.ModifiedTerminal = obj.TerminalID;
                            }

                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(obj.CalendarType))
                            {
                                var bcal = new GtDncnbc
                                {
                                    BusinessKey = obj.BusinessKey,
                                    DocumentId = obj.DocumentId,
                                    CalendarType = obj.CalendarType,
                                    EffectiveFrom = obj.EffectiveFrom,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId = obj.FormID,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtDncnbcs.Add(bcal);
                            }

                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                }
            }
        }

        #endregion
    }
}
