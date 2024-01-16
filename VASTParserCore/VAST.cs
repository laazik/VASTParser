using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using static System.Net.Mime.MediaTypeNames;

namespace VASTParserCore
{
    [XmlRoot(ElementName = "VAST")]
    public class VAST
    {
        #region Attributes
        /// <summary>
        /// The sequence attribute enables ad servers to serve multiple ads as an Ad Pod to be 
        /// played in sequence as indicated by sequence values.
        /// </summary>
        [XmlAttribute(AttributeName = "sequence")]
        public string Sequence { get; set; } = string.Empty;
        #endregion

        #region Mandatory elements
        #endregion

        #region Optional elements
        [XmlElement(ElementName = "Ad")]
        public List<Ad> Ad { get; set; } = new List<Ad>();

        [XmlElement(ElementName = "Error")]
        public string Error { get; set; } = string.Empty;
        #endregion
    }

    public class Ad
    {
        #region Attributes
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } = string.Empty;
        #endregion

        #region Mandatory elements
        /// <summary>
        /// Each <Ad> contains a single <InLine> element or <Wrapper> element (but never both).
        /// </summary>
        [XmlElement(ElementName = "InLine", Type = typeof(InLine))]
        [XmlElement(ElementName = "Wrapper", Type = typeof(Wrapper))]
        public object AdDetail { get; set; } = new object();
        #endregion

