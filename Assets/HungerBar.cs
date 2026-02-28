using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public int maxValue = 15;
    public float currentValue = 15;

    public RectTransform rectTransform;
    public Image image;
    public FeedMe feedMe;

    Vector2 originalSize;

    void Start()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (image == null)
            image = GetComponent<Image>();

        originalSize = rectTransform.sizeDelta;
        UpdateBar();
    }

    void Update()
    {
        UpdateBar();
    }

    void UpdateBar()
    {
        currentValue=feedMe.hunger;
        float percent = Mathf.Clamp01((float)currentValue / maxValue);

        rectTransform.sizeDelta = new Vector2(
            originalSize.x * percent,
            originalSize.y
        );

        UpdateColor(percent);
    }

    void UpdateColor(float percent)
    {
        if (image == null) return;

        if (percent > 0.66f)
            image.color = Color.green;
        else if (percent > 0.33f)
            image.color = Color.yellow;
        else
            image.color = Color.red;
    }
}
