using System.Collections.Generic;
using static tictactoe.TictactoeSituation;
using static tictactoe.TictactoeHelper;

namespace tictactoe
{
    class TictactoeEnumerator
    {
        public int Rows {get;}
        public int Cols {get;}
        public int WinningLen {get;}
        public TictactoeEnumerator(int rows, int cols, int winningLen)
        {
            Rows = rows;
            Cols = cols;
            WinningLen = winningLen;
        }

        public IEnumerable<TictactoeSituation> GenerateTree()
        {
            var z = new TictactoeSituation(new Stone[Rows, Cols]);
            yield return z;
            var id = 0;
            z.Id = id++;
            var hashLevel = 1;
            var tshash = new HashSet<TictactoeSituation>();

            var q = new Queue<TictactoeSituation>();
            q.Enqueue(z);

            while (q.Count > 0)
            {
                var p = q.Dequeue();
                if (p.StoneCount+1 > hashLevel)
                {
                    tshash.Clear();
                    hashLevel = p.StoneCount+1;
                }
                var newItems = SetIncidentalsAndReturnNew(p, tshash);
                if (newItems != null)
                {
                    foreach(var newItem in newItems)
                    {
                        newItem.Id = id++;
                        q.Enqueue(newItem);
                        yield return newItem;
                    }
                }
                foreach (var c in p.Indicdentals)
                {
                    System.Diagnostics.Debug.Assert(c.Id != 0);
                }
            }
        }

        /// Returns new patterns
        public List<TictactoeSituation> SetIncidentalsAndReturnNew(TictactoeSituation ts, HashSet<TictactoeSituation> tsHash)
        {
            var newNodes = new List<TictactoeSituation>();
            var nextStone = ts.StoneCount % 2 == 0? Stone.Black : Stone.White;
            var curr = new HashSet<TictactoeSituation>();
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Cols; j++)
                {
                    if (ts.Stones[i,j] == Stone.None)
                    {
                        var n = ts.Stones.Copy();
                        if (CheckWin(n, i, j, nextStone, WinningLen))
                        {
                            ts.SetVerdict(nextStone==Stone.Black ? VerdictResult.BlackSecured : VerdictResult.WhiteSecured);
                            ts.Indicdentals.Clear();
                            foreach (var nn in newNodes)
                            {
                                tsHash.Remove(nn);
                            }
                            return null;
                        }
                        n[i,j] = nextStone;
                        if (CheckTie(n, WinningLen))
                        {
                            ts.SetVerdict(VerdictResult.TiePossible);
                            ts.Indicdentals.Clear();
                            foreach (var nn in newNodes)
                            {
                                tsHash.Remove(nn);
                            }
                            return null;
                        }
                        var tsIn = new TictactoeSituation(n);
                        if (!curr.Contains(tsIn))
                        {
                            if (!tsHash.TryGetValue(tsIn, out var tsOut))
                            {
                                tsHash.Add(tsIn);
                                newNodes.Add(tsIn);
                                tsOut = tsIn;
                            }
                            ts.AddIncidental(tsOut);
                            curr.Add(tsOut);
                        }
                    }
                }
            }
            return newNodes;
        }
    }
}
