using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    } // Represents a cell in the maze

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;
        public Vector2 offset;
        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }
            return 0;
        } // Returns the probability of spawning a room based on the position
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public GameObject player;
    public GameObject enemyPrefab;
    public int maxEnemies = 10;
    public float spawnInterval = 5f;

    private List<Cell> board;
    private Vector3 playerSpawnPosition;
    private List<Transform> roomPositions = new List<Transform>();
    private int currentEnemyCount = 0;

    void Start()
    {
        MazeGenerator();
        StartCoroutine(SpawnEnemies());
    } // Generates the dungeon and starts spawning enemies

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        if (p == 2)
                        {
                            randomRoom = k;
                            break;
                        } // Checks if a room is obligatory
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        } // Adds the room to the available rooms
                    } // Checks if a room can be spawned in the current position

                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        } // Spawns a random room if no obligatory room is found
                        else
                        {
                            randomRoom = Random.Range(0, 3);
                        } // Spawns a random available room if no available room is found
                    }

                    var roomOffset = rooms[randomRoom].offset;
                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * roomOffset.x, 0, -j * roomOffset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;

                    roomPositions.Add(newRoom.transform);

                    if (i == 0 && j == 0)
                    {
                        playerSpawnPosition = newRoom.transform.position;
                    } // Sets the player's spawn position
                } 
            } 
        }

        if (player != null)
        {
            player.transform.position = playerSpawnPosition;
        } // Spawns the player
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            } // Creates a cell for each position in the maze
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
            {
                break;
            } // Stops the generation if the last cell is reached

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            } // Backtracks if no neighbors are available
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                } // Updates the status of the cells based on the direction of the path
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                } // Updates the status of the cells based on the direction of the path
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        if (cell - size.x >= 0 && !board[cell - size.x].visited)
        {
            neighbors.Add(cell - size.x);
        } // Checks if the cell above is available

        if (cell + size.x < board.Count && !board[cell + size.x].visited)
        {
            neighbors.Add(cell + size.x);
        }

        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
        {
            neighbors.Add(cell + 1);
        }

        if (cell % size.x != 0 && !board[cell - 1].visited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    } // Checks the available neighbors of a cell

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
                currentEnemyCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    } // Spawns enemies at regular intervals

    private void SpawnEnemy()
    {
        if (roomPositions.Count > 0)
        {
            Transform randomRoom = roomPositions[Random.Range(0, roomPositions.Count)];
            Vector3 spawnPosition = randomRoom.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    } // Spawns an enemy in a random room

    public void EnemyDestroyed()
    {
        currentEnemyCount--;
    } // Decreases the enemy count when an enemy is destroyed
}
