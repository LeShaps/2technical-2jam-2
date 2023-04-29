using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class GlobalShaderVariables : MonoBehaviour
{
    public Texture2D celshadeGradient;
    public Color shadowcolor;

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalTexture("_CelShadeGradient", celshadeGradient);
        Shader.SetGlobalColor("_ShadowColor", shadowcolor);
    }
}
