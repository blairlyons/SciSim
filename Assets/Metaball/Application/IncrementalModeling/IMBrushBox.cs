//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class IMBrushBox : IMBrush {
    public Vector3 extents = Vector3.one;

    protected override void DoDraw()
    {
        im.AddBox(transform, extents, PowerScale, fadeRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, extents*2.0f);

        if( im != null )
        {
            Gizmos.color = bSubtract ? Color.red : Color.white;
            float th = im.powerThreshold;

            Vector3 drawExtents = new Vector3( 
                Mathf.Lerp(extents.x, extents.x-fadeRadius, th),
                Mathf.Lerp(extents.y, extents.y-fadeRadius, th),
                Mathf.Lerp(extents.z, extents.z-fadeRadius, th)
                );
            Gizmos.DrawWireCube(Vector3.zero, drawExtents * 2.0f);
        }
    }
}
