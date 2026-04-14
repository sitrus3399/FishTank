using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    void Start()
    {
        
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 screenMousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, Camera.main.nearClipPlane));
            Vector2 finalPos = new Vector2(worldPos.x, worldPos.y);

            Collider2D hit = Physics2D.OverlapPoint(finalPos);

            if (hit != null)
            {
                if (hit.CompareTag("Trash"))
                {
                    Trash trash = hit.gameObject.GetComponent<Trash>();

                    gameEvent.RemoveTrash(trash);
                }
                else if (hit.CompareTag("Fish"))
                {
                    Fish fish = hit.gameObject.GetComponent<Fish>();

                    gameEvent.ScaringFish(fish);
                }
            }
            else
            {
                gameEvent.Feeding(finalPos);
            }
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
}
