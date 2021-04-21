using System;

namespace DinoGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DinoGame())
                game.Run();
        }
    }
}
