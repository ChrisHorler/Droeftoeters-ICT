using droeftoeters_api.ViewModels;

namespace droeftoeters_api.Interfaces;

public interface IProcedureItemData : IDatabaseObject<ProcedureItem>
{
    public Procedure Parent(string id);
}