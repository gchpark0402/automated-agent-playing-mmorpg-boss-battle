using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;


public class AgentCurriculumScript : Agent
{
    private bool isDebug = false;

    public int number;

    private Rigidbody agentRb;
    public GameObject Boss;
    private GameObject attackCollider;
    public GameObject area;
    public GameObject slashEffect;
    [SerializeField]
    private GameObject attack1;
    [SerializeField] 
    private GameObject attack2;

    public Vector3 areaCenter;

    public bool isAttackable = false;
    public CharacterClass characterClass = CharacterClass.Warrior;

    public bool inDanger = false;
    private Collider tempCol = null;

    public bool isDamaged = false;
    public float invincibilityTime = 1.0f;
    public float[] attackCooltimeArr = new float[2] { 0.0f, 0.0f };
    public int HP;

    public float speed = 0.00000005f;
    public float turnSpeed = 300;
    public float attackRange;
    public float dist;

    Vector3 nextMove;

    public override void Initialize()
    {
        agentRb = this.GetComponent<Rigidbody>();
        agentRb.velocity = new Vector3(0, 0, 0);
        attackCollider = transform.GetChild(2).gameObject;
        HP = 5;
        areaCenter = area.transform.position;

        switch(characterClass)
        {
            case CharacterClass.Warrior:
                attackRange = 15;
                break;
            case CharacterClass.Magician:
                attackRange = 30;
                break;
            default:
                break;
        }
    }
    
   
    public override void OnEpisodeBegin()
    {
        gameObject.transform.position = areaCenter + new Vector3(0, 0, 10);
        agentRb = this.GetComponent<Rigidbody>();
        agentRb.velocity = new Vector3(0, 0, 0);
        Boss.transform.position = areaCenter + new Vector3(0, 0, -10);
        HP = 5;
        isAttackable = false;
        isDamaged = false;
        inDanger = false;
        invincibilityTime = 1.0f;
        Boss.GetComponent<EnemyScript2>().HP = 100;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var localVelocity = transform.InverseTransformDirection(agentRb.velocity);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
        sensor.AddObservation(Boss.transform.position - this.transform.position);
        sensor.AddObservation(Boss.GetComponent<EnemyScript2>().HP);
        sensor.AddObservation(Boss.GetComponent<EnemyScript2>().isChargeAttack);
        sensor.AddObservation(inDanger);
        sensor.AddObservation(isAttackable);
        sensor.AddObservation(attackCooltimeArr);
        sensor.AddObservation(HP);
        sensor.AddObservation(Boss.GetComponent<EnemyScript2>().attackType);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions);

        if(HP <= 0) 
        { 
            SetReward(-10);
            //GameController.Instance.ResetScene(number);
            EndEpisode();

        }

        if(Boss.GetComponent<EnemyScript2>().HP <= 0)
        {
            DebugMsg("win!");
            SetReward(+10);
            //GameController.Instance.ResetScene(number);
            EndEpisode();
        }

        if(!inDanger)
        {
            SetReward(0.03f);
        }

        CheckAttackable();
        if (isAttackable) { SetReward(0.05f); }
        else { SetReward(0.01f); }

