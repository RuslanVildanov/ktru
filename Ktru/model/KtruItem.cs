using System;
using System.Collections.Generic;

namespace Ktru.model
{
    class KtruItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public IList<string> Units { get; set; } = new List<string>();
        public DateTime StartDate { get; set; }
        public int Version { get; set; }
        public bool Actual { get; set; }

        public override bool Equals(Object obj)
        {
            KtruItem obj2 = obj as KtruItem;
            return Code.Equals(obj2.Code) && Version == obj2.Version;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode() + Version.GetHashCode();
        }
    }
}
