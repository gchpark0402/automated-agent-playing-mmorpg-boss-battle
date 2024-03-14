using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneScript : MonoBehaviour
{
    public GameObject safeZonePrefab;
    public GameObject safeZone;

    // Start is called before the first frame update
    void Start()
    {
        safeZonePrefab = this.transform.GetChild(1).gameObject;
        StartCoroutine(StartPattern());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartPattern()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < 3; i++)
        {
            int tempX = Random.Range(-25, 25);
            int tempZ = Random.Range(-25, 25);
            Vector3 tempVec = new Vector3(tempX, 0, tempZ);

            safeZone = Instantiate(safeZonePrefab, tempVec, Quaternion.identity);
            safeZone.transform.localScale = new Vector3(10, 10, 10);
            safeZone.transform.parent = this.transform;
        }

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
