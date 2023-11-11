using Unity.Netcode;
using UnityEngine;

public class UnitHealthbar : NetworkBehaviour
{
    [SerializeField] private RectTransform healthBar = null;
    public delegate void OnDiedDelegate(Unit unit);
    public event OnDiedDelegate OnDied;
    public NetworkVariable<float> Health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void OnEnable() {
        Health.OnValueChanged += OnHealthChange;
    }

    private void OnDisable() {
        Health.OnValueChanged -= OnHealthChange;
    }

    public override void OnNetworkSpawn() {
        if (IsServer) {
            var unitData = GetComponent<Unit>().unitData;
            Health.Value = unitData.health;
        }
    }

    private void OnHealthChange(float oldHealth, float newHealth) {
        Debug.Log("OnHealthChange " + newHealth);
        var healthBarScript = healthBar.GetComponent<HealthBar>();
        var unit = GetComponent<Unit>();

        healthBarScript.UpdateHealthBar(newHealth, unit.unitData.health);

        if (newHealth <= 0) {
            Debug.Log("Unit died");
            OnDied?.Invoke(unit);
            Destroy(gameObject);
        }
    }
}
