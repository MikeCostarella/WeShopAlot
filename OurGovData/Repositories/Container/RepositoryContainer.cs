namespace MeShopAlot.Data.Repositories.Container
{
    public class RepositoryContainer
    {
        #region Member Variables

        protected MeShopAlotContext dbContext;

        #endregion Member Variables

        #region Repositories

        public AddressRepository AddressRepository { get; set; }

        #endregion Repositories

        #region Constructors

        public RepositoryContainer(MeShopAlotContext dbContext)
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
