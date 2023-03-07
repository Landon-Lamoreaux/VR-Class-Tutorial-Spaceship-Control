using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchEvents : MonoBehaviour
{
    private List<GameObject> nodes = new List<GameObject>();
    private int location = 0;
    public float timeDelay = 1f;
    private float lastCall = 0;
    public void OnToggle()
    {
        // On each timeout, toggle another switch.
        if (Time.realtimeSinceStartup - lastCall > timeDelay)
        {
            Debug.Log("Toggle!");
            // Toggle is jsut a rotation.
            GameObject obj = nodes[location];
            obj.transform.localRotation = Quaternion.Euler(60, 0, 0);
            location = (location + 1) % nodes.Count;

            // Reset timer for next timout.
            lastCall = Time.realtimeSinceStartup;
        }
    }
    void Start()
    {
        // Get each of the switch children, and store them.
        foreach (Transform child in transform)
        {
            nodes.Add(child.gameObject);
        }

        InputMapper filter = Object.FindObjectsOfType<InputMapper>()[0];
        filter.register(InputMapper.Actions.TOGGLE, (InputMapper.VoidEventHandler)this.OnToggle);

    }

    // Update is called once per frame.
    void Update()
    {
        
    }
}
