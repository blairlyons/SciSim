//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class HitPSControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        Destroy(this.gameObject, particleSystem.duration);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
