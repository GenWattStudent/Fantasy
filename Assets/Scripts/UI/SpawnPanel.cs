using TMPro;
using UnityEngine;

public class SpawnPanel : MonoBehaviour
{
    public void SetSpawnData(SoUnit soUnit, int unitQueueCount, float currentTime) {
        gameObject.SetActive(true);

        var spawnUnitCountText = GetComponentsInChildren<TextMeshProUGUI>(true)[0];
        var timeText = GetComponentsInChildren<TextMeshProUGUI>(true)[1];
        var progressBar = GetComponentInChildren<HealthBar>(true);
        var timeRounded = Mathf.RoundToInt(currentTime);

        timeText.text = timeRounded.ToString() + "s";
        spawnUnitCountText.text = unitQueueCount.ToString() + "x";
        progressBar.UpdateHealthBar(currentTime, soUnit.spawnTime);
    }
}
