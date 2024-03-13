namespace Matrix.Sdk.Core.Domain.Room
{
    using System.Collections.Generic;
    using Infrastructure.Dto.Sync;
    using Infrastructure.Dto.Sync.Event.Room;
    using RoomEvent;

    public record MatrixRoom(string Id, MatrixRoomStatus Status, List<string> JoinedUserIds)
    {
        public static MatrixRoom Create(string roomId, RoomResponse joinedRoom, MatrixRoomStatus status)
        {
            var joinedUserIds = new List<string>();
            foreach (RoomEventResponse timelineEvent in joinedRoom.Timeline.Events)
                if (JoinRoomEvent.Factory.TryCreateFrom(timelineEvent, roomId, out JoinRoomEvent joinRoomEvent))
                    joinedUserIds.Add(joinRoomEvent!.SenderUserId);

            return new MatrixRoom(roomId, status, joinedUserIds);
        }

        public static MatrixRoom CreateInvite(string roomId, InvitedRoom invitedRoom)
        {
            var joinedUserIds = new List<string>();
            foreach (RoomStrippedState timelineEvent in invitedRoom.InviteState.Events)
                if (JoinRoomEvent.Factory.TryCreateFromStrippedState(timelineEvent, roomId,
                        out JoinRoomEvent joinRoomEvent))
                    joinedUserIds.Add(joinRoomEvent!.SenderUserId);

            return new MatrixRoom(roomId, MatrixRoomStatus.Invited, joinedUserIds);
        }
    }
}