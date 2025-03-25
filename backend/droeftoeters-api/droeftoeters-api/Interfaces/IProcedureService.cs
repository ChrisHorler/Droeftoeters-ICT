using droeftoeters_api.ViewModels;

namespace droeftoeters_api.Interfaces;

public interface IProcedureService : IRepositoryObject<Procedure>
{
    public bool AddProcedureItem(ProcedureItem procedureItem);

    public bool RemoveProcedureItem(string procedureItemId);
}