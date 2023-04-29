using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderVariables : MonoBehaviour
{
    public Texture2D celshadeGradient;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalTexture("_CelShadeGradient", celshadeGradient);
    }
}
