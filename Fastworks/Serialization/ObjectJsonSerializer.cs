using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Fastworks.Serialization
{
    public class ObjectJsonSerializer : IObjectSerializer
    {
        #region IObjectSerializer Members
       
        // Serializes an object into a byte stream.
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            Type graphType = obj.GetType();
            DataContractJsonSerializer js = new DataContractJsonSerializer(graphType);
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, obj);
                ret = ms.ToArray();
                ms.Close();
            }
            return ret;
        }
       
        // Deserializes an object from the given byte stream.
        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(TObject));
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)js.ReadObject(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
