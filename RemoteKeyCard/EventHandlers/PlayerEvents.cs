using Exiled.Events.EventArgs.Player;
using Interactables.Interobjects.DoorUtils;
using MapGeneration.Distributors;
using System.Reflection;

namespace RemoteKeyCard.EventHandlers
{
    internal class PlayerEvents
    {
        internal void Register()
        {
            Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
            Exiled.Events.Handlers.Player.UnlockingGenerator += OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel += OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker += OnInteractingLocker;
        }

        internal void Unregister()
        {
            Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
            Exiled.Events.Handlers.Player.UnlockingGenerator -= OnUnlockingGenerator;
            Exiled.Events.Handlers.Player.ActivatingWarheadPanel -= OnActivatingWarheadPanel;
            Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractingLocker;
        }

        private void OnInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (ev.Player == null || ev.Door.IsLocked || ev.IsAllowed)
                return;

            if (Plugin.CheckPermission(ev.Player, ev.Door.RequiredPermissions.RequiredPermissions))
                ev.IsAllowed = true;
        }

        private void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;
            
            var requiredPermissionField = typeof(Scp079Generator).GetField(nameof(Scp079Generator._requiredPermission), BindingFlags.NonPublic | BindingFlags.Instance);
            var requiredPermission = (KeycardPermissions)requiredPermissionField.GetValue(ev.Generator.Base);

            if (Plugin.CheckPermission(ev.Player, requiredPermission))
                ev.IsAllowed = true;
        }

        private void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (Plugin.CheckPermission(ev.Player, KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (Plugin.CheckPermission(ev.Player, ev.InteractingChamber.Base.RequiredPermissions))
                ev.IsAllowed = true;
        }
    }
}
