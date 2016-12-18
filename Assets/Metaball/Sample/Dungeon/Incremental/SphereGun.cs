//--------------------------------
// Skinned Metaball Builder
// Copyright © 2015 JunkGames
//--------------------------------

using UnityEngine;
using System.Collections;

public class SphereGun : Weapon
{
    public ParticleSystem hitPS;

    protected override void DoShoot(DungeonControl2 dungeon, Vector3 from, Vector3 to)
    {
        weaponBody.transform.position = to;
        dungeon.Attack(brush);

        Instantiate(hitPS.gameObject, to, Quaternion.identity);
    }
}
