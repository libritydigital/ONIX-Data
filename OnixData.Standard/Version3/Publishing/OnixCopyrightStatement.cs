using System.Xml.Serialization;

namespace OnixData.Standard.Version3.Publishing
{
    public partial class OnixCopyrightStatement
    {
        #region CONSTANTS

        public const string CONST_RIGHTS_TYPE_COPYRIGHT = "C";
        public const string CONST_RIGHTS_TYPE_PHONOGRAM = "P";
        public const string CONST_RIGHTS_TYPE_DATABASE = "D";

        #endregion

        #region ONIX Lists

        public OnixCopyrightOwner[] OnixCopyrightOwnerList
        {
            get
            {
                OnixCopyrightOwner[] CopyrightOwners = null;

                if (this.CopyrightOwner != null)
                    CopyrightOwners = this.CopyrightOwner;
                else
                    CopyrightOwners = new OnixCopyrightOwner[0];

                return CopyrightOwners;
            }
        }

        #endregion

        #region Reference Tags

        public string CopyrightType { get; set; }
        public string CopyrightYear { get; set; }
        public OnixCopyrightOwner[] CopyrightOwner { get; set; }

        #endregion

        #region Short Tags

        public string x512
        {
            get { return CopyrightType; }
            set { CopyrightType = value; }
        }

        public string b087
        {
            get { return CopyrightYear; }
            set { CopyrightYear = value; }
        }

        [XmlElement("copyrightowner")]
        public OnixCopyrightOwner[] copyrightowner
        {
            get { return CopyrightOwner; }
            set { CopyrightOwner = value; }
        }

        #endregion
    }
}
