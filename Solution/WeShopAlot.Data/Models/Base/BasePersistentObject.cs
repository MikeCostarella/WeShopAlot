namespace WeShopAlot.Data.Model.Base
{
    public class BasePersistentObject
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
