namespace DarkHavoc.EntitiesInterfaces
{
    public interface IStunnable
    {
        float StunTime { get; }
        void Stun();
    }
}