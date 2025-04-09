using UnityEngine;

public class LightExposure : MonoBehaviour
{
    public Light[] sceneLights;  // Assign lights in the Inspector
    public float exposureLevel = 0f;
    public float exposureSpeed = 0.5f;  // Speed of exposure change
    public float threshold = 0.1f;      // Minimum intensity to count as "lit"
    public float maxExposure = 1f;

    void Update()
    {
        float totalIntensity = 0f;

        foreach (Light light in sceneLights)
        {
            if (light.enabled)
                totalIntensity += GetLightIntensityAtPoint(light, transform.position);
        }

        bool isInLight = totalIntensity > threshold;

        if (isInLight)
        {
            exposureLevel += exposureSpeed * Time.deltaTime;
        }
        else
        {
            exposureLevel -= exposureSpeed * Time.deltaTime;
        }

        exposureLevel = Mathf.Clamp(exposureLevel, 0f, maxExposure);

        Debug.Log("Exposure Level: " + exposureLevel);
    }

    float GetLightIntensityAtPoint(Light light, Vector3 point)
    {
        Vector3 toLight = point - light.transform.position;

        switch (light.type)
        {
            case LightType.Point:
            case LightType.Spot:
                float distance = toLight.magnitude;
                float attenuation = 1.0f - Mathf.Clamp01(distance / light.range);
                float angleFactor = 1f;

                if (light.type == LightType.Spot)
                {
                    float angle = Vector3.Angle(-light.transform.forward, toLight);
                    if (angle > light.spotAngle / 2f)
                        return 0f;
                    angleFactor = 1.0f - (angle / (light.spotAngle / 2f));
                }

                return light.intensity * attenuation * angleFactor;

            case LightType.Directional:
                return light.intensity;

            default:
                return 0f;
        }
    }
}
