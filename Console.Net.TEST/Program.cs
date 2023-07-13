using System.Configuration;

namespace Console.Net.TEST
{
    internal class Program : ConsoleContext
    {
        static void Main(string[] args)
        {
            SetNoResizeWindow();
            HideMaximized();
            WindowSize(120, 32);
            WindowCenter();
            Title = "Тест фреймворка";
            Icon = Properties.Resources.ding;
            Origin.Show();
            WaitPressAnyKey(new []{'y','Y'});
        }
    }
}
