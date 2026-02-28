using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ManualKeyboard : MonoBehaviour
{
    public TMP_InputField inputField; // Assign in Inspector
    public TextMeshProUGUI betterText;

    [Header("Letter Buttons")]
    public Button A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z;


    [Header("Special Buttons")]
    public Button submitButton, deleteButton;
    
    [Header("Navigation Settings")]
    public GameObject firstSelectedButton; // First button selected for joystick navigation

    void Start()
    {
        // Assign button actions
        AssignButtonListeners();

        // Ensure controller navigation starts at the first button
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    void AssignButtonListeners()
    {
        // Capital letters
        A.onClick.AddListener(() => AddCharacter('A'));
        B.onClick.AddListener(() => AddCharacter('B'));
        C.onClick.AddListener(() => AddCharacter('C'));
        D.onClick.AddListener(() => AddCharacter('D'));
        E.onClick.AddListener(() => AddCharacter('E'));
        F.onClick.AddListener(() => AddCharacter('F'));
        G.onClick.AddListener(() => AddCharacter('G'));
        H.onClick.AddListener(() => AddCharacter('H'));
        I.onClick.AddListener(() => AddCharacter('I'));
        J.onClick.AddListener(() => AddCharacter('J'));
        K.onClick.AddListener(() => AddCharacter('K'));
        L.onClick.AddListener(() => AddCharacter('L'));
        M.onClick.AddListener(() => AddCharacter('M'));
        N.onClick.AddListener(() => AddCharacter('N'));
        O.onClick.AddListener(() => AddCharacter('O'));
        P.onClick.AddListener(() => AddCharacter('P'));
        Q.onClick.AddListener(() => AddCharacter('Q'));
        R.onClick.AddListener(() => AddCharacter('R'));
        S.onClick.AddListener(() => AddCharacter('S'));
        T.onClick.AddListener(() => AddCharacter('T'));
        U.onClick.AddListener(() => AddCharacter('U'));
        V.onClick.AddListener(() => AddCharacter('V'));
        W.onClick.AddListener(() => AddCharacter('W'));
        X.onClick.AddListener(() => AddCharacter('X'));
        Y.onClick.AddListener(() => AddCharacter('Y'));
        Z.onClick.AddListener(() => AddCharacter('Z'));

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
        if (inputField.text.Length < 9) {
        inputField.text += character;
        betterText.text=inputField.text;
        }
    }

    void SubmitName()
    {
        Debug.Log("Submitted Name: " + inputField.text);
        PlayerPrefs.SetString("currentName", inputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("TeamEntry");
    }
}
