using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Fastworks.Serialization
{
    // Binary Serializer
    public class ObjectBinarySerializer : IObjectSerializer
    {
        #region Private Fields
        private readonly BinaryFormatter binaryFormatter = new BinaryFormatter();
        #endregion

        #region IObjectSerializer Members

        // Serializes an object into a byte stream.
        public virtual byte[] Serialize<TObject>(TObject obj)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream())
            {
                binaryFormatter.Serialize(ms, obj);
                ret = ms.ToArray();
                ms.Close();
            }
            return ret;
        }
       
        // Deserializes an object from the given byte stream.
        public virtual TObject Deserialize<TObject>(byte[] stream)
        {
            using (MemoryStream ms = new MemoryStream(stream))
            {
                TObject ret = (TObject)binaryFormatter.Deserialize(ms);
                ms.Close();
                return ret;
            }
        }

        #endregion
    }
}
