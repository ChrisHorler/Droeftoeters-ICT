namespace droeftoeters_api.Interfaces;

public interface IDatabaseObject<T>
{
    /// <summary>
    /// Reads all the items in a sql table
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> ReadAll();

    /// <summary>
    /// Searches an item in a sql table based on their guid id
    /// </summary>
    /// <param name="id">The guid being used to search</param>
    /// <returns></returns>
    public T? Read(string id);

    /// <summary>
    /// Writes an item to a sql table
    /// </summary>
    /// <param name="item">The item being written to the sql table</param>
    /// <returns></returns>
    public bool Write(T? item);

    /// <summary>
    /// Updates an item in a sql table. The item id is the target id
    /// </summary>
    /// <param name="item">The item being updated</param>
    /// <returns></returns>
    public bool Update(T? item);

    /// <summary>
    /// Deletes an item from a sql table
    /// </summary>
    /// <param name="id">The target id being deleted</param>
    /// <returns></returns>
    public bool Delete(string id);
}