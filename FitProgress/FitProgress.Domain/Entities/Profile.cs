using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace FitProgress.Domain.Entities
{
    [Table("profiles")]
    public class Profile : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}