using System.Collections.Generic;
using UnityEngine;

public class Procedure
{
    public string Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public List<ProcedureItem> ProcedureItems { get; set; }
}
