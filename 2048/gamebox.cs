using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Threading.Tasks;
using static _2048.gamegrid;

namespace _2048


{
    public class gamegrid
    {
        private gamebox[,] grid = new gamebox[4, 4];
        Dictionary<Tuple<int, int>, bool> emptyPositions = new Dictionary<Tuple<int, int>, bool>();

        public gamebox this[int row, int col]
        {
            get => grid[row, col];
            set => grid[row, col] = value;
        }

        public void Clear()
        {
            grid = new gamebox[4, 4];
            emptyPositions.Clear();
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    emptyPositions.Add(new Tuple<int, int>(row, col), true);
                }
            }
        }

        public Tuple<int, int> GetRandomEmptyPosition()
        {
            if (emptyPositions.Count == 0)
            {
                return null;
            }
            int randomIndex = new Random().Next(0, emptyPositions.Count);
            return emptyPositions.Keys.ElementAt(randomIndex);
        }


        public void generateNewBox()
        {
            List<int> options = new List<int>() { 2 };
            
            Random rand = new Random();
            int optindex = rand.Next(options.Count);
            
            Tuple<int, int> emptyPosition = GetRandomEmptyPosition();
            if (emptyPosition != null)
            {
                int row = emptyPosition.Item1;
                int col = emptyPosition.Item2;
                this.AddGameBox(row, col, options[optindex]);

            }
            
        }


        public void AddGameBox(int row, int col, int value)
        {
            gamebox newbox = new gamebox();
            newbox.Value = value;
            this[row, col] = newbox;
            emptyPositions.Remove(new Tuple<int, int>(row, col));
        }

        public void RemoveGameBox(int row, int col)
        {
            this[row, col] = null;
            emptyPositions.Add(new Tuple<int, int>(row, col), true);
        }
        public void MoveGameBox(int oldrow, int oldcol, int newrow, int newcol)
        {
            int value = this[oldrow, oldcol].Value;
            this[newrow, newcol] = this[oldrow, oldcol];
            this.RemoveGameBox(oldrow, oldcol);
            this.AddGameBox(newrow, newcol, value);
        }


        public void PushBoxes(Direction direction)
        {
            bool[,] merged = new bool[4, 4]; // keep track of boxes that have already been merged

            switch (direction)
            {
                case Direction.Left:
                    for (int row = 0; row < 4; row++)
                    {
                        for (int col = 1; col < 4; col++)
                        {
                            if (this[row, col] == null)
                            {
                                continue;
                            }
                            // move box as far left as possible
                            int targetCol = col;
                            while (targetCol > 0 && this[row, targetCol - 1] == null)
                            {
                                targetCol--;
                            }
                            if (targetCol != col)
                            {
                                
                                this.MoveGameBox(row, col, row, targetCol);
                                col = targetCol; // skip the rest of this row
                            }

                            // merge with adjacent box if possible
                            if (targetCol > 0 && this[row, targetCol - 1] != null &&
                                !merged[row, targetCol - 1] && this[row, targetCol - 1].Value == this[row, targetCol].Value)
                            {
                                this[row, targetCol - 1].Value *= 2;
                                this.RemoveGameBox(row, targetCol);
                                merged[row, targetCol - 1] = true;
                            }
                            
                        }
                    }
                    break;

                case Direction.Right:               
                    for (int row = 0; row < 4; row++)
                    {
                        for (int col = 2; col >= 0; col--)
                        {
                            if (this[row, col] == null)
                            {
                                continue;
                            }
                            // move box as far left as possible
                            int targetCol = col;
                            while (targetCol < 3 && this[row, targetCol + 1] == null)
                            {
                                targetCol++;
                            }
                            if (targetCol != col)
                            {
                                this.MoveGameBox(row, col, row, targetCol);
                                col = targetCol; // skip the rest of this row
                            }

                            // merge with adjacent box if possible
                            if (targetCol < 3 && this[row, targetCol + 1] != null &&
                                !merged[row, targetCol + 1] && this[row, targetCol + 1].Value == this[row, targetCol].Value)
                            {
                                this[row, targetCol + 1].Value *= 2;
                                this.RemoveGameBox(row, targetCol);
                                merged[row, targetCol + 1] = true;
                            }

                        }
                    }
                    break;

                case Direction.Up:
                    for (int col = 0; col < 4; col++)
                    {
                        for (int row = 1; row < 4; row++)
                        {
                            if (this[row, col] == null)
                            {
                                continue;
                            }
                            // move box as far up as possible
                            int targetRow = row;
                            while (targetRow > 0 && this[targetRow - 1, col] == null)
                            {
                                targetRow--;
                            }
                            if (targetRow != row)
                            {
                                
                                this.MoveGameBox(row, col, targetRow, col);
                                row = targetRow; // skip the rest of this column
                            }

                            // merge with adjacent box if possible
                            if (targetRow > 0 && this[targetRow - 1, col] != null &&
                                !merged[targetRow - 1, col] && this[targetRow - 1, col].Value == this[targetRow, col].Value)
                            {
                                this[targetRow - 1, col].Value *= 2;
                                this.RemoveGameBox(targetRow, col);
                                merged[targetRow - 1, col] = true;
                            }
                        }
                    }
                    break;

                case Direction.Down:
                    for (int col = 0; col < 4; col++)
                    {
                        for (int row = 2; row >= 0; row--)
                        {
                            if (this[row, col] == null)
                            {
                                continue;
                            }
                            // move box as far down as possible
                            int targetRow = row;
                            while (targetRow < 3 && this[targetRow + 1, col] == null)
                            {
                                targetRow++;
                            }
                            if (targetRow != row)
                            {
                                this.MoveGameBox(row, col, targetRow, col);
                                row = targetRow; // skip the rest of this column
                            }

                            // merge with adjacent box if possible
                            if (targetRow < 3 && this[targetRow + 1, col] != null &&
                                !merged[targetRow + 1, col] && this[targetRow + 1, col].Value == this[targetRow, col].Value)
                            {
                                this[targetRow + 1, col].Value *= 2;
                                this.RemoveGameBox(targetRow, col);
                                merged[targetRow + 1, col] = true;
                            }
                        }
                    }
                    break;

                    // implement cases for other directions (right, up, down) similarly
            }
        }
    }

    public class gamebox
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public void push()
        {
            Console.WriteLine("I'm called!");
        }
    }

}
