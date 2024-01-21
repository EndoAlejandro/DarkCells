using DarkHavoc.DungeonGeneration.GridBasedGenerator;
using DarkHavoc.PlayerComponents;
using DarkHavoc.ServiceLocatorComponents;

namespace DarkHavoc
{
    public class Exit : RepetitiveInteractive<Player>
    {
        protected override void TriggerInteraction(Player player)
        {
            ServiceLocator.GetService<LevelManager>()?.ExitLevel();
        }
    }
}