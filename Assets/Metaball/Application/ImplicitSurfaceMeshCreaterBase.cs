//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public abstract class ImplicitSurfaceMeshCreaterBase : MonoBehaviour {
    
    // cell size of grid (marching cubes algorithm)
    public float gridSize = 0.2f;

    public float GridSize
    {
        get { return Mathf.Max(float.Epsilon, gridSize); }
    }

    [Tooltip("Ignore gridSize and use automatically determined value by autoGridQuarity")]
    public bool bAutoGridSize = false;

    [Range(0.005f, 1.0f)]
    public float autoGridQuarity = 0.2f;

    // guide for texture coordinates
    public MetaballUVGuide uvProjectNode;

    // threshold determining surface from scalar field
    public float powerThreshold = 0.15f;


    // build reverse surface?
    public bool bReverse = false;

    // fixed bounds, used only if "bUseFixedBounds" is true
    public Bounds fixedBounds = new Bounds(Vector3.zero, Vector3.one * 10);


    public abstract void CreateMesh();

    public abstract Mesh Mesh
    {
        get;
        set;
    }

    protected virtual void Update()
    {
    }

    protected void GetUVBaseVector(out Vector3 uDir, out Vector3 vDir, out Vector3 offset)
    {
        if (uvProjectNode != null)
        {
            float uScale = Mathf.Max(uvProjectNode.uScale, 0.001f);
            float vScale = Mathf.Max(uvProjectNode.vScale, 0.001f);

            uDir = uvProjectNode.transform.right / uScale;
            vDir = uvProjectNode.transform.up / vScale;

            offset = -uvProjectNode.transform.localPosition;
        }
        else
        {
            uDir = Vector3.right;
            vDir = Vector3.up;

            offset = Vector3.zero;
        }
    }
}