        if(attackCooltimeArr[0] == 0 || attackCooltimeArr[1] == 0)
        {
            SetReward(-0.005f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isDamaged == false)
        {
            if (other.transform.tag == "Danger")
            {
                inDanger = true;
                tempCol = other;
                SetReward(-0.7f);
            }
        }

        if(other.transform.tag == "Wall")
        {
            SetReward(-20);
            //GameController.Instance.ResetScene(number);
            EndEpisode();
        }
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Danger")
        {
            inDanger = true;
            tempCol = other;
            SetReward(-0.15f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Danger")
        {
            inDanger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move();
        if(isDamaged)
        {
            invincibilityTime -= Time.deltaTime;
            if(invincibilityTime <= 0)
            {
                invincibilityTime= 1;
                isDamaged= false;
            }
        }

        if(inDanger)
        {
            if (tempCol == null  || !tempCol.enabled)  inDanger = false;
        }
     }

    public void MoveAgent(ActionSegment<int> act)
    {
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        var dirToGoForwardAction = act[0];
        var rotateDirAction = act[1];
        var dirToGoSideAction = act[2];
        var attackAction = act[3];


        if (dirToGoForwardAction == 1)
            dirToGo = 1f * transform.forward;
        else if (dirToGoForwardAction == 2)
            dirToGo = -1f * transform.forward;
        if (rotateDirAction == 1)
            rotateDir = transform.up * -1f;
        else if (rotateDirAction == 2)
            rotateDir = transform.up * 1f;
        if (dirToGoSideAction == 1)
            dirToGo = -0.6f * transform.right;
        else if (dirToGoSideAction == 2)
            dirToGo = 0.6f * transform.right;
        if (attackAction == 1)
        {
            if (attackCooltimeArr[0] <= 0f && isAttackable)
            {
                Attack1();
            }
        }
        else if (attackAction == 2)
        {
            if (attackCooltimeArr[1] <= 0f && isAttackable)
            {
                Attack2();
            }
        }


        transform.Rotate(rotateDir, Time.fixedDeltaTime * 300f);
        agentRb.AddForce(dirToGo * speed, ForceMode.VelocityChange);

        if (this.transform.position.y <= -1 || 
            this.transform.position.x >= areaCenter.x + 25 ||
             this.transform.position.x <= areaCenter.x - 25 ||
              this.transform.position.z >= areaCenter.z + 25 ||
               this.transform.position.z <= areaCenter.z - 25)
        {
            SetReward(-20);
            DebugMsg("out of area");
            //GameController.Instance.ResetScene(number);
            EndEpisode();
        }

        attackCooltimeArr[0] -= Time.fixedDeltaTime;
        attackCooltimeArr[1] -= Time.fixedDeltaTime;
        /* float moveZ = 0f;
         float moveX = 0f;

         if (Input.GetKey(KeyCode.W))
         {
             moveZ += 1f;
         }


         if (Input.GetKey(KeyCode.S))
         {
             moveZ -= 1f;
         }


         if (Input.GetKey(KeyCode.A))
         {
             moveX -= 1f;
         }


         if (Input.GetKey(KeyCode.D))
         {
             moveX += 1f;
         }

         agentRb.AddForce(new Vector3(moveX, 0f, moveZ) * 10);
         //transform.Translate(new Vector3(moveX, 0f, moveZ) * 0.1f);*/


    }

    void CheckAttackable()
    {
        float dist = Vector3.Distance(this.transform.position, Boss.transform.position);

        if (dist <= attackRange) { isAttackable = true; }
        else { isAttackable = false; }
    }

    void Attack1()
    {
        DebugMsg("attack!");
        Instantiate(attack1, this.transform.position, Quaternion.identity).transform.parent = this.transform;
        isAttackable = false;
        attackCooltimeArr[0] = 2;
    }

    void Attack2() 
    {
        DebugMsg("attack!");
        Instantiate(attack2, this.transform.position, Quaternion.identity).transform.parent = this.transform;
        isAttackable = false;
        attackCooltimeArr[1] = 3;
    }

    IEnumerator StopAttack(int i)
    {
        yield return null;

        isAttackable = false;
        attackCooltimeArr[i] = 2.0f;
    }

    void DebugMsg(string msg)
    {
        if (isDebug) UnityEngine.Debug.Log(msg);
    }

    public void Damaged()
    {
        SetReward(-1.7f);
        isDamaged = true;
        HP -= 1;
        DebugMsg("damaged!");
       /* if(Boss.GetComponent<EnemyScript2>() != null)
        {
            Boss.GetComponent<EnemyScript2>().SetReward(1);
        }*/
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        discreteActionsOut[3] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }


}
