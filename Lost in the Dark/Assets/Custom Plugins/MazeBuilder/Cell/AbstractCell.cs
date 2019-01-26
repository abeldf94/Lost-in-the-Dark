using UnityEngine;

namespace Maze {
    // Abstract implementation of a cell with walls inherited
    public abstract class AbstractCell : Walls {

        private Vector2Int location; // 2D location
        private bool visited; // Was cell visited?

        // Instantiates a new Abstract cell
        public AbstractCell(int x, int y) : base() {
            location = new Vector2Int(x, y);
            visited = false; // Cell starts unvisited always
        }

        // Return an object that represent a 2D point
        public Vector2Int GetLocation() {
            return location;
        }

        /** Getters and Setters **/
        public bool IsVisited() {
            return visited;
        }
        
        public void SetVisited(bool visited) {
            this.visited = visited;
        }
    }
}