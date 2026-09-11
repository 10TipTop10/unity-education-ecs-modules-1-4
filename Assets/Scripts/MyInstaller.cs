using UnityEngine;
using Zenject;

public class MyInstaller : MonoInstaller
{
    [SerializeField] private Settings _settings;
    [SerializeField] private bool _settingsFromDummy;
    public override void InstallBindings()
    {
        if (_settingsFromDummy)
        {
            Container.Bind<IConfigLoader>().To<DummyConfigLoader>().AsSingle().NonLazy();
        }
        else
        {
            Container.Bind<IConfigLoader>().To<ScriptableObjectConfigLoader>().AsSingle().NonLazy();
            Container.BindInstance(_settings);
        }

    }
}