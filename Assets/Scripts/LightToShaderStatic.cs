﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToShaderStatic : MonoBehaviour
{
    public LayerMask lightLayer;
    Renderer mRenderer;
    public int maxLights = 4;
    public float sphereSize = 50f;
    Vector3 naught = new Vector3(-9999f, -9999f, -9999f);

    // Start is called before the first frame update
    void Awake()
    {
        mRenderer = this.GetComponent<Renderer>();
        int i = 0;
        Collider[] lights = Physics.OverlapSphere(this.transform.position, sphereSize, lightLayer);
        float[] positionX = new float[maxLights];
        float[] positionY = new float[maxLights];
        float[] positionZ = new float[maxLights];
        Vector4[] colours = new Vector4[maxLights];
        //Detect up to four nearby lights
        foreach (var collider in lights)
        {
            if (i > 3)
            {
                break;
            }
            Transform light = collider.transform;
            positionX[i] = light.position.x;
            positionY[i] = light.position.y;
            positionZ[i] = light.position.z;
            colours[i] = collider.GetComponent<Light>().color;
            i++;
        }
        //Set the remaining light position and color variables to values that won't impact the lighting in the shader
        if (i < 3)
        {
            while (i < 3)
            {
                positionX[i] = naught.x;
                positionY[i] = naught.y;
                positionZ[i] = naught.z;
                colours[i] = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
                i++;
            }
        }
        Vector4 X = new Vector4(positionX[0], positionX[1], positionX[2], positionX[3]);
        Vector4 Y = new Vector4(positionY[0], positionY[1], positionY[2], positionY[3]);
        Vector4 Z = new Vector4(positionZ[0], positionZ[1], positionZ[2], positionZ[3]);
        Vector4 Red = new Vector4(colours[0][0], colours[1][0], colours[2][0], colours[3][0]);
        Vector4 Blue = new Vector4(colours[0][1], colours[1][1], colours[2][1], colours[3][1]);
        Vector4 Green = new Vector4(colours[0][2], colours[1][2], colours[2][2], colours[3][2]);
        //Pass the light information into the shader
        foreach (Material material in mRenderer.materials){
            material.SetVector("_PointLightPositionX", X);
            material.SetVector("_PointLightPositionY", Y);
            material.SetVector("_PointLightPositionZ", Z);
            material.SetVector("_PointLightReds", Red);
            material.SetVector("_PointLightBlues", Blue);
            material.SetVector("_PointLightGreens", Green);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
