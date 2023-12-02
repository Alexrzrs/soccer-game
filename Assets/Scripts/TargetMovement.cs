using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Up
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * 3f * Time.deltaTime);
        }

        //Left
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-Vector3.right * 3f * Time.deltaTime);
        }

        //Right
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * 3f * Time.deltaTime);
        }

        //Down
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(-Vector3.up * 3f * Time.deltaTime);
        }
        
    }
}
