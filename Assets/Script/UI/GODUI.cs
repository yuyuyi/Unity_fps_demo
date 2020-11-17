using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GODUI : MonoBehaviour
{
    public GameObject[] Panels;
    // Start is called before the first frame update




    void SwAllFalse()
    {
        for(int i=0;i<4;i++)
        {
            Panels[i].gameObject.SetActive(false);
        }
    }

    public void SwBody()
    {
        SwAllFalse();
        Panels[0].gameObject.SetActive(true);
    }

    public void SwGun()
    {
        SwAllFalse();
        Panels[1].gameObject.SetActive(true);
    }

    public void SwLineage()
    {
        SwAllFalse();
        Panels[2].gameObject.SetActive(true);
    }

    public void SwSkill()
    {
        SwAllFalse();
        Panels[3].gameObject.SetActive(true);
    }

}
