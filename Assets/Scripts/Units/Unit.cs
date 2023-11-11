using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;
    public SoUnit unitData = null;

    public void Select()
    {
        if (!IsOwner) return;
        onSelected?.Invoke();
    }

    public void Deselect()
    {
        if (!IsOwner) return;
        onDeselected?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        var healthBar = GetComponent<UnitHealthbar>();
        if (IsServer) {
            healthBar.Health.Value -= damage;
        }
    }

    public void RotateToTarget(Vector3 targetPosition, float rotateSpeed) {
        Vector3 direction = targetPosition - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void OnTriggerEnter(Collider other) {
        var bullet = other.GetComponentInParent<Bullet>();

        if (bullet != null) {
            var bulletOwnerId = other.GetComponentInParent<NetworkObject>().OwnerClientId;
            if (bulletOwnerId != OwnerClientId) {
                Debug.Log("Bullet hit " + OwnerClientId);
                TakeDamage(bullet.damage);
                if (!IsServer) return;
                bullet.GetComponent<NetworkObject>().Despawn(true);
            }
        }
    }

    private void Update() {
       
    }
}
