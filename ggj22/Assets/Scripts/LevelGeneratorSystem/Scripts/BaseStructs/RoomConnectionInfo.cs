using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelGeneratorSystem.Room;

namespace LevelGeneratorSystem {
	public class RoomConnectionInfo {
		public enum ConnectionType {
			None,
			Opened,
			Closed,
			ClosedButCanBeOpened,
		}

		public Vector2Int direction;
		public ConnectionType connectionType;
		public BaseRoomGenerator room;
		public BaseRoomGenerator roomToConnect;

		public RoomConnectionInfo() {

		}

		public RoomConnectionInfo(Vector2Int direction, ConnectionType connectionType, BaseRoomGenerator room = null, BaseRoomGenerator roomToConnect = null) {
			this.direction = direction;
			this.connectionType = connectionType;
			this.room = room;
			this.roomToConnect = roomToConnect;
		}
	}
}
