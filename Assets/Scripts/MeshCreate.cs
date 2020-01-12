using System;
using System.Collections.Generic;
using UnityEngine;


public class Vertex
{
    private Vector3 pos;
    private Vector3 nor;
    private Color col;
    private Vector2 tex;
    private List<int> nearby;
    private List<int> including;
    private float rust;
    private float bond;

    public Vertex(Vector3 p)
    {
        pos = p;
        nor = new Vector3(0, 0, -1);
        col = new Color(1.0f, 1.0f, 1.0f);
        tex = new Vector2(0, 0);
        nearby = new List<int>();
        including = new List<int>();
        rust = 0.0f;
    }
    public Vertex(Vertex Ver)
    {
        pos = Ver.Pos;
        nor = Ver.Nor;
        col = Ver.Col;
        tex = Ver.Tex;
        nearby = new List<int>();
        including = new List<int>();
        rust = 0.0f;
    }
    public Vertex(Vector3 p, Vector3 n)
    {
        pos = p;
        nor = n;
        col = new Color(1.0f, 1.0f, 1.0f);
        tex = new Vector2(0, 0);
        nearby = new List<int>();
        including = new List<int>();
        rust = 0.0f;
    }
    public void SetNearbyVertex(int v)
    {
        if (!nearby.Contains(v))
            nearby.Add(v);
    }
    public int GetNearbyVertex(int i)
    {
        return nearby[i];
    }
    public int GetNearbyCount()
    {
        return nearby.Count;
    }
    public void SetIncludingPolygon(int p)
    {
        if (!including.Contains(p))
            including.Add(p);
    }
    public int GetIncludingPolygon(int i)
    {
        return including[i];
    }
    public int GetIncludingCount()
    {
        return including.Count;
    }
    public Vector3 Pos
    {
        set { pos = value; }
        get { return pos; }
    }

    public Vector3 Nor
    {
        set { nor = value; }
        get { return nor; }
    }

    public Color Col
    {
        set { col = value; }
        get { return col; }
    }

    public Vector2 Tex
    {
        set { tex = value; }
        get { return tex; }
    }
    public float Rust
    {
        set { rust = value; }
        get { return rust; }
    }
}

public class HalfEdge
{
    private int oppositeVertex;
    private float liftingForce;
    private float bindingForce;
    private int end1;
    private int end2;
    private int pair;
    private int next;
    private int including;
    private bool connected;

    public HalfEdge(int P, int Q, int R)
    {
        oppositeVertex = P;
        liftingForce = 0.0f;
        bindingForce = 100.0f;
        end1 = Q;
        end2 = R;
        pair = -1;
        next = -1;
        including = -1;
        connected = true;
    }
    public HalfEdge(int P, int Q, int R, int inc)
    {
        oppositeVertex = P;
        liftingForce = 0.0f;
        bindingForce = 100.0f;
        end1 = Q;
        end2 = R;
        pair = -1;
        next = -1;
        including = inc;
        connected = true;
    }

