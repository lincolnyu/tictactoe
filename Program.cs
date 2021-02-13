using System;
using System.Linq;

namespace tictactoe
{
    class Program
    {
        static void Main(string[] args)
        {
            // Stone[,] stones = new[,]
            // {
            //     {Stone.Black, Stone.White, Stone.Black},
            //     {Stone.Black, Stone.White, Stone.Black},
            //     {Stone.White, Stone.Black, Stone.White},
            // };
            // Console.WriteLine($"checktie = {TictactoeHelper.CheckTie(stones, 3)}");

            var te = new TictactoeEnumerator(3,3,3);
            var l = te.GenerateTree().ToArray();
            foreach(var t in l)
            {
                Console.Write(t);
                Console.WriteLine();
            }
            var f = l.First();
            Console.WriteLine(f.Verdict);

            // var l = ge.GenerateTree().ToArray();
            // foreach(var g in l)
            // {
            //     Console.Write(g);
            //     for (var i = 0; i < g.Indicdentals.Count; i++)
            //     {
            //         Console.Write($"{g.Indicdentals[i].Id}");
            //         if (i < g.Indicdentals.Count-1)
            //         {
            //             Console.Write(",");
            //         }
            //     }
            //     Console.WriteLine();
            //     Console.WriteLine();
            // }
        }
    }
}
