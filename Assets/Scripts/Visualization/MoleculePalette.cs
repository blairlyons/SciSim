using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	[CreateAssetMenu(fileName = "MoleculePalette", menuName = "ScienceTools/Colors/Molecule Palette", order = 1)]
	[System.Serializable]
	public class MoleculePalette : ScriptableObject
	{
		public List<AtomColor> atomColors = new List<AtomColor>();

		public Color ColorForElement (Element element)
		{
			return atomColors.Find( a => a.element == element ).color;
		}
	}

	[System.Serializable]
	public class AtomColor
	{
		public Element element;
		public Color color;
	}
}