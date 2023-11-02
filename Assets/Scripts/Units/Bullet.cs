using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    public int damage = 10;
    private float lifeTime = 2f;
    public int ownerId;

    private void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update() {
        Destroy(gameObject, lifeTime);
    }
}
