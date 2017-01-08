using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
	Material _psMaterial;
	Material psMaterial
	{
		get
		{
			if (_psMaterial == null)
			{
				_psMaterial = GetComponent<ParticleSystemRenderer>().material;
			}
			return _psMaterial;
		}
	}

	public void OnWillRenderObject()
	{
		psMaterial.SetVector("_Center", transform.position);
		psMaterial.SetVector("_Scaling", transform.lossyScale);
		psMaterial.SetMatrix("_Camera", Camera.current.worldToCameraMatrix);
		psMaterial.SetMatrix("_CameraInv", Camera.current.worldToCameraMatrix.inverse);
	}
}