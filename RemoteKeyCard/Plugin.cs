using Exiled.API.Features;
using Exiled.API.Features.Items;
using RemoteKeyCard.EventHandlers;
using KeycardPermission = Interactables.Interobjects.DoorUtils.KeycardPermissions;

namespace RemoteKeyCard
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "RemoteKeyCard";
        public override string Name => "RemoteKeyCard";
        public override string Author => "angelseraphim.";

        private PlayerEvents playerEvents;

        public override void OnEnabled()
        {
            playerEvents = new PlayerEvents();
            playerEvents.Register();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            playerEvents.Unregister();
            playerEvents = null;

            base.OnDisabled();
        }

        internal static bool CheckPermission(Player player, KeycardPermission keycardPermissions)
        {
            foreach (Item item in player.Items)
            {
                if (item is Keycard keycard)
                {
                    if ((keycard.Base.Permissions & keycardPermissions) != 0)
                        return true;
                }
            }

            return false;
        }
    }
}
