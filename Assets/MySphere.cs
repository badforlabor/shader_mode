using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MySphere : MonoBehaviour {

    public bool refresh = false;
    public int Rings = 12;
    public int Sectors = 24;


    private void Awake()
    {
        var mf = this.GetComponent<MeshFilter>();
        if (!mf)
        {
            mf = this.gameObject.AddComponent<MeshFilter>();
        }
        var mr = this.GetComponent<MeshRenderer>();
        if (!mr)
        {
            mr = this.gameObject.AddComponent<MeshRenderer>();
        }

        //把mesh赋给MeshFilter
        mf.mesh = CreateSphere(1, 12, 24);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var mf = this.GetComponent<MeshFilter>();
        if (refresh)
        {
            refresh = false;
            mf.mesh = CreateSphere(1, Rings, Sectors);
        }
    }

    static Mesh CreateSphere(float radius, int rings, int sectors)
    {
        rings = Mathf.Max(2, rings);
        sectors = Mathf.Max(2, sectors);

        var mesh = new Mesh();

        var verts = new List<Vector3>();
        var ns = new List<Vector3>();
        var uvs = new List<Vector2>();
        var trs = new List<int>();

        var radianDelta = Mathf.Deg2Rad * (180.0f / (rings - 1));
        var sectorDelta = Mathf.Deg2Rad * (360.0f / (sectors));

        for (int r = 0; r < rings; r++)
        {
            var R = Mathf.Sin(radianDelta * r);
            var y = Mathf.Cos(radianDelta * r);
            for (int s = 0; s < sectors; s++)
            {
                var x = R * Mathf.Cos(sectorDelta * s);
                var z = R * Mathf.Sin(sectorDelta * s);

                verts.Add(new Vector3(x, y, z) * radius);
                ns.Add(new Vector3(x, y, z));
                uvs.Add(new Vector2(1.0f * r / (rings - 1), 1.0f * s / (sectors - 1)));
            }
        }


        for (int r = 0; r < rings-1; r++)
        {
            for (int s = 0; s < sectors; s++)
            {
                var a = r * sectors + s;
                var b = r * sectors + (s + 1)%sectors;
                var c = (r + 1) * sectors + (s + 1) % sectors;
                var d = (r + 1) * sectors + s;

                trs.Add(a);
                trs.Add(b);
                trs.Add(c);

                trs.Add(a);
                trs.Add(c);
                trs.Add(d);
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.normals = ns.ToArray();
        mesh.triangles = trs.ToArray();
        mesh.uv = uvs.ToArray();

        return mesh;
    }
}
