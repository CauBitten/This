using System.Collections;
using UnityEngine;

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
    }

    private void Update()
    {
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

    public void StartRespawn(GameObject prefabToRespawn, Vector3 spawnPosition, float delay)
    {
        StartCoroutine(PerformRespawn(prefabToRespawn, spawnPosition, delay));
    }

    private IEnumerator PerformRespawn(GameObject prefab, Vector3 position, float delay)
    {
        if (prefab == null)
        {
            Debug.LogError("Tentativa de respawn falhou: o prefab é nulo.");
            yield break; 
        }

        Debug.Log($"Waiting {delay} seconds to respawn {prefab.name}.");

        yield return new WaitForSeconds(delay);

        Instantiate(prefab, position, Quaternion.identity);

        Debug.Log($"{prefab.name} respawned!");
    }

    public void StartPlayerRespawn(GameObject prefabToRespawn, Vector3 spawnPosition, float delay)
    {
        StartCoroutine(PerformPlayerRespawn(prefabToRespawn, spawnPosition, delay));
    }

    private IEnumerator PerformPlayerRespawn(GameObject prefab, Vector3 position, float delay)
    {
        if (prefab == null)
        {
            Debug.LogError("Tentativa de respawn falhou: o prefab é nulo.");
            yield break;
        }

        Debug.Log($"Waiting {delay} seconds to respawn {prefab.name}.");

        yield return new WaitForSeconds(delay);

        GameObject newPlayerObject = Instantiate(prefab, position, Quaternion.identity);
        PlayerController newPlayer = newPlayerObject.GetComponent<PlayerController>();

        CurrentPlayer = newPlayer;

        newPlayer.ResetState();

        Debug.Log($"{prefab.name} respawned!");
    }
}
