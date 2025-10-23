using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    FreeRoam,
    Dialogue
}

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerPrefabReference; 
    public PlayerController CurrentPlayer { get; private set; }

    GameState state;

    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 50f;
    [SerializeField] private TMP_Text timerText; 
    public bool timerIsRunning = true;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () => state = GameState.Dialogue;

        DialogManager.Instance.OnHideDialog += () =>
        {
            if (state == GameState.Dialogue)
                state = GameState.FreeRoam;
        };

        CurrentPlayer = FindFirstObjectByType<PlayerController>();

        timerIsRunning = true;
        UpdateTimerDisplay(timeRemaining);
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; 
                UpdateTimerDisplay(timeRemaining); 
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;

                EndGame();
            }
        }

        if (state == GameState.FreeRoam)
        {
            if (CurrentPlayer != null)
            {
                CurrentPlayer.HandleUpdate();
            }
        }
        else if (state == GameState.Dialogue)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }

    public void UpdateTimerDisplay(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        string timeFormat = string.Format("{0:F0}", timeToDisplay);

        timerText.text = timeFormat;
    }

    public void AddTime(int amount)
    {
        timeRemaining += amount;
    }

    public void StartRespawn(GameObject prefabToRespawn, Vector3 spawnPosition, float delay)
    {
        StartCoroutine(PerformRespawn(prefabToRespawn, spawnPosition, delay));
    }

    private IEnumerator PerformRespawn(GameObject prefab, Vector3 position, float delay)
    {
        if (prefab == null)
        {
            Debug.LogError("Respawn try failed.");
            yield break; 
        }

        Debug.Log($"Waiting {delay} seconds to respawn {prefab.name}.");

        yield return new WaitForSeconds(delay);

        Instantiate(prefab, position, Quaternion.identity);

        Debug.Log($"{prefab.name} respawned!");
    }

    public void EndGame()
    {
        timeRemaining = 0;
        timerIsRunning = false;

        SceneManager.LoadScene("GameOverScene");
    }

    public void WinGame()
    {
        timerText.gameObject.SetActive(false);
        WinController.setWin();
    }

}
