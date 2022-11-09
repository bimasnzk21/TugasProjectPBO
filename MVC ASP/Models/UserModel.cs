using NpgsqlTypes;

namespace MVC_ASP.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int Usia { get; set; }
        public string? Jenis { get; set; }
        public string? Tanggal { get; set; }


    }
}
