using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Random = System.Random;
using System.Collections;


public class bob : MonoBehaviour
{
    public Random rand = new Random();
    float randomFloat;
    public GameObject assface;
    public GameObject assface2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
{
StartCoroutine(MoveLoop());
}

IEnumerator MoveLoop()
{
    while (true)
    {
        float wait = GenerateRandomFloat(rand, 0f, 1f);
        if (wait<0.5f) {
            assface.SetActive(false);
            assface2.SetActive(true);
        } else {
            assface2.SetActive(false);
            assface.SetActive(true);
        }
        wait*=2;
        yield return new WaitForSecondsRealtime(wait);
    }
}

public static float GenerateRandomFloat(Random random, float minValue, float maxValue)
{
    return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
}

}
