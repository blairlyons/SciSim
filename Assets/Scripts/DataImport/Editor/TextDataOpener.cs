using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace SciSim
{
	public class TextDataOpener 
	{
		string filePath;
		bool madeTXTFile;

		public string textData;

		public TextDataOpener (string _filePath)
		{
			filePath = _filePath;

			MakeTXTFile();
			LoadTextData();
			DeleteTXTFile();
		}

		void MakeTXTFile ()
		{
			if (!Path.GetExtension(filePath).Contains("txt")) 
			{
				madeTXTFile = true;
				string newPath = Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filePath) + ".txt";
				File.Copy(filePath, newPath, true);
				AssetDatabase.ImportAsset(newPath);
				filePath = newPath;
			}
		}

		void LoadTextData () 
		{
			TextAsset data = (TextAsset)AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset));

			if (data) 
			{
				textData = data.text;
			}
			else 
			{
				Debug.LogWarning("Data at " + filePath + " could not be loaded as a text asset or is empty.");
			}
		}

		void DeleteTXTFile ()
		{
			if (madeTXTFile && Path.GetExtension(filePath).Contains("txt")) 
			{
				AssetDatabase.DeleteAsset(filePath);
			}
		}
	}
}