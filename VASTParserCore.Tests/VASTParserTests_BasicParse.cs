///
/// Copyright 2024 Marek Laasik, Rockit Holding OÜ
/// Licensed under EUPL. Please see the following link for details
/// https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12
///
using Rockit.VASTParserCore;

namespace VASTParserCore.Tests
{
    [TestClass]
    [DeploymentItem(@".\VASTXML\Empty.xml", "")]
    [DeploymentItem(@".\VASTXML\Wrapper.xml", "")]
    [DeploymentItem(@".\VASTXML\InLine.xml", "")]
    public class VASTParserTests_BasicParse
    {
        [TestMethod]
        public void TestEmptyParsing()
        {
            var fileContent = new StreamReader(new FileStream("Empty.xml", FileMode.Open)).ReadToEnd();
            var vast = VASTParser.ParseFromXML(fileContent);
            Assert.IsNotNull(vast);
            Assert.AreEqual(0, vast.Ad.Count());
        }

        [TestMethod]
        public void TestWrapperParsing()
        {
            var fileContent = new StreamReader(new FileStream("Wrapper.xml", FileMode.Open)).ReadToEnd();
            var vast = VASTParser.ParseFromXML(fileContent);

            Assert.IsNotNull(vast);
            Assert.AreEqual(1, vast.Ad.Count());
            var ad = vast.Ad[0];
            Assert.AreEqual("12345678", ad.Id);
            Assert.IsInstanceOfType<Wrapper>(ad.AdDetail);
            var wrapper = (Wrapper)ad.AdDetail;
            Assert.AreEqual(2, wrapper.Impressions.Count());
            Assert.AreEqual("TestAdSystem", wrapper.AdSystem);
            Assert.AreEqual("http://localhost/inline.xml", wrapper.VASTAdTagURI);
            Assert.AreEqual("http://localhost/notification/123?foo=bar&baz=bar&foobar=bazbar&", wrapper.Impressions[0].ImpressionUrl);
            Assert.AreEqual("http://localhost/notification/456?foo=bar&baz=bar&", wrapper.Impressions[1].ImpressionUrl);
        }

        [TestMethod]
        public void TestInLineParsing()
        {
            var fileContent = new StreamReader(new FileStream("InLine.xml", FileMode.Open)).ReadToEnd();
            var vast = VASTParser.ParseFromXML(fileContent);
            Assert.AreEqual(1, vast.Ad.Count());
            var ad = vast.Ad[0];
            Assert.IsInstanceOfType<InLine>(ad.AdDetail);

            // TODO: Write rest of the parser tests
        }
    }
}
