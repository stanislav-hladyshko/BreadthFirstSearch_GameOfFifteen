using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GameOfFifteen
{
    public static class ViewClass
    {
        public const string _shuffledBoardPath = "c:\\Users\\Admin\\Desktop\\Start_Position.txt";
        public const string _solvedBoardPath = "c:\\Users\\Admin\\Desktop\\Solved_moves.txt";
        public const string _userStartPossition = "c:\\Users\\Admin\\Desktop\\UserStartPossition.txt";
        public static int _timeOut = 0;

        public static bool StringInputValidator()
        {
            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                if (key.Key == ConsoleKey.Y)
                    return true;
                if (key.Key == ConsoleKey.N)
                    return false;
                if (key.Key == ConsoleKey.Escape)
                    Environment.Exit(0);
                else
                    Console.Write("Invalid input, please write correct input: ");
            }
        }

        public static void Choice(bool choice)
        {
            if (choice)
                LoadOwnGame();
            else
                LoadNewGame();
        }

        private static void LoadNewGame()
        {
            Console.WriteLine("Starting of default game: ");
            var board = new Board();
            Console.Write("How much time do you want to shuffle thees desk: ");
            board.Shuffle(IntInputValidator());
            GameOfFifteen.Shuffler(board);
            List<Board> solution = GameOfFifteen.BreadthFirstSearch(board);
            GameOfFifteen.Solver(solution);
            Console.WriteLine($"Your mixed board is ready, it saved here: {_shuffledBoardPath}");
            Console.WriteLine($"Your solving way is ready, it saved here: {_solvedBoardPath}");
            Console.Write("Do you want to implement \"Player\" mode? (y/n): ");
            var choice = StringInputValidator();
            if (!choice)
            {
                Console.WriteLine("Exit game");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
            Console.Write("What timeout do you prefer?(sec): ");
            _timeOut = GetTimeoutMiliseconds(FloatInputValidator());
            GameOfFifteen.Solver(solution, true, _timeOut);
        }

        private static void LoadOwnGame()
        {
            var userBoard = new Board(File.ReadAllText(_userStartPossition));
            List<Board> userSolution = GameOfFifteen.BreadthFirstSearch(userBoard);
            GameOfFifteen.Shuffler(userBoard);
            GameOfFifteen.Solver(userSolution);
            Console.WriteLine($"Your solving way is ready, it saved here: {_solvedBoardPath}");
            Console.Read();
        }

        private static int GetTimeoutMiliseconds(float input)
        {
            return (int)(input * 1000);
        }

        private static float FloatInputValidator()
        {
            if (float.TryParse(Console.ReadLine(), out var result))
                return result;
            else
            {
                Console.Write("Invalid input, please write correct input: ");
                return FloatInputValidator();
            }
        }

        private static int IntInputValidator()
        {
            if (int.TryParse(Console.ReadLine(), out var result))
                return result;
            else
            {
                Console.Write("Invalid input, please write correct input: ");
                return IntInputValidator();
            }
        }

        public static void Show(this Board board)
        {
            Console.WriteLine("-----------------");
            if (board._lastMove == null)
            {
                Console.Write("Start board combination:");
            }
            Console.WriteLine(board._lastMove);
            int rows = 4, cols = 4;
            for (int i = 0; i < rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < cols; j++)
                {
                    int n = board._cells[i, j];
                    string s;
                    if (n > 0)
                    {
                        s = n.ToString();
                    }
                    else
                    {
                        s = "";
                    }
                    while (s.Length < 2)
                    {
                        s += " ";
                    }
                    Console.Write(s + "| ");
                }
                Console.Write("\n");
            }
            Console.Write("-----------------\n\n");
        }
    }
}
