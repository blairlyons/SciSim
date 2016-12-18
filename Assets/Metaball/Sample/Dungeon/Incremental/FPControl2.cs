//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class FPControl2 : MonoBehaviour {

    public Camera myCamera;
    public CharacterController cc;

    public float walkSpeed = 3.0f;

    float _theta;
    float _phi;

    public float rotSpeed = 1.0f;

    float _mx;
    float _my;

//	public IMBrush currentBrush;
    int _currentWeaponIdx = -1;
    Weapon _currentWeapon;
    public Weapon [] weapons;

    void ChangeWeapon()
    {
        if( _currentWeapon != null )
        {
            _currentWeapon.OnRemove();
        }

        _currentWeaponIdx = (_currentWeaponIdx + 1) % weapons.Length;

        _currentWeapon = weapons[_currentWeaponIdx];
        _currentWeapon.OnEquip();
    }

	// Use this for initialization
	void Start () {
        Vector3 forward = myCamera.transform.forward;
        _phi = Mathf.Asin(forward.y);
        _theta = Mathf.Atan2(forward.x, forward.z);

        _mx = Input.mousePosition.x;// Input.GetAxis("Mouse X");
        _my = Input.mousePosition.y;// Input.GetAxis("Mouse Y");

        ChangeWeapon();
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float mx = Input.mousePosition.x;// Input.GetAxis("Mouse X");
        float my = Input.mousePosition.y;// Input.GetAxis("Mouse Y");

        float deltaMx = mx - _mx;
        float deltaMy = my - _my;

        _mx = mx;
        _my = my;

        Vector3 cameraForward = myCamera.transform.forward;
        cameraForward.y = 0.0f;
        cameraForward.Normalize();

        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);

        Vector3 moveSpeed = walkSpeed * ( cameraForward * v + cameraRight * h ) + Vector3.down*3.0f;

        cc.Move(moveSpeed * Time.deltaTime);        
        
        _theta = _theta - deltaMx * rotSpeed;

        if( _theta > Mathf.PI )
        {
            _theta -= (Mathf.PI * 2);
        }
        else if (_theta <= -Mathf.PI)
        {
            _theta += (Mathf.PI * 2);
        }

        _phi = _phi + deltaMy * rotSpeed;
        if( _phi > 1.0f )
        {
            _phi = 1.0f;
        }
        else if( _phi < -1.0f )
        {
            _phi = -1.0f;
        }

        Vector3 fwd = new Vector3(Mathf.Cos(_phi) * Mathf.Cos(_theta), Mathf.Sin(_phi), Mathf.Cos(_phi) * Mathf.Sin(_theta));
        myCamera.transform.LookAt(myCamera.transform.position + fwd, Vector3.up);




        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        if( Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) )
        {
            ChangeWeapon();
        }
	}

    public void Shoot()
    {
        Ray ray = myCamera.ScreenPointToRay(new Vector3(myCamera.pixelWidth * 0.5f, myCamera.pixelHeight * 0.5f, 0.0f));
       // ray.origin = ray.origin + ray.direction;

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo ))
        {
            Transform target = hitInfo.collider.transform;

            DungeonControl2 dungeon = Utils.FindComponentInParents<DungeonControl2>(target);

            if (dungeon != null)
            {
                _currentWeapon.Shoot(dungeon, myCamera.transform.position, hitInfo.point);
            }
        }
    }
}
