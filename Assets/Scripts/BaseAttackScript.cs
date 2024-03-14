using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackScript : MonoBehaviour
{
    public bool isHit = false;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Base attack : " + other.tag);
        if(other.tag == "Boss")
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boss")
        {
            isHit = false;
        }
    }
}
