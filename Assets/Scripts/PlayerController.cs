using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Respawn Setup")]
    [SerializeField] private GameObject palyerPrefab;

    [Header("Life Stats")]
    [SerializeField] private int maxLife = 100;
    private int currentLife;

    public float moveSpeed;

    public bool isMoving;

    public Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

    private int coins;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        animator = GetComponent<Animator>();
        currentLife = maxLife;
        coins = 0;
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseController.setPaused(!PauseController.IsGamePaused);

        if (!isMoving && !PauseController.IsGamePaused)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0; 

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                Vector3 targetPos = transform.position;
                targetPos.x += input.x * moveSpeed * Time.deltaTime;
                targetPos.y += input.y * moveSpeed * Time.deltaTime;

                if (IsWalkable(targetPos)) 
                    StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetMouseButtonDown(0) && !PauseController.IsGamePaused)
        {
            Attack();
            animator.SetTrigger("Attack");
        }

        if (Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private Collider2D GetCollider()
    {
        Vector3 facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        Vector3 interactPos = transform.position + facingDir;

        return Physics2D.OverlapCircle(interactPos, 0.2f, interactableLayer);
    }

    private void Attack()
    {
        Collider2D collider = GetCollider();

        if (collider != null)
        {
            collider.GetComponent<IInteractable>()?.Attack();
        }
    }

    private void Interact()
    {
        Collider2D collider = GetCollider();

        if (collider != null)
        {
            collider.GetComponent<IInteractable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
            return false;

        return true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentLife <= 0) return;

        currentLife -= damageAmount;
        currentLife = Mathf.Max(currentLife, 0);

        Debug.Log($"Player took {damageAmount} damage. Current life: {currentLife}");


        if (currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("You died!");

        GameController.Instance.EndGame();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"Player Coins: {coins}");
    }

}
