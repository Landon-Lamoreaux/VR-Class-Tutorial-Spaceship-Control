using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Key"))
        {
            Renderer r = this.GetComponent<Renderer>();
            Material m = r.material;
            m.color = Color.magenta;
        }
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Contains("Key"))
        {
            Renderer r = this.GetComponent<Renderer>();
            Material m = r.material;
            m.color = Color.white;
        }
    }
}
