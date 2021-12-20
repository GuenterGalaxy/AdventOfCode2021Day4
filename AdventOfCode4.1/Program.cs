var boards = ReadBoards(@"C:\Users\Lars_\source\repos\AdventOfCode4.1\AdventOfCode4.1\boards.txt", 5);
var numberDrawSequence = File.ReadLines(@"C:\Users\Lars_\source\repos\AdventOfCode4.1\AdventOfCode4.1\input.txt")
    .First()
    .Split(",")
    .Select(x => int.Parse(x))
    .ToList();
int? currentNumber = null;
var boardDimension = 5;
var notDrawnNumbers = Flatten(GetWinnerBoard(boards, numberDrawSequence, true)).Where(x => !x.IsDrawnNumber).Select(x => x.Number);
Console.WriteLine(notDrawnNumbers.Sum() * currentNumber.Value);
var lastWinnerNotDrawnNumbers = Flatten(GetWinnerBoard(boards, numberDrawSequence, false)).Where(x => !x.IsDrawnNumber).Select(x => x.Number);
Console.WriteLine(lastWinnerNotDrawnNumbers.Sum() * currentNumber.Value);

BoardCell[,] GetWinnerBoard(List<BoardCell[,]> boards, List<int> numberDrawSequence, bool getFirst)
{
    List<BoardCell[,]> winnerBoards = null;
    while (boards.Any())
    {
        currentNumber = numberDrawSequence.First();
        boards.ForEach(board =>
        {
            for (int verticalI = 0; verticalI < boardDimension; verticalI++)
            {
                for (int horizontalI = 0; horizontalI < boardDimension; horizontalI++)
                {
                    if (board[verticalI, horizontalI].Number == currentNumber)
                    {
                        board[verticalI, horizontalI].IsDrawnNumber = true;
                    }
                }
            }
        });

        winnerBoards = boards.Where(board => CheckForBingo(board)).ToList();

        if (getFirst && winnerBoards.Any() ||
            (!getFirst && boards.Count == 1 && winnerBoards.Count == 1))
        {
            return winnerBoards.First();
        }

        boards = boards.Except(winnerBoards).ToList();
        numberDrawSequence.Remove(currentNumber.Value);
    }
    return null;
}

List<BoardCell[,]> ReadBoards(string inputFilePath, int boardDimension)
{
    var boardInput = File.ReadAllLines(inputFilePath).ToList();
    var boards = new List<BoardCell[,]>();

    boardInput.RemoveAll(x => string.IsNullOrWhiteSpace(x));
    while (boardInput.Count > 0)
    {
        var singleBoardInput = boardInput.Take(boardDimension).ToList();
        var board = new BoardCell[boardDimension, boardDimension];
        for (int verticalI = 0; verticalI < boardDimension; verticalI++)
        {
            for (int horizontalI = 0; horizontalI < boardDimension; horizontalI++)
            {
                board[verticalI, horizontalI] = new BoardCell(int.Parse(singleBoardInput[verticalI][(horizontalI * 3)..(horizontalI * 3 + 2)]));
            }
        }
        boardInput.RemoveRange(0, boardDimension);
        boards.Add(board);
    };
    return boards;
}

bool CheckForBingo(BoardCell[,] board)
{
    for (int diagonalI = 0; diagonalI < boardDimension; diagonalI++)
    {
        for (int horizontalI = 0; horizontalI < boardDimension; horizontalI++)
        {
            if (!board[diagonalI, horizontalI].IsDrawnNumber)
            {
                break;
            }

            if (horizontalI == 4)
            {
                return true;
            }
        }

        for (int verticalI = 0; verticalI < boardDimension; verticalI++)
        {
            if (!board[verticalI, diagonalI].IsDrawnNumber)
            {
                break;
            }

            if (verticalI == 4)
            {
                return true;
            }
        }
    }
    return false;
}

IEnumerable<T> Flatten<T>(T[,] map)
{
    for (int row = 0; row < map.GetLength(0); row++)
    {
        for (int col = 0; col < map.GetLength(1); col++)
        {
            yield return map[row, col];
        }
    }
}

public class BoardCell
{
    public BoardCell(int number)
    {
        Number = number;
        IsDrawnNumber = false;
    }

    public int Number { get; set; }

    public bool IsDrawnNumber { get; set; }
}