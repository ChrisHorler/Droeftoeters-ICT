using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using droeftoeters_api.Data;
using Newtonsoft.Json;

namespace droeftoeters_api.DataModels;

public class Procedure
{
    [Key] 
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public string? Description { get; set; }
    
    public List<ProcedureItem>? ProcedureItems { get; set; }
    
    public Procedure(ViewModels.Procedure procedure)
    {
        //Try to parse the guids
        if(!Guid.TryParse(procedure.Id, out var parsedId)) throw new("Failed parsing procedure id to guid");
        
        Id = parsedId;
        Title = procedure.Title;
        Description = procedure.Description;
        
        //Converts the viewmodel procedure items list to the datamodel variant
        ProcedureItems = procedure.ProcedureItems?
            .Select(x => new ProcedureItem(x))
            .ToList() ?? new();
    }
}