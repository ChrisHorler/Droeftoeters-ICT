using droeftoeters_api.Interfaces;
using droeftoeters_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureController : ControllerBase
    {
        private IProcedureData _procedureData;
        private ILogger<ProcedureController> _logger;

        public ProcedureController(IProcedureData procedureData, ILogger<ProcedureController> logger)
        {
            _procedureData = procedureData;
            _logger = logger;
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll()
        {
            try
            {
                return Ok(_procedureData.ReadAll());
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
                return Ok(_procedureData.Read(id));
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
                
                return Ok(_procedureData.Write(procedure));
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
                return Ok(_procedureData.Delete(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpPost("additem")]
        public IActionResult AddItem(ProcedureItem procedureItem)
        {
            try
            {
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
