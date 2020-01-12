using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PSModules                                                    // パーティクルシステムモジュールの構造体
{
    public ParticleSystem.ForceOverLifetimeModule Folm;                 // 受ける力の制御
    public ParticleSystem.EmissionModule Em;                            // 粒子放出量の制御

    public PSModules(ParticleSystem.ForceOverLifetimeModule folm, ParticleSystem.EmissionModule em)     // 一応コンストラクタも作った．引数は必要．
    {
        Folm = folm;
        Em = em;
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

        ParticleSystem ParticleObj;                                                                     
                                                                                                    // モジュールの配列に炎の要素を登録していく
        ParticleObj = transform.Find("PS_Fire_ALPHA").GetComponent<ParticleSystem>();                   // 背後の暗めなやつ　ベース
        Fire[0].Folm = ParticleObj.forceOverLifetime;
        Fire[0].Em   = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Fire_ADD").GetComponent<ParticleSystem>();                     // 真ん中のめちゃめちゃ光ってるやつ
        Fire[1].Folm = ParticleObj.forceOverLifetime;
        Fire[1].Em = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Glow").GetComponent<ParticleSystem>();                         // 周りでぼんやり明るいやつ
        Fire[2].Folm = ParticleObj.forceOverLifetime;
        Fire[2].Em = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Sparks").GetComponent<ParticleSystem>();                       // 火の粉
        Fire[3].Folm = ParticleObj.forceOverLifetime;
        Fire[3].Em = ParticleObj.emission;

    }

    private float mCount = 0;       //←時間計測用　　　　　　　デフォであったやつ　今は使ってない
    private bool mSwitch = true;    //←切り替えスイッチ用　　　　　　　　　　同上

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
                                                            // 点火処理
        if (Input.GetMouseButtonDown(0))                        // ボタンを押された瞬間は座標を取得するだけ
        {
            NowPos = Input.mousePosition;
        } else if (Input.GetMouseButton(0))                     // ボタンを押している最中は前フレームと距離の差を計算
        {
            PrePos = NowPos;
            NowPos = Input.mousePosition;
            if (Vector2.Distance(NowPos, PrePos) > 75.0f)       // 前のフレームから75より大きく動いたら点火
                particle.Play();
        }

                                                            // 揺らぎの処理
        float force = Input.GetAxis("Horizontal") * 2f;         // 左右キーの入力を取得，forceは  -2(左) ～ +2（右）
        for (int i = 0; i < Fire.Length; i++)
        {
            if (i == 3)                                         // 火の粉だけ処理が別
            {

                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(10);                        // 変数の代入がめんどいけどこうやってね．放出量が5で固定になるよ
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-2 + force, 2 + force);     // ２つ入れると範囲指定 force == 0 なら受ける力は -2 ~ +2 でランダムになる
            }
            else
            {
                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(15);
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(force);
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
