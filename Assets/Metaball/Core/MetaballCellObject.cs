//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class MetaballCellObject : MonoBehaviour {

    protected MetaballCell _cell;

    public MetaballCell Cell
    {
        get { return _cell; }
    }
    public virtual void Setup(MetaballCell cell)
    {
        _cell = cell;
    }
}
