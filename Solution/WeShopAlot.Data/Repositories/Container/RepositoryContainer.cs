namespace WeShopAlot.Data.Repositories.Container
{
    public class RepositoryContainer
    {
        #region Member Variables

        protected WeShopAlotContext dbContext;

        #endregion Member Variables

        #region Repositories

        public AddressRepository AddressRepository { get; set; }

        #endregion Repositories

        #region Constructors

        public RepositoryContainer(WeShopAlotContext dbContext)
        {
            this.dbContext = dbContext;
            InitializeRepositories();
        }

        #endregion Constructors

        #region Initialization

        private void InitializeRepositories()
        {
            AddressRepository = new AddressRepository(dbContext);
        }

        #endregion Initialization
    }
}
