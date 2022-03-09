using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 速度
    public float speed = 5f;
    public int maxHealth = 5;
    private int _currentHealth;
    public float invincibleTotalFrozonTime = 2;
    private float invincibleTimer;
    private bool isInvincible = false;
    public Transform respawnPos;
    // 刚体
    private Rigidbody2D _rigidbody2D;
    // 目光 初始(0,-1)向下看
    private Vector2 _lookDirection = Vector2.down; 
    // 当前输入的坐标
    private Vector2 _currentInput;
    // 水平输入值
    private float _x;
    // 垂直输入值
    private float _y;
    //绑定的动画
    private Animator _animator;
    public int Health => _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        // 实例化刚体
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // 实例化动画
        _animator = GetComponent<Animator>();
        _currentHealth = 1;
    }

    // Update is called once per frame 每帧工作
    void Update()
    {
        // 获取水平输入轴的数值
        _x = Input.GetAxis("Horizontal");
        // 获取垂直输入轴的数值
        _y = Input.GetAxis("Vertical");
        // x,y轴的移动坐标
        Vector2 movement = new Vector2(_x, _y);
        // x和y和0.0f做比较
        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            _lookDirection.Set(movement.x, movement.y);
            // 所有坐标和大小都在0到1之间
            _lookDirection.Normalize();
        }
        // 设置动画的目光水平坐标
        _animator.SetFloat("lookX", _lookDirection.x);
        // 设置动画的目光垂直坐标
        _animator.SetFloat("lookY", _lookDirection.y);
        // 设置站立到跑动的切换速率
        _animator.SetFloat("speed", movement.magnitude);

        // 赋值给当前输入坐标
        _currentInput = movement;

        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer <= 0) {
                isInvincible = false;
            }
        }
    }

    // 定时工作 unity逻辑，物理操作都是和时间关联的
    // 不同性能的设备，发生的动作速率是不一致的，为了保持一致，所以需要和时间同步
    private void FixedUpdate()
    {
        Vector2 position = _rigidbody2D.position;
        position += _currentInput * speed * Time.deltaTime; // Time.deltaTime：返回自上一帧完成以来经过的时间量
        // 刚体默认处理方式：具有 2D 刚体和 2D 碰撞体的游戏对象在物理更新期间可以重叠或穿过彼此（如果移动得足够快）。仅会在新位置生成碰撞触点
        // 因此这里用内置处理
        // _rigidbody2D.transform.position = position;
        // 在运动刚体由关节连接到其他刚体情况下，切勿使用直接变换访问。
        // 这样做会跳过 PhysX 计算相应刚体的内部速度的步骤，导致解算器提供不良结果。
        // 我们看到过一些 2D 项目使用直接变换访问通过在骨架的根节点上更改 transform.direction 来翻转角色。
        // 如果改用 MovePosition/MoveRotation/Move，那么此行为会改善很多。
        _rigidbody2D.MovePosition(position);
    }


    public void changeHealth(int amount)
    {
        if(amount < 0)
        {
            if(isInvincible) {
                return;
            }

            isInvincible = true;
            invincibleTimer = invincibleTotalFrozonTime;
        }
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        print("Current health: " + _currentHealth);

        if(0 == _currentHealth)
        {
            respawn();
        }
    }

    private void respawn()
    {
        changeHealth(maxHealth);
        _rigidbody2D.position = respawnPos.position;
    }
}
