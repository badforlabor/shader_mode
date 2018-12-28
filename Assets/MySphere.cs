using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MySphere : MonoBehaviour {

    public bool refresh = false;
    public int Rings = 12;
    public int Sectors = 24;

    // 如果三角面上的法线和三角面顶点的法线是一致的，就能达到了flatshader效果。
    // 这时候，就需要不共享顶点了。（正常情况下，1个平面=2个三角面=4个顶点。此时，不共享的情况下，1个平面=2个三角面=6个顶点）
    public bool NoShareVert = false;    


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
        mf.mesh = CreateSphere(1, 12, 24, false);
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
            mf.mesh = CreateSphere(1, Rings, Sectors, NoShareVert);
        }
    }

    static Mesh CreateSphere(float radius, int rings, int sectors, bool noShareVert)
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

        if (noShareVert)
        {
            var newVerts = new List<Vector3>(trs.Count);
            for (int i = 0; i < trs.Count; i++)
            {
                newVerts.Add(verts[trs[i]]);
                trs[i] = i;
            }
            mesh.vertices = newVerts.ToArray();
            mesh.triangles = trs.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
        else
        {

            mesh.vertices = verts.ToArray();
            mesh.triangles = trs.ToArray();
            mesh.normals = ns.ToArray();
            mesh.uv = uvs.ToArray();
        }

        return mesh;
    }
}
