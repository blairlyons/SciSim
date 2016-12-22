using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public class CreateNuclearOrganelleWindow : CreateMeshWindow 
	{
		NuclearOrganelleProperties _properties;
		NuclearOrganelleProperties properties
		{
			get 
			{
				if (_properties == null)
				{
					_properties = new NuclearOrganelleProperties();
				}
				return _properties;
			}
		}

		[MenuItem ("ScienceTools/Create/Organelles", false, 0)]
		public static void OpenPDBWindow ()
		{
			EditorWindow.GetWindow(typeof(CreateNuclearOrganelleWindow));
		}

		void OnGUI ()
		{
			GUILayout.Label("Nucleus", EditorStyles.largeLabel);

			properties.nuclearPosition = EditorGUILayout.Vector3Field("Position", properties.nuclearPosition);
			properties.nuclearRadius = EditorGUILayout.FloatField("Radius (nm)", properties.nuclearRadius);

			EditorGUILayout.Separator();

			GUILayout.Label("Rough (Lamellar) Endoplasmic Reticulum", EditorStyles.largeLabel);

			properties.roughERcontinuous = EditorGUILayout.Toggle("Continuous with nucleus?", properties.roughERcontinuous);
			properties.roughERfolds = EditorGUILayout.IntField("Folds", properties.roughERfolds);;
			properties.roughERspread = EditorGUILayout.FloatField("Spread", properties.roughERspread);

			EditorGUILayout.Separator();

			GUILayout.Label("Smooth (Tubular) Endoplasmic Reticulum", EditorStyles.largeLabel);

			properties.smoothERcontinuous = EditorGUILayout.Toggle("Continuous with rough ER?", properties.smoothERcontinuous);
			properties.smoothERlayers = EditorGUILayout.IntField("Layers", properties.smoothERlayers);
			properties.smoothERspread = EditorGUILayout.FloatField("Spread", properties.smoothERspread);

			EditorGUILayout.Separator();

			quality = EditorGUILayout.FloatField("Quality", quality);

			if (GUILayout.Button("Create Organelle Mesh"))
			{
				ClearGUI();
				GenerateMesh();
			}

			if (mesh != null)
			{
				GUILayout.Label("Generated mesh successfully: " + mesh.vertexCount + " vertices", EditorStyles.miniLabel);

				EditorGUILayout.Separator();

				if (GUILayout.Button("Clear Mesh"))
				{
					ResetGenerator();
				}

				if (GUILayout.Button("Save Mesh"))
				{
					SaveMesh("Assets/Organelles/Mesh", "NuclearOrganelleMesh");
				}

				if (savedMesh)
				{
					GUILayout.Label("Saved mesh successfully", EditorStyles.miniLabel);
				}
			}
		}

		void ClearGUI ()
		{
			mesh = null;
			savedMesh = false;
		}

		#region --------------------------------------------------------------------- Generate Mesh

		protected override void CreateMeshGenerator () 
		{
			generator = new NuclearOrganelleMeshGenerator(properties, quality);
		}

		#endregion
	}
}