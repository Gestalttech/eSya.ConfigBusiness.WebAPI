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
    public class LicenseRepository: ILicenseRepository
    {
        private readonly IStringLocalizer<LicenseRepository> _localizer;
        public LicenseRepository(IStringLocalizer<LicenseRepository> localizer)
        {
            _localizer = localizer;
        }
        #region Business Entity
        public async Task<List<DO_BusinessEntity>> GetBusinessEntities()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsens
                                   .Where(w => w.ActiveStatus)
                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessId.ToString() + " - " + be.BusinessDesc,
                                      BusinessUnitType = be.BusinessUnitType,
                                      NoOfUnits = be.NoOfUnits,
                                      ActiveNoOfUnits = be.ActiveNoOfUnits,
                                      UsageStatus = be.UsageStatus,
                                      ActiveStatus = be.ActiveStatus
                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessEntity> GetBusinessEntityInfo(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsens
                        .Where(w => w.BusinessId == BusinessId)
                        .Select(r => new DO_BusinessEntity
                        {
                            BusinessId = r.BusinessId,
                            BusinessDesc = r.BusinessDesc,
                            BusinessUnitType = r.BusinessUnitType,
                            NoOfUnits = r.NoOfUnits,
                            ActiveNoOfUnits = r.ActiveNoOfUnits,
                            UsageStatus = r.UsageStatus,
                            ActiveStatus = r.ActiveStatus
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinessEntity(DO_BusinessEntity obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityDescExists = db.GtEcbsens.FirstOrDefault(u => u.BusinessDesc.ToUpper().Replace(" ", "") == obj.BusinessDesc.ToUpper().Replace(" ", ""));
                        if (is_EntityDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0062", Message = string.Format(_localizer[name: "W0062"]) };
                        }

                        int _businessID = db.GtEcbsens.Select(c => c.BusinessId).DefaultIfEmpty().Max();
                        _businessID = _businessID + 1;

                        var b_Entity = new GtEcbsen
                        {
                            BusinessId = _businessID,
                            BusinessDesc = obj.BusinessDesc,
                            BusinessUnitType = obj.BusinessUnitType,
                            NoOfUnits = obj.NoOfUnits,
                            ActiveNoOfUnits = obj.ActiveNoOfUnits,
                            UsageStatus = false,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcbsens.Add(b_Entity);
                        await db.SaveChangesAsync();

                        if (obj.l_Preferredlang != null)
                        {
                            foreach (var pl in obj.l_Preferredlang)
                            {
                                var plExist = db.GtEcbspls.Where(w => w.BusinessId == _businessID && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcbspls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }
                            foreach (var _pl in obj.l_Preferredlang)
                            {

                                var plang = new GtEcbspl
                                {
                                    BusinessId = _businessID,
                                    PreferredLanguage = _pl.CultureCode,
                                    Pldesc = _pl.Pldesc,
                                    DefaultLanguage = _pl.DefaultLanguage,
                                    ActiveStatus = _pl.ActiveStatus,
                                    FormId = _pl.FormID,
                                    CreatedBy = _pl.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = _pl.TerminalID
                                };
                                db.GtEcbspls.Add(plang);
                                await db.SaveChangesAsync();

                            }
                            //await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };
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

        public async Task<DO_ReturnParameter> UpdateBusinessEntity(DO_BusinessEntity obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityExists = db.GtEcbsens.FirstOrDefault(be => be.BusinessDesc.ToUpper().Replace(" ", "") == obj.BusinessDesc.ToUpper().Replace(" ", "") && be.BusinessId != obj.BusinessId);
                        if (is_EntityExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0062", Message = string.Format(_localizer[name: "W0062"]) };

                        }


                        GtEcbsen b_Entity = db.GtEcbsens.Where(be => be.BusinessId == obj.BusinessId).FirstOrDefault();
                        if (b_Entity != null)
                        {
                            b_Entity.BusinessDesc = obj.BusinessDesc;
                            b_Entity.BusinessUnitType = obj.BusinessUnitType;
                            b_Entity.NoOfUnits = obj.NoOfUnits;
                            b_Entity.ActiveNoOfUnits = obj.ActiveNoOfUnits;
                            b_Entity.ActiveStatus = obj.ActiveStatus;
                            b_Entity.ModifiedBy = obj.UserID;
                            b_Entity.ModifiedOn = System.DateTime.Now;
                            b_Entity.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();


                            if (obj.l_Preferredlang != null)
                            {
                                foreach (var pl in obj.l_Preferredlang)
                                {
                                    var plExist = db.GtEcbspls.Where(w => w.BusinessId == pl.BusinessId && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                                    if (plExist != null)
                                    {
                                        db.GtEcbspls.Remove(plExist);
                                        await db.SaveChangesAsync();
                                    }
                                }
                                foreach (var _pl in obj.l_Preferredlang)
                                {

                                    var plang = new GtEcbspl
                                    {
                                        BusinessId = _pl.BusinessId,
                                        PreferredLanguage = _pl.CultureCode,
                                        Pldesc = _pl.Pldesc,
                                        DefaultLanguage = _pl.DefaultLanguage,
                                        ActiveStatus = _pl.ActiveStatus,
                                        FormId = _pl.FormID,
                                        CreatedBy = _pl.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _pl.TerminalID
                                    };
                                    db.GtEcbspls.Add(plang);
                                    await db.SaveChangesAsync();

                                }
                                //await db.SaveChangesAsync();
                            }


                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0063", Message = string.Format(_localizer[name: "W0063"]) };

                        }
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

        public async Task<DO_ReturnParameter> DeleteBusinessEntity(int BusinessEntityId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen bu_en = db.GtEcbsens.Where(w => w.BusinessId == BusinessEntityId).FirstOrDefault();
                        if (bu_en == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0063", Message = string.Format(_localizer[name: "W0063"]) };
                        }

                        if (bu_en.UsageStatus == true)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0064", Message = string.Format(_localizer[name: "W0064"]) };

                        }

                        db.GtEcbsens.Remove(bu_en);

                        await db.SaveChangesAsync();


                        var plist = db.GtEcbspls.Where(x => x.BusinessId == BusinessEntityId).ToList();
                        if (plist != null)
                        {
                            foreach (var pl in plist)
                            {
                                var plExist = db.GtEcbspls.Where(w => w.BusinessId == pl.BusinessId && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.PreferredLanguage.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcbspls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }

                        }


                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0005", Message = string.Format(_localizer[name: "S0005"]) };
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

        public async Task<List<DO_BusinessEntity>> GetActiveBusinessEntities()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsens.Where(x => x.ActiveStatus == true)

                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessDesc

                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_EntityPreferredLanguage>> GetPreferredLanguagebyBusinessKey(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEbeculs.Where(x => x.ActiveStatus)
                        .Select(r => new DO_EntityPreferredLanguage
                        {
                            BusinessId = BusinessId,
                            CultureCode = r.CultureCode,
                            CultureDesc = r.CultureDesc,
                            ActiveStatus = false,
                            DefaultLanguage = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbspl cul = db.GtEcbspls.Where(x => x.BusinessId == BusinessId && x.PreferredLanguage.ToUpper().Replace(" ", "") == obj.CultureCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cul != null)
                        {
                            obj.ActiveStatus = cul.ActiveStatus;
                            obj.Pldesc = cul.Pldesc;
                            obj.DefaultLanguage = cul.DefaultLanguage;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                            obj.Pldesc = string.Empty;
                            obj.DefaultLanguage = false;
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

        #endregion  Business Entity

        #region  Business Location
        public async Task<List<DO_BusinessLocation>> GetBusinessLocationByBusinessId(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var locs = db.GtEcbslns.Where(x => x.BusinessId == BusinessId)
                        .Join(db.GtEccucos,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_BusinessLocation
                        {
                            BusinessId = x.BusinessId,
                            BusinessKey = x.BusinessKey,
                            LocationId = x.LocationId,
                            LocationDescription = x.LocationDescription,
                            ShortDesc = x.ShortDesc,
                            BusinessName = x.BusinessName,
                            Isdcode = x.Isdcode,
                            CityCode = x.CityCode,
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName,
                            TolocalCurrency = x.TolocalCurrency,
                            TocurrConversion = x.TocurrConversion,
                            TorealCurrency = x.TorealCurrency,
                            Lstatus=x.Lstatus,
                            ActiveStatus = x.ActiveStatus
                        }).ToListAsync();

                    return await locs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinessLocation(DO_BusinessLocation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region Location Master

                        int _locationID = db.GtEcbslns.Where(x => x.BusinessId == obj.BusinessId).Select(c => c.LocationId).DefaultIfEmpty().Max();
                        _locationID = _locationID + 1;
                        obj.LocationId = _locationID;

                        GtEcbsln exists = db.GtEcbslns.Where(x => x.BusinessId == obj.BusinessId && x.LocationId == obj.LocationId).FirstOrDefault();
                        if (exists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0065", Message = string.Format(_localizer[name: "W0065"]) };
                        }


                        GtEcbsln is_locDescExists = db.GtEcbslns.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == obj.LocationDescription.ToUpper().Replace(" ", "")
                        && l.BusinessId == obj.BusinessId);

                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0066", Message = string.Format(_localizer[name: "W0066"]) };

                        }
                        int Business_Key = Convert.ToInt32(obj.BusinessId.ToString() + obj.LocationId.ToString());
                        var is_BusinessKeyExist = db.GtEcbslns.Where(x => x.ActiveStatus == true && x.BusinessKey == Business_Key).FirstOrDefault();
                        if (is_BusinessKeyExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0067", Message = string.Format(_localizer[name: "W0067"]) };
                        }
                        var b_Location = new GtEcbsln
                        {
                            BusinessId = obj.BusinessId,
                            LocationId = obj.LocationId,
                            BusinessKey = Business_Key,
                            LocationDescription = obj.LocationDescription,
                            BusinessName = obj.BusinessName,
                            ShortDesc = obj.ShortDesc,
                            Isdcode = obj.Isdcode,
                            CityCode = obj.CityCode,
                            CurrencyCode = obj.CurrencyCode,
                            TolocalCurrency = obj.TolocalCurrency,
                            TocurrConversion = obj.TocurrConversion,
                            TorealCurrency = obj.TorealCurrency,
                            Lstatus = false,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,
                        };
                        db.GtEcbslns.Add(b_Location);
                        await db.SaveChangesAsync();

                        #endregion Location Master

                        #region location currencies

                        if (obj.TorealCurrency)
                        {
                            var fa = await db.GtEcbsscs.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcbssc f in fa)
                            {
                                f.ActiveStatus = false;
                                f.ModifiedBy = obj.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = obj.TerminalID;
                            }
                            await db.SaveChangesAsync();

                            if (obj.l_BSCurrency != null)
                            {
                                foreach (DO_BusienssSegmentCurrency i in obj.l_BSCurrency)
                                {
                                    var obj_FA = await db.GtEcbsscs.Where(w => w.BusinessKey == Business_Key && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.ModifiedBy = obj.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcbssc();
                                        obj_FA.BusinessKey = Business_Key;
                                        obj_FA.TocurrencyCode = i.CurrencyCode;
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.FormId = obj.FormId;
                                        obj_FA.CreatedBy = obj.UserID;
                                        obj_FA.CreatedOn = DateTime.Now;
                                        obj_FA.CreatedTerminal = System.Environment.MachineName;
                                        db.GtEcbsscs.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
                            }
                        }

                        #endregion location currencies

                        #region location parameter

                        if (obj.l_FormParameter != null)
                        {
                            foreach (DO_eSyaParameter i in obj.l_FormParameter)
                            {
                                var obj_FA = await db.GtEcpabls.Where(w => w.BusinessKey == Business_Key && w.ParameterId == i.ParameterID).FirstOrDefaultAsync();
                                if (obj_FA != null)
                                {
                                    obj_FA.ParmAction = i.ParmAction;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcpabl();
                                    obj_FA.BusinessKey = Business_Key;
                                    obj_FA.ParameterId = i.ParameterID;
                                    obj_FA.ParmPerc = 0;
                                    obj_FA.ParmAction = i.ParmAction;
                                    obj_FA.ParmDesc = null;
                                    obj_FA.ParmValue = 0;
                                    obj_FA.ActiveStatus = obj.ActiveStatus;
                                    obj_FA.FormId = obj.FormId;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = System.DateTime.Now;
                                    obj_FA.CreatedTerminal = obj.TerminalID;

                                    db.GtEcpabls.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }

                        else
                        {
                            var fa = await db.GtEcpabls.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcpabl f in fa)
                            {
                                f.ParmAction = false;
                                f.ActiveStatus = false;
                                f.ModifiedBy = obj.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = obj.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        #endregion location parameter

                        #region Location Preferred Language
                        if (obj.l_Preferredlanguage != null)
                        {
                            foreach (var pl in obj.l_Preferredlanguage)
                            {
                                var plExist = db.GtEcblpls.Where(w => w.BusinessKey == Business_Key && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.PreferredLanguage.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcblpls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }
                            foreach (var _pl in obj.l_Preferredlanguage)
                            {

                                var plang = new GtEcblpl
                                {
                                    BusinessKey = Business_Key,
                                    PreferredLanguage = _pl.PreferredLanguage,
                                    ActiveStatus = _pl.ActiveStatus,
                                    FormId = _pl.FormID,
                                    CreatedBy = _pl.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = _pl.TerminalId
                                };
                                db.GtEcblpls.Add(plang);

                            }
                            await db.SaveChangesAsync();

                        }
                        #endregion Location Preferred Language

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]), Key = Business_Key.ToString() };
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

        public async Task<DO_ReturnParameter> UpdateBusinessLocation(DO_BusinessLocation obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        #region Location Master

                        GtEcbsln is_locDescExists = db.GtEcbslns.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == obj.LocationDescription.ToUpper().Replace(" ", "") &&
                        l.BusinessId == obj.BusinessId && l.LocationId != obj.LocationId);
                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0066", Message = string.Format(_localizer[name: "W0066"]) };
                        }

                        GtEcbsln b_loc = db.GtEcbslns.Where(bl => bl.BusinessId == obj.BusinessId && bl.LocationId == obj.LocationId).FirstOrDefault();

                        if (b_loc != null)
                        {
                            b_loc.LocationDescription = obj.LocationDescription;
                            b_loc.BusinessName = obj.BusinessName;
                            b_loc.ShortDesc = obj.ShortDesc;
                            b_loc.Isdcode = obj.Isdcode;
                            b_loc.CityCode = obj.CityCode;
                            b_loc.CurrencyCode = obj.CurrencyCode;
                            b_loc.TolocalCurrency = obj.TolocalCurrency;
                            b_loc.TocurrConversion = obj.TocurrConversion;
                            b_loc.TorealCurrency = obj.TorealCurrency;
                            b_loc.ActiveStatus = obj.ActiveStatus;
                            b_loc.ModifiedBy = obj.UserID;
                            b_loc.ModifiedOn = System.DateTime.Now;
                            b_loc.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();

                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0068", Message = string.Format(_localizer[name: "W0068"]) };
                        }
                        #endregion Location Master

                        #region Location Currencies

                        if (obj.TorealCurrency)
                        {
                            if (obj.l_BSCurrency != null)
                            {
                                foreach (DO_BusienssSegmentCurrency i in obj.l_BSCurrency)
                                {
                                    var obj_FA = await db.GtEcbsscs.Where(w => w.BusinessKey == b_loc.BusinessKey && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.ModifiedBy = obj.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcbssc();
                                        obj_FA.BusinessKey = b_loc.BusinessKey;
                                        obj_FA.TocurrencyCode = i.CurrencyCode;
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.FormId = obj.FormId;
                                        obj_FA.CreatedBy = obj.UserID;
                                        obj_FA.CreatedOn = DateTime.Now;
                                        obj_FA.CreatedTerminal = System.Environment.MachineName;
                                        db.GtEcbsscs.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var bsCurrency = db.GtEcbsscs.Where(w => w.BusinessKey == b_loc.BusinessKey).ToList();

                            if (bsCurrency != null)
                                db.GtEcbsscs.RemoveRange(bsCurrency);
                            await db.SaveChangesAsync();
                        }

                        #endregion Location Currencies

                        #region Location Parameters
                        if (obj.l_FormParameter != null)
                        {
                            foreach (DO_eSyaParameter i in obj.l_FormParameter)
                            {
                                var obj_FA = await db.GtEcpabls.Where(w => w.BusinessKey == b_loc.BusinessKey && w.ParameterId == i.ParameterID).FirstOrDefaultAsync();
                                if (obj_FA != null)
                                {
                                    obj_FA.ParmAction = i.ParmAction;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcpabl();
                                    obj_FA.BusinessKey = b_loc.BusinessKey;
                                    obj_FA.ParameterId = i.ParameterID;
                                    obj_FA.ParmPerc = 0;
                                    obj_FA.ParmAction = i.ParmAction;
                                    obj_FA.ParmDesc = null;
                                    obj_FA.ParmValue = 0;
                                    obj_FA.ActiveStatus = obj.ActiveStatus;
                                    obj_FA.FormId = obj.FormId;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = System.DateTime.Now;
                                    obj_FA.CreatedTerminal = obj.TerminalID;

                                    db.GtEcpabls.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }
                        #endregion Location Parameter

                        #region Location Preferred Language
                        if (obj.l_Preferredlanguage != null)
                        {
                            foreach (var pl in obj.l_Preferredlanguage)
                            {
                                var plExist = db.GtEcblpls.Where(w => w.BusinessKey == pl.BusinessKey && w.PreferredLanguage.ToUpper().Replace(" ", "") == pl.PreferredLanguage.ToUpper().Replace(" ", "")).FirstOrDefault();
                                if (plExist != null)
                                {
                                    db.GtEcblpls.Remove(plExist);
                                    await db.SaveChangesAsync();
                                }
                            }
                            foreach (var _pl in obj.l_Preferredlanguage)
                            {

                                var plang = new GtEcblpl
                                {
                                    BusinessKey = _pl.BusinessKey,
                                    PreferredLanguage = _pl.PreferredLanguage,
                                    ActiveStatus = _pl.ActiveStatus,
                                    FormId = _pl.FormID,
                                    CreatedBy = _pl.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = _pl.TerminalId
                                };
                                db.GtEcblpls.Add(plang);

                            }
                            await db.SaveChangesAsync();
                        }
                        #endregion

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveBusinessLocation(bool status, int BusinessId, int locationId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsln b_loc = db.GtEcbslns.Where(bl => bl.BusinessId == BusinessId && bl.LocationId == locationId).FirstOrDefault();
                        if (b_loc == null)
                        {
                            return new DO_ReturnParameter() { Status = false, StatusCode = "W0068", Message = string.Format(_localizer[name: "W0068"]) };
                        }

                        b_loc.ActiveStatus = status;
                        if (b_loc != null)
                        {
                            GtEcbsfi fininfo = db.GtEcbsfis.Where(bl => bl.BusinessKey == b_loc.BusinessKey).FirstOrDefault();
                            if (fininfo != null)
                            {
                                fininfo.ActiveStatus = status;

                            }
                        }
                        //if (b_loc != null)
                        //{
                        //    GtEcbsli licinfo = db.GtEcbslis.Where(bl => bl.BusinessKey == b_loc.BusinessKey).FirstOrDefault();
                        //    if (licinfo != null)
                        //    {
                        //        licinfo.ActiveStatus = status;
                        //    }
                        //}
                        if (b_loc != null)
                        {
                            GtEcbstx taxinfo = db.GtEcbstxes.Where(bl => bl.BusinessKey == b_loc.BusinessKey).FirstOrDefault();
                            if (taxinfo != null)
                            {
                                taxinfo.ActiveStatus = status;
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0003", Message = string.Format(_localizer[name: "S0003"]) };
                        else
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0004", Message = string.Format(_localizer[name: "S0004"]) };
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

        public async Task<List<DO_BusienssSegmentCurrency>> GetCurrencybyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEccucos
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusienssSegmentCurrency
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName,
                            ActiveStatus = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbssc sbCurrency = db.GtEcbsscs.Where(x => x.BusinessKey == BusinessKey && x.TocurrencyCode.ToUpper().Replace(" ", "") == obj.CurrencyCode.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (sbCurrency != null)
                        {
                            obj.ActiveStatus = sbCurrency.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
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

        public async Task<DO_BusinessEntity> GetBusinessUnitType(int businessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_BusinessEntity _entity = new DO_BusinessEntity();

                    var bt = await db.GtEcbsens.Where(x => x.BusinessId == businessId).FirstOrDefaultAsync();
                    if (bt != null)
                    {
                        _entity.BusinessUnitType = bt.BusinessUnitType;
                    }

                    return _entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_eSyaParameter>> GetLocationParametersbyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcpabls
                        .Where(w => w.BusinessKey == BusinessKey)
                        .Select(r => new DO_eSyaParameter
                        {
                            ParameterID = r.ParameterId,
                            ParmAction = r.ParmAction,
                        }).ToListAsync();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_LocationPreferredLanguage>> GetLocationPreferredLanguagebyBusinessKey(int BusinessID, int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcbspls.Where(x => x.ActiveStatus && x.BusinessId == BusinessID)
                        .Join(db.GtEbeculs.Where(x => x.ActiveStatus),
                         x => x.PreferredLanguage.ToUpper().Replace(" ", ""),
                         y => y.CultureCode.ToUpper().Replace(" ", ""),

                        (x, y) => new DO_LocationPreferredLanguage
                        {
                            BusinessKey = BusinessKey,
                            PreferredLanguage = x.PreferredLanguage,
                            Pldescription = x.Pldesc,
                            CultureDesc = y.CultureDesc,
                            ActiveStatus = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcblpl cul = db.GtEcblpls.Where(x => x.BusinessKey == BusinessKey && x.PreferredLanguage.ToUpper().Replace(" ", "") == obj.PreferredLanguage.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (cul != null)
                        {
                            obj.ActiveStatus = cul.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
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

        #endregion

        #region Location Financial Info
        public async Task<DO_ReturnParameter> InsertOrUpdateLocationFinancialInfo(DO_LocationFinancialInfo obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        if (obj.IsBookOfAccounts == true)
                        {
                            obj.BusinessSegmentId = 0;
                        }

                        GtEcbsfi locfin = db.GtEcbsfis.FirstOrDefault(l => l.BusinessKey == obj.BusinessKey);

                        if (locfin != null)
                        {
                            locfin.IsBookOfAccounts = obj.IsBookOfAccounts;
                            locfin.BusinessSegmentId = obj.BusinessSegmentId;
                            locfin.ActiveStatus = obj.ActiveStatus;
                            locfin.ModifiedBy = obj.UserID;
                            locfin.ModifiedOn = System.DateTime.Now;
                            locfin.ModifiedTerminal = obj.TerminalId;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            var locinfo = new GtEcbsfi
                            {

                                BusinessKey = obj.BusinessKey,
                                IsBookOfAccounts = obj.IsBookOfAccounts,
                                BusinessSegmentId = obj.BusinessSegmentId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                            };
                            db.GtEcbsfis.Add(locinfo);
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
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_LocationFinancialInfo> GetLocationFinancialInfo(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsfis.Where(w => w.BusinessKey == BusinessKey)
                      .Select(x => new DO_LocationFinancialInfo
                      {
                          BusinessKey = x.BusinessKey,
                          IsBookOfAccounts = x.IsBookOfAccounts,
                          BusinessSegmentId = x.BusinessSegmentId,
                          ActiveStatus = x.ActiveStatus
                      }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_BusinessLocation>> GetActiveLocationsAsSegments()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsfis.Where(w => w.IsBookOfAccounts && w.ActiveStatus).Join(db.GtEcbslns.Where(w => w.ActiveStatus),
                         x => x.BusinessKey,
                         y => y.BusinessKey,
                        (x, y) => new DO_BusinessLocation
                        {
                            SegmentId = y.BusinessKey,
                            BusinessName = y.BusinessName
                        }).OrderBy(o => o.SegmentId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Location Tax Info

        public async Task<DO_ReturnParameter> InsertOrUpdateLocationTaxInfo(DO_LocationTaxInfo obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {


                        GtEcbstx taxinfo = db.GtEcbstxes.FirstOrDefault(l => l.BusinessKey == obj.BusinessKey);
                        if (taxinfo != null)
                        {
                            taxinfo.BusinessKey = obj.BusinessKey;
                            taxinfo.TaxIdentificationId = obj.TaxIdentificationId;
                            taxinfo.ActiveStatus = obj.ActiveStatus;
                            taxinfo.ModifiedBy = obj.UserID;
                            taxinfo.ModifiedOn = System.DateTime.Now;
                            taxinfo.ModifiedTerminal = obj.TerminalId;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, StatusCode = "S0002", Message = string.Format(_localizer[name: "S0002"]) };

                        }
                        else
                        {
                            var tax = new GtEcbstx
                            {

                                BusinessKey = obj.BusinessKey,
                                TaxIdentificationId = obj.TaxIdentificationId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalId,
                            };
                            db.GtEcbstxes.Add(tax);
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
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public async Task<DO_LocationTaxInfo> GetLocationLocationTaxInfo(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbstxes.Where(w => w.BusinessKey == BusinessKey)
                      .Select(x => new DO_LocationTaxInfo
                      {
                          BusinessKey = x.BusinessKey,
                          TaxIdentificationId = x.TaxIdentificationId,
                          ActiveStatus = x.ActiveStatus
                      }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntis.Where(w => w.Isdcode == ISDCode && w.ActiveStatus == true)
                        .Select(x => new DO_TaxIdentification
                        {
                            Isdcode = x.Isdcode,
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
                            ActiveStatus = x.ActiveStatus
                        }).OrderBy(o => o.TaxIdentificationId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_CountryCodes>> GetCurrencyListbyIsdCode(int IsdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var Currency = db.GtEccncds.Where(c => c.Isdcode == IsdCode && c.ActiveStatus).Join(db.GtEccucos,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes
                        {
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName
                        }).ToListAsync();


                    return await Currency;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<DO_Cities>> GetCityListbyISDCode(int isdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var pf = db.GtAddrcts.Where(x => x.Isdcode == isdCode && x.ActiveStatus)
                   .Select(s => new DO_Cities
                   {
                       CityCode = s.CityCode,
                       CityDesc = s.CityDesc
                   }).
                    Distinct()
                   .ToListAsync();
                    return await pf;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_TaxIdentification> GetStateCodeByISDCode(int isdCode, int TaxIdentificationId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntis.Where(w => w.Isdcode == isdCode && w.TaxIdentificationId == TaxIdentificationId && w.ActiveStatus)
                        .Select(x => new DO_TaxIdentification
                        {
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
                            StateCode = x.StateCode,

                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Payment Method Business Link
        public async Task<List<DO_PaymentMethodBusinessLink>> GetPaymentMethodInterfacebyISDCode(int ISDCode,int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEccnpms.Where(x => x.Isdcode == ISDCode && x.ActiveStatus)
                       .Join(db.GtEcapcds,
                       a => a.PaymentMethod,
                       p => p.ApplicationCode,
                       (a, p) => new { a, p })
                      .Join(db.GtEcapcds,
                      aa => aa.a.InstrumentType,
                      I => I.ApplicationCode,
                      (aa, I) => new { aa, I })
                     .Select(r => new DO_PaymentMethodBusinessLink
                     {
                         Isdcode = r.aa.a.Isdcode,
                         PaymentMethod = r.aa.a.PaymentMethod,
                         InstrumentType = r.aa.a.InstrumentType,
                         PaymentMethodDesc = r.aa.p.CodeDesc,
                         InstrumentTypeDesc = r.I.CodeDesc,
                         //ActiveStatus = r.aa.a.ActiveStatus,
                         InterfaceReqd=false,
                         ActiveStatus=false,
                         BusinessKey=BusinessKey,
                     }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbspm blink = db.GtEcbspms.Where(x => x.BusinessKey == BusinessKey && x.Isdcode == ISDCode && x.PaymentMethod==obj.PaymentMethod
                        && x.InstrumentType==obj.InstrumentType).FirstOrDefault();
                        if (blink != null)
                        {
                            obj.InterfaceReqd = blink.InterfaceReqd;
                            obj.ActiveStatus = blink.ActiveStatus;
                        }
                        else
                        {
                            obj.InterfaceReqd = false;
                            obj.ActiveStatus = false;
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
        public async Task<DO_ReturnParameter> InsertOrUpdatePaymentMethodInterfaceBusinessLink(List<DO_PaymentMethodBusinessLink> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        //foreach (var p in obj)
                        //{
                        //    var payExists = db.GtEcbspms.Where(w => w.BusinessKey == p.BusinessKey && w.Isdcode == p.Isdcode && w.PaymentMethod==p.PaymentMethod && w.InstrumentType==p.InstrumentType).FirstOrDefault();
                        //    if (payExists != null)
                        //    {
                        //        db.GtEcbspms.Remove(payExists);
                        //        await db.SaveChangesAsync();
                        //    }
                        //}
                        foreach (var p in obj)
                        {
                            if (p.InterfaceReqd == true && p.ActiveStatus == false)
                            {
                                return new DO_ReturnParameter() { Status = false, StatusCode = "W0114", Message = string.Format(_localizer[name: "W0114"]) };
                            }
                        }
                        foreach (var _pl in obj)
                        {
                            var payExists = db.GtEcbspms.Where(w => w.BusinessKey == _pl.BusinessKey && w.Isdcode == _pl.Isdcode && w.PaymentMethod == _pl.PaymentMethod && w.InstrumentType == _pl.InstrumentType).FirstOrDefault();
                            if (payExists != null)
                            {
                                payExists.ActiveStatus = _pl.ActiveStatus;
                                payExists.InterfaceReqd = _pl.InterfaceReqd;
                                payExists.ModifiedBy = _pl.UserID;
                                payExists.ModifiedOn = System.DateTime.Now;
                                payExists.ModifiedTerminal = _pl.TerminalID;
                            }
                            else
                            {
                                var plink = new GtEcbspm
                                {
                                    BusinessKey = _pl.BusinessKey,
                                    Isdcode = _pl.Isdcode,
                                    PaymentMethod = _pl.PaymentMethod,
                                    InstrumentType = _pl.InstrumentType,
                                    InterfaceReqd = _pl.InterfaceReqd,
                                    ActiveStatus = _pl.ActiveStatus,
                                    FormId = _pl.FormID,
                                    CreatedBy = _pl.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = _pl.TerminalID
                                };
                                db.GtEcbspms.Add(plink);
                            }
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, StatusCode = "S0001", Message = string.Format(_localizer[name: "S0001"]) };

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
        #region Define Menu Link to Location
        public async Task<DO_ConfigureMenu> GetLocationMenuLinkbyBusinessKey(int businesskey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    DO_ConfigureMenu mn = new DO_ConfigureMenu();
                    mn.l_MainMenu = await db.GtEcmamns.Where(x => x.ActiveStatus)
                                    .Select(m => new DO_MainMenu()
                                    {
                                        MainMenuId = m.MainMenuId,
                                        MainMenu = m.MainMenu,
                                        MenuIndex = m.MenuIndex,
                                        ActiveStatus = m.ActiveStatus
                                    }).ToListAsync();

                    mn.l_SubMenu = await db.GtEcsbmns.Where(x => x.ActiveStatus)
                                    .Select(s => new DO_SubMenu()
                                    {
                                        MainMenuId = s.MainMenuId,
                                        MenuItemId = s.MenuItemId,
                                        MenuItemName = s.MenuItemName,
                                        MenuIndex = s.MenuIndex,
                                        ParentID = s.ParentId,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();
                    mn.l_FormMenu = await db.GtEcmnfls.Where(x => x.ActiveStatus)

                   .GroupJoin(db.GtEcbsmns.Where(x => x.BusinessKey == businesskey),
                    m => new { m.MenuKey },
                    fm => new { fm.MenuKey },
                    (m, fm) => new { m, fm })
                    .SelectMany(z => z.fm.DefaultIfEmpty(),

                     (a, b) => new DO_FormMenu()
                     {
                       MainMenuId = a.m.MainMenuId,
                        MenuItemId = a.m.MenuItemId,
                        FormId = a.m.MenuKey,
                        FormNameClient = a.m.FormNameClient,
                       FormIndex = a.m.FormIndex,
                       ActiveStatus = b == null ? false : b.ActiveStatus

                     }).ToListAsync();

                    //mn.l_FormMenu = await db.GtEcmnfls.Where(x => x.ActiveStatus)
                    //                .Select(f => new DO_FormMenu()
                    //                {
                    //                    MainMenuId = f.MainMenuId,
                    //                    MenuItemId = f.MenuItemId,
                    //                    //FormId = f.FormId,
                    //                    FormId = f.MenuKey,
                    //                    FormNameClient = f.FormNameClient,
                    //                    FormIndex = f.FormIndex,
                    //                    ActiveStatus = f.ActiveStatus,

                    //                }).ToListAsync();
                    //foreach (var obj in mn.l_FormMenu)
                    //{
                    //    GtEcbsmn menulink = db.GtEcbsmns.Where(c => c.BusinessKey == businesskey && c.MenuKey == obj.FormId).FirstOrDefault();
                    //    if (menulink != null)
                    //    {
                    //        obj.ActiveStatus = menulink.ActiveStatus;
                    //    }
                    //    else
                    //    {
                    //        obj.ActiveStatus = false;
                    //    }
                    //}
                    return mn;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DO_ReturnParameter> InsertOrUpdateLocationMenuLink(List<DO_LocationMenuLink> obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var _link in obj)
                        {
                            var _linkExist = db.GtEcbsmns.Where(w => w.MenuKey == _link.MenuKey && w.BusinessKey == _link.BusinessKey).FirstOrDefault();
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
                                    var _loclink = new GtEcbsmn
                                    {
                                        BusinessKey = _link.BusinessKey,
                                        MenuKey = _link.MenuKey,
                                        ActiveStatus = _link.ActiveStatus,
                                        CreatedBy = _link.UserID,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedTerminal = _link.TerminalID
                                    };
                                    db.GtEcbsmns.Add(_loclink);
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
