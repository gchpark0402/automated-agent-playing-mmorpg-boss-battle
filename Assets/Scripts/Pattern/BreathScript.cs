using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathScript : MonoBehaviour
{
    public GameObject breath;

    // Start is called before the first frame update
    void Start()
    {
        breath = this.gameObject.transform.GetChild(0).gameObject;
        breath.SetActive(false);
        StartCoroutine(SpawnBreath());
    }

    IEnumerator SpawnBreath()
    {
        yield return new WaitForSeconds(1);
        breath.SetActive(true);

    }
}
