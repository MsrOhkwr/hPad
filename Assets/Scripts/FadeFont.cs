using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeFont : MonoBehaviour
{
    float alfa = 0.0f;
    float speed = 0.1f;
    float time = 0.0f;
    float red, green, blue;
    bool FadeStarted = false;

    void Start()
    {
        red = GetComponent<Text>().color.r;
        green = GetComponent<Text>().color.g;
        blue = GetComponent<Text>().color.b;

        GetComponent<Text>().color = new Color(red, green, blue, alfa);
    }

    void Update()
    {
        Invoke("Fade", 1.0f);
        if (FadeStarted) {
            GetComponent<Text>().color = new Color(red, green, blue, 0.5f - 0.5f * Mathf.Cos(time));
            time += speed;
       }
    }

    void Fade()
    {
        FadeStarted = true;
    }
}