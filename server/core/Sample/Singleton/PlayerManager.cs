using Shared.Sample.MessagePack;
using System.Collections.Concurrent;
using Cysharp.Threading;
using Core.Sample.StreamingHub;
using UnityEngine;

namespace Core.Sample.Singleton
{
    public class PlayerManager
    {
        private readonly ILogger _logger;
        private readonly ILogicLooperPool _looperPool;
        private ConcurrentBag<Player> _players = new ConcurrentBag<Player>();
        private int _incrementId = 0;

        public PlayerManager(ILogger<PlayerManager> logger, ILogicLooperPool looperPool)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _looperPool = looperPool ?? throw new ArgumentNullException(nameof(looperPool));
        }

        public (int, Vector3) CreateNewPlayer(PlayerHub hub)
        {
            _incrementId++;
            var player = new Player(_incrementId, new Vector3(0, 0, 0));
            _players.Add(player);
            _logger.LogDebug($"{_incrementId}が作成されました");

            return (player.Id, player.Position);
        }

        public ConcurrentBag<Player> GetCurrentPlayers() 
        {
            return _players;
        }

        public Vector3 UpdatePosition(int id, Vector3 direction)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            if (player == null) return new Vector3(0, 0, 0);

            player.Position = direction;
            return player.Position;
        }

        public void RemovePlayer(int id)
        {
            var player = _players.FirstOrDefault(p => p.Id == id);
            var newBag = new ConcurrentBag<Player>();
            while (_players.TryTake(out var item))
            {
                if (item != player)
                {
                    newBag.Add(item);
                }
            }
            _players = newBag;
        }
    }
}
