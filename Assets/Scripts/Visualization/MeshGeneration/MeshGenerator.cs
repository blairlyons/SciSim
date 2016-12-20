using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class MeshGenerator 
	{
		string name;
		protected float percentOfPointsToUse = 100f;
		protected float pointCloudScale = 1f;
		protected float blobSize = 5f;
		protected float quality = 0.4f;

		ImplicitSurfaceMeshCreaterBase generator;
		protected Transform root;

		public MeshGenerator (string _name, float _percentOfPointsToUse, float _pointCloudScale, float _blobSize, float _quality)
		{
			name = _name;
			percentOfPointsToUse = _percentOfPointsToUse;
			pointCloudScale = _pointCloudScale;
			blobSize = _blobSize;
			quality = _quality;
		}

		public Mesh GenerateMesh ()
		{
			CreateGenerator();
			MakeNodes();

			generator.CreateMesh();
			return generator.transform.FindChild("StaticMesh").GetComponent<MeshFilter>().sharedMesh;
		}

		protected void CreateGenerator ()
		{
			GameObject prefab = Resources.Load("MeshGenerator") as GameObject;
			if (prefab != null)
			{
				GameObject g = GameObject.Instantiate(prefab);
				g.name = name + " Mesh Generator";
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

		protected virtual void MakeNodes() { }

		public void CleanUp ()
		{
			if (generator != null)
			{
				GameObject.DestroyImmediate(generator.gameObject);
			}
		}
	}
}