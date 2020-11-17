using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("固定量")]
    [Tooltip("最高滞空时间，跳跃时用")]
    public float MAX_JUMP_TIME = 0.3f;
    [Tooltip("手枪攻击间隔")]
    public float PISTOL_ATTACK_TIME = 0.30f;
    [Tooltip("枪械攻击间隔")]
    public float SWORD_ATTACK_TIME = 0.50f;
    [Tooltip("手枪子弹上限")]
    public int pistol_bullet_use_max = 13;
    [Tooltip("手枪弹夹上限")]
    public int pistol_bullet_max = 52;



    [Header("当前组件")]
    Transform m_transform;
    CharacterController controller;

    [Header("外部组件")]
    [Tooltip("枪械机瞄组件")]
    public GameObject gunanimatorGameobject;
    [Tooltip("枪械的机瞄动画")]
    Animator gunanimator;
    Animator myanimator;
    [Tooltip("摄像机")]
    public GameObject playercamera;
    [Tooltip("瞄准的准心")]
    public GameObject miaozhun;
    [Tooltip("血液组件")]
    public GameObject blood;
    public GameObject gamemanager;

    [Header("状态")]
    [Tooltip("当前持有武器是第几个")]
    public int isgun = 1;
    [Tooltip("当前是否在跑步")]
    public bool isrun = false;
    [Tooltip("是否在跳跃")]
    public bool isjump = false;
    [Tooltip("是否换弹")]
    public bool isreload = false;
    [Tooltip("当前武器.0为手枪")]
    int gun_type = 0;
    [Tooltip("当前子弹")]
    public int[] pistolBullet = new int[2] { 13, 26 };
    [Tooltip("手枪攻击时间")]
    float pistol_attack_time = 0;
    [Tooltip("剑攻击时间")]
    float sword_attack_time = 0;
    [Tooltip("换弹最大时间")]
    float reload_time_max = 1.5f;
    [Tooltip("换弹时间")]
    float reload_time = 0;
    [Tooltip("可以使用近战武器")]
    public bool is_can_sword=false;
    [Tooltip("可以使用近战武器")]
    public bool magic_sword = false;
    [Tooltip("等价代换")]
    public bool djdh = false;



    [Header("属性")]
    [Tooltip("当前速度，默认为1，按下shift为2")]
    public float m_speed = 1.0f;
    public float jumptime = 0f;
    //public float gravity = 10.0f;
    [Tooltip("生命值")]
    public float m_health = 100;
    public float m_health_max = 100;
    [Tooltip("防御力")]
    public float def = 0;
    [Tooltip("枪械攻击力")]
    public int gundama = 25;
    [Tooltip("近战攻击力")]
    public int sworddama = 50;

    [Tooltip("魔法伤害加成")]
    public float magicdama =0;
    [Tooltip("技能冷却时间缩减")]
    public float magic_cd = 1;

    public int[] shuxing = new int[4] { 0, 0, 0, 0 };

    [Header("音效")]
    AudioSource music;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;
    public AudioClip getBulletAudio;


    public void Awake()
    {
        music = gameObject.AddComponent<AudioSource>();
        music.playOnAwake = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //获取当前组件
        m_transform = this.transform;
        gunanimator = gunanimatorGameobject.GetComponent<Animator>();
        myanimator = this.gameObject.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        //其他
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAiming();
        move();
        IsReload();
        if (!isreload) run();
        if(!isreload && Time.timeScale !=0 && !gamemanager.GetComponent<GameManager>().isInPOG) shoot();
        guntime();
        sword();
    }

    void sword()
    {
        if (sword_attack_time >= 0)sword_attack_time -= Time.deltaTime;
        if(is_can_sword && Input.GetKeyDown(KeyCode.E)&& sword_attack_time<=0)
        {
            sword_attack_time = SWORD_ATTACK_TIME;
            gunanimator.SetTrigger("attack");
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("sword");
        }
    }

    /// <summary>
    /// 属性增加，num，0表示肌肉强度，1表示神经反应，2表示精神力，3表示细胞活力
    /// </summary>
    /// <param name="num">0表示肌肉强度，1表示神经反应，2表示精神力，3表示细胞活力</param>
    public void AddShuxing(int num)
    {
        shuxing[num]++;
        switch(num)
        {
            case 0:
                {
                    sworddama += 2;
                    m_health_max += 5;
                    m_health += 5;
                    break;
                }
            case 1:
                {
                    gundama += 1;
                    reload_time_max -= 0.05f;
                    if(shuxing[1] %3==0)
                    {
                        MAX_JUMP_TIME += 0.15f;
                    }
                    break;
                }
            case 2:
                {
                    magicdama += 0.05f;
                    magic_cd -= 0.025f;
                    break;
                }
            case 3:
                {
                    m_health_max += 15;
                    m_health += 15;
                    if (shuxing[3] % 2 == 0)
                    {
                        def += 1;
                    }
                    break;
                }
            default:break;
        }
    }

    public void GetBulletAudio()
    {
        music.clip = getBulletAudio;
        music.Play();
    }

    void IsReload()
    {
        reload_time = (reload_time - Time.deltaTime <= 0) ? 0 : reload_time - Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) && pistolBullet[1] ==0)
        {

        }
        else if(pistolBullet[0] == 0 && Input.GetMouseButtonDown(0) && pistolBullet[1] ==0)
        {

        }
        else if ((Input.GetKeyDown(KeyCode.R) && !isreload && pistolBullet[0] < 13) | (pistolBullet[0] == 0 && Input.GetMouseButtonDown(0)))
        {
            music.clip = reloadAudio;
            music.Play();
            isreload = true;
            gunanimator.SetBool("isreload", true);
            reload_time = reload_time_max;

            if(pistolBullet[1]< pistol_bullet_use_max)
            {
                if(13-pistolBullet[0] >pistolBullet[1])
                {
                    pistolBullet[0] += pistolBullet[1];
                    pistolBullet[1] = 0;
                }
                else
                {
                    pistolBullet[1] -= pistol_bullet_use_max - pistolBullet[0];
                    pistolBullet[0] = pistol_bullet_use_max;

                    //pistolBullet[0] += pistolBullet[1];
                    //pistolBullet[1] = 0;
                }
            }
            else
            {
                pistolBullet[1] -= pistol_bullet_use_max - pistolBullet[0];
                pistolBullet[0] = pistol_bullet_use_max;
                
            }

        }
        if(reload_time<=0)
        {
            isreload = false;
            gunanimator.SetBool("isreload", false);
        }
    }
    /// <summary>
    /// 子弹到下一次开枪时间
    /// </summary>
    void guntime()
    {
        pistol_attack_time = (pistol_attack_time - Time.deltaTime < 0) ? 0 : pistol_attack_time - Time.deltaTime;
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="dama">伤害数值</param>
    /// <param name="type">伤害类型，1表示魔法，其他表示普通</param>

    public void GetDamage(int dama,int type)
    {
        if(dama < 0)
        {
            m_health -= dama;
            m_health = (m_health > m_health_max) ? m_health_max : m_health;
        }
        else if(type == 1)
        {
            m_health -= dama;
            return;
        }
        else 
            m_health -= (dama - def > 0) ? dama - def : 0;
        gamemanager.GetComponent<GameManager>().UpdataHealth(m_health,m_health_max);
    }

       
    void SwitchAiming()
    {
        if (Input.GetMouseButtonDown(1) && isgun ==  1)
        {
            miaozhun.SetActive((miaozhun.activeSelf==true)?false:true);
            gunanimator.SetInteger("isgun",(gunanimator.GetInteger("isgun")==1)?2:1);
        }
    }


    void run()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {

            isrun = true;
            m_speed = m_speed*1.5f;
            gunanimator.SetBool("isrun", true);
            myanimator.SetTrigger("runTrigger");
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            isrun = false;
            m_speed = m_speed / 1.5f;
            gunanimator.SetBool("isrun", false);
            myanimator.SetTrigger("idleTrigger");
        }
    }


    void shoot()
    {
        if(Input.GetMouseButton(0))
        {
            if (gun_type == 0 && pistol_attack_time <= 0 && pistolBullet[0]>0)
            {
                pistolBullet[0]--;
                music.clip = shootAudio;
                music.Play();


                gunanimator.SetBool("isshoot", true);
                Ray shootRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
                //创建一个射线，射线从主相机向主相机的中心（也就是Screen.width / 2, Screen.height / 2这个点，直线延伸。）

                RaycastHit shootRayCast;
                //这里是一个射碰撞，用来检测碰撞目标
                if (Physics.Raycast(shootRay, out shootRayCast))//用物理的raycast语句，来连接射线和射线碰撞
                {
                    if (shootRayCast.collider.tag == "MonsterA")//如果射线碰撞的collider的tag是怪物，则执行语句
                        //在这里，shootRayCast已经是射线的碰撞目标了，我们可以用它调用目标的transform和gameobject
                    {
                        GameObject.Instantiate(blood, shootRayCast.point, Quaternion.Euler(new Vector3(-transform.localEulerAngles.x, -transform.localEulerAngles.y, -transform.localEulerAngles.z)));
                        shootRayCast.transform.gameObject.GetComponent<MonsterA>().GetDamage(gundama, 0, false);
                    }
                    else if(shootRayCast.collider.tag == "MonsterB")
                    {
                        GameObject.Instantiate(blood, shootRayCast.point, Quaternion.Euler(new Vector3(-transform.localEulerAngles.x, -transform.localEulerAngles.y, -transform.localEulerAngles.z)));
                        shootRayCast.transform.gameObject.GetComponent<MonsterB>().GetDamage(gundama, 0, false);
                    }

                    //射线只会和它碰撞的第一个拥有collider的目标触发，不会穿透
                    if (shootRayCast.collider.tag == "MonsterAhead")
                    {
                        shootRayCast.transform.GetComponentInParent<MonsterA>().GetDamage(gundama, 0, true);
                        GameObject.Instantiate(blood, shootRayCast.point, Quaternion.Euler(new Vector3(-transform.localEulerAngles.x, -transform.localEulerAngles.y, -transform.localEulerAngles.z)));
                    }
                    else if (shootRayCast.collider.tag == "MonsterBhead")
                    {
                        shootRayCast.transform.GetComponentInParent<MonsterB>().GetDamage(gundama, 0, true);
                        GameObject.Instantiate(blood, shootRayCast.point, Quaternion.Euler(new Vector3(-transform.localEulerAngles.x, -transform.localEulerAngles.y, -transform.localEulerAngles.z)));
                    }
                }
                pistol_attack_time = PISTOL_ATTACK_TIME;
            }
        }
        else
        {
            gunanimator.SetBool("isshoot", false);
        }
    }

    void move()
    {
        
        Vector3 moveDirection = playercamera.transform.forward * Input.GetAxis("Vertical") * m_speed;
        moveDirection += playercamera.transform.right * Input.GetAxis("Horizontal") * m_speed;
        if(Input.GetKeyDown(KeyCode.W)) myanimator.SetTrigger("walkTrigger");
        if (Input.GetKeyUp(KeyCode.W)) myanimator.SetTrigger("idleTrigger");

        
        if (Input.GetKeyDown(KeyCode.Space) && !isjump && controller.isGrounded)
        {
            isjump = true;
        }
        if (isjump)
        {
            jumptime += Time.deltaTime;
            if (jumptime >= MAX_JUMP_TIME)
            {
                isjump = false;
                //jumptime = 0;
            }
        }
        else
        {
            jumptime = (jumptime >= 0) ? jumptime - Time.deltaTime :0;
        }
        moveDirection.y += (isjump) ? 2.5f * (1 - jumptime/ MAX_JUMP_TIME) : -2.5f * (1 - jumptime / MAX_JUMP_TIME);
        controller.Move(moveDirection * Time.deltaTime);
    }

    public void GoToPOG()
    {
        controller.enabled = false;
        //Debug.Log("goto");
        m_transform.position = new Vector3(25, 0.5f, -30);
        controller.enabled = true;
    }
    public void GoToTesk2()
    {
        controller.enabled = false;
        //Debug.Log("goto");
        m_transform.position = new Vector3(48, 0.5f, -2);
        controller.enabled = true;
    }
}
