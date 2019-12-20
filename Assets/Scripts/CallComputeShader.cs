using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallComputeShader : MonoBehaviour
{
    #region need compute_shader_sample

    public ComputeShader compute_shader;
    private ComputeBuffer buffer;
    private int shader_kanel;
    private int[] set_data;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        int shader_kanel = compute_shader.FindKernel("CSMain");
        buffer = new ComputeBuffer(5, sizeof(int));
        set_data = new int[5];
        for (int _i = 0; _i < 5; _i++)
        {
            set_data[_i] = _i;

            Debug.Log("数字：" + set_data[_i]);
        }
        buffer.SetData(set_data);
        compute_shader.SetBuffer(shader_kanel, "int_result", buffer);
        compute_shader.Dispatch(shader_kanel, 1, 1, 1);
        buffer.GetData(set_data);
        
        for (int _i = 0; _i < 5; _i++)
        {
            Debug.Log("処理後の数字:" + set_data[_i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        compute_shader.Dispatch(shader_kanel, 4096, 64, 1);
    }
}
