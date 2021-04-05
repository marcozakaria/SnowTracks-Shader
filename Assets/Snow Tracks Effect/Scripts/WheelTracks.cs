using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTracks : MonoBehaviour
{
    public Shader drawShader; // DrawTrack.shader
    public GameObject terrainWithScript;// that contains snowtrack script
    public Transform[] wheel;

    [Header("Bush Settings")]
    [SerializeField] LayerMask layermask;
    [Range(0, 10)]
    public float _brushSize;
    [Range(0, 2)]
    public float _brushStrength;

    [Header("SplatMap Settings")]
    [Range(64,1024)]
    [SerializeField] int splatMapSize = 512;

    private RenderTexture splatMap;  // for splat map red and black
    private Material snowMaterial, drawMaterial;

    RaycastHit groundHit;
    RenderTexture temp;

    private void Start()
    {
        //layermask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        //drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = terrainWithScript.GetComponent<MeshRenderer>().material; // will get snowTrack shader
        splatMap = new RenderTexture(splatMapSize, splatMapSize, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_Splat", splatMap);

        drawMaterial.SetFloat("_Strength", _brushStrength);
        drawMaterial.SetFloat("_Size", _brushSize);
    }

    private void OnValidate()
    {
        if (drawMaterial != null)
        {
            drawMaterial.SetFloat("_Strength", _brushStrength);
            drawMaterial.SetFloat("_Size", _brushSize);
        }      
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wheel.Length; i++)
        {
            if (Physics.Raycast(wheel[i].position, -Vector3.up, out groundHit, 1f,layermask))
            {   // very costly on performace need optimization
                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                
                temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp); // release render texture to not take space in memory
            }
        }
    }
}
