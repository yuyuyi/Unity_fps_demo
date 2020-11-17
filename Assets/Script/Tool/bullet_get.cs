using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_get : MonoBehaviour
{
    public float dietime = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dietime -= Time.deltaTime;
        if (dietime <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetBulletAudio();
            int pis = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistolBullet[1];
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistolBullet[1] = (pis + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistol_bullet_use_max >= GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistol_bullet_max) ? GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistol_bullet_max : pis + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().pistol_bullet_use_max;
            Destroy(gameObject);
        }
    }

}
