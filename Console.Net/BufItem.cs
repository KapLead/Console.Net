using System;
using System.IO;

namespace Console.Net
{
    public class BufItem
    {
        public static BufItem Empty = new BufItem();
        private bool _blink;

        public char Char { get; set; } = ' ';
        public ConsoleColor Foreground { get; set; } = ConsoleColor.Black;
        public ConsoleColor Background { get; set; } = ConsoleColor.White;
        public bool IsBlinked { get; set; }
        public bool Blink
        {
            get => _blink;
            set
            {
                _blink = value;
                if (value)
                {
                    if (AlternateForeground == ConsoleColor.White)
                        AlternateForeground = Background;
                    if (AlternateBackground == ConsoleColor.Black)
                        AlternateBackground = Foreground;
                }
            }
        }
        public ConsoleColor AlternateForeground { get; set; } = ConsoleColor.White;
        public ConsoleColor AlternateBackground { get; set; } = ConsoleColor.Black;

        public BufItem() { }
        public BufItem(char ch)
        {
            Char = ch;
        }
        public BufItem(BinaryReader r)
        {
            Char = r.ReadChar();
            Foreground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), r.ReadString());
            Background = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), r.ReadString());
            IsBlinked = r.ReadBoolean();
            Blink = r.ReadBoolean();
            AlternateForeground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), r.ReadString());
            AlternateBackground = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), r.ReadString());
        }
        public BufItem Set(ConsoleColor f, ConsoleColor b)
        {
            Foreground = f;
            Background = b;
            return this;
        }
        public BufItem Alternate(ConsoleColor f, ConsoleColor b)
        {
            AlternateForeground = f;
            AlternateBackground = b;
            return this;
        }
        public BufItem Clone()
        {
            return new BufItem(Char)
            {
                Foreground = Foreground,
                Background = Background,
                IsBlinked = IsBlinked,
                Blink = Blink,
                AlternateForeground = AlternateForeground,
                AlternateBackground = AlternateBackground
            };
        }
        public void Save(BinaryWriter w)
        {
            w.Write(Char);
            w.Write(Foreground.ToString());
            w.Write(Background.ToString());
            w.Write(IsBlinked);
            w.Write(Blink);
            w.Write(AlternateForeground.ToString());
            w.Write(AlternateBackground.ToString());
        }

    }
}
