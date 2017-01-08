using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public abstract class CreateMeshWindow : EditorWindow
	{
		protected MeshGenerator generator;
		protected Mesh mesh;
		protected bool savedMesh;
		protected float quality = 0.4f;

		protected void ResetGenerator ()
		{
			if (generator != null)
			{
				generator.CleanUp();
			}
			generator = null;
		}

		protected void GenerateMesh ()
		{
			ResetGenerator();
			CreateMeshGenerator();
			mesh = generator.GenerateMesh();
		}

		protected virtual void CreateMeshGenerator () { }

		protected void SaveMesh (string directory, string defaultName)
		{
			ResetGenerator();

			string path = EditorUtility.SaveFilePanel("Save Mesh", directory, defaultName, "asset");
			path = path.Substring(path.IndexOf("Assets"));

			AssetDatabase.CreateAsset(mesh, path);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = mesh;
			savedMesh = true;
		}
	}
}
