using System;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class CameraFollowTarget : MonoBehaviour
{
    Transform _target;
    public bool trackPlayerX = true;
    public bool trackPlayerY = true;
    public bool trackPlayerRot = true;
    
    private void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        float x = trackPlayerX ? _target.position.x : transform.position.x;
        float y = trackPlayerY ? _target.position.y : transform.position.y;
        Vector3 rot = trackPlayerRot ? _target.rotation.eulerAngles : Vector3.zero;
        
        transform.position = new Vector3(x, y, transform.position.z);
        transform.rotation = Quaternion.Euler(rot);
    }
}
