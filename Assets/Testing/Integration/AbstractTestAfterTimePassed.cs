using UnityEngine;
using System.Collections;

public abstract class AbstractTestAfterTimePassed : MonoBehaviour 
{
	public float waitTime = 0.25f;
	private bool tested = false;

	protected virtual void Update () 
	{
		if (Time.time > waitTime && !tested) 
		{
			bool testResult = DoTest();

			if (testResult) 
			{
				IntegrationTest.Pass();
			} 
			else 
			{
				IntegrationTest.Fail();
			}
			tested = true;
		}
	}

	protected abstract bool DoTest ();
}
