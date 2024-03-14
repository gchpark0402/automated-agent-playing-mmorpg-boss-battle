using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CrashScript : MonoBehaviour
{
    public GameObject crash;
    public int temp;
    public GameObject[] crashes;
    public float speed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        temp = 0;
        crash = transform.GetChild(0).gameObject;
        crashes = new GameObject[21];
        
        speed = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement", 1) * 0.2f;
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
        crashTemp.transform.localPosition = new Vector3(0, 0.2f * i + 1, 10 * i + 2);
        crashTemp.transform.localScale = new Vector3(i + 1, i + 0.2f, 10);
        crashes[3 * i ] = crashTemp;

        crashTemp = Instantiate(crash, this.transform.position + new Vector3(5f * i, 0.5f * i + 1, 5f * i), Quaternion.Euler(new Vector3(0, 45 + this.transform.eulerAngles.y, 0)));
        crashTemp.transform.parent = this.transform;
        crashTemp.transform.localPosition = new Vector3(5f * i, 0.2f * i + 1, 5f * i + 2);
        crashTemp.transform.localScale = new Vector3(i + 1, i + 0.2f, 10);
        crashes[3 * i + 1] = crashTemp;

        crashTemp = Instantiate(crash, this.transform.position + new Vector3(-5f * i, 0.5f * i + 1, 5f * i), Quaternion.Euler(new Vector3(0, -45 + this.transform.eulerAngles.y, 0)));
        crashTemp.transform.parent = this.transform;
        crashTemp.transform.localPosition = new Vector3(-5f * i, 0.2f * i + 1, 5f * i + 2);
        crashTemp.transform.localScale = new Vector3(i + 1, i + 0.2f, 10);
        crashes[3 * i + 2] = crashTemp;

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
        yield return new WaitForSeconds(1f);

        for (int j = 0; j < 3; j++)
        {
            Destroy(crashes[j + 3 * i]);
            crashes[j + 3 * i] = null;
        }

    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
