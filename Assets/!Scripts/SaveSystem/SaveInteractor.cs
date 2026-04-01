// --- START OF FILE SaveInteractor.cs ---
using System.Collections.Generic;

public class SaveInteractor : ISaveInteractor
{
    private readonly ISaveRepository _repository;
    private readonly SaveableRegistry _registry;
    
    public SaveInteractor(ISaveRepository repository, SaveableRegistry registry)
    {
        _repository = repository;
        _registry = registry;
    }

    public bool HasSave() => _repository.HasSave();

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.enemies = new List<EnemySaveData>();

        // МГНОВЕННО получаем только те объекты, которые нужно сохранить!
        foreach (var saveable in _registry.GetAll())
        {
            saveable.SaveState(data);
        }

        _repository.Save(data);
    }

    public void LoadGame()
    {
        SaveData data = _repository.Load();
        if (data == null) return;

        foreach (var saveable in _registry.GetAll())
        {
            saveable.LoadState(data);
        }
    }
}