using UnityEngine;

public class Monster : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private float _monsterSpeed = 0.05f;
    [SerializeField] private float _chaseRadius = 2f;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = (_player.transform.position - transform.position).normalized * _monsterSpeed;
    }
}
