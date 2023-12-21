using Cysharp.Net.Http;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Sample;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class PlayerHubClient: MonoBehaviour, IPlayerHubReceiver
{
    public string Address = "https://localhost:32768";
    private int _myId = 0;
    private IPlayerHub _playerHubClient;
    private Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();
    private Dictionary<int, Dictionary<int, GameObject>> _pawns = new Dictionary<int, Dictionary<int, GameObject>>();
    private bool _initialized = false;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _pawnPrefab;
    private async void Start()
    {
        var httpHandler = new YetAnotherHttpHandler() { SkipCertificateVerification = true, Http2Only = true };
        var channel = GrpcChannel.ForAddress(Address, new GrpcChannelOptions() { HttpHandler = httpHandler });

        _playerHubClient = await StreamingHubClient.ConnectAsync<IPlayerHub, IPlayerHubReceiver>(channel, this);
        _myId = await _playerHubClient.JoinAsync();
        var players = await _playerHubClient.FetchCurrentPlayerAsync();
        foreach(var p in players) { AddPlayer(p.Id); }

        _initialized = true;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 26);
            var currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            _playerHubClient.SummonPawn(10, currentPosition);
        }
    }

    public void OnPlayerJoined(int id, Vector3 position)
    {
        AddPlayer(id);
        _players[id].transform.position = position;
    }

    public void OnPlayerLeaved(int id)
    {
        DestroyImmediate(_players[id].gameObject);
        _players.Remove(id);
    }

    public void OnPlayerMoved(int id, Vector3 position)
    {
        if (!_initialized) return;
        _players[id].transform.position = position;
    }

    public void OnPawnMoved(int ownerId, int pawnId, Vector3 position)
    {
        if(!_pawns[ownerId].ContainsKey(pawnId)) return;
        _pawns[ownerId][pawnId].transform.position = position;
    }

    public void OnPawnDied(int ownerId, int pawnId)
    {
        if (_pawns.ContainsKey(ownerId))
        {
            Destroy(_pawns[ownerId][pawnId]);
            var pawnDictionary = _pawns[ownerId];
            pawnDictionary.Remove(pawnId);

            // もし内側の辞書が空になったら、その辞書も削除する
            if (pawnDictionary.Count == 0)
            {
                _pawns.Remove(ownerId);
            }
        }
    }

    public void OnSummonedPawn(int ownerId, int pawnId, int typeId, Vector3 position)
    {
        // TODO：typeIdを元に生成するキャラクターを変える
        if (!_pawns.ContainsKey(ownerId))
        {
            _pawns[ownerId] = new Dictionary<int, GameObject>();
        }
        _pawns[ownerId][pawnId] = Instantiate(_pawnPrefab, position, Quaternion.identity);
    }

    public void AddPlayer(int id)
    {
        _players.Add(id, Instantiate(_playerPrefab));
    }

    public void OnApplicationQuit()
    {
        _playerHubClient.LeaveAsync();
        _playerHubClient.DisposeAsync();
    }
}
