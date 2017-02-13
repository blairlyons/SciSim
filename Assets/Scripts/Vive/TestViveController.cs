using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TestViveController : ViveController
{
    public Button upButton;
    public Button leftButton;
    public Button rightButton;
    public Button downButton;

    public List<GameObject> prefabs = new List<GameObject>();

    public override void OnDPadUpEnter()
    {
        EventSystem.current.SetSelectedGameObject(upButton.gameObject);
    }

    public override void OnDPadLeftEnter()
    {
        EventSystem.current.SetSelectedGameObject(leftButton.gameObject);
    }

    public override void OnDPadRightEnter()
    {
        EventSystem.current.SetSelectedGameObject(rightButton.gameObject);
    }

    public override void OnDPadDownEnter()
    {
        EventSystem.current.SetSelectedGameObject(downButton.gameObject);
    }

    public override void OnDPadExit()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnDPadUpPressed()
    {
        //SetCameraBackground( upButton.colors.normalColor );
        AddObject(0);
    }

    public override void OnDPadLeftPressed()
    {
        //SetCameraBackground( leftButton.colors.normalColor );
        AddObject(1);
    }

    public override void OnDPadRightPressed()
    {
        //SetCameraBackground( rightButton.colors.normalColor );
        AddObject(2);
    }

    public override void OnDPadDownPressed()
    {
        //SetCameraBackground( downButton.colors.normalColor );
        AddObject(3);
    }

    void SetCameraBackground(Color color)
    {
        GameObject.Find("Camera (eye)").GetComponent<Camera>().backgroundColor = color;
    }

    void AddObject (int index)
    {
        if (index < prefabs.Count && prefabs[index] != null)
        {
            Instantiate(prefabs[index], transform.position + 0.25f * transform.forward, Quaternion.identity);
        }
    }
}
