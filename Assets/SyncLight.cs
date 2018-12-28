using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SyncLight : MonoBehaviour {

    public MyFakeLight Light;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr && Light)
        {
            mr.sharedMaterial.SetVector("_LightPos", Light.transform.position);
            mr.sharedMaterial.SetFloat("_Intensity", Light.Intensity);
        }
    }
}
