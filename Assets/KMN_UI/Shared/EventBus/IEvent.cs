namespace KMN.EventsBus
{
    public interface IEvent
    {
        void Raise()
        {
            EventBus.Raise(this);
        }
    }
}