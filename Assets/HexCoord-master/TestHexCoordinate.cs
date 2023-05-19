using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Settworks.Hexagons;

public class TestHexCoordinate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int ij = 0; ij < 10; ij++)
            {
                HexCoord hexCoord = new HexCoord(i, ij);
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.SetParent(transform);
                go.transform.localPosition = hexCoord.Position();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
