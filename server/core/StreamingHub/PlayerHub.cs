using MagicOnion.Server.Hubs;
using Microsoft.Extensions.Logging;
using UnityEngine;
using Omino.Infra.StreamingHub;
using Core.Singleton;

namespace Core.StreamingHub
{
    public class PlayerHub : StreamingHubBase<IPlayerHub, IPlayerHubReceiver>, IPlayerHub
    {
        public int Id;

        private readonly ILogger _logger;
        private IGroup _group;
        private readonly PlayerManager _playerManager;
        public PlayerHub(PlayerManager playerManager, ILogger<PlayerHub> logger)
        {
            _playerManager = playerManager;
            _logger = logger;
        }

        public async ValueTask<int> JoinAsync()
        {
            _group = await Group.AddAsync("sample");
            Vector3 current;
            // 新しいプレイヤーを作成して保存
            Id = _playerManager.CreateNewPlayer();

            BroadcastExceptSelf(_group).OnPlayerJoined(Id);
            _logger.LogDebug($"{Id}が参加しました");

            return Id;
        }

        public async ValueTask<int[]> FetchCurrentPlayerAsync()
        {
            var result = _playerManager.GetCurrentPlayers().ToArray();
            foreach (var player in result)
            {
                _logger.LogDebug($"現在のプレイヤー: {player}");
            }

            return result;
        }

        public async ValueTask LeaveAsync()
        {
            if (_group != null)
            {
                _playerManager.RemovePlayer(Id);
                Broadcast(_group).OnPlayerLeaved(Id);
                _logger.LogDebug($"{Id}が退出しました");

                _group = null;

            }
        }
    }
}