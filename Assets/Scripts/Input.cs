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
				ZoomIn();
			}
			if (UnityEngine.Input.GetKey(KeyCode.Period))
			{
				ZoomOut();
			}
		}

		void ZoomIn ()
		{
			BubbleGenerator.Instance.Zoom(-zoomSpeed);
		}

		void ZoomOut ()
		{
			BubbleGenerator.Instance.Zoom(+zoomSpeed);
		}
	}
}
