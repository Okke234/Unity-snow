using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSnow : MonoBehaviour
{
    public Shader drawShader;
    public GameObject terrain;
    private RenderTexture depthTexture;
    private Material snowMaterial, drawMaterial;
    private RaycastHit groundHit;
    private int layerMask;
    
    [Range(1,50)]
    public float brushSize;
    [Range(0,1000)]
    public float brushStrength;
    
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);
        
        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        depthTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_DispTex", depthTexture);
    }

    // Update is called once per frame
    void Update()
    { 
        if (Physics.Raycast(this.transform.position, Vector3.down, out groundHit, 0.6f, layerMask))
        {
                drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                drawMaterial.SetFloat("_Size", brushSize);
                drawMaterial.SetFloat("_Strength", brushStrength);
                RenderTexture temp = RenderTexture.GetTemporary(depthTexture.width, depthTexture.height, 0,
                    RenderTextureFormat.ARGBFloat);
                Graphics.Blit(depthTexture, temp);
                Graphics.Blit(temp, depthTexture, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
        }
    }
}
