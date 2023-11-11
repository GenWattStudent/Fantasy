using System;
using Unity.Netcode;

public struct PlayerData : INetworkSerializable, IEquatable<PlayerData>
{
    public ulong ClientId;
    public byte TextureId;

    public PlayerData(ulong clientId, byte textureId)
    {
        ClientId = clientId;
        TextureId = textureId;
    }

    public bool Equals(PlayerData other)
    {
        return ClientId == other.ClientId && TextureId == other.TextureId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref TextureId);
    }
}
