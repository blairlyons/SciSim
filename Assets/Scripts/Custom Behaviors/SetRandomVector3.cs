using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.Math
{
	[TaskCategory("SciSim/Math")]
	[TaskDescription("Sets a random Vector3 value")]
	public class SetRandomVector3 : Action
	{
		[Tooltip("The minimum location bounds")]
		public SharedVector3 minLocation;
		[Tooltip("The maximum location bounds")]
		public SharedVector3 maxLocation;
		[Tooltip("The starting position")]
		public SharedVector3 anchor;
		[Tooltip("The maximum amount")]
		public SharedFloat maxDistance;
		[Tooltip("The variable to store the result")]
		public SharedVector3 storeResult;

		public override TaskStatus OnUpdate()
		{
			Vector3 random = anchor.Value + maxDistance.Value * Random.insideUnitSphere;

			storeResult.Value = new Vector3( Mathf.Clamp( random.x, minLocation.Value.x, maxLocation.Value.x ), 
				Mathf.Clamp( random.y, minLocation.Value.y, maxLocation.Value.y ), Mathf.Clamp( random.z, minLocation.Value.z, maxLocation.Value.z ) );
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			minLocation.Value = -Vector3.one;
			maxLocation.Value = Vector3.one;
			maxDistance.Value = 1f;
			storeResult.Value = Vector3.zero;
		}
	}
}