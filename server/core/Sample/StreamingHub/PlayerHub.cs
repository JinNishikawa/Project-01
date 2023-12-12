using Shared.Sample;
using MagicOnion.Server.Hubs;
using Core.Sample.Singleton;
using Shared.Sample.MessagePack;
using System.Numerics;

namespace Core.Sample.StreamingHub
{
    public class PlayerHub : StreamingHubBase<IPlayerHub, IPlayerHubReceiver>, IPlayerHub
    {
        public int Id;

        private readonly ILogger _logger;
        private IGroup _group;
        private readonly PlayerManager _playerManager;
        private readonly PawnManager _pawnManger;

        public PlayerHub(PlayerManager playerManager, PawnManager pawnManager, ILogger<PlayerHub> logger)
        {
            _playerManager = playerManager;
            _pawnManger = pawnManager;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<int> JoinAsync()
        {
            _group = await Group.AddAsync("sample");
            Vector3Dto current;
            // 新しいプレイヤーを作成して保存
            (Id, current) = _playerManager.CreateNewPlayer(this);
            _pawnManger.AddPlayer(this);

            BroadcastExceptSelf(_group).OnPlayerJoined(Id, current);
            _logger.LogDebug($"{Id}が参加しました");

            return Id;
        }

        public async ValueTask<Player[]> FetchCurrentPlayerAsync()
        {
            var result = _playerManager.GetCurrentPlayers().ToArray();
            foreach(var player in result)
            {
                _logger.LogDebug($"現在のプレイヤー: {player.Id}");
            }

            return result;
        }

        public async ValueTask SummonPawn(int typeId, Vector3Dto position)
        {
            var pawn = _pawnManger.CreatePawn(Id, typeId, position.ToNumericsVector());

            Broadcast(_group).OnSummonedPawn(Id, pawn.Id, typeId, Vector3Dto.FromNumericsVector(pawn.Position));
        }

        public void Move(int id, Vector3Dto direction)
        {
            var result = _playerManager.UpdatePosition(id, direction);

            Broadcast(_group).OnPlayerMoved(id, result);
        }

        public void MovePawn(int pawnId, Vector3 position)
        {
            _logger.LogDebug($"Move {pawnId} to {position}");
            Broadcast(_group).OnPawnMoved(Id, pawnId, Vector3Dto.FromNumericsVector(position));
        }

        public void RemovePawn(int pawnId)
        {
            Broadcast(_group).OnPawnDied(Id, pawnId);
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
