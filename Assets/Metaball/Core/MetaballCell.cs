//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetaballCell
{
    public Vector3 baseColor;
  //  public float resource;
    public string tag;

    public float radius;

    public float density = 1.0f;

    // calculated
    // position in the basis of root
    public Vector3 modelPosition = Vector3.zero;
    // rotation in the basis of root
    public Quaternion modelRotation = Quaternion.identity;
}
