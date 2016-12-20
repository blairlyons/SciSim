//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class ImplicitSurface : ImplicitSurfaceMeshCreaterBase{

    public MeshFilter meshFilter;
    
    public MeshFilter MeshFilter
    {
        get
        {
            if( meshFilter == null )
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            return meshFilter;
        }
    }

    public MeshCollider meshCollider;

    protected Vector3[] _positionMap;
    protected float[] _powerMap;
    protected float[] _powerMapMask;

    protected int _countX;
    protected int _countY;
    protected int _countZ;

    bool _bMapsDirty = true;

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

    protected void ResetMaps()
    {
        int maxCellCount = MetaballBuilder.MaxGridCellCount;

        float c = 1.0f;
        // decide cell size
        if( bAutoGridSize )
        {
            int targetCellCount = (int)(maxCellCount * Mathf.Clamp01(autoGridQuarity));
            c = Mathf.Pow(fixedBounds.size.x * fixedBounds.size.y * fixedBounds.size.z / targetCellCount, 1.0f / 3.0f);
        }
        else
        {
            c = gridSize;
        }

        int halfResolutionX = (int)Mathf.CeilToInt(fixedBounds.extents.x / c) + 1;
        int halfResolutionY = (int)Mathf.CeilToInt(fixedBounds.extents.y / c) + 1;
        int halfResolutionZ = (int)Mathf.CeilToInt(fixedBounds.extents.z / c) + 1;

        _countX = halfResolutionX * 2;
        _countY = halfResolutionY * 2;
        _countZ = halfResolutionZ * 2;

        Vector3 actualExtent = new Vector3(halfResolutionX * c, halfResolutionY * c, halfResolutionZ * c);

        Vector3 gridOrigin = fixedBounds.center - actualExtent;

        int gridStrideY = _countX;
        int gridStrideZ = _countX * _countY;

        int count = _countX * _countY * _countZ;

        _positionMap = new Vector3[count];
        _powerMap = new float[count];
        _powerMapMask = new float[count];

        for (int i = 0; i < count;++i )
        {
            _powerMap[i] = 0.0f;
        }

        for (int z = 0; z < _countZ; ++z)
        {
            for (int y = 0; y < _countY; ++y)
            {
                for (int x = 0; x < _countX; ++x)
                {
                    int idx = x + y * gridStrideY + z * gridStrideZ;
                    _positionMap[idx] = gridOrigin + new Vector3(c * x, c * y, c * z);
     //               _powerMap[idx] = 0.0f;

                    if (z == 0 || z == _countZ - 1 || y == 0 || y == _countY - 1 || x == 0 || x == _countX - 1)
                    {
                        _powerMapMask[idx] = 0.0f;
                    }
                    else
                    {
                        _powerMapMask[idx] = 1.0f;
                    }
                }
            }
        }

		InitializePowerMap ();

        _bMapsDirty = false;
    }

	protected virtual void InitializePowerMap()
	{
		int count = _countX * _countY * _countZ;

		for (int i=0; i<count; ++i)
		{
			_powerMap[i] = 0.0f;
		}
	}

    public override void CreateMesh()
    {
        if( _bMapsDirty )
        {
            ResetMaps();
        }

        Vector3 uDir, vDir, uvOffset;

        GetUVBaseVector(out uDir, out vDir, out uvOffset);

        Mesh mesh = MetaballBuilder.Instance.CreateImplicitSurfaceMesh(_countX, _countY, _countZ,
            _positionMap, _powerMap, bReverse, powerThreshold, uDir, vDir, uvOffset);

        mesh.RecalculateBounds();
        Mesh = mesh;

        if( meshCollider != null )
        {
            meshCollider.sharedMesh = mesh;
        }
    }
    /*
    [ContextMenu("Reset")]
    public void Reset()
    {
        ResetMaps();
        CreateMesh();
    }
    */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(fixedBounds.center + transform.position, fixedBounds.size);
    }
}
