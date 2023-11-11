using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Create Unit")]
public class SoUnit : ScriptableObject
{
    public enum UnitType {
        Worker,
        Tank,
        Soldier,
        Ship,
        Building,
        Plane
    }

    public UnitType unitType;
    public string unitName;
    public float health;
    public float damage;
    public float attackRange;
    public float attackSpeed;
    public float movementSpeed;
    public float sightRange;
    public float attackCooldown;
    public float buildTime;
    public int ammo;
    public float spawnTime;
    public float cost;
    public GameObject prefab;
    public Sprite sprite;
    public GameObject bulletPrefab;
    public float rotateSpeed;
    public float flieldOfView;
    public bool CanShoot { get { return bulletPrefab != null; } }
}
