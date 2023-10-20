namespace AttackComponents
{
    public interface ITakeDamage
    {
        int Health { get; }
        bool IsAlive => Health > 0f;
        void TakeDamage(IDoDamage damageDealer);
        void Death();
    }
}