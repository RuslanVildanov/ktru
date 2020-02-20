using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Okpd2.model
{
    class Okpd2StateMachineController
    {
        enum Okpd2Field
        {
            Undefined,
            Id,
            ParentId,
            Code,
            ParentCode,
            Name,
            Actual
        }

        public Okpd2StateMachineController(XmlTextReader reader)
        {
            _xmlReader = reader;
        }

        public void Run()
        {
            while (_xmlReader.Read())
            {
                switch (_xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        ProcessInputTag(_xmlReader.Name);
                        _tags.Add(_xmlReader.Name);
                        break;
                    case XmlNodeType.Text:
                        ProcessInputText(_xmlReader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        _tags.RemoveAt(_tags.Count - 1);
                        break;
                }
            }
            if (_newOkpd2 != null)
            {
                _okpd2List.Add(_newOkpd2);
            }
        }

        public IEnumerable<Okpd2> GetOkpd2List()
        {
            return _okpd2List;
        }

        private void ProcessInputTag(string tag)
        {
            if (_tags.Count == 2 && tag == "nsiOKPD2" && _tags[1] == "nsiOKPD2List")
            {
                CreateNewOkpd2();
            }
            else if (_tags.Count == 3 && tag == "oos:id" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.Id;
            }
            else if (_tags.Count == 3 && tag == "oos:parentId" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.ParentId;
            }
            else if (_tags.Count == 3 && tag == "oos:code" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.Code;
            }
            else if (_tags.Count == 3 && tag == "oos:parentCode" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.ParentCode;
            }
            else if (_tags.Count == 3 && tag == "oos:name" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.Name;
            }
            else if (_tags.Count == 3 && tag == "oos:actual" && _tags[2] == "nsiOKPD2")
            {
                _okpd2Field = Okpd2Field.Actual;
            }
        }

        private void CreateNewOkpd2()
        {
            if (_newOkpd2 != null)
            {
                _okpd2List.Add(_newOkpd2);
            }
            _newOkpd2 = new Okpd2();
        }

        private void ProcessInputText(string value)
        {
            bool ok;
            int intValue;
            var v = value.Trim();
            switch (_okpd2Field)
            {
                case Okpd2Field.Id:
                    ok = int.TryParse(v, out intValue);
                    _newOkpd2.Id = ok ? intValue : -1;
                    break;
                case Okpd2Field.ParentId:
                    ok = int.TryParse(v, out intValue);
                    _newOkpd2.ParentId = ok ? intValue : -1;
                    break;
                case Okpd2Field.Code:
                    _newOkpd2.Code = v;
                    break;
                case Okpd2Field.ParentCode:
                    _newOkpd2.ParentCode = v;
                    break;
                case Okpd2Field.Name:
                    _newOkpd2.Name = v;
                    break;
                case Okpd2Field.Actual:
                    ok = bool.TryParse(v, out bool a);
                    _newOkpd2.Actual = ok ? a : false;
                    break;
                default:
                    break;
            }
            _okpd2Field = Okpd2Field.Undefined;
        }

        private XmlTextReader _xmlReader;
        private Okpd2 _newOkpd2 = null;
        private Okpd2Field _okpd2Field = Okpd2Field.Undefined;
        private IList<string> _tags = new List<string>();
        private IList<Okpd2> _okpd2List = new List<Okpd2>();
    }
}
