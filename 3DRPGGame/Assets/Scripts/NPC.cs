using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話資訊")]
    public GameObject panel;
    [Header("對話名稱")]
    public Text textName;
    [Header("對話內容")]
    public Text textContent;

    private AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 打字效果
    /// </summary>
    private IEnumerator Type()
    {
        textContent.text = "";                                  // 對話內容清空

        string dialog = data.dialogs[0];                        // 取得要顯示的對話

        for (int i = 0; i < dialog.Length; i++)                 // 迴圈執行對話每個字
        {
            textContent.text += dialog[i];                      // 遞增對話內容
            aud.PlayOneShot(data.soundType, 0.5f);              // 播放打字音效
            yield return new WaitForSeconds(data.speed);        // 等待
        }
    }

    private void NoMission()
    {

    }

    private void Missioning()
    {

    }

    private void Finish()
    {

    }

    /// <summary>
    /// 對話開始
    /// </summary>
    private void DialogStart()
    {
        panel.SetActive(true);          // 顯示對話資訊介面
        textName.text = name;           // 更新對話名稱
        StartCoroutine(Type());         // 啟動打字效果
    }

    /// <summary>
    /// 對話結束
    /// </summary>
    private void DialogStop()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// 面向玩家
    /// </summary>
    /// <param name="other">玩家</param>
    private void LookAtPlayer(Collider other)
    {
        Quaternion angle = Quaternion.LookRotation(other.transform.position - transform.position);      // 取得面向玩家的方向
        transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * 5);           // 角度插值
    }

    /// <summary>
    /// 玩家進入觸發區域
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Unity 藍") DialogStart();
    }

    /// <summary>
    /// 玩家離開觸發區域
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Unity 藍") DialogStop();
    }

    /// <summary>
    /// 玩家待在觸發區域會持續執行，約 60FPS
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Unity 藍") LookAtPlayer(other);
    }
}
