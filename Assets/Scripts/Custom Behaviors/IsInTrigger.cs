using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Returns success while the collider is colliding.")]
	[TaskCategory("SciSim/Physics")]
	public class IsInTrigger : Conditional
	{
		[Tooltip("The tag of the GameObject to check for a collision against")]
		public SharedInt layer;
		[Tooltip("The object that collided with the collider most recently and is still colliding")]
		public SharedGameObject collidingGameObject;

		private List<GameObject> collidingObjects = new List<GameObject>();

		public override TaskStatus OnUpdate()
		{
			return collidingObjects.Count > 0 ? TaskStatus.Success : TaskStatus.Failure;
		}

		public override void OnEnd()
		{
			collidingObjects.Clear();
		}

		public void OnTriggerEnter (Collider other)
		{
			Debug.Log("collision");
			if (other.gameObject.layer == layer.Value) 
			{
				Debug.Log(gameObject.name + " collided with " + other.gameObject.name);
				collidingGameObject.Value = other.gameObject;
				collidingObjects.Add( other.gameObject );
			}
		}

		public void OnTriggerExit (Collider other)
		{
			Debug.Log("exit");
			if (collidingObjects.Contains( other.gameObject )) 
			{
				Debug.Log(gameObject.name + " STOP collision with " + other.gameObject.name);
				collidingObjects.Remove( other.gameObject );
				if (collidingGameObject.Value == other.gameObject)
				{
					ResetCollidingObject();
				}
			}
		}

		void ResetCollidingObject ()
		{
			if (collidingObjects.Count > 0) 
			{
				collidingGameObject.Value = collidingObjects[collidingObjects.Count - 1];
			}
			else 
			{
				collidingGameObject.Value = null;
			}
		}

		public override void OnReset()
		{
			layer = 0;
			collidingGameObject = null;
		}
	}
}