using eSya.ConfigBusiness.DL.Repository;
using eSya.ConfigBusiness.DO;
using eSya.ConfigBusiness.IF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eSya.ConfigBusiness.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RulesController : ControllerBase
    {
        private readonly IRulesRepository _rulesRepository;
        public RulesController(IRulesRepository rulesRepository)
        {
            _rulesRepository = rulesRepository;
        }
        #region Process Rule by Business Location wise
        
             [HttpGet]
        public async Task<IActionResult> GetProcessMaster()
        {
            var ds = await _rulesRepository.GetProcessMaster();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetProcessRulebySegmentwise()
        {
            var ds = await _rulesRepository.GetProcessRulebySegmentwise();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetProcessRulebyBusinessKey(int BusinessKey)
        {
            var ds = await _rulesRepository.GetProcessRulebyBusinessKey(BusinessKey);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj)
        {
            var msg = await _rulesRepository.InsertorUpdateProcessRulebySegment(obj);
            return Ok(msg);

        }
        #endregion

        #region  Map Rules with Location
        [HttpGet]
        public async Task<IActionResult> GetProcessforLocationLink()
        {
            var ds = await _rulesRepository.GetProcessforLocationLink();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetProcessRuleforLocationLink()
        {
            var ds = await _rulesRepository.GetProcessRuleforLocationLink();
            return Ok(ds);
        }
        [HttpGet]
        public async Task<IActionResult> GetProcessRulesMappedwithLocationByID(int processID, int ruleID)
        {
            var ds = await _rulesRepository.GetProcessRulesMappedwithLocationByID(processID, ruleID);
            return Ok(ds);
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateProcessRulesMapwithLocation(List<DO_ProcessRulebySegment> obj)
        {
            var msg = await _rulesRepository.InsertOrUpdateProcessRulesMapwithLocation(obj);
            return Ok(msg);

        }
        #endregion
    }
}
