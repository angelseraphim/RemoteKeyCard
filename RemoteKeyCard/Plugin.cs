using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Enums;
using Exiled.API.Features.Items;

namespace RemoteKeyCard
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "RemoteKeyCard";
        public override string Name => "RemoteKeyCard";
        public override string Author => "angelseraphim.";
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.OpeningGenerator += OnOpeningGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.OpeningGenerator -= OnOpeningGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            base.OnDisabled();
        }
        private bool CheckPermission(Player player, KeycardPermissions keycardPermissions)
        {
            foreach (Item item in player.Items.Where(i => i.IsKeycard))
            {
                if (item is Keycard keycard && keycard.Permissions.HasFlag(keycardPermissions))
                {
                    return true;
                }
            }
            return false;
        }
        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player == null || ev.Door.IsLocked)
                return;
            
            if (CheckPermission(ev.Player, (KeycardPermissions)ev.Door.RequiredPermissions.RequiredPermissions))
                ev.IsAllowed = true;
        }
        private void OnOpeningGenerator(OpeningGeneratorEventArgs ev)
        {
            if (ev.Player == null)
                return;

            if (CheckPermission(ev.Player, (KeycardPermissions)ev.Generator.Base._requiredPermission))
                ev.IsAllowed = true;
        }
        private void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player == null)
                return;

            if (CheckPermission(ev.Player, KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }
        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == null)
                return;

            if (CheckPermission(ev.Player, (KeycardPermissions)ev.Chamber.RequiredPermissions))
                ev.IsAllowed = true;
        }
    }
}
