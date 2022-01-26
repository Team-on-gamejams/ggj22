using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelGeneratorSystem.Room;

namespace LevelGeneratorSystem.Level {
	public class LevelGenerator : BaseLevelGenerator {
		[Header("Rooms prefabs"), Space]
		[SerializeField] Vector2 randomRoomsCount = new Vector2(5, 10);
		[SerializeField] Vector2 roomPrefabSize = Vector2.one;

		[Header("Rooms prefabs"), Space]
		[SerializeField] GameObject firstRoomPrefab;
		[SerializeField] GameObject preBossRoomPrefab;
		[SerializeField] GameObject bossRoomPrefab;
		[Space]
		[SerializeField] GameObject[] randomRoomPrefabs;

		List<BaseRoomGenerator> roomGenerators = new List<BaseRoomGenerator>();

		protected override void GenerateLevel() {
			base.GenerateLevel();

			List<RoomConnectionInfo> possibleConnections = new List<RoomConnectionInfo>();
			Queue<GameObject> roomPrefabsToAdd = new Queue<GameObject>();
			LevelGeneratorPass pass = LevelGeneratorPass.Started + 1;

			do {
				/////////////////////////////// Init data for pass
				switch (pass) {
					case LevelGeneratorPass.FirstRooms:
						if (firstRoomPrefab)
							roomPrefabsToAdd.Enqueue(firstRoomPrefab);
						break;

					case LevelGeneratorPass.RandomRooms:
						int count = randomRoomsCount.GetRandomValue();
						for (int i = 0; i < count; ++i) {
							roomPrefabsToAdd.Enqueue(randomRoomPrefabs.Random());
						}
						break;

					case LevelGeneratorPass.BossRooms:
						if (preBossRoomPrefab)
							roomPrefabsToAdd.Enqueue(preBossRoomPrefab);
						if (bossRoomPrefab)
							roomPrefabsToAdd.Enqueue(bossRoomPrefab);
						break;
				}
				///////////////////////////////


				/////////////////////////////// Do actuall pass
				while (roomPrefabsToAdd.Count > 0) {
					GameObject room = Instantiate(roomPrefabsToAdd.Dequeue(), transform);
					BaseRoomGenerator roomGenerator = room.GetComponent<BaseRoomGenerator>();
					RoomConnectionInfo roomConnection = null;

					// Find possible connection
					while (roomConnection == null && possibleConnections.Count != 0) {
						int randomId = Random.Range(0, possibleConnections.Count);
						roomConnection = possibleConnections[randomId];
						possibleConnections.RemoveAt(randomId);

						if (roomConnection.connectionType != RoomConnectionInfo.ConnectionType.Opened)
							roomConnection = null;
					}

					// Place room
					if (roomConnection == null) {
						if (pass == LevelGeneratorPass.FirstRooms) {
							roomGenerator.Position = new Vector2Int(0, 0);
						}
						else {
							Debug.LogError($"No more free space for room - {room.name}");
							Destroy(room);
							continue;
						}
					}
					else {
						roomGenerator.Position = roomConnection.room.Position + roomConnection.direction;

						if (roomConnection.direction == Vector2Int.left) {
							roomConnection.roomToConnect = roomGenerator;
							roomGenerator.RightRoom.roomToConnect = roomConnection.room;
						}
						else if (roomConnection.direction == Vector2Int.right) {
							roomConnection.roomToConnect = roomGenerator;
							roomGenerator.LeftRoom.roomToConnect = roomConnection.room;
						}
						else if (roomConnection.direction == Vector2Int.up) {
							roomConnection.roomToConnect = roomGenerator;
							roomGenerator.BottomRoom.roomToConnect = roomConnection.room;
						}
						else if (roomConnection.direction == Vector2Int.down) {
							roomConnection.roomToConnect = roomGenerator;
							roomGenerator.TopRoom.roomToConnect = roomConnection.room;
						}
					}

					//Set room data
					roomGenerator.transform.position = transform.position + new Vector3(roomGenerator.Position.x * roomPrefabSize.x, 0, roomGenerator.Position.y * roomPrefabSize.y);
					roomGenerator.transform.name = roomGenerator.transform.name.Replace("(Clone)", "") + roomGenerator.Position.ToString();

					//Close all other passes
					foreach (var otherRoom in roomGenerators) {
						Vector2Int diff = roomGenerator.Position - otherRoom.Position;

						if (diff == Vector2Int.left) {
							if (otherRoom.LeftRoom.roomToConnect == null) {
								otherRoom.LeftRoom.roomToConnect = roomGenerator;
								roomGenerator.RightRoom.roomToConnect = otherRoom;

								if (otherRoom.LeftRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									otherRoom.LeftRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
								if (roomGenerator.RightRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									roomGenerator.RightRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
							}
						}
						else if (diff == Vector2Int.right) {
							if (otherRoom.RightRoom.roomToConnect == null) {
								otherRoom.RightRoom.roomToConnect = roomGenerator;
								roomGenerator.LeftRoom.roomToConnect = otherRoom;

								if (otherRoom.RightRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									otherRoom.RightRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
								if (roomGenerator.LeftRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									roomGenerator.LeftRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
							}
						}
						else if (diff == Vector2Int.up) {
							if (otherRoom.TopRoom.roomToConnect == null) {
								otherRoom.TopRoom.roomToConnect = roomGenerator;
								roomGenerator.BottomRoom.roomToConnect = otherRoom;

								if (otherRoom.TopRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									otherRoom.TopRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
								if (roomGenerator.BottomRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									roomGenerator.BottomRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
							}
						}
						else if (diff == Vector2Int.down) {
							if (otherRoom.BottomRoom.roomToConnect == null) {
								otherRoom.BottomRoom.roomToConnect = roomGenerator;
								roomGenerator.TopRoom.roomToConnect = otherRoom;

								if (otherRoom.BottomRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									otherRoom.BottomRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
								if (roomGenerator.TopRoom.connectionType == RoomConnectionInfo.ConnectionType.Opened)
									roomGenerator.TopRoom.connectionType = RoomConnectionInfo.ConnectionType.ClosedButCanBeOpened;
							}
						}
					}

					// Add possible connections
					switch (pass) {
						case LevelGeneratorPass.RandomRooms:
							foreach (var connection in roomGenerator.Connections) {
								if (connection.connectionType == RoomConnectionInfo.ConnectionType.Opened && connection.roomToConnect == null) {
									possibleConnections.Add(connection);
								}
							}
							break;

						case LevelGeneratorPass.FirstRooms:
						case LevelGeneratorPass.BossRooms:
							foreach (var connection in roomGenerator.Connections) {
								if (connection.connectionType == RoomConnectionInfo.ConnectionType.Opened && connection.roomToConnect == null && connection.direction == Vector2Int.up) {
									possibleConnections.Add(connection);
									break;
								}
							}
							break;
					}

					roomGenerators.Add(roomGenerator);
				}
				///////////////////////////////


				/////////////////////////////// End pass
				if (pass == LevelGeneratorPass.RandomRooms) {
					RoomConnectionInfo lastFreeWithUp = null;

					foreach (var conn in possibleConnections) {
						if (conn.direction == Vector2Int.up && conn.connectionType == RoomConnectionInfo.ConnectionType.Opened && conn.roomToConnect == null) {
							lastFreeWithUp = conn;
						}
					}

					if(lastFreeWithUp == null) {
						foreach (var conn in possibleConnections) {
							if (conn.connectionType == RoomConnectionInfo.ConnectionType.Opened && conn.roomToConnect == null) {
								lastFreeWithUp = conn;
							}
						}
					}

					possibleConnections.Clear();
					possibleConnections.Add(lastFreeWithUp);
				}

				++pass;
				///////////////////////////////
			} while (pass != LevelGeneratorPass.Ended);
		}

		protected override void OnEndGenerateLevel() {
			base.OnEndGenerateLevel();

			foreach (var roomGenerator in roomGenerators) {
				roomGenerator.GenerateRoomSequence();
			}
		}

		enum LevelGeneratorPass : byte {
			Started,
			FirstRooms,
			RandomRooms,
			BossRooms,
			Ended,
		}
	}
}
