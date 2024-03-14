using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempScript : MonoBehaviour
{
    public bool b_temp = false;
    public Collider cl_temp= null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(b_temp && cl_temp == null)
        {
            b_temp = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Danger")
        {
            cl_temp = other;
            b_temp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Danger")
        {
            b_temp = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Danger")
        {
            Debug.Log("hell0");
        }
    }

}
