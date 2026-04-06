using UnityEngine;

public class S_MiniGamePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private S_MiniGameManager gameManager;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        // On supporte le systme d'input classique ou on hardcode ZQSD/WASD si l'input manager est en rade
        float moveX = 0f;
        float moveZ = 0f;
        
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        Vector3 _moveDir = new Vector3(moveX, 0, moveZ).normalized;
        transform.position += _moveDir * speed * Time.deltaTime;

        if (_moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_moveDir), 700f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            if (gameManager != null) gameManager.WinGame();
        }
    }
}
