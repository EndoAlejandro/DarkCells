namespace DarkHavoc.Enemies.Summoner
{
    public class Summoner : Enemy
    {
        public override float Damage => 1f;
        public new SummonEnemyHitBox HitBox => hitbox as SummonEnemyHitBox;
    }
}