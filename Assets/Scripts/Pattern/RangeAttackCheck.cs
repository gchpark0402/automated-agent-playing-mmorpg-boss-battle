using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackCheck : MonoBehaviour
{
    public bool isAgentExist = false;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Agent") isAgentExist= true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Agent") isAgentExist = false;
    }
}
