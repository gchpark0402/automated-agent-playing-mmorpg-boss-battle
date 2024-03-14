using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;

using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using Unity.MLAgents;
using System;

using Random = UnityEngine.Random;

public class EnemyScript1 : MonoBehaviour
{
    private bool isDebug = false;

    public bool isAttacking = false;

    [SerializeField]
    private GameObject agent;
    private Vector3 agentPos;
    private Vector3 chargeDestination;

    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private GameObject meteor;
    [SerializeField]
    private GameObject swirl;
    [SerializeField]
    private GameObject chargeAttackCollider;
    private GameObject chargeColliderTemp;
    [SerializeField]
    private GameObject breathPrefab;
    [SerializeField]
    private GameObject earthQuake;
    [SerializeField]
    private GameObject fireWork;
    [SerializeField]
    private GameObject pizza;
    [SerializeField]
    private GameObject crash;
    [SerializeField]
    private GameObject crashOneWay;
    [SerializeField]
    private GameObject crack;
    [SerializeField]
    private GameObject frontAttack1;
    [SerializeField]
    private GameObject frontAttack2;
    [SerializeField]
    private GameObject leftRight;
    [SerializeField]
    private GameObject sideAttack;
    [SerializeField]
    private GameObject hitEffect;
    [SerializeField]
    private GameObject breathEffectPrefab;
    [SerializeField]
    private Transform headPos;
    [SerializeField]
    private GameObject rangeAttackEffect;
    [SerializeField]
    private GameObject chargeAttackEffect;

    private GameObject breath;
    private GameObject breathEffect;
    private EnemySoundScript audioController;

    private GameObject rangeAttackCollider;
    private Collider meteorSpawnArea;
    public int attackType;

    public bool isChargeAttack = false;
    public bool beforeBreath = false;
    public bool isBreath = false;

    public bool isLooking = false;
    private float lookingTemp = 0.0f;

    public bool checkBossCollider = false;
    private float speed = 6.5f;
    private Quaternion targetRotation;
    public int HP = 50;
    public int patternCountStart = 1;
    public int patternCountEnd = 12;

    [SerializeField]
    private Transform area;
    private Vector3 leftFoot;
    private Vector3 rightFoot;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agentPos = agent.transform.position;
        rangeAttackCollider = this.gameObject.transform.GetChild(2).gameObject;
        meteorSpawnArea = this.gameObject.transform.GetChild(3).gameObject.GetComponent<BoxCollider>();

        leftFoot = this.transform.GetChild(4).position;
        rightFoot= this.transform.GetChild(5).position;

        rangeAttackCollider.SetActive(false);
        HP = 100;

