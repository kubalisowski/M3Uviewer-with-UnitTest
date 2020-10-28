using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaylistMain;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace M3UTest
{
    [TestClass]
    public class TestClass1
    {
        public static string path = @"c:\Users\Sferis\source\repos\nubirius\M3UTest\playlist.m3u";
        List<string> paths = new List<string>() { path };

        [TestMethod]
        public void M3UItemEmptinessTest()
        {
            M3UItem item = new M3UItem(new FileInfo(path), new LoadOptions(/*ShowComments*/false, /*ShowPath*/true));
            List<M3UItem> listItem = new List<M3UItem>();
            listItem.Add(item);
            bool isEmpty = listItem.Any();

            Assert.IsTrue(isEmpty);
        }

        [TestMethod]
        public void loadFileTest()
        {
            // HidePath 
            Dictionary<string, bool?> loadSnapshot = new Dictionary<string, bool?>
            {
                { "ShowComments", true },
                { "ShowPath", false }
            };
            
            //M3USingleItem item = new M3USingleItem(path, "AddedFile", loadSnapshot);
            M3USingleItem item = new M3USingleItem(path, "AddedFile", loadSnapshot["ShowComments"]);

            Console.WriteLine(item.ContentLine);
        }

        [TestMethod]
        public void hidePathTest()
        {
            M3U backend = new M3U();
            // List as argument
            List<string> resultList = backend.HidePath(paths);
            // String as argument
            //string resultStr = backend.HidePath(path);

            //Console.WriteLine(resultList[resultList.Count - 1]);
            foreach (string i in resultList)
            {
                Console.WriteLine(i);
            }
            
        }
    }
}
