using System;
using System.Diagnostics;
using System.Threading;

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