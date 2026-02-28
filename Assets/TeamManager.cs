using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NewKeyboard : MonoBehaviour
{
    public TMP_InputField inputField; // Assign in Inspector
    public TextMeshProUGUI betterText;


    [Header("Number Buttons")]
    public Button _0, _1, _2, _3, _4, _5, _6, _7, _8, _9;

    [Header("Special Buttons")]
    public Button submitButton;
    public Button deleteButton;
    
    void Start()
    {
        // Assign button actions
        AssignButtonListeners();

        // Ensure controller navigation starts at the first button
    }

    void AssignButtonListeners()
    {
        // Numbers
        _0.onClick.AddListener(() => AddCharacter('0'));
        _1.onClick.AddListener(() => AddCharacter('1'));
        _2.onClick.AddListener(() => AddCharacter('2'));
        _3.onClick.AddListener(() => AddCharacter('3'));
        _4.onClick.AddListener(() => AddCharacter('4'));
        _5.onClick.AddListener(() => AddCharacter('5'));
        _6.onClick.AddListener(() => AddCharacter('6'));
        _7.onClick.AddListener(() => AddCharacter('7'));
        _8.onClick.AddListener(() => AddCharacter('8'));
        _9.onClick.AddListener(() => AddCharacter('9'));

        // Special buttons
        submitButton.onClick.AddListener(SubmitName);
        deleteButton.onClick.AddListener(delete);
    }
    void delete() {
            if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
            betterText.text=inputField.text;
        }
        }

    void AddCharacter(char character)
    {
        if (inputField.text.Length<=7){
        inputField.text += character;
        betterText.text=inputField.text;
    }}

    void SubmitName()
    {
        Debug.Log("Submitted Name: " + inputField.text);
        PlayerPrefs.SetString("currentTeam", inputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LBoard");
    }
}
