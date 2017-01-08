using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class Input : MonoBehaviour 
	{
		public float zoomSpeed = 0.1f;

		void Update () 
		{
			if (UnityEngine.Input.GetKey(KeyCode.Comma))
			{
				ZoomOut();
			}
			if (UnityEngine.Input.GetKey(KeyCode.Period))
			{
				ZoomIn();
			}
		}

		void ZoomIn ()
		{
			ZoomController.Instance.Zoom(-zoomSpeed);
		}

		void ZoomOut ()
		{
			ZoomController.Instance.Zoom(+zoomSpeed);
		}
	}
}
