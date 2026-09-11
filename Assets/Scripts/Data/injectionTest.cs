using UnityEngine;
using Zenject;

public class injectionTest : MonoBehaviour
{
    private IConfigLoader _configLoader;

    [Inject]
    public void Init(IConfigLoader c)
    {
        _configLoader = c;
    }

    private void Start()
    {
        //Debug.Log($"{_configLoader.HeroHealth}");
    }
}
