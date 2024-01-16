using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VASTParserCore
{
    public sealed class VASTParser
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(VAST));



        public static VAST ParseFromXML(string xml)
        {
            using(StringReader reader = new StringReader(xml))
            {
                var vast = (VAST)(_serializer.Deserialize(reader) ?? 
                    throw new VASTParserException ("Deserializing VAST XML failed."));

                return vast;
            }
        }

        public static Task<VAST> ParseFromXMLAsync(string xml)
        {
            var result = Task.Run(() =>
            {
                return ParseFromXML(xml);
            });

            return result;
        }
    }
}
