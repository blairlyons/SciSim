//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class DungeonControl2 : MonoBehaviour {

    public Camera myCamera;
    public IncrementalModeling metaball;
//    public ParticleSystem hitPS;
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

    public void Attack( IMBrush brush /*, Vector3 position*/ )
    {
        audioSource.Play();

		brush.Draw ();

        MeshCollider mc = metaball.GetComponent<MeshCollider>();

        if (mc != null)
        {
            mc.sharedMesh = metaball.Mesh;
        }

//        Instantiate(hitPS.gameObject, position, Quaternion.identity);
    }
}
