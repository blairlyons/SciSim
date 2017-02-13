using UnityEngine;
using System.Collections;

public class RandomMotion : MonoBehaviour
{
    public float speed;

    float lastTime;

    Rigidbody _body;
    Rigidbody body
    {
        get
        {
            if (_body == null)
            {
                _body = GetComponent<Rigidbody>();
            }
            return _body;
        }
    }

    Vector3 randomVector
    {
        get
        {
            return new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f);
        }
    }

    void Start ()
    {
        transform.rotation = Random.rotation;
    }

    void FixedUpdate ()
    {
        if (Time.time - lastTime > 0.1f)
        {
            body.AddForce(speed * randomVector, ForceMode.Impulse);
            body.AddTorque(10000000f * speed * randomVector, ForceMode.Impulse);

            lastTime = Time.time;
        }
	}
}
