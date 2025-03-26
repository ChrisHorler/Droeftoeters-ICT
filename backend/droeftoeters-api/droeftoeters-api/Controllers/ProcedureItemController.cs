using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
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
                if(!Guid.TryParse(id, out _)) throw new($"Invalid id guid supplied: {id}");

                var result = _procedureItemData.Read(id);

                //Check if item exists
                if (result == null) throw new("procedure item not found");
                    
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
                if(!Guid.TryParse(procedureItem.Id, out _)) throw new($"Invalid id guid supplied: {procedureItem.Id}");

                //Check if item id or name already exists in the database
                var checkItem = _procedureItemData.Read(procedureItem.Id);
                if (checkItem != null || checkItem!.Title == procedureItem.Title) 
                    throw new("creation of procedure item failed because id or name of item already exists");

                var success = _procedureItemData.Write(procedureItem);
                
                //Check if execution succeeded 
                if (!success) throw new("Writing procedure item to database failed");
                
                return Ok(success);
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
                if(!Guid.TryParse(procedureItem.Id, out _)) throw new($"Invalid id guid supplied: {procedureItem.Id}");

                if (!Exists(procedureItem.Id))
                    throw new Exception("Updating procedure item failed because the id does not exist");
                
                var success = _procedureItemData.Update(procedureItem);
                
                //Check if execution succeeded 
                if (!success) throw new("Writing procedure item to database failed");
                
                return Ok(success);
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
                //Check if id exists
                if (!Exists(id)) throw new("Deleting procedure item failed because the id does not exist");
                
                var success = _procedureItemData.Delete(id);
                
                //Check if execution succeeded 
                if (!success) throw new("Writing item to database failed");
                
                return Ok(success);
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
                //Check if id exists
                if (!Exists(id)) throw new("Getting parent from item failed because the item id does not exist");
                
                var item = _procedureItemData.Read(id)!;

                var result = _procedureItemData.Parent(item.ProcedureId);
                
                //Add the procedure items
                result.ProcedureItems = _procedureData.Children(result.Id);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }

        /// <summary>
        /// Check if procedure item exists
        /// </summary>
        /// <param name="id">the procedure item id being looked up</param>
        /// <returns></returns>
        private bool Exists(string id) => _procedureItemData.Read(id) != null;
    }
}
