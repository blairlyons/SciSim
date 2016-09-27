using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("SciSim/Animation")]
	[TaskDescription("Move the gameobject from the current position to the target position.")]
	public class MoveGameobjectTowards : Action
	{
		[Tooltip("The gameobject to move")]
		public SharedGameObject gameobjectToMove;
		[Tooltip("The target position")]
		public SharedVector3 targetPosition;
		[Tooltip("The movement speed")]
		public SharedFloat speed;

		public override TaskStatus OnUpdate()
		{
			gameobjectToMove.Value.transform.position = Vector3.MoveTowards(gameobjectToMove.Value.transform.position, targetPosition.Value, speed.Value * Time.deltaTime);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			targetPosition = Vector3.zero;
			speed = 0;
		}
	}
}