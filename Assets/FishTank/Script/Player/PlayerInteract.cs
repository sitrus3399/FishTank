using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    [SerializeField] private ParticleSystem bubbleBlast;
    [SerializeField] private List<ParticleSystem> bubbleBlastList;
    [SerializeField] private ParticleSystem bigBubbleBlast;
    [SerializeField] private List<ParticleSystem> bigBubbleBlastList;

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

                    ParticleSystem bubble = GotBigBubbleBlast();
                    bubble.gameObject.SetActive(true);
                    bubble.gameObject.transform.position = finalPos;

                    AudioManager.Instance.PlaySFX("Bubble");

                    gameEvent.RemoveTrash(trash);
                }
                else if (hit.CompareTag("Fish"))
                {
                    Fish fish = hit.gameObject.GetComponent<Fish>();

                    ParticleSystem bubble = GotBigBubbleBlast();
                    bubble.gameObject.SetActive(true);
                    bubble.gameObject.transform.position = finalPos;

                    AudioManager.Instance.PlaySFX("Bubble");

                    gameEvent.ScaringFish(fish);
                }
            }
            else
            {
                ParticleSystem bubble = GotBubbleBlast();
                bubble.gameObject.SetActive(true);
                bubble.gameObject.transform.position = finalPos;

                AudioManager.Instance.PlaySFX("Bubble");

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

    private ParticleSystem GotBubbleBlast()
    {
        foreach (ParticleSystem particle in bubbleBlastList)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                return particle;
            }
        }

        return CreateBubbleBlast();
    }

    public ParticleSystem CreateBubbleBlast()
    {
        ParticleSystem newParticle = Instantiate(bubbleBlast);
        newParticle.gameObject.SetActive(false);
        bubbleBlastList.Add(newParticle);
        return newParticle;
    }

    private ParticleSystem GotBigBubbleBlast()
    {
        foreach (ParticleSystem particle in bigBubbleBlastList)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                return particle;
            }
        }

        return CreateBigBubbleBlast();
    }

    public ParticleSystem CreateBigBubbleBlast()
    {
        ParticleSystem newParticle = Instantiate(bigBubbleBlast);
        newParticle.gameObject.SetActive(false);
        bigBubbleBlastList.Add(newParticle);
        return newParticle;
    }
}