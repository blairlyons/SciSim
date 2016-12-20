//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FarCamera : MonoBehaviour {

    public GameObject target;

    Vector3 _relativePosition;

	// Use this for initialization
	void Start () {
        _relativePosition = transform.position - target.transform.position;
	}
	
    void FixedUpdate()
    {
        transform.position = target.transform.position + _relativePosition;
    }
}
