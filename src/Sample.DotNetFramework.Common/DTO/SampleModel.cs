using Framework.Core.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sample.DotNetFramework.Common.DTO
{
    [CrudOperation("RUD")]
    [Table("Sample")]
    public class SampleModel
    {
        [Key]
        [Column("SampleId")]
        public int SampleId { get; set; }

        [Column("SampleName")]
        public string SampleName { get; set; }

        [Column("Status")]
        public int Status{ get; set; }
    }
}
