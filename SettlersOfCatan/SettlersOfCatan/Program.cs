/* This is the main program that starts and loops.  This is what starts all the magic
 * 
 */
using System;

namespace SettlersOfCatan
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SettlersOfCatan game = new SettlersOfCatan())
            {
                game.Run();
            }
        }
    }
#endif
}

