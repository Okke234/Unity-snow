using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    public Camera camera;
    public Shader drawShader;

    [Range(1,50)]
    public float brushSize;
    [Range(0,1)]
    public float brushStrength;

    private RenderTexture depthTexture;
    private Material snowMaterial, drawMaterial;
    private RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector("_Color", Color.red);

        snowMaterial = GetComponent<MeshRenderer>().material;
        depthTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.SetTexture("_DispTex", depthTexture);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
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

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 256, 256), depthTexture, ScaleMode.ScaleToFit, false, 1);
    }
}
