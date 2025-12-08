using Algorithms_DataStruct_Lib.Trees;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Algorithms.DataStruct.Lib.Tests.SymbolTables
{
    [TestFixture]
    public class BstSymbolTableTests
    {
        [Test]
        public void Contains_ValueTypeTable_ReturnsFalseForMissingKey()
        {
            var table = new BstSymbolTable<string, int>();

            ClassicAssert.IsFalse(table.contains("missing"));

            table.put("a", 10);

            ClassicAssert.IsTrue(table.contains("a"));
            ClassicAssert.IsFalse(table.contains("missing"));
        }

        [Test]
        public void GetCount_UsesExclusiveUpperBoundWhenKeyMissing()
        {
            var table = new BstSymbolTable<int, int>();

            table.put(1, 10);
            table.put(3, 30);

            ClassicAssert.AreEqual(1, table.GetCount(1, 2));
            ClassicAssert.AreEqual(2, table.GetCount(1, 3));
        }
    }
}
