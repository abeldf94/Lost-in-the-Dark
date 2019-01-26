using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    // Depth-First search algorithm using backtracking for generate a maze that is all connected as a graph
    public class RecursiveBacktracker : MazeAlgorithm {

        // Single instance, if we call as new every time we need, it instantiates with same seed,
        // so it's return same value every time
        private readonly System.Random random;

        // Instantiates a new depth first search algorithm with a random seed
        public RecursiveBacktracker(MazeGrid maze) : base(maze) {
            random= new System.Random();
        }

        // Instantiates a new depth first search algorithm with a specific seed
        public RecursiveBacktracker(MazeGrid maze, int seed) : base(maze) {
            random = new System.Random(seed);
        }

        public override void ComputeAlgorithm() { 

            Vector2Int start = new Vector2Int(GetMaze().GetRows() / 2, GetMaze().GetColumns() / 2);
            Stack<MazeCell> stack = new Stack<MazeCell>();
            // Make the initial cell the current cell and mark it as visited
            MazeCell current = GetMaze().GetGrid()[start.x, start.y];
            current.SetVisited(true);
            
            int visitedCells = 1; // Start cell was visited already
            int amountCells = GetMaze().GetGrid().GetLength(0) * GetMaze().GetGrid().GetLength(1);
            while (visitedCells < amountCells) {
                // If the current cell has any neighbors which have not been visited 
                List<MazeCell> neighbors = FindNeighbors(current.GetX(), current.GetY());
                // Choose randomly one of the unvisited neighbors
                MazeCell next = ChooseRandom(neighbors);

                if (next != null) { // This mean that a neighbor exists and is not visited
                    // Push the current cell to the stack 
                    stack.Push(current);
                    // Remove the wall between the current cell and the chosen cell
                    RemoveWalls(current, next);
                    // Make the chosen cell the current cell and mark it as visited
                    current = next;
                    current.SetVisited(true);
                    visitedCells++;
                } else if (stack.Count != 0) { // In java: if (!stack.isEmpty())
                    current = stack.Pop();
                }
            }
        }
        
        // Removes the walls between the current and next cells.
        private void RemoveWalls(MazeCell current, MazeCell next) {
            if (current.GetX() != next.GetX()) {
                int dif = current.GetX() - next.GetX();
                if (dif == 1) {
                    current.SetNorth(false);
                    next.SetSouth(false);
                } else if (dif == -1) {
                    current.SetSouth(false);
                    next.SetNorth(false);
                }
            } else if (current.GetY() != next.GetY()) {
                int dif = current.GetY() - next.GetY();
                if (dif == 1) {
                    current.SetWest(false);
                    next.SetEast(false);
                } else if (dif == -1) {
                    current.SetEast(false);
                    next.SetWest(false);
                }
            } else {
                Debug.Log("unable to delete wall");
            }
        }

        // Find the neighbors of the current cell.
        private List<MazeCell> FindNeighbors(int x, int y) {

            List<MazeCell> neighbors = new List<MazeCell>();

            MazeCell north = GetMaze().GetElementAt(x, y - 1);
            MazeCell south = GetMaze().GetElementAt(x, y + 1);
            MazeCell east = GetMaze().GetElementAt(x + 1, y);
            MazeCell west = GetMaze().GetElementAt(x - 1, y);

            if (north != null && !north.IsVisited())
                neighbors.Add(north);

            if (south != null && !south.IsVisited())
                neighbors.Add(south);

            if (east != null && !east.IsVisited())
                neighbors.Add(east);

            if (west != null && !west.IsVisited())
                neighbors.Add(west);

            return neighbors;
        }

        // Choose a random neighbor from the list. 
        private MazeCell ChooseRandom(List<MazeCell> neighbors) {
            return (neighbors.Count > 0) ? neighbors[random.Next(neighbors.Count)] : null;
        }
    }
}
