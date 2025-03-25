using System.ComponentModel.DataAnnotations;
using droeftoeters_api.Interfaces;

namespace droeftoeters_api.ViewModels;

public class Procedure
{
    [Key] 
    public string Id { get; set; }

    [Required(ErrorMessage = "A Title is required to make a procedure item")]
    [StringLength(64, ErrorMessage = "Title cannot be longer than 64 characters")]
    public string Title { get; set; }

    public string? Description { get; set; }

    public IEnumerable<ProcedureItem>? ProcedureItems { get; set; }
}