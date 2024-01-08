using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc
{
    public class ExitDoor : TriggerInteractive<Player>
    {
        protected override void TriggerInteraction()
        {
            //TODO: Go to next level.
            ServiceLocator.Instance.GetService<GameManager>().GoToNextLevel();
        }
    }
}