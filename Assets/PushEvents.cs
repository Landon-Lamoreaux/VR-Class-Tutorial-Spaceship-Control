using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushEvents : MonoBehaviour
{
    private float thetaX = 0;
    private float thetaZ = 0;
    [SerializeField]
    private float speed = 0.2f;
    [SerializeField]
    private bool right = false;
    public void Push(object info)
    {
        //Debug.Log("Pushed " + right + (Vector3)info);
        Vector3 delta = (Vector3)info;
        thetaX -= delta.x * speed;
        thetaZ += delta.y * speed;
        thetaX = Mathf.Clamp(thetaX, -20, 20);
        thetaZ = Mathf.Clamp(thetaZ, -20, 20);
        transform.localRotation = Quaternion.Euler(thetaZ, 0, thetaX);
    }
    public void Start()
    {
        InputMapper filter = Object.FindObjectsOfType<InputMapper>()[0];
        if (right)
        {
            filter.register(InputMapper.Actions.PUSH_RIGHT, (InputMapper.DataEventHandler)this.Push);
        }
        else
        {
            filter.register(InputMapper.Actions.PUSH_LEFT, (InputMapper.DataEventHandler)this.Push);
        }
    }


    // Update is called once per frame.
    void Update()
    {
        
    }
}
