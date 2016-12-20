//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class DungeonControl : MonoBehaviour {

    public Camera myCamera;
    public StaticMetaballSeed metaball;
    public ParticleSystem hitPS;
    public AudioSource audioSource;

	// Use this for initialization
	void Start () {

        MeshCollider mc = metaball.GetComponent<MeshCollider>();
        if (mc != null)
        {
            mc.sharedMesh = metaball.Mesh;
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void AddCell( Vector3 position, float size )
    {
        audioSource.Play();

        GameObject child = new GameObject("MetaballNode");
        child.transform.parent = metaball.sourceRoot.transform;
        child.transform.position = position;
        child.transform.localScale = Vector3.one;
        child.transform.localRotation = Quaternion.identity;

        MetaballNode newNode = child.AddComponent<MetaballNode>();
        newNode.baseRadius = size;

        metaball.CreateMesh();

        MeshCollider mc = metaball.GetComponent<MeshCollider>();

        if (mc != null)
        {
            mc.sharedMesh = metaball.Mesh;
        }

        Instantiate(hitPS.gameObject, position, Quaternion.identity);
    }
}
