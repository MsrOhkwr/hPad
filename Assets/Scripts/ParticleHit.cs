using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleHit : MonoBehaviour
{

    //　ダメージUIプレハブ
    [SerializeField]
    private GameObject damageUI = null;

    //　ダメージを受けた場所にダメージUIを表示
    public void Hit(Vector3 pos)
    {
        //Debug.Log(pos);
        //Instantiate<GameObject>(damageUI, pos, Quaternion.identity);
    }
}