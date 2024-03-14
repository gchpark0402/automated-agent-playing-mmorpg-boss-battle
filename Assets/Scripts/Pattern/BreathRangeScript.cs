using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class BreathRangeScript : MonoBehaviour
{
    public GameObject breath;

    public Vector3 curRotation;
    public float rotTemp2;
    public float increasement;
    public float speed;
    public bool temp;


    // Start is called before the first frame update
    void Start()
    {
        breath = transform.GetChild(0).gameObject;
        breath.SetActive(false);
        temp = false;
        increasement = Academy.Instance.EnvironmentParameters.GetWithDefault("increasement", 1);
        speed = 90 * increasement;
        curRotation = new Vector3(0, this.transform.eulerAngles.y-70, 0);
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        if(temp)
        {
            rotTemp2 += Time.deltaTime * speed;
            breath.transform.rotation =
                Quaternion.Euler(curRotation + new Vector3(0, rotTemp2, 0));
        }

        if (rotTemp2 > 150) Destroy(this.gameObject);
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        breath.SetActive(true);
        temp = true;
    }
}
