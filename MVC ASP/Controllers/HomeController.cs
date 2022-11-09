using Microsoft.AspNetCore.Mvc;
using MVC_ASP.Models;
using System.Diagnostics;
using Npgsql;
using System.Data;

namespace MVC_ASP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Helper helper;
        // Object DataSet
        DataSet ds;
        // Object List of NpgsqlParameter
        NpgsqlParameter[] param;
        // Query ke Database
        string query;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            // Inisialisasi Object
            helper = new Helper();
            ds = new DataSet();
            param = new NpgsqlParameter[] { };
            query = "";
        }

        public IActionResult Process()
        {
            // Reinisialiasi ds dan param agar dataset dan parameter nya kembali null
            ds = new DataSet();
            param = new NpgsqlParameter[] { };

            // Query Select
            query = "SELECT * FROM pengguna.\"Pelanggan\"";
            // Panggil DBConn untuk eksekusi Query
            helper.DBConn(ref ds, query, param);

            // List of User untuk menampung data user
            List<UserModel> users = new List<UserModel>();
            // Mengambil value dari tabel di index 0
            var data = ds.Tables[0];

            // Perulangan untuk mengambil instance tiap baris dari tabel
            foreach (DataRow u in data.Rows)
            {
                // Membuat object User baru
                UserModel user = new UserModel();
                // Mengisi id dan username dari object user dengan nilai dari database
                user.Id = u.Field<Int32>(data.Columns[0]);
                user.Username = u.Field<string>(data.Columns[1]);
                user.Usia = u.Field<Int32>(data.Columns[2]);
                user.Jenis = u.Field<string>(data.Columns[3]);
                user.Tanggal = u.Field<string>(data.Columns[4]);
                // Menambahkan user ke users (List of User)
                users.Add(user);
            }

            ViewData["data"] = users;

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Insert()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int Id, string Username)
        {
            return View();
        }


        public void GetUserById(int id)
        {
            ds = new DataSet();
            param = new NpgsqlParameter[] { 
            // Parameter untuk id
            new NpgsqlParameter("@id_pelanggan", id)
        };

            query = "SELECT * FROM pengguna.\"Pelanggan\" WHERE id = @id_pelanggan;";
            helper.DBConn(ref ds, query, param);


            List<UserModel> users = new List<UserModel>();

            var data = ds.Tables[0];

            foreach (DataRow u in data.Rows)
            {
                UserModel user = new UserModel();
                user.Id = u.Field<Int32>(data.Columns[0]);
                user.Username = u.Field<string>(data.Columns[1]);
                user.Usia = u.Field<Int32>(data.Columns[2]);
                user.Jenis = u.Field<string>(data.Columns[3]);
                user.Tanggal = u.Field<string>(data.Columns[4]);
                users.Add(user);
            }

            foreach (UserModel user in users)
            {
                Console.WriteLine($"ID: {user.Id} -- Username: {user.Username}");
            }
        }

        public IActionResult InsertUser(UserModel user)
        {
            ds = new DataSet();
            param = new NpgsqlParameter[] {
            // Parameter untuk id dan username
            new NpgsqlParameter("@id_pelanggan", user.Id),
            new NpgsqlParameter("@nama_pelanggan", user.Username),
            new NpgsqlParameter("@usia", user.Usia),
            new NpgsqlParameter("@jenis_perawatan", user.Jenis),
            new NpgsqlParameter("@tanggal_perawatan", user.Tanggal),
        };

            query = "INSERT INTO pengguna.\"Pelanggan\" VALUES (@id_pelanggan, @nama_pelanggan, @usia, @jenis_perawatan, @tanggal_perawatan);";
            helper.DBConn(ref ds, query, param);

            return RedirectToAction("Index");
        }

        public IActionResult UpdateUser(UserModel user)
        {
            ds = new DataSet();
            param = new NpgsqlParameter[] {
            new NpgsqlParameter("@id_pelanggan", user.Id),
            new NpgsqlParameter("@nama_pelanggan", user.Username),
            new NpgsqlParameter("@usia", user.Usia),
            new NpgsqlParameter("@jenis_perawatan", user.Jenis),
            new NpgsqlParameter("@tanggal_perawatan", user.Tanggal),
        };

            query = "UPDATE pengguna.\"Pelanggan\" SET nama_pelanggan = @nama_pelanggan, usia = @usia, jenis_perawatan = @jenis_perawatan, tanggal_perawatan = @tanggal_perawatan WHERE id_pelanggan = @id_pelanggan;";
            helper.DBConn(ref ds, query, param);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteUser(UserModel user)
        {
            ds = new DataSet();
            param = new NpgsqlParameter[] {
            new NpgsqlParameter("@id_pelanggan", user.Id)
        };

            query = "DELETE FROM pengguna.\"Pelanggan\" WHERE id_pelanggan = @id_pelanggan;";
            helper.DBConn(ref ds, query, param);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // Helper untuk koneksi ke DB
    class Helper
        {
            public void DBConn(ref DataSet ds, string query, NpgsqlParameter[] param)
            {
                // Data Source Name berisi credential dari database
                string dsn = "Host=localhost;Username=postgres;Password=Bima12345;Database=PBO;Port=5432";
                // Membuat koneksi ke db
                var conn = new NpgsqlConnection(dsn);
                // Command untuk eksekusi query
                var cmd = new NpgsqlCommand(query, conn);

                try
                {
                    // Perulangan untuk menyisipkan nilai yang ada pada parameter ke query
                    foreach (var p in param)
                    {
                        cmd.Parameters.Add(p);
                    }
                    // Membuka koneksi ke database
                    cmd.Connection!.Open();
                    // Mengisi ds dengan data yang didapatkan dari database
                    new NpgsqlDataAdapter(cmd).Fill(ds);
                    Console.WriteLine("Query berhasil dieksekusi");
                }
                catch (NpgsqlException e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    // Menutup koneksi ke database
                    cmd.Connection!.Close();
                }

            }
        }
    }






