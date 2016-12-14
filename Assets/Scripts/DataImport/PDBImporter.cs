using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public class PDBImporter : Importer
	{
		public PDBImporter (string data)
		{
			Debug.Log("Import! " + data);
		}
	}
}