using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
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

                if (results == null) throw new("Reading procedures resulted in null list");
                
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
                //Validate guid
                if(!Guid.TryParse(id, out _)) throw new($"Id not valid guid: {id}");
                
                //Get procedure by id
                var result = _procedureData.Read(id);
                
                //Check if the id fetched anything
                if (result == null) throw new("Procedure id fetch resulted in null");
                
                //Add the procedure items
                result.ProcedureItems = _procedureItemData.Parent(result.Id);
                
                //Return result
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
                if(!Guid.TryParse(procedure.Id, out _))  throw new($"Id not valid guid: {procedure.Id}");

                //Check if the procedure id already exists
                if (ProcedureExists(procedure.Id)) throw new("Procedure with this id already exists");

                //Check if writing to table succeeded
                var result = _procedureData.Write(procedure);
                if (!result) throw new("Writing procedure to table resulted in nothing happening");
                
                //Return result
                return Ok(result);
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
                if(!Guid.TryParse(procedure.Id, out _))  throw new($"Id not valid guid: {procedure.Id}");
                
                //Check if the procedure id doesn't exist
                if (!ProcedureExists(procedure.Id)) throw new("Procedure with this id doesn't exist");
                
                //Check if updating to table succeeded
                var result = _procedureData.Update(procedure);
                if (!result) throw new("Updating procedure to table resulted in nothing happening");
                
                //Return result
                return Ok(result);
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
                if(!Guid.TryParse(id, out _))  throw new($"Id not valid guid: {id}");
                
                //Check if the procedure id already exists
                if (!ProcedureExists(id)) throw new("Procedure with this id doesn't exist");
                
                //Check if deleting on table succeeded
                var result = _procedureData.Delete(id);
                if (!result) throw new("Deleting procedure from table resulted in nothing happening");
                
                //Return result
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        private bool ItemExists(string id) => _procedureItemData.Read(id) != null;

        private bool ProcedureExists(string id) => _procedureData.Read(id) != null;
    }
}
