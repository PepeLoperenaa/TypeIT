using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Utilities
{
    class KeyStrokeValidater
    {
        public static void KeyStrokeExample()
        {
            ConsoleKeyInfo keyInfo;
            StringBuilder stringBuilder = new StringBuilder();
            do
            {
                keyInfo = Console.ReadKey();

                switch (keyInfo.Key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.B:
                    case ConsoleKey.C:
                    case ConsoleKey.D:
                    case ConsoleKey.E:
                    case ConsoleKey.F:
                    case ConsoleKey.G:
                    case ConsoleKey.H:
                    case ConsoleKey.I:
                    case ConsoleKey.J:
                    case ConsoleKey.K:
                    case ConsoleKey.L:
                    case ConsoleKey.M:
                    case ConsoleKey.N:
                    case ConsoleKey.O:
                    case ConsoleKey.P:
                    case ConsoleKey.Q:
                    case ConsoleKey.R:
                    case ConsoleKey.S:
                    case ConsoleKey.T:
                    case ConsoleKey.U:
                    case ConsoleKey.V:
                    case ConsoleKey.W:
                    case ConsoleKey.X:
                    case ConsoleKey.Y:
                    case ConsoleKey.Z:
                    case ConsoleKey.OemPeriod:
                    case ConsoleKey.OemComma:
                    case ConsoleKey.Spacebar:
                        {
                            stringBuilder.Append(keyInfo.Key);
                            break;
                        }
                    case ConsoleKey.Backspace:
                        {
                            stringBuilder.Clear();
                            break;
                        }
                }
            } while (keyInfo.Key != ConsoleKey.Escape);
        }
    }
}
