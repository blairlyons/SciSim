using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "MoveEffect", menuName = "Effects/Move", order = 3 )]
	public class MoveEffect : Effect 
	{
		public bool moveRandomly;
		public float maxDistancePerMove = 1f;
		public Transform destinationObject;
		public Vector3 destination = Vector3.zero;

		public bool moveWithDuration;
		public float duration = 5f;
		public float velocity = 5f;

		Mover mover;

		public override void Setup (Agent _agent)
		{
			base.Setup( _agent );

			mover = agent.gameObject.AddComponent<Mover>();
			mover.SetFinishedMovingDelegate( ExecutionComplete, false );
		}

		public override void Execute ()
		{
			if (moveRandomly)
			{
				destination = mover.transform.position + Random.Range( 0.1f * maxDistancePerMove, maxDistancePerMove ) * Random.insideUnitSphere;
			}

			if (moveWithDuration)
			{
				mover.MoveToWithDuration( destination, duration, destinationObject );
			}
			else 
			{
				mover.MoveToWithVelocity( destination, velocity, destinationObject );
			}
		}
	}
}
