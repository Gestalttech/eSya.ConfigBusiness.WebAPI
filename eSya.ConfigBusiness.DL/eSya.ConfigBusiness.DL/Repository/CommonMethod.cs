using eSya.ConfigBusiness.DL.Entities;
using eSya.ConfigBusiness.DO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.DL.Repository
{
    public class CommonMethod
    {
        #region Common Methods
        public static string GetValidationMessageFromException(DbUpdateException ex)
        {
            string msg = ex.InnerException == null ? ex.ToString() : ex.InnerException.Message;

            if (msg.LastIndexOf(',') == msg.Length - 1)
                msg = msg.Remove(msg.LastIndexOf(','));
            return msg;
        }

        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var bk = db.GtEcbslns
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessKey = r.BusinessKey,
                            LocationDescription = r.BusinessName + "-" + r.LocationDescription
                        }).ToListAsync();

                    return await bk;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_CountryCodes>> GetISDCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccncds
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_CountryCodes
                        {
                            Isdcode = r.Isdcode,
                            CountryName = r.CountryName
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<List<DO_TaxStructure>> GetTaxCodeByISDCodes(int ISDCode)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var ds = db.GtEccntcs
        //                .Where(w => w.Isdcode == ISDCode)
        //                .Select(r => new DO_TaxStructure
        //                {
        //                    TaxCode = r.TaxCode,
        //                    TaxDescription = r.TaxDescription,
        //                }).OrderBy(o => o.TaxDescription).ToListAsync();

        //            return await ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<List<DO_Forms>> GetFormDetails()
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {
        //            var result = db.GtEcfmfds
        //                          .Where(w => w.ActiveStatus)
        //                          .Select(r => new DO_Forms
        //                          {
        //                              FormID = r.FormId,
        //                              FormName = r.FormName
        //                          }).OrderBy(o => o.FormName).ToListAsync();
        //            return await result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<List<DO_TaxStructure>> GetTaxCode(int ISDCode)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var ds = db.GtEccntcs
        //                .Where(w => w.Isdcode == ISDCode && w.IsSplitApplicable && w.ActiveStatus)
        //                .Select(r => new DO_TaxStructure
        //                {
        //                    TaxCode = r.TaxCode,
        //                    TaxDescription = r.TaxDescription,
        //                }).OrderBy(o => o.TaxDescription).ToListAsync();

        //            return await ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public async Task<List<DO_TaxStructure>> GetTaxCodesListByISDCode(int Isdcode)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var ds = db.GtEccnsds.Where(x => x.Isdcode == Isdcode && x.ActiveStatus)
        //               .Join(db.GtEcsupas.Where(w => w.ParameterId == 5),
        //                x => new { x.Isdcode, x.StatutoryCode },
        //                y => new { y.Isdcode, y.StatutoryCode },
        //               (x, y) => new DO_TaxStructure
        //               {
        //                   TaxCode = x.StatutoryCode,
        //                   TaxDescription = x.StatutoryDescription,
        //               }).ToListAsync();

        //            return await ds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<List<DO_CountryCodes>> GetIndiaISDCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccncds
                        .Where(w => w.Isdcode == 91 && w.ActiveStatus)
                        .Select(r => new DO_CountryCodes
                        {
                            Isdcode = r.Isdcode,
                            CountryName = r.CountryName
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<DO_CurrencyMaster>> GetActiveCurrencyCodes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var currencies = db.GtEccucos
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_CurrencyMaster
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName
                        }).ToListAsync();

                    return await currencies;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
