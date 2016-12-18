using UnityEngine;
using System.Collections;

namespace SciSim
{
	public abstract class PDBVisualizer : MonoBehaviour 
	{
		public bool renderOnStart = true;
		public int currentStructure = 0;
		public PDBAsset[] structures = new PDBAsset[1];
		public float scale = 1f;
		public float resolution = 0.5f;
		public float atomSize = 10f;
		public MoleculePalette palette;

		void Start ()
		{
			if (renderOnStart)
			{
				Render();
			}
		}

		public abstract void Render ();

		protected bool morphing;
		bool warned;

		public void Morph (int goalStructure, float duration)
		{
			if (goalStructure < structures.Length && structures[goalStructure] != null)
			{
				if (!warned && structures[currentStructure].atoms.Count != structures[goalStructure].atoms.Count)
				{
					Debug.LogWarning(name + " trying to morph to a structure with a different number of atoms!");
					warned = true;
				}

				StartMorph(goalStructure, duration);

				morphing = true;
				currentStructure = goalStructure;

				Invoke("EndMorph", duration);
			}
		}

		public abstract void StartMorph (int goalStructure, float duration);

		void EndMorph ()
		{
			StopMorph();
			morphing = false;
		}

		protected abstract void StopMorph ();

		void Update ()
		{
			TestMorph();
		}

		protected abstract void TestMorph ();
	}
}