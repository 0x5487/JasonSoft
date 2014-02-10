using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace JasonSoft.IO.Compression
{
    public static class CompressionExtension
    {
        /// <summary>
        /// Returns an array of bytes compressed by GZIP
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] data)
        {
            MemoryStream output = new MemoryStream();
            GZipStream gzip = new GZipStream(output,
                                             CompressionMode.Compress, true);
            gzip.Write(data, 0, data.Length);
            gzip.Close();
            return output.ToArray();
        }

        /// <summary>
        /// Returns an array of bytes Decompressed by GZIP
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] data)
        {
            MemoryStream input = new MemoryStream();
            input.Write(data, 0, data.Length);
            input.Position = 0;
            GZipStream gzip = new GZipStream(input,
                                             CompressionMode.Decompress, true);
            MemoryStream output = new MemoryStream();
            byte[] buff = new byte[64];
            int read = -1;
            read = gzip.Read(buff, 0, buff.Length);
            while (read > 0)
            {
                output.Write(buff, 0, read);
                read = gzip.Read(buff, 0, buff.Length);
            }
            gzip.Close();
            return output.ToArray();
        }
    }
}
