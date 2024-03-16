using eSya.ConfigBusiness.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSya.ConfigBusiness.IF
{
    public interface IRulesRepository
    {
        #region Process Rule by Business Location wise
        Task<List<DO_ProcessMaster>> GetProcessMaster();
        Task<List<DO_ProcessRule>> GetProcessRulebySegmentwise();
        Task<List<DO_ProcessRule>> GetProcessRulebyBusinessKey(int BusinessKey);
        Task<DO_ReturnParameter> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj);
        #endregion

        #region Map Rules with Location
        Task<List<DO_ProcessMaster>> GetProcessforLocationLink();
        Task<List<DO_ProcessRule>> GetProcessRuleforLocationLink();
        Task<List<DO_BusinessLocation>> GetProcessRulesMappedwithLocationByID(int processID, int ruleID);
        Task<DO_ReturnParameter> InsertOrUpdateProcessRulesMapwithLocation(List<DO_ProcessRulebySegment> obj);
        #endregion
    }
}
