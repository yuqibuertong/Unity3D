public interface ISceneController {
    void LoadResources();
}

public class SSDirector : System.Object {
    private static SSDirector _instance;

    public ISceneController currentScenceController { get; set; }
    public bool running { get; set; }

    public static SSDirector getInstance() {
        if (_instance == null) _instance = new SSDirector();
        return _instance;
    }
}
