using BSTW.Player;

namespace BSTW.Equipments
{
    public class PlayerJetBackpack : JetBackpackUser
    {
        public override bool CanFuelJetpack()
        {
            return PlayerFoot.IsOnTheGround;
        }
    }
}

