using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBImporter
	{
		public PDBAsset molecule;
		public List<Residue> residueSequence;
		public string[] lines;

		public PDBImporter (string data)
		{
			molecule = ScriptableObject.CreateInstance<PDBAsset>();
			molecule.rawData = data;

			ParseData();
			CenterAtoms();
		}

		void ParseData ()
		{
			int i = 0;
			residueSequence = new List<Residue>();
			lines = molecule.rawData.Split('\n');

			molecule.pdbID = lines[0].Substring(62, 4);

			foreach (string line in lines) 
			{
				PDBAtom newAtom = ParseLine(i, line);
				if (newAtom != null)
				{
					molecule.atoms.Add(newAtom);
					i++;
				}
			}
//			PrintResidueSequence();
		}

		PDBAtom ParseLine (int index, string lineData)
		{
			if (lineData.Length >= 54) 
			{
				if (lineData.Substring(0, 6).Trim().Equals("SEQRES")) 
				{
					ParseResidueSequence(lineData);
				}
				else if (lineData.Substring(0, 6).Trim().Equals("ATOM")) 
				{
					return ParseAtom(index, lineData);
				}
			}
			return null;
		}

		void ParseResidueSequence (string lineData)
		{
			string resSeq = lineData.Substring(19, 51);
			string[] residues = resSeq.Split(' ');
			foreach (string residue in residues)
			{
				if (residue.Length > 2)
				{
					Residue res = ParseResidueType(residue, -1);
					if (res != Residue.none) 
					{
						residueSequence.Add(res);
					}
				}
			}
		}

		public PDBAtom ParseAtom (int index, string lineData)
		{
			//record
			string chainID = lineData.Substring(21, 1);
			string residueNumberString = lineData.Substring(22, 4).Trim();
			string residueTypeString = lineData.Substring(17, 3);
			string atomNumberString = lineData.Substring(6, 5).Trim ();
			string elementTypeString = lineData.Substring(13, 1);
			string xPositionString = lineData.Substring(30, 8).Trim();
			string yPositionString = lineData.Substring(38, 8).Trim();
			string zPositionString = lineData.Substring(46, 8).Trim();

			//parse
			int atomNumber, residueNumber;
			int.TryParse(atomNumberString, out atomNumber);
			int.TryParse(residueNumberString, out residueNumber);
			Vector3 position = ParsePosition(xPositionString, yPositionString, zPositionString, index);
			Element elementType = ParseElementType(elementTypeString, index);
			Residue residueType = ParseResidueType(residueTypeString, index);

			return new PDBAtom(index, chainID, residueNumber, residueType, atomNumber, elementType, position);
		}

		Vector3 ParsePosition (string xPositionString, string yPositionString, string zPositionString, int index)
		{
			float xPosition = 0;
			float yPosition = 0;
			float zPosition = 0;
			try 
			{
				xPosition = float.Parse(xPositionString);
				yPosition = float.Parse(yPositionString);
				zPosition = float.Parse(zPositionString);
			} 
			catch (System.Exception e) 
			{
				Debug.LogWarning ("Exception while parsing position @ line " + index + ": " + e.Message);
			}

			return new Vector3(xPosition, yPosition, zPosition);
		}

		Element ParseElementType (string type, int index)
		{
			try 
			{
				return (Element)System.Enum.Parse(typeof(Element), type);
			} 
			catch (System.Exception e) 
			{
				Debug.LogWarning ("Exception while parsing element type " + type + " @ line " + index + ": " + e.Message);
				return Element.none;
			}
		}

		Residue ParseResidueType (string type, int index)
		{
			try 
			{
				return (Residue)System.Enum.Parse(typeof(Residue), type);
			} 
			catch (System.Exception e) 
			{
				Debug.LogWarning ("Exception while parsing residue type " + type + " @ line " + index + ": " + e.Message);
				return Residue.none;
			}
		}

		void CenterAtoms ()
		{
			GetCenter();
			foreach (PDBAtom atom in molecule.atoms) {
				atom.localPosition -= molecule.centerOffset;
			}
		}

		void GetCenter ()
		{
			Vector3 centerOffset = Vector3.zero;
			foreach (PDBAtom atom in molecule.atoms) {
				centerOffset += atom.localPosition;
			}
			centerOffset /= molecule.atoms.Count;
			molecule.centerOffset = centerOffset;
		}

		void PrintResidueSequence ()
		{
			string result = "";
			foreach (Residue residue in residueSequence)
			{
				result += residue.ToString() + " ";
			}
			Debug.Log(result);
		}
	}
}