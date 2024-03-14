using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackScript : MonoBehaviour
{
    public GameObject crack;

    // Start is called before the first frame update
    void Start()
    {
        crack = this.transform.GetChild(0).gameObject;
        crack.SetActive(false);
        StartCoroutine(Crack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Crack()
    {
        yield return new WaitForSeconds(1);
        crack.SetActive(true);
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
