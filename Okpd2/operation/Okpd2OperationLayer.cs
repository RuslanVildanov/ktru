using Okpd2.model;
using System.Diagnostics;

namespace Okpd2.operation
{
    class Okpd2OperationLayer
    {
        public Okpd2OperationLayer(Okpd2Model m)
        {
            model = m;
            Trace.Assert(model != null);
        }

        private Okpd2Model model;
    }
}
