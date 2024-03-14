using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class FireWorkScript : MonoBehaviour
{
    public Vector3 destination;
    public GameObject fireball;
    public GameObject[] sparks;
    public GameObject[] sparkRanges;
    public float speed;
    public float tempTimer;

    // Start is called before the first frame update
    void Start()
    {
        fireball = this.transform.GetChild(0).gameObject;
        sparkRanges = new GameObject[4];
        sparks = new GameObject[4];
        speed = Academy.Instance.EnvironmentParameters.GetWithDefault("increasement", 1) * 40f;
        tempTimer = 0.0f;

        for(int i = 0; i < 4; i++)
        {
            sparkRanges[i] = this.transform.GetChild(i+1).gameObject;
            sparks[i] = sparkRanges[i].transform.GetChild(0).gameObject;
            sparks[i].SetActive(false);
            sparkRanges[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;

        if(Vector3.Distance(destination, this.transform.position) <= 0.3f || tempTimer >= 3)
        {
            StartCoroutine(Burst());
        }
         else
        {
            Vector3 dir = (destination - this.transform.position).normalized;
            this.transform.Translate(dir * Time.deltaTime * speed, Space.World);
        }
        
    }

    public void SetDestination(Vector3 dest)
    {
        destination= dest;  
    }

    IEnumerator Burst()
    {
        yield return null;
        fireball.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            sparkRanges[i].SetActive(true);
        }

        StartCoroutine(Spark());
    }

    IEnumerator Spark()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < 4; i++)
        {
            sparks[i].SetActive(true);
        }

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

}
