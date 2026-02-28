using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
public TMP_Text timerText;
public float time;
public bool ready2;
void Update()
{
    if (ready2){
    time += Time.deltaTime;

    int minutes = (int)(time / 60f);
    int seconds = (int)(time % 60f);

    timerText.text = $"{minutes:00}:{seconds:00}";
}}}