using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace Console.Net
{
    public class ConsoleCore
    {
        private static Icon _icon;

        /// <summary> Заголовок окна </summary>
        public static string Title
        {
            get => System.Console.Title;
            set => System.Console.Title = value;
        }

        public static int Width => System.Console.WindowWidth;
        public static int Height => System.Console.WindowHeight;

        /// <summary> координаты Х курсора </summary>
        public static int X
        {
            get => System.Console.CursorLeft;
            set => System.Console.CursorLeft = value;
        }

        /// <summary> координаты У курсора </summary>
        public static int Y
        {
            get => System.Console.CursorTop;
            set => System.Console.CursorTop = value;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        public static Icon Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                var mwHandle = ConsoleWindow.GetMainHandle;
                if (mwHandle == IntPtr.Zero) return;
                SendMessage(mwHandle, 0x0080, 0, _icon.Handle);
                SendMessage(mwHandle, 0x0080, 1, _icon.Handle);
            }
        }

        /// <summary> Ожидание ввода с клавиатуры символа/ов (или любого символа, если не указать символы) </summary>
        /// <param name="prm"> Символы, на которые должна быть реакция </param>
        public static void WaitPressAnyKey(params char[] prm)
        {
            while (true)
            {
                var ch = System.Console.ReadKey(false);
                if(prm!=null && !prm.Contains(ch.KeyChar))
                    continue;
                break;
            }
        }


    }
}
