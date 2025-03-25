using droeftoeters_api.DataModels;

namespace droeftoeters_api.Interfaces;

public interface IProcedureData : IDatabaseObject<Procedure>
{
    public bool AddProcedureItem(ProcedureItem procedureItem);

    public bool RemoveProcedureItem(string procedureItemId);
}