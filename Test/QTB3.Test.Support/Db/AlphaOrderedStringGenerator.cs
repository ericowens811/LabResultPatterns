using System;
using System.Collections.Generic;

namespace QTB3.Test.Support.Db
{
    public class AlphaOrderedStringGenerator
    {
        private Dictionary<int, string> Dl = new Dictionary<int, string>
        {
            [0] = "a",
            [1] = "b",
            [2] = "c",
            [3] = "d",
            [4] = "e",
            [5] = "f",
            [6] = "g",
            [7] = "h",
            [8] = "i",
            [9] = "j"
        };

        public string GetStringFor(int i)
        {
            if (i > 1000) throw new ArgumentException("i must be less than 1000!");
            var baseName = "_";
            var amod = i % 10; // ones
            var adiv = i / 10;
            var bmod = adiv % 10; // tens
            var bdiv = adiv / 10;
            var cmod = bdiv % 10; // hundreds
            return baseName + Dl[cmod] + Dl[bmod] + Dl[amod];
        }
    }
}
