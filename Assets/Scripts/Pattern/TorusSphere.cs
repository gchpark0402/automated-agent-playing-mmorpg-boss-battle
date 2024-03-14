using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class TorusSphere : MonoBehaviour
{
    public EarthQuakeTorus torus;
    // Start is called before the first frame update
    void Start()
    {
        torus = this.transform.parent.GetComponent<EarthQuakeTorus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Agent")
        {
            torus.SetIsAgentInFalse(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Agent")
        {
            torus.SetIsAgentInTrue(other);
        }
    }
}
