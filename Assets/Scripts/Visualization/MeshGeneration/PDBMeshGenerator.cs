using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBMeshGenerator : MeshGenerator
	{
		PDBAsset molecule;

		public PDBMeshGenerator (PDBAsset _molecule, float _percentOfPointsToUse, float _pointCloudScale, float _blobSize, float _quality)
			: base ("", _percentOfPointsToUse, _pointCloudScale, _blobSize, _quality)
		{ 
			molecule = _molecule;
		}

		protected override void MakeNodes ()
		{
			foreach (PDBAtom atom in molecule.atoms)
			{
				if (atom.index % Mathf.Ceil(100f / percentOfPointsToUse) == 0)
				{
					AddAtom(atom);
				}
			}
		}

		void AddAtom (PDBAtom atomData)
		{
			GameObject atom = new GameObject(atomData.elementType.ToString() + atomData.atomNumber, typeof(MetaballNode));
			atom.transform.SetParent(root);
			atom.transform.localPosition = pointCloudScale * atomData.localPosition;
			atom.GetComponent<MetaballNode>().baseRadius = blobSize * MoleculeUtility.SizeForElement(atomData.elementType);
		}
	}
}