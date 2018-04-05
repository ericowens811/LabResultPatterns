using QTB3.Model.LabResultPatterns.UnitsOfMeasure;
using QTB3.Test.Support.Db;
using System;
using System.Collections.Generic;


namespace QTB3.Test.LabResultPatterns.Support.TestData
{

    public static class UomTestData
    {
        public static Uom[] GetUomsArray()
        {
            var initialUoms =  new Uom[200];
            initialUoms[0] = new Uom( 1, "mg/dL", "milligrams/deciliter" );
            initialUoms[1] = new Uom ( 2, "in", "inch" );
            initialUoms[2] = new Uom ( 3, "mmHg", "millimeter of mercury" );
            var gen = new AlphaOrderedStringGenerator();
            for (var i = 3; i < 200; i++)
            {
                var uom = new Uom
                (
                    i + 4,
                    $"uom{gen.GetStringFor(i)}",
                    "unit description"
                );
                initialUoms[i] = uom;
            }
            return initialUoms;
        }

        public static List<IEnumerable<Object>> GetInitialData()
        {
            return new List<IEnumerable<object>>
            {
                GetUomsArray()
            };
        }

        public static List<IEnumerable<Object>> GetInitialDataNoIds()
        {
            var uoms = GetUomsArray();
            var idLessUoms = new Uom[uoms.Length];
            for (var i=0; i< uoms.Length; i++)
            {
                idLessUoms[i] = new Uom(0, uoms[i].Name, uoms[i].Description);
            }
            return new List<IEnumerable<object>>
            {
                idLessUoms
            };
        }
    }
}
