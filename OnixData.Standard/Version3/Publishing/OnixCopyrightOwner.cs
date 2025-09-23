namespace OnixData.Standard.Version3.Publishing
{
    public partial class OnixCopyrightOwner
    {
        #region Reference Tags

        public string CorporateName { get; set; }

        #endregion

        #region Short Tags

        public string b047
        {
            get { return CorporateName; }
            set { CorporateName = value; }
        }

        #endregion
    }
}
