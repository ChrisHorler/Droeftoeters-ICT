using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers
{
    [Route("api/[controller]")]
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
                
                //Fill the procedure items list with its children
                foreach (var result in results)
                {
                    result.ProcedureItems = _procedureData.Children(result.Id);
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
                if(!Guid.TryParse(id, out _)) throw new($"Invalid id guid supplied: {id}");
                
                var result = _procedureData.Read(id);

                if (result == null) throw new("Procedure not found");
                
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

        [HttpPost]
        public IActionResult Write([FromBody] Procedure procedure)
        {
            try
            {
                //Validate guid
                if(!Guid.TryParse(procedure.Id, out _)) throw new($"Invalid id guid supplied: {procedure.Id}");
                
                //Check if procedure id or name already exists in the database
                var checkItem = _procedureData.Read(procedure.Id);
                if (checkItem != null || checkItem!.Title == procedure.Title) 
                    throw new("creation of procedure item failed because id or name of item already exists");

                var success = _procedureData.Write(procedure);
                
                //Check if execution succeeded
                if (!success) throw new("Writing procedure to database failed");
                    
                return Ok(success);
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
                if(!Guid.TryParse(procedure.Id, out _)) throw new($"Invalid id guid supplied: {procedure.Id}");

                //Checks if id exists
                if (!Exists(procedure.Id))
                    throw new Exception("Updating procedure failed because the id does not exist");
                
                var success = _procedureData.Update(procedure);
                
                //Check if execution succeeded
                if (!success) throw new("Updating procedure to database failed");
                
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
                //Validate guid
                if(!Guid.TryParse(id, out _)) throw new("Invalid guid supplied");
                
                //Checks if id exists
                if (!Exists(id))
                    throw new Exception("Deleting procedure failed because the id does not exist");
                
                var success= _procedureData.Delete(id);
                
                //Check if execution succeeded
                if (!success) throw new("Deleting procedure on database failed");
                
                return Ok(success);
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
                //Checks if procedure exists
                if (!Exists(procedureItem.ProcedureId))
                    throw new Exception("Adding procedure item failed because the procedure does not exist");

                var success = _procedureData.AddProcedureItem(procedureItem);
                
                //Check if execution succeeded
                if (!success) throw new("Adding item to procedure on database failed");
                
                return Ok(success);
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
                //Checks if procedure exists
                var procedure = _procedureItemData.Parent(id);
                if (!Exists(procedure.Id))
                    throw new Exception("Removing procedure item failed because the procedure does not exist");
                
                var success = _procedureData.RemoveProcedureItem(id);
                
                //Check if execution succeeded
                if (!success) throw new("Adding item to procedure on database failed");
                
                return Ok(success);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        [HttpGet("children/{id}")]
        public IActionResult Children(string id)
        {
            try
            {
                //Check if id exists
                if (!Exists(id)) throw new("Getting children from procedure failed because the procedure id does not exist");

                return Ok(_procedureData.Children(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Check if procedures exists
        /// </summary>
        /// <param name="id">the procedure id being looked up</param>
        /// <returns></returns>
        private bool Exists(string id) => _procedureData.Read(id) != null;
    }
}
