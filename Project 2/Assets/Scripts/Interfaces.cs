using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Fly();
}
public interface ITurret
{
    void ArmTurret();
    void FireProjectile(GameObject projectile);
    bool SearchPlayer();
}
