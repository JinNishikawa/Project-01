using System.Collections.Concurrent;
using Cysharp.Threading;
using Core.StreamingHub;
using UnityEngine;

namespace Core.Singleton
{
    public class PlayerManager
    {
        private readonly ILogger _logger;
        private readonly ILogicLooperPool _looperPool;
        private ConcurrentBag<int> _players = new ConcurrentBag<int>();
        private int _incrementId = 0;

        public PlayerManager(ILogger<PlayerManager> logger, ILogicLooperPool looperPool)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _looperPool = looperPool ?? throw new ArgumentNullException(nameof(looperPool));
        }

        public int CreateNewPlayer()
        {
            _incrementId++;
            _players.Add(_incrementId);
            _logger.LogDebug($"{_incrementId}が作成されました");

            return _incrementId;
        }

        public ConcurrentBag<int> GetCurrentPlayers()
        {
            return _players;
        }

        public void RemovePlayer(int id)
        {
            var player = _players.FirstOrDefault(p => p == id);
            var newBag = new ConcurrentBag<int>();
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