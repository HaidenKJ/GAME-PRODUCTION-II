using UnityEngine;

public class LightLevelDetector : MonoBehaviour
{
    public Light[] sceneLights;         // Assign lights in Inspector
    public float exposureLevel { get; private set; }
    public float exposureSpeed = 10f;
    public float maxExposure = 100f;
    public LayerMask obstructionMask;   // What counts as "blocking" the light
    public BPMManager BPMM;

    // void Start()
    // {
    //     exposureLevel = 100f;
    // }

    void Update()
    {
        float totalLight = 0f;

        foreach (Light light in sceneLights)
        {
            if (light.enabled && IsLitByLight(light))
            {
                totalLight += GetLightIntensityAtPoint(light, transform.position);
            }
        }

        // Update exposure
        if (totalLight > 0f)
        {
            exposureLevel += exposureSpeed * Time.deltaTime;
        }
        else
        {
            exposureLevel -= exposureSpeed * Time.deltaTime;
        }

        exposureLevel = Mathf.Clamp(exposureLevel, 0f, maxExposure);
        Debug.Log("Exposure (Raycast): " + exposureLevel);

        GrueBehaviour();
    }

    bool IsLitByLight(Light light)
    {
        Vector3 directionToLight;

        if (light.type == LightType.Directional)
        {
            directionToLight = -light.transform.forward;
        }
        else
        {
            directionToLight = (light.transform.position - transform.position).normalized;
        }

        // Raycast from object toward light
        Ray ray = new Ray(transform.position, directionToLight);
        float maxDistance = (light.type == LightType.Directional) ? Mathf.Infinity : Vector3.Distance(transform.position, light.transform.position);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, obstructionMask))
        {
            // Hit something before reaching the light — it's blocked
            return false;
        }

        return true;
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
                    if (angle > light.spotAngle / 2f) return 0f;
                    angleFactor = 1.0f - (angle / (light.spotAngle / 2f));
                }

                return light.intensity * attenuation * angleFactor;

            case LightType.Directional:
                return light.intensity;

            default:
                return 0f;
        }
    }

    public void GrueBehaviour()
    {

    }
}
