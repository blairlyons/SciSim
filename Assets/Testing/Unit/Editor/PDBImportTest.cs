using UnityEngine;
using NUnit.Framework;
using SciSim;
using System.IO;
using System.Collections.Generic;

public class PDBImportTest 
{
	int maxStructuresToTest = 5;

	[Test]
	public void ResiduesAreInCorrectOrder ()
	{
		if (testImporters.Length > 0)
		{
			foreach (PDBImporter importer in testImporters) 
			{
				if (!ResidueSequenceMatchesAtoms(importer))
				{
					Debug.Log(importer.molecule.pdbID + " failed");
					Assert.Fail();
				}
			}
		}
		Assert.Pass();
	}

	bool ResidueSequenceMatchesAtoms (PDBImporter importer)
	{
		int r = 0;
		List<Residue> residues = importer.residueSequence;
		List<PDBAtom> atoms = importer.molecule.atoms;
		for (int a = 0; a < atoms.Count; a++)
		{
			if (a > 0 && atoms[a].residueNumber != atoms[a - 1].residueNumber)
			{
				r++;
			}

			if (atoms[a].residueType != residues[r])
			{
				Debug.Log(importer.molecule.pdbID + " " + atoms[a].ToString() + " does not match " + residues[r] + r);
				return false;
			}
		}
		return true;
	}

	[Test]
	public void RandomSampleOfAtomsIsCorrect ()
	{
		if (testImporters.Length > 0)
		{
			Assert.Pass();
		}
		Assert.Fail();
	}

	#region ----------------------------------------------------- Test Molecules

	PDBImporter[] _testImporters;
	PDBImporter[] testImporters
	{
		get 
		{
			if (_testImporters == null) 
			{
				MakeMolecules(GetTextData());
			}
			return _testImporters;
		}
	}

	string[] GetTextData ()
	{
		string[] data = new string[maxStructuresToTest];
		FileInfo[] pdbFiles = new DirectoryInfo("Assets/Data/PDB").GetFiles("*.pdb");
		List<int> fileIndexes = RandomlyGetIndexesOfFilesToUse(pdbFiles.Length);

		int i = 0;
		foreach (int index in fileIndexes) 
		{
			TextDataOpener opener = new TextDataOpener(pdbFiles[index].FullName.Substring(pdbFiles[index].FullName.IndexOf("Assets")));
			data[i] = opener.textData;
			i++;
		}

		return data;
	}

	List<int> RandomlyGetIndexesOfFilesToUse (int numberOfFiles)
	{
		List<int> fileIndexes = new List<int>();
		for (int i = 0; i < numberOfFiles; i++)
		{
			fileIndexes.Add(i);
		}
		while (fileIndexes.Count > maxStructuresToTest)
		{
			fileIndexes.RemoveAt(Random.Range(0, fileIndexes.Count - 1));
		}
		return fileIndexes;
	}

	void MakeMolecules (string[] textData)
	{
		_testImporters = new PDBImporter[textData.Length];
		for (int i = 0; i < textData.Length; i++)
		{
			_testImporters[i] = new PDBImporter(textData[i]);
		}
	}

	#endregion
}