        #region Optional elements
        [XmlElement(ElementName = "Error")]
        public string Error { get; set; } = string.Empty;
        #endregion
    }

    public class Wrapper
    {
        #region Attributes
        /// <summary>
        /// Boolean value that identifies whether subsequent wrappers after a requested VAST response is allowed.
        /// If false, any Wrappers received(i.e.not an Inline VAST response should be ignored.Otherwise, VAST 
        /// Wrappers received should be accepted.
        /// </summary>
        [XmlAttribute(AttributeName = "followAdditionalWrappers")]
        public string FollowAdditionalWrappers { get; set; } = string.Empty;

        /// <summary>
        /// Boolean value that identifies whether multiple ads are allowed in the
        /// requested VAST response.If true, both Pods and stand-alone ads are allowed.If false, only the first
        /// stand-alone Ad (i.e.no sequence value for the Ad) in the requested VAST response is allowed.
        /// </summary>
        [XmlAttribute(AttributeName = "allowMultipleAds")]
        public string AllowMultipleAds { get; set; } = string.Empty;

        /// <summary>
        /// Boolean value that provides instruction for using an available Ad when the requested VAST response 
        /// returns no ads.If true, the video player should select from any stand-alone
        /// ads available.If false and the Wrapper represents an Ad in a Pod, the video player should move on to
        /// the next Ad in a Pod; otherwise, the video player can follow through at its own discretion where no-ad
        /// responses are concerned.
        /// </summary>
        [XmlAttribute(AttributeName = "fallbackOnNoAd")]
        public string FallbackOnNoAd { get; set; } = string.Empty;
        #endregion

        #region Mandatory elements
        /// <summary>
        /// name of the system serving the VAST Wrapper response; the attribute version can be used 
        /// to identify the VAST version used by the system.
        /// </summary>
        [XmlElement(ElementName = "AdSystem")]
        public string AdSystem { get; set; } = string.Empty;

        /// <summary>
        /// redirecting URI to the next VAST response.
        /// </summary>
        [XmlElement(ElementName = "VASTAdTagURI")]
        public string VASTAdTagURI { get; set; } = string.Empty;

        /// <summary>
        /// Contains a URI to a tracking resource that is requested when the Inline Ad is displayed.
        /// </summary>
        [XmlElement(ElementName = "Impression")]
        public List<Impression> Impressions { get; set; } = new List<Impression>();
        #endregion

        #region Optional elements
        /// <summary>
        /// URI to a tracking resource that the video player should request if for some reason the InLine ad 
        /// could not be served.
        /// </summary>
        [XmlElement(ElementName = "Error")]
        public string Error { get; set; } = string.Empty;

        // TODO: To implement
        [XmlElement(ElementName = "Creatives")]
        public XmlElement? Creatives { get; set; }

        // TODO: To implement
        [XmlElement(ElementName = "Extensions")]
        public XmlElement? Extensions { get; set; }
        #endregion
    }

    public class InLine
    {
        #region Mandatory elements
        /// <summary>
        /// Name of the ad server that returned the ad.
        /// </summary>
        [XmlElement(ElementName = "AdSystem")]
        public AdSystem AdSystem { get; set; } = new AdSystem();

        /// <summary>
        /// Common name of the ad.
        /// </summary>
        [XmlElement(ElementName = "AdTitle")]
        public string AdTitle { get; set; } = string.Empty;

        /// <summary>
        /// URI that directs the video player to a tracking resource file that the video player should request when the first frame of the ad is displayed.
        /// </summary>
        [XmlElement(ElementName = "Impression")]
        public List<Impression> Impressions { get; set; } = new List<Impression>();

        /// <summary>
        /// Container for one or more <Creative> elements
        /// </summary>
        [XmlElement(ElementName = "Creatives")]
        public Creatives Creatives { get; set; } = new Creatives();
        #endregion

        #region Optional elements
        /// <summary>
        /// 
        /// </summary>
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Name of the advertiser as defined by the ad serving party. This element can be
        /// used to prevent displaying ads with advertiser competitors.Ad serving parties and publishers should
        /// identify how to interpret values provided
        /// </summary>
        [XmlElement(ElementName = "Advertiser")]
        public string Advertiser { get; set; } = string.Empty;

        /// <summary>
        /// URI to a survey vendor that could be the survey, a tracking pixel, or anything to do with
        /// the survey.Multiple survey elements can be provided.A type attribute is available to specify the MIME
        /// type being served. For example, the attribute might be set to type=”text/javascript”. Surveys
        /// can be dynamically inserted into the VAST response as long as cross-domain issues are avoided.
        /// </summary>
        [XmlElement(ElementName = "Survey")]
        public List<Survey> Surveys { get; set; } = new List<Survey>();

        /// <summary>
        /// URI representing an error-tracking pixel; this element can occur multiple times. Errors are defined in section 2.4.2.3.
        /// </summary>
        [XmlElement(ElementName = "Error")]
        public string Error { get; set; } = string.Empty;

        [XmlElement(ElementName = "Pricing")]
        public Pricing Pricing { get; set; } = new Pricing();

        /// <summary>
        /// XML node for custom extensions, as defined by the ad server. When used, a custom
        /// element should be nested under<Extensions> to help separate custom XML elements from VAST
        /// elements.The following example includes a custom xml element within the Extensions element.
        /// </summary>
        [XmlElement(ElementName = "Extensions")]
        public Extensions Extensions { get; set; } = new Extensions();
        #endregion
    }

    public class Impression
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } = string.Empty;

        [XmlText]
        public string ImpressionUrl { get; set; } = string.Empty;
    }

    public class Survey
    {
        /// <summary>
        /// type attribute is available to specify the MIMEtype being served.
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }

    public class Pricing
    {
        [XmlAttribute(AttributeName = "model")]
        public string Model { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }

    public class AdSystem
    {
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }

    public class Creatives
    {
        [XmlElement(ElementName = "Creative")]
        public List<Creative> Creative { get; set; } = new List<Creative>();
    }

    public class Creative
    {
        [XmlElement(ElementName = "Linear")]
        public Linear Linear { get; set; } = new Linear();

        [XmlElement(ElementName = "NonLinear")]
        public XmlElement? NonLinear { get; set; }

        [XmlElement(ElementName = "CompanionAds")]
        public XmlElement? CompanionAds { get; set; }


        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "sequence")]
        public string Sequence { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "AdID")]
        public string AdID { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "apiFramework")]
        public string APIFramework { get; set; } = string.Empty;

        [XmlElement(ElementName = "CreativeExtensions")]
        public List<CreativeExtension> CreativeExtensions { get; set; } = new List<CreativeExtension>();
    }

    public class Linear
    {
        [XmlAttribute(AttributeName = "skipoffset")]
        public string SkipOffset { get; set; } = string.Empty;

        #region Mandatory elements
        [XmlElement(ElementName = "Duration")]
        public string Duration { get; set; } = string.Empty;

        [XmlElement(ElementName = "MediaFiles")]
        public MediaFiles MediaFiles { get; set; } = new MediaFiles();
        #endregion

        #region Optional elements
        [XmlElement(ElementName = "TrackingEvents")]
        public TrackingEvents TrackingEvents { get; set; } = new TrackingEvents();

        [XmlElement(ElementName = "VideoClicks")]
        public VideoClicks VideoClicks { get; set; } = new VideoClicks();

        [XmlElement(ElementName = "AdParameters")]
        public AdParameters AdParameters { get; set; } = new AdParameters();

        // TODO: Needs implementation (VAST 3.0 - 2.4.3.2)
        [XmlElement(ElementName = "Icons")]
        public XmlElement? Icons { get; set; }
        #endregion
    }

    public class CreativeExtension
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; } = string.Empty;
    }

    public class TrackingEvents
    {
        [XmlElement(ElementName = "Tracking")]
        public List<Tracking> Tracking { get; set; } = new List<Tracking>();
    }

    public class Tracking
    {
        // TODO: Add XML attribute validation (through "set" body and value validation there)
        [XmlAttribute(AttributeName = "event")]
        public string Event { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
    }

    public class VideoClicks
    {
        [XmlElement(ElementName = "ClickThrough")]
        public string ClickThrough { get; set; } = string.Empty;

        [XmlElement(ElementName = "ClickTracking")]
        public XmlElement? ClickTracking { get; set; }

        [XmlElement(ElementName = "CustomClick")]
        public XmlElement? CustomClick { get; set; }
    }

    public class AdParameters
    {
        [XmlAttribute(AttributeName = "xmlEncoded")]
        public string XmlEncoded { get; set; } = string.Empty;
        [XmlText]
        public string Data { get; set; } = string.Empty;
    }

    public class IconClicks
    {
        [XmlElement(ElementName = "IconClickThrough")]
        public string IconClickThrough { get; set; } = string.Empty;

        [XmlElement(ElementName = "IconClickTracking")]
        public string IconClickTracking { get; set; } = string.Empty;
    }

    public class MediaFiles
    {
        [XmlElement(ElementName = "MediaFile")]
        public List<MediaFile> MediaFile { get; set; } = new List<MediaFile>();
    }

    public class MediaFile
    {
        #region Mandatory elements
        [XmlAttribute(AttributeName = "delivery")]
        public string Delivery { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "width")]
        public string Width { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "height")]
        public string Height { get; set; } = string.Empty;
        #endregion

        #region Optional elements
        [XmlAttribute(AttributeName = "codec")]
        public string Codec { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "bitrate")]
        public string Bitrate { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "minBitrate")]
        public string MinBitrate { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "maxBitrate")]
        public string MaxBitrate { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "scalable")]
        public string Scalable { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "maintainAspectRatio")]
        public string MaintainAspectRatio { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "apiFramework")]
        public string ApiFramework { get; set; } = string.Empty;

        [XmlText]
        public string Value { get; set; } = string.Empty;
        #endregion
    }

    public class Extensions
    {
        [XmlElement(ElementName = "Extension")]
        public List<Extension> Extension { get; set; } = new List<Extension>();
    }

    public class Extension
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; } = string.Empty;

        // Define additional properties based on the content of the Extension element
        [XmlAnyElement]
        public List<XmlElement> ExtensionElements { get; set; } = new List<XmlElement>();
    }

}
