using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterB : MonoBehaviour
{
    [Header("固定数值")]
    [Tooltip("视野相关")]
    static float EYEVIEWDISTANCE = 25; //距离
    static float VIEWANGLE = 180f; //角度
    



    [Header("属性")]
    [Tooltip("移动速度,闲逛下是0.6，寻找下是0.8，追逐下是1.2")]
    public float m_speed = 1f;
    public float m_health = 100.0f;
    public float def = 5;
    public int gold = 5;
    public int point = 0;
    public int attack = 10;
    enum status
    {
        Idle,
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
    public GameObject player;
    [Tooltip("掉落子弹")]
    public GameObject bullet;
    [Tooltip("寻路")]
    NavMeshAgent nav;
    [Tooltip("魔法伤害特效")]
    public GameObject magic_dama;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if(nowstatus == status.Idle)
        {
            nowstatus = status.run;
        }

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
            case status.run:MyRun(); break;
            case status.attack:MyAttack(); break;
            case status.die:MyDie();break;
            default:break;
        }
    }

    void MyIdle()
    {
        if(Vector3.Distance(transform.position,player.transform.position) <=6f | seeplayer())
        {
            nowstatus = status.run;
        }

        sleepTime -= Time.deltaTime;
    }

    void MyRun()
    {
        if (laststatus != status.run)
        {
            laststatus = status.run;
            gameObject.GetComponent<Animator>().SetTrigger("run");
            m_speed = 1.2f;
        }

        nav.SetDestination(player.transform.position);
        nav.speed = 1;
        if (Vector3.Distance(player.transform.position, transform.position) < 0.65f)
        {
            nowstatus = status.attack;
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
        nav.speed = 0;
        die_time -= Time.deltaTime;
        if (die_time <= 0) Destroy(gameObject);
    }

    bool seeplayer()
    {
        return (Vector3.Angle(transform.forward, player.transform.position - transform.position) <= VIEWANGLE / 2 && Vector3.Distance(player.transform.position, transform.position) < EYEVIEWDISTANCE);

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sword")
        {
            if (player.GetComponent<Player>().magic_sword)
            {

                if (Random.Range(0, 4) == 0)
                {
                    GetDamage(player.GetComponent<Player>().sworddama, 1, false);
                    Instantiate(magic_dama, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
                }
                else
                    GetDamage(player.GetComponent<Player>().sworddama, 0, false);
            }
            else
                GetDamage(player.GetComponent<Player>().sworddama, 0, false);
        }
    }
    
}
