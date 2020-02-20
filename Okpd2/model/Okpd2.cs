using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
<nsiOKPD2>
    <oos:id>72783</oos:id>
    <oos:parentId>8877570</oos:parentId>
    <oos:code>25.11.23.150</oos:code>
    <oos:parentCode>25.11.23</oos:parentCode>
    <oos:name>
Металлоконструкции специальные и детали металлоконструкций для области использования атомной энергии
    </oos:name>
    <oos:actual>true</oos:actual>
</nsiOKPD2>
*/

namespace Okpd2.model
{
    class Okpd2
    {
        public Okpd2()
        {}

        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public string Name { get; set; }
        public bool Actual { get; set; }

        public override bool Equals(Object obj)
        {
            Okpd2 obj2 = obj as Okpd2;
            return Id == obj2.Id
                && ParentId == obj2.ParentId
                && Code.Equals(obj2.Code)
                && ((ParentCode == null && obj2.ParentCode == null) || (ParentCode != null && obj2.ParentCode != null && ParentCode.Equals(obj2.ParentCode)))
                && Name.Equals(obj2.Name)
                && Actual == obj2.Actual;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() + ParentId.GetHashCode()
                + Code.GetHashCode() + ParentCode.GetHashCode()
                + Name.GetHashCode() + Actual.GetHashCode();
        }

        public override string ToString()
        {
            return Id + " " + ParentId + " " + Code + " " + ParentCode + " " + Name + " " + Actual;
        }
    }
}
