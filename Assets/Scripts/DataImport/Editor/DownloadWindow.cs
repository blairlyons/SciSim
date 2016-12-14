using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace SciSim
{
	public class DownloadWindow : EditorWindow 
	{
		protected WWW downloader;
		protected bool downloadInProgress;

		protected void StartDownload (string url) 
		{
			downloader = new WWW(url);
			EditorApplication.update += Download;
			downloadInProgress = true;
		}

		void Download () 
		{
			if (downloader.isDone) 
			{
				if (!string.IsNullOrEmpty(downloader.error)) 
				{
					Debug.LogWarning("DOWNLOAD ERROR: " + downloader.error);
				}
				else 
				{
					DownloadFinished();
				}

				StopDownload();
			}
		}

		public void StopDownload () 
		{
			EditorApplication.update -= Download;
			downloader.Dispose();
			downloadInProgress = false;
		}

		protected virtual void DownloadFinished ()
		{
			Debug.Log("DOWNLOADED: " + downloader.text);
		}
	}
}