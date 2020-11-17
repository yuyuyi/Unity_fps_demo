using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task2 : MonoBehaviour
{
    [Header("终点")]
    GameObject endPoint;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(endPoint.transform.position,player.transform.position)<5)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GetGold(1000,1);
            player.transform.position = GameObject.FindGameObjectWithTag("GOD").transform.position;
            player.GetComponent<Player>().GoToPOG();
            player.GetComponent<Player>().m_health = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().m_health_max;
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isInPOG = true;
            Destroy(gameObject);
        }
    }
}
