using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour
{
    public Shader drawShader; // DrawTrack.shader
    public GameObject terrainWithScript;// that contains snowtrack script
    public Transform[] wheel;

    [Range(0, 10)]
    public float _brushSize;
    [Range(0, 1)]
    public float _brushStrength;

    private RenderTexture splatMap;  // for splat map red and black
    private Material snowMaterial, drawMaterial;

    RaycastHit groundHit;
    int layermask; // to make sure that we are on ground

    private void Start()
    {
        layermask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        //drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = terrainWithScript.GetComponent<MeshRenderer>().material; // will get snowTrack shader
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);
    }

    private void Update()
    {
        for (int i = 0; i < wheel.Length; i++)
        {
            if (Physics.Raycast(wheel[i].position, -Vector3.up, out groundHit, 1f,layermask))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Strength", _brushStrength);
                drawMaterial.SetFloat("_Size", _brushSize);

                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp); // release render texture to not take space in memory
            }
        }
    }
}
