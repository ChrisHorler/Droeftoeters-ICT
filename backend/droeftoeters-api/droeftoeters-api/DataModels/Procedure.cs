using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using droeftoeters_api.Data;
using Newtonsoft.Json;

namespace droeftoeters_api.DataModels;

public class Procedure
{
    [Key] 
    public required string Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public List<ProcedureItem>? ProcedureItems { get; set; }
}