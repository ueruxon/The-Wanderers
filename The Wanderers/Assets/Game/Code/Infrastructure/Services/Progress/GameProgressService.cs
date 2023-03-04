using Game.Code.Data.Game;

namespace Game.Code.Infrastructure.Services.Progress
{
    public class GameProgressService : IGameProgressService
    {
        public GameProgress Progress { get; set; }
    }
}