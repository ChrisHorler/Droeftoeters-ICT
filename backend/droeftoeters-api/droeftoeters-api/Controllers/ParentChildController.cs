using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace droeftoeters_api.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class ParentChildController : Controller
{
    private IParentChildData _parentChildData;
    private ILogger<ParentChildController> _logger;

    public ParentChildController(IParentChildData parentChildData, ILogger<ParentChildController> logger)
    {
        _parentChildData = parentChildData;
        _logger = logger;
    }
    
    
    [HttpGet("all")]
        public IActionResult ReadAll()
        {
            try
            {
                //Get all parent child combinations
                var results = _parentChildData.ReadAll();
                
                //Check if the data layer fetched anything
                if (results == null) throw new("Parent child list read all resulted in null");
                
                //Return result
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
                if (!Guid.TryParse(id, out _)) throw new($"Parent child combination id not valid guid: {id}");
                
                //Get parent child combination
                var result = _parentChildData.Read(id);

                //Check if the id fetched anything
                if (result == null) throw new("Parent child id fetch resulted in null");
                
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
        public IActionResult Write([FromBody] ParentChild parentChild)
        {
            try
            {
                //Validate guids
                if(!Guid.TryParse(parentChild.Id, out _)) 
                    throw new($"Invalid parentchild id supplied: {parentChild.Id}");
                if(!Guid.TryParse(parentChild.ParentId, out _)) 
                    throw new($"Invalid parent id supplied: {parentChild.Id}");
                if(!Guid.TryParse(parentChild.ChildId, out _)) 
                    throw new($"Invalid child id supplied");

                //Check if parent child combination doesn't already exist
                if (Exists(parentChild)) throw new("Parent child combination already exists");
                
                //Return result
                return Ok(_parentChildData.Write(parentChild));
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
                
                //Check if the id exists
                if (!IdExists(id)) throw new Exception("Target id for parent child deletion does not exist");
                
                //Return result
                return Ok(_parentChildData.Delete(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + "\n" + e.InnerException);
                return BadRequest();
            }
        }
        
        private bool IdExists(string id) => _parentChildData.Read(id) != null;

        private bool Exists(ParentChild parentChild)
        { 
            var results = _parentChildData.ReadAll();
            foreach (var result in results)
            {
                if (result.ChildId == parentChild.ChildId && result.ParentId == parentChild.ParentId)
                {
                    return true;
                }
            }
            return false;
        }
}