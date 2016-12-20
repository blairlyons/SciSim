//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class SkinnedMetaballSeed : MetaballSeedBase
{
    public SkinnedMeshRenderer skinnedMesh;

    SkinnedMetaballCell _rootCell;
    
    [ContextMenu("CreateMesh")]
    public override void CreateMesh()
    {
        CleanupBoneRoot();

        _rootCell = new SkinnedMetaballCell();
        _rootCell.radius = sourceRoot.Radius;
        _rootCell.tag = sourceRoot.gameObject.name;
        _rootCell.density = sourceRoot.Density;
        _rootCell.modelPosition = sourceRoot.transform.position - transform.position;

        Matrix4x4 toLocalMtx = skinnedMesh.transform.worldToLocalMatrix;
        ConstructTree(sourceRoot.transform, _rootCell, toLocalMtx);
                
        Mesh mesh;
        Transform[] bones;

        Vector3 uDir;
        Vector3 vDir;
        Vector3 uvOffset;

        GetUVBaseVector(out uDir, out vDir, out uvOffset);

        Bounds? bounds = null;
        if( bUseFixedBounds )
        {
            bounds = fixedBounds;
        }
        _errorMsg = MetaballBuilder.Instance.CreateMeshWithSkeleton(_rootCell, boneRoot.transform, powerThreshold, GridSize, uDir, vDir, uvOffset, out mesh, out bones, cellObjPrefab, bReverse,
            bounds, bAutoGridSize, autoGridQuarity);

        if( !string.IsNullOrEmpty( _errorMsg ) )
        {
            Debug.LogError("MetaballError : " + _errorMsg);
            return;
        }

        mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 500.0f);

        skinnedMesh.bones = bones;
        skinnedMesh.sharedMesh = mesh;
        skinnedMesh.localBounds = new Bounds(Vector3.zero, Vector3.one * 500.0f);

        skinnedMesh.rootBone = boneRoot;

        EnumBoneNodes();
    }

    void ConstructTree( Transform node, SkinnedMetaballCell cell, Matrix4x4 toLocalMtx )
    {
        for( int i=0; i<node.childCount; ++i )
        {
            Transform c = node.GetChild(i);
            MetaballNode n = c.GetComponent<MetaballNode>();

            if (n != null)
            {
                SkinnedMetaballCell childCell = cell.AddChild( toLocalMtx * ( c.transform.position - transform.position ), n.Radius, 0.0f);
                childCell.tag = c.gameObject.name;
                childCell.density = n.Density;
                ConstructTree(c, childCell, toLocalMtx);
            }
        }
    }

    public override Mesh Mesh
    {
        get
        {
            return skinnedMesh.sharedMesh;
        }
        set
        {
            skinnedMesh.sharedMesh = value;
        }
    }

    public override bool IsTreeShape
    {
        get { return true; }
    }
}
