using System;


namespace Fastworks.Serialization
{
    public interface IObjectSerializer
    {
        // Serializes an object into a byte stream.
        byte[] Serialize<TObject>(TObject obj);

        // Deserializes an object from the given byte stream.
        TObject Deserialize<TObject>(byte[] stream);
    }
}
