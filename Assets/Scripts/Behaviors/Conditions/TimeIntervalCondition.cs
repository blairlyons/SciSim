using UnityEngine;
using System.Collections;

namespace SciSim
{
	[CreateAssetMenu( fileName = "TimeIntervalCondition", menuName = "Conditions/Time Interval", order = 2 )]
	public class TimeIntervalCondition : Condition 
	{
		public float timeInterval = 5f;
		public float variation = 2f;

		Timer timer;

		public override void Setup (Agent _agent)
		{
			base.Setup( _agent );

			timer = agent.gameObject.AddComponent<Timer>();
			timer.SetTimeOutDelegate( Satisfied );
			timer.Set( timeInterval, variation, true );
		}
	}
}
