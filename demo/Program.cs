using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevelDB;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            LeveldbUtil leveldbUtil = LeveldbUtil.Instance;
            DateTime now = DateTime.Now;

            //for (int i = 0; i < 300000; ++i)
            //{
            //    string key = now.AddDays(i).ToString("yyyy-MM-dd");
            //    string valBase64Str = Convert.ToBase64String(BitConverter.GetBytes(i));

            //    byte[] val = Convert.FromBase64String(valBase64Str);

            //    leveldbUtil.writeData(key, val);

            //    Console.WriteLine("Add key: " + key + " i: " + i);
            //}

            string firstKey = "";
            byte[] firstVal = leveldbUtil.getFirstValue(ref firstKey);

            Console.WriteLine("First Key: " + firstKey 
                + " value: " + getRealData(firstVal));

            leveldbUtil.delDataWithKey(firstKey);

            byte[] val = leveldbUtil.getValueWithKey(now.AddDays(10000)
                .ToString("yyyy-MM-dd"));

            if (val != null)
                Console.WriteLine("Real value is {0}", getRealData(val));

            leveldbUtil.closeDB();

            Console.Read();
        }

        static int getRealData(byte[] val)
        {
            string valBase64Str = Convert.ToBase64String(val);
            int realValue = BitConverter.ToInt32(
                Convert.FromBase64String(valBase64Str), 0);

            return realValue;
        }
    }
}
