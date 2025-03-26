using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;

namespace droeftoeters_api.Data;

public class ProcedureItemData : IProcedureItemData
{
    private const string TABLE = "ProcedureItems";
    private readonly IDataService _dataService;
    
    public ProcedureItemData(IDataService dataService)
    {
        _dataService = dataService;
    }

    public IEnumerable<ProcedureItem> ReadAll()
    {
        string query = @$"SELECT * FROM {TABLE}";
        
        var result = _dataService.QuerySql<ProcedureItem>(query);
        return result;
    }

    public ProcedureItem Read(string id)
    {
        string query = @$"SELECT * FROM {TABLE} WHERE [Id] = @Id";
        
        var result = _dataService.QueryFirstSql<ProcedureItem>(query, new {Id = id});
        return result;
    }

    public bool Write(ProcedureItem procedureItem)
    {
        string query = @$"INSERT INTO {TABLE}
(Id, ProcedureId, Title, Description, PreviousItemId, NextItemId)
VALUES(@Id, @ProcedureId, @Title, @Description, @PreviousItemId, @NextItemId)";
        
        var result = _dataService.ExecuteSql(query, procedureItem);
        return result;
    }

    public bool Update(ProcedureItem procedureItem)
    {
        string query = @$"UPDATE {TABLE}
SET
ProcedureId = @ProcedureId, 
Title = @Title, 
Description = @Description, 
PreviousItemId = @PreviousItemId, 
NextItemId = @NextItemId
WHERE [Id] = @Id";
        
        var result = _dataService.ExecuteSql(query, procedureItem);
        return result;
    }

    public bool Delete(string id)
    {
        string query =
            $@"DELETE FROM {TABLE}
where [Id] = @Id";

        var result = _dataService.ExecuteSql(query, new { Id = id });
        return result;
    }

    public IEnumerable<ProcedureItem> Parent(string id)
    {
        string query = $@"SELECT * FROM {TABLE} WHERE [ProcedureId] = @Id";
        
        var result = _dataService.QuerySql<ProcedureItem>(query, new { Id = id });
        return result;
    }
}