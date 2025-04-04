using System.ComponentModel.DataAnnotations;

namespace droeftoeters_api.ViewModels;

public class ParentChild
{
    [Key]
    public string Id { get; set; }
    public string ParentId { get; set; }
    public string ChildId { get; set; }
}