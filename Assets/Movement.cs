using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private GameObject player;
    private List<GameObject> nodes = new List<GameObject>();
    private int location = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        // Strangely, Unity have the children GameObject not in a getChildren() function
        // but as an element returned in a foreach iterator.

        foreach (Transform child in transform)
        {
            nodes.Add(child.gameObject);
        }

        InputMapper filter = Object.FindObjectsOfType<InputMapper>()[0];
        filter.register(InputMapper.Actions.TELEPORT, (InputMapper.VoidEventHandler)this.teleport);

    }

    private void teleport()
    {
        location = (location + 1) % nodes.Count;
        player.transform.position = nodes[location].transform.position;
        player.transform.localRotation = nodes[location].transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
