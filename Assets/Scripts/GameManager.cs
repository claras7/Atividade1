using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScene("Splash");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadGameAndGUI()
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);
    }

    private void Start()
    {
        LoadGameAndGUI();
    }
}
