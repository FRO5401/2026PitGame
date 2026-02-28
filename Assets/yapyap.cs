using UnityEngine;
using System.Collections;
using TMPro;

public class yapyap : MonoBehaviour
{
    public TMP_Text yaptext;
    public GameObject yapper;

    public string dialogue1 = "Ah, so you finally made it!";
    public string dialogue2 = "Welcome to my archeologic exhibit!";
    public string dialogue3 = "While I was doing some digging I found some fossils called \"games\".";
    public string dialogue4 = "While you're here, do you think you could test it for me?";
    public string dialogue5 = "Perfect! Now then, putting these together...";

    public string cookie = "Wait I think I found something! Let me just... Tada!";
    public string cookie2 = "Now for the price of 2 cookies, you can revive, pretty cool right?";

    public string assface = "Woah check this out! I found a little egg creature, although he seems pretty hungry...";

    public string[] passiveLines;

    int storyIndex = 0;
    int cookieIndex = 0;
    int lastPassive = -1;

    bool assfacePlaying = false;
    bool rotatePositive = true;

    Coroutine typingCoroutine;

    enum Mode
    {
        Story,
        Cookie,
        Assface,
        Passive
    }

    Mode currentMode = Mode.Story;

    void Start()
    {
        PlayStory();
    }

    void OnDisable()
    {
        StopAllCoroutines();
        typingCoroutine = null;
    }

    void PlayStory()
    {
        currentMode = Mode.Story;
        storyIndex++;

        switch (storyIndex)
        {
            case 1: Read(dialogue1); break;
            case 2: Read(dialogue2); break;
            case 3: Read(dialogue3); break;
            case 4: Read(dialogue4); break;
            case 5: Read(dialogue5); break;
        }
    }

    public void CookieRead()
    {
        currentMode = Mode.Cookie;
        cookieIndex++;

        if (cookieIndex == 1)
        {
            Read(cookie);
        }
        else if (cookieIndex == 2)
        {
            Read(cookie2);
        }
        else
        {
            cookieIndex = 0;
            currentMode = Mode.Story;
        }
    }

    public void AssfaceRead()
    {
        if (assfacePlaying) return;

        assfacePlaying = true;
        currentMode = Mode.Assface;
        Read(assface);
    }

    public void SayPassiveIfIdle()
    {
        if (typingCoroutine != null) return;
        if (passiveLines == null || passiveLines.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, passiveLines.Length);
        }
        while (index == lastPassive && passiveLines.Length > 1);

        lastPassive = index;
        currentMode = Mode.Passive;
        Read(passiveLines[index]);
    }

    public void Read(string words)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(words));
    }

    IEnumerator TypeLine(string words)
    {
        yaptext.text = "";

        for (int i = 0; i < words.Length; i++)
        {
            yaptext.text += words[i];

            if (Random.value <= 0.2f)
            {
                float angle = Random.Range(0f, 30f);
                yapper.transform.rotation = Quaternion.Euler(
                    0f,
                    0f,
                    rotatePositive ? angle : -angle
                );
                rotatePositive = !rotatePositive;
            }

            if (words[i] == '.')
                yield return new WaitForSeconds(0.1f);
            else if (words[i] == ',')
                yield return new WaitForSeconds(0.66f);
            else
                yield return new WaitForSeconds(0.033f);
        }

        yapper.transform.rotation = Quaternion.identity;
        typingCoroutine = null;

        yield return new WaitForSeconds(1.5f);

        switch (currentMode)
        {
            case Mode.Assface:
                assfacePlaying = false;
                currentMode = Mode.Story;
                break;

            case Mode.Cookie:
                CookieRead();
                break;

            case Mode.Story:
                if (storyIndex < 5)
                    PlayStory();
                break;

            case Mode.Passive:
                currentMode = Mode.Story;
                break;
        }
    }
}