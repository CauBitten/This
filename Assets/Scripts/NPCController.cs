using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [SerializeField] Dialog dialog;

    public void Interact()
    {
        Debug.Log("Interagindo com NPC");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

    public void Attack()
    {
        return;
    }
}
