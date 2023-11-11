using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private DayNightSO dayNightSO = null;
    [SerializeField] private Light directionalLight = null;
    [SerializeField, Range(0, 24)] private float timeOfDay = 0;

    private void OnValidate() {
        if (directionalLight != null) return;
        if (RenderSettings.sun != null) {
            directionalLight = RenderSettings.sun;
        } else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights) {
                if (light.type == LightType.Directional) {
                    directionalLight = light;
                    return;
                }
            }
        }
    }

    private void UpdateLighting(float timePercent) {
        RenderSettings.ambientLight = dayNightSO.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = dayNightSO.fogColor.Evaluate(timePercent);

        if (directionalLight != null) return;

        directionalLight.color = dayNightSO.directionalColor.Evaluate(timePercent);
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0f));
    }

    private void Update() {
        if (Application.isPlaying) {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        } else {
            UpdateLighting(timeOfDay / 24f);
        }
    }
}
