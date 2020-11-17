using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task1 : MonoBehaviour
{
    public GameObject[] monsterType;
    GameObject[] monsters;
    GameObject[] allpoint;

    public GameObject UIMain;
    public GameObject gunui;
    public GameObject player;
    public GameObject renwutishi;
    public GameObject task0;

    public int task = 1;

    public float live_time = 180;

    float waittime = 5.0f;
    public int round = 4;
    int[] roundmonster = new int[6] {0,3,4,6,7,10};
    //int[] roundmonster = new int[6] { 0, 1, 1, 1, 1, 1 };


    float taosha_time = 1.0f;//task3刷怪间隔为1秒
                             // Start is called before the first frame update

    void Start()
    {
        allpoint = GameObject.FindGameObjectsWithTag("targetpoint");
        
    }
    
    void TheTask()
    {
        if(task ==1)
        {
            monsters = GameObject.FindGameObjectsWithTag("MonsterA");
            if (monsters.Length == 0)
            {
                waittime -= Time.deltaTime;
            }

            if (waittime <= 0)
            {
                round++;
                if (round > 5)
                {
                    TaskUpdata();
                    task++;
                    nextTask();
                    waittime = 5.0f;
                }
                else
                {
                    monstercreat();
                    waittime = 5.0f;
                }
            }
        }
        else if(task == 2)
        {
            monsters = GameObject.FindGameObjectsWithTag("MonsterA");
            if (monsters.Length == 0)
            {
                TaskUpdata();
                task++;
                nextTask();
            }
        }
        else if(task == 3)
        {
            monsters = GameObject.FindGameObjectsWithTag("MonsterA");
            taosha_time -= Time.deltaTime;
            if (taosha_time <= 0)
            {
                monstercreattaosha();
                taosha_time = 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        live_time -= Time.deltaTime;
        gunui.transform.Find("live_time").GetComponent<Text>().text = "剩余回归时间：" + (int)live_time;

        if(live_time<=0)
        {
            gunui.transform.Find("live_time").GetComponent<Text>().text = "按下b打开兑换菜单";
            endTask();
        }

        TheTask();
    }

    void TaskUpdata()
    {
        Instantiate(renwutishi,gameObject.transform);
    }
    void monstercreat()
    {
        for(int i=0;i<roundmonster[round];i++)
        {
            Random.seed *= Random.seed;
            if(Random.Range(0,10)<2)Instantiate(monsterType[1],allpoint[Random.Range(0,5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
            else if (Random.Range(0, 10) < 2) Instantiate(monsterType[2], allpoint[Random.Range(0, 5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
            else  Instantiate(monsterType[0], allpoint[Random.Range(0, 5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
        }
    }
    void monstercreattaosha()
    {
        if (Random.Range(0, 10) < 2) Instantiate(monsterType[1], allpoint[Random.Range(0, 5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
        else if (Random.Range(0, 10) < 2) Instantiate(monsterType[2], allpoint[Random.Range(0, 5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
        else Instantiate(monsterType[0], allpoint[Random.Range(0, 5)].transform.position, allpoint[Random.Range(0, 5)].transform.rotation, this.gameObject.transform);
    }

    void nextTask()
    {
        if(task==2)
        {
            live_time = 60;
            //UIMain.transform.Find("task").GetComponent<Text>().text = "场景：《教堂》\r\n\r\n主线任务2：存活1分钟，或者击杀变体丧尸。\r\n\r\n击杀变体丧尸，奖励500奖励点+1剧情点数";
            UIMain.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "场景：《教堂》\r\n\r\n主线任务2：存活1分钟，或者击杀变体丧尸，开启主线任务3,。\r\n\r\n击杀变体丧尸，奖励500奖励点+1剧情点数";
            Instantiate(monsterType[3], allpoint[0].transform.position, allpoint[0].transform.rotation, this.gameObject.transform);
        }
        if(task == 3)
        {
            UIMain.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = "场景：《教堂》\r\n\r\n主线任务3：存活1分钟，在狂暴的怪物潮中活下去。\r\n\r\n击杀变体丧尸，奖励500奖励点+1剧情点数";
            live_time = 60;

        }
    }

    void endTask()
    {
        Instantiate(task0);

        if(task == 3) GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetGold(0,1);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetGold(1000);
        player.transform.position = GameObject.FindGameObjectWithTag("GOD").transform.position;
        //player.GetComponent<CharacterController>().enabled = false;
        //player.GetComponent<CharacterController>().Move(new Vector3(25, 0.5f, -30));
        //player.transform.position = new Vector3(25, 0.5f, -30);
        //player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<Player>().GoToPOG();
        player.GetComponent<Player>().m_health = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().m_health_max;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isInPOG = true;
        Destroy(gameObject);
    }


}
