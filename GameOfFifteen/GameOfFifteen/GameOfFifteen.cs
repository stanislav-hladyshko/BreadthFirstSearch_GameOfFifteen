using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace GameOfFifteen
{
    static class GameOfFifteen
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Write("Do you want to load your own board?(y/n): ");
            var choice = ViewClass.StringInputValidator();

            ViewClass.Choice(choice);

            Console.ReadKey();
        }

        public static void Shuffler(Board board)
        {
            string ResultToFile = "";
            int rows = 4, cols = 4;
            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < cols; k++)
                {
                    CellPosition textWtriter = new CellPosition(i, k);
                    ResultToFile += Convert.ToString(board.GetTileValue(textWtriter));
                    if (k < cols - 1)
                        ResultToFile += ",";
                }
                ResultToFile += "\n";
            }
            File.WriteAllText(ViewClass._shuffledBoardPath, ResultToFile);
        }

        public static void Solver(List<Board> solution, bool playerMode = false, int timeout = 0)
        {
            if (solution == null)
            {
                Console.WriteLine("Did not solve. :(");
                return;
            }
            if (playerMode)
                SolveWithPlayerMode(solution, timeout);
            else
                Solve(solution);
        }

        private static void SolveWithPlayerMode(List<Board> solution, int timeout)
        {
            foreach (Board board in solution)
            {
                Thread.Sleep(timeout);
                board.Show();
            }
            Console.WriteLine("Done!");
            Thread.Sleep(2000);
        }

        private static void Solve(List<Board> solution)
        {
            solution.Reverse();
            string solutionResult = "";
            foreach (Board board in solution)
            {
                if (board._lastMove != null)
                {
                    solutionResult += board._lastMove;
                    solutionResult += "\n";
                }
            }
            File.WriteAllText(ViewClass._solvedBoardPath, solutionResult);
        }

        public static List<Board> BreadthFirstSearch(Board boardToSolve)
        {
            var toVisit = new Queue<Board>(); 
            var predecessor = new Dictionary<Board, Board>(); 
            toVisit.Enqueue(boardToSolve);
            predecessor[boardToSolve] = null; 
            int counter = 0;
            while (toVisit.Count > 0)
            {
                Board candidate = toVisit.Dequeue(); 
                counter++;
                if (candidate.IsSolved())
                {
                    var solution = new List<Board>();
                    Board backtrace = candidate;
                    while (backtrace != null)
                    {
                        solution.Add(backtrace);
                        backtrace = predecessor[backtrace];
                    }
                    return solution;
                }
                foreach (Board board in candidate.GetAllNeighborBoards())
                {
                    if (!predecessor.ContainsKey(board))
                    {
                        predecessor[board] = candidate;
                        toVisit.Enqueue(board);
                    }
                }
            }
            return null;
        }

    }
}
