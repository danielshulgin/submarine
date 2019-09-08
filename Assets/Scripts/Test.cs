using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public     class Test : MonoBehaviour
    {
    Renderer rend;

    //MeshRenderer meshRenderer;
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Custom/Echolocation");
        //meshRenderer = GetComponent<MeshRenderer>();
        //rend = GetComponent<Renderer>();

        // Use the Specular shader on the material
        //rend.material.shader = Shader.Find("Echolocation");
    }

    private void Update()
    {
        //Shader specularShader = Shader.Find("Standard (Specular setup)");
        //Fetch the Renderer from the GameObject
        

        //Set the main Color of the Material to green
        //Find the Specular shader and change its Color to red
        
        rend.material.SetFloat("_Radius", MapManager.instance.value);
        Vector3 position = MapManager.instance.submarine.transform.position;
        rend.material.SetVector("_Center", new Vector4(position.x, position.y));
        //meshRenderer.material.SetFloat("_Radius", value);
    }
}

