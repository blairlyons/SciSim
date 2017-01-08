using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class NuclearOrganelleMeshGenerator : MeshGenerator 
	{
		NuclearOrganelleProperties properties;
		int n;

		public NuclearOrganelleMeshGenerator (NuclearOrganelleProperties _properties, float _quality)
			: base ("Nuclear Organelle", 100f, 1f, 10f, _quality)
		{ 
			properties = _properties;
		}

		protected override void MakeNodes ()
		{
			float radius = properties.nuclearRadius;
			for (int i = 0; i < properties.roughERfolds; i++)
			{
				CreateNodeShell(radius + i * 20f);
			}
		}

		void CreateNodeShell (float radius)
		{
			float quantity = 0.1f * Mathf.Pow(radius, 2f);
			float inc = Mathf.PI * (3f - Mathf.Sqrt(5f));
			float off = 2f / quantity;

			for (int i = 0; i < quantity; i++)
			{
				float y = i * off - 1 + (off / 2);
				float r = Mathf.Sqrt(1 - y * y);
				float phi = i * inc;

				if (y < properties.roughERspread - 0.5f)
				{
					AddNode(properties.nuclearPosition + radius * new Vector3(Mathf.Cos(phi) * r, y, Mathf.Sin(phi) * r));
				}
			}
		}

		void AddNode (Vector3 position)
		{
			GameObject node = new GameObject(n.ToString(), typeof(MetaballNode));
			node.transform.SetParent(root);
			node.transform.localPosition = position;
			node.GetComponent<MetaballNode>().baseRadius = blobSize;
			n++;
		}
	}

	public class NuclearOrganelleProperties
	{
		public Vector3 nuclearPosition = Vector3.zero;
		public float nuclearRadius = 100f;
		public bool roughERcontinuous;
		public int roughERfolds = 5;
		public float roughERspread = 0.3f;
		public int smoothERlayers = 3;
		public float smoothERspread = 0.2f;
		public bool smoothERcontinuous;
	}
}