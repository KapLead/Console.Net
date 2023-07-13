using System.Linq;

namespace Console.Net
{
    public class ConsoleCore
    {
        /// <summary> Ожидание ввода с клавиатуры символа (любого символа) </summary>
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
