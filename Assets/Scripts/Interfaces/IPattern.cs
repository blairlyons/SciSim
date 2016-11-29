using UnityEngine;
using System.Collections;

namespace SciSim
{
	//!  IPattern
	/*!
	  Interface describing a spawn pattern
	*/
	public interface IPattern
	{
		Vector3 GetPosition (Container container, int index, int n);
		/*!< Get the position inside the container for agent of index out of n agents */

		Quaternion GetRotation (Container container, int index, int n);
		/*!< Get the rotation inside the container for agent of index out of n agents */
	}
}