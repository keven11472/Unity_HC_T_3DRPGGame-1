using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 欄位：基本資料
    [Header("移動速度"), Range(0, 1000)]
    public float speed = 5;
    [Header("旋轉速度"), Range(0, 1000)]
    public float turn = 5;
    [Header("攻擊力"), Range(0, 500)]
    public float attack = 20;
    [Header("血量"), Range(0, 500)]
    public float hp = 250;
    [Header("魔力"), Range(0, 500)]
    public float mp = 50;
    [Header("吃道具音效")]
    public AudioClip soundProp;
    [Header("任務數量")]
    public Text textMission;
    

    private int count;
    

    public float exp;
    public int lv = 1;

    // 在屬性 (Inspector) 面板上隱藏
    [HideInInspector]
    /// <summary>
    /// 是否停止
    /// </summary>
    public bool stop;

    private Animator ani;
    private Rigidbody rig;
    private AudioSource aud;
    private NPC npc;
    /// <summary>
    /// 攝影機根物件
    /// </summary>
    private Transform cam;
    #endregion

    [Header("吧條")]
    public Image barHp;
    public Image barMp;
    public Image barExp;

    private float maxHp;
    private float maxMp;
    private float maxExp;

    #region 方法：功能
    private void Move()
    {
        float h = Input.GetAxis("Horizontal");      // A D & 左 右：A 左 -1，D 右 1，沒按 0
        float v = Input.GetAxis("Vertical");        // S W & 下 上：S 下 -1，W 上 1，沒按 0

        // Vector3 pos = new Vector3(h, 0, v);                          // 要移動的座標 - 世界座標版本
        // Vector3 pos = transform.forward * v + transform.right * h;   // 要移動的座標 - 區域座標版本
        Vector3 pos = cam.forward * v + cam.right * h;                  // 要移動的座標 - 攝影機的區域座標版本

        // 剛體.移動作標(本身的座標 + 要移動的座標 * 速度 * 1/50)
        rig.MovePosition(transform.position + pos * speed * Time.fixedDeltaTime);
        // 動畫.設定浮點數("參數名稱"，前後 + 左右 的 絕對值)
        ani.SetFloat("移動", Mathf.Abs(v) + Mathf.Abs(h));

        // 如果有控制再轉向：避免沒移動時轉回原點
        if (h != 0 || v != 0)
        {
            pos.y = 0;                                                                                      // 鎖定 Y 軸 避免吃土
            Quaternion angle = Quaternion.LookRotation(pos);                                                // 將前往的座標資訊轉為角度
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, turn * Time.fixedDeltaTime);   // 角度插值
        }
    }

    private void EatProp()
    {
        count++;                                                                // 遞增
        textMission.text = "骷髏頭：" + count + " / " + npc.data.count;          // 更新介面

        // 如果 數量 等於 NPC 需求數量 就 呼叫 NPC 結束任務
        if (count == npc.data.count) npc.Finish();
    }

    /// <summary>
    /// 受傷
    /// </summary>
    /// <param name="damage">傷害值</param>
    /// <param name="direction">擊退方向</param>
    public void Hit(float damage, Transform direction)
    {
        hp -= damage;
        rig.AddForce(direction.forward * 100 + direction.up * 150);

        barHp.fillAmount = hp / maxHp;
        ani.SetTrigger("受傷觸發");
    }
    #endregion

    #region 事件：入口
    /// <summary>
    /// 喚醒：會在 Start 前執行一次
    /// </summary>
    private void Awake()
    {
        // 取得元件<泛型>() - 泛型 任何類型
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();

        cam = GameObject.Find("攝影機根物件").transform;     // 遊戲物件.尋找("物件名稱") - 建議不要在 Update 內使用

        npc = FindObjectOfType<NPC>();                      // 取得 NPC

        maxHp = hp;
        maxMp = mp;
    }

    /// <summary>
    /// 固定更新：固定 50 FPS
    /// 有物理運動在這裡呼叫 Rigidbody
    /// </summary>
    private void FixedUpdate()
    {
        if (stop) return;           // 如果 停止 就跳出

        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "骷髏頭")
        {
            aud.PlayOneShot(soundProp);             // 播放音效
            Destroy(collision.gameObject);          // 刪除道具
            EatProp();                              // 呼叫吃道具
        }
    }
    #endregion
}
