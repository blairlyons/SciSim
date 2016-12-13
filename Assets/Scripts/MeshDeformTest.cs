using UnityEngine;
using System.Collections;

public class MeshDeformTest : MonoBehaviour 
{
	void Update() 
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		int i = 0;
		while (i < vertices.Length) 
		{
			vertices[i] += normals[i] * 0.01f * Mathf.Sin(Time.time);
			i++;
		}
		mesh.vertices = vertices;
	}
}