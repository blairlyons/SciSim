using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class PDBMeshGenerator 
	{
		PDBAsset molecule;
		float atomResolution = 1f;
		float moleculeScale = 1f;
		float atomSize = 5f;
		float quality = 0.4f;

		ImplicitSurfaceMeshCreaterBase generator;
		Transform root;

		public PDBMeshGenerator (float _atomResolution, float _moleculeScale, float _atomSize, float _quality)
		{
			atomResolution = _atomResolution;
			moleculeScale = _moleculeScale;
			atomSize = _atomSize;
			quality = _quality;
		}

		public Mesh GenerateMesh (PDBAsset _molecule)
		{
			molecule = _molecule;

			CreateGenerator();
			MakeNodes();

			generator.CreateMesh();
			return generator.transform.FindChild("StaticMesh").GetComponent<MeshFilter>().sharedMesh;
		}

		public void CleanUp ()
		{
			if (generator != null)
			{
				GameObject.DestroyImmediate(generator.gameObject);
			}
		}

		void CreateGenerator ()
		{
			GameObject prefab = Resources.Load("MeshGenerator") as GameObject;
			if (prefab != null)
			{
				GameObject g = GameObject.Instantiate(prefab);
				g.name = molecule.pdbID + " Mesh Generator";
				generator = g.GetComponent<ImplicitSurfaceMeshCreaterBase>();
				generator.gridSize = 1f +  0.5f / quality;
				generator.powerThreshold = 0.4f * quality;
				root = g.transform.FindChild("RootNode");
			}
			else
			{
				Debug.LogWarning("Couldn't load MeshCreator prefab");
			}
		}

		void MakeNodes ()
		{
			foreach (PDBAtom atom in molecule.atoms)
			{
				if (atom.index % Mathf.Ceil(1 / atomResolution) == 0)
				{
					AddNode(atom);
				}
			}
		}

		void AddNode (PDBAtom atomData)
		{
			GameObject node = new GameObject(atomData.elementType.ToString() + atomData.atomNumber, typeof(MetaballNode));
			node.transform.SetParent(root);
			node.transform.localPosition = moleculeScale * atomData.localPosition;
			node.GetComponent<MetaballNode>().baseRadius = atomSize * MoleculeUtility.SizeForElement(atomData.elementType);
		}
	}
}