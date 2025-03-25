using System.ComponentModel.DataAnnotations;

namespace droeftoeters_api.DataModels;

public class ProcedureItem
{
    [Key] 
    public Guid Id { get; set; }
    
    public string ProcedureId { get; set; }
    
    public string Title { get; set; }
    
    public string? Description { get; set; }
    
    public Guid? PreviousItemId { get; set; }
    
    public Guid? NextItemId { get; set; }
    
    public ProcedureItem(ViewModels.ProcedureItem procedureItem)
    {
        //Try to parse all the guids
        if(!Guid.TryParse(procedureItem.Id, out var parsedId)) throw new("Failed parsing procedure item id to guid");
        if(!Guid.TryParse(procedureItem.PreviousItemId, out var parsedPreviousId)) throw new("Failed parsing procedure item previousItemId to guid");
        if(!Guid.TryParse(procedureItem.NextItemId, out var parsedNextId)) throw new("Failed parsing procedure item nextItemId to guid");
        
        Id = parsedId;
        ProcedureId = procedureItem.ProcedureId;
        Title = procedureItem.Title;
        Description = procedureItem.Description;
        PreviousItemId = parsedPreviousId;
        NextItemId = parsedNextId;
    }
}