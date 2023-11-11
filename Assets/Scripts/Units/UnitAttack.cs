using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class UnitAttack : NetworkBehaviour
{
    [SerializeField] private SoUnit unitData;
    [SerializeField] private float findEnemyInAttackRadiusSpeed = .25f;
    [SerializeField] private Transform bulletSpawnPoint = null;
    private Unit target = null;
    private Coroutine findTargetCoroutine = null;
    private float attackSpeedTimer = 0f;
    private float reloadTimer = 0f;
    private int bulletsShootedBeforeReload = 0;

    private Turret FindTurret() {
        return GetComponentInChildren<Turret>();
    }

    private void FindEnemyInAttackRadius() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, unitData.attackRange);
        
        foreach (Collider collider in colliders) {
            Unit unit = collider.GetComponent<Unit>();
            if (unit == null) continue;

            var colliderOwnerId = collider.GetComponent<NetworkObject>().OwnerClientId;
            if (unit != null && OwnerClientId != colliderOwnerId) {
                target = unit;
                return;
            }
        }
    }

    public void ShootBullet() {
        attackSpeedTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;

        if (unitData.CanShoot && attackSpeedTimer <= 0f && reloadTimer <= 0f) {
            BulletSpawnServerRpc();

            attackSpeedTimer = unitData.attackSpeed;
            bulletsShootedBeforeReload ++;

            if (bulletsShootedBeforeReload >= unitData.ammo) {
                reloadTimer = unitData.attackCooldown;
                bulletsShootedBeforeReload = 0;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void BulletSpawnServerRpc() {
        Debug.Log("RpcPredictionBulletSpawn " + OwnerClientId);
        var bullet = Instantiate(unitData.bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bulletScript = bullet.GetComponentInChildren<Bullet>();

        if (bulletScript == null) return;
        bulletScript.SetBulletStats(unitData);
        bullet.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    private void Attack() {
        if (target == null) return;

        var turret = FindTurret();
        if (turret != null) {
            turret.RotateToTarget(target.transform.position, unitData.rotateSpeed);
            if (turret.IsInFieldOfView(target.transform.position, unitData.flieldOfView)) {
                ShootBullet();
            }
        } else {
            var unit = GetComponent<Unit>();
            unit.RotateToTarget(target.transform.position, unitData.rotateSpeed);
            if (Vector3.Angle(transform.forward, target.transform.position - transform.position) < unitData.flieldOfView * 0.5f) {
                ShootBullet();
            }
        }
    }

    private IEnumerator FindEnemyInAttackRadiusCoroutine() {
        while (true) {
            FindEnemyInAttackRadius();
            yield return new WaitForSeconds(findEnemyInAttackRadiusSpeed);
        }
    }

    private void OnDisable() {
        if (!IsOwner) return;
        StopCoroutine(findTargetCoroutine);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        findTargetCoroutine = StartCoroutine(FindEnemyInAttackRadiusCoroutine());
    }

    void Update()
    {
        if (!IsOwner) return;
        Attack();
    }
}
