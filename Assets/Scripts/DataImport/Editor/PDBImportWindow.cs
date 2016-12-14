using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SciSim
{
	public class PDBImportWindow : DownloadWindow
	{
		bool download = true;
		string pdbID = "";
		public string filePath = "";

		string pdbData;

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
					StartDownload("http://files.rcsb.org/download/" + pdbID + ".pdb");
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
				if (GUILayout.Button("Import"))
				{
					ImportTextData();
				}
			}
		}

		protected override void DownloadFinished ()
		{
			Debug.Log("DOWNLOADED PDB: " + downloader.text);
			pdbData = downloader.text;
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