using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string sceneName; 
    public void changeScene() {
        SceneManager.LoadScene(sceneName);
    }
    public void Update() {
        if (Input.GetButtonDown("Start")) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
