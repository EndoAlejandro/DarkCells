using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc
{
    public class LobbyController : InteractiveTrigger<Player>
    {
        protected override void TriggerInteraction(Player player) =>
            ServiceLocator.Instance.GetService<GameManager>()?.StartGame();
    }
}