using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private float speed = 100f;
    public float damage = 20;
    private float lifeTime = 2f;

    public void SetBulletStats(SoUnit unitData) {
        damage = unitData.damage;
    }

    private void Start() {
        var rigidbody = GetComponentInChildren<Rigidbody>();
        if (rigidbody != null) {
            rigidbody.velocity = transform.forward * speed;
        }
    }

    private void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f && IsServer) {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
