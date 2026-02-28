using UnityEngine;
using UnityEngine.UI;
public class FeedMe : MonoBehaviour
{
    [SerializeField] private Button myButton; 
    public float hunger;
    public int belly = 15;
    public bool lose = false;
    public bool ready = false;
    public MapGenerator mapGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hunger = belly;
    }

    private void Awake()
    {
        myButton.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {if(ready==true&&!lose){
        check();
    }
    }

    public void OnButtonClick()
    {
        if(mapGenerator.cookiesCollected>=1&&hunger<=belly*0.8&&!lose){
        mapGenerator.cookiesCollected--;
        mapGenerator.UpdateCookies();
        hunger=belly;
    }}


    void check() {
        if(!lose){
        hunger-=1*Time.deltaTime;
        if (hunger <= 0) {
            Debug.Log("Game Over");
            lose = true;
            mapGenerator.GameOver();
        }
    }}
    }

