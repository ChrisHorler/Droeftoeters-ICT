using droeftoeters_api.Interfaces;
using droeftoeters_api.DataModels;

namespace droeftoeters_api.Data;

public class ProcedureData : IProcedureData
{
    private const string TABLE = "Procedures";
    private readonly IDataService _dataService;
    private readonly IProcedureItemData _procedureItemData;

    public ProcedureData(IDataService dataService, IProcedureItemData procedureItemData)
    {
        _dataService = dataService;
        _procedureItemData = procedureItemData;
    }

    public IEnumerable<Procedure> ReadAll()
    {
        string query = @$"SELECT * FROM {TABLE}";
        var result = _dataService.QuerySql<Procedure>(query);
        return result;
    }

    public Procedure Read(string id)
    {
        var guidId = Guid.Parse(id);
        string query = @$"SELECT * FROM {TABLE} WHERE [Id] = @Id";
        var result = _dataService.QueryFirstSql<Procedure>(query, new {Id = guidId});
        return result;
    }

    public bool Write(Procedure procedure)
    {
        string query = @$"INSERT INTO {TABLE}
(Id, Title, Description)
VALUES(@Id, @Title, @Description)";
        var result = _dataService.ExecuteSql(query, procedure);
        
        if (!result) throw new("Writing procedure to table resulted in nothing happening");
        
        return result;
    }

    public bool Update(Procedure procedure)
    {
        string query = @$"UPDATE {TABLE}
SET
Title = @Title, 
Description = @Description
WHERE [Id] = @Id";
        var result = _dataService.ExecuteSql(query, procedure);
        
        if (!result) throw new("Updating procedure to table resulted in nothing happening");
        
        return result;
    }

    public bool Delete(string id)
    {
        string query =
            $@"DELETE FROM {TABLE}
where [Id] = @Id";

        var result = _dataService.ExecuteSql(query, new { Id = id });
        
        if (!result) throw new("Deleting procedure from table resulted in nothing happening");

        return result;
    }

    public bool AddProcedureItem(ProcedureItem procedureItem)
    {
        var result = _procedureItemData.Write(procedureItem);
        
        if (!result) throw new("Adding procedureitem to table resulted in nothing happening");

        return result;
    }

    public bool RemoveProcedureItem(string procedureItemId)
    {
        var result = _procedureItemData.Delete(procedureItemId);
        
        if (!result) throw new("Adding procedureitem to table resulted in nothing happening");

        return result;
    }
}