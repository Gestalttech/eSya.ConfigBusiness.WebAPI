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
    public class RulesRepository:IRulesRepository
    {
        private readonly IStringLocalizer<RulesRepository> _localizer;
        public RulesRepository(IStringLocalizer<RulesRepository> localizer)
        {
            _localizer = localizer;
        }

        #region Process Rule by Business Location wise

        public async Task<List<DO_ProcessRule>> GetProcessRulebySegmentwise()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEcaprls
                              .GroupJoin(db.GtEcaprbs,
                              a => new { a.RuleId, a.ProcessId },
                              f => new { f.RuleId, f.ProcessId },
                              (a, f) => new { a, f = f.FirstOrDefault() })
                              .Select(r => new DO_ProcessRule
                              {
                                  RuleId = r.a.RuleId,
                                  ProcessId = r.a.ProcessId,
                                  RuleDesc = r.a.RuleDesc,
                                  ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                              }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessRule>> GetProcessRulebyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {


                    var ds = await db.GtEcprrls.Where(x => x.IsSegmentSpecific == true)
                      .Join(db.GtEcaprls,
                      f => f.ProcessId,
                      p => p.ProcessId,
                      (f, p) => new { f, p })
                  .GroupJoin(db.GtEcaprbs.Where(w => w.BusinessKey == BusinessKey),
                    e => new { e.p.RuleId, e.p.ProcessId },
                    d => new { d.RuleId, d.ProcessId },
                   (emp, depts) => new { emp, depts })
                  .SelectMany(z => z.depts.DefaultIfEmpty(),
                   (a, b) => new DO_ProcessRule
                   {
                       RuleId = a.emp.p.RuleId,
                       ProcessId = a.emp.p.ProcessId,
                       RuleDesc = a.emp.p.RuleDesc,
                       ActiveStatus = b == null ? false : b.ActiveStatus
                   }).ToListAsync();
                    return ds;

                    //var Distinctforms = ds.GroupBy(x => x.FormID).Select(y => y.First());
                    //return Distinctforms.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pr_ruleExists = db.GtEcaprbs.Where(x => x.BusinessKey == obj.BusinessKey && x.ProcessId == obj.ProcessId && x.RuleId == obj.RuleId).FirstOrDefault();
                        if (pr_ruleExists == null)
                        {
                            var pr_rule = new GtEcaprb
                            {
                                BusinessKey = obj.BusinessKey,
                                RuleId = obj.RuleId,
                                ProcessId = obj.ProcessId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcaprbs.Add(pr_rule);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
                        }
                        else
                        {
                            pr_ruleExists.BusinessKey = obj.BusinessKey;
                            pr_ruleExists.ProcessId = obj.ProcessId;
                            pr_ruleExists.RuleId = obj.RuleId;
                            pr_ruleExists.ActiveStatus = obj.ActiveStatus;
                            pr_ruleExists.ModifiedBy = obj.UserID;
                            pr_ruleExists.ModifiedOn = System.DateTime.Now;
                            pr_ruleExists.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
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

        #region Map Rules with Location
        public async Task<List<DO_ProcessMaster>> GetProcessforLocationLink()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrls.Where(x => x.ActiveStatus && x.IsSegmentSpecific == true)
                        .Select(r => new DO_ProcessMaster
                        {
                            ProcessId = r.ProcessId,
                            ProcessDesc = r.ProcessDesc,
                            IsSegmentSpecific = r.IsSegmentSpecific,
                            SystemControl = r.SystemControl,
                            ProcessControl = r.ProcessControl,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_ProcessRule>> GetProcessRuleforLocationLink()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrls.Where(x => x.ActiveStatus && x.IsSegmentSpecific)
                        .Join(db.GtEcaprls.Where(x => x.ActiveStatus),
                         x => x.ProcessId,
                         y => y.ProcessId,
                        (x, y) => new DO_ProcessRule
                        {
                            ProcessId = y.ProcessId,
                            RuleId = y.RuleId,
                            RuleDesc = y.RuleDesc,
                            ActiveStatus = y.ActiveStatus
                        }).ToListAsync();


                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_BusinessLocation>> GetProcessRulesMappedwithLocationByID(int processID, int ruleID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {

                    var ds = await db.GtEcbslns.Where(x => x.ActiveStatus == true)
                     .Select(f => new DO_BusinessLocation()
                     {
                         BusinessKey = f.BusinessKey,
                         LocationDescription = f.BusinessName + "-" + f.LocationDescription,
                         ActiveStatus = f.ActiveStatus
                     }).ToListAsync();


                    foreach (var _link in ds)
                    {
                        GtEcaprb _lexista = db.GtEcaprbs.Where(c => c.BusinessKey == _link.BusinessKey && c.ProcessId == processID && c.RuleId == ruleID).FirstOrDefault();
                        if (_lexista != null)
                        {
                            _link.ActiveStatus = _lexista.ActiveStatus;

                        }
                        else
                        {
                            _link.ActiveStatus = false;
                        }
                    }

                    return ds;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateProcessRulesMapwithLocation(List<DO_ProcessRulebySegment> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _link in obj)
                        {
                            var _linkExist = db.GtEcaprbs.Where(w => w.BusinessKey == _link.BusinessKey && w.ProcessId == _link.ProcessId && w.RuleId == _link.RuleId).FirstOrDefault();
                            if (_linkExist != null)
                            {
                                if (_linkExist.ActiveStatus != _link.ActiveStatus)
                                {
                                    _linkExist.ActiveStatus = _link.ActiveStatus;
                                    _linkExist.ModifiedBy = _link.UserID;
                                    _linkExist.ModifiedOn = System.DateTime.Now;
                                    _linkExist.ModifiedTerminal = _link.TerminalID;
                                }

                            }
                            else
                            {
                                if (_link.ActiveStatus)
                                {
                                    var _loclink = new GtEcaprb
                                    {
                                        BusinessKey = _link.BusinessKey,
                                        ProcessId = _link.ProcessId,
                                        RuleId = _link.RuleId,
                                        ActiveStatus = _link.ActiveStatus,
                                        CreatedBy = _link.UserID,
                                        FormId = _link.FormID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _link.TerminalID
                                    };
                                    db.GtEcaprbs.Add(_loclink);
                                }

                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        return new DO_ReturnParameter() { Status = false, Message = ex.Message };
                    }
                }
            }
        }
        #endregion
    }
}
