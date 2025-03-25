using droeftoeters_api.ViewModels;

namespace droeftoeters_api.Interfaces;

public interface IProcedureData : IDatabaseObject<Procedure>
{
    public bool AddProcedureItem(ProcedureItem procedureItem);

    public bool RemoveProcedureItem(string procedureItemId);
}