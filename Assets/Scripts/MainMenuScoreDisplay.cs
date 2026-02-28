using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class MainMenuScoreDisplay : MonoBehaviour
{
    public Button deleteButton;
    void Start() {
        deleteButton.onClick.AddListener(delete);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
    private void delete() {
        SceneManager.LoadScene("LBoard");
    }
}
