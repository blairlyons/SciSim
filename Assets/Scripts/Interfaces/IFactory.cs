using UnityEngine;
using System.Collections;

namespace SciSim
{
	//!  IFactory
	/*!
	  Factory that makes agents inside containers
	*/
	public interface IFactory
	{
		void MakeAgents ();
		/*!< Make the agents */
	}
}