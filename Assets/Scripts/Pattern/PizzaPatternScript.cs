using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class PizzaPatternScript : MonoBehaviour
{
    public GameObject[] cone;
    public int temp = 0;
    public float speed = 0.3f;

    private void Start()
    {
        cone = new GameObject[this.transform.childCount-2];
        speed = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement", 1) * 0.3f;
        for (int i = 0; i < cone.Length; i++)
        {
            cone[i] = this.transform.GetChild(i+2).gameObject;
            cone[i].SetActive(false);
        }

        StartCoroutine(StartPattern1());

    }

    IEnumerator StartPattern1()
    {
        yield return new WaitForSeconds(1);

        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);
        cone[0].SetActive(true);
        cone[6].SetActive(true);
        temp++;
        StartCoroutine(StartPattern2(temp));

    }

    IEnumerator StartPattern2(int i)
    {
        yield return new WaitForSeconds(speed);
        if(i <= 5)
        {
            this.GetComponent<AudioSource>().Play();
            cone[i].SetActive(true);
            cone[i+6].SetActive(true);
            cone[i - 1].SetActive(false);
            cone[i + 5].SetActive(false);
            temp++;
            StartCoroutine(StartPattern2(temp));
        }
        else
        {
            StartCoroutine(DestroyObj());
        }    
        
    }
/*
    IEnumerator StopPattern1()
    {
        yield return new WaitForSeconds(1.7f);


        for (int i = 0; i < this.transform.childCount; i += 2)
        {
            cone[i].SetActive(false);
        }

        StartCoroutine(StartPattern2());
        
    }

    IEnumerator StopPattern2()
    {
        yield return new WaitForSeconds(1.7f);

        for (int i = 1; i < this.transform.childCount; i += 2)
        {
            cone[i].SetActive(false);
        }

        StartCoroutine(DestroyObj());
    }
*/
    IEnumerator DestroyObj() 
    {
        yield return new WaitForSeconds(0.1f);
        cone[0].SetActive(true);
        cone[6].SetActive(true);
        cone[5].SetActive(false);
        cone[11].SetActive(false);
        StartCoroutine(StartPattern2(temp));
        yield return new WaitForSeconds(speed);
        Destroy(this.gameObject);
    }
}
