using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private AudioClip interactSound;  // Sound that plays when interacted with
    [SerializeField] private AudioSource audioSource;  // AudioSource for the interaction sound

    void Start()
    {
        // Assign an AudioSource if one is not already attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Interact()
    {
        // Play the interaction sound when the player interacts
        if (interactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(interactSound);
        }
        else
        {
            Debug.LogWarning("Interact sound or AudioSource is missing!");
        }

        // Additional interaction logic can go here (e.g., showing a UI, triggering animations, etc.)
        Debug.Log("Interacted with " + gameObject.name);
    }
}
