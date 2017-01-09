using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SciSim
{
	public delegate void NavigationEvent();

	public class Input : MonoBehaviour 
	{
		public float zoomSpeed = 0.01f;
		public float moveSpeed = 0.01f;

		Vector3 moveDirection = Vector3.zero;

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

			if (UnityEngine.Input.GetKey(KeyCode.W))
			{
				moveDirection += Vector3.forward;
			}
			if (UnityEngine.Input.GetKey(KeyCode.A))
			{
				moveDirection += Vector3.left;
			}
			if (UnityEngine.Input.GetKey(KeyCode.S))
			{
				moveDirection += Vector3.back;
			}
			if (UnityEngine.Input.GetKey(KeyCode.D))
			{
				moveDirection += Vector3.right;
			}
			if (UnityEngine.Input.GetKey(KeyCode.Z))
			{
				moveDirection += Vector3.down;
			}
			if (UnityEngine.Input.GetKey(KeyCode.X))
			{
				moveDirection += Vector3.up;
			}
			Move();
		}

		void ZoomIn ()
		{
			ZoomController.Instance.Zoom(-zoomSpeed);
		}

		void ZoomOut ()
		{
			ZoomController.Instance.Zoom(+zoomSpeed);
		}

		void Move ()
		{
			if (moveDirection != Vector3.zero)
			{
				MoveController.Instance.Move(moveDirection.normalized, moveSpeed);
				moveDirection = Vector3.zero;
			}
		}
	}
}
