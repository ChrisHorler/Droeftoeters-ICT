using System.ComponentModel.DataAnnotations;

namespace droeftoeters_api.Models;

public class Procedure
{
    [Key] 
    public required string Id { get; set; }
    
    [Required(ErrorMessage = "A Title is required to make a procedure item")]
    [StringLength(64, ErrorMessage = "Title cannot be longer than 64 characters")]
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public List<ProcedureItem>? ProcedureItems { get; set; }
}