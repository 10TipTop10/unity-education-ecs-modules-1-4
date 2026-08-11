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

//public class GreetMe
//{
//    public GreetMe(string message)
//    {
//        Debug.Log(message);
//    }
//}

//public class Test1 : ITest
//{
//    public void Echo()
//    {
//        Debug.Log("Test 1");
//    }
//}
//public class Test2 : ITest
//{
//    public void Echo()
//    {
//        Debug.Log("Test 2");
//    }
//}

//public interface ITest
//{
//    void Echo();
//}