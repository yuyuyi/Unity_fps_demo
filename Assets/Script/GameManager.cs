using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("属性")]
    public int gold = 0;
    public int spPoint = 0;

    [Header("状态")]
    [Tooltip("是否在主神空间")]
    public bool isInPOG = false;
    

    [Header("设置")]
    [Tooltip("光照强度，和全局光照相关")]
    public float light = 1.0f;
    [Tooltip("是否有声音")]
    public bool isAudio = false;

    [Header("外部组件")]
    public GameObject UIMain;
    public GameObject playercamera;
    public GameObject lightBar;
    public GameObject gunUI;
    public GameObject GODUI;

    public GameObject health,health_t;

    //public GameObject audioMain;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //同步数据
        light = lightBar.GetComponent<Scrollbar>().value;
        UpdataGold();
        OpenExchange();



        Exit();
    }

    void OpenExchange()
    {
        if (isInPOG && Input.GetKeyDown(KeyCode.Escape) && GODUI.active == true)
        {
            GODUI.SetActive(false);
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
            //Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

            playercamera.GetComponent<PlayerCamera>().ismenu = (playercamera.GetComponent<PlayerCamera>().ismenu == true) ? false : true;

        }
        if (isInPOG && Input.GetKeyDown(KeyCode.B))
        {
            GODUI.SetActive((GODUI.active==true)?false:true);
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
            //Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

            playercamera.GetComponent<PlayerCamera>().ismenu = (playercamera.GetComponent<PlayerCamera>().ismenu == true) ? false : true;

        }
    }

    /// <summary>
    /// 更新生命值
    /// </summary>
    /// <param name="m_health">更新后的生命值</param>
    /// <param name="max_health">更新后的最大生命值</param>
    public void UpdataHealth(float m_health , float max_health)
    {
        health.GetComponent<Text>().text = m_health.ToString();
        health_t.transform.localScale = new Vector3(m_health/max_health,1,1);
        
        if(m_health<=0)
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
            Time.timeScale = (Time.timeScale == 0) ? 1 : 0;
            playercamera.GetComponent<PlayerCamera>().ismenu = (playercamera.GetComponent<PlayerCamera>().ismenu == true) ? false : true;
            gunUI.transform.Find("lose").gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 更新视野左上角的奖励点数和支线剧情
    /// </summary>
    void UpdataGold()
    {
        gunUI.gameObject.transform.Find("gold").GetComponent<Text>().text = "奖励点：" + gold + "\r\n\r\n剧情点数：" + spPoint;
    }


    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="a">奖励点</param>
    public void GetGold(int a)
    {
        gold += a;
    }

    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="a">奖励点</param>
    /// <param name="b">剧情点数</param>
    public void GetGold(int a,int b)
    {
        gold += a;
        spPoint += b;
    }

    /// <summary>
    /// 按下esc后招出菜单，阿姨你快一点
    /// </summary>
    void Exit()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GODUI.active == false )
        {
            UIMain.SetActive((UIMain.active == true) ? false : true);
            Cursor.lockState =(Cursor.lockState == CursorLockMode.Locked)?CursorLockMode.Confined:CursorLockMode.Locked;
            Time.timeScale = (Time.timeScale == 0)?1:0;
            
            playercamera.GetComponent<PlayerCamera>().ismenu = (playercamera.GetComponent<PlayerCamera>().ismenu == true) ? false : true;
            
            
            //UnityEditor.EditorApplication.isPlaying = false;
            //Application.Quit();
        }
    }

    /// <summary>
    /// 按下返回之后返回，给阿姨倒一杯卡布奇诺
    /// </summary>
    public void ReExit()
    {
        UIMain.SetActive((UIMain.active == true) ? false : true);
        Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.Confined : CursorLockMode.Locked;
        Time.timeScale = (Time.timeScale == 0) ? 1 : 0;

        playercamera.GetComponent<PlayerCamera>().ismenu = (playercamera.GetComponent<PlayerCamera>().ismenu == true) ? false : true;

    }

    /// <summary>
    /// 按下退出游戏后退出游戏，反手给一个超级退出
    /// </summary>
    public void SuperExit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    public void IsSure()
    {
        //光芒组件
        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");
        for(int i=0;i<lights.Length;i++)
        {
            lights[i].GetComponent<Light>().intensity = light;
        }

        //声音组件
        playercamera.GetComponent<AudioListener>().enabled = isAudio;
    }

    public void AudioChange(bool res)
    {
        isAudio = res;
    }
}
