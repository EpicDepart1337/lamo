using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform target;
    public Transform lookTarget;
    public Vector3 offset;
    private Vector3 speed;
    // Start is called before the first frame update
    void Start()
    {
        offset = target.position - lookTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, lookTarget.position+offset, ref speed, 0.1f);
       // transform.LookAt(lookTarget);
    }
}
