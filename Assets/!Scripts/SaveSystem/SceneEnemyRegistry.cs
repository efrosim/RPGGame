using System.Collections.Generic;
using UnityEngine;

public class SceneEnemyRegistry : IEnemyRegistry
{
    public IEnumerable<IEnemyStateProvider> GetAllEnemies()
    {
        return Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }
}
