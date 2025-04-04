using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProcedureItemController : ControllerBase
    {
        private readonly IProcedureItemData _procedureItemData;
        private readonly IProcedureData _procedureData;
        private readonly ILogger<ProcedureItemController> _logger;

        public ProcedureItemController(IProcedureItemData procedureItemData, ILogger<ProcedureItemController> logger, IProcedureData procedureData)
        {
            _procedureItemData = procedureItemData;
            _logger = logger;
            _procedureData = procedureData;
        }
        
        [HttpGet("all")]
        public IActionResult ReadAll()
        {
            try
            {
                var results = _procedureItemData.ReadAll();

                if (results == null) throw new("Reading procedure items resulted in null list");
                
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
                
                //Get procedure item by id
                var result = _procedureItemData.Read(id);
                
                //Check if the id fetched anything
                if (result == null) throw new("Procedure id fetch resulted in null");
                
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
        public IActionResult Write([FromBody] ProcedureItem procedureItem)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedureItem.Id, out _))  throw new($"Id not valid guid: {procedureItem.Id}");

                //Check if the procedure item id already exists
                if (ItemExists(procedureItem.Id)) throw new("Procedure item with this id already exists");

                //Check if writing to table succeeded
                var result = _procedureItemData.Write(procedureItem);
                if (!result) throw new("Writing procedure item to table resulted in nothing happening");
                
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
        public IActionResult Update([FromBody] ProcedureItem procedureItem)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedureItem.Id, out _))  throw new($"Id not valid guid: {procedureItem.Id}");
                
                //Check if the procedure item id doesn't exist
                if (!ItemExists(procedureItem.Id)) throw new("Procedure item with this id doesn't exist");
                
                //Check if updating to table succeeded
                var result = _procedureItemData.Update(procedureItem);
                if (!result) throw new("Updating procedure item to table resulted in nothing happening");
                
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
                
                //Check if the procedure item id already exists
                if (!ItemExists(id)) throw new("Procedure item with this id doesn't exist");
                
                //Check if deleting on table succeeded
                var result = _procedureItemData.Delete(id);
                if (!result) throw new("Deleting procedure item from table resulted in nothing happening");
                
                //Return result
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        [HttpGet("parent/{id}")]
        public IActionResult Parent(string id)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(id, out _))  throw new($"Id not valid guid: {id}");
                
                //Check if the procedure item and procedure exist
                var item = _procedureItemData.Read(id) ?? throw new("Procedure Item with this id does not exist");
                if (!ProcedureExists(item.ProcedureId)) throw new("Procedure id not found");
                return Ok(_procedureItemData.Parent(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        private bool ProcedureExists(string id) => _procedureData.Read(id) != null;
        private bool ItemExists(string id) => _procedureItemData.Read(id) != null;
    }
}
