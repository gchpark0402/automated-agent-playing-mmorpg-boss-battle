using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuakeScript : MonoBehaviour
{
    public GameObject[] torus;
    public int temp = 0;

    // Start is called before the first frame update
    void Start()
    {
        torus = new GameObject[this.transform.childCount];

        for (int i = 0; i < this.transform.childCount; i++)
        {
            torus[i] = this.transform.GetChild(i).gameObject;
            torus[i].SetActive(false);   
        }

        StartCoroutine(SpawnRange(temp));
    }


    IEnumerator SpawnRange(int i)
    {
        yield return new WaitForSeconds(0.5f);

        if(i < this.transform.childCount)
        {
            torus[i].SetActive(true);
            StartCoroutine(torus[i].GetComponent<EarthQuakeTorus>().AgentDamage(2));
            temp++;
            StartCoroutine(SpawnRange(temp));
        }
        else
        {
            StartCoroutine(DestroyObj());
        }
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(2.5f);

        Destroy(this.gameObject);
    }

}
