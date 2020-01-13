using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    float alfa = 0.0f;
    float speed = 0.01f;
    float red, green, blue;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        GetComponent<Image>().color = new Color(red, green, blue, alfa);

    }

    void Update()
    {
        GetComponent<Image>().color = new Color(red, green, blue, alfa);
        alfa += speed;
    }
}