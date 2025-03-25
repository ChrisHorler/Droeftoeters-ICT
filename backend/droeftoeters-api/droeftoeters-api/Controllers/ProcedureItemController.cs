using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcedureItemController : ControllerBase
    {
        private IProcedureItemData _procedureItemData;
        private ILogger<ProcedureItemController> _logger;

        public ProcedureItemController(IProcedureItemData procedureItemData, ILogger<ProcedureItemController> logger)
        {
            _procedureItemData = procedureItemData;
            _logger = logger;
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll()
        {
            try
            {
                return Ok(_procedureItemData.ReadAll());
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
                return Ok(_procedureItemData.Read(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Write([FromBody] ProcedureItem procedureItem)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedureItem.Id, out _)) throw new("Invalid guid supplied");
                
                //TODO: check if id or name exists
                
                return Ok(_procedureItemData.Write(procedureItem));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }          
        
        [HttpPut]
        public IActionResult update([FromBody] ProcedureItem procedureItem)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedureItem.Id, out _)) throw new("Invalid guid supplied");
                
                //TODO: check if procedure item exists
                
                return Ok(_procedureItemData.Update(procedureItem));
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
                //TODO: check if id exists
                return Ok(_procedureItemData.Delete(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
    }
}
