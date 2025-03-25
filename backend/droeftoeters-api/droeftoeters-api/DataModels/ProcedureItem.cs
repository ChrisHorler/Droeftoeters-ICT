using System.ComponentModel.DataAnnotations;

namespace droeftoeters_api.DataModels;

public class ProcedureItem
{
    [Key] 
    public string Id { get; set; }
    
    public string ProcedureId { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? PreviousItemId { get; set; }
    
    public string? NextItemId { get; set; }
}