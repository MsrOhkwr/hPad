using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PSModules                                                    // パーティクルシステムモジュールの構造体
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
    

    PSModules[] Fire = new PSModules[4];                                                                // 炎は4レイヤーで作ってるので4にしてるだけ
    ParticleSystem particle;                                                                            // こいつは親パーティクル．放出開始とか移動みたいな簡単な操作なら4つまとめてできるよ．
    Vector2 PrePos;
    Vector2 NowPos;

    // Start is called before the first frame update
    void Start()
    {
        NowPos = Input.mousePosition;
        GameObject obj = GameObject.Find("PS_Parent");                                                  // obj にPS_Parentって名前のオブジェクトを渡す
        particle = obj.GetComponent<ParticleSystem>();                                                  // PS_Parentの コンポーネントをいじれるようになる
                                                                                                        // コンポーネントはインスペクタに一覧表示されるやつ．同じようにレンダラーとかコライダーとかもとってこれる


        //炎は4重のパーティクルのインスタンスから構成する
        ParticleSystem ParticleObj;
        ParticleObj = transform.Find("PS_Fire_ALPHA").GetComponent<ParticleSystem>();

        Fire[0].Folm = ParticleObj.forceOverLifetime;
        Fire[0].Em   = ParticleObj.emission;
        Fire[0].Sh   = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Fire_ADD").GetComponent<ParticleSystem>();                     // 真ん中のめちゃめちゃ光ってるやつ
        Fire[1].Folm = ParticleObj.forceOverLifetime;
        Fire[1].Em = ParticleObj.emission;
        Fire[1].Sh = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Glow").GetComponent<ParticleSystem>();                         // 周りでぼんやり明るいやつ
        Fire[2].Folm = ParticleObj.forceOverLifetime;
        Fire[2].Em = ParticleObj.emission;
        Fire[2].Sh = ParticleObj.shape;

        ParticleObj = transform.Find("PS_Sparks").GetComponent<ParticleSystem>();                       // 火の粉
        Fire[3].Folm = ParticleObj.forceOverLifetime;
        Fire[3].Em = ParticleObj.emission;
        Fire[3].Sh = ParticleObj.shape;
    }

    private float mCount = 0;       //←時間計測用　　　　　　　デフォであったやつ　今は使ってない
    private bool mSwitch = true;    //←切り替えスイッチ用　　　　　　　　　　同上

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
            if (Vector2.Distance(NowPos, PrePos) > 75.0f)       // 前のフレームから75より大きく動いたら点火
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


        for (int i = 0; i < Fire.Length; i++)
        {
            if (i == 3)                                         // 火の粉だけ処理が別
            {


                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(5);
                //x方向の加速度の絶対値に応じて調整
                //Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-2 + force, 2 + force);
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-10 + 5 * System.Math.Abs(acc.x), 10 + 5 * System.Math.Abs(acc.x));


                //y方向の加速度の絶対値に応じて調整
                
                //Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(-10 + 5 * v_force, 10 + 5 * v_force);
                //Fire[i].Sh.angle = 12 + 6 * v_force;

                Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(-10 + 5 * System.Math.Abs(acc.y), 10 + 5 * System.Math.Abs(acc.y));
                Fire[i].Sh.angle = 12 + 24 * System.Math.Abs(acc.y);



            }
            else
            {
                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(15);
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(force);
                Fire[i].Folm.y = new ParticleSystem.MinMaxCurve(5 * v_force);


                //Fire[i].Sh.angle = 12 + 6 * v_force;
                Fire[i].Sh.angle = 12 + 24 * System.Math.Abs(acc.y);


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
