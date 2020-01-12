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

    readonly int size = 32;

    // Start is called before the first frame update
    void Start()
    {
        int shader_kanel = compute_shader.FindKernel("CSMain");
        buffer = new ComputeBuffer(size, sizeof(int));
        set_data = new int[size];
        buffer.SetData(set_data);
        compute_shader.SetBuffer(shader_kanel, "int_result", buffer);
        compute_shader.Dispatch(shader_kanel, size, 1, 1);
        buffer.Release();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 1024; i++)
        {
            compute_shader.Dispatch(shader_kanel, size, 1, 1);
        }
    }
}
