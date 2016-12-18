using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public class PDBImportWindow : EditorWindow
	{
		bool download = true;
		string pdbID = "";
		public string filePath = "";

		string pdbData;
		PDBAsset molecule;
		Mesh mesh;
		bool savedMesh;

		float atomResolution = 1f;
		float moleculeScale = 1f;
		float atomSize = 5f;
		float quality = 0.4f;

		[MenuItem ("ScienceTools/Import/PDB", false, 0)]
		public static void OpenPDBWindow ()
		{
			EditorWindow.GetWindow(typeof(PDBImportWindow));
		}

		void OnGUI ()
		{
			download = EditorGUILayout.ToggleLeft("Download from RCSB?", download);

			if (download) 
			{
				pdbID = EditorGUILayout.TextField("PDB ID", pdbID);

				if (GUILayout.Button("Download"))
				{
					StartDownload();
				}
			}
			else 
			{
				if (GUILayout.Button("Choose PDB File"))
				{
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

						if (GUILayout.Button("Save Mesh"))
						{
							SaveMesh();
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
			ClearGUI();
			downloader.StartDownload("http://files.rcsb.org/download/" + pdbID + ".pdb", DownloadFinished);
		}

		void DownloadFinished (string downloadedText)
		{
			Debug.Log("DOWNLOADED PDB: " + downloadedText);
			pdbData = downloadedText;
		}

		#endregion

		#region --------------------------------------------------------------------- Local File

		void ChoosePDBFile ()
		{
			ClearGUI();
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

		PDBMeshGenerator generator;

		void GenerateMesh ()
		{
			if (generator != null)
			{
				generator.CleanUp();
				generator = null;
			}
			generator = new PDBMeshGenerator(atomResolution, moleculeScale, atomSize, quality);
			mesh = generator.GenerateMesh(molecule);
		}

		void SaveMesh ()
		{
			generator.CleanUp();
			generator = null;

			string path = EditorUtility.SaveFilePanel("Save Mesh", "Assets/Molecules/Mesh", pdbID + "Mesh" , "asset");
			path = path.Substring(path.IndexOf("Assets"));

			AssetDatabase.CreateAsset(mesh, path);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = mesh;
			savedMesh = true;
		}
	}
}