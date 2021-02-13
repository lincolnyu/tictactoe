using System.Collections.Generic;
using System.Text;
using static tictactoe.TictactoeSituation;
using static tictactoe.TictactoeHelper;

namespace tictactoe
{
    static class TictactoeHelper
    {
        public static bool CheckWin(Stone[,] stones, int i, int j, Stone stone, int winningLen)
        {
            var c = 1;
            for (var jj = j+1; jj < stones.GetLength(1); jj++)
            {
                if (stones[i,jj] == stone)
                {
                    c++;
                }
            }
            for (var jj = j-1; jj >= 0; jj--)
            {
                if (stones[i,jj] == stone)
                {
                    c++;
                }
            }
            if (c >= winningLen)
            {
                return true;
            }

            c = 1;
            for (var ii = i+1; ii < stones.GetLength(0); ii++)
            {
                if (stones[ii, j] == stone)
                {
                    c++;
                }
            }
            for (var ii = i-1; ii >= 0; ii--)
            {
                if (stones[ii,j] == stone)
                {
                    c++;
                }
            }
            if (c >= winningLen)
            {
                return true;
            }

            c = 1;
            for (var k = 1; i+k<stones.GetLength(0) && j+k<stones.GetLength(1); k++)
            {
                if (stones[i+k,j+k] == stone)
                {
                    c++;
                }
            }
            for (var k = 1; i-k>=0 && j-k>=0; k++)
            {
                if (stones[i-k,j-k] == stone)
                {
                    c++;
                }
            }
            if (c >= winningLen)
            {
                return true;
            }

            c = 1;
            for (var k = 1; i-k>=0 && j+k < stones.GetLength(1); k++)
            {
                if (stones[i-k,j+k] == stone)
                {
                    c++;
                }
            }
            for (var k = 1; i+k<stones.GetLength(0) && j-k>=0; k++)
            {
                if (stones[i+k,j-k] == stone)
                {
                    c++;
                }
            }
            if (c >= winningLen)
            {
                return true;
            }

            return false;
        }

        enum TypeCheckResult
        {
            NeitherWillWin,
            BlackMightWin,
            WhiteMightWin,
            EitherMightWin
        }

        private static TypeCheckResult TieCheck(IEnumerable<Stone> stones, int winningLen)
        {
            var currMaxWhite = 0;
            var currMaxBlack = 0;
            bool whiteMightWin = false;
            bool blackMightWin = false;
            foreach (var stone in stones)
            {
                if (stone == Stone.None)
                {
                    currMaxWhite++;
                    currMaxBlack++;
                }
                else if (stone == Stone.Black)
                {
                    currMaxBlack++;
                    currMaxWhite = 0;
                }
                else if (stone == Stone.White)
                {
                    currMaxWhite++;
                    currMaxBlack = 0;
                }
                if (currMaxBlack >= winningLen)
                {
                    blackMightWin = true;
                }
                if (currMaxWhite >= winningLen)
                {
                    whiteMightWin = true;
                }
                if (blackMightWin && whiteMightWin)
                {
                    return TypeCheckResult.EitherMightWin;
                }
            }
            if (blackMightWin)
            {
                return TypeCheckResult.BlackMightWin;
            }
            if (whiteMightWin)
            {
                return TypeCheckResult.WhiteMightWin;
            }
            return TypeCheckResult.NeitherWillWin;
        }

        private static IEnumerable<Stone> YieldRow(Stone[,] stones, int row)
        {
            for (var j = 0; j < stones.GetLength(1); j++)
            {
                yield return stones[row,j];
            }
        }

        private static IEnumerable<Stone> YieldCol(Stone[,] stones, int col)
        {
            for (var i = 0; i < stones.GetLength(0); i++)
            {
                yield return stones[i,col];
            }
        }

        private static IEnumerable<Stone> YieldDiagDown(Stone[,] stones, int i, int j)
        {
            for (var k = 0; i+k < stones.GetLength(0) && j+k < stones.GetLength(1); k++)
            {
                yield return stones[i+k,j+k];
            }
        }

        private static IEnumerable<Stone> YieldDiagUp(Stone[,] stones, int i, int j)
        {
            for (var k = 0; i>=k && j+k < stones.GetLength(1); k++)
            {
                yield return stones[i-k,j+k];
            }
        }

