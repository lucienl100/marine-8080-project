using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToShaderSkin : MonoBehaviour
{
    public LayerMask lightLayer;
    SkinnedMeshRenderer renderer;
    Vector3 naught = new Vector3(-9999f, -9999f, -9999f);
    Transform player;
    public int maxLights = 4;
    public float sphereSize = 30f;
    void Start()
    {
        renderer = this.GetComponent<SkinnedMeshRenderer>();
        player = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        Collider[] locatedLights = Physics.OverlapSphere(player.position, sphereSize, lightLayer);
        float[] positionX = new float[maxLights];
        float[] positionY = new float[maxLights];
        float[] positionZ = new float[maxLights];
        Vector4[] colours = new Vector4[maxLights];
        foreach (var collider in locatedLights)
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
        if (i < 3)
        {
            while (i < 3)
            {
                positionX[i] = naught.x;
                positionY[i] = naught.y;
                positionZ[i] = naught.z;
                colours[i] = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                i++;
            }
        }
        Vector4 X = new Vector4(positionX[0], positionX[1], positionX[2], positionX[3]);
        Vector4 Y = new Vector4(positionY[0], positionY[1], positionY[2], positionY[3]);
        Vector4 Z = new Vector4(positionZ[0], positionZ[1], positionZ[2], positionZ[3]);
        Vector4 Red = new Vector4(colours[0][0], colours[1][0], colours[2][0], colours[3][0]);
        Vector4 Blue = new Vector4(colours[0][1], colours[1][1], colours[2][1], colours[3][1]);
        Vector4 Green = new Vector4(colours[0][2], colours[1][2], colours[2][2], colours[3][2]);
        foreach (Material material in renderer.materials){
            material.SetVector("_PointLightPositionX", X);
            material.SetVector("_PointLightPositionY", Y);
            material.SetVector("_PointLightPositionZ", Z);
            material.SetVector("_PointLightReds", Red);
            material.SetVector("_PointLightBlues", Blue);
            material.SetVector("_PointLightGreens", Green);
        }
        // renderer.material.SetVector("_PointLightColor1", colours[0]);
        // renderer.material.SetVector("_PointLightColor2", colours[1]);
        // renderer.material.SetVector("_PointLightColor3", colours[2]);
        // renderer.material.SetVector("_PointLightColor4", colours[3]);
    }
}