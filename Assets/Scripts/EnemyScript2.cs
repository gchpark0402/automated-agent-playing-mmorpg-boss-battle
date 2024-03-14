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
using System.Security.Cryptography;

public class EnemyScript2 : MonoBehaviour
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
    private GameObject breath;
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
    public float difficulty;
    public float increasement;
    public float decreasement;

    [SerializeField]
    private Transform area;
    private Vector3 leftFoot;
    private Vector3 rightFoot;

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agentPos = agent.transform.position;
        rangeAttackCollider = this.gameObject.transform.GetChild(3).gameObject;
        meteorSpawnArea = this.gameObject.transform.GetChild(4).gameObject.GetComponent<BoxCollider>();

        leftFoot = this.transform.GetChild(5).position;
        rightFoot = this.transform.GetChild(6).position;

        rangeAttackCollider.SetActive(false);
        HP = 100;
        difficulty = Academy.Instance.EnvironmentParameters.GetWithDefault("difficulty", 2);
        increasement = Academy.Instance.EnvironmentParameters.GetWithDefault("increasement1", 1);
        decreasement = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement1", 1);
        animator = this.GetComponent<Animator>();

        //일정 시간마다 패턴 진행
        InvokeRepeating("Attack", 2, 1);
        //취소시 CancleInvoke
    }

    // Update is called once per frame
    void Update()
    {
        agentPos = new Vector3(agent.transform.position.x,
            this.transform.position.y, agent.transform.position.z);

        if (Vector3.Distance(agentPos, this.transform.position) >= 0.01)
            targetRotation = Quaternion.LookRotation(agentPos - transform.position);

        if (isLooking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

            lookingTemp += Time.deltaTime;
            if (lookingTemp > 2)
            {
                isLooking = false;
                lookingTemp = 0;
            }
        }

        if (isChargeAttack)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, chargeDestination, Time.deltaTime * speed);
        }

        if (beforeBreath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                transform.rotation * Quaternion.Euler(new Vector3(0, -70, 0)), speed * Time.deltaTime);
        }
        if (isBreath)
        {
            this.transform.Rotate(new Vector3(0, Time.deltaTime * 50f, 0));
        }

    }


    void Attack()
    {
        if (!isAttacking)
        {
            isLooking = true;
            attackType = Random.Range(1, 12);
            increasement = Academy.Instance.EnvironmentParameters.GetWithDefault("increasement", 1);
            decreasement = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement", 1);

            DebugMsg(attackType.ToString());

            switch (attackType)
            {
                case 1:
                    StartChargeAttack(); //1
                    break;
                case 2:
                    StartRangeAttack(); //2
                    break;
                case 3:
                    Breath(); //3
                    break;
                case 4:
                    PizzaQuake(); //4
                    break;
                case 5:
                    Crack(); //5
                    break;
                case 6:
                    Crash(); //6
                    break;
                case 7:
                    LeftRight(); //7
                    break;
                case 8:
                    FastCrash(); //8
                    break;
                case 9:
                    FireWork(); //9
                    break;
                case 10:
                    Fireball();//10
                    break;
                case 11:
                    Meteor1();//11
                    break;
                default: break;
            }

            isAttacking = true;
        }
    }


    void FrontAttack(float starttime, float delay)
    {
        if (Vector3.Distance(agentPos, this.transform.position) <= 20)
        {
            if (Random.Range(1, 20) <= 10)
            {
                StartCoroutine(FrontAttack2(starttime + decreasement - 1));
            }
            else
                StartCoroutine(StopAttack(delay + decreasement - 1));
        }
        else
        {
            if (Random.Range(1, 20) <= 5)
            {
                StartCoroutine(FrontAttack2(starttime + decreasement - 1));
            }
            else
                StartCoroutine(StopAttack(delay + decreasement - 1));
        }

    }

    IEnumerator FrontAttack2(float starttime)
    {
        yield return new WaitForSeconds(starttime);
        yield return new WaitForSeconds(1);


        Vector3 tempVec = this.transform.position + this.transform.forward * 5;
        Instantiate(frontAttack1, tempVec, Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));
        animator.SetBool("isMeleeAttack", true);

        yield return new WaitForSeconds(1);

        Instantiate(frontAttack2, tempVec, Quaternion.Euler(new Vector3(0, this.transform.eulerAngles.y, 0)));

        StartCoroutine(StopAttack(1));

        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isMeleeAttack", false);
    }




    void Fireball()
    {
        animator.SetBool("isAttack", true);

        //this.transform.LookAt(agentPos);
        isLooking = true;

        //fireball 개수
        int fireballCount = (int)(10 * increasement);

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 6 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 4);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -6 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 4);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }

        StartCoroutine(FireBall2());
    }

    IEnumerator FireBall2()
    {
        yield return new WaitForSeconds(0.7f);

        animator.SetBool("isAttack", true);
        isLooking = true;


        //fireball 개수
        int fireballCount = (int)(10 * increasement);

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }

        yield return new WaitForSeconds(0.7f);

        animator.SetBool("isAttack", true);
        isLooking = true;

        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, 8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }
        for (int i = 0; i < fireballCount; i++)
        {
            Vector3 tempVec = new Vector3(0, -8 * i + this.transform.eulerAngles.y, 0);
            int randBool = Random.Range(0, 3);
            if (randBool != 0) Instantiate(fireball, this.transform.position, Quaternion.Euler(tempVec));
        }

        FrontAttack(0f, 3f);
        //StartCoroutine(StopAttack(3f));
    }

    void StartRangeAttack()
    {
        rangeAttackCollider.SetActive(true);

        animator.SetBool("isScream", true);
        DebugMsg("start range attack...");
        StartCoroutine(RangeAttack());
    }

    IEnumerator RangeAttack()
    {
        yield return new WaitForSeconds(1.5f);
        if (rangeAttackCollider != null)
        {
            if (rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist)
            {
                DebugMsg("range attack!");
                agent.GetComponent<AgentCurriculumScript>().Damaged();
            }
        }

        rangeAttackCollider.SetActive(false);
        rangeAttackCollider.GetComponent<RangeAttackCheck>().isAgentExist = false;
        FrontAttack(0, 1);
        //StartCoroutine(StopAttack(1));
    }


    void Meteor1()
    {
        Vector3 tempVec = area.transform.position;
        animator.SetBool("isScream", true);
        StartCoroutine(Meteor3());

        //메테오 개수
        int meteorCount = (int)(4 * increasement);

        for (int i = 0; i < meteorCount; i++)
        {
            for (int j = 0; j < meteorCount; j++)
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
        Vector3 tempVec = area.transform.position;

        //메테오 개수
        int meteorCount = (int)(4 * increasement);

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
        int meteorCount = (int)(5 * increasement);

        for (int i = 0; i < meteorCount; i++)
        {
            yield return new WaitForSeconds(1);
            Instantiate(meteor, agentPos, Quaternion.identity);
        }
    }

    void Breath()
    {
        animator.SetBool("isFlyAttack", true);
        beforeBreath = true;
        Vector3 relativePosition = agentPos - transform.position;
        float targetRotationAngle = Mathf.Atan2(relativePosition.x, relativePosition.z) * Mathf.Rad2Deg;
        Instantiate(breath, this.transform.position, Quaternion.Euler(0, targetRotationAngle, 0));
        StartCoroutine(StopBreath());
    }

    IEnumerator StopBreath()
    {
        yield return new WaitForSeconds(1+decreasement);
        beforeBreath = false;
        animator.SetBool("isFlyAttack", true);
        isBreath = true;
        yield return new WaitForSeconds(1 + decreasement);
        isBreath = false;

        FrontAttack(0, 1);
        //StartCoroutine(StopAttack(1));
    }

    void FireWork()
    {
        animator.SetBool("isAttack", true);
        Instantiate(fireWork, this.transform.position, Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);
        StartCoroutine(FireWork2());
    }

    IEnumerator FireWork2()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttack", true);
        isLooking = true;
        Instantiate(fireWork, this.transform.position, Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isAttack", true);
        isLooking = true;
        Instantiate(fireWork, this.transform.position, Quaternion.identity).GetComponent<FireWorkScript>().SetDestination(agentPos);

        FrontAttack(0, 3.5f);
        //StartCoroutine(StopAttack(3.5f));
    }

    void PizzaQuake()
    {
        animator.SetBool("isScream", true);
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
        leftFoot = this.transform.GetChild(5).position;
        rightFoot = this.transform.GetChild(6).position;
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y + 10, 0);

        Instantiate(crash, leftFoot, Quaternion.Euler(tempVec));

        StartCoroutine(Crash2());
    }

    IEnumerator Crash2()
    {
        yield return new WaitForSeconds(1+decreasement-1);
        animator.SetBool("isScream", true);
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y - 10, 0);
        Instantiate(crash, rightFoot, Quaternion.Euler(tempVec));

        yield return new WaitForSeconds(1.5f+decreasement-1);
        tempVec = new Vector3(0, this.transform.eulerAngles.y + 120, 0);
        Instantiate(sideAttack, this.transform.position, Quaternion.Euler(tempVec));

        FrontAttack(0.1f, 2f);
    }



    void FastCrash()
    {
        animator.SetBool("isScream", true);
        StartCoroutine(_FastCrash());

    }

    IEnumerator _FastCrash()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(crashOneWay, this.transform.position, Quaternion.Euler(tempVec));

        yield return new WaitForSeconds(1f);
        tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(crashOneWay, this.transform.position, Quaternion.Euler(tempVec));

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
        StartCoroutine(StopChargeAttack());
    }

    IEnumerator StopChargeAttack()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isWalk", true);
        checkBossCollider = true;
        isChargeAttack = true;
        StartCoroutine(StopAttack(2.5f));
        yield return new WaitForSeconds(2.5f);
        Destroy(chargeColliderTemp);
        checkBossCollider = false;
        chargeColliderTemp = null;
        isChargeAttack = false;

    }

    void Crack()
    {
        int i = 1;
        StartCoroutine(StartCrack(i));
    }

    IEnumerator StartCrack(int i)
    {
        float crackDelay = 0.5f * decreasement;

        yield return new WaitForSeconds(crackDelay);

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
        Vector3 tempVec = new Vector3(0, this.transform.eulerAngles.y, 0);
        Instantiate(leftRight, this.transform.position, Quaternion.Euler(tempVec));

        FrontAttack(4f, 4f);
    }

    public void Damaged(int i)
    {
        HP -= i;
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

    private void OnTriggerEnter(Collider other)
    {
        if (checkBossCollider)
        {
            if (other.tag == "Agent")
            {
                other.gameObject.GetComponent<AgentCurriculumScript>().Damaged();
            }
        }
    }
}
