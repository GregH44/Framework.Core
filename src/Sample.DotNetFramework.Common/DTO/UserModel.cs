using Framework.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DotNetFramework.Common.DTO
{
    [CrudOperation("CUD")]
    [Table("User")]
    public class UserModel
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Account")]
        public string Account { get; set; }
    }
}
