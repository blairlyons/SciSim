//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    public GameObject weaponBody;
    public IMBrush brush;

    public Animator animator;
//    public AudioSource audio;
    
    protected abstract void DoShoot(DungeonControl2 dungeon, Vector3 from, Vector3 to);

    public AudioClip equipAudio;
    public AudioClip shotAudio;


    public void Shoot(DungeonControl2 dungeon, Vector3 from, Vector3 to)
    {
        if (GetComponent<AudioSource>() != null && shotAudio != null)
        {
//            audio.PlayOneShot(shotAudio);
        }
        DoShoot(dungeon, from, to);
    }

    public void OnEquip()
    {
        animator.SetBool("EQUIP", true);
        if( GetComponent<AudioSource>() != null && equipAudio != null )
        {
//            audio.PlayOneShot(equipAudio);
        }
    }

    public void OnRemove()
    {
        animator.SetBool("EQUIP", false);
    }
}
