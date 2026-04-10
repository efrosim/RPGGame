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

        // Bootstrapper handling the initial launch (ПЕРЕДАЕМ ПАРАМЕТР СЮДА)
        builder.RegisterEntryPoint<Bootstrapper>().WithParameter(_mainMenuSceneIndex);
    }
}