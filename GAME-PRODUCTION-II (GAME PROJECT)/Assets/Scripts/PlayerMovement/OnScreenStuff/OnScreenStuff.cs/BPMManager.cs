using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BPMManager : MonoBehaviour
{
    public static BPMManager Instance { get; private set; } // Singleton instance

    public Image heartIcon; // Assign in Inspector
    public float bpm = 75f; // Default BPM value, I might change it tho
    private float beatInterval;
    private bool isBeating = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    void Start()
    {
        UpdateBeatInterval();
        StartCoroutine(HeartBeatRoutine());
    }

    void UpdateBeatInterval()
    {
        beatInterval = 60f / bpm;
    }

    public void SetBPM(float newBPM)
    {
        bpm = newBPM;
        UpdateBeatInterval();
    }

    IEnumerator HeartBeatRoutine()
    {
        while (isBeating)
        {
            yield return new WaitForSeconds(beatInterval);
            StartCoroutine(HeartPulse());
        }
    }

    IEnumerator HeartPulse()
    {
        float duration = beatInterval / 4f; // Short pulse duration
        // Vector3 originalScale = heartIcon.transform.localScale;
        // Vector3 enlargedScale = originalScale * 1.2f; // Enlarge slightly

        // Scale Up
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            // heartIcon.transform.localScale = Vector3.Lerp(originalScale, enlargedScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Scale Down
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            // heartIcon.transform.localScale = Vector3.Lerp(enlargedScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
