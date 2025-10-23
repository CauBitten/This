using UnityEngine;

public class WinController : MonoBehaviour
{
    private static GameObject winText;

    private void Start()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas != null)
        {
            winText = canvas.transform.Find("Win")?.gameObject;

            if (winText != null)
                winText.SetActive(false);
        }
    }

    public static void setWin()
    {
        Time.timeScale = 0f;

        winText.SetActive(true);
    }
}
