using droeftoeters_api.Interfaces;
using droeftoeters_api.ViewModels;

namespace droeftoeters_api.Data;

public class ParentChildData : IParentChildData
{
 private const string TABLE = "Parent_Child_Connections";
    private readonly IDataService _dataService;

    public ParentChildData(IDataService dataService)
    {
        _dataService = dataService;
    }

    public IEnumerable<ParentChild> ReadAll()
    {
        string query = @$"SELECT * FROM {TABLE}";
        var result = _dataService.QuerySql<ParentChild>(query);
        return result;
    }

    public ParentChild Read(string id)
    {
        string query = @$"SELECT * FROM {TABLE} WHERE [ParentId] = @Id";
        var result = _dataService.QueryFirstSql<ParentChild>(query, new {Id = id});
        return result;
    }

    public bool Write(ParentChild parentChild)
    {
        string query = @$"INSERT INTO {TABLE}
(Id, ParentId, ChildId)
VALUES(@Id, @ParentId, @ChildId)";
        var result = _dataService.ExecuteSql(query, parentChild);
        
        if (!result) throw new("Writing parent child combination to table resulted in nothing happening");
        
        return result;
    }

    public bool Update(ParentChild parentChild)
    {
        //Nothing to see here
        return false;
    }

    public bool Delete(string id)
    {
        string query =
            $@"DELETE FROM {TABLE}
where [ParentId] = @Id";

        var result = _dataService.ExecuteSql(query, new { Id = id });
        
        if (!result) throw new("Deleting parent child combination from table resulted in nothing happening");

        return result;
    }
}