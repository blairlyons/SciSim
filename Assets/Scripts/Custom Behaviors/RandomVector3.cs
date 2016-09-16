using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("Basic/Math")]
	[TaskDescription("Sets a random Vector3 value")]
	public class RandomVector3 : Action
	{
		[Tooltip("The minimum amount")]
		public SharedVector3 min;
		[Tooltip("The maximum amount")]
		public SharedVector3 max;
		[Tooltip("Is the maximum value inclusive?")]
		public bool inclusive;
		[Tooltip("Use anchor?")]
		public SharedBool useAnchor;
		[Tooltip("The point to anchor around")]
		public SharedVector3 anchor;
		[Tooltip("The maximum distance result can be from anchor")]
		public SharedFloat maxDistanceFromAnchor;
		[Tooltip("The variable to store the result")]
		public SharedVector3 storeResult;

		public override TaskStatus OnUpdate()
		{
			Vector3 result;

			if (inclusive) {
				result = new Vector3( Random.Range( min.Value.x, max.Value.x + 1 ), Random.Range( min.Value.y, max.Value.y + 1 ), Random.Range( min.Value.z, max.Value.z + 1 ) );
			} else {
				result = new Vector3( Random.Range( min.Value.x, max.Value.x ), Random.Range( min.Value.y, max.Value.y ), Random.Range( min.Value.z, max.Value.z ) );
			}

			if (useAnchor.Value)
			{
				result = anchor.Value + Vector3.ClampMagnitude( result - anchor.Value, maxDistanceFromAnchor.Value );
			}
			Debug.Log("Random position = " + result);
			storeResult.Value = result;
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			min.Value = Vector3.zero;
			max.Value = Vector3.zero;
			inclusive = false;
			storeResult.Value = Vector3.zero;
		}
	}
}