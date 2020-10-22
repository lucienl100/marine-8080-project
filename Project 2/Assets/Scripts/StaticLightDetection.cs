using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLightDetection : MonoBehaviour
{
    public LayerMask lightLayer;
    Renderer renderer;
    public int maxLights = 4;
    public float sphereSize = 50f;
    Vector3 naught = new Vector3(-9999f, -9999f, -9999f);

    // Start is called before the first frame update
    void Awake()
    {
        renderer = this.GetComponent<Renderer>();
        int i = 0;
        Collider[] lights = Physics.OverlapSphere(this.transform.position, sphereSize, lightLayer);
        float[] positionX = new float[maxLights];
        float[] positionY = new float[maxLights];
        float[] positionZ = new float[maxLights];
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
            i++;
        }
        if (i < 3)
        {
            while (i < 3)
            {
                positionX[i] = naught.x;
                positionY[i] = naught.y;
                positionZ[i] = naught.z;
                i++;
            }
        }
        Vector4 X = new Vector4(positionX[0], positionX[1], positionX[2], positionX[3]);
        Vector4 Y = new Vector4(positionY[0], positionY[1], positionY[2], positionY[3]);
        Vector4 Z = new Vector4(positionZ[0], positionZ[1], positionZ[2], positionZ[3]);
        renderer.material.SetVector("_PointLightPositionX", X);
        renderer.material.SetVector("_PointLightPositionY", Y);
        renderer.material.SetVector("_PointLightPositionZ", Z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
