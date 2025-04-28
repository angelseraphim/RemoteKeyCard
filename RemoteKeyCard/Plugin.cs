using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using RemoteKeyCard.EventHandlers;

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

        internal static bool CheckHasFlag(Player player, KeycardPermissions keycardPermissions)
        {
            foreach (Item item in player.Items)
            {
                if (item is Keycard keycard)
                {
                    if (keycard.Permissions.HasFlag(keycardPermissions))
                        return true;
                }
            }

            return false;
        }

        internal static bool CheckByteFlag(Player player, KeycardPermissions keycardPermissions)
        {
            foreach (Item item in player.Items)
            {
                if (item is Keycard keycard)
                {
                    if ((keycard.Permissions & keycardPermissions) != 0)
                        return true;
                }
            }

            return false;
        }
    }
}
