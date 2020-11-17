using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bullet.GetComponent<Text>().text = player.GetComponent<Player>().pistolBullet[0] + "  /  " + player.GetComponent<Player>().pistolBullet[1];
    }


}
