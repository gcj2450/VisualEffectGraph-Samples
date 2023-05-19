using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class TestHex : MonoBehaviour
{
    public VisualEffect vfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool isActive = true;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            vfx.gameObject.SetActive(isActive);
            isActive = !isActive;
        }
    }
}
