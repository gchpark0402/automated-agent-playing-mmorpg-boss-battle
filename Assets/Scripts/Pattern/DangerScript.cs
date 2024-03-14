using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScript : MonoBehaviour
{
    public bool isdestroy;
    // Start is called before the first frame update
    void Start()
    {
        
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
            if(isdestroy)  this.gameObject.SetActive(false);
            if(other.GetComponent<AgentScript>() != null)
            {
                if (!other.GetComponent<AgentScript>().isDamaged)
                {
                    other.GetComponent<AgentScript>().Damaged();
                }
            }
            else if(other.GetComponent<AgentCurriculumScript>() != null)
            {
                if (!other.GetComponent<AgentCurriculumScript>().isDamaged)
                {
                    other.GetComponent<AgentCurriculumScript>().Damaged();
                }
            }
           
            
        }
    }
}
