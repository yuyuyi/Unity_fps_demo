using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesInTime : MonoBehaviour
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
        m_time -= Time.deltaTime;
        if (m_time <= 0) Destroy(gameObject);
    }
}
