using WebApiBaseLibrary.DataAccess.Entities;

namespace SingleSignOn.DataAccess.Entities
{
    public class Account : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}