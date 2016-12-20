//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Connect bones with joints so that mesh is driven by physics engine
/// </summary>
public class TentacleControl : MonoBehaviour {

    public SkinnedMetaballSeed seed;

	// Use this for initialization
	void Start () {
        SetupPhysicsBones();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetupPhysicsBones()
    {
        MetaballCellObject rootObj = seed.boneRoot.GetComponentInChildren<MetaballCellObject>();
        if( rootObj != null )
        {
            SetupPhysicsBonesRecursive(rootObj, true);
        }
    }

    void SetupPhysicsBonesRecursive( MetaballCellObject obj, bool bRoot = false )
    {
        Rigidbody r = obj.GetComponent<Rigidbody>();
        if( r == null )
        {
            r = obj.gameObject.AddComponent<Rigidbody>();
        }
        r.useGravity = false;

        if( bRoot )
        {
            FixedJoint j = obj.GetComponent<FixedJoint>();
            if( j == null )
            {
                j = obj.gameObject.AddComponent<FixedJoint>();
            }
            j.connectedBody = seed.GetComponent<Rigidbody>();
        }
        else
        {            
            {
                HingeJoint j = obj.GetComponent<HingeJoint>();
                if (j == null)
                {
                    j = obj.gameObject.AddComponent<HingeJoint>();
                }
                j.connectedBody = obj.transform.parent.GetComponent<Rigidbody>();
                j.useLimits = true;
                j.limits = new JointLimits() { max = 30.0f, min = -30.0f };               
            }
        }

        for( int i=0; i<obj.transform.childCount; ++i )
        {
            Transform c = obj.transform.GetChild(i);

            MetaballCellObject childObj = c.GetComponent<MetaballCellObject>();
            if( childObj != null )
            {
                SetupPhysicsBonesRecursive(childObj);
            }
        }
    }
}
