//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils {
	public static void DestroyChildren( Transform parent )
	{
        int childCount = parent.childCount;
        GameObject[] children = new GameObject[childCount];

        for (int i = 0; i < childCount; ++i)
        {
            children[i] = parent.GetChild(i).gameObject;
        }

        parent.DetachChildren();

        for (int i = 0; i < childCount; ++i)
        {
    #if UNITY_EDITOR
            GameObject.DestroyImmediate(children[i]);
    #else
            GameObject.Destroy(children[i]);
    #endif
        }
    }

	
	public static T StringToEnumType<T>(string value, T defaultValue)
	{
		T retval;
		
		try
		{
			if( string.IsNullOrEmpty(value) )
			{
				retval = defaultValue;
			}
			else
			{
				retval = (T)System.Enum.Parse(typeof(T), value);
			}
		}
		catch( System.ArgumentException e )
		{
			throw new UnityException(e.Message + System.Environment.NewLine + "failed to parse string ["+value+"] -> enum type ["+typeof(T).ToString()+"]");
		}
		
		return retval;
	}
	
	public static List<T> GetComponentsRecursive<T>( Transform t )
		where T : Component
	{
		List<T> retval = new List<T>();
		
		T mine = t.GetComponent<T>();
		if( mine != null )
		{
			retval.Add(mine);
		}
		
		for( int i=0, imax=t.childCount; i<imax; ++i )
		{
			retval.AddRange( GetComponentsRecursive<T>( t.GetChild(i) ) );
		}
		
		return retval;
	}

    public static T FindComponentInParents<T>( Transform t )
        where T : Component
    {
        T retval = t.GetComponent<T>();
        if( retval != null )
        {
            return retval;
        }
        else if(t.parent != null)
        {
            return FindComponentInParents<T>(t.parent);
        }
        else
        {
            return null;
        }
    }

    public static void ConvertMeshIntoWireFrame( Mesh mesh )
    {
        MeshTopology mt = mesh.GetTopology(0);
        if (mt != MeshTopology.Triangles)
        {
            return;
        }

        int[] oldIndices = mesh.GetIndices(0);
        int [] newIndices = new int[oldIndices.Length*2];

        for (int triIdx = 0; triIdx < oldIndices.Length / 3; ++triIdx)
        {
            int idx0 = oldIndices[triIdx * 3];
            int idx1 = oldIndices[triIdx * 3 + 1];
            int idx2 = oldIndices[triIdx * 3 + 2];

            newIndices[triIdx * 6 + 0] = idx0;
            newIndices[triIdx * 6 + 1] = idx1;

            newIndices[triIdx * 6 + 2] = idx1;
            newIndices[triIdx * 6 + 3] = idx2;

            newIndices[triIdx * 6 + 4] = idx2;
            newIndices[triIdx * 6 + 5] = idx0;
        }

        mesh.SetIndices(newIndices, MeshTopology.Lines, 0);
    }
}
