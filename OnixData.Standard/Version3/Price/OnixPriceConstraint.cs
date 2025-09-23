namespace OnixData.Standard.Version3.Price
{
    public partial class OnixPriceConstraint
    {
        #region CONSTANTS

        public const int CONST_PRICE_CONSTRAINT_TYPE_NOTHING = 0;
        public const int CONST_PRICE_CONSTRAINT_TYPE_PREVIEW = 1;
        public const int CONST_PRICE_CONSTRAINT_TYPE_PRINT = 2;
        public const int CONST_PRICE_CONSTRAINT_TYPE_COPY_PASTE = 3;
        public const int CONST_PRICE_CONSTRAINT_TYPE_LEND = 6;
        public const int CONST_PRICE_CONSTRAINT_TYPE_TIME_LIMITED_LICENSE = 7;
        public const int CONST_PRICE_CONSTRAINT_TYPE_LIBRARY_LOAN_RENEWAL = 8;
        public const int CONST_PRICE_CONSTRAINT_TYPE_MULTI_USER_LICENSE = 9;
        public const int CONST_PRICE_CONSTRAINT_TYPE_PREVIEW_ON_PREMISES = 10;
        public const int CONST_PRICE_CONSTRAINT_TYPE_TEXT_AND_DATA_MINING = 11;
        public const int CONST_PRICE_CONSTRAINT_TYPE_LIBRARY_LOAN = 16;

        public const int CONST_PRICE_CONSTRAINT_USAGE_STATUS_UNLIMITED = 1;
        public const int CONST_PRICE_CONSTRAINT_USAGE_STATUS_SUBJECT_TO_LIMIT = 2;
        public const int CONST_PRICE_CONSTRAINT_USAGE_STATUS_PROHIBITED = 3;

        #endregion

        #region Reference Tags

        public int PriceConstraintType { get; set; }

        public int PriceConstraintStatus { get; set; }

        #endregion

        #region Short Tags

        public int x529
        {
            get => PriceConstraintType;
            set => PriceConstraintType = value;
        }

        public int x530
        {
            get => PriceConstraintStatus;
            set => PriceConstraintStatus = value;
        }

        #endregion
    }
}
