//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MetaballSeedBase : ImplicitSurfaceMeshCreaterBase {

    // root node of constructed "skeleton"
    public Transform boneRoot;

    // root node of metaball graph
    public MetaballNode sourceRoot;

    // prefab for nodes in "skeleton"
    public MetaballCellObject cellObjPrefab;

    // defaultRadius used in Editor
    public float baseRadius = 1.0f;

    // build reverse surface?
    //public bool bReverse = false;

    // use fixed bounds?
    /// <summary>
    /// If fixed bounds enabled,
    /// - Mesh never goes out of the bounds
    /// - Existing vertices do not move at metaball node added/removed (if far enough from the new node). 
    ///   Reccomended for runtime use that requires continuity through metaball shape change.
    ///   (Like the "dungeon" sample in this package).
    /// </summary>
    public bool bUseFixedBounds = false;
    
    protected string _errorMsg;
   

    public abstract bool IsTreeShape { get; }

    [SerializeField]
    GameObject[] _boneNodes = new GameObject[0];

    void OnDrawGizmos()
    {
        if( bUseFixedBounds )
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(fixedBounds.center+transform.position, fixedBounds.size);
        }
    }

    void OnGUI()
    {
        if (!string.IsNullOrEmpty(_errorMsg))
        {
            GUILayout.Label("MetaballError : " + _errorMsg);
        }
    }

    protected void EnumBoneNodes()
    {
        List<GameObject> list = new List<GameObject>();
        EnumerateGameObjects(boneRoot.gameObject, list);

        _boneNodes = list.ToArray();
    }

    void EnumerateGameObjects( GameObject parent, List<GameObject> list )
    {
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            list.Add(child);
            EnumerateGameObjects(child, list);
        }
    }

    protected void CleanupBoneRoot()
    {
        if( _boneNodes == null )
        {
            _boneNodes = new GameObject[0];
        }
        int count = _boneNodes.Length;
        for( int i=0; i<count; ++i )
        {
            if( _boneNodes[i] == null )
            {
                continue;
            }
            _boneNodes[i].transform.DetachChildren();

#if UNITY_EDITOR
            DestroyImmediate(_boneNodes[i]);
#else
            Destroy(_boneNodes[i]);
#endif
        }
    }
}
