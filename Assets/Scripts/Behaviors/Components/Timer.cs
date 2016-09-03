using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Timer : MonoBehaviour 
	{
		event RuleDelegate timeOutDelegate;

		bool continuous;
		float timeInterval = 5f;
		float variation = 2f;

		float lastTime;
		float t = Mathf.Infinity;

		public void SetTimeOutDelegate (RuleDelegate _timeOutDelegate)
		{
			timeOutDelegate = _timeOutDelegate;
		}

		public void Set (float _timeInterval, float _variation, bool _continuous)
		{
			timeInterval = _timeInterval;
			variation = _variation;
			continuous = _continuous;

			StartTimer();
		}

		void StartTimer ()
		{
			t = Random.Range( timeInterval - variation, timeInterval + variation );
			lastTime = Time.time;
		}

		public void StopTimer ()
		{
			t = Mathf.Infinity;
		}

		void Update ()
		{
			if (Time.time - lastTime >= t)
			{
				if (timeOutDelegate != null)
				{
					timeOutDelegate();
				}

				if (continuous)
				{
					StartTimer();
				}
				else 
				{
					StopTimer();
				}
			}
		}
	}
}