using System.Collections;
using UnityEngine;

public class ChallengerController : MonoBehaviour, IInteractable
{
    [Header("Respawn Setup")]
    [SerializeField] private GameObject challengerPrefab;

    [Header("Life Stats")]
    [SerializeField] private int maxLife = 100; 
    private int currentLife;

    [SerializeField] private float respawnTime = 5f; 

    private void Awake()
    {
        currentLife = maxLife;
    }

    public void Attack() 
    {
        TakeDamage(10);
    }

    public void Interact()
    {
        PlayerController.Instance.TakeDamage(100);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentLife <= 0) return; 

        currentLife -= damageAmount;
        currentLife = Mathf.Max(currentLife, 0);

        Debug.Log($"Challenger took {damageAmount} damage. Current life: {currentLife}");


        if (currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Challenger defeated!");

        GameController.Instance.StartRespawn(challengerPrefab, transform.position, respawnTime);

        Destroy(gameObject);
    }

}
