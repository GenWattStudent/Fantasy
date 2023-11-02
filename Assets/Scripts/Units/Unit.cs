using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselected = null;
    [SerializeField] private SoUnit unitData = null;
    [SerializeField] private Transform bulletSpawnPoint = null;
    [SerializeField] private RectTransform healthBar = null;
    public delegate void OnDiedDelegate(Unit unit);
    public event OnDiedDelegate OnDied;
    private float attackSpeedTimer = 0f;
    private float reloadTimer = 0f;
    [SyncVar] private int bulletsShootedBeforeReload = 0;
    [SyncVar(hook = "OnHealthChange")] public float Health;
    public int ownerId;

    private void Start() {
        ownerId = GetComponent<NetworkIdentity>().connectionToClient.connectionId;
        Health = unitData.health;
    }

    [Client]
    public void Select()
    {
        if (!isOwned) return;
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!isOwned) return;
        onDeselected?.Invoke();
    }

    public void ShootBullet() {
        attackSpeedTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && unitData.CanShoot && attackSpeedTimer <= 0f && reloadTimer <= 0f) {
            Debug.Log("ShootBullet " + ownerId);
            CmdShootBullet();
            attackSpeedTimer = unitData.attackSpeed;
            bulletsShootedBeforeReload++;

            if (bulletsShootedBeforeReload >= unitData.ammo) {
                reloadTimer = unitData.attackCooldown;
                bulletsShootedBeforeReload = 0;
            }
        }
    }

    [Command]
    private void CmdShootBullet() {
        RpcPredictionBulletSpawn();
    }

    [ClientRpc]
    private void RpcPredictionBulletSpawn() {
        Debug.Log("RpcPredictionBulletSpawn");
        // calculate ticks to bullet spawn
        // var ticksToBulletSpawn = (int)(NetworkTime.time - unitData.lastShootTime) / 1000;
        var bullet = Instantiate(unitData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bulletScript = bullet.GetComponentInChildren<Bullet>();
        Debug.Log(bulletScript);
        if (bulletScript == null) return;
        bulletScript.ownerId = ownerId;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (unitData.health <= 0) {
            OnDied?.Invoke(this);
            NetworkServer.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!isServer) return;

        var bullet = other.GetComponent<Bullet>();
        // Debug.Log(bullet.ownerId + " " + ownerId);
        if (bullet != null && bullet.ownerId != ownerId) {
            Debug.Log("OnTriggerEnter");
            TakeDamage(bullet.damage);

            NetworkServer.Destroy(other.gameObject);
        }
    }

    private void OnHealthChange(float oldHealth, float newHealth) {
        var healthBarScript = healthBar.GetComponent<HealthBar>();
        healthBarScript.UpdateHealthBar(newHealth, unitData.health);
    }

    private void Update() {
        if (!isOwned) return;

        ShootBullet();
    }
}
