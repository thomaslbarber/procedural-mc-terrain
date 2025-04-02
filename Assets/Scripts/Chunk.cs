using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public ChunkCoord coord;

    GameObject chunkObj;
    MeshRenderer renderer;
    MeshFilter filter;

    private int vertIndex = 0;
    private List<Vector3> verts = new List<Vector3>();
    private List<int> tris = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    public byte[,,] map = new byte[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];

    World world;

    public Chunk(ChunkCoord coord, World world)
    {
        this.coord = coord;
        this.world = world;

        chunkObj = new GameObject();
        filter = chunkObj.AddComponent<MeshFilter>();

        renderer = chunkObj.AddComponent<MeshRenderer>();
        renderer.material = world.material;

        chunkObj.transform.SetParent(world.transform);
        chunkObj.transform.position = new Vector3(coord.x * VoxelData.chunkWidth, 0.0f, coord.z * VoxelData.chunkWidth);
        chunkObj.name = "Chunk " + coord.x + ", " + coord.z;

        PopulateMap();
        CreateMeshData();
        CreateMesh();
    }

    void PopulateMap()
    {
        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.chunkWidth; z++)
                {
                    map[x, y, z] = world.GetVoxel(new Vector3(x, y, z) + Position);
                }
            }
        }
    }

    void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.chunkWidth; z++)
                {
                    if (world.blockTypes[map[x, y, z]].isSolid)
                    {
                        AddDataToChunk(new Vector3(x, y, z));
                    }
                }
            }
        }
    }

    public bool IsActive
    {
        get { return chunkObj.activeSelf; }
        set { chunkObj.SetActive(value); }
    }

    public Vector3 Position
    {
        get { return chunkObj.transform.position; }
    }

    bool VoxelInChunk(int x, int y, int z)
    {
        if (x < 0 || x > VoxelData.chunkWidth - 1) { return false; } // X boundaries.
        if (y < 0 || y > VoxelData.chunkHeight - 1) { return false; } // Y boundaries.
        if (z < 0 || z > VoxelData.chunkWidth - 1) { return false; } // Z boundaries.

        return true;
    }

    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);

        if (!VoxelInChunk(x, y, z)) { return world.blockTypes[world.GetVoxel(pos + Position)].isSolid; }

        return world.blockTypes[map[x, y, z]].isSolid;
    }

    void AddDataToChunk(Vector3 pos)
    {
        for (int i = 0; i < 6; i++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[i]))
            {
                byte blockID = map[(int)pos.x, (int)pos.y, (int)pos.z];

                verts.Add(pos + VoxelData.verts[VoxelData.tris[i, 0]]);
                verts.Add(pos + VoxelData.verts[VoxelData.tris[i, 1]]);
                verts.Add(pos + VoxelData.verts[VoxelData.tris[i, 2]]);
                verts.Add(pos + VoxelData.verts[VoxelData.tris[i, 3]]);

                AddTexture(world.blockTypes[blockID].GetTextureID(i));

                tris.Add(vertIndex);
                tris.Add(vertIndex + 1);
                tris.Add(vertIndex + 2);
                tris.Add(vertIndex + 2);
                tris.Add(vertIndex + 1);
                tris.Add(vertIndex + 3);

                vertIndex += 4;
            }
        }
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.SetUVs(0, uvs.ToArray());

        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }

    void AddTexture(int textureID)
    {
        float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}