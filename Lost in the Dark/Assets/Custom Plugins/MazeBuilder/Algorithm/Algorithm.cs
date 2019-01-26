using UnityEngine;

namespace Maze {
    // Abstract implementation that all the maze algorithms for this proyect should have
    public abstract class MazeAlgorithm {

        private readonly MazeGrid maze; // Maze for the algorithm

        // Instantiates a new maze
        protected MazeAlgorithm(MazeGrid maze) {
            this.maze = maze;
        }

        // All the algorithms that implements thi
        public abstract void ComputeAlgorithm();
    
        protected MazeGrid GetMaze() {
            return maze;
        }
    }
}
