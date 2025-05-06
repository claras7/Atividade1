using UnityEngine;

public class SplashController : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadNextScene", 2f);
    }

    void LoadNextScene()
    {
        GameManager.Instance.LoadScene("MainMenu");
    }
}
