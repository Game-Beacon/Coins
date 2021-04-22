using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Tools
{
    public static class FileHandler
{
        static string CardsXmlFilePath
        {
            get
            {
                string path= Path.Combine(Application.dataPath, "Data\\TestCards.xml");
                return path;
            }
        }
        private static XmlNode ReadXmlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string end = Path.GetExtension(filePath);
            if (end != ".xml")
            {
                return null;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            return doc.DocumentElement;
        }
        private static List<XmlNode> ReadInnerNode(XmlNode xmlNode)
        {
            XmlNode root = xmlNode;
            IEnumerator enumerator = root.GetEnumerator();
            List<XmlNode> list = new List<XmlNode>();
            while (enumerator.MoveNext())
            {
                XmlNode node = (XmlNode)enumerator.Current;
                list.Add(node);
            }
            return list;
        }
        private static string[] ReadInnerText(XmlNode xmlNode)
        {
            XmlNode root = xmlNode;
            IEnumerator enumerator = root.GetEnumerator();
            string[] list = new string[7];

            int index = 0;
            while (enumerator.MoveNext())
            {
                XmlNode node = (XmlNode)enumerator.Current;
                list[index++]=node.InnerText;
            }
            return list;
        }
        public static List<Card> ReadCardsFile()
        {
            XmlNode root = ReadXmlFile(CardsXmlFilePath);
            List<XmlNode> CardNodes = ReadInnerNode(root);
            List<Card> returnList= new List<Card>();
            string[] cardInfo;
            foreach (var cardNode in CardNodes)
            {
                cardInfo= ReadInnerText(cardNode);
                Card card = new Card(cardInfo);
                returnList.Add(card);
            }
            return returnList;
        }
    }
}
