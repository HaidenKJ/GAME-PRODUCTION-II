using UnityEngine;

public class SpinAndInteract : MonoBehaviour
{
    [Header("Spinning Settings")]
    [SerializeField] private float rotationSpeed = 50f;  // Adjust this to control how fast the cube spins

    [Header("Interaction Settings")]
    [SerializeField] private AudioClip interactSound;  // The sound to play on interaction
    [SerializeField] private AudioSource audioSource;  // The AudioSource to play the sound

    void Start()
    {
        if (audioSource == null) 
        {
            audioSource = GetComponent<AudioSource>();  // Automatically assign if not already assigned
        }
    }

    void Update()
    {
        // Spin the cube continuously in all directions
        //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);  // Rotation around y-axis
        //transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);  // Rotation around x-axis
        //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);  // Rotation around z-axis

        // Detect interaction (press "F" key)
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        PlayInteractionSound();
    }

    void PlayInteractionSound()
    {
        if (interactSound != null)
        {
            audioSource.PlayOneShot(interactSound);  // Play the sound effect
        }
        else
        {
            Debug.LogWarning("Interact sound not assigned!");
        }
    }
}
