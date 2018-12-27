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

        var mesh = new Mesh();

        //8个顶点
        Vector3[] Vertices = new Vector3[8]
        {
            new Vector3(0, 0, 0), new Vector3(5, 0, 0), new Vector3(0, 0, 5), new Vector3(5, 0, 5),
            new Vector3(0, 5, 0), new Vector3(5, 5, 0), new Vector3(0, 5, 5), new Vector3(5, 5, 5)
        };

        //每个顶点有自己的法线坐标，并且是相邻三个平面的法线坐标的和。normalize之后向量长度变为1。
        Vector3[] Normals = new Vector3[8]
        {
            new Vector3(-1, -1, -1).normalized, new Vector3(1, -1, -1).normalized, new Vector3(-1, -1, 1).normalized, new Vector3(1, -1, 1).normalized,
            new Vector3(-1, 1, -1).normalized, new Vector3(1, 1, -1).normalized, new Vector3(-1, 1, 1).normalized, new Vector3(1, 1, 1).normalized
        };

        //每个顶点有自己的UV坐标，不熟悉可以不用管，本篇教程不关注UV坐标
        Vector2[] UVs = new Vector2[8]
        {
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1),
            new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 0)
        };

        //对顶点的索引，每三个数组成一个三角面，比如0， 1， 2对应Vertices中的第一个、第二个、第三个点，注意这里要用左手法则写顺序。
        int[] Triangles = new int[36]
        {
            0, 1, 2, 1, 3, 2, 4, 5, 0, 5, 1, 0, 4, 0, 6, 6, 0, 2, 2, 7, 6, 7, 2, 3, 3, 5, 7, 1, 5, 3, 5, 4, 6, 5, 6, 7
        };        

        //给mesh赋值
        mesh.vertices = Vertices;
        mesh.normals = Normals;
        mesh.triangles = Triangles;
        mesh.uv = UVs;

        //自己起一个名字
        mesh.name = "Cube";

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

        var radianDelta = Mathf.Deg2Rad * (180 / (rings - 1));
        var sectorDelta = Mathf.Deg2Rad * (360 / (sectors));

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
