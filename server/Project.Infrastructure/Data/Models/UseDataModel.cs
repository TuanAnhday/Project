using Microsoft.AspNetCore.Identity;
using Project.Domain.Aggregates.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Data.Models
{
    [Table("Users")]
    public sealed class UseDataModel : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsActive { get; set; }

        private UseDataModel() { }
        public UseDataModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            FullName = user.FullName;
            Dob = user.Dob;
            CreateDate = user.CreateDate;
            EditDate = user.EditDate;
            IsActive = user.IsActive;
            PhoneNumber = user.PhoneNumber;
        }

        public User ToEntity()
        {
            return new User(Id)
            {
                UserName = UserName,
                FullName = FullName,
                Dob = Dob,
                CreateDate = CreateDate,
                EditDate = EditDate,
                IsActive = IsActive,
                PhoneNumber = PhoneNumber,
            };
        }
    }
}
