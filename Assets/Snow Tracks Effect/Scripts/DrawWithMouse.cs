using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{   // script will be attached to plane with snowTrack material
    public Camera mainCamera;
    public Shader drawShader; // DrawTrack.shader

    [Range(1,500)]
    public float _brushSize;
    [Range(0,1)]
    public float _brushStrength;

    private RenderTexture splatMap;  // for splat map red and black
    private Material snowMaterial, drawMaterial;

    private RaycastHit hit;

    private void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = GetComponent<MeshRenderer>().material; // will get snowTrack shader
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
    }
     
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition),out hit))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", _brushStrength);
                drawMaterial.SetFloat("_Size", _brushSize);

                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap,drawMaterial);
                RenderTexture.ReleaseTemporary(temp); // release render texture to not take space in memory
            }
        }
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), splatMap, ScaleMode.ScaleToFit ,false,1);
    }
}
