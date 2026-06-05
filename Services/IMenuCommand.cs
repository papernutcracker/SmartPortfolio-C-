namespace SmartDividendTracker.Services
{
    public interface IMenuCommand
    {
        string DisplayName { get; }

        void Execute();
    }
}