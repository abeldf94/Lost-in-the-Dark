using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze {
    // Creates a maze, starts the algorithm to build it and generate in Unity3D using GameObjects
    public class MazeGenerator {
        // The maze
        private MazeGrid maze;

        // Containers for child objects
        private GameObject floorContainer;
        private GameObject wallContainer;
        private GameObject ceilingContainer;

        // Constants
        private readonly string FLOORS = "floors";
        private readonly string WALLS = "walls";
        private readonly string CEILINGS = "ceilings";
        private readonly string NORTH = "north_";
        private readonly string SOUTH = "south_";
        private readonly string WEST = "west_";
        private readonly string EAST = "east_";
        private readonly string FLOOR = "floor_";

        // Instantiates a new generator
        public MazeGenerator(int columns, int rows, bool useSeed, int seed) {
            maze = new MazeGrid(columns, rows);
            wallContainer = new GameObject();
            floorContainer = new GameObject();
            ceilingContainer = new GameObject();

            InitContainers();

            MazeAlgorithm algorithm;

            if (useSeed)
                algorithm = new RecursiveBacktracker(maze, seed);
            else
                algorithm = new RecursiveBacktracker(maze);

            algorithm.ComputeAlgorithm();
        }

        // Add some defaults parameters to the parent containers
        private void InitContainers() {
            floorContainer.transform.position = new Vector3();
            floorContainer.name = FLOORS;

            wallContainer.transform.position = new Vector3();
            wallContainer.name = WALLS;

            ceilingContainer.transform.position = new Vector3();
            ceilingContainer.name = CEILINGS;
        }

        // Creates the maze in Unity using objects
        public void BuildInUnity(GameObject floor, GameObject wall, float roomSize) {

            MazeCell[,] grid = maze.GetGrid();

            float floorX = roomSize / 2;
            float floorY = floor.transform.localScale.y / 2; // Size of floor in y axis divided by 2, the coordinate system is in center
            float floorZ = -(roomSize / 2);

            float wallX = roomSize / 2;
            // The wall starts above the floor, so his y coord must be the floor scale + half of wall(coordinate system it's in middle)
            float wallY = floor.transform.localScale.y + wall.transform.localScale.y / 2;
            float wallZ = -wall.transform.localScale.z / 2;

            float width = roomSize / 2;
            float height = wall.transform.localScale.z / 2;
            float translateDistance = -width + height;

            // Iterate over the grid and build for every cell the floor and the walls
            for (int x = 0; x < grid.GetLength(0); x++) {
                for (int z = 0; z < grid.GetLength(1); z++) {
                    // It builds the floor of the cell
                    GameObject floorObject = GameObject.Instantiate(floor, floorContainer.transform);
                    Vector3 size = floorObject.transform.localScale;
                    /*
                    if (size.x != roomSize || size.z != roomSize) {
                        size.x = roomSize;
                        size.z = roomSize;
                        floorObject.transform.localScale = size;
                    }
                    */
                    floorObject.transform.position = new Vector3(floorX, floorY, floorZ);
                    floorObject.name = FLOOR + x + "," + z;

                    // It builds the north wall if it's a cell in top of the grid and the cell has this wall
                    if (x == 0) {
                        if (grid[x, z].HasNorthWall()) {
                            GameObject northWall = GameObject.Instantiate(wall, wallContainer.transform);
                            size = northWall.transform.localScale;
                            /*
                            if (size.x != roomSize || size.y != roomSize) {
                                size.x = roomSize;
                                size.y = roomSize;
                                northWall.transform.localScale = size;
                            }
                            */
                            northWall.transform.position = new Vector3(wallX, wallY, wallZ + height);
                            northWall.name = NORTH + x + "," + z;
                        }
                    }

                    // It builds the west wall if it's a cell in the left of the grid and the cell has this wall
                    if (z == 0) {
                        if (grid[x, z].HasWestWall()) {
                            GameObject westWallObject = GameObject.Instantiate(wall, wallContainer.transform);
                            size = westWallObject.transform.localScale;
                            /*
                            if (size.x != roomSize || size.y != roomSize) {
                                size.x = roomSize;
                                size.y = roomSize;
                                westWallObject.transform.localScale = size;
                            }
                            */

                            westWallObject.transform.position = new Vector3(wallX, wallY, wallZ);
                            // The order it's important, so we first rotate wall and after translate his position
                            westWallObject.transform.Rotate(Vector3.up * 90f); // Rotate the wall
                            westWallObject.transform.Translate(new Vector3(-width, 0, translateDistance), Space.World); // Translate
                            westWallObject.name = WEST + x + "," + z;
                        }
                    }

                    // It builds the east wall if the cell has it
                    if (grid[x, z].HasEastWall()) {
                        GameObject eastWallObject = GameObject.Instantiate(wall, wallContainer.transform);
                        eastWallObject.transform.position = new Vector3(wallX, wallY, wallZ);
                        size = eastWallObject.transform.localScale;
                        /*
                        if (size.x != roomSize || size.y != roomSize) {
                            size.x = roomSize;
                            size.y = roomSize;
                            eastWallObject.transform.localScale = size;
                        }
                        */
                        // The order it's important, so we first rotate wall and after translate his position
                        eastWallObject.transform.Rotate(Vector3.up * 90f); // Rotate the wall
                        eastWallObject.transform.Translate(new Vector3(width, 0, translateDistance), Space.World); // Translate
                        eastWallObject.name = EAST + x + "," + z;
                    }

                    // It builds the south wall if the cell has it
                    if (grid[x, z].HasSouthWall()) {
                        GameObject southWallObject = GameObject.Instantiate(wall, wallContainer.transform);
                        size = southWallObject.transform.localScale;
                        /*
                        if (size.x != roomSize || size.y != roomSize) {
                            size.x = roomSize;
                            size.y = roomSize;
                            southWallObject.transform.localScale = size;
                        }
                        */
                        southWallObject.transform.position = new Vector3(wallX, wallY, wallZ);
                        // Translate to south
                        southWallObject.transform.Translate(new Vector3(0, 0, -roomSize + height), Space.World);
                        southWallObject.name = SOUTH + x + "," + z;
                    }
                    // Increase in parallel to room size input
                    floorX += roomSize;
                    wallX += roomSize;
                }
                floorX = roomSize / 2;
                floorZ -= roomSize;
                wallX = roomSize / 2;
                wallZ -= roomSize;
            }
        }
    }
}
