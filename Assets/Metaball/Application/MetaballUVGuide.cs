//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class MetaballUVGuide : MonoBehaviour {

    public float uScale = 1.0f;
    public float vScale = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmosSelected()
    {
        Matrix4x4 oldMtx = Gizmos.matrix;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(new Vector3(uScale*0.5f, vScale*0.5f, 0.0f), new Vector3(uScale, vScale, 15.0f));

        Gizmos.matrix = oldMtx;
    }
}
