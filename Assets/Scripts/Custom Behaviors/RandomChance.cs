using UnityEngine;
using System.Collections;

public class RandomChance : StateMachineBehaviour {

	public float updateInterval = 0.5f;
	public float reverseStateProbability = 0.3f;
	public float switchStateProbability = 0.3f;
	public int numberOfTransitionsAway;
	float lastTime;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (animator.GetInteger("nextState") >= 0)
		{
			animator.SetInteger("nextState", -1);
		}
//		updateInterval = 0.8f * stateInfo.length;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (Time.time - lastTime > updateInterval)
		{
			float reverse = Random.Range(0, 1f);
			if (reverse < reverseStateProbability)
			{
				animator.SetInteger("nextState", 0);
			}
			else 
			{
				float switchState = Random.Range(0, 1f);
				if (switchState < switchStateProbability)
				{
					int newState = Random.Range(1, numberOfTransitionsAway);
					animator.SetInteger("nextState", newState);
				}
			}
		}
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
