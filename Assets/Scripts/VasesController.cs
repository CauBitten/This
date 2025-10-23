using UnityEngine;

public class VasesController : MonoBehaviour, IInteractable
{
    public void Attack()
    {
        return;
    }

    public void Interact()
    {
        int coins = Random.Range(1, 7);
        int time = Random.Range(6, 11);

        Debug.Log($"You broke the vases and found {coins} coins!");
        Debug.Log($"Time gained {time}s");

        PlayerController.Instance.AddCoins(coins);
        GameController.Instance.AddTime(time);

        Destroy(gameObject);
    }
}
