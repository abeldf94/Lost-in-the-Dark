
namespace Maze {
    // A cell template for the maze
    public class MazeCell : AbstractCell {

        private bool hasWall;

        // Instantiates a new maze cell
        public MazeCell(int x, int y) : base(x, y) {
            hasWall = false;
        }

        // Getters and Setters
        public bool HasWall() {
            return hasWall;
        }

        public void SetWall(bool hasWall) {
            this.hasWall = hasWall;
        }
        
        public int GetX() {
            return GetLocation().x;
        }

        public int GetY() {
            return GetLocation().y;
        }
    }
}
