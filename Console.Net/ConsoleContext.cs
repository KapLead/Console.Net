namespace Console.Net
{
    public class ConsoleContext : ConsoleBuffer
    {
        private static Buffer _origin;

        public static Buffer Origin
        {
            get
            {
                if (_origin == null)
                {
                    _origin = new Buffer(Width, Height);
                    _origin.Clear();
                }
                return _origin;
            }
            set => _origin = value;
        }
    }
}
