namespace WeShopAlot.Data.Models.Base
{
    public class BasePersistentObject
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
