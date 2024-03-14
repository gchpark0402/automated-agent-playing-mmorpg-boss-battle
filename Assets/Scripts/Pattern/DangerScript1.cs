using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "Agent")
        {
            if (other.GetComponent<AgentScript>() != null)
            {
                if (!other.GetComponent<AgentScript>().isDamaged)
                {
                    other.GetComponent<AgentScript>().Damaged();
                }
            }
            else if (other.GetComponent<AgentCurriculumScript>() != null)
            {
                if (!other.GetComponent<AgentCurriculumScript>().isDamaged)
                {
                    other.GetComponent<AgentCurriculumScript>().Damaged();
                }
            }

            //Debug.Log("agent damaged!");
        }
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(1);
        if(this.transform.parent!= null)
            Destroy(this.transform.parent.gameObject);
        Destroy(gameObject);
    }
}
