using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Visualization : MonoBehaviour 
	{
		public float radius = 10f;
		public Units units = Units.Nanometers;
		public float resolution = 100f;

		public float cameraDistance
		{
			get
			{
				return Vector3.Distance(transform.position, Camera.main.transform.position);
			}
		}
	}
}