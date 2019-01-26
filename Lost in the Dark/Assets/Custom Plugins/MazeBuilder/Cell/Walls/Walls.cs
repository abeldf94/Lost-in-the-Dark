namespace Maze {
    // Abstract implementation for objects that could have walls in North, South, West and East
    public abstract class Walls {

        private bool north; // North wall
        private bool south; // South wall
        private bool east; // East wall
        private bool west; // West wall

        // Instanciate the walls, by default all the walls exist
        public Walls() {
            north = true;
            south = true;
            east = true;
            west = true;
        }

        // Getters and Setters
        public bool HasNorthWall() {
            return north;
        }

        public bool HasSouthWall() {
            return south;
        }

        public bool HasEastWall() {
            return east;
        }

        public bool HasWestWall() {
            return west;
        }

        public void SetNorth(bool north) {
            this.north = north;
        }

        public void SetSouth(bool south) {
            this.south = south;
        }

        public void SetEast(bool east) {
            this.east = east;
        }

        public void SetWest(bool west) {
            this.west = west;
        }
    } 
}