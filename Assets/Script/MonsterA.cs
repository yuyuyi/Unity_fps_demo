using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterA : MonoBehaviour
{
    [Header("固定数值")]
    [Tooltip("嗅觉范围")]
    static float RANGE_SMELL = 4;
    [Tooltip("狂暴范围")]
    static float RANGE_VIOLENT = 2.5f;
    [Tooltip("视野相关")]
    static float EYEVIEWDISTANCE = 8; //距离
    static float VIEWANGLE = 180f; //角度
    [Tooltip("壁障范围数据")]
    static float minimumDistToAvoid = 0.6f;
    



    [Header("属性")]
    [Tooltip("移动速度,闲逛下是0.6，寻找下是0.8，追逐下是1.2")]
    public float m_speed = 0.6f;
    public float m_speed_find = 0.8f;
    public float m_speed_run = 1.2f;
    public float m_health = 100.0f;
    public float def = 5;
    public int gold = 5;
    public int point = 0;
    public int attack = 10;
    enum status
    {
        Idle,
        walk,
        find,
        run,
        attack,
        die,
    }
    [Tooltip("状态转换等待时间，默认为3秒")]
    float sleepTime = 3.0f;
    [Tooltip("攻击间隔")]
    public float attack_time = 1.0f;
    
    [Tooltip("死亡时间")]
    float die_time = 2.0f;


    [Tooltip("当前状态")]
    status nowstatus = status.Idle;
    //用于记录上一个状态，来完成对不同动画的转换
    status laststatus = status.Idle;

    [Header("外部组件")]
    [Tooltip("玩家角色")]
    GameObject player;
    [Tooltip("下一个路径点")]
    GameObject nextpoint;
    [Tooltip("所有路径点")]
    GameObject[] allpoint;
    [Tooltip("和玩家距离的三维向量")]
    Vector3 dir;
    [Tooltip("掉落子弹")]
    public GameObject bullet;
    [Tooltip("魔法伤害特效")]
    public GameObject magic_dama;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        allpoint = GameObject.FindGameObjectsWithTag("targetpoint");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir = (player.transform.position - transform.position);
        dir.Normalize();
        AvoidObstacles(ref dir);
        if (m_health <= 0) nowstatus = status.die;
        StateUpdata();
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="dama">伤害数值</param>
    /// <param name="type">伤害类型，1表示魔法，其他表示普通</param>
    /// <param name="boom">是否暴击</param>

    public void GetDamage(int dama, int type ,bool boom)
    {
        if (type == 1)
        {
            m_health -= dama;
            return;
        }
        if (boom)
        {
            m_health -= ((dama * 3 - def) > 0) ? (dama * 3 - def) : 0;
            return;
        }
        m_health -= (dama - def > 0) ? dama - def : 0;
    }

    void StateUpdata()
    {
        attack_time = (attack_time- Time.deltaTime <= 0) ? 0 : attack_time- Time.deltaTime;

        switch(nowstatus)
        {
            case status.Idle:MyIdle(); break;
            case status.walk:MyWalk(); break;
            case status.find: MyFind(); break;
            case status.run:MyRun(); break;
            case status.attack:MyAttack(); break;
            case status.die:MyDie();break;
            default:break;
        }
    }

    void MyIdle()
    {
        if(laststatus != status.Idle)
        {
            laststatus = status.Idle;
            gameObject.GetComponent<Animator>().SetTrigger("idle");
        }

        if (Vector3.Distance(player.transform.position, transform.position) < RANGE_SMELL | seeplayer())
        {
            nowstatus = status.find;
        }

        sleepTime -= Time.deltaTime;

        if(sleepTime <=0)
        {
            sleepTime = 3.0f;
            nextpoint = allpoint[Random.Range(0,5)];
            nowstatus = status.walk;
            
        }
        
    }

    void MyWalk()
    {
        if (laststatus != status.walk)
        {
            laststatus = status.walk;
            gameObject.GetComponent<Animator>().SetTrigger("walk");
            m_speed = 0.6f;
        }
        /*
        if (nextpoint.transform.position.x >= transform.position.x) transform.localEulerAngles = new Vector3(0, Vector3.Angle(transform.forward, nextpoint.transform.position - transform.position), 0);
        else transform.localEulerAngles = new Vector3(0, -Vector3.Angle(transform.forward, nextpoint.transform.position - transform.position), 0);
        */
        transform.LookAt(new Vector3(nextpoint.transform.position.x, 0 , nextpoint.transform.position.z));
        if (Vector3.Distance(player.transform.position, transform.position) <RANGE_SMELL | seeplayer())
        {
            nowstatus = status.find;
        }

        //transform.localEulerAngles = new Vector3(0, Vector3.Angle(transform.forward, nextpoint.transform.position - transform.position), 0);
        //transform.localEulerAngles = new Vector3(0, Vector3.Angle(transform.position,nextpoint.transform.position), 0);
        gameObject.GetComponent<CharacterController>().SimpleMove((nextpoint.transform.position - transform.position).normalized * m_speed);
        //var rot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);
        //gameObject.GetComponent<CharacterController>().SimpleMove(Vector3.forward.normalized * m_speed);
        if (Vector3.Distance(nextpoint.transform.position,transform.position)<=0.5f)
        {
            nowstatus = status.Idle;
        }
    }

    void MyFind()
    {
        if (laststatus != status.find)
        {
            laststatus = status.find;
            gameObject.GetComponent<Animator>().SetTrigger("find");
            m_speed = 0.8f;
        }

        //transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));

        //gameObject.GetComponent<CharacterController>().SimpleMove((player.transform.position - transform.position).normalized * m_speed);
        var rot = Quaternion.LookRotation(dir);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);
        gameObject.GetComponent<CharacterController>().SimpleMove(transform.forward.normalized * m_speed_find);
        if (Vector3.Distance(player.transform.position, transform.position) < RANGE_VIOLENT)
        {
            nowstatus = status.run;
        }
        if (Vector3.Distance(player.transform.position, transform.position) > RANGE_SMELL & !seeplayer())
        {
            nowstatus = status.Idle;
        }
    }
    void MyRun()
    {
        if (laststatus != status.run)
        {
            laststatus = status.run;
            gameObject.GetComponent<Animator>().SetTrigger("run");
            m_speed = 1.2f;
        }
        //transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        //gameObject.GetComponent<CharacterController>().SimpleMove((player.transform.position - transform.position).normalized * m_speed);
        var rot = Quaternion.LookRotation(dir);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);
        gameObject.GetComponent<CharacterController>().SimpleMove(transform.forward.normalized * m_speed_run);
        if (Vector3.Distance(player.transform.position, transform.position) < 0.65f)
        {
            nowstatus = status.attack;
        }
        if (Vector3.Distance(player.transform.position, transform.position) > RANGE_VIOLENT)
        {
            nowstatus = status.find;
        }
    }

    void MyAttack()
    {
        if (laststatus != status.attack)
        {
            laststatus = status.attack;
            gameObject.GetComponent<Animator>().SetTrigger("attack");
        }
        
        if (attack_time<=0 && Vector3.Distance(player.transform.position, transform.position) < 0.65f)
        {
            attack_time = 1.0f;
            player.GetComponent<Player>().GetDamage(attack, 0);
        }

        //transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        //gameObject.GetComponent<CharacterController>().SimpleMove((player.transform.position - transform.position).normalized * m_speed);
        var rot = Quaternion.LookRotation(dir);
        rot.x = 0;
        rot.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5.0f * Time.deltaTime);
        if (Vector3.Distance(player.transform.position, transform.position) > 0.35f) gameObject.GetComponent<CharacterController>().SimpleMove(transform.forward.normalized * m_speed_run);
        if (Vector3.Distance(player.transform.position, transform.position) > 0.65f)
        {
            nowstatus = status.run;
        }
    }

    void MyDie()
    {
        if (laststatus != status.die)
        {
            laststatus = status.die;
            gameObject.GetComponent<Animator>().SetTrigger("die");
            gameObject.GetComponent<Animator>().ResetTrigger("attack"); 
            gameObject.GetComponent<Animator>().ResetTrigger("run");
            
            if (Random.Range(0, 100) < 50)
            {
                Instantiate(bullet, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.35f, gameObject.transform.position.z), gameObject.transform.rotation); ;
            }
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetGold(gold, point);
        }
        m_speed = 0;
        die_time -= Time.deltaTime;
        if (die_time <= 0) Destroy(gameObject);
    }

    bool seeplayer()
    {
        return (Vector3.Angle(transform.forward, player.transform.position - transform.position) <= VIEWANGLE / 2 && Vector3.Distance(player.transform.position, transform.position) < EYEVIEWDISTANCE);

    }


    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;

        
        int layerMask = 1 << 9;//只检测第九层，也就是我设置的wall障碍物

        
        if (Physics.Raycast(transform.position, transform.forward, out hit, minimumDistToAvoid, layerMask))
        {
            //获得法线计算方向
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f; //不改变y值，不过由于我使用的是move和角色控制器，就不需要了

            //获取新的方向矢量，这里由于使用的是角色控制器，所以直接将其加载在玩家坐标上。
            dir = transform.forward + hitNormal * 80f ;
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "sword")
        {
            if(player.GetComponent<Player>().magic_sword)
            {
                
                if(Random.Range(0,4)==0)
                {
                    GetDamage(player.GetComponent<Player>().sworddama, 1, false);
                    Instantiate(magic_dama,new Vector3(transform.position.x,transform.position.y+0.3f,transform.position.z),transform.rotation);
                }
                else
                    GetDamage(player.GetComponent<Player>().sworddama, 0, false);
            }
            else
                GetDamage(player.GetComponent<Player>().sworddama,0,false);

            
        }
    }

}
