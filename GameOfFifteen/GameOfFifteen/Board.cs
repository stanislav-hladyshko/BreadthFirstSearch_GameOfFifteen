using System;
using System.Collections.Generic;

namespace GameOfFifteen
{
    public class Board
    {
        private const int ROWS = 4, COLS = 4;
        public int[,] _cells { get; private set; } = new int[ROWS, COLS];
        private CellPosition _emptyCellPosition = null;
        private readonly static Board _solvedBoard = new Board();
        public string _lastMove { get; private set; } = null;

        public Board()
        {
            InitialiseDefaultBoardMethod();
        }

        public Board(string positions)
        {
            InitialiseUserBoardMethod(positions);
        }

        public Board(Board initBoard)
        {
            foreach (var position in GetAllTilePositions())
            {
                _cells[position.X, position.Y] = initBoard.GetTileValue(position);
            }

            _emptyCellPosition = initBoard._emptyCellPosition;
        }

        public override bool Equals(object obj)
        {
            foreach (var tilePosition in GetAllTilePositions())
            {
                if (GetTileValue(tilePosition) != ((Board)obj).GetTileValue(tilePosition))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (var tilePosition in GetAllTilePositions())
            {
                hashCode = (hashCode * ROWS * COLS) + GetTileValue(tilePosition);
            }
            return hashCode;
        }

        private void InitialiseDefaultBoardMethod()
        {
            int counter = 1;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    _cells[i, j] = counter;
                    counter++;
                }
            }
            _emptyCellPosition = new CellPosition(ROWS - 1, COLS - 1);
            _cells[_emptyCellPosition.X, _emptyCellPosition.Y] = 0;
        }

        private void InitialiseUserBoardMethod(string positions)
        {
            positions = positions.Replace(',', ' ');
            positions = positions.Replace("\r\n", " ");
            string[] userInputArray = positions.Split(' ');
            int counter = 0;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    _cells[i, j] = int.Parse(userInputArray[counter]);
                    if (userInputArray[counter] == "0")
                    {
                        _emptyCellPosition = new CellPosition(i, j);
                    }
                    counter++;
                }
            }
        }

        private List<CellPosition> GetAllTilePositions()
        {
            var allTilePostions = new List<CellPosition>();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    allTilePostions.Add(new CellPosition(i, j));
                }
            }
            return allTilePostions;
        }

        public int GetTileValue(CellPosition position)
        {
            return _cells[position.X, position.Y];
        }

        private void Move(CellPosition tilePosition)
        {
            if (!IsValidMove(tilePosition))
                throw new Exception("Move is not valid");

            _cells[_emptyCellPosition.X, _emptyCellPosition.Y] = _cells[tilePosition.X, tilePosition.Y];
            _cells[tilePosition.X, tilePosition.Y] = 0;
            _emptyCellPosition = tilePosition;
        }

        private Board MoveClone(CellPosition tilePosition)
        {
            var clonedBoard = new Board(this);

            if (_emptyCellPosition.X == tilePosition.X && _emptyCellPosition.Y < tilePosition.Y)
                clonedBoard._lastMove = GetTileValue(tilePosition) + ",←";

            if (_emptyCellPosition.X == tilePosition.X && _emptyCellPosition.Y > tilePosition.Y)
            {
                clonedBoard._lastMove = GetTileValue(tilePosition) + ",→";
            }
            if (_emptyCellPosition.X < tilePosition.X && _emptyCellPosition.Y == tilePosition.Y)
            {
                clonedBoard._lastMove = GetTileValue(tilePosition) + ",↑";
            }
            if (_emptyCellPosition.X > tilePosition.X && _emptyCellPosition.Y == tilePosition.Y)
            {
                clonedBoard._lastMove = GetTileValue(tilePosition) + ",↓";
            }

            clonedBoard.Move(tilePosition);

            return clonedBoard;
        }

        public void Shuffle(int times)
        {
            var random = new Random();
            for (int i = 0; i < times; i++)
            {
                List<CellPosition> possibleMoves = GetAllValidMoves();
                int which = random.Next(possibleMoves.Count);
                CellPosition move = possibleMoves[which];
                Move(move);
            }
        }

        private int NumberMisplacedTiles()
        {
            int wrong = 0;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    if ((_cells[i, j] > 0) && (_cells[i, j] != _solvedBoard._cells[i, j]))
                    {
                        wrong++;
                    }
                }
            }
            return wrong;
        }

        public bool IsSolved()
        {
            return NumberMisplacedTiles() == 0;
        }

        public List<Board> GetAllNeighborBoards()
        {
            var neighborBoards = new List<Board>();
            foreach (var move in GetAllValidMoves())
            {
                neighborBoards.Add(MoveClone(move));
            }
            return neighborBoards;
        }

        private List<CellPosition> GetAllValidMoves()
        {
            var allValidMoves = new List<CellPosition>();
            for (int dx = -1; dx < 2; dx++)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    var moveToPosition = new CellPosition(_emptyCellPosition.X + dx, _emptyCellPosition.Y + dy);
                    if (IsValidMove(moveToPosition))
                    {
                        allValidMoves.Add(moveToPosition);
                    }
                }
            }
            return allValidMoves;
        }

        private bool IsValidMove(CellPosition position)
        {
            if ((position.X < 0) || (position.X >= COLS))
            {
                return false;
            }
            if ((position.Y < 0) || (position.Y >= ROWS))
            {
                return false;
            }
            int dx = _emptyCellPosition.X - position.X;
            int dy = _emptyCellPosition.Y - position.Y;
            if ((Math.Abs(dx) + Math.Abs(dy) != 1) || (dx * dy != 0))
            {
                return false;
            }
            return true;
        }
    }
}
