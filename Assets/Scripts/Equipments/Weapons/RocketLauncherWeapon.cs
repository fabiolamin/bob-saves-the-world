using System.Collections;
using UnityEngine;

namespace BSTW.Equipments.Weapons
{
    public class RocketLauncherWeapon : Weapon
    {
        [SerializeField] private float _loadingDelay = 2f;
        protected override void SetProjectile()
        {
            if (projectiles.Count > 0)
            {
                currentProjectile = projectiles.Dequeue();
                currentProjectile.gameObject.SetActive(true);
            }
        }

        protected override IEnumerator LoadProjectile()
        {
            isProjectileLoaded = false;

            yield return new WaitForSeconds(_loadingDelay);

            isProjectileLoaded = true;
            SetProjectile();
        }
    }
}

