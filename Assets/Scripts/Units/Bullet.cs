using Mirror;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    public int damage = 10;
    private float lifeTime = 2f;
    public int ownerId;

    private void Start() {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void Update() {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            Destroy(gameObject);
        }
    }
}
