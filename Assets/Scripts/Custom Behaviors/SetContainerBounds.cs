using UnityEngine;
using SciSim;

namespace BehaviorDesigner.Runtime.Tasks.SciSim
{
	[TaskCategory("SciSim/Container")]
	[TaskDescription("Sets variables to the container bounds")]
	public class SetValue : Action
	{
		[Tooltip("Gameobject for the agent")]
		public SharedGameObject targetAgent;
		[Tooltip("Where to save the min bounds")]
		public SharedVector3 containerMin;
		[Tooltip("Where to save the max bounds")]
		public SharedVector3 containerMax;

		public override TaskStatus OnUpdate()
		{
			Container container = targetAgent.Value.transform.GetComponentInParent<Container>();
			if (container != null)
			{
				containerMin.Value = container.bounds.min;
				containerMax.Value = container.bounds.max;
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			containerMin = containerMax = Vector3.zero;
		}
	}
}