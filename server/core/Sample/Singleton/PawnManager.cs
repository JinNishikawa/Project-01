//using System.Collections.Concurrent;
//using UnityEngine;

//namespace Core.Sample.Singleton
//{
//    public class PawnManager
//    {
//        private readonly ILogger _logger;
//        private ConcurrentDictionary<int, Pawn> _pawns = new ConcurrentDictionary<int, Pawn>();
//        private ConcurrentDictionary<int, PlayerHub> _currentPlayers = new ConcurrentDictionary<int, PlayerHub>();
//        private int _pawnCount;

//        public PawnManager(ILogger<PawnManager> logger)
//        {
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public void Update()
//        {
//            var players = _currentPlayers;
//            var pawnsToRemove = new List<int>();

//            // ポーンの移動処理
//            foreach (var kvp in _pawns)
//            {
//                var pawn = kvp.Value;
//                pawn.MoveForward();

//                foreach (var player in players.Values)
//                {
//                    player.MovePawn(pawn.Id, pawn.Position);
//                }

//                if (pawn.LifeTime < 0) pawnsToRemove.Add(pawn.Id);
//            }

//            // 死んだポーンの削除
//            foreach (var pawnId in pawnsToRemove)
//            {
//                if (_pawns.TryRemove(pawnId, out var removedPawn))
//                {
//                    foreach (var player in _currentPlayers.Values)
//                    {
//                        if (player.Id == removedPawn.OwnerId)
//                        {
//                            player.RemovePawn(removedPawn.Id);
//                        }
//                    }
//                }
//            }
//        }

//        public void AddPlayer(PlayerHub hub)
//        {
//            _currentPlayers.TryAdd(hub.Id, hub);
//        }

//        public void RemovePlayer(PlayerHub hub)
//        {
//            _currentPlayers.TryRemove(hub.Id, out _);
//        }

//        public Pawn CreatePawn(int ownerId, int typeId, Vector3 position)
//        {
//            // TODO: typeIdを元にPawnableインターフェースを実装した特定のPawnを生成する的な感じにする
//            var pawn = new Pawn(ownerId, _pawnCount, typeId, 60);
//            _pawnCount++;
//            pawn.Position = position;
//            _pawns.TryAdd(pawn.Id, pawn);
//            return pawn;
//        }
//    }
//}
