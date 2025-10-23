using UnityEngine;
using UnityEngine.AI;

public class ChallengerController : MonoBehaviour, IInteractable
{
    [Header("Respawn Setup")]
    [SerializeField] private GameObject challengerPrefab;

    [Header("Life Stats")]
    [SerializeField] private int maxLife = 100; 
    private int currentLife;

    // [SerializeField] private float respawnTime = 5f;

    public static int challengerCount = 5;

    public bool isMoving;

    [Header("Player to Follow")]
    [SerializeField] public Transform target;

    private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.stoppingDistance = 1.3f;
        agent.autoBraking = false;

        currentLife = maxLife;
    }

    private void Update()
    {
        if (target == null || agent == null || animator == null) return;

        agent.SetDestination(target.position);

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        Vector2 velocity2D = new Vector2(agent.velocity.x, agent.velocity.y);

        isMoving = velocity2D.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            Vector2 localDirection = velocity2D.normalized;

            animator.SetFloat("moveX", localDirection.x);
            animator.SetFloat("moveY", localDirection.y);
        }
        else
        {
            animator.SetFloat("moveX", 0f);
            animator.SetFloat("moveY", 0f);
        }
    }

    public void Attack() 
    {
        TakeDamage(10);
    }

    public void Interact()
    {
        PlayerController.Instance.TakeDamage(10);
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
        challengerCount -= 1;
        Destroy(gameObject);

        Debug.Log($"Challenger defeated! {challengerCount} remaining.");

        if (challengerCount <= 0)
        {
            GameController.Instance.WinGame();
        }

        /*
        if (challengerPrefab != null && challengerCount == 0)
            GameController.Instance.StartRespawn(challengerPrefab, transform.position, respawnTime);
        */
    }

}
