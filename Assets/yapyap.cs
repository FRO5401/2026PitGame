using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class yapyap : MonoBehaviour
{
    public TMP_Text yaptext;

    int tracker = 1;
    int tracker2 = 0;

    bool assfacePlaying = false;
    bool don2 = false;
    bool don = false;
    bool rotatePositive = true;


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
    public float passiveDelay = 8f;

    Coroutine typingCoroutine;
    Coroutine passiveCoroutine;
    int lastPassive = -1;

    void Start()
    {
        PlayStory();
        StartPassiveTimer();
    }

    void OnEnable()
    {
        StartPassiveTimer();
    }

    void PlayStory()
    {
        switch (tracker)
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
        if(!don2){
        tracker2++;

        if (tracker2 == 1)
    Read(cookie);
else if (tracker2 == 2)
    Read(cookie2);
else
    tracker2 = 0;
    don2 = true;
        }
    }

    public void AssfaceRead()
    {
        if (!don) {
        assfacePlaying = true;
        Read(assface);
        don =true;
        }
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

        // 50% chance to rotate
        if (UnityEngine.Random.value <= 0.2f)
        {
            float randomAngle = UnityEngine.Random.Range(0f, 30f);

            if (rotatePositive)
                yapper.transform.rotation = Quaternion.Euler(0f, 0f, randomAngle);
            else
                yapper.transform.rotation = Quaternion.Euler(0f, 0f, -randomAngle);

            rotatePositive = !rotatePositive; // alternate direction
        }

        if (words[i] == '.')
            yield return new WaitForSeconds(0.15f);
        else if (words[i] == ',')
            yield return new WaitForSeconds(0.1f);
        else
            yield return new WaitForSeconds(0.05f);
    }

    typingCoroutine = null;

    yield return new WaitForSeconds(2f);

    // Reset rotation after line finishes
    yapper.transform.rotation = Quaternion.identity;

    if (assfacePlaying)
    {
        assfacePlaying = false;
        StartPassiveTimer();
        yield break;
    }

    if (tracker2 == 0)
    {
        tracker++;
        PlayStory();
    }
    else
    {
        CookieRead();
    }

    StartPassiveTimer();
}


    void StartPassiveTimer()
    {
        StopPassiveTimer();
        passiveCoroutine = StartCoroutine(PassiveDialogue());
    }

    void StopPassiveTimer()
    {
        if (passiveCoroutine != null)
        {
            StopCoroutine(passiveCoroutine);
            passiveCoroutine = null;
        }
    }

    IEnumerator PassiveDialogue()
    {
        yield return new WaitForSeconds(passiveDelay);

        while (typingCoroutine != null)
            yield return null;

        if (tracker2 != 0) yield break;
        if (passiveLines == null || passiveLines.Length == 0) yield break;

        int index;
        do
        {
            index = UnityEngine.Random.Range(0, passiveLines.Length);
        }
        while (index == lastPassive && passiveLines.Length > 1);

        lastPassive = index;
        Read(passiveLines[index]);
    }
}
