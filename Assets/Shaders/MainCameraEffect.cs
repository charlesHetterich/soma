using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MainCameraEffect : MonoBehaviour {

    public Material EffectMaterial;
	
	void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }
}
