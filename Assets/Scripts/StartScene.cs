using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    float alfa = 1.0f;
    float speed = 0.02f;
    float red, green, blue;
    // Start is called before the first frame update
    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;

        Invoke("InactivateCanvas", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
        alfa -= speed;
    }

    void InactivateCanvas()
    {
        this.enabled = false;
    }
}
