using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUNButton : MonoBehaviour, IPointerClickHandler
{

    GameManager gameManager;// = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    Player player;// = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    public GameObject exv, exx;
    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    bool isUp = false;
    bool hf(int a)
    {
        if (gameManager.gold >= a)
        {
            Instantiate(exv, transform.parent);
            gameManager.gold -= a;
            isUp = true;
            gameObject.GetComponent<Image>().color = Color.gray;
            return true;
        }
        Instantiate(exx, transform.parent);
        return false;
    }
    bool hf(int a,int b)
    {
        if(gameManager.gold>=a && gameManager.spPoint>=b)
        {
            gameManager.gold -= a;
            gameManager.spPoint -= b;
            gameObject.GetComponent<Image>().color = Color.gray;
            isUp = true;
            return true;
        }
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isUp)
            switch (gameObject.name)
            {
                case "ccdt":
                    if (hf(200))
                    {
                        player.gundama += 5;
                    }
                    break;
                case "zxdk":
                    if(hf(300))
                    {
                        player.gundama += 5;
                        player.PISTOL_ATTACK_TIME += 0.05f;
                    }
                    break;
                case "ghg":
                    if (hf(400))
                    {
                        
                    }
                    break;
                case "djyd":
                    if (hf(350))
                    {
                        player.pistol_bullet_max += 45;
                        player.pistol_bullet_use_max += 5;
                    }
                    break;
                case "dlx":
                    if (hf(200))
                    {
                        player.m_speed += 0.15f;
                    }
                    break;
                case "phgz":
                    if (hf(450))
                    {
                        player.m_speed += 0.08f;
                        player.gundama += 3;
                    }
                    break;
                case "fsgz":
                    if (hf(450))
                    {
                        player.m_speed += 0.06f;
                        player.magicdama += 0.25f;
                    }
                    break;
                case "zjgz":
                    if (hf(400))
                    {
                        player.m_speed += 0.06f;
                        player.def += 5;
                    }
                    break;
                case "jd":
                    if (hf(300))
                    {
                        player.is_can_sword = true;
                    }
                    break;
                case "sx":
                    if (hf(600,1))
                    {
                        player.sworddama += 15;
                        player.magic_sword = true;
                    }
                    break;
                case "hd":
                    if (hf(600,1))
                    {
                        player.sworddama += 10;
                        player.SWORD_ATTACK_TIME -= 0.15f;
                        player.def += 3;
                    }
                    break;
                case "pj":
                    if (hf(300))
                    {
                        player.sworddama += 10;
                    }
                    break;
                case "djdh":
                    if (hf(300))
                    {
                        player.djdh = true;
                    }
                    break;
                case "qg":
                    if (hf(500,1))
                    {
                        player.m_speed += 0.5f;
                        player.MAX_JUMP_TIME += 0.3f;
                    }
                    break;
                default:
                    break;
            }
    }
}
