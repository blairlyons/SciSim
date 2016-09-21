using UnityEngine;
using System.Collections;
using SciSim;

public class FactoryMakeAgentsTest : AbstractTestAfterTimePassed 
{
	public float minAgents = 8;
	public float maxAgents = 12;

	protected override bool DoTest ()
	{
		int n = GetComponent<Factory>().agents.Count;
		return (n >= minAgents && n <= maxAgents);
	}
}
