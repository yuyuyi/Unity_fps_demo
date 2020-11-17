using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 veca;
    private Vector3 vecb;
    public float lingmingdu = 2.0f;

    public GameObject player;

    [Tooltip("是否正在菜单页面")]
    public bool ismenu = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!ismenu)
        {
            veca.x -= Input.GetAxis("Mouse Y") * lingmingdu;
            vecb.y += Input.GetAxis("Mouse X") * lingmingdu;
            this.transform.localEulerAngles = veca;
            player.transform.localEulerAngles = vecb;
        }
    }
}
