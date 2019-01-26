using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    // The grid that contains all the maze cells as matrix
    public class MazeGrid {

        private int columns; // Number of columns
        private int rows; // Number of rows
        private MazeCell[,] grid; // Matrix for the maze

        // Instantiates a new maze grid
        public MazeGrid(int columns, int rows) {
            this.columns = columns;
            this.rows = rows;
            grid = new MazeCell[rows, columns];
            InitializeGrid();
        }

        // Set all the cells with given size
        public void InitializeGrid() {
            for (int x = 0; x < rows; x++) {
                for (int y = 0; y < columns; y++) {
                    grid.SetValue(new MazeCell(x, y), x, y);
                }
            }
        }

        // Return the element located in the (x,y) grid position
        public MazeCell GetElementAt(int x, int y) {
            if (x >= 0 && x < GetRows() && y >= 0 && y < GetColumns())
                return grid[x, y];
            else
                return null;
        }

        // Resize the maze grid and initialize again 
        public void ResizeMazeGrid(int columns, int rows) {
            this.columns = columns;
            this.rows = rows;
            grid = new MazeCell[rows, columns];
            InitializeGrid();
        }

        // Getters and Setters
        public int GetColumns() {
            return columns;
        }

        public int GetRows() {
            return rows;
        }

        public MazeCell[,] GetGrid() {
            return grid;
        }
    }
}
