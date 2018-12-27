using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShowVertexCount : MonoBehaviour {

    public int VertCount;
    public int TrCount;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        var mesh = this.GetComponent<MeshFilter>();
        if (mesh)
        {
            VertCount = mesh.mesh.vertices.Length;
            TrCount = mesh.mesh.triangles.Length;
        }
        else
        {
            VertCount = 0;
            TrCount = 0;
        }
	}
}
