using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct PSModules
{
    public ParticleSystem.ForceOverLifetimeModule Folm;
    public ParticleSystem.EmissionModule Em;

    public PSModules(ParticleSystem.ForceOverLifetimeModule folm, ParticleSystem.EmissionModule em)
    {
        Folm = folm;
        Em = em;
    }
}

public class ControlFire : MonoBehaviour
{
    
    PSModules[] Fire = new PSModules[4];
    ParticleSystem particle;
    Vector2 PrePos;
    Vector2 NowPos;

    private readonly int SampleNum = (2 << 9); // サンプリング数は2のN乗(N=5-12)
    [SerializeField, Range(0f, 1000f)]
    float m_gain = 200f; // 倍率
    AudioSource m_source;
    float[] currentValues;

    // Start is called before the first frame update
    void Start()
    {
        NowPos = Input.mousePosition;
        GameObject obj = GameObject.Find("PS_Parent");
        particle = obj.GetComponent<ParticleSystem>();

        ParticleSystem ParticleObj;
        ParticleObj = transform.Find("PS_Fire_ALPHA").GetComponent<ParticleSystem>();
        Fire[0].Folm = ParticleObj.forceOverLifetime;
        Fire[0].Em   = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Fire_ADD").GetComponent<ParticleSystem>();
        Fire[1].Folm = ParticleObj.forceOverLifetime;
        Fire[1].Em = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Glow").GetComponent<ParticleSystem>();
        Fire[2].Folm = ParticleObj.forceOverLifetime;
        Fire[2].Em = ParticleObj.emission;

        ParticleObj = transform.Find("PS_Sparks").GetComponent<ParticleSystem>();
        Fire[3].Folm = ParticleObj.forceOverLifetime;
        Fire[3].Em = ParticleObj.emission;


        m_source = GetComponent<AudioSource>();
        currentValues = new float[SampleNum];
        if ((m_source != null) && (Microphone.devices.Length > 0)) // Audio source and mic exist
        {
            string devName = Microphone.devices[0]; // use 0th mic
            int minFreq, maxFreq;
            Microphone.GetDeviceCaps(devName, out minFreq, out maxFreq); // get max and min sampling num
            int ms = minFreq / SampleNum; // take sampling time
            m_source.loop = true; // loop
            m_source.clip = Microphone.Start(devName, true, ms, minFreq); // set clip to mic
            while (!(Microphone.GetPosition(devName) > 0)) { } // wait
            Microphone.GetPosition(null);
            m_source.Play();
        }
    }

    private float mCount = 0;       //←時間計測用
    private bool mSwitch = true;    //←切り替えスイッチ用

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NowPos = Input.mousePosition;
        } else if (Input.GetMouseButton(0))
        {
            PrePos = NowPos;
            NowPos = Input.mousePosition;
            if (Vector2.Distance(NowPos, PrePos) > 75.0f)
                particle.Play();
        }

        float force = Input.GetAxis("Horizontal") * 2f;
        for (int i = 0; i < Fire.Length; i++)
        {
            if (i == 3)
            {

                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(5);
                Fire[i].Folm.x = new ParticleSystem.MinMaxCurve(-2 + force, 2 + force);
            }
            else
            {
                Fire[i].Em.rate = new ParticleSystem.MinMaxCurve(10);
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

        m_source.GetSpectrumData(currentValues, 0, FFTWindow.Hamming);
        float sum = 0f;
        for (int i = 0; i < currentValues.Length; ++i)
        {
            sum += currentValues[i]; // add data (power of each wavelength)
        }
        // calculate volume
        float volumeRate = Mathf.Clamp01(sum * m_gain / (float)currentValues.Length);
        Debug.Log(volumeRate);
    }
}
