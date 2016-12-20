//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class MetaballNode : MonoBehaviour {

    // Radius of this metaball node. Actual radius is affected by tranform.scale
    public float baseRadius = 1.0f;

    // If "subtract" mode
    public bool bSubtract = false;


    MetaballSeedBase _seed;


    // for editor
    Mesh _boneMesh;

    public virtual float Density
    {
        get { return bSubtract ? -1.0f : 1.0f; }
    }

    public float Radius
    {
        get { return /*transform.localScale.x * */ baseRadius; }
    }

    void OnDrawGizmosSelected()
    {
        if (_seed == null)
        {
            _seed = Utils.FindComponentInParents<MetaballSeedBase>(transform);
        }

        if( Density == 0.0f )
        {
            return;
        }

        // static seed root node is not shown nor used for building mesh
        if( _seed != null /*&& !_seed.IsTreeShape*/ && _seed.sourceRoot != null && _seed.sourceRoot.gameObject == gameObject )
        {
            return;
        }

        {
            Gizmos.color = bSubtract ? Color.red : Color.white;

            float drawRadius = Radius;

            if( _seed != null )
            {
                drawRadius *= (1.0f- Mathf.Sqrt(_seed.powerThreshold));
            }

            Matrix4x4 oldMtx = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.DrawWireSphere(Vector3.zero, drawRadius);

            MetaballNode parentNode = transform.parent.GetComponent<MetaballNode>();

            if (parentNode != null && parentNode.Density != 0.0f && _seed != null && _seed.IsTreeShape )
            {
                if( _boneMesh == null )
                {
                    _boneMesh = new Mesh();

                    Vector3[] verts = new Vector3[5];
                    Vector3[] normals = new Vector3[5];
                    int[] indxs = new int[6];

                    verts[0] = new Vector3(0.1f, 0.0f, 0.0f);
                    verts[1] = new Vector3(-0.1f, 0.0f, 0.0f);
                    verts[2] = new Vector3(0.0f, 0.1f, 0.0f);
                    verts[3] = new Vector3(0.0f, -0.1f, 0.0f);
                    verts[4] = new Vector3(0.0f, 0.0f, 1.0f);

                    normals[0] = new Vector3(0, 0, 1.0f);
                    normals[1] = new Vector3(0, 0, 1.0f);
                    normals[2] = new Vector3(0, 0, 1.0f);
                    normals[3] = new Vector3(0, 0, 1.0f);
                    normals[4] = new Vector3(0, 0, 1.0f);

                    indxs[0] = 0;
                    indxs[1] = 1;
                    indxs[2] = 4;
                    indxs[3] = 2;
                    indxs[4] = 3;
                    indxs[5] = 4;

                    _boneMesh.vertices = verts;
                    _boneMesh.normals = normals;
                    _boneMesh.SetIndices(indxs, MeshTopology.Triangles, 0);
                }
                Vector3 scale = Vector3.one;
                Vector3 pos = transform.position;
                Vector3 parentPos = transform.parent.position;

                if ((parentPos - pos).sqrMagnitude < float.Epsilon)
                {

                }
                else
                {
                    scale *= ((parentPos - pos).magnitude);

                    Matrix4x4 mtx = Matrix4x4.TRS(parentPos, Quaternion.LookRotation(pos-parentPos), scale);

                    Gizmos.color = Color.blue;
                    Gizmos.matrix = mtx;
                    Gizmos.DrawWireMesh(_boneMesh);
                }
            }

            Gizmos.color = Color.white;
            Gizmos.matrix = oldMtx;
        }
    }
}
