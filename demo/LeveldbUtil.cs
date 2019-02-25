using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelDB;

namespace demo
{
    sealed class LeveldbUtil
    {
        private static LeveldbUtil _instance = null;
        private static readonly object synObject = new object();

        private LevelDB.DB levelDB;
        private string dbName = "data";

        private LeveldbUtil()
        {
            Options options = new Options()
            {
                CreateIfMissing = true,
            };

            this.levelDB = LevelDB.DB.Open(this.dbName, options);
        }

        public static LeveldbUtil Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (synObject)
                    {
                        if (null == _instance)
                            _instance = new LeveldbUtil();
                    }
                }

                return _instance;
            }
        }

        public void writeData(string key, byte[] value)
        {
            this.levelDB.Put(new WriteOptions(), key, value);
        }

        public void delDataWithKey(string key)
        {
            this.levelDB.Delete(new WriteOptions() { Sync = true }, key);
        }

        public byte[] getFirstValue(ref string key)
        {
            Iterator iterator = this.levelDB.NewIterator(new ReadOptions());

            iterator.SeekToFirst();

            if (!iterator.Valid())
                return null;

            key = iterator.Key().ToString();
            byte[] res = iterator.Value().ToArray();

            iterator.Dispose();

            return res;
        }

        public byte[] getValueWithKey(string key)
        {
            byte[] val = this.levelDB.Get(new ReadOptions(), key).ToArray();

            return val == null ? null : val;
        }

        public void getDataSize(ref long dataCount, ref long dataSize)
        {
            dataCount = 0;
            dataSize = 0;

            Iterator iterator = this.levelDB.NewIterator(new ReadOptions());

            iterator.SeekToFirst();

            while (iterator.Valid())
            {
                ++dataCount;

                byte[] valBytes = Convert.FromBase64String(iterator.Value().ToString());

                dataSize += valBytes.LongLength;

                iterator.Next();
            }

            iterator.Dispose();
        }

        public void closeDB()
        {
            this.levelDB.Dispose();
        }
    }
}
