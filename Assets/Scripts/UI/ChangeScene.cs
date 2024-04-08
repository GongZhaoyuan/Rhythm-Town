using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeTo(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
