using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneratorSystem.Room {
	public class BaseRoomGenerator : MonoBehaviour {
		public Vector2Int Position { get; set; }
		public RoomConnectionInfo LeftRoom {
			get => Connections[0];
			set {
				Connections[0] = value;
			}
		}
		public RoomConnectionInfo TopRoom {
			get => Connections[1];
			set {
				Connections[1] = value;
			}
		}
		public RoomConnectionInfo RightRoom {
			get => Connections[2];
			set {
				Connections[2] = value;
			}
		}
		public RoomConnectionInfo BottomRoom {
			get => Connections[3];
			set {
				Connections[3] = value;
			}
		}
		public RoomConnectionInfo[] Connections { get; set; }

		[Header("Connections"), Space]
		[SerializeField] bool isLeftOpened;
		[SerializeField] bool isRightOpened;
		[SerializeField] bool isTopOpened;
		[SerializeField] bool isBottomOpened;

		private void Awake() {
			Connections = new RoomConnectionInfo[4] {
				new RoomConnectionInfo(Vector2Int.left,		isLeftOpened ?		RoomConnectionInfo.ConnectionType.Opened : RoomConnectionInfo.ConnectionType.Closed,		this, null),
				new RoomConnectionInfo(Vector2Int.up,		isTopOpened ?		RoomConnectionInfo.ConnectionType.Opened : RoomConnectionInfo.ConnectionType.Closed,		this, null),
				new RoomConnectionInfo(Vector2Int.right,	isRightOpened ?		RoomConnectionInfo.ConnectionType.Opened : RoomConnectionInfo.ConnectionType.Closed,		this, null),
				new RoomConnectionInfo(Vector2Int.down,		isBottomOpened ?	RoomConnectionInfo.ConnectionType.Opened : RoomConnectionInfo.ConnectionType.Closed,		this, null),
			};
		}

		public void GenerateRoomSequence() {
			GenerateRoom();
			OnEndGenerateRoom();
		}

		protected virtual void GenerateRoom() { }

		protected virtual void OnEndGenerateRoom() { }
	}
}