        public static bool CheckTie(Stone[,] stones, int winningLen)
        {
            for (var row = 0; row < stones.GetLength(0); row++)
            {
                if (TieCheck(YieldRow(stones, row), winningLen) != TypeCheckResult.NeitherWillWin)
                {
                    return false;
                }
            }
            for (var col = 0; col < stones.GetLength(1); col++)
            {
                if (TieCheck(YieldCol(stones, col), winningLen) != TypeCheckResult.NeitherWillWin)
                {
                    return false;
                }
            }
            if (TieCheck(YieldDiagDown(stones, 0, 0), winningLen) != TypeCheckResult.NeitherWillWin
                || TieCheck(YieldDiagUp(stones, stones.GetLength(0)-1, 0), winningLen) != TypeCheckResult.NeitherWillWin)
            {
                return false;
            }
            for (var k = 1; k <= stones.GetLength(1) - winningLen; k++)
            {
                if (TieCheck(YieldDiagDown(stones, 0, k), winningLen) != TypeCheckResult.NeitherWillWin
                    || TieCheck(YieldDiagUp(stones, stones.GetLength(0)-1, k), winningLen) != TypeCheckResult.NeitherWillWin)
                {
                    return false;
                }
            }
            for (var k = 1; k <= stones.GetLength(0) - winningLen; k++)
            {
                if (TieCheck(YieldDiagDown(stones, k, 0), winningLen) != TypeCheckResult.NeitherWillWin
                    || TieCheck(YieldDiagUp(stones, stones.GetLength(0)-1-k, 0), winningLen) != TypeCheckResult.NeitherWillWin)
                {
                    return false;
                }
            }
            return true;
        }

