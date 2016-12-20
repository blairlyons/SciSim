//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class IMBrushSphere : IMBrush
{
    public float radius = 1.0f;

    protected override void DoDraw()
    {
        im.AddSphere(transform, radius, PowerScale, fadeRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, radius);


        if( im != null )
        {
            Gizmos.color = bSubtract ? Color.red : Color.white;
            float th = im.powerThreshold;

            float drawRadius = Mathf.Lerp(radius, radius - fadeRadius, th);
            Gizmos.DrawWireSphere(Vector3.zero, drawRadius);
        }
    }
}
