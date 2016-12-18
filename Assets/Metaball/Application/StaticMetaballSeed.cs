//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class StaticMetaballSeed : MetaballSeedBase
{
    public MeshFilter meshFilter;

    MetaballCellCluster _cellCluster;
    
    void ConstructCellCluster( MetaballCellCluster cluster, Transform parentNode, Matrix4x4 toLocalMtx )
    {
        for (int i = 0; i < parentNode.childCount; ++i)
        {
            Transform c = parentNode.GetChild(i);

            MetaballNode n = c.GetComponent<MetaballNode>();

            if (n != null)
            {
                MetaballCell cell = _cellCluster.AddCell(toLocalMtx * (c.position - transform.position), 0.0f, n.Radius, c.gameObject.name);
                cell.density = n.Density;
            }

            ConstructCellCluster(cluster, c, toLocalMtx);
        }
    }

    [ContextMenu("CreateMesh")]
    public override void CreateMesh()
    {
        CleanupBoneRoot();

        _cellCluster = new MetaballCellCluster();

        Matrix4x4 toLocalMtx = meshFilter.transform.worldToLocalMatrix;
        ConstructCellCluster(_cellCluster, sourceRoot.transform, toLocalMtx);
  
        Mesh mesh;

        Vector3 uDir;
        Vector3 vDir;
        Vector3 uvOffset;

        GetUVBaseVector(out uDir, out vDir, out uvOffset);

        Bounds? bounds = null;
        if (bUseFixedBounds)
        {
            bounds = fixedBounds;
        }
        _errorMsg = MetaballBuilder.Instance.CreateMesh(_cellCluster, boneRoot.transform, powerThreshold, GridSize, uDir, vDir, uvOffset, out mesh, cellObjPrefab, bReverse,
            bounds, bAutoGridSize, autoGridQuarity);

        if (!string.IsNullOrEmpty(_errorMsg))
        {
            Debug.LogError("MetaballError : " + _errorMsg);
            return;
        }

        mesh.RecalculateBounds();

        meshFilter.sharedMesh = mesh;

        EnumBoneNodes();
    }

    public override Mesh Mesh
    {
        get
        {
            return meshFilter.sharedMesh;
        }
        set
        {
            meshFilter.sharedMesh = value;
        }
    }

    public override bool IsTreeShape
    {
        get { return false; }
    }
}
