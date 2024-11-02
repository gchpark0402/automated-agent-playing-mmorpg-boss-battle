using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AgentAttackScript : MonoBehaviour
{
    public AgentScript agent;
    public float lifetime;
    public bool isAttack = false;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        if(this.transform.parent != null)  agent = this.transform.parent.GetComponent<AgentScript>();
        StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Boss")
        {
            if (!isAttack)
            {
                if(other.GetComponent<EnemyScript1>() != null)
                {
                    other.GetComponent<EnemyScript1>().Damaged(damage);
                    if (agent != null) agent.SetReward(2.3f + Mathf.Log(100 - other.GetComponent<EnemyScript1>().HP));
                    other.GetComponent<EnemyScript1>().Hit(other.bounds.ClosestPoint(transform.position));
                }
                else if (other.GetComponent<EnemyScript2>() != null)
                {
                    other.GetComponent<EnemyScript2>().Damaged(damage);
                    if (agent != null) agent.SetReward(2.3f + Mathf.Log(100 - other.GetComponent<EnemyScript2>().HP));
                }
                
                isAttack = true;
                //Debug.Log("attack!");
                //other.GetComponent<EnemyScript1>().Hit(other.bounds.ClosestPoint(transform.position));
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Boss")
        {
            if(!isAttack)
            {
                if (other.GetComponent<EnemyScript1>() != null)
                {
                    other.GetComponent<EnemyScript1>().Damaged(damage);
                    other.GetComponent<EnemyScript1>().Hit(other.ClosestPoint(this.transform.position));
                    if (agent != null) agent.SetReward(2.3f + Mathf.Log(100 - other.GetComponent<EnemyScript1>().HP));
                }
                else if (other.GetComponent<EnemyScript2>() != null)
                {
                    other.GetComponent<EnemyScript2>().Damaged(damage);
                    if (agent != null) agent.SetReward(2.3f + Mathf.Log(100 - other.GetComponent<EnemyScript2>().HP));
                }
                isAttack = true;
                //Debug.Log("attack!");
                
            }
            
        }
    }


    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
