using UnityEngine;
using System.Collections;
using UnityEditor;

public class Corral : MonoBehaviour
{
    public float maxDistance;
    public float currentDistance;

	void Update ()
    {
        currentDistance = Vector3.Distance(Stake.Instance.transform.position, transform.position);
        if (currentDistance > maxDistance)
        {
            Destroy(gameObject);
        }
	}
}
