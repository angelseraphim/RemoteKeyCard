using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;

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

            if (Plugin.CheckByteFlag(ev.Player, ev.Door.KeycardPermissions))
                ev.IsAllowed = true;
        }

        private void OnUnlockingGenerator(UnlockingGeneratorEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (Plugin.CheckByteFlag(ev.Player, ev.Generator.KeycardPermissions))
                ev.IsAllowed = true;
        }

        private void OnActivatingWarheadPanel(ActivatingWarheadPanelEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (Plugin.CheckByteFlag(ev.Player, KeycardPermissions.AlphaWarhead))
                ev.IsAllowed = true;
        }

        private void OnInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (ev.Player == null || ev.IsAllowed)
                return;

            if (Plugin.CheckHasFlag(ev.Player, ev.InteractingChamber.RequiredPermissions))
                ev.IsAllowed = true;
        }
    }
}
