using UnityEngine;
using System.Collections;

namespace SciSim
{
	public static class MoleculeUtility 
	{
		// Single bond covalent atomic radii in angstroms
		public static float SizeForElement (Element element)
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
