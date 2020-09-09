using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [Header("NPC 資料")]
    public NPCData data;
    [Header("對話資訊")]
    public GameObject panel;
    [Header("對話名稱")]
    public Text textName;

    private void Type()
    {

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
        panel.SetActive(true);
        textName.text = name;
    }

    /// <summary>
    /// 對話結束
    /// </summary>
    private void DialogStop()
    {
        panel.SetActive(false);
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
}
