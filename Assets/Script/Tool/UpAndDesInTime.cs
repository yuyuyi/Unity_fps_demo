using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDesInTime : MonoBehaviour
{
    [Tooltip("用来表示需要多少时间后会消失")]
    public float m_time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(0,Time.deltaTime *0.1f,0);
        GetComponent<RectTransform>().Translate(0, Time.deltaTime * 4, 0);
        m_time -= Time.deltaTime;
        if (m_time <= 0) Destroy(gameObject);
    }
}
