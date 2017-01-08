using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBAsset : ScriptableObject
	{
		public string pdbID;
		public List<PDBAtom> atoms;
		public string rawData;
		public Vector3 centerOffset;

		void OnEnable()
		{
			if (atoms == null)
			{
				atoms = new List<PDBAtom>();
			}
		}
	}

	[System.Serializable]
	public class PDBAtom 
	{
		public int index;
		public string chainID;
		public int residueNumber;
		public Residue residueType;
		public int atomNumber;
		public Element elementType;
		public Vector3 localPosition; //angstroms

		public PDBAtom (int _index, string _chainID, int _residueNumber, Residue _residueType, int _atomNumber, Element _elementType, Vector3 _localPosition)
		{
			index = _index;
			chainID = _chainID;
			residueNumber = _residueNumber;
			residueType = _residueType;
			atomNumber = _atomNumber;
			elementType = _elementType;
			localPosition = _localPosition;
		}

		public override string ToString ()
		{
			return "atom#" + index + "-" + chainID + ":" + residueType + residueNumber + ":" + elementType + atomNumber;
		}

		public bool EqualsAtom (PDBAtom other)
		{
			return chainID == other.chainID && residueNumber == other.residueNumber && residueType == other.residueType
				&& atomNumber == other.atomNumber && elementType == other.elementType && localPosition == other.localPosition;
		}
	}
}
