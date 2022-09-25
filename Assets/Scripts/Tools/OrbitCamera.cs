using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f;

    private float _rotY;
    private Vector3 _offset;

    [SerializeField]
    private float xMin = -9.5f;
    [SerializeField]
    private float xMax = -0.6f;
    [SerializeField]
    private float zMin = -28.5f;
    [SerializeField]
    private float zMax = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _offset = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - _offset;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax),
                                       transform.position.y,
                                       Mathf.Clamp(transform.position.z, zMin, zMax));
        //transform.LookAt(target);
    }
}
