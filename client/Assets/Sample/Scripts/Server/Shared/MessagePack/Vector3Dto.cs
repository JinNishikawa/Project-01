using MessagePack;

// UnityEngine.Vector3に依存しないVector3の定義
[MessagePackObject]
public struct Vector3Dto
{
    [Key(0)]
    public float X { get; set; }
    [Key(1)]
    public float Y { get; set; }
    [Key(2)]
    public float Z { get; set; }

    public Vector3Dto(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Vector3Dto operator +(Vector3Dto a, Vector3Dto b)
    {
        return new Vector3Dto(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    // UnityEngine.Vector3に変換するメソッド（クライアントサイドで使用）
    public UnityEngine.Vector3 ToUnityVector()
    {
        return new UnityEngine.Vector3(X, Y, Z);
    }

    // UnityEngine.Vector3からこのDTOを生成する静的メソッド（クライアントサイドで使用）
    public static Vector3Dto FromUnityVector(UnityEngine.Vector3 vector)
    {
        return new Vector3Dto(vector.x, vector.y, vector.z);
    }

    // System.Numerics.Vector3に変換するメソッド（サーバーサイドで使用）
    public System.Numerics.Vector3 ToNumericsVector()
    {
        return new System.Numerics.Vector3(X, Y, Z);
    }

    //System.Numerics.Vector3からこのDTOを生成する静的メソッド（サーバーサイドで使用）
    public static Vector3Dto FromNumericsVector(System.Numerics.Vector3 vector)
    {
        return new Vector3Dto(vector.X, vector.Y, vector.Z);
    }
}
