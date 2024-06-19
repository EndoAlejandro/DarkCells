using DarkHavoc.Managers;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc.Interactable
{
    public class FinalDoor : TriggerInteractive<Player>
    {
        protected override void TriggerInteraction() => ServiceLocator.GetService<GameManager>()?.GoToGameOver();
    }
}