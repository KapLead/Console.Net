using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Console.Net
{
    public class Buffer
    {
        private BufItem[,] items;

        public int Left { get; private set; } = 0;
        public int Top { get; private set; } = 0;
        public int Right => Width - Left;
        public int Button => Height - Top;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Point Location
        {
            get => new Point(Left, Top);
            set
            {
                Left = value.X;
                Top = value.Y;
            }
        }

        public BufItem this[int x, int y]
        {
            get => items[x, y];
            set => items[x, y] = value;
        }

        public Buffer(int width, int height)
        {
            items = new BufItem[ Width=width,Height=height ];
            Clear();
        }

        /// <summary> Очистка буфера </summary>
        public void Clear()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    items[j,i] = BufItem.Empty;
        }

        [DllImport("kernel32.dll", SetLastError = true)] public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, CharacterAttributes wAttributes);
        [DllImport("kernel32.dll")] public static extern IntPtr GetStdHandle(int nStdHandle);

        public enum CharacterAttributes
        {
            FOREGROUND_BLUE = 0x0001,
            FOREGROUND_GREEN = 0x0002,
            FOREGROUND_RED = 0x0004,
            FOREGROUND_INTENSITY = 0x0008,
            BACKGROUND_BLUE = 0x0010,
            BACKGROUND_GREEN = 0x0020,
            BACKGROUND_RED = 0x0040,
            BACKGROUND_INTENSITY = 0x0080,
            COMMON_LVB_LEADING_BYTE = 0x0100,
            COMMON_LVB_TRAILING_BYTE = 0x0200,
            COMMON_LVB_GRID_HORIZONTAL = 0x0400,
            COMMON_LVB_GRID_LVERTICAL = 0x0800,
            COMMON_LVB_GRID_RVERTICAL = 0x1000,
            COMMON_LVB_REVERSE_VIDEO = 0x4000,
            COMMON_LVB_UNDERSCORE = 0x8000
        }

        /// <summary>
        /// Отобразить текущий буфер
        /// </summary>
        public void Show()
        {
            var visible=System.Console.CursorVisible;
            System.Console.CursorVisible=false;
            IntPtr hOut = GetStdHandle(-11); 

            for (int y = 0; y < Height; y++)
            {
                System.Console.SetCursorPosition(Left,Top + y);
                string line = null;
                for (int x = 0; x < Width; x++)
                {
                    SetConsoleTextAttribute(hOut, )
                    System.Console.ForegroundColor = items[x,y].IsBlinked? items[x,y].Foreground:items[x,y].AlternateForeground;
                    System.Console.BackgroundColor = items[x,y].IsBlinked? items[x,y].Background:items[x,y].AlternateBackground;
                
                    line += items[x, y].Char;
                }
                System.Console.Write(line);
            }
            System.Console.CursorVisible = visible;
        }

        /// <summary> Получить указанный прямоугольник </summary>
        /// <param name="rect"> прямоугольник </param>
        /// <returns> новый буффер с содержимым </returns>
        public Buffer Get(Rectangle rect)
        {
            Correct(ref rect);
            Buffer b = new Buffer(rect.Width, rect.Height){Location=rect.Location};
            for (int y = 0; y < rect.Height; y++)
                for (int x = 0; x < rect.Width; x++)
                    b[x, y] = items[rect.Left + x, rect.Top + y].Clone();
            return b;
        }

        /// <summary>
        /// Корректировка прямоугольника на возможность выхода за размеры буфера
        /// </summary>
        public Rectangle Correct(ref Rectangle rect)
        {
            int _x = rect.X, _y = rect.Y, _w = rect.Width, _h = rect.Height;
            if (_x < 0) _x = 0;
            if (_y < 0) _y = 0;
            if(_w>Width-_x) _w= Width-_x;
            if (_h > Height - _y) _h = Height - _y;
            return rect = new Rectangle(_x, _y, _w, _h);
        }

        /// <summary>
        /// Сохранение буфера в файл
        /// </summary>
        public void SaveTo(string fname)
        {
            using (var w = new BinaryWriter(new FileStream(fname, FileMode.Create)))
            {
                w.Write(Left);
                w.Write(Top);
                w.Write(Width);
                w.Write(Height);
                for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++) 
                        items[x,y].Save(w);
            }
        }

        /// <summary>
        /// Загрузка с файла в буфер
        /// </summary>
        public void LoadFrom(string fname)
        {
            if(File.Exists(fname))
                using (var r = new BinaryReader(new FileStream(fname, FileMode.Open)))
                {
                    Left = r.ReadInt32();
                    Top = r.ReadInt32();
                    Width = r.ReadInt32();
                    Height = r.ReadInt32();
                    items = new BufItem[Width, Height];
                    for (int y = 0; y < Height; y++)
                    for (int x = 0; x < Width; x++)
                        items[x, y] = new BufItem(r);
                }
        }
    }
}
