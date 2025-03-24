namespace droeftoeters_api.Interfaces;

public interface IDatabaseObject<T>
{
    public IEnumerable<T> ReadAll();

    public T Read(string id);

    public bool Write(T item);

    public bool Update(T item);

    public bool Delete(string id);
}