using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public class ZoomController : MonoBehaviour 
	{
		public delegate void ScaleEvent();
		public static event ScaleEvent OnScaleChange;

		public float currentScale = 1f; // = one unit in the unity grid (ex: 1 unity unit = 1 nm)
		public Units currentUnits = Units.Nanometers;

		static ZoomController _Instance;
		public static ZoomController Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = GameObject.FindObjectOfType<ZoomController>();
				}
				return _Instance;
			}
		}

		Transform anchor;

		void Start ()
		{
			CreateAnchor();
		}

		void CreateAnchor ()
		{
			anchor = new GameObject("Anchor").transform;
			anchor.position = Vector3.zero;

			GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (GameObject root in roots) 
			{
				if (root.GetComponent<Agent>())
				{
					root.transform.SetParent(anchor);
				}
			}
		}

		public void Zoom (float dZoom)
		{
			currentScale += dZoom * currentScale;
			CheckChangeUnits();
			UpdateAnchorScale();

			if (OnScaleChange != null) 
			{
				OnScaleChange();
			}
		}

		void CheckChangeUnits ()
		{
			if (currentScale < 1f || currentScale > 1E3f)
			{
				currentScale = Mathf.Clamp(currentScale, 1f, 1E3f);
				Units newUnits = ScaleUtility.GetNextScale(currentUnits, currentScale > 1f ? 1 : -1);
				currentScale = currentScale * ScaleUtility.ConvertUnitMultiplier(currentUnits, newUnits);
				currentUnits = newUnits;
			}
		}

		void UpdateAnchorScale ()
		{
			anchor.localScale = 1E3f / currentScale * Vector3.one;
		}
	}
}