        static void Swap(ref Stone a, ref Stone b)
        {
            var t = a;
            a = b;
            b = t;
        }
        public static void MirrorHorizontal(this Stone[,] input)
        {
            var width = input.GetLength(1);
            for (var j = 0; j < width/2; j++)
            {
                for (var i = 0; i < input.GetLength(0); i++)
                {
                    Swap(ref input[i,j], ref input[i, width-1-j]);
                }
            }
        }
        public static void MirrorVertical(this Stone[,] input)
        {
            var height = input.GetLength(0);
            for (var i = 0; i < height/2; i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    Swap(ref input[i,j], ref input[height-1-i,j]);
                }
            }
        }
        public static void Transpose(this Stone[,] input)
        {
            for (var i = 1; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < i; j++)
                {
                    Swap(ref input[i,j], ref input[j,i]);
                }
            }
        }

        public static bool AreEqual(this Stone[,] a, Stone[,] b)
        {
            for (var i = 0; i < a.GetLength(0); i++)
            {
                for (var j = 0; j < b.GetLength(1); j++)
                {
                    if (a[i,j] != b[i,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool LessThan(this Stone[,] a, Stone[,] b)
        {
            bool? aHasWhiteFirst = null;
            for (var i = 0; i < a.GetLength(0); i++)
            {
                for (var j = 0; j < b.GetLength(1); j++)
                {
                    if (a[i,j] == Stone.Black && b[i,j] != Stone.Black)
                    {
                        return true;
                    }
                    if (b[i,j] == Stone.Black && a[i,j] != Stone.Black)
                    {
                        return false;
                    }
                    if (!aHasWhiteFirst.HasValue && a[i,j] != b[i,j])
                    {
                        // NOTE Neither A nor B is black
                        //      And one has to be white and the other none
                        if (a[i,j] == Stone.White)
                        {
                            aHasWhiteFirst = true;
                        }
                        else
                        {
                            aHasWhiteFirst = false;
                        }
                    }
                }
            }
            return aHasWhiteFirst == true;
        }

        public static Stone[,] Copy(this Stone[,] s)
        {
            var copy = new Stone[s.GetLength(0), s.GetLength(1)];
            for (var i = 0; i < s.GetLength(0); i++)
            {
                for (var j = 0; j < s.GetLength(1); j++)
                {
                    copy[i,j] = s[i,j];
                }
            }
            return copy;
        }

        public static void Normalize(ref Stone[,] t)
        {
            // original
            // 12
            // 34
            var mincfg = t.Copy();

            // mirror-hori
            // 21
            // 43
            t.MirrorHorizontal();
            if (t.LessThan(mincfg))
            {
                mincfg = t.Copy();
            }

            // mirror-verti
            // 43
            // 21
            t.MirrorVertical();
            if (t.LessThan(mincfg))
            {
                mincfg = t.Copy();
            }

            // mirror-hori
            // 34
            // 12
            t.MirrorHorizontal();
            if (t.LessThan(mincfg))
            {
                mincfg = t.Copy();
            }
            
            if (t.GetLength(0) == t.GetLength(1))
            {
                // transpose
                // 31
                // 42
                t.Transpose();
                if (t.LessThan(mincfg))
                {
                    mincfg = t.Copy();
                }

                // mirror-hori
                // 13
                // 24
                t.MirrorHorizontal();
                if (t.LessThan(mincfg))
                {
                    mincfg = t.Copy();
                }

                // mirror-verti
                // 24
                // 13
                t.MirrorVertical();
                if (t.LessThan(mincfg))
                {
                    mincfg = t.Copy();
                }

                // mirror-hori
                // 42
                // 31
                t.MirrorHorizontal();
                if (t.LessThan(mincfg))
                {
                    mincfg = t.Copy();
                }
            }

            t = mincfg;
        }
    }
    class TictactoeSituation
    {
        public enum Stone 
        {
            None,
            Black,
            White
        }

        public enum VerdictResult
        {
            Unverdicted,
            BlackSecured,
            WhiteSecured,
            TiePossible
        }

        public Stone[,] Stones {get; private set;}

        public List<TictactoeSituation> Parents {get;} = new List<TictactoeSituation>();
        public List<TictactoeSituation> Indicdentals {get;} = new List<TictactoeSituation>();
        public int StoneCount{get; private set;} = 0;
        public int Id {get; set;}
        private int _hash;

        public VerdictResult Verdict {get; set;} = VerdictResult.Unverdicted;
        public int IncidentalsVerdicted {get; private set;} = 0;

        public override string ToString()
        {
            var sbRes = new StringBuilder();
            sbRes.Append($"[{Id}]{Verdict}{{");
            foreach (var c in Indicdentals)
            {
                sbRes.Append($"{c.Id},");
            }
            if (Indicdentals.Count > 0)
            {
                sbRes.Remove(sbRes.Length-1,1);
            }
            sbRes.AppendLine("}");
            for (var i = 0; i < Stones.GetLength(0); i++)
            {
                for (var j = 0; j < Stones.GetLength(1); j++)
                {
                    switch (Stones[i,j])
                    {
                        case Stone.Black:
                            sbRes.Append('X');
                            break;
                        case Stone.White:
                            sbRes.Append('O');
                            break;
                        default:
                            sbRes.Append(' ');
                            break;
                    }
                    if (j < Stones.GetLength(0)-1)
                    {
                        sbRes.Append('|');
                    }
                }
                if (i < Stones.GetLength(0)-1)
                {
                    sbRes.AppendLine();
                    sbRes.Append('-', Stones.GetLength(1)*2-1);
                    sbRes.AppendLine();
                }
            }
            return sbRes.ToString();
        }

        public TictactoeSituation(Stone[,] stones)
        {
            this.Stones = stones;
            StoneCount = 0;
            foreach (var s in stones)
            {
                if (s != Stone.None)
                {
                    StoneCount++;
                }
            }
            stones = Stones;
            Normalize(ref stones);
            Stones = stones;
            CalcHash();
        }

        public override int GetHashCode() => _hash;

        public void CalcHash()
        {
            var c = 0;
            foreach (var v in Stones)
            {
                if (c >= StoneCount)
                {
                    break;
                }
                switch (v)
                {
                    case Stone.Black:
                        c *= 3;
                        c += 1;
                        break;
                    case Stone.White:
                        c *= 3;
                        c += 1;
                        break;
                    default:
                        c *= 3;
                        break;
                }
            }
            _hash = c;
        }

        public override bool Equals(object obj)
        {
            var p = obj as TictactoeSituation;
            if (p != null)
            {
                if (StoneCount != p.StoneCount)
                {
                    return false;
                }
                return this.Stones.AreEqual(p.Stones);
            }
            return false;
        }

        public void SetVerdict(VerdictResult verdict)
        {
            Verdict = verdict;
            foreach (var parent in Parents)
            {
                if (parent.Verdict != VerdictResult.Unverdicted)
                {
                    continue;
                }
                var parentMover = parent.StoneCount%2==0? Stone.Black : Stone.White;
                if (parentMover == Stone.Black && verdict == VerdictResult.BlackSecured)
                {
                    parent.SetVerdict(VerdictResult.BlackSecured);
                }
                else if (parentMover == Stone.White && verdict == VerdictResult.WhiteSecured)
                {
                    parent.SetVerdict(VerdictResult.WhiteSecured);
                }
                else
                {
                    parent.IncidentalsVerdicted++;
                    if (parent.IncidentalsVerdicted >= parent.Indicdentals.Count)
                    {
                        if (verdict == VerdictResult.TiePossible)
                        {
                            parent.SetVerdict(verdict);
                        }
                        else
                        {
                            foreach(var c in parent.Indicdentals)
                            {
                                System.Diagnostics.Debug.Assert(
                                    !(parentMover == Stone.Black && c.Verdict == VerdictResult.BlackSecured)
                                );
                                System.Diagnostics.Debug.Assert(
                                    !(parentMover == Stone.White && c.Verdict == VerdictResult.WhiteSecured)
                                );
                                if (c.Verdict != verdict)
                                {
                                    System.Diagnostics.Debug.Assert(c.Verdict == VerdictResult.TiePossible);
                                    parent.SetVerdict(VerdictResult.TiePossible);
                                    break;
                                }
                            }
                            if (parent.Verdict == VerdictResult.Unverdicted)
                            {
                                //All incidentals verdicted the same
                                parent.SetVerdict(verdict);
                            }
                        }
                    }
                }
            }
        }

        public void AddIncidental(TictactoeSituation ts)
        {
            Indicdentals.Add(ts);
            ts.Parents.Add(this);
        }
    }
}
