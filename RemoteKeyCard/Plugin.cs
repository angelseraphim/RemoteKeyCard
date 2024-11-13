using System.Linq;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using KeycardPermission = Interactables.Interobjects.DoorUtils.KeycardPermissions;
using MapGeneration.Distributors;
using System.Reflection;

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
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
            base.OnDisabled();
        }
        private bool CheckPermission(Player player, KeycardPermission keycardPermissions)
        {
            foreach (Item item in player.Items.Where(i => i.IsKeycard))
            {
                if (item is Keycard keycard)
                {
                    Log.Debug(keycard.Base.Permissions + "\n" + keycardPermissions);
                    if ((keycard.Base.Permissions & keycardPermissions) != 0)
                        return true;
                }
            }
            return false;
        }
        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player == null || ev.Door.IsLocked || ev.IsAllowed)
                return;

            if (CheckPermission(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions))
                ev.IsAllowed = true;
        }
        private void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            var requiredPermissionField = typeof(Scp079Generator).GetField(nameof(Scp079Generator._requiredPermission), BindingFlags.NonPublic | BindingFlags.Instance);
            var requiredPermission = (KeycardPermission)requiredPermissionField.GetValue(ev.Generator.Base);

            if (CheckPermission(ev.Player, requiredPermission))
                ev.IsAllowed = true;
        }
        private void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (CheckPermission(ev.Player, KeycardPermission.AlphaWarhead))
                ev.IsAllowed = true;
        }
        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (CheckPermission(ev.Player, ev.Chamber.RequiredPermissions))
                ev.IsAllowed = true;
        }
    }
}
