using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    float alfa;
    float speed = 0.02f;
    float red, green, blue;
    bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += speed;
        }
    }

    public void OnClick()
    { // 必ず public にする
        clicked = true;
        Invoke("Change", 1.0f);
        Debug.Log("clicked");
    }

    void Change()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
