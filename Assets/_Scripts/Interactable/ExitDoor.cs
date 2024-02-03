using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc.Interactable
{
    public class ExitDoor : TriggerInteractive<Player>
    {
        protected override void TriggerInteraction() => ServiceLocator.GetService<GameManager>().GoToNextLevel();
    }
}