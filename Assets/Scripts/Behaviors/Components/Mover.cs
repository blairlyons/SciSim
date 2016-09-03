using UnityEngine;
using System.Collections;

namespace SciSim
{
	public class Mover : MonoBehaviour 
	{
		event RuleDelegate finishedMovingDelegate;
		bool listenOnce;

		bool shouldMove;
		float startTime;
		float percentComplete;
		float increment;
		float duration;
		float velocity = 5f;
		Vector3 startPosition;
		Transform destinationObject;
		Vector3 destination;
		Vector3 goalPosition;

		public void SetFinishedMovingDelegate (RuleDelegate _finishedMovingDelegate, bool _listenOnce)
		{
			finishedMovingDelegate = _finishedMovingDelegate;
			listenOnce = _listenOnce;
		}

		public void MoveToWithDuration (Vector3 _destination, float _duration, Transform _destinationObject = null)
		{
			SetDestination( _destination, _destinationObject );

			duration = _duration;
			increment = 0;

			StartMoving();
		}

		public void MoveToWithVelocity (Vector3 _destination, float _velocity = 0, Transform _destinationObject = null)
		{
			SetDestination( _destination, _destinationObject );

			SetIncrement( _velocity );

			StartMoving();
		}

		void SetDestination (Vector3 _destination, Transform _destinationObject = null)
		{
			destination = _destination;
			destinationObject = _destinationObject;
			if (destinationObject != null) 
			{
				goalPosition = destinationObject.position + destination;
			}
			else 
			{
				goalPosition = destination;
			}
		}

		void SetIncrement (float _velocity = 0)
		{
			if (_velocity != 0) 
			{
				velocity = _velocity;
			}
			increment = velocity / Vector3.Distance( transform.position, goalPosition );
		}

		void StartMoving ()
		{
			startTime = Time.time;
			startPosition = transform.position;
			percentComplete = 0;
			shouldMove = true;
		}

		void Update ()
		{
			if (shouldMove)
			{
				Move();
			}
		}

		void Move ()
		{
			UpdateDestination();

			if (increment != 0) 
			{
				percentComplete += increment;
			} 
			else 
			{
				percentComplete = (Time.time - startTime) / duration;
			}

			transform.position = Vector3.Lerp( startPosition, goalPosition, percentComplete );
			
			if (percentComplete >= 1) 
			{
				StopMoving();
			}
		}

		void UpdateDestination ()
		{
			if (destinationObject != null && goalPosition != destinationObject.position + destination) 
			{
				goalPosition = destinationObject.position + destination;

				if (increment != 0)
				{
					//startPosition = transform.position; //???
					SetIncrement();
				}
			}
		}

		void StopMoving ()
		{
			transform.position = destinationObject.position + destination;
			shouldMove = false;

			if (finishedMovingDelegate != null) 
			{
				finishedMovingDelegate();
			}
			if (listenOnce) 
			{
				finishedMovingDelegate = null;
			}
		}
	}
}