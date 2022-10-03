using System;

namespace LightsOutWPF
{
    public class LightsOutGame
    {
        public LightsOutLevels CurrentLevel { get; set; }
        public bool[,] LightsOnGrid { get; set; }

        public LightsOutGame(LightsOutLevels level)
        {
            CurrentLevel = level;
            LightsOnGrid = new bool[CurrentLevel.Rows, CurrentLevel.Columns];
            NewGame();
        }

        public bool GetGameValue(int row, int column)
        {
            return LightsOnGrid[row, column];
        }

        public void NewGame()
        {
            for (int row = 0; row < CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < CurrentLevel.Columns; column++)
                {
                    LightsOnGrid[row, column] = CurrentLevel.On.Contains(CurrentLevel.Columns * row + column);
                }
            }
        }

        public void ProcessLightSwitch(int rowClicked, int columnClicked)
        {
            if (rowClicked < 0 || rowClicked >= CurrentLevel.Rows || columnClicked < 0 || columnClicked >= CurrentLevel.Columns)
                throw new ArgumentException($"Row or Column clicked is outside the range [{CurrentLevel.Rows},{CurrentLevel.Columns}]");

            // Flip top light.
            if (rowClicked - 1 >= 0 && rowClicked - 1 < CurrentLevel.Rows)
                LightsOnGrid[rowClicked - 1, columnClicked] = !LightsOnGrid[rowClicked - 1, columnClicked];

            // Flip right light.
            if (columnClicked + 1 >= 0 && columnClicked + 1 < CurrentLevel.Columns)
                LightsOnGrid[rowClicked, columnClicked + 1] = !LightsOnGrid[rowClicked, columnClicked + 1];

            // Flip bottom light.
            if (rowClicked + 1 >= 0 && rowClicked + 1 < CurrentLevel.Rows)
                LightsOnGrid[rowClicked + 1, columnClicked] = !LightsOnGrid[rowClicked + 1, columnClicked];

            // Flip left light.
            if (columnClicked - 1 >= 0 && columnClicked - 1 < CurrentLevel.Columns)
                LightsOnGrid[rowClicked, columnClicked - 1] = !LightsOnGrid[rowClicked, columnClicked - 1];

            // Flip current clickede light.
            LightsOnGrid[rowClicked, columnClicked] = !LightsOnGrid[rowClicked, columnClicked];
        }

        public bool IsGameOver()
        {
            for (int row = 0; row < CurrentLevel.Rows; row++)
            {
                for (int column = 0; column < CurrentLevel.Columns; column++)
                {
                    if (LightsOnGrid[row, column])
                        return false;
                }
            }
            return true;
        }
    }
}
