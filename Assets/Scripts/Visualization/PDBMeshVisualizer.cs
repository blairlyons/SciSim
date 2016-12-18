using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class PDBMeshVisualizer : MonoBehaviour 
	{
		public bool renderOnStart = true;
		public Material material;

		Metaballs metaballs;

		void Start ()
		{
			if (renderOnStart)
			{
				Render();
			}
		}

		void Update ()
		{
			metaballs.UpdateMetaballs();
		}

		public void Render ()
		{
			metaballs = new Metaballs(gameObject, material);
		}
	}
}
