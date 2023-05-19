using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGridGenerator : MonoBehaviour {

    public bool rewriteVertexStreams = true;
    public float particleSize = 1f;
    public Color particleColor = Color.white;
    public Vector3 particleRotation3D;
    public bool randomColorAlpha = true; // For MetallicSmoothness random offset
    public float xDistance = 0.25f;
    public float yDistance = 0.25f;
    public float zDistance = 0.25f;
    public int xSize = 10;
    public int ySize = 10;
    public int zSize = 10;
    public float OffsetEven = 0.125f;
    public bool updateEveryFrame = false;

    private float even;
    public Vector3[] positions;
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;
    private List<Vector4> customData = new List<Vector4>();
    private List<Vector4> customData2 = new List<Vector4>();

    void Start () {
        ps = GetComponent<ParticleSystem>();
        UpdateGrid();
    }

    private void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        UpdateGrid();
    }

    public void UpdateGrid()
    {
        GenerateGrid();
        GenerateParticles();
        CreateOffsetVector();

        ParticleSystemRenderer psrend = GetComponent<ParticleSystemRenderer>();
        if (rewriteVertexStreams == true)
        {
            psrend.SetActiveVertexStreams(new List<ParticleSystemVertexStream>(
                new ParticleSystemVertexStream[] {
                    ParticleSystemVertexStream.Position, ParticleSystemVertexStream.Normal,
                    ParticleSystemVertexStream.Color, ParticleSystemVertexStream.UV,
                    ParticleSystemVertexStream.Center, ParticleSystemVertexStream.Tangent, 
                    ParticleSystemVertexStream.Custom1XYZ }));

        }
        psrend.alignment = ParticleSystemRenderSpace.Local;
    }

    // Generating array of positions
    private void GenerateGrid()
    {
        //positions = new Vector3[xSize * ySize * zSize];
        //for (int z = 0, i = 0; z < zSize; z++)
        //{
        //    even = 0f;
        //    if (z % 2 == 0)
        //    {
        //        even = OffsetEven;
        //    }
        //    for (int y = 0; y < ySize; y++)
        //    {
        //        for (int x = 0; x < xSize; x++, i++)
        //        {
        //            positions[i] = new Vector3(x * xDistance + even, y * yDistance, z * zDistance);
        //        }
        //    }
        //}

        positions = InitMethod(30, 0.25f);
    }

    /// <summary>
    /// 按圈生成一个正六边形排列的位置数组
    /// </summary>
    /// <param name="_roundCount"></param>
    /// <param name="_radius"></param>
    /// <returns></returns>
    Vector3[] InitMethod(int _roundCount, float _radius)
    {
        Vector3[] Pos_6 = new Vector3[6];

        List<Vector3> posList = new List<Vector3>();
        posList.Add(transform.position);

        for (int round = 1; round <= _roundCount; round++)
        {
            //每一层环的循环体
            for (int id = 0; id < 6; id++)
            {
                float angle = 60.0f * id * Mathf.PI / 180.0f;
                //放置每一环的顶点物体
                //记录6个正确位置
                Pos_6[id] = new Vector3(Mathf.Sin(angle) * _radius * round, 0f, Mathf.Cos(angle) * _radius * round) + transform.position;
                //生成物体
                posList.Add(Pos_6[id]);
            }
            //第2圈开始执行插入
            if (round > 1)
            {
                //逐个区间插入物体
                for (int id = 0; id < 6; id++)
                {
                    //获取下一个位置ID,在0~5中循环取值
                    int NextID = (id + 1) % 6;
                    //单位朝向(下一个点-当前点)
                    Vector3 Orientation = Vector3.Normalize(Pos_6[NextID] - Pos_6[id]);
                    for (int addID = 1; addID < round; addID++)
                    {
                        //插入点 = 单位方向*当前偏移距离 + 起点偏移
                        Vector3 Insert_Pos = Orientation * (_radius * addID) + Pos_6[id];
                        posList.Add(Insert_Pos);
                    }
                }
            }
        }
        return posList.ToArray();
    }

    // Generating particles with grid based positions
    private void GenerateParticles()
    {
        particles = new ParticleSystem.Particle[positions.Length];
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].position = positions[i];
            if (randomColorAlpha == true)
                particleColor.a = Random.Range(0f, 1f);
            particles[i].startColor = particleColor;
            particles[i].startSize = particleSize;
            particles[i].rotation3D = particleRotation3D;
        }
        ps.SetParticles(particles, particles.Length);


        //particles = new ParticleSystem.Particle[xSize * ySize * zSize];
        //for (int i = 0; i < particles.Length; i++)
        //{
        //    particles[i].position = positions[i];
        //    if (randomColorAlpha == true)
        //    particleColor.a = Random.Range(0f, 1f);
        //    particles[i].startColor = particleColor;
        //    particles[i].startSize = particleSize;
        //    particles[i].rotation3D = particleRotation3D;
        //}
        //ps.SetParticles(particles, particles.Length);
    }

    // Creating Vector for Offset
    private void CreateOffsetVector()
    {
        ps.GetCustomParticleData(customData, ParticleSystemCustomData.Custom1);        

        for (int i = 0; i < particles.Length; i++)
        {
            customData[i] = this.gameObject.transform.up;
        }

        ps.SetCustomParticleData(customData, ParticleSystemCustomData.Custom1);        
    }

    private void FixedUpdate()
    {
        if (updateEveryFrame == true)
        {
            UpdateGrid();
        }
    }
}
