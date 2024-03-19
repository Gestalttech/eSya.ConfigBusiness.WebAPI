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
        #region Document Calendar Business Link

        public async Task<List<DO_BusinessCalendar>> GetBusinesslinkedCalendarkeys(int businessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result =await db.GtEcblcls.Where(x => x.ActiveStatus && x.BusinessKey==businessKey)
                        .Select(x => new DO_BusinessCalendar
                        {
                            CalenderKey=x.CalenderKey
                        }).ToListAsync();
                    var Distinctcals = result.GroupBy(x => x.CalenderKey).Select(y => y.First());
                    return Distinctcals.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_DocumentControl>> GetActiveDocuments()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtDccnsts.Where(x => x.ActiveStatus)
                        .Select(x => new DO_DocumentControl
                        {
                            DocumentId = x.DocumentId,
                            DocumentDesc=x.DocumentDesc
                        }).ToListAsync();
                    
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessCalendar>> GetBusinessCalendarBYBusinessKey(int businessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = await db.GtDncnbcs.Where(x => x.BusinessKey==businessKey)
                        .Join(db.GtDccnsts,
                        dl => dl.DocumentId,
                        dm => dm.DocumentId,
                        (dl, dm) => new { dl, dm })
                        .Select(x => new DO_BusinessCalendar
                                 {
                                     BusinessKey =x.dl.BusinessKey,
                                     CalenderKey=x.dl.CalenderKey,
                                     DocumentId = x.dl.DocumentId,
                                     GeneNoYearOrMonth=x.dl.GeneNoYearOrMonth,
                                     DocumentDesc = x.dm.DocumentDesc,
                                     ActiveStatus = x.dl.ActiveStatus,

                                 }
                        ).ToListAsync();
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

                        var bcExist = db.GtDncnbcs.Where(w => w.DocumentId == obj.DocumentId && w.BusinessKey == obj.BusinessKey && w.CalenderKey.ToUpper().Replace(" ", "") == obj.CalenderKey.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (bcExist != null)
                        {

                            bcExist.BusinessKey = obj.BusinessKey;
                            bcExist.CalenderKey = obj.CalenderKey;
                            bcExist.DocumentId = obj.DocumentId;
                            bcExist.GeneNoYearOrMonth = obj.GeneNoYearOrMonth;
                            bcExist.ActiveStatus = obj.ActiveStatus;
                            bcExist.ModifiedBy = obj.UserID;
                            bcExist.ModifiedOn = DateTime.Now;
                            bcExist.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                        }
                        else
                        {

                            var bc = new GtDncnbc
                            {
                                BusinessKey = obj.BusinessKey,
                                CalenderKey = obj.CalenderKey,
                                DocumentId = obj.DocumentId,
                                GeneNoYearOrMonth = obj.GeneNoYearOrMonth,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncnbcs.Add(bc);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }

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
