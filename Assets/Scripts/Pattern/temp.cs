using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    public float moveSpeed = 1000.0f;
    public float rotationSpeed = 2.0f;

    private float rotationX = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.transform.rotation);
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(verticalInput, 0, -horizontalInput) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

    }

    public void print()
    {
        Debug.Log("hello temp");
    }
}
