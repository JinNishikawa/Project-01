//using System;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Cysharp.Threading;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System.Collections.Concurrent;
//using System.Collections.Generic;

//namespace Core.Sample.Singleton
//{
//    public class LoopHostedService : IHostedService
//    {
//        private readonly ILogicLooperPool _looperPool;
//        private readonly ILogger _logger;
//        private readonly PawnManager _pawnManager;

//        public LoopHostedService(ILogicLooperPool looperPool, PawnManager pawnManager, ILogger<LoopHostedService> logger)
//        {
//            _looperPool = looperPool ?? throw new ArgumentNullException(nameof(looperPool));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _pawnManager = pawnManager ?? throw new ArgumentNullException(nameof(pawnManager));
//        }
//        public Task StartAsync(CancellationToken cancellationToken)
//        {
//            _ = _looperPool.RegisterActionAsync((in LogicLooperActionContext ctx) =>
//            {
//                if (ctx.CancellationToken.IsCancellationRequested)
//                {
//                    _logger.LogInformation("LoopHostedService will be shutdown soon. The registered action is shutting down gracefully.");
//                    return false;
//                }

//                // TODO: ゲーム内に関する様々なイベントに対応できるよう設計する
//                _pawnManager.Update();

//                return true;
//            });
//            _logger.LogInformation($"LoopHostedService is started. (Loopers={_looperPool.Loopers.Count}; TargetFrameRate={_looperPool.Loopers[0].TargetFrameRate:0}fps)");

//            return Task.CompletedTask;
//        }

//        public async Task StopAsync(CancellationToken cancellationToken)
//        {
//            _logger.LogInformation("LoopHostedService is shutting down. Waiting for loops.");

//            await _looperPool.ShutdownAsync(TimeSpan.FromSeconds(2));

//            var remainedActions = _looperPool.Loopers.Sum(x => x.ApproximatelyRunningActions);
//            _logger.LogInformation($"{remainedActions} actions are remained in loop.");
//        }
//    }
//}
