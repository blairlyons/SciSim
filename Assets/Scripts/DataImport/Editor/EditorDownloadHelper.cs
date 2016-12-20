using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace SciSim
{
	public delegate void DownloadFinishedCallback (string downloadedText);

	public class EditorDownloadHelper
	{
		WWW www;
		DownloadFinishedCallback callback;

		public void StartDownload (string url, DownloadFinishedCallback _callback) 
		{
			www = new WWW(url);
			callback = _callback;
			EditorApplication.update += Download;
		}

		public void Download () 
		{
			if (www.isDone) 
			{
				if (!string.IsNullOrEmpty(www.error)) 
				{
					Debug.LogWarning("DOWNLOAD ERROR: " + www.error);
				}
				else 
				{
					callback(www.text);
				}

				StopDownload();
			}
		}

		public void StopDownload () 
		{
			EditorApplication.update -= Download;
			www.Dispose();
		}
	}
}