        animator = this.GetComponent<Animator>();
        audioController = this.transform.GetChild(6).GetComponent<EnemySoundScript>();
        //일정 시간마다 패턴 진행
        InvokeRepeating("Attack", 2, 1);
        //취소시 CancleInvoke
    }

    // Update is called once per frame
    void Update()
    {
        agentPos = new Vector3(agent.transform.position.x, 
            this.transform.position.y, agent.transform.position.z) ;

        if(Vector3.Distance(agentPos, this.transform.position) >= 0.01)
            targetRotation = Quaternion.LookRotation(agentPos - transform.position);

        if(isLooking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            lookingTemp += Time.deltaTime;
            if(lookingTemp > 2)
            {
                isLooking = false;
                lookingTemp= 0;
            }
        }

        if (isChargeAttack) 
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, chargeDestination, Time.deltaTime * speed);
        }

        if(beforeBreath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                transform.rotation * Quaternion.Euler(new Vector3(0, -70, 0)), speed * Time.deltaTime );
        }
        if(isBreath)
        {
            if (breath != null)
                this.transform.LookAt(breath.transform.GetChild(0).transform.GetChild(2).position);
            if(breathEffect != null)
            {
                breathEffect.transform.LookAt(breath.transform.GetChild(0).transform.GetChild(1).position);
                breathEffect.transform.position = headPos.position;
            }
        }

    }


    void Attack()
    {
        if (!isAttacking)
        {
            isLooking = true;
            attackType = Random.Range(patternCountStart, patternCountEnd);
            DebugMsg(attackType.ToString());

            switch (attackType)
            {
                case 1:
                    Meteor1();
                    break;
                case 2:
                    Fireball();
                    break;
                case 3:
                    StartChargeAttack();
                    break;
                case 4:
                    Breath();
                    break;
                case 5:
                    FireWork();
                    break;
                case 6:
                    PizzaQuake();
                    break;
                case 7:
                    Crash();
                    break;
                case 8:
                    StartRangeAttack();  
                    break;
                case 9:
                    Crack();
                    break;
                case 10:
                    FastCrash();
                    break;
                case 11:
                    LeftRight();
                    break;
                default: break;
            }

            isAttacking= true;
        }
    }

    void FrontAttack(float starttime, float delay)
    {
        if (Vector3.Distance(agentPos, this.transform.position) <= 20)
        {
            if(Random.Range(1, 20) <= 10)
            {
                StartCoroutine(FrontAttack2(starttime));
            }
            else
                StartCoroutine(StopAttack(delay));
        }
        else
        {
            if (Random.Range(1, 20) <= 5)
            {
                StartCoroutine(FrontAttack2(starttime));
            }
            else
                StartCoroutine(StopAttack(delay));
        }
            
    }

    IEnumerator FrontAttack2(float starttime)
    {
        yield return new WaitForSeconds(starttime);
        yield return new WaitForSeconds(1);


        Vector3 tempVec = this.transform.position + this.transform.forward * 5;
        Instantiate(frontAttack1, tempVec, Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));
        animator.SetBool("isMeleeAttack", true);
        audioController.PlaySound(2, 1);
        yield return new WaitForSeconds(1);

        audioController.PlaySound(2, 1);
        Instantiate(frontAttack2, tempVec, Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));

        StartCoroutine(StopAttack(1));

        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isMeleeAttack", false);
    }




    void Fireball()
    {
        animator.SetBool("isAttack", true);
        audioController.PlaySound(4, 1);
        //this.transform.LookAt(agentPos);
        isLooking = true;

        //fireball 개수
        int fireballCount = 10;

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 6 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 4);
            if(randBool != 0)   Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -6 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 4);
            if (randBool != 0) Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }

        StartCoroutine(FireBall2());
    }

    IEnumerator FireBall2()
    {
        yield return new WaitForSeconds(0.7f);

        animator.SetBool("isAttack", true);
        audioController.PlaySound(4, 1);
        isLooking = true;


        //fireball 개수
        int fireballCount = 10;

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }

        yield return new WaitForSeconds(0.7f);

        animator.SetBool("isAttack", true);
        audioController.PlaySound(4, 1);
        isLooking = true;

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position + new Vector3(0, 1f, 0), Quaternion.Euler(tempVec));
        }

        FrontAttack(0f, 3f);
        //StartCoroutine(StopAttack(3f));
    }
    
    void StartRangeAttack()
    {
        rangeAttackCollider.SetActive(true);

        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        audioController.PlaySound(8, 1);
        DebugMsg("start range attack...");
        StartCoroutine(RangeAttack());
    }

    IEnumerator RangeAttack()
    {
        yield return new WaitForSeconds(1.5f);
        if(rangeAttackCollider != null ) 
        {
            if(rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist)
            {
                DebugMsg("range attack!");
                agent.GetComponent<AgentScript>().Damaged();
                StartCoroutine(RangeAttackEffect());
            }
        }

        rangeAttackCollider.SetActive(false);
        rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist = false;
        FrontAttack(0, 1);
        //StartCoroutine(StopAttack(1));
    }

    IEnumerator RangeAttackEffect()
    {
        yield return null;
        GameObject raEffect = Instantiate(rangeAttackEffect, this.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(2);
        Destroy(raEffect);
    }

    void Meteor1()
    {
        Vector3 tempVec = area.transform.position;
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        StartCoroutine(Meteor3());

        //메테오 개수
        int meteorCount = 4;

        for (int i = 0; i < meteorCount; i++)
        {
            for(int j = 0; j < meteorCount; j++)
            {
                tempVec.x = Random.Range(-20, 20) + area.transform.position.x;
                tempVec.z = Random.Range(-20, 20) + area.transform.position.z;
                Instantiate(meteor, tempVec, Quaternion.identity);
                
            }
        }

        Instantiate(meteor, agentPos, Quaternion.identity);

        StartCoroutine(Meteor2());
        
    }

    IEnumerator Meteor2()
    {
        yield return new WaitForSeconds(4.3f);
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        Vector3 tempVec = area.transform.position;

        //메테오 개수
        int meteorCount = 4;

        for (int i = 0; i < meteorCount; i++)
        {
            for (int j = 0; j < meteorCount; j++)
            {
                tempVec.x = Random.Range(-15, 15) + area.transform.position.x;
                tempVec.z = Random.Range(-15, 15) + area.transform.position.z;
                Instantiate(meteor, tempVec, Quaternion.identity);

            }
        }


        Instantiate(meteor, agentPos, Quaternion.identity);

        FrontAttack(4, 4);
        //StartCoroutine(StopAttack(4));
    }

    IEnumerator Meteor3()
    {
        yield return null;

        //메테오 개수
        int meteorCount = 5;

        for (int i = 0; i < meteorCount; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(meteor, agentPos, Quaternion.identity);
        }
    }

    void Breath()
    {
        animator.SetBool("isFlyAttack", true);
        audioController.PlaySound(0, 1);
        beforeBreath = true;
        Vector3 relativePosition = agentPos - transform.position;
        float targetRotationAngle = Mathf.Atan2(relativePosition.x, relativePosition.z) * Mathf.Rad2Deg;
        breath = Instantiate(breathPrefab, this.transform.position, Quaternion.Euler(0, targetRotationAngle, 0));
        breathEffect = Instantiate(breathEffectPrefab, headPos.position, Quaternion.identity);
        breathEffect.transform.localScale = new Vector3(5, 3, 3);
        breathEffect.SetActive(false);
        StartCoroutine(StopBreath());
    }

    IEnumerator StopBreath()
    {
        yield return new WaitForSeconds(1);
        beforeBreath = false;
        animator.SetBool("isFlyAttack", true);
        audioController.PlaySound(3, 1);
        audioController.PlaySound(1, 1);
        isBreath = true;
        breathEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        audioController.PlaySound(3, 1);
        yield return new WaitForSeconds(1.15f);
        audioController.PlaySound(3, 1);
        isBreath = false;
        Destroy(breathEffect);

        FrontAttack(0, 1);
        //StartCoroutine(StopAttack(1));
    }

    void FireWork()
    {
        animator.SetBool("isAttack", true);
        audioController.PlaySound(4, 1);
        audioController.PlaySound(9, .5f);
        Instantiate(fireWork, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);
        StartCoroutine(FireWork2());
    }

    IEnumerator FireWork2()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttack", true);
        audioController.PlaySound(0, 1);
        audioController.PlaySound(9, .5f);
        isLooking = true;
        Instantiate(fireWork, this.transform.position, Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttack", true);
        audioController.PlaySound(4, 1);
        audioController.PlaySound(9, .5f);
        isLooking = true;
        Instantiate(fireWork, this.transform.position, Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);

        FrontAttack(0, 3.5f);
        //StartCoroutine(StopAttack(3.5f));
    }

    void PizzaQuake()
    {
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        Instantiate(pizza, this.transform.position, Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));
        FrontAttack(2, 4);
        //StartCoroutine(StopAttack(4));
    }

    /*void EarthQuake()
    {
        animator.SetBool("isScream", true);
        leftFoot = this.transform.GetChild(5).position;
        rightFoot = this.transform.GetChild(6).position;

        Instantiate(earthQuake,leftFoot, Quaternion.identity);

        //StartCoroutine(EarthQuake2());
        StartCoroutine(StopAttack(5));
    }

    IEnumerator EarthQuake2()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("isScream", true);
        Instantiate(earthQuake, rightFoot, Quaternion.identity);
        StartCoroutine(StopAttack(5));
    }*/

    void Crash()
    {
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        audioController.PlaySound(2, 1);
        leftFoot = this.transform.GetChild(4).position;
        rightFoot = this.transform.GetChild(5).position;
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y + 20, 0);

        Instantiate(crash, leftFoot, Quaternion.Euler(tempVec));

        StartCoroutine(Crash2());
    }

    IEnumerator Crash2()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("isScream", true);
        audioController.PlaySound(2, 1);
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y + 20, 0);
        Instantiate(crash, rightFoot, Quaternion.Euler(tempVec));

        yield return new WaitForSeconds(1.5f);
        tempVec = new Vector3(0, this.transform.eulerAngles.y + 120, 0);
        Instantiate(sideAttack, this.transform.position, Quaternion.Euler(tempVec));

        audioController.PlaySound(9, .5f);
        FrontAttack(0.1f, 2f);
    }

    

    void FastCrash()
    {
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        StartCoroutine(_FastCrash());
        
    }

    IEnumerator _FastCrash()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(crashOneWay, this.transform.position, Quaternion.Euler(tempVec));
        audioController.PlaySound(2, 1);

        yield return new WaitForSeconds(1f);
        tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(crashOneWay, this.transform.position, Quaternion.Euler(tempVec));
        audioController.PlaySound(2, 1);

        yield return new WaitForSeconds(0.5f);
        FireWork();
    }

    void StartChargeAttack()
    {
        isLooking = false;
        chargeDestination = agentPos;
        this.transform.LookAt(chargeDestination);
        chargeColliderTemp = Instantiate(chargeAttackCollider, this.transform.position,
            Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));
        chargeColliderTemp.transform.position =
            chargeColliderTemp.transform.position + chargeColliderTemp.transform.forward * 12;
        audioController.PlaySound(0, 1);
        StartCoroutine(StopChargeAttack());
    }

    IEnumerator StopChargeAttack()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject tempEffect = Instantiate(chargeAttackEffect, this.transform.position, Quaternion.identity);
        tempEffect.transform.parent= this.transform;

        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isWalk", true);
        checkBossCollider = true;
        isChargeAttack = true;
        audioController.PlaySound(0, 1);
        audioController.PlaySound(10, 1);
        StartCoroutine(StopAttack(2.5f));

        yield return new WaitForSeconds(2.5f);
        Destroy(chargeColliderTemp);
        checkBossCollider = false;
        chargeColliderTemp = null;
        isChargeAttack = false;
        Destroy(tempEffect);

    }

    void Crack()
    {
        int i = 1;
        StartCoroutine(StartCrack(i));
    }

    IEnumerator StartCrack(int i)
    {
        float crackDelay = 0.5f;

        yield return new WaitForSeconds(crackDelay);

        audioController.PlaySound(2, 1);
        if (i <= 6)
        {
            float length = 5 * i * Mathf.Pow(-1, i);
            float angle = 60;
            

            Vector3 tempVec1 = this.transform.position + this.transform.forward * length;
            Vector3 tempVec2 = this.transform.position - this.transform.right * length - this.transform.forward * length;
            Vector3 tempVec3 = this.transform.position + this.transform.right * length - this.transform.forward * length;

            Instantiate(crack, tempVec1, Quaternion.Euler(new Vector3(0, 90 + Random.Range(-60, 60), 0))).transform.localScale = new Vector3(10 * i, 1, 2);
            Instantiate(crack, tempVec2, Quaternion.Euler(new Vector3(0, angle + Random.Range(-60, 60), 0))).transform.localScale = new Vector3(10 * i, 1, 2);
            Instantiate(crack, tempVec3, Quaternion.Euler(new Vector3(0, -angle + Random.Range(-60, 60), 0))).transform.localScale = new Vector3(10 * i, 1, 2);

            i += 1;
            StartCoroutine(StartCrack(i));
        }
        else
        {
            FrontAttack(0, 3);
        }


    }

    void LeftRight()
    {
        animator.SetBool("isScream", true);
        audioController.PlaySound(0, 1);
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(leftRight, this.transform.position, Quaternion.Euler(tempVec));

        FrontAttack(4f, 4f);
    }

    public void Damaged(int i)
    {
        HP -= i;
        audioController.PlaySound(5, 0.3f);
        audioController.PlaySound(6, 0.3f);
        audioController.PlaySound(7, 0.2f);
    }

    void DebugMsg(string msg)
    {
        if (isDebug)
            UnityEngine.Debug.Log(msg);
    }

    IEnumerator StopAttack(float second)
    {
        yield return new WaitForSeconds(second);
        

        animator.SetBool("isScream", false);
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalk", false);
        animator.SetBool("isFlyAttack", false);
        animator.SetBool("isMeleeAttack", false);
        isAttacking = false;
    }

    public void Hit(Vector3 hitpos)
    {
        UnityEngine.Debug.Log("hit!");
        StartCoroutine(HitEffect(hitpos));
    }
    IEnumerator HitEffect(Vector3 hitpos)
    {
        yield return null;
        Vector3 tempPos = new Vector3(hitpos.x, 1, hitpos.z);
        GameObject tempEffect = Instantiate(hitEffect, tempPos, Quaternion.identity);
        tempEffect.transform.localScale = new Vector3(3, 3, 3);
        
        yield return new WaitForSeconds(.7f);
        Destroy(tempEffect);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(checkBossCollider)
        {
            if(other.tag == "Agent")
            {
                other.gameObject.GetComponent<AgentScript>().Damaged();
            }
        }
    }
}
