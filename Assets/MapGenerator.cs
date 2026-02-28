using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    public Canvas canvas;
    public Button buttonPrefab;
    public int gridSize = 5;
    public TMP_Text bombUI;
    public TMP_Text cookieUI;
    public TMP_Text timerText;
    public TMP_Text congrats;
    public TMP_Text congrats2;
    public TMP_Text yap;
    public GameObject cookie;
    public GameObject yapper;
    public Button feeder;
    public GameObject bar;
    public GameObject assface;
    public GameObject assface2;

    float totalSize;

    public bool gameStarted;
    public bool gameOver;

    public int cookiesCollected;
    const int COOKIE_COUNT = 2;

    public FeedMe feedMe;
    public Timer timer;
    public yapyap yappy;

    public int bombCount;
    public int bombCounter;

    public Sprite closed;
    private Tile[,] tiles;
    public Sprite bombtile;

    [HideInInspector] public int currentTiles;
    public float canvasHeight;

    bool assfaceTriggered = false;

    void Start()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        canvasHeight = canvasRect.rect.height;
        totalSize = Mathf.Min(canvasWidth, canvasHeight) * 0.8f;
        Reset();
    }

    void Reset()
    {
        gameOver = false;
        gameStarted = false;

        timer.ready2 = false;
        feedMe.lose = false;
        currentTiles = gridSize * gridSize;
        float chance = Mathf.Pow(1.01f, gridSize + 1) - 1f;
        bombCount = (int)(gridSize * chance * gridSize);
        if (bombCount < 3) bombCount = 3;
        bombCounter = bombCount + 1;
        AddFlag();

        Generate();
        UpdateCookies();
    }

    void Generate()
    {
        gameStarted = false;
        yap.gameObject.SetActive(true);
        yapper.SetActive(true);

        if (gridSize < 8)
        {
            cookieUI.gameObject.SetActive(gridSize >= 6);
            cookie.gameObject.SetActive(gridSize >= 6);
            feeder.gameObject.SetActive(false);
            assface.gameObject.SetActive(false);
            bar.SetActive(false);

            if (gridSize == 6)
                yappy.CookieRead();
        }
        else
        {
            cookieUI.gameObject.SetActive(true);
            cookie.gameObject.SetActive(true);
            feeder.gameObject.SetActive(true);
            assface.gameObject.SetActive(true);
            bar.SetActive(true);

            if (!assfaceTriggered)
            {
                assfaceTriggered = true;
                yappy.AssfaceRead();
            }
        }

        tiles = new Tile[gridSize, gridSize];
        float size = totalSize / gridSize;
        float start = -(gridSize * size) / 2f + size / 2f;

        for (int x = 0; x < gridSize; x++)
        for (int y = 0; y < gridSize; y++)
            SpawnTile(x, y, size, start);

        SetAllTilesInteractable(true);
        yappy.SayPassiveIfIdle();
    }

    void SpawnTile(int x, int y, float size, float start)
    {
        Button b = Instantiate(buttonPrefab, canvas.transform);
        RectTransform r = b.GetComponent<RectTransform>();

        r.anchorMin = r.anchorMax = new Vector2(0.5f, 0.5f);
        r.pivot = new Vector2(0.5f, 0.5f);

        r.anchoredPosition = new Vector2(
            start + x * size,
            (start + y * size)-(float)(canvasHeight*0.1)
        );

        r.sizeDelta = new Vector2(size, size);
        r.localScale = Vector3.one;

        Tile tile = b.GetComponent<Tile>();
        tiles[x, y] = tile;

        b.image.sprite = closed;
    }

    public void FirstClick(Tile safeTile)
    {
        gameStarted = true;
        PlaceBombs(safeTile);
        CountBombs();
        if(gridSize>=6){PlaceCookies();}
        safeTile.Reveal();
        if(gridSize>=8){feedMe.ready=true;}
        timer.ready2=true;
    }

    void PlaceBombs(Tile safeTile)
    {

        List<Tile> candidates = new List<Tile>();

        foreach (Tile tile in tiles)
            candidates.Add(tile);

            candidates.Remove(safeTile);
        foreach (Tile n in GetNeighbors(safeTile))
        candidates.Remove(n);

        for (int i = 0; i < bombCount && candidates.Count > 0; i++)
    {
        int index = Random.Range(0, candidates.Count);
        candidates[index].isBomb = true;
        candidates.RemoveAt(index);
    }
    }

    void PlaceCookies()
    {
        List<Tile> validTiles = new List<Tile>();

        foreach (Tile tile in tiles)
        {
            if (!tile.isBomb)
                validTiles.Add(tile);
        }

        for (int i = 0; i < COOKIE_COUNT && validTiles.Count > 0; i++)
        {
            int index = Random.Range(0, validTiles.Count);
            validTiles[index].hasCookie = true;
            validTiles.RemoveAt(index);
        }
    }

    public void CheckWin()
{
    if (gameOver) return;

    if  (currentTiles!=bombCount) return;

    PlayerWins();
}

    void PlayerWins()
{
    gameOver = true;
    SetAllTilesInteractable(false);

    foreach (Tile tile in tiles)
    {
        if (tile.isBomb)
        {
            Image img = tile.GetComponent<Image>();
            img.sprite = bombtile;
            if (img != null) img.color = Color.green;
        }
    }

    feedMe.ready = false;
    timer.ready2 = false;
    StartCoroutine(WaitAndPrint());
}


    IEnumerator<object> WaitAndPrint()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);
        ClearBoard();

        gridSize++;
        
        Reset();
        if(gridSize>=8){
        feedMe.ready=true;
        timer.ready2=true;
        }
    }

    IEnumerator<object> Losing()
    {
        yield return new WaitForSeconds(3f);
        ClearBoard();
        cookieUI.gameObject.SetActive(false);
            cookie.gameObject.SetActive(false);
            feeder.gameObject.SetActive(false);
            bar.SetActive(false);
            assface.gameObject.SetActive(false);
            assface2.gameObject.SetActive(false);
            bombUI.gameObject.SetActive(false);
            yap.gameObject.SetActive(false);
            yapper.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            congrats.text=("Congrats On Making It To Board "+(gridSize-4)+"!");
            congrats2.text=("With a Time Of "+timer.timerText.text+" No Less");
            congrats.gameObject.SetActive(true);
            congrats2.gameObject.SetActive(true);

            StartCoroutine(Losing2());

        }
    IEnumerator<object> Losing2()
    {
        yield return new WaitForSeconds(3f);
        PlayerPrefs.SetInt("LastScore", gridSize-4);
        PlayerPrefs.SetFloat("LastTime", timer.time);
        PlayerPrefs.Save();
        if (gridSize-4<=PlayerPrefs.GetInt("LB_Score_3", 0)){
        if (gridSize-4==PlayerPrefs.GetInt("LB_Score_3", 0)&&timer.time<PlayerPrefs.GetInt("LB_Time_3", 0)) {
        gridSize=5;
        SceneManager.LoadScene("NameEntry");
        }else {
        gridSize=5;
        PlayerPrefs.SetInt("LastScore", 0);
        PlayerPrefs.SetFloat("LastTime", 0);
        SceneManager.LoadScene("SampleScene");
        }
        }else {
        gridSize=5;
        SceneManager.LoadScene("NameEntry");
        }

    }

    void SetAllTilesInteractable(bool state)
{
    if (tiles == null) return;

    foreach (Tile tile in tiles)
    {
        if (tile == null) continue;

        Button b = tile.GetComponent<Button>();
        if (b != null)
            b.interactable = state;
    }
}

    void ClearBoard()
{
    if (tiles == null) return;

    foreach (Tile tile in tiles)
    {
        if (tile != null)
            Destroy(tile.gameObject);
    }

    tiles = null;
}



    void CountBombs()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y].isBomb) continue;

                int count = 0;
                foreach (Tile n in GetNeighbors(x, y))
                {
                    if (n.isBomb) count++;
                }
                tiles[x, y].adjacentBombs = count;
            }
        }
    }

    public void FloodReveal(Tile start)
    {
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            Tile current = queue.Dequeue();

            foreach (Tile n in GetNeighbors(current))
            {
                if (n.revealed || n.flagged || n.isBomb) continue;

                n.RevealInternal();

                if (n.adjacentBombs == 0)
                    queue.Enqueue(n);
            }
        }
    }

    List<Tile> GetNeighbors(Tile tile)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (tiles[x, y] == tile)
                    return GetNeighbors(x, y);
            }
        }
        return new List<Tile>();
    }

    List<Tile> GetNeighbors(int x, int y)
    {
        List<Tile> neighbors = new List<Tile>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize)
                    neighbors.Add(tiles[nx, ny]);
            }
        }
        return neighbors;
    }

    public void CollectCookie()
    {
        cookiesCollected++;
        UpdateCookies();
    }

    public void AddFlag() {
        bombCounter--;
        bombUI.text = ("Unflagged: "+bombCounter);
    }

    public void UpdateCookies(){
        cookieUI.text=cookiesCollected.ToString();
    }

    public void GameOver()
{
    if (gameOver) return;

    gameOver = true;
    SetAllTilesInteractable(false);
    timer.ready2 = false;

    if (cookiesCollected < 2 || feedMe.lose)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.isBomb)
            {
                Image img = tile.GetComponent<Image>();
                img.sprite = bombtile;
                if (img != null) img.color = Color.red;
            }
        }

        feedMe.lose = true;
        StartCoroutine(Losing());
    }
    else
    {
        cookiesCollected -= 2;
        gridSize--;

        foreach (Tile tile in tiles)
        {
            if (tile.isBomb)
            {
                Image img = tile.GetComponent<Image>();
                img.sprite = bombtile;
                if (img != null) img.color = Color.yellow;
            }
        }

        feedMe.ready = false;
        StartCoroutine(WaitAndPrint());
    }
}

}
