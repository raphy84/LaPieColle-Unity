using UnityEngine;

public class S_TreeClimberProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifetime = 5f;
    private S_TreeClimberManager manager;

    public void Initialize(Vector3 dir, float s, S_TreeClimberManager mgr)
    {
        direction = dir.normalized;
        speed = s;
        manager = mgr;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (manager != null) manager.LoseGame();
            Destroy(gameObject);
        }
    }
}