    public int ReloadPair(List<HalfEdge> EList)
    {
        for (int i = 0; i < EList.Count; i++)
            if ((this.end1 == EList[i].End1 && this.end2 == EList[i].End2) || (this.end1 == EList[i].End2 && this.end2 == EList[i].End1))
            {
                if (this.oppositeVertex != EList[i].OppositeVertex)
                {
                    this.pair = i;
                    return i;
                }
            }
        return -1;
    }
    public int ReloadIncluding(List<TPolygon> TList)
    {
        for (int i = 0; i < TList.Count; i++)
        {
            TPolygon TPi = TList[i];
            if ((this.end1 == TPi.V1 && this.end2 == TPi.V2 && this.oppositeVertex == TPi.V3) || (this.end1 == TPi.V2 && this.end2 == TPi.V1 && this.oppositeVertex == TPi.V3)
             || (this.end1 == TPi.V2 && this.end2 == TPi.V3 && this.oppositeVertex == TPi.V1) || (this.end1 == TPi.V3 && this.end2 == TPi.V2 && this.oppositeVertex == TPi.V1)
             || (this.end1 == TPi.V3 && this.end2 == TPi.V1 && this.oppositeVertex == TPi.V2) || (this.end1 == TPi.V1 && this.end2 == TPi.V3 && this.oppositeVertex == TPi.V2)
               )
            {
                this.including = i;
                return i;
            }
        }
        Debug.Log("Including Polygon does NOT exsit.");
        return -1;
    }
    public int OppositeVertex
    {
        set { oppositeVertex = value; }
        get { return oppositeVertex; }
    }
    public float LiftingForce
    {
        set { liftingForce = value; }
        get { return liftingForce; }
    }
    public float BindingForce
    {
        set { bindingForce = value; }
        get { return bindingForce; }
    }
    public int End1
    {
        set { end1 = value; }
        get { return end1; }
    }
    public int End2
    {
        set { end2 = value; }
        get { return end2; }
    }
    public int Including
    {
        set { including = value; }
        get { return including; }
    }
    public int Pair
    {
        set { pair = value; }
        get { return pair; }
    }
    public int Next
    {
        set { next = value; }
        get { return next; }
    }
    public bool Connected
    {
        set { connected = value; }
        get { return connected; }
    }
}
public class TPolygon
{
    private int v1;
    private int v2;
    private int v3;
    private int t1;
    private int t2;
    private int t3;
    private float contractionForce;
    private Vector3 normal;
    private bool peeled;
    private int peelScale;

    public TPolygon(int u1, int u2, int u3)
    {
        v1 = u1;
        v2 = u2;
        v3 = u3;
        t1 = -1;
        t2 = -1;
        t3 = -1;
        contractionForce = 0.0f;
        normal = new Vector3(0f, 0f, -1f);
        peeled = false;
        peelScale = 0;
    }
    public TPolygon(int u1, int u2, int u3, Vector3 n)
    {
        v1 = u1;
        v2 = u2;
        v3 = u3;
        t1 = -1;
        t2 = -1;
        t3 = -1;
        contractionForce = 0.0f;
        normal = n;
        peeled = false;
        peelScale = 0;
    }
    public Vector3 ReloadNormal(List<Vertex> VList)
    {
        normal = Vector3.Cross(VList[v2].Pos - VList[v1].Pos, VList[v3].Pos - VList[v2].Pos).normalized;
        return normal;
    }
    public void ReloadTriangles(List<TPolygon> TList)
    {
        for (int i = 0; i < TList.Count; i++)
            if ((this.v2 == TList[i].v2 && this.v3 == TList[i].v3) || ((this.v2 == TList[i].v3 && this.v3 == TList[i].v2)))
            {
                if (this.v1 != TList[i].v1)
                {
                    this.t1 = i;
                    break;
                }
            }
        for (int i = 0; i < TList.Count; i++)
            if ((this.v3 == TList[i].v3 && this.v1 == TList[i].v1) || ((this.v3 == TList[i].v1 && this.v1 == TList[i].v3)))
            {
                if (this.v2 != TList[i].v2)
                {
                    this.t2 = i;
                    break;
                }
            }
        for (int i = 0; i < TList.Count; i++)
            if ((this.v1 == TList[i].v1 && this.v2 == TList[i].v2) || ((this.v1 == TList[i].v2 && this.v2 == TList[i].v1)))
            {
                if (this.v3 != TList[i].v3)
                {
                    this.t3 = i;
                    break;
                }
            }
    }

    public int V1
    {
        set { v1 = value; }
        get { return v1; }
    }
    public int V2
    {
        set { v2 = value; }
        get { return v2; }
    }
    public int V3
    {
        set { v3 = value; }
        get { return v3; }
    }
    public int T1
    {
        set { t1 = value; }
        get { return t1; }
    }
    public int T2
    {
        set { t2 = value; }
        get { return t2; }
    }
    public int T3
    {
        set { t3 = value; }
        get { return t3; }
    }
    public float ContractionForce
    {
        set { contractionForce = value; }
        get { return contractionForce; }
    }
    public Vector3 Normal
    {
        set { normal = value; }
        get { return normal; }
    }
    public bool Peeled
    {
        set { peeled = value; }
        get { return peeled; }
    }

