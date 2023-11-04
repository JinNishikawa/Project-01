using Cysharp.Threading.Tasks;

public interface IFactory<T, Data> where T : IFactoryObject<Data>
{
    UniTask<T> Create(uint id);
}

public interface IFactoryObject<Arg>
{
    UniTask Setup(Arg arg);
}
