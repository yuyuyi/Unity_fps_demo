using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBbody : MonoBehaviour
{
    public MonsterB father;
    // Start is called before the first frame update
    void Start()
    {
        father = GetComponentInParent<MonsterB>();
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "sword")
        {
            if (father.player.GetComponent<Player>().magic_sword)
            {

                if (Random.Range(0, 4) == 0)
                {
                    father.GetDamage(father.player.GetComponent<Player>().sworddama, 1, false);
                    Instantiate(father.magic_dama, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), transform.rotation);
                }
                else
                    father.GetDamage(father.player.GetComponent<Player>().sworddama, 0, false);
            }
            else
                father.GetDamage(father.player.GetComponent<Player>().sworddama, 0, false);
        }
    }
}
