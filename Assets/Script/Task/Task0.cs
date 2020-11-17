using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task0 : MonoBehaviour
{
    public float live_time = 60;

    public GameObject gunui;
    public GameObject player;

    public GameObject task2;

    private void Awake()
    {
        gunui = GameObject.FindGameObjectWithTag("gunUI");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        gunui = GameObject.FindGameObjectWithTag("gunUI");
    }

    // Update is called once per frame
    void Update()
    {
        live_time -= Time.deltaTime;
        gunui.GetComponent<Text>().text = "按下b打开兑换菜单\n下一个任务时间：" + (int)live_time;

        if (live_time<=0)
        {
            player.GetComponent<Player>().GoToTesk2();
            Instantiate(task2);
            Destroy(gameObject);
        }
    }
}
