using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class LeftRightScript : MonoBehaviour
{
    public GameObject left;
    public GameObject right;
    public GameObject leftAura;
    public float delay = 1;

    // Start is called before the first frame update
    void Start()
    {
        left = this.transform.GetChild(0).gameObject;
        right = this.transform.GetChild(1).gameObject;
        leftAura= this.transform.GetChild(2).gameObject;

        left .SetActive(false);
        right .SetActive(false);
        leftAura.SetActive(false);

        delay = Academy.Instance.EnvironmentParameters.GetWithDefault("decreasement", 1) * 1;
        StartCoroutine(StartPattern());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1);
        leftAura.SetActive(true);
        yield return new WaitForSeconds(1);
        left.SetActive(true);
        leftAura.SetActive(false);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.3f);
        left.SetActive(false);
        yield return new WaitForSeconds(delay);
        right.SetActive(true);
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
