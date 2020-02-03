using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Ktru.model
{
    class KtruStateMachineController
    {
        enum KtruField
        {
            Undefined,
            Code,
            Name,
            Unit,
            StartDate,
            Version,
            Actual
        }

        public KtruStateMachineController(XmlTextReader reader)
        {
            xmlReader = reader;
        }

        public void Run()
        {
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        ProcessInputTag(xmlReader.Name);
                        tags.Add(xmlReader.Name);
                        break;
                    case XmlNodeType.Text:
                        ProcessInputText(xmlReader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        tags.RemoveAt(tags.Count - 1);
                        break;
                }
            }
            if (newKtru != null)
            {
                ktrus.Add(newKtru);
            }
        }

        public IEnumerable<KtruItem> GetKtrus()
        {
            return ktrus;
        }

        private void ProcessInputTag(string tag)
        {
            if (tags.Count == 2 && tag == "oos:position" && tags[1] == "nsiKTRUs")
            {
                CreateNewKtru();
            }
            else if (tags.Count == 4 && tag == "oos:code" && tags[3] == "oos:data")
            {
                ktruField = KtruField.Code;
            }
            else if (tags.Count == 4 && tag == "oos:version" && tags[3] == "oos:data")
            {
                ktruField = KtruField.Version;
            }
            else if (tags.Count == 4 && tag == "oos:name" && tags[3] == "oos:data")
            {
                ktruField = KtruField.Name;
            }
            else if (tags.Count == 4 && tag == "oos:actual" && tags[3] == "oos:data")
            {
                ktruField = KtruField.Actual;
            }
            else if (tags.Count == 4 && tag == "oos:applicationDateStart" && tags[3] == "oos:data")
            {
                ktruField = KtruField.StartDate;
            }
            else if (tags.Count == 6 && tag == "oos:name" && tags[5] == "oos:OKEI")
            {
                ktruField = KtruField.Unit;
            }
        }

        private void CreateNewKtru()
        {
            if (newKtru != null)
            {
                ktrus.Add(newKtru);
            }
            newKtru = new KtruItem();
        }

        private void ProcessInputText(string value)
        {
            bool ok;
            var v = value.Trim();
            switch(ktruField)
            {
                case KtruField.Code:
                    newKtru.Code = v;
                    break;
                case KtruField.Name:
                    newKtru.Name = v;
                    break;
                case KtruField.Unit:
                    newKtru.Units.Add(v);
                    break;
                case KtruField.StartDate:
                    ok = DateTime.TryParse(v, out DateTime dt);
                    Trace.Assert(ok);
                    newKtru.StartDate = dt;
                    break;
                case KtruField.Version:
                    ok = int.TryParse(v, out int ver);
                    Trace.Assert(ok);
                    newKtru.Version = ver;
                    break;
                case KtruField.Actual:
                    ok = Boolean.TryParse(v, out bool result);
                    Trace.Assert(ok);
                    newKtru.Actual = result;
                    break;
                default:
                    break;
            }
            ktruField = KtruField.Undefined;
        }

        private XmlTextReader xmlReader;
        private KtruItem newKtru = null;
        private KtruField ktruField = KtruField.Undefined;
        private IList<string> tags = new List<string>();
        private IList<KtruItem> ktrus = new List<KtruItem>();
    }
}
