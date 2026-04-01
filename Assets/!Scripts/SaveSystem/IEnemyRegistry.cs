using System.Collections.Generic;

public interface IEnemyRegistry
{
    IEnumerable<IEnemyStateProvider> GetAllEnemies();
}
