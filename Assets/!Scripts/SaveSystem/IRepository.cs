// --- REPOSITORY PATTERN ---
public interface IRepository<T> where T : class
{
    void Save(T data);
    T Load();
    bool HasSave();
}