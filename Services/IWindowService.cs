namespace DispoDataAssistant.Services
{
    public interface IWindowService
    {
        void CloseWindow();
        void MaximizeWindow();
        void MinimizeWindow();
        void RestoreWindow();
    }
}
