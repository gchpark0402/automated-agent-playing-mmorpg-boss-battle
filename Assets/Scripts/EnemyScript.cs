using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.MLAgents;

public class EnemyScript : MonoBehaviour
{
    private bool isDebug = true;
    [SerializeField]
    private GameObject agent;
    private Vector3 agentPos;
    private Vector3 agentLookDir;
    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private GameObject rangeAttackCollider;
    public bool isRangeAttack = false;
    public bool isChargeAttack = false;
    private float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        agentPos = agent.transform.position;
        rangeAttackCollider = this.gameObject.transform.GetChild(3).gameObject;
        rangeAttackCollider.SetActive(false);
        //일정 시간마다 패턴 진행
        InvokeRepeating("Attack", 2, 3);
        //취소시 CancleInvoke
    }

    // Update is called once per frame
    void Update()
    {
        agentPos = new Vector3(agent.transform.position.x, 
            this.transform.position.y, agent.transform.position.z) ;
        this.transform.LookAt(agentPos);
        if(isRangeAttack) { StartCoroutine(CheckRangeAttack()); }
        if (isChargeAttack) { StartCoroutine(ChargeAttack()); }
    }

    void Attack()
    {
        int randint = Random.Range(1, 4);
        DebugMsg(randint.ToString());

        switch (randint) 
        {
            case 1:
                FireBall();
                break;
            case 2:
                StartRangeAttack();
                break;
            case 3:
                StartChargeAttack();
                break;
            default: break;
        }
    }

    void FireBall()
    {
        if (fireball!= null) 
        {
            Vector3 tempVec = new Vector3(this.transform.GetChild(2).transform.position.x, 0, this.transform.GetChild(2).transform.position.z);
            Instantiate(fireball, tempVec, Quaternion.identity);
            if(fireball.GetComponent<FireBallScript>() != null)
            {
                DebugMsg("spawn fireball");
            }
        }
    }

    void StartRangeAttack()
    {
        rangeAttackCollider.SetActive(true);
        isRangeAttack = true;
        DebugMsg("start range attack...");
    }

    IEnumerator CheckRangeAttack()
    {
        yield return new WaitForSeconds(3);
        if(rangeAttackCollider != null ) 
        {
            if(rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist)
            {
                DebugMsg("range attack!");
                agent.GetComponent<AgentScript>().Damaged();
            }
        }

        isRangeAttack = false;
        rangeAttackCollider.SetActive(false);
        rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist = false;
    }

    void StartChargeAttack()
    {
       isChargeAttack= true;
       //Debug.Log("start charge attack!");
    }

    IEnumerator ChargeAttack()
    {
        yield return null;
        agentPos = new Vector3(agent.transform.position.x,
            this.transform.position.y, agent.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, agentPos, Time.deltaTime * speed);
        StartCoroutine(StopChargeAttack());
        
    }

    IEnumerator StopChargeAttack() 
    { 
        yield return new WaitForSeconds(3f);
        isChargeAttack = false;
        //Debug.Log("stop charge attack!");
        StopCoroutine(StopChargeAttack());
    }

    void DebugMsg(string msg)
    {
        if (isDebug)
            UnityEngine.Debug.Log(msg);
    }

}
