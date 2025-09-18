using OnixData.Standard.Version3;
using OnixData.Standard.Version3.Header;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace OnixData.Standard
{
    public class OnixPlusStreamParser : IDisposable, IEnumerable
    {
        #region CONSTANTS

        private const int    CONST_MSG_REFERENCE_LENGTH = 500;
        private const int    CONST_BLOCK_COUNT_SIZE     = 50000000;

        private const string CONST_ONIX_MESSAGE_REFERENCE_TAG = "ONIXMessage";
        private const string CONST_ONIX_MESSAGE_SHORT_TAG     = "ONIXmessage";

        private const string CONST_ONIX_HEADER_REFERENCE_TAG = "Header";
        private const string CONST_ONIX_HEADER_SHORT_TAG     = "header";

        #endregion

        private bool       ParserRefVerFlag  = false;
        private Stream     ParserStream      = null;

        public bool ReferenceVersion
        {
            get { return this.ParserRefVerFlag; }
        }

        public OnixPlusStreamParser(Stream OnixStream, bool ReferenceVersion)
        {
            string sOnixMsgTag = ReferenceVersion ? CONST_ONIX_MESSAGE_REFERENCE_TAG : CONST_ONIX_MESSAGE_SHORT_TAG;

            this.ParserRefVerFlag = ReferenceVersion;
            this.ParserStream = OnixStream;
        }

        static public XmlReader CreateXmlReader(Stream OnixStream)
        {
            XmlReader OnixXmlReader = null;

            OnixXmlReader = new OnixXmlTextReader(OnixStream) { DtdProcessing = DtdProcessing.Ignore };

            return OnixXmlReader;
        }

        public OnixHeader MessageHeader
        {
            get
            {
                string sOnixHdrTag = 
                    this.ParserRefVerFlag ? CONST_ONIX_HEADER_REFERENCE_TAG : CONST_ONIX_HEADER_SHORT_TAG;

                OnixHeader Header = new OnixHeader();

                using (XmlReader reader = CreateXmlReader(this.ParserStream))
                {
                    reader.MoveToContent();
                    for (int nLineCount = 0; reader.Read(); ++nLineCount)
                    {
                        if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == sOnixHdrTag))
                        {
                            string sHeaderBody = reader.ReadOuterXml();

                            Header =
                                new XmlSerializer(typeof(OnixHeader), new XmlRootAttribute(sOnixHdrTag))
                                .Deserialize(new StringReader(sHeaderBody)) as OnixHeader;

                            break;
                        }
                        else if (nLineCount > 100)
                            break;
                    }
                }

                return Header;
            }
        }

        public void Dispose()
        { }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public OnixPlusStreamEnumerator GetEnumerator()
        {
            return new OnixPlusStreamEnumerator(this, this.ParserStream);
        }

        #region Support Methods

        public bool DetectVersionReference(FileInfo OnixFileInfo)
        {
            bool bReferenceVersion = true;

            if (OnixFileInfo.Length < CONST_MSG_REFERENCE_LENGTH)
                throw new Exception("ERROR!  ONIX File is smaller than expected!");

            byte[] buffer = new byte[CONST_MSG_REFERENCE_LENGTH];
            using (FileStream fs = new FileStream(OnixFileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
            }

            string sRefMsgTag = "<" + CONST_ONIX_MESSAGE_REFERENCE_TAG;
            string sFileHead  = Encoding.Default.GetString(buffer);
            if (sFileHead.Contains(sRefMsgTag))
                bReferenceVersion = true;
            else
                bReferenceVersion = false;

            return bReferenceVersion;
        }

        #endregion
    }

    public class OnixPlusStreamEnumerator : IDisposable, IEnumerator
    {
        private OnixPlusStreamParser OnixParser = null;
        private XmlReader      OnixReader = null;
        
        private string        ProductXmlTag     = null;
        private OnixProduct   CurrentRecord     = null;
        private XmlSerializer ProductSerializer = null;

        public OnixPlusStreamEnumerator(OnixPlusStreamParser ProvidedParser, Stream OnixStream) 
        {
            this.ProductXmlTag = ProvidedParser.ReferenceVersion ? "Product" : "product";

            this.OnixParser = ProvidedParser;
            this.OnixReader = OnixPlusStreamParser.CreateXmlReader(OnixStream);

            this.OnixReader.MoveToContent();

            ProductSerializer = new XmlSerializer(typeof(OnixProduct), new XmlRootAttribute(this.ProductXmlTag));
        }

        public void Dispose()
        {
            if (this.OnixReader != null)
            {
                this.OnixReader.Close();
                this.OnixReader = null;
            }
        }

        public bool MoveNext()
        {
            bool   bResult      = false;
            string sProductBody = null;

            do
            {
                if ((this.OnixReader.NodeType == XmlNodeType.Element) && (this.OnixReader.Name == this.ProductXmlTag))
                {
                    sProductBody = this.OnixReader.ReadOuterXml();
                    break;
                }

            } while (this.OnixReader.Read());

            if (!String.IsNullOrEmpty(sProductBody))
            {
                try
                {
                    bResult = true;

                    CurrentRecord =
                        this.ProductSerializer.Deserialize(new StringReader(sProductBody)) as OnixProduct;
                }
                catch (Exception ex)
                {
                    CurrentRecord = new OnixProduct();

                    CurrentRecord.SetParsingError(ex);
                    CurrentRecord.SetInputXml(sProductBody);
                }
            }

            return bResult;
        }

        public void Reset()
        {
            return;
        }

        public object Current
        {
            get
            {
                return CurrentRecord;
            }
        }
    }
}

