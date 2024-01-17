///
/// Copyright 2024 Marek Laasik, Rockit Holding OÜ
/// Licensed under EUPL. Please see the following link for details
/// https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12
///

using System.Xml.Serialization;

namespace Rockit.VASTParserCore
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
