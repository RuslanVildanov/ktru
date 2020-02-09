using System;

namespace Okpd2.infrastructure
{
    class FalseWhile : IDisposable
    {
        private readonly Action<bool> modify;

        public FalseWhile(Action<bool> m)
        {
            modify = m;
            modify(false);
        }

        public void Dispose()
        {
            modify(true);
        }
    }
}
