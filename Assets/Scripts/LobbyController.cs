using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc
{
    public class LobbyController : RepetitiveInteractive<Player>
    {
        protected override void TriggerInteraction(Player player) =>
            ServiceLocator.GetService<GameManager>()?.StartGame();
    }
}