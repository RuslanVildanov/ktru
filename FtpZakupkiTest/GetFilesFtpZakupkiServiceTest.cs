using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZakupkiUtils;

namespace FtpZakupkiTest
{
    [TestClass]
    public class GetFilesFtpZakupkiServiceTest
    {
        [TestMethod]
        public void SuccessTest()
        {
            IEnumerable<ZakupkiFile> result = FtpZakupkiService.GetFiles(FtpZakupkiService.KTRU_FTP_URL);
            Assert.IsTrue(result.Any());
        }
    }
}
