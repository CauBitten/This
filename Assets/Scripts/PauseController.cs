using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool IsGamePaused { get; private set; } = false;

    private static GameObject pauseText;

    private void Start()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas != null)
        {
            pauseText = canvas.transform.Find("PauseBackground")?.gameObject;

            if (pauseText != null)
                pauseText.SetActive(false);
        }
    }

    public static void setPaused(bool paused)
    {
        IsGamePaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        if (paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            PlayerController.Instance.enabled = false;

            if (pauseText != null)
                pauseText.SetActive(paused);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (pauseText != null)
                pauseText.SetActive(paused);
        }
    }
}
