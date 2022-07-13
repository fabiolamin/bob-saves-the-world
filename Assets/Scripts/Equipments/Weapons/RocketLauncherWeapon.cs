namespace BSTW.Equipments.Weapons
{
    public class RocketLauncherWeapon : Weapon
    {
        public override void SetProjectile()
        {
            if (projectiles.Count > 0)
            {
                currentProjectile = projectiles.Dequeue();
                currentProjectile.gameObject.SetActive(true);
                isProjectileLoaded = true;
            }
        }
    }
}

