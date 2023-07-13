
namespace Console.Net.TEST
{
    internal class Program : ConsoleContext
    {
        static void Main(string[] args)
        {
            HideMaximized();
            SetNoResizeWindow();
            WindowSize(120, 32);
            WindowCenter();
            WaitPressAnyKey(new []{'y','Y'});
        }
    }
}
