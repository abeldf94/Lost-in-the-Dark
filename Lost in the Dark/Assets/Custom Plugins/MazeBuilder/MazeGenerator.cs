using UnityEngine;

namespace Maze {

    /// <summary>
    /// Creates a maze, starts the algorithm to build it and generate in Unity3D using GameObjects
    /// </summary>
    public class MazeGenerator {
        // The maze
        private MazeGrid maze;

        // Parent container level
        private GameObject gameLevel;

        // Containers for child objects
        private GameObject floorContainer;
        private GameObject wallContainer;
        private GameObject pillarContainer;
        private GameObject ceilingContainer;

        // Constants
        private readonly string GAME_LEVEL = "Level";
        private readonly string FLOORS = "floors";
        private readonly string WALLS = "walls";
        private readonly string PILLARS = "pillars";
        private readonly string CEILINGS = "ceilings";
        private readonly string NORTH = "north_";
        private readonly string SOUTH = "south_";
        private readonly string WEST = "west_";
        private readonly string EAST = "east_";
        private readonly string FLOOR = "floor_";
        private readonly string CEILING = "ceiling_";
        private readonly string PILLAR = "pillar_";
        private readonly string TR = "_TR";
        private readonly string TL = "_TL";
        private readonly string BR = "_BR";
        private readonly string BL = "_BL";

        // Walls coords
        private float wallX;
        private float wallY;
        private float wallZ;
        // Floor coords
        private float floorX;
        private float floorY;
        private float floorZ;

        private GameObject[] wall;
        private GameObject[] arch;
        private GameObject pillar;

        // Some important bounds to consider
        private Vector3 floorBounds;
        private Vector3 wallBounds;

        // Maze generator algorithm
        private MazeAlgorithm algorithm;

        private readonly int[] doorArchProbabilities = new[] { 85, 15 };
        private readonly int[] wallProbabilities = new[] { 70, 30 };

        /// <summary>
        /// Instantiates a new maze generator
        /// </summary>
        /// <param name="columns"> Number of columns </param>
        /// <param name="rows"> Number of rows </param>
        /// <param name="useSeed"> Use a seed for the random </param>
        /// <param name="seed"> The seed </param>
        /// <param name="wall"> The walls prefabs </param>
        /// <param name="arch"> The arch prefabs </param>
        /// <param name="pillar"> The pillar prefabs </param>
        public MazeGenerator(int columns, int rows, bool useSeed, int seed, GameObject[] wall, GameObject[] arch, GameObject pillar) {
            maze = new MazeGrid(columns, rows);
            gameLevel = new GameObject();
            wallContainer = new GameObject();
            floorContainer = new GameObject();
            ceilingContainer = new GameObject();
            pillarContainer = new GameObject();

            this.wall = wall;
            this.arch = arch;
            this.pillar = pillar;

            wallBounds = wall[0].GetComponent<Renderer>().bounds.size;

            InitPositions();
            InitContainers();

            if (useSeed)
                algorithm = new RecursiveBacktracker(maze, seed);
            else
                algorithm = new RecursiveBacktracker(maze);

            algorithm.ComputeAlgorithm();
        }

        /// <summary>
        /// Add some defaults parameters to the parent containers
        /// </summary>
        private void InitContainers() {
            gameLevel.transform.position = Vector3.zero;
            gameLevel.name = GAME_LEVEL;
            gameLevel.tag = GAME_LEVEL;

            floorContainer.transform.position = Vector3.zero;
            floorContainer.name = FLOORS;

            wallContainer.transform.position = Vector3.zero;
            wallContainer.name = WALLS;

            pillarContainer.transform.position = Vector3.zero;
            pillarContainer.name = PILLARS;

            ceilingContainer.transform.position = Vector3.zero;
            ceilingContainer.name = CEILINGS;
        }

        /// <summary>
        /// Initialize variables
        /// </summary>
        private void InitPositions() {
            wallX = 0.0f;
            wallY = 0.0f;
            wallZ = 0.0f;

            floorX = 0.0f;
            floorY = 0.0f;
            floorZ = 0.0f;
        }

