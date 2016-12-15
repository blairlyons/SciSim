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

		[MenuItem ("ScienceTools/Import/PDB", false, 16)]
		public static void OpenPDBWindow ()
		{
			EditorWindow.GetWindow(typeof(PDBImportWindow));
		}

		void OnGUI ()
		{
			download = EditorGUILayout.ToggleLeft("Download from RCSB?", download);
			if (download) 
			{
				GUILayout.Label("PDB ID", EditorStyles.miniLabel);
				pdbID = GUILayout.TextField(pdbID);
				if (GUILayout.Button("Download"))
				{
					downloader.StartDownload("http://files.rcsb.org/download/" + pdbID + ".pdb", DownloadFinished);
				}
			}
			else 
			{
				if (GUILayout.Button("Choose PDB file"))
				{
					filePath = EditorUtility.OpenFilePanel("Select a PDB file", "Assets/Data/PDB", "pdb");
					filePath = filePath.Substring(filePath.IndexOf("Assets"));
				}
				if (!string.IsNullOrEmpty(filePath))		
				{
					EditorGUILayout.LabelField("File path", filePath);
					if (GUILayout.Button("Open"))
					{
						TextDataOpener opener = new TextDataOpener(filePath);
						Debug.Log("OPENED PDB: " + opener.textData);
						pdbData = opener.textData;
					}
				}
			}

			EditorGUILayout.Separator();

			if (!string.IsNullOrEmpty(pdbData))
			{
				if (download) 
				{
					GUILayout.Label("Downloaded " + pdbID + " Successfully", EditorStyles.miniLabel);
				}
				if (GUILayout.Button("Import"))
				{
					ImportTextData();
				}
			}
		}

		void DownloadFinished (string downloadedText)
		{
			Debug.Log("DOWNLOADED PDB: " + downloadedText);
			pdbData = downloadedText;
		}

		void ImportTextData ()
		{
			PDBImporter importer = new PDBImporter(pdbData);
			SavePDBAsset(importer.molecule);
		}

		void SavePDBAsset (PDBAsset molecule)
		{
			string path = EditorUtility.SaveFilePanel("Save Molecule", "Assets/Molecules", "newMolecule", "asset");
			path = path.Substring(path.IndexOf("Assets"));

			AssetDatabase.CreateAsset(molecule, path);
			AssetDatabase.SaveAssets();

			EditorUtility.FocusProjectWindow();
			Selection.activeObject = molecule;
		}
	}
}