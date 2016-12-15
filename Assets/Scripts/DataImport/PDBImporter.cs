﻿using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class PDBImporter : Importer
	{
		public PDBAsset molecule;

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
			string[] lines = molecule.rawData.Split ('\n');
			foreach (string line in lines) 
			{
				PDBAtom newAtom = ParseLine(i, line);
				if (newAtom != null)
				{
					molecule.atoms.Add(newAtom);
					i++;
				}
			}
		}

		PDBAtom ParseLine (int index, string lineData)
		{
			if (lineData.Length >= 54) 
			{
				if (lineData.Substring(0, 6).Trim().Equals("ATOM")) 
				{
					//read
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
			}
			return null;
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
	}
}