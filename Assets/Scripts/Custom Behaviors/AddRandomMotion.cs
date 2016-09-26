using UnityEngine;
using SciSim;

namespace BehaviorDesigner.Runtime.Tasks.SciSim
{
	[TaskCategory("SciSim/Animation")]
	[TaskDescription("Add an animator component with ambient animation")]
	public class AddRandomMotion : Action
	{
		[Tooltip("Gameobject for the agent")]
		public SharedGameObject targetAgent;
		[Tooltip("Multiply speed at which to play animation")]
		public SharedFloat speedMultiplier;
		[Tooltip("Multiply distance of animation path")]
		public SharedFloat sizeMultiplier;

		Agent _agent;
		Agent agent
		{
			get 
			{
				if (_agent == null)
				{
					_agent = targetAgent.Value.GetComponent<Agent>();
				}
				return _agent;
			}
		}

		public override TaskStatus OnUpdate()
		{
			if (agent == null || agent.visualization == null)
			{
				return TaskStatus.Running;
			}

			Animator animation = agent.visualization.gameObject.AddComponent<Animator>();
			animation.runtimeAnimatorController = Resources.Load("Animation/Ambient") as RuntimeAnimatorController;
			animation.SetFloat( "randomTimeOffset", Random.Range( 0, 1f ) );
			animation.SetFloat( "sizeOffset", sizeMultiplier.Value / agent.transform.localScale.x );
			animation.SetFloat( "speed", speedMultiplier.Value / agent.transform.localScale.x );

			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			speedMultiplier.Value = 1f;
			sizeMultiplier.Value = 0.5f;
		}
	}
}