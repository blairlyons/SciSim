using UnityEngine;
using System.Collections;

namespace SciSim
{
	//!  IAgent
	/*!
	  Interface describing an agent, the basic unit of simulation
	*/
	public interface IAgent
	{
		void Init ();
		/*!< Initialize the agent: set scale, visualize, add behaviors */

		bool IsSameAgent (IAgent other);
		/*!< is this agent the same type of instance as the other? */
	}
}