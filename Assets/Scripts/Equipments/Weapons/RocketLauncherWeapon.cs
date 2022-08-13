namespace BSTW.Equipments.Weapons
{
    public class RocketLauncherWeapon : Weapon
    {
        public override void SetProjectile()
        {
            if (projectiles.Count > 0)
            {
                CurrentProjectile = projectiles.Dequeue();
                CurrentProjectile.gameObject.SetActive(true);
                isProjectileLoaded = true;

                characterShooting.PlayReloadSFX();
            }
        }
    }
}

