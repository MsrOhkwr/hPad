using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PSModules
{
    public ParticleSystem.ForceOverLifetimeModule Folm;
    public ParticleSystem.EmissionModule Em;
    public ParticleSystem.ShapeModule Sh;

    public PSModules(ParticleSystem.ForceOverLifetimeModule folm, ParticleSystem.EmissionModule em, ParticleSystem.ShapeModule sh)
    {
        Folm = folm;
        Em = em;
        Sh = sh;
    }
}
public class ControlFire : MonoBehaviour
{
    

    PSModules[] Fire = new PSModules[4];
    ParticleSystem particle;
    Vector2 PrePos;
    Vector2 NowPos;

    // Start is called before the first frame update
    void Start()
    {
        NowPos = Input.mousePosition;
        GameObject obj = GameObject.Find("PS_Parent");
        particle = obj.GetComponent<ParticleSystem>();

        //炎は4重のパーティクルのインスタンスから構成する
        ParticleSystem ParticleObj;
        ParticleObj = transform.Find("PS_Fire_ALPHA").GetComponent<ParticleSystem>();
        Fire[0].Folm = ParticleObj.forceOverLifetime;
        Fire[0].Em   = ParticleObj.emission;
        Fire[0].Sh   = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Fire_ADD").GetComponent<ParticleSystem>();
        Fire[1].Folm = ParticleObj.forceOverLifetime;
        Fire[1].Em = ParticleObj.emission;
        Fire[1].Sh = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Glow").GetComponent<ParticleSystem>();
        Fire[2].Folm = ParticleObj.forceOverLifetime;
        Fire[2].Em = ParticleObj.emission;
        Fire[2].Sh = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Sparks").GetComponent<ParticleSystem>();
        Fire[3].Folm = ParticleObj.forceOverLifetime;
        Fire[3].Em = ParticleObj.emission;
        Fire[3].Sh = ParticleObj.shape;
    }

    private float mCount = 0;       //←時間計測用
    private bool mSwitch = true;    //←切り替えスイッチ用

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {

        //着火の判定 ==========================================================================

        if (Input.GetMouseButtonDown(0))
        {
            NowPos = Input.mousePosition;
 
        } else if (Input.GetMouseButton(0))
        {
            Debug.Log("KeyBoard input accepted");
            PrePos = NowPos;
            NowPos = Input.mousePosition;
            if (Vector2.Distance(NowPos, PrePos) > 75.0f)
                particle.Play();
        }

        //炎の傾き方向の判定 ==========================================================================
        //水平方向の値の取得
        float force = Input.GetAxis("Horizontal") * 2f;

        //鉛直方向の値の取得
        float v_force = Input.GetAxis("Vertical") * 2f;

        //加速度の値の取得
        Vector3 acc = Input.acceleration;


        //Debug.Log(Input.GetAxis("Vertical") * 2f);
        //Debug.Log(Input.GetAxis("Horizontal") * 2f);

        float v_force_float = (float)25.0 * v_force;
        int v_force_int = (int)v_force;

        for (int i = 0; i < Fire.Length; i++)
        {
            if (i == 3)
            {

                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(5);
                //x方向の加速度の絶対値に応じて調整
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-2 + force, 2 + force);

                //y方向の加速度の絶対値に応じて調整
                Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(-10 + 5 * v_force, 10 + 5 * v_force);
                Fire[i].Sh.angle = 12 + 6 * v_force;
                //Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(-10 + 5 * System.Math.Abs(acc.y), 10 + 5 * System.Math.Abs(acc.y));
                Fire[i].Sh.angle = 12 + 6 * System.Math.Abs(acc.y);

            }
            else
            {
                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(10);
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(force);
                Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(5 * v_force);
                Fire[i].Sh.angle = 12 + 6 * v_force;
                //Fire[i].Sh.angle = 12 + 6 * System.Math.Abs(acc.y);
            }
        }
        
        /*
        mCount = mCount + Time.deltaTime;   //←時間計測中
        if (mCount >= 5.0f)
        { //← 5秒経過する度に if 成立
            mCount = 0; // 時間計測用変数を初期化
            if (mSwitch == true)
            {
                //↓Rate を 100 に変更
                for (int i = 0; i < Fire.Length; i++)
                {
                    Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(10);
                    Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(2);

                    if (i == 3)
                        Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(0, 4);
                }
            }

            else
            {
                //↓Rate を 10 に変更
                for (int i = 0; i < Fire.Length; i++)
                {
                    Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(10);
                    Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(0);

                    if (i == 3)
                        Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-2, 2);
                }
            }

            //↓ true が false に、false が true に交互に入れ替わり続ける
            mSwitch = !mSwitch;
        }
        */


    }
}
