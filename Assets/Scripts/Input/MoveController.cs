using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class MoveController : MonoBehaviour 
	{
		public static event NavigationEvent OnMove;

		static MoveController _Instance;
		public static MoveController Instance
		{
			get 
			{
				if (_Instance == null)
				{
					_Instance = GameObject.FindObjectOfType<MoveController>();
				}
				return _Instance;
			}
		}

		public void Move (Vector3 direction, float moveSpeed)
		{
			transform.position += moveSpeed * Camera.main.transform.TransformDirection(direction);

			if (OnMove != null)
			{
				OnMove();
			}
		}
	}
}