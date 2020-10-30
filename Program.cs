using System;
using System.Diagnostics;

namespace PokeMan
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var game = new PokeManGame())
                game.Run();
        }
    }
}