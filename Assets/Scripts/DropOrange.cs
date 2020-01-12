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

        GameObject Obj = GameObject.Find("Head");
        float height = Obj.transform.position.y + Obj.GetComponent<SphereCollider>().center.y + Obj.GetComponent<SphereCollider>().radius + 0.5f;
        Debug.Log(height);

        if (this.GetComponent<Rigidbody>().useGravity && this.transform.position.y < height)
        {
            this.GetComponent<Transform>().position = new Vector3(-2.5f, height, 2.0f);
        }
    }
}
