using UnityEngine;
using System.Collections;

namespace SciSim
{
	public abstract class Distribution : ScriptableObject
	{
		public int amount; //mol

		//mol/L
		public abstract float GetConcentrationAtLocalPosition (Vector3 localPosition, float radius, Units units);

		//mol/L
		public float GetAverageConcentration (float radius, Units units)
		{
			float radiusCM = radius * ScaleUtility.Conversion(units, Units.Centimeters);
			return amount / (4f/3f * Mathf.PI * Mathf.Pow(radiusCM, 3f) * 1E-3f);
		}
	}
}
