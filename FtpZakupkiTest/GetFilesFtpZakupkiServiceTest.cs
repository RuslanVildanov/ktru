using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZakupkiUtils.infrastructure;
using ZakupkiUtils.ftp;

namespace FtpZakupkiTest
{
    [TestClass]
    public class GetFilesFtpZakupkiServiceTest
    {
        [TestMethod]
        public void SuccessTest()
        {
            IEnumerable<ZakupkiFile> result = FtpZakupkiServiceStatic.GetFiles(FtpZakupkiServiceStatic.KTRU_FTP_URL);
            Assert.IsTrue(result.Any());
        }
    }
}
