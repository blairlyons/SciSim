using UnityEngine;
using System.Collections;

namespace SciSim
{
	public abstract class Distribution : ScriptableObject
	{
		public float maxConcentration = 2E-2f; // mol/L
		 
		//mol/L
		public abstract float GetConcentrationAtLocalPosition (Vector3 localPosition, float radius, Units units);

		public virtual Vector3 GetPosition (Vector3 bubblePosition, float bubbleRadius, Vector3 agentPosition, float agentRadius, int index, int n)
		{
			return Vector3.zero;
		}

		public virtual Quaternion GetRotation (int index, int n)
		{
			return Random.rotation;
		}
	}
}
