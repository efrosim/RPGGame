// --- REPOSITORY PATTERN ---
public interface ISaveRepository
{
    void Save(SaveData data);
    SaveData Load();
    bool HasSave();
}