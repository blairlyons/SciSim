using UnityEngine;
using System.Collections;

public class Stake : MonoBehaviour
{
    static Stake _Instance;
    public static Stake Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<Stake>();
            }
            return _Instance;
        }
    }
}
