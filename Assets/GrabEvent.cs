using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabEvent : MonoBehaviour
{
    private InputMapper filter;
    private List<GameObject> inRange = new List<GameObject>();
    private GameObject inHand = null;

    // Start is called before the first frame update.
    void Start()
    {
        // Get actions.
        filter = FindObjectsOfType<InputMapper>()[0];
    }
    private void PlaceInHand(GameObject inHand)
    {
        Transform hand = transform.Find("Hand");

        // Break old parenting and positioning.
        inHand.transform.parent = null;

        // Parent to hand.
        inHand.transform.parent = hand;
        inHand.transform.localPosition = new Vector3(0, 0, 0);
        inHand.transform.localRotation = hand.localRotation;
    }

    // Register and unregister within range.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            filter.register(InputMapper.Actions.GRAB, (InputMapper.VoidEventHandler)this.OnGrab);
            filter.register(InputMapper.Actions.RELEASE, (InputMapper.VoidEventHandler)this.OnRelease);
            inRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Grabbable")
        {
            inRange.Remove(other.gameObject);
            filter.deregister((InputMapper.VoidEventHandler)this.OnGrab);
            filter.deregister((InputMapper.VoidEventHandler)this.OnRelease);
        }
    }

    // Grab action.
    public void OnGrab()
    {
        Debug.Log("Grabbed");

        // Sanity check, do not grab is hands are full.
        if (inHand == null)
        {
            GameObject closest = inRange[0];
            for (int i = 1; i < inRange.Count; i++)
            {
                if ((this.transform.position - closest.transform.position).magnitude >
                (this.transform.position - inRange[i].transform.position).magnitude)
                {
                    closest = inRange[i];
                }
            }
            inHand = closest;
            PlaceInHand(inHand);
        }
    }

    // Release action.
    public void OnRelease()
    {
        if (inHand != null)
        {
            inHand.transform.parent = null;
            inHand = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
