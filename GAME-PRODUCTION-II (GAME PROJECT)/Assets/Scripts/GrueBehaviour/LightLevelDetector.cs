using UnityEngine;

public class LightLevelDetector : MonoBehaviour
{
    public Light[] sceneLights; // Assign your scene lights in the Inspector
    public float detectedIntensity = 0f;

    void Update()
    {
        detectedIntensity = 0f;

        foreach (Light light in sceneLights)
        {
            if (light.enabled)
            {
                float intensity = GetLightIntensityAtPoint(light, transform.position);
                detectedIntensity += intensity;
            }
        }

        Debug.Log("Grue Detected Light Intensity of " + detectedIntensity);
    }

    float GetLightIntensityAtPoint(Light light, Vector3 point)
    {
        float intensity = 0f;
        Vector3 toLight = point - light.transform.position;

        switch (light.type)
        {
            case LightType.Point:
            case LightType.Spot:
                float distance = toLight.magnitude;
                float attenuation = 1.0f - Mathf.Clamp01(distance / light.range);
                intensity = light.intensity * attenuation;

                if (light.type == LightType.Spot)
                {
                    float angle = Vector3.Angle(-light.transform.forward, toLight);
                    if (angle > light.spotAngle / 2f)
                        intensity = 0f;
                }
                break;

            case LightType.Directional:
                // No attenuation for directional light
                intensity = light.intensity;
                break;
        }

        return intensity;
    }
}
