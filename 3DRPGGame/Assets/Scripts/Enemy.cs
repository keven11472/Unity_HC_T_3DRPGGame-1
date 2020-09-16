using UnityEngine;
using UnityEngine.AI;       // 引用 人工智慧 API

public class Enemy : MonoBehaviour
{
    #region 欄位
    [Header("移動速度"), Range(0, 100)]
    public float speed = 1.5f;
    [Header("攻擊"), Range(0, 100)]
    public float attack = 20f;
    [Header("血量"), Range(0, 1000)]
    public float hp = 350f;
    [Header("經驗值"), Range(0, 1000)]
    public float exp = 30f;
    [Header("掉落道具的機率"), Range(0f, 1f)]
    public float prop = 0.3f;
    [Header("掉落的道具")]
    public Transform skull;
    [Header("停止距離：攻擊距離"), Range(0, 10)]
    public float rangeAttack = 1.5f;
    [Header("攻擊冷卻時間"), Range(0, 10)]
    public float cd = 3.5f;

    private NavMeshAgent nma;
    private Animator ani;
    private Player player;
    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;
    #endregion

    #region 方法
    /// <summary>
    /// 移動方法：追蹤玩家
    /// </summary>
    private void Move()
    {
        // 代理器.設定目的地(玩家.變形.座標)
        nma.SetDestination(player.transform.position);
        ani.SetFloat("移動", nma.velocity.magnitude);     // nma.velocity.magnitude 加速度.長度

        // 如果 剩下的距離 <= 攻擊範圍 就 攻擊
        if (nma.remainingDistance <= rangeAttack) Attack();
    }

    /// <summary>
    /// 攻擊
    /// </summary>
    private void Attack()
    {
        timer += Time.deltaTime;                // 累加時間

        if (timer >= cd)                        // 如果 計時器 >= 冷卻時間
        {
            timer = 0;                          // 計時器歸零
            ani.SetTrigger("攻擊觸發");          // 攻擊動畫
        }
    }
    #endregion

    #region 事件
    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        player = FindObjectOfType<Player>();

        nma.speed = speed;                      // 更新速度
        nma.stoppingDistance = rangeAttack;     // 更新停止距離
    }

    private void Update()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.8f, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, rangeAttack);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);

        if (other.name == "玩家")
        {
            other.GetComponent<Player>().Hit(attack, transform);
        }
    }
    #endregion
}
