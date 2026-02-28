using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class Tile : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerExitHandler
{
    [HideInInspector] public bool isBomb;
    [HideInInspector] public bool revealed;
    [HideInInspector] public bool flagged;
    [HideInInspector] public int adjacentBombs;
    [HideInInspector] public bool hasCookie;

    Button button;
    TMP_Text numberText;
    MapGenerator map;

    public Sprite open;
    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five;
    public Sprite six;
    public Sprite seven;
    public Sprite eight;
    public Sprite flag;
    public Sprite closed;
    public Sprite cookietile;
    public Sprite bombtile;

    Coroutine longPressRoutine;
    const float longPressTime = 0.2f;

    bool pointerDown;
    bool flagConsumed;
    bool pointerValid;

    void Awake()
    {
        button = GetComponent<Button>();
        numberText = GetComponentInChildren<TMP_Text>();
        map = FindAnyObjectByType<MapGenerator>();

        button.onClick.RemoveAllListeners();
        button.interactable = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (revealed || map.gameOver) return;

        pointerDown = true;
        pointerValid = true;
        flagConsumed = false;

        if (longPressRoutine != null)
            StopCoroutine(longPressRoutine);

        longPressRoutine = StartCoroutine(LongPressCheck());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!pointerDown) return;

        pointerDown = false;

        if (longPressRoutine != null)
        {
            StopCoroutine(longPressRoutine);
            longPressRoutine = null;
        }

        if (!pointerValid) return;

        if (flagConsumed) return;

        Reveal();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerValid = false;
    }

    IEnumerator LongPressCheck()
    {
        yield return new WaitForSeconds(longPressTime);

        if (!pointerDown) yield break;
        if (!pointerValid) yield break;
        if (revealed || map.gameOver) yield break;

        ToggleFlag();

        flagConsumed = true;
        pointerValid = false;
    }

    void ToggleFlag()
    {
        if (revealed || map.gameOver) return;

        if (!flagged)
        {
            map.AddFlag();
        }
        else
        {
            map.bombCounter += 2;
            map.AddFlag();
        }

        flagged = !flagged;
        button.image.sprite = flagged ? flag : closed;
    }

    public void Reveal()
    {
        if (revealed || flagged || map.gameOver) return;

        if (!map.gameStarted)
            map.FirstClick(this);

        if (isBomb)
{
    revealed = true;
    button.image.sprite = bombtile;
    button.image.color = Color.red;
    map.GameOver();
    return;
}

RevealInternal();

        if (adjacentBombs == 0)
            map.FloodReveal(this);
    }

    public void RevealInternal()
    {
        revealed = true;

        button.image.sprite = open;

        map.currentTiles--;
        map.CheckWin();

        if (hasCookie)
        {
            hasCookie = false;
            StartCoroutine(CookieFlash());
            map.CollectCookie();
        }
        else
        {
            ApplyNumber();
        }
    }

    IEnumerator CookieFlash()
    {
        ApplyNumber();
        Image img = button.image;
        Sprite original = img.sprite;

        for (int i = 0; i < 3; i++)
        {
            img.sprite = cookietile;
            yield return new WaitForSeconds(0.15f);
            img.sprite = original;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ApplyNumber()
    {
        if (adjacentBombs > 0)
            button.image.sprite = GetNumberColor(adjacentBombs);
    }

    Sprite GetNumberColor(int number)
    {
        switch (number)
        {
            case 1: return one;
            case 2: return two;
            case 3: return three;
            case 4: return four;
            case 5: return five;
            case 6: return six;
            case 7: return seven;
            case 8: return eight;
            default: return open;
        }
    }
}