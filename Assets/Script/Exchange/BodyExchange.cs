using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyExchange : MonoBehaviour ,IPointerClickHandler
{
    GameObject player;
    GameObject gameManager;

    public GameObject exv, exx;

    public int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameManager.GetComponent<GameManager>().gold>=100)
        {
            gameManager.GetComponent<GameManager>().gold -= 100;
            Instantiate(exv,transform.parent);
            player.GetComponent<Player>().AddShuxing(num);

        }
        else
        {
            Instantiate(exx,transform.parent);
        }
        

        //throw new System.NotImplementedException();
    }
}
