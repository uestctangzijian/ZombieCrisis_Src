using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private Vector3 relativePos;

    // Start is called before the first frame update
    void Start()
    {
        relativePos = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 跟随玩家
        transform.position = target.transform.position + relativePos;
        // 面朝相机
        transform.rotation = Camera.main.transform.rotation;
    }
}
