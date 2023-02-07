using System;

namespace Project.Domain.Aggregates.User
{
    public class User : AggregateRoot
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public User(Guid id) : base(id) { }
    }
}
