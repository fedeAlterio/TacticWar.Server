namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public interface IRoomsManager
    {
        Task<IRoom> NewRoom();
        Task DeleteRoom(int roomId);
        Task<IRoom> FindById(int id);
    }
}
