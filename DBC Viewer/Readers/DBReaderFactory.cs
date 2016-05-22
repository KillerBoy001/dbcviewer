﻿using PluginInterface;
using System.IO;

namespace DBCViewer
{
    class DBReaderFactory
    {
        public static IClientDBReader GetReader(string file, Table def)
        {
            IClientDBReader reader = null;

            var ext = Path.GetExtension(file).ToUpperInvariant();
            if (ext == ".DBC")
                reader = new DBCReader(file);
            else if (ext == ".DB2")
            {
                using (var br = Extensions.FromFile(file))
                {
                    uint magic = br.ReadUInt32();

                    br.Close();

                    switch (magic)
                    {
                        case DB2Reader.DB2FmtSig:
                            return new DB2Reader(file);
                        case DB3Reader.DB3FmtSig:
                            return new DB3Reader(file);
                        case DB4Reader.DB4FmtSig:
                            return new DB4Reader(file);
                        case DB5Reader.DB5FmtSig:
                            return new DB5Reader(file);
                    }
                }
            }
            else if (ext == ".ADB")
                reader = new ADBReader(file);
            else if (ext == ".WDB")
                reader = new WDBReader(file);
            else if (ext == ".STL")
                reader = new STLReader(file);
            else
                throw new InvalidDataException(string.Format("Unknown file type {0}", ext));

            return reader;
        }
    }
}
