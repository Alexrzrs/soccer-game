using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balon : MonoBehaviour
{
    public Transform target;
    public float Force;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }
    }

    // Patear balon
    void shoot()
    {
        Vector3 Shoot = (target.position - this.transform.position).normalized;
        GetComponent<Rigidbody>().AddForce(Shoot * Force + new Vector3(0,3f,0), ForceMode.Impulse);
    }
}
