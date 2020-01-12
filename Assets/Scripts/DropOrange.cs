using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOrange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            this.GetComponent<Rigidbody>().useGravity = true;

        if (this.GetComponent<Rigidbody>().useGravity && this.transform.position.y < 2.5f)
        {
            this.GetComponent<Transform>().position = new Vector3(-3.0f, 2.5f, 0.0f);
        }
    }
}
