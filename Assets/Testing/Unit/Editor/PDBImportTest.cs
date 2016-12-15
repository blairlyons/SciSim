using UnityEngine;
using NUnit.Framework;
using SciSim;
using System.IO;

public class PDBImportTest 
{
	int maxStructuresToTest = 5;

	PDBAsset _testMolecule;
	PDBAsset testMolecule
	{
		get 
		{
			if (_testMolecule == null) 
			{
				
			}
			return _testMolecule;
		}
	}

	[Test]
	public void ResiduesAreInCorrectOrder ()
	{
		
		Assert.Pass();
	}

	[Test]
	public void CorrectTotalNumberOfAtoms ()
	{

		Assert.Pass();
	}

	[Test]
	public void RandomSampleOfAtomsIsCorrect ()
	{

		Assert.Pass();
	}


}
