//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public abstract class IMBrush : MonoBehaviour {

    public IncrementalModeling im;

    public float fadeRadius = 0.2f;
    public bool bSubtract = false;

    public float PowerScale
    {
        get { return bSubtract ? -1.0f : 1.0f; }
    }

    [ContextMenu("Draw")]
    public void Draw()
    {
        if( im == null )
        {
            im = Utils.FindComponentInParents<IncrementalModeling>(transform);
        }

        if (im != null)
        {
            DoDraw();
        }
        else
        {
            Debug.LogError("no IncrementalModeling component for this brush found in hierarchy.");
        }
    }

    protected abstract void DoDraw();
}
