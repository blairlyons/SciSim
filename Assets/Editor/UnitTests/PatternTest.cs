using UnityEngine;
using NUnit.Framework;
using SciSim;

public class PatternTest 
{
	Container _testContainer;
	Container testContainer
	{
		get 
		{
			if (_testContainer == null) 
			{
				_testContainer = new GameObject().AddComponent<Container>();
				_testContainer.transform.position = Vector3.zero;
			}
			return _testContainer;
		}
	}

	[Test]
	public void RandomPatternPositionsAreInContainer ()
	{
		RandomPattern pattern = ScriptableObject.CreateInstance<RandomPattern>();
		Bounds bounds = testContainer.bounds;
		for (int i = 0; i < 50; i++)
		{
			Vector3 pos = pattern.GetPositionInContainer( testContainer, i, 50 );
			if (!bounds.Contains(pos))
			{
				Assert.Fail();
			}
		}
		Assert.Pass();
	}
}
