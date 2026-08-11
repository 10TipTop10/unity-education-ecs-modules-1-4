
public class ScriptableObjectConfigLoader : IConfigLoader
{
    private readonly Settings _settings;

    public ScriptableObjectConfigLoader(Settings settings)
    {
        _settings = settings;
    }

    public int HeroHealth => _settings.HeroHealth;
}