using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour
{
    public float speed = 0.3f;
    public float lifetime;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        if(lifetime >= 4f) Destroy(gameObject);
        this.transform.Translate(this.transform.forward * speed * Time.deltaTime * 10);
    }


    private void OnTriggerEnter(Collider other)
    {
        if( other.transform.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.transform.tag == "Agent" )
        {
            if(other.GetComponent<AgentScript>() != null)
                other.gameObject.GetComponent<AgentScript>().Damaged();
            else if(other.GetComponent<AgentCurriculumScript>() != null)
                other.gameObject.GetComponent<AgentCurriculumScript>().Damaged();
            Destroy(gameObject);
        }

    }
}
