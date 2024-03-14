using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CrashScript1 : MonoBehaviour
{
    public GameObject crash;
    public int temp;
    public GameObject[] crashes;
    public float speed = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        temp = 0;
        crash = transform.GetChild(0).gameObject;
        crashes = new GameObject[7];
        speed = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement", 1) * 0.15f;
        StartCoroutine(StartCrash(temp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator StartCrash(int i)
    {
        yield return new WaitForSeconds(speed);
        GameObject crashTemp = null;
        crashTemp = Instantiate(crash, this.transform.position, Quaternion.Euler(new Vector3(0, 0 + this.transform.eulerAngles.y, 0)));
        crashTemp.transform.parent = this.transform;
        crashTemp.transform.localPosition = new Vector3(0, 0, 5 * i);
        crashTemp.transform.localScale = new Vector3(6, i + 0.2f, 8);
        crashes[i] = crashTemp;

        StartCoroutine(DestroyCrash(temp));

        if (i < 5)
        {
            temp += 1;
            StartCoroutine(StartCrash(temp));
        }
        else
        {
            StartCoroutine(DestroyObj());
        }

    }

    IEnumerator DestroyCrash(int i)
    {
        yield return new WaitForSeconds(0.7f);

        Destroy(crashes[ i]);
        crashes[i] = null;

    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
