using VContainer;
using VContainer.Unity;
using Omino.Infra.Master;
using UnityEngine;
using MessagePack.Resolvers;
using MessagePack;

public class GameManager : LifetimeScope
{
    [SerializeField]
    private TextAsset _masterTable;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        StaticCompositeResolver.Instance.Register(
            MasterMemoryResolver.Instance,
            GeneratedResolver.Instance,
            StandardAotResolver.Instance
        );
        var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
        MessagePackSerializer.DefaultOptions = options;
    }

    protected override void Configure(IContainerBuilder builder)
    {
        var database = new MemoryDatabase(_masterTable.bytes);

        builder.RegisterInstance<MemoryDatabase>(database);
        builder.Register<PartyFactory>(Lifetime.Singleton);
        builder.Register<CharacterFactory>(Lifetime.Singleton);
        builder.RegisterEntryPoint<TestManager>(Lifetime.Singleton);
    }
}