        /// <summary>
        /// Creates the maze in Unity using objects and calculating all the positions for each one
        /// </summary>
        /// <param name="floor"> floor prefab for calculate bounds </param>
        /// <param name="ceiling"> ceiling prefab for calculate bounds </param>
        public void BuildInUnity(GameObject floor, GameObject ceiling) {

            floorBounds = floor.GetComponent<Renderer>().bounds.size;

            // We built the object having his initial point in the center
            // This are the position in x, y and z that the floor objects will have
            floorX = floorBounds.x / 2;
            floorY = floorBounds.y;
            floorZ = floorBounds.z / 2;

            // This are the position in x, y and z that the wall objects will have
            wallX = wallBounds.x / 2;
            wallY = floorBounds.y;
            wallZ = wallBounds.z / 2;

            // This are the position in x, y and z that the ceiling objects will have
            float ceilingY = floorBounds.y + wallBounds.y;
            
            float translateX = (floorBounds.x / 2) - (wallBounds.x / 2);
            float translateY = floorBounds.x - wallBounds.x;
            float translateZ = (floorBounds.z / 2) - (wallBounds.x / 2);

            for (int x = 0; x < maze.GetGrid().GetLength(0); x++) {
                for (int z = 0; z < maze.GetGrid().GetLength(1); z++) {
                    // It builds the floor of the cell
                    GameObject floorObject = GameObject.Instantiate(floor, floorContainer.transform);
                    floorObject.transform.position = new Vector3(floorX, floorY, floorZ);
                    floorObject.name = FLOOR + x + "," + z;

                    // Lets build the cell walls
                    BuildWalls(x, z, translateX, translateY, translateZ);

                    // The pillars
                    BuildPillars(x, z);

                    // Finally the ceilling
                    GameObject ceilingObject = GameObject.Instantiate(ceiling, ceilingContainer.transform);
                    ceilingObject.transform.position = new Vector3(floorX, ceilingY, floorZ);
                    ceilingObject.name = CEILING + x + "," + z;

                    // Move the floor position every iteration his size
                    floorZ += floorBounds.z;
                    // Move the wall position every iteration his size
                    wallZ += floorBounds.z;
                }
                // New column for floors
                floorZ = floorBounds.z / 2;
                floorX += floorBounds.x;
                // New column for walls
                wallZ = floorBounds.z / 2;
                wallX += floorBounds.x;
            }
            SetParentObject();            
        }

        /// <summary>
        /// Build the walls for the current maze cell
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        /// <param name="translateX"> Bounds translated </param>
        /// <param name="translateY"> Bounds translated </param>
        /// <param name="translateZ"> Bounds translated </param>
        private void BuildWalls(int x, int z, float translateX, float translateY, float translateZ) {
            // North wal
            if (x == 0)
                if (maze.GetGrid()[x, z].HasNorthWall())
                    BuildNorthWall(x, z, true);
                else
                    BuildNorthWall(x, z, false);
            // West wall
            if (z == 0) 
                if (maze.GetGrid()[x, z].HasWestWall())
                    BuildWestWall(x, z, translateX, translateZ, true);
                else
                    BuildWestWall(x, z, translateX, translateZ, false);
            // East wall
            if (maze.GetGrid()[x, z].HasEastWall())
                BuildEastWall(x, z, translateX, translateZ, true);
            else
                BuildEastWall(x, z, translateX, translateZ, false);
            // South WAll
            if (maze.GetGrid()[x, z].HasSouthWall())
                BuildSouthWall(x, z, translateY, true);
            else
                BuildSouthWall(x, z, translateY, false);
        }

        /// <summary>
        /// Build the north wall
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        /// <param name="isWall"> If is not a wall, it put an arch door </param>
        private void BuildNorthWall(int x, int z, bool isWall) {
            GameObject northWall;
            if (isWall) {
                if (x == 0 || z == 0 || x == maze.GetGrid().GetLength(0) - 1 || z == maze.GetGrid().GetLength(1) - 1)
                    northWall = GameObject.Instantiate(wall[0], wallContainer.transform);
                else
                    northWall = GameObject.Instantiate(wall[GetRandomDoorIndex(wallProbabilities)], wallContainer.transform);
            } else {
                if (maze.GetGrid()[x, z].HasWall())
                    northWall = GameObject.Instantiate(arch[0], wallContainer.transform);
                else {
                    northWall = GameObject.Instantiate(arch[GetRandomDoorIndex(doorArchProbabilities)], wallContainer.transform);
                    maze.GetGrid()[x, z].SetWall(true);
                }
            }
            northWall.transform.position = new Vector3(wallX, wallY, wallZ);
            northWall.name = NORTH + x + "," + z;
        }

        /// <summary>
        /// Build the west wall
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        /// <param name="translateX"> Bounds translated </param>
        /// <param name="translateY"> Bounds translated </param>
        /// <param name="isWall"> If is not a wall, it put an arch door </param>
        private void BuildWestWall(int x, int z, float translateX, float translateZ, bool isWall) {
            GameObject westWall;

            if (isWall) {
                if (x == 0 || z == 0 || x == maze.GetGrid().GetLength(0) - 1 || z == maze.GetGrid().GetLength(1) - 1)
                    westWall = GameObject.Instantiate(wall[0], wallContainer.transform);
                else
                    westWall = GameObject.Instantiate(wall[GetRandomDoorIndex(wallProbabilities)], wallContainer.transform);
            } else {
                if (maze.GetGrid()[x, z].HasWall())
                    westWall = GameObject.Instantiate(arch[0], wallContainer.transform);
                else {
                    westWall = GameObject.Instantiate(arch[GetRandomDoorIndex(doorArchProbabilities)], wallContainer.transform);
                    maze.GetGrid()[x, z].SetWall(true);
                }
            }
            westWall.transform.position = new Vector3(wallX, wallY, wallZ);
            westWall.transform.Translate(new Vector3(translateX, 0, -translateZ), Space.World);
            westWall.transform.Rotate(new Vector3(0, -90, 0), Space.World);
            westWall.name = WEST + x + "," + z;
        }

