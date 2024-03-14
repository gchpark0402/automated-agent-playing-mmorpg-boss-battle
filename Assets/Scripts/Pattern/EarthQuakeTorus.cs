using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EarthQuakeTorus : MonoBehaviour
{
    public bool isAgentIn = false;
    public AgentScript agent = null;

    // Start is called before the first frame update
    void Start()
    {

    }


    public IEnumerator AgentDamage(float second)
    {
        yield return new WaitForSeconds(second);

        if(isAgentIn)
        {
            agent.Damaged();
        }

        this.gameObject.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Agent")
        {
            SetIsAgentInTrue(other);
            
        }
        else
        {
            SetIsAgentInFalse(other);
        }
           
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.tag == "Agent")
        {
            SetIsAgentInTrue(other);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.tag == "Agent")
        {
            SetIsAgentInFalse(other);
        }
    }

    public void SetIsAgentInTrue(Collider other)
    {
        isAgentIn = true;
        agent = other.GetComponent<AgentScript>();
    }

    public void SetIsAgentInFalse(Collider other)
    {
        isAgentIn = false;
        agent = null;
    }
}
