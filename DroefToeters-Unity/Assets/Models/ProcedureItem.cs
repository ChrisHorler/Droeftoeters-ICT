using System.Collections.Generic;
using UnityEngine;

public class ProcedureItem
{
    public string Id { get; set; }
    
    public string ProcedureId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string PreviousItemId { get; set; }
    
    public string NextItemId { get; set; }
}