        /// <summary>
        /// Build the east wall
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        /// <param name="translateX"> Bounds translated </param>
        /// <param name="translateY"> Bounds translated </param>
        /// <param name="isWall"> If is not a wall, it put an arch door </param>
        private void BuildEastWall(int x, int z, float translateX, float translateZ, bool isWall) {
            GameObject eastWall;

            if (isWall) {
                if (x == 0 || z == 0 || x == maze.GetGrid().GetLength(0) - 1 || z == maze.GetGrid().GetLength(1) - 1)
                    eastWall = GameObject.Instantiate(wall[0], wallContainer.transform);
                else
                    eastWall = GameObject.Instantiate(wall[GetRandomDoorIndex(wallProbabilities)], wallContainer.transform);
            } else {
                if (maze.GetGrid()[x, z].HasWall())
                    eastWall = GameObject.Instantiate(arch[0], wallContainer.transform);
                else {
                    eastWall = GameObject.Instantiate(arch[GetRandomDoorIndex(doorArchProbabilities)], wallContainer.transform);
                    maze.GetGrid()[x, z].SetWall(true);
                }
            }
            eastWall.transform.position = new Vector3(wallX, wallY, wallZ);
            eastWall.transform.Translate(new Vector3(translateX, 0, translateZ), Space.World);
            eastWall.transform.Rotate(new Vector3(0, -90, 0), Space.World);
            eastWall.name = EAST + x + "," + z;
        }

        /// <summary>
        /// Build the south wall
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        /// <param name="translateY"> Bounds translated </param>
        /// <param name="isWall"> If is not a wall, it put an arch door </param>
        private void BuildSouthWall(int x, int z, float translateY, bool isWall) {
            GameObject southWall;

            if (isWall) {
                if (x == 0 || z == 0 || x == maze.GetGrid().GetLength(0) - 1 || z == maze.GetGrid().GetLength(1) - 1)
                    southWall = GameObject.Instantiate(wall[0], wallContainer.transform);
                else
                    southWall = GameObject.Instantiate(wall[GetRandomDoorIndex(wallProbabilities)], wallContainer.transform);
            } else {
                if (maze.GetGrid()[x, z].HasWall())
                    southWall = GameObject.Instantiate(arch[0], wallContainer.transform);
                else {
                    southWall = GameObject.Instantiate(arch[GetRandomDoorIndex(doorArchProbabilities)], wallContainer.transform);
                    maze.GetGrid()[x, z].SetWall(true);
                }
            }
            southWall.transform.position = new Vector3(wallX, wallY, wallZ);
            southWall.transform.Translate(new Vector3(translateY, 0, 0), Space.World);
            southWall.name = SOUTH + x + "," + z;
        }

        /// <summary>
        /// Build the pillars
        /// </summary>
        /// <param name="x"> Current x </param>
        /// <param name="z"> Current z </param>
        private void BuildPillars(int x, int z) {
            GameObject pillarObject = GameObject.Instantiate(pillar, pillarContainer.transform);
            pillarObject.transform.position = new Vector3(floorX + (floorBounds.x / 2), floorBounds.y, floorZ + (floorBounds.x / 2));
            pillarObject.name = PILLAR + x + "," + z + TR;

            if (x == 0 && z == 0) {
                pillarObject = GameObject.Instantiate(pillar, pillarContainer.transform);
                pillarObject.transform.position = new Vector3(floorX - (floorBounds.x / 2), floorBounds.y, floorZ - (floorBounds.x / 2));
                pillarObject.name = PILLAR + x + "," + z + BL;
            }
            if (x == 0) {
                pillarObject = GameObject.Instantiate(pillar, pillarContainer.transform);
                pillarObject.transform.position = new Vector3(floorX - (floorBounds.x / 2), floorBounds.y, floorZ + (floorBounds.x / 2));
                pillarObject.name = PILLAR + x + "," + z + TL;
            }
            if (z == 0) {
                pillarObject = GameObject.Instantiate(pillar, pillarContainer.transform);
                pillarObject.transform.position = new Vector3(floorX + (floorBounds.x / 2), floorBounds.y, floorZ - (floorBounds.x / 2));
                pillarObject.name = PILLAR + x + "," + z + BR;
            }
        }

        /// <summary>
        /// Returns a random index to choose a door prefab
        /// </summary>
        /// <param name="probabilities"></param>
        /// <returns> The index </returns>
        private int GetRandomDoorIndex(int[] probabilities) {

            int randomValue = Random.Range(0, 100);

            for (int i = 0; i < probabilities.Length; i++) {
                if (randomValue <= probabilities[i])
                    return i;
                else
                    randomValue -= probabilities[i];
            }

            throw new UnityException("Something was wrong");
        }

        /// <summary>
        /// Set the level object as parente for all the containers
        /// </summary>
        private void SetParentObject() {
            wallContainer.transform.SetParent(gameLevel.transform);
            floorContainer.transform.SetParent(gameLevel.transform);
            ceilingContainer.transform.SetParent(gameLevel.transform);
            pillarContainer.transform.SetParent(gameLevel.transform);
        }
    }
}
