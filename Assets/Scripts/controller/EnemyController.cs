using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // 速度
    public float speed = 2f;
    // 转向间隔
    public float timeToChangeDirection = 2f;
    // 判断要横着走还是竖着走
    public bool horizontal;

    // 时间控制器
    private float _remainingTime;
    // 方向
    private Vector2 _direction = Vector2.left;

    private Rigidbody2D _rigidbody2d;
    private Animator _animator;
    private static readonly int lookX = Animator.StringToHash("lookX");
    private static readonly int lookY = Animator.StringToHash("lookY");

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _remainingTime = timeToChangeDirection;

        _direction = horizontal ? Vector2.left : Vector2.down;
    }

    // Update is called once per frame
    void Update()
    {
        _remainingTime -= Time.deltaTime;

        if(_remainingTime <= 0)
        {
            _remainingTime = timeToChangeDirection;
            _direction *= -1;
        }

        _animator.SetFloat(lookX, _direction.x);
        _animator.SetFloat(lookY, _direction.y);
    }

    private void FixedUpdate()
    {
        _rigidbody2d.MovePosition(_rigidbody2d.position + _direction * speed * Time.deltaTime); 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController controller = other.collider.GetComponent<PlayerController>();

        if(controller)
        {
            controller.changeHealth(-1);
        }
    }
}
