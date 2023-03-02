using Game.Code.Data;
using Game.Code.Infrastructure.Context;
using Game.Code.Infrastructure.Factories;
using Game.Code.Logic.Actors.Villagers;
using UnityEngine;

namespace Game.Code.Logic.Actors
{
    public class ActorSpawner
    {
        private readonly GameConfig _gameConfig;
        private readonly GameFactory _gameFactory;
        private readonly DynamicGameContext _gameContext;

        private GameObject _actorsContainer;

        public ActorSpawner(GameConfig gameConfig, 
            GameFactory gameFactory, 
            DynamicGameContext gameContext)
        {
            _gameConfig = gameConfig;
            _gameFactory = gameFactory;
            _gameContext = gameContext;
        }

        public void Init() => 
            _actorsContainer = new GameObject("ActorsContainer");

        public void InitActors()
        {
            for (int i = 0; i < _gameConfig.InitialVillagerCount; i++)
            {
                Vector3 spawnPoint = new Vector3(
                    _actorsContainer.transform.position.x, 
                    _actorsContainer.transform.position.y, 
                    _actorsContainer.transform.position.z + i);
                
                SpawnVillager(spawnPoint);
            }
        }

        private void SpawnVillager(Vector3 spawnPoint)
        {
            Villager villager = _gameFactory.CreateActor<Villager>(ActorType.Villager, _actorsContainer, spawnPoint);
            
            _gameContext.AddVillager(villager);
            _gameContext.AddHomelessVillager(villager);
        }
    }
}