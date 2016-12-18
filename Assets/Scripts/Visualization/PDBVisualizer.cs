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

		// Single bond covalent atomic radii in angstroms
		protected float SizeForElement (Element element)
		{
			switch (element)
			{
			case Element.C :
				return 0.77f;

			case Element.N :
				return 0.75f;

			case Element.O :
				return 0.73f;

			case Element.S :
				return 0.102f;

			case Element.P :
				return 0.106f;

			case Element.H :
				return 0.38f;

			default :
				return 1f;
			}
		}
	}
}