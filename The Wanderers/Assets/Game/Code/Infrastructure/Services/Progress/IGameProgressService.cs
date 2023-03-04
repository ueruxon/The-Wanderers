using Game.Code.Data.Game;

namespace Game.Code.Infrastructure.Services.Progress
{
    public interface IGameProgressService
    {
        public GameProgress Progress { get; set; }
    }
}