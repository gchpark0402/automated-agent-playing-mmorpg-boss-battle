using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class DangerPatternScript : MonoBehaviour
{
    public GameObject sphere;
    public Transform destination;
    public bool moveSphere = false;
    public float speed = 3f;
    public float startTime;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        sphere.SetActive(false);
        StartCoroutine(MoveSphere());
        //this.transform.forward = new Vector3(0, 0, this.transform.rotation.y);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, Vector3.forward * 5, Color.green);

        if (moveSphere)
        {
            Vector3 dir = (destination.localPosition - sphere.transform.localPosition).normalized;
            sphere.transform.Translate(dir * Time.deltaTime * speed);
        }
    }

    IEnumerator MoveSphere()
    {
        yield return new WaitForSeconds(startTime);
        sphere.SetActive(true);
        moveSphere= true;
        StartCoroutine(DestroySphere());
    }

    IEnumerator DestroySphere()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

   
}
