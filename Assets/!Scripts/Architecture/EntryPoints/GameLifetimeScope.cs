using VContainer;
using VContainer.Unity;
using UnityEngine;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private int _mainMenuSceneIndex = 1;

    protected override void Configure(IContainerBuilder builder)
    {
        // Core Services
        builder.Register<IAudioService, UnityAudioService>(Lifetime.Singleton);
        builder.Register<ISceneLoaderService, UnitySceneLoader>(Lifetime.Singleton);

        // Repositories
        builder.Register<IPlayerRepository, PlayerJsonRepository>(Lifetime.Singleton);
        builder.Register<IEnemyRepository, EnemyJsonRepository>(Lifetime.Singleton);

        // Interactors
        builder.Register<ISaveInteractor, SaveInteractor>(Lifetime.Singleton);

        // Factories
        builder.Register<IWeaponFactory, EnemyWeaponFactory>(Lifetime.Singleton);

        // Инжектим зависимости во все LocalSpawner, которые уже висят на сцене
        var spawners = FindObjectsByType<LocalSpawner>(FindObjectsSortMode.None);
        foreach (var spawner in spawners)
        {
            builder.RegisterComponent(spawner);
        }

        // Bootstrapper handling the initial launch
        builder.RegisterEntryPoint<Bootstrapper>().WithParameter(_mainMenuSceneIndex);
    }
}