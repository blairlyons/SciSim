//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ImplicitSurfaceMeshCreaterBase))]
public class AutoUpdate : MonoBehaviour {

	ImplicitSurfaceMeshCreaterBase _surface;

	void Awake()
	{
		_surface = GetComponent<ImplicitSurfaceMeshCreaterBase> ();
	}
	
	// Update is called once per frame
	void Update () {
		_surface.CreateMesh ();
	}
}
