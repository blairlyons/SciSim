using UnityEngine;
using System.Collections;

namespace SciSim
{
	public static class ScaleUtility 
	{
		public static float MultiplierFromMeters (Units fromUnits)
		{
			switch (fromUnits)
			{
			case Units.Femtometers :
				return 1E15f;
			case Units.Picometers :
				return 1E12f;
			case Units.Angstroms :
				return 1E10f;
			case Units.Nanometers :
				return 1E9f;
			case Units.Micrometers :
				return 1E6f;
			case Units.Millimeters :
				return 1E3f;
			case Units.Centimeters :
				return 1E2f;
			case Units.Meters :
				return 1f;
			case Units.Kilometers :
				return 1E-3f;
			case Units.Megameters :
				return 1E-6f;
			case Units.Gigameters :
				return 1E-9f;
			case Units.Terameters :
				return 1E-12f;
			case Units.Lightyears :
				return (1 / 9.46E15f);
			default :
				return 1f;
			}
		}

		public static float Conversion (Units fromUnits, Units toUnits)
		{
			return ScaleUtility.MultiplierFromMeters(toUnits) / ScaleUtility.MultiplierFromMeters(fromUnits);
		}
	}
}