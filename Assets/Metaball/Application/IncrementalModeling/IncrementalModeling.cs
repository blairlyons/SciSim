//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IncrementalModeling : ImplicitSurface{

    public bool bSaveBrushHistory = true;

    [SerializeField]
    List<Brush> _brushHistory = new List<Brush>();

    [System.Serializable]
    public class Brush
    {
        public enum Shape
        {
            sphere,
            box
        }
        public float fadeRadius = 0.1f;
        public float powerScale = 1.0f;
        public Matrix4x4 invTransform;

        public float sphereRadius = 0.5f;
        public Vector3 boxExtents = Vector3.one * 0.5f;

        public Shape shape;

		public Brush()
		{
		}
        
        public Brush( Shape shape_, Matrix4x4 invTransformMtx_, float fadeRadius_, float powerScale_, float sphereRadius_, Vector3 boxExtents_ )
        {
            shape = shape_;
            fadeRadius = fadeRadius_;
            powerScale = powerScale_;
            invTransform = invTransformMtx_;
            sphereRadius = sphereRadius_;
            boxExtents = boxExtents_;
        }
        
        public void Draw(IncrementalModeling model)
        {
            switch( shape )
            {
                case Shape.sphere:
                    DrawSphere(model);
                    break;
                case Shape.box:
                    DrawBox(model);
                    break;
            }
        }
        
        void DrawSphere(IncrementalModeling model)
        {
            int count = model._countX * model._countY * model._countZ;
            for (int i = 0; i < count; ++i)
            {
                float distance = invTransform.MultiplyPoint(model._positionMap[i]).magnitude;
                if (distance < sphereRadius)
                {
                    float power = 1.0f;

                    if (fadeRadius > 0.0f)
                    {
                        power = Mathf.Clamp01((sphereRadius - distance) / fadeRadius);
                    }
                    model._powerMap[i] = Mathf.Clamp01(model._powerMap[i] + powerScale * power);
                    model._powerMap[i] *= model._powerMapMask[i];
                }
            }
        }

        void DrawBox(IncrementalModeling model)
        {
            int count = model._countX * model._countY * model._countZ;
            for (int i = 0; i < count; ++i)
            {
                float power = 1.0f;
                Vector3 position = invTransform.MultiplyPoint(model._positionMap[i]);

                for (int j = 0; j < 3; ++j)
                {
                    float distance = Mathf.Abs(position[j]);

                    float r = boxExtents[j];
                    if (distance < r)
                    {
                        if (fadeRadius > 0.0f)
                        {
                            power *= Mathf.Clamp01((r - distance) / fadeRadius);
                        }
                    }
                    else
                    {
                        power = 0.0f;
                        break;
                    }
                }

                if (power > 0.0f)
                {
                    model._powerMap[i] = Mathf.Clamp01(model._powerMap[i] + powerScale * power);
                    model._powerMap[i] *= model._powerMapMask[i];
                }
            }
        }
    }
    
	protected override void InitializePowerMap ()
	{
		foreach( Brush b in _brushHistory )
		{
			b.Draw(this);
		}
	}

    [ContextMenu("Rebuild")]
    public void Rebuild()
    {
        ResetMaps();

        foreach( Brush b in _brushHistory )
        {
            b.Draw(this);
        }

        CreateMesh();
    }

    [ContextMenu("ClearHistory")]
    public void ClearHistory()
    {
        _brushHistory.Clear();

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty (this);
#endif
    }
    
    public void AddSphere( Transform brushTransform, float radius, float powerScale, float fadeRadius )
    {
        Matrix4x4 invTransformMtx = brushTransform.worldToLocalMatrix * transform.localToWorldMatrix;

        Brush newBrush = new Brush( Brush.Shape.sphere, invTransformMtx, fadeRadius, powerScale, radius, Vector3.one);

        newBrush.Draw(this);

        if (bSaveBrushHistory)
        {
            _brushHistory.Add(newBrush);

			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty (this);
			#endif
        }
        
        CreateMesh();
    }

    public void AddBox( Transform brushTransform, Vector3 extents, float powerScale, float fadeRadius)
    {
        Matrix4x4 invTransformMtx = brushTransform.worldToLocalMatrix * transform.localToWorldMatrix;

        Brush newBrush = new Brush( Brush.Shape.box, invTransformMtx, fadeRadius, powerScale, 1.0f, extents);

        newBrush.Draw(this);

        if (bSaveBrushHistory)
        {
			_brushHistory.Add(newBrush);
			
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty (this);
			#endif
		}        

        CreateMesh();
    }    
}
