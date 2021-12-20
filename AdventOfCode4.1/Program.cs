var boardDimension = 5;
var boards = ReadBoards(@"C:\Users\Lars_\source\repos\AdventOfCode4.1\AdventOfCode4.1\boards.txt", boardDimension);
var numberDrawSequence = File.ReadLines(@"C:\Users\Lars_\source\repos\AdventOfCode4.1\AdventOfCode4.1\input.txt")
    .First()
    .Split(",")
    .Select(x => int.Parse(x))
    .ToList();
int? currentNumber = null;
var notDrawnNumbers = Flatten(GetWinnerBoard(boards, numberDrawSequence, true)).Where(x => !x.IsDrawnNumber).Select(x => x.Number);
Console.WriteLine(notDrawnNumbers.Sum() * currentNumber.Value);
var lastWinnerNotDrawnNumbers = Flatten(GetWinnerBoard(boards, numberDrawSequence, false)).Where(x => !x.IsDrawnNumber).Select(x => x.Number);
Console.WriteLine(lastWinnerNotDrawnNumbers.Sum() * currentNumber.Value);

List<BoardCell[,]> ReadBoards(string inputFilePath, int boardDimension)
{
    var boardInput = File.ReadAllLines(inputFilePath).ToList();
    var boards = new List<BoardCell[,]>();

    boardInput.RemoveAll(x => string.IsNullOrWhiteSpace(x));
    while (boardInput.Count > 0)
    {
        var singleBoardInput = boardInput.Take(boardDimension).ToList();
        var board = new BoardCell[boardDimension, boardDimension];
        for (int row = 0; row < boardDimension; row++)
        {
            for (int col = 0; col < boardDimension; col++)
            {
                board[row, col] = new BoardCell(int.Parse(singleBoardInput[row][(col * 3)..(col * 3 + 2)]));
            }
        }
        boardInput.RemoveRange(0, boardDimension);
        boards.Add(board);
    };
    return boards;
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

BoardCell[,] GetWinnerBoard(List<BoardCell[,]> boards, List<int> numberDrawSequence, bool getFirst)
{
    List<BoardCell[,]> winnerBoards = null;
    while (boards.Any())
    {
        currentNumber = numberDrawSequence.First();
        boards.ForEach(board =>
        {
            for (int row = 0; row < boardDimension; row++)
            {
                for (int col = 0; col < boardDimension; col++)
                {
                    if (board[row, col].Number == currentNumber)
                    {
                        board[row, col].IsDrawnNumber = true;
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

bool CheckForBingo(BoardCell[,] board)
{
    for (int diagonal = 0; diagonal < boardDimension; diagonal++)
    {
        for (int col = 0; col < boardDimension; col++)
        {
            if (!board[diagonal, col].IsDrawnNumber)
            {
                break;
            }

            if (col == 4)
            {
                return true;
            }
        }

        for (int row = 0; row < boardDimension; row++)
        {
            if (!board[row, diagonal].IsDrawnNumber)
            {
                break;
            }

            if (row == 4)
            {
                return true;
            }
        }
    }
    return false;
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