    public int PeelScale
    {
        set { peelScale = value; }
        get { return peelScale; }
    }

}
public class MeshCreate : MonoBehaviour
{
    private float timeOut = 0.1f;
    private float timeElapsed = 0.0f;
    private int count = 0;
    private bool firstloop = true;

    [SerializeField]
    private GameObject m_object = null;
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Material _submat;

    private float _speed = 0.15f;
    private bool _simulating = true;
    private Vector3 _gravity = new Vector3(0.0f, -1.0f, 0.0f);
    private bool _vis = false;

    [SerializeField]
    private uint _seed = 1000;

    private Mesh _mesh;

    // (1) 頂点座標（この配列のインデックスが頂点インデックス）
    private List<Vertex> _vertices = new List<Vertex>();

    private Vector3[] _positions = new Vector3[]{                                                                    // position, normal, triangles, uvsは配列　ほかはリスト
    
    };
    private Vector3[] _Bpositions;

    // (2) ポリゴンを形成する頂点インデックスを順番に指定する
    private List<TPolygon> _tpolygons = new List<TPolygon>();
    private List<HalfEdge> _halfedges = new List<HalfEdge>();

    private int[] _triangles = new int[] { 0, 1, 2 };

    // (3) 法線
    private Vector3[] _normals = new Vector3[]{
        new Vector3(0, 0, -1),
        new Vector3(0, 0, -1),
        new Vector3(0, 0, -1)
    };
    private Vector3[] _Bnormals;

    // uv座標
    private Vector2[] _uvs = new Vector2[]{
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 0),
    };

    // uv座標
    private Color[] _colors = new Color[]{
        Color.white,
        Color.blue,
        Color.red,
    };

    private float Dist2BetweenPointAndLine(Vector3 P, Vector3 Q, Vector3 R)
    {
        Vector3 u = P - Q;
        Vector3 v = R - Q;
        float uu = Vector3.Dot(u, u);
        float vv = Vector3.Dot(v, v);
        float uv = Vector3.Dot(u, v);
        return (vv - uv * uv / uu);
    }

    // ##############################################################################################################################################################
    // ##############################################################################################################################################################
    private void Awake()
    {
        // 各リストの初期化

        _mesh = new Mesh();

        // Objファイル読込み start
        
        for (int i = 0; i < LoadText.Positions.Count; i++)
        {
            _vertices.Add(new Vertex(LoadText.Positions[i], LoadText.PNormals[i]));

        }
        for (int i = 0; i < LoadText.Polygons.Count; i++)
        {
            _tpolygons.Add(new TPolygon(LoadText.Polygons[i].x, LoadText.Polygons[i].y, LoadText.Polygons[i].z));
        }
        
        //Objファイル読込み end

        //Dorney subdivision Start

        _vertices.Add(new Vertex(new Vector3(0, 20, 0))); _vertices.Add(new Vertex(new Vector3(15, -10, 0))); _vertices.Add(new Vertex(new Vector3(-15, -10, 0)));
        _tpolygons.Add(new TPolygon(0, 1, 2));


        for (int i = _tpolygons.Count - 1; i >= 0; i--)
        {

            TPolygon TP = _tpolygons[i];

        }

        for (int i = 0; i < _tpolygons.Count; i++)
        {
            TPolygon TP = _tpolygons[i];

            _halfedges.Add(new HalfEdge(TP.V1, TP.V2, TP.V3, i));
            _halfedges.Add(new HalfEdge(TP.V2, TP.V3, TP.V1, i));
            _halfedges.Add(new HalfEdge(TP.V3, TP.V1, TP.V2, i));
            _halfedges[_halfedges.Count - 3].Next = _halfedges.Count - 2;
            _halfedges[_halfedges.Count - 2].Next = _halfedges.Count - 1;
            _halfedges[_halfedges.Count - 1].Next = _halfedges.Count - 3;

            _vertices[TP.V1].SetNearbyVertex(TP.V2);
            _vertices[TP.V1].SetNearbyVertex(TP.V3);
            _vertices[TP.V2].SetNearbyVertex(TP.V1);
            _vertices[TP.V2].SetNearbyVertex(TP.V3);
            _vertices[TP.V3].SetNearbyVertex(TP.V1);
            _vertices[TP.V3].SetNearbyVertex(TP.V2);
        }

        for (int i = 0; i < _halfedges.Count; i++)
        {
            _halfedges[i].ReloadPair(_halfedges);
            //_halfedges[i].ReloadIncluding(_tpolygons);
        }
        for (int i = 0; i < _halfedges.Count; i++)
        {
            HalfEdge HEi = _halfedges[i];

            if (HEi.Pair == -1) // 簡易曲率計算 & かける揚力計算
            {
                HEi.LiftingForce = 0.05f / Dist2BetweenPointAndLine(_vertices[HEi.OppositeVertex].Pos, _vertices[HEi.End1].Pos, _vertices[HEi.End2].Pos) * 2.0f;
            }
            else
            {
                bool convex = Vector3.Dot(_vertices[HEi.OppositeVertex].Nor - _vertices[_halfedges[HEi.Pair].OppositeVertex].Nor, _vertices[HEi.OppositeVertex].Pos - _vertices[_halfedges[HEi.Pair].OppositeVertex].Pos) > 0;
                float rad = Vector3.Dot(_vertices[HEi.OppositeVertex].Nor.normalized, _vertices[_halfedges[HEi.Pair].OppositeVertex].Nor.normalized);
                float d = Mathf.Clamp(rad, -1.0f, 1.0f);
                HEi.LiftingForce = 0.05f / Dist2BetweenPointAndLine(_vertices[HEi.OppositeVertex].Pos, _vertices[HEi.End1].Pos, _vertices[HEi.End2].Pos)
                    * (convex ? Mathf.PI + Mathf.Acos(d) : Mathf.PI - Mathf.Acos(d));
            }

        }


        for (int i = 0; i < _tpolygons.Count; i++)
            _tpolygons[i].ReloadTriangles(_tpolygons);
        for (int i = 0; i < _tpolygons.Count; i++)
            _tpolygons[i].ReloadNormal(_vertices);
    }
    // ##############################################################################################################################################################
    // ##############################################################################################################################################################

    public void Hit(Vector3 pos)
    {
        for (int i = 0; i < _vertices.Count; i++)
        {
            //pos = new Vector3(-0.0f, 1.5f, 0.0f);    // パーティクル衝突位置付近で焦げ目がつくようにしたかったんだけど
            // unityの衝突位置判定がガバ過ぎてうまくいかなかったので，焦げ目中心を固定しちゃいました

            //pos = pos + new Vector3(-3.0f, 1.5f, 0.0f); //this.GetComponent<Transform>().position;
            pos = new Vector3(1.0f, 0.5f, 0.0f);
            float delta = Mathf.Exp((-Vector3.Distance(_vertices[i].Pos, pos)) * 1.5f);
            _vertices[i].Pos += 0.03f * delta * _vertices[i].Nor;

            delta = Mathf.Exp((-Vector3.Distance(_vertices[i].Pos, pos) * 3.0f));
            _vertices[i].Col += (new Color(0.5f, 0.2f, 0.1f) - _vertices[i].Col) * (delta) * 0.25f;
            delta = Mathf.Exp((-Vector3.Distance(_vertices[i].Pos, pos) * 4.0f));
            _vertices[i].Col += (new Color(0.0f, 0.0f, 0.0f) - _vertices[i].Col) * (delta) * 0.5f;
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        Array.Resize(ref _triangles, _tpolygons.Count * 3);

        for (int i = 0; i < _tpolygons.Count; i++)  // 三角ポリゴンごとの処理
        {
            TPolygon TPi = _tpolygons[i];

        }

        for (int i = 0; i < _halfedges.Count; i++)  // ハーフエッジごとの処理
        {
            HalfEdge HEi = _halfedges[i];
            HalfEdge HEp = HEi.Pair == -1 ? HEi : _halfedges[HEi.Pair];
            TPolygon HEiP = _tpolygons[HEi.Including];
            TPolygon HEpP = _tpolygons[HEp.Including];
        }

        for (int i = 0; i < _tpolygons.Count; i++)      // ポリゴンの最終処理：ポリゴン座標をメッシュに代入
        {
                _triangles[i * 3] = _tpolygons[i].V1;
                _triangles[i * 3 + 1] = _tpolygons[i].V2;
                _triangles[i * 3 + 2] = _tpolygons[i].V3;
        }

        Array.Resize(ref _positions, _vertices.Count);
        Array.Resize(ref _normals, _vertices.Count);
        Array.Resize(ref _uvs, _vertices.Count);
        Array.Resize(ref _colors, _vertices.Count);

        // 位置計算
        for (int i = 0; i < _vertices.Count && _simulating; i++)
            _positions[i] = _vertices[i].Pos * this.GetComponent<Transform>().localScale.x / 3.5f + this.GetComponent<Transform>().position;

        // 法線計算
        int[] Nface = new int[_vertices.Count];
        Vector3[] NormalSum = new Vector3[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++) Nface[i] = 0;
        for (int i = 0; i < _tpolygons.Count; i++)
        {
            TPolygon TP = _tpolygons[i];
            NormalSum[TP.V1] += TP.Normal; Nface[TP.V1]++;
            NormalSum[TP.V2] += TP.Normal; Nface[TP.V2]++;
            NormalSum[TP.V3] += TP.Normal; Nface[TP.V3]++;
        }
        for (int i = 0; i < _vertices.Count; i++)
        {
            _vertices[i].Nor = NormalSum[i] / Nface[i];
            _normals[i] = _vertices[i].Nor;

        }

        // 収縮力計算 （デバッグ用）
        // Debug.Log("Contract: " + _tpolygons[100].ContractionForce);
        int[] Cface = new int[_vertices.Count];
        float[] ContractSum = new float[_vertices.Count];
        for (int i = 0; i < _vertices.Count; i++) Cface[i] = 0;
        for (int i = 0; i < _tpolygons.Count; i++)
        {
            TPolygon TP = _tpolygons[i];
            ContractSum[TP.V1] += TP.ContractionForce; Cface[TP.V1]++;
            ContractSum[TP.V2] += TP.ContractionForce; Cface[TP.V2]++;
            ContractSum[TP.V3] += TP.ContractionForce; Cface[TP.V3]++;
        }
        for (int i = 0; i < _vertices.Count; i++)
        {
            _vertices[i].Rust = ContractSum[i] / Cface[i];

        }

        Array.Resize(ref _uvs, _positions.Length);
        if (Input.GetKeyDown(KeyCode.V))
            _vis = !_vis;

        for (int i = 0; i < _positions.Length; i++)
                _uvs[i] = new Vector2((_positions[i].x + 5) / 10.0f, (_positions[i].y + 5) / 10.0f);

        for (int i = 0; i < _vertices.Count; i++)
        {
            //_colors[i] = new Color(132.0f / 255.0f, 51.0f / 255.0f, 0.0f, 1.0f) + new Color(123.0f / 255.0f, 204.0f / 255.0f, 1.0f, 0.0f) * Mathf.Exp(-(_vertices[i].Rust > 10.0f ? _vertices[i].Rust - 10.0f : 0.0f) / 100.0f);
            _colors[i] = _vertices[i].Col;
            // _colors[i] = Color.white;
        }

        // (4) Meshに頂点情報を代入
        _mesh.vertices = _positions;
        _mesh.triangles = _triangles;
        _mesh.normals = _normals;
        _mesh.uv = _uvs;
        _mesh.colors = _colors;

        _mesh.RecalculateBounds();

        // (5) 描画
        Graphics.DrawMesh(_mesh, Vector3.zero, Quaternion.identity, _material, 0);

    }
}