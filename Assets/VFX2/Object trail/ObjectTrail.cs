using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ObjectTrail : MonoBehaviour
{
    [SerializeField] VisualEffect _vfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _vfx.SetVector3("ObjPos", this.transform.position);
    }
}
