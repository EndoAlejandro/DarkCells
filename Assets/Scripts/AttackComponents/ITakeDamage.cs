namespace AttackComponents
{
    public interface ITakeDamage
    {
        int Health { get; }
        bool IsAlive => Health > 0f;
        void TakeDamage(int damage);
        void Death();
    }
}