using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureData _procedureData;
        private readonly IProcedureItemData _procedureItemData;
        private readonly ILogger<ProcedureController> _logger;
        
        public ProcedureController(IProcedureData procedureData, IProcedureItemData procedureItemData, ILogger<ProcedureController> logger)
        {
            _procedureData = procedureData;
            _procedureItemData = procedureItemData;
            _logger = logger;
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll()
        {
            try
            {
                var results = _procedureData.ReadAll();

                foreach (var result in results)
                {
                    result.ProcedureItems = _procedureItemData.Parent(result.Id);
                }
                
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult Read(string id)
        {
            try
            {
                if(!Guid.TryParse(id, out _)) throw new($"Id not valid guid: {id}");
                //TODO: check if id exists
                
                var result = _procedureData.Read(id);
                
                //Add the procedure items
                result.ProcedureItems = _procedureItemData.Parent(result.Id);
                
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Write([FromBody] Procedure procedure)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedure.Id, out _)) throw new("Invalid guid supplied");
                
                //TODO: check if procedure doesnt exist
                
                return Ok(_procedureData.Write(procedure));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }   
        
        [HttpPut]
        public IActionResult Update([FromBody] Procedure procedure)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedure.Id, out _)) throw new("Invalid guid supplied");
                
                //TODO: check if procedure exists
                
                return Ok(_procedureData.Update(procedure));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }  
        
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(id, out _)) throw new("Invalid guid supplied");
                
                //TODO: check if id exists
                return Ok(_procedureData.Delete(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpPost("additem")]
        public IActionResult AddProcedureItem(ProcedureItem procedureItem)
        {
            try
            {
                //TODO: check if procedure exists
                return Ok(_procedureData.AddProcedureItem(procedureItem));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        [HttpDelete("removeitem/{id}")]
        public IActionResult RemoveProcedureItem(string id)
        {
            try
            {
                //TODO: check if procedure item id exists
                return Ok(_procedureData.RemoveProcedureItem(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
    }
}
