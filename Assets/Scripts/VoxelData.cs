using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    // Vertices of a cube.
    public static readonly Vector3[] verts = new Vector3[8] {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 1.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 1.0f)
    };

    // Faces of a cube.
    public static readonly int[,] tris = new int[6, 4]
    {
        {0, 3, 1, 2 }, // Back face.
        {5, 6, 4, 7 }, // Front face.
        {3, 7, 2, 6 }, // Top face.
        {1, 5, 0, 4 }, // Bottom face.
        {4, 7, 0, 3 }, // Left face.
        {1, 2, 5, 6 }  // Right face.
    };

    public static readonly Vector2[] uvs = new Vector2[4] {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f)
    };

    public static readonly int chunkWidth = 16;
    public static readonly int chunkHeight = 128;

    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f,  0.0f,  -1.0f),
        new Vector3(0.0f,  0.0f,  1.0f),
        new Vector3(0.0f,  1.0f,  0.0f),
        new Vector3(0.0f,  -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f,  0.0f),
        new Vector3(1.0f,  0.0f,  0.0f)
    };

    public static readonly int TextureAtlasSizeInBlocks = 4;
    public static float NormalizedBlockTextureSize
    {
        get { return 1f / (float) TextureAtlasSizeInBlocks; }
    }

    public static readonly int WorldSizeInChunks = 32;

    public static int WorldSizeInVoxels
    {
        get { return WorldSizeInChunks * chunkWidth; }
    }

    public static readonly int ViewDistanceInChunks = 10;

	public static readonly int ChunkWidth = 16;
	public static readonly int ChunkHeight = 96;
}