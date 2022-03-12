using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float _bulletSpeed = 1f;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody.velocity = transform.forward * _bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
