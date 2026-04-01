using System.Collections.Generic;

public class SaveableRegistry
{
    private readonly List<ISaveable> _saveables = new List<ISaveable>();

    // Объект сам просит добавить его в список
    public void Register(ISaveable saveable)
    {
        if (!_saveables.Contains(saveable))
            _saveables.Add(saveable);
    }

    // Объект просит удалить его из списка (например, при смерти)
    public void Unregister(ISaveable saveable)
    {
        _saveables.Remove(saveable);
    }

    // Выдаем список всех зарегистрированных объектов
    public IEnumerable<ISaveable> GetAll() => _saveables;
}