using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlaceGo : MonoBehaviour
{
    public Button start;
    public Button leaderboard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start.onClick.AddListener(starT);
        leaderboard.onClick.AddListener(LeaderBoard);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void starT() {
        SceneManager.LoadScene("Game");
    }

    void LeaderBoard() {
        SceneManager.LoadScene("LBoard");
    }
}
