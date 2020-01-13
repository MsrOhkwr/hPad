using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpButton : MonoBehaviour
{
    private GameObject bodyObj;
    private bool status;

    // Start is called before the first frame update
    void Start()
    {
        bodyObj = GameObject.Find("UIBody");
        status = false;
        bodyObj.SetActive(status);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick()
    { // 必ず public にする
        status = !status;
        bodyObj.SetActive(status);
    }
}
