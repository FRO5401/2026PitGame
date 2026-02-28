using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI first;
    public TextMeshProUGUI second;
    public TextMeshProUGUI third;
    public Button Leave;

    [System.Serializable]
    public class LeaderboardEntry
    {
        public int score;
        public float lastTime;
        public string name;
        public string team;

        public string ToDisplayString()
        {
            return "Board " + score + " | " + lastTime.ToString("0.00") + "s : " + name + " " + team;
        }
    }

    void Start()
    {
        AssignButtonListeners();

        List<LeaderboardEntry> entries = LoadLeaderboard();

        int lastScore = PlayerPrefs.GetInt("LastScore", 0);
        float lastTime = PlayerPrefs.GetFloat("LastTime", 0f);

        if (lastScore > 0)
        {
            entries.Add(new LeaderboardEntry
            {
                score = lastScore,
                lastTime = lastTime,
                name = PlayerPrefs.GetString("currentName"),
                team = PlayerPrefs.GetString("currentTeam")
            });
        }

        entries = entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.lastTime)
            .Take(5)
            .ToList();

        SaveLeaderboard(entries);
        DisplayLeaderboard(entries);

        PlayerPrefs.SetInt("LastScore", 0);
        PlayerPrefs.SetFloat("LastTime", 0);
    }

    void AssignButtonListeners()
    {
        Leave.onClick.AddListener(LeaveBoard);
    }

    void LeaveBoard()
    {
        SceneManager.LoadScene("SampleScene");
    }

    List<LeaderboardEntry> LoadLeaderboard()
    {
        List<LeaderboardEntry> list = new List<LeaderboardEntry>();

        for (int i = 0; i < 3; i++)
        {
            if (!PlayerPrefs.HasKey("LB_Score_" + i))
                continue;

            list.Add(new LeaderboardEntry
            {
                score = PlayerPrefs.GetInt("LB_Score_" + i),
                lastTime = PlayerPrefs.GetFloat("LB_Time_" + i),
                name = PlayerPrefs.GetString("LB_Name_" + i),
                team = PlayerPrefs.GetString("LB_Team_" + i)
            });
        }

        return list;
    }

    void SaveLeaderboard(List<LeaderboardEntry> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            PlayerPrefs.SetInt("LB_Score_" + i, entries[i].score);
            PlayerPrefs.SetFloat("LB_Time_" + i, entries[i].lastTime);
            PlayerPrefs.SetString("LB_Name_" + i, entries[i].name);
            PlayerPrefs.SetString("LB_Team_" + i, entries[i].team);
        }
    }

    void DisplayLeaderboard(List<LeaderboardEntry> entries)
    {
        TextMeshProUGUI[] slots = { first, second, third};

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].text = i < entries.Count ? entries[i].ToDisplayString() : "";
        }
    }
}
