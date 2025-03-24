using System.ComponentModel.DataAnnotations;

namespace droeftoeters_api.Models;

public class ProcedureItem
{
    [Key] 
    public string Id { get; set; }
    
    [Required(ErrorMessage = "The procedureId is required to bind a procedure item to a procedure")]
    public string ProcedureId { get; set; }
    
    [Required(ErrorMessage = "A Title is required to make a procedure item")]
    [StringLength(64, ErrorMessage = "Title cannot be longer than 64 characters")]
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? PreviousItemId { get; set; }
    
    public string? NextItemId { get; set; }
}