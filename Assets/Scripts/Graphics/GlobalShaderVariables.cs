using UnityEngine;

[ExecuteInEditMode]
public class GlobalShaderVariables : MonoBehaviour
{
    public Texture2D celshadeGradient;
    public Color shadowcolor;

    void Update()
    {
        Shader.SetGlobalTexture("_CelShadeGradient", celshadeGradient);
        Shader.SetGlobalColor("_ShadowColor", shadowcolor);
    }
}
