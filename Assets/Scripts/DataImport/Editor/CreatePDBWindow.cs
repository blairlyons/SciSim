using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public class CreatePDBWindow : CreateMeshWindow
	{
		bool download = true;
		string pdbID = "";
		public string filePath = "";

		string pdbData;
		PDBAsset molecule;

		float atomResolution = 100f;
		float moleculeScale = 1f;
		float atomSize = 5f;

		[MenuItem ("ScienceTools/Create/PDB", false, 0)]
		public static void OpenPDBWindow ()
		{
			EditorWindow.GetWindow(typeof(CreatePDBWindow));
		}

		void OnGUI ()
		{
			download = EditorGUILayout.ToggleLeft("Download from RCSB?", download);

			if (download) 
			{
				pdbID = EditorGUILayout.TextField("PDB ID", pdbID);

				if (GUILayout.Button("Download"))
				{
					ClearGUI();
					StartDownload();
				}
			}
			else 
			{
				if (GUILayout.Button("Choose PDB File"))
				{
					ClearGUI();
					ChoosePDBFile();
				}

				if (!string.IsNullOrEmpty(filePath))		
				{
					EditorGUILayout.LabelField("File path", filePath);

					if (GUILayout.Button("Open"))
					{
						OpenPDBFile();
					}
				}
			}

			if (!string.IsNullOrEmpty(pdbData))
			{
				if (download) 
				{
					GUILayout.Label("Downloaded " + pdbID + " successfully", EditorStyles.miniLabel);
				}
				else 
				{
					GUILayout.Label("Opened Successfully", EditorStyles.miniLabel);
				}

				EditorGUILayout.Separator();

				if (GUILayout.Button("Import"))
				{
					ImportTextData();
				}
				if (molecule != null)
				{
					GUILayout.Label("Imported Successfully", EditorStyles.miniLabel);

					EditorGUILayout.Separator();

					atomResolution = EditorGUILayout.FloatField("Atom resolution", atomResolution);
					moleculeScale = EditorGUILayout.FloatField("Molecule scale", moleculeScale);
					atomSize = EditorGUILayout.FloatField("Atom size", atomSize);
					quality = EditorGUILayout.FloatField("Quality", quality);

					if (GUILayout.Button("Generate Mesh"))
					{
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
							SaveMesh("Assets/Molecules/Mesh", pdbID + "Mesh");
						}

						if (savedMesh)
						{
							GUILayout.Label("Saved mesh successfully", EditorStyles.miniLabel);
						}
					}
				}
			}
		}

		void ClearGUI ()
		{
			filePath = pdbData = "";
			molecule = null;
			mesh = null;
			savedMesh = false;
		}

		#region --------------------------------------------------------------------- Download

		EditorDownloadHelper _downloader;
		EditorDownloadHelper downloader
		{
			get 
			{
				if (_downloader == null)
				{
					_downloader = new EditorDownloadHelper();
				}
				return _downloader;
			}
		}

		void StartDownload ()
		{
			downloader.StartDownload("http://files.rcsb.org/download/" + pdbID + ".pdb", DownloadFinished);
		}

		void DownloadFinished (string downloadedText)
		{
			Debug.Log("DOWNLOADED PDB: " + downloadedText);
			pdbData = downloadedText;
		}

		#endregion

		#region --------------------------------------------------------------------- Load Local File

		void ChoosePDBFile ()
		{
			filePath = EditorUtility.OpenFilePanel("Select a PDB file", "Assets/Data/PDB", "pdb");
			filePath = filePath.Substring(filePath.IndexOf("Assets"));
		}

		void OpenPDBFile ()
		{
			TextDataOpener opener = new TextDataOpener(filePath);
			Debug.Log("OPENED PDB: " + opener.textData);
			pdbData = opener.textData;
		}

		#endregion

		#region --------------------------------------------------------------------- Import

		void ImportTextData ()
		{
			PDBImporter importer = new PDBImporter(pdbData);
			molecule = importer.molecule;
			SavePDBAsset();
		}

		void SavePDBAsset ()
		{
			string path = EditorUtility.SaveFilePanel("Save Molecule", "Assets/Molecules", "newMolecule", "asset");
			path = path.Substring(path.IndexOf("Assets"));

			AssetDatabase.CreateAsset(molecule, path);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = molecule;
		}

		#endregion

		#region --------------------------------------------------------------------- Generate Mesh

		protected override void CreateMeshGenerator () 
		{
			generator = new PDBMeshGenerator(molecule, atomResolution, moleculeScale, atomSize, quality);
		}

		#endregion
	}
}