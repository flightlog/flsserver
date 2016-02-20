using System.IO;

namespace FLS.Common.Extensions
{
    public static class ByteExtensions
    {
        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            var memoryStream = new MemoryStream(bytes);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
