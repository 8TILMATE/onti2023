using onti2024.Models;
using onti2024.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


namespace onti2024.Helpers
{
    public static class DatabaseHelper
    {
        public static UserModel userLogat = new UserModel();
        public static List<RezultateModel> RezultateModel = new List<RezultateModel>();
        public static List<RezultateModel> AllResultModels = new List<RezultateModel>();
        public static void DeleteClearDatabase()
        {
            DeleteAllDatabases();
            ClearAllDatabases();
        }
        public static void DeleteAllDatabases()
        {
            DeleteTable("Utilizatori");
            DeleteTable("Rezultate");
        }
        public static void ClearAllDatabases()
        {
            ClearDatabase("Utilizatori");
            ClearDatabase("Rezultate");
        }
        public static void DeleteTable(string Table)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Delete From " + Table, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void ClearDatabase(string Table)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("DBCC CHECKIDENT('" + Table + "',RESEED,1)", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void InsertDatabase()
        {
            InsertUtilizatori();
            InsertRezultate();
        }
        public static void DatabaseStarter()
        {
            DeleteAllDatabases();
            InsertDatabase();
        }
        public static void InsertUtilizatori()
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (StreamReader rdr = new StreamReader(Resources.utilizatoriString))
                {
                    while (rdr.Peek() != -1)
                    {
                        var line = rdr.ReadLine().Split(';');
                        using (SqlCommand cmd = new SqlCommand("Insert into Utilizatori values(@e,@n,@s)", con))
                        {
                            cmd.Parameters.AddWithValue("e", line[0]);
                            cmd.Parameters.AddWithValue("n", line[1]);
                            cmd.Parameters.AddWithValue("s", line[2]);
                            cmd.ExecuteNonQuery();

                        }
                    }
                }
            }
        }
        public static void InsertRezultate()
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (StreamReader rdr = new StreamReader(Resources.rezultateString))
                {
                    while (rdr.Peek() != -1)
                    {
                        var line = rdr.ReadLine().Split(';');
                        using (SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@t,@e,@p,@d)", con))
                        {
                            cmd.Parameters.AddWithValue("t", Int32.Parse(line[0]));
                            cmd.Parameters.AddWithValue("e", line[1]);
                            cmd.Parameters.AddWithValue("p", Int32.Parse(line[2]));
                            cmd.Parameters.AddWithValue("d", DateTime.ParseExact(line[3], "dd.MM.yyyy", CultureInfo.InvariantCulture));
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
            }
        }
        public static void InsertUser(UserModel model)
        {
            if (CheckEmail(model.email))
            {
                InsertUtilizator(model);
                WriteUtilizator(model);
            }
        }
        private static bool CheckEmail(string email)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            { 
                con.Open(); 
                using(SqlCommand cmd = new SqlCommand("Select * from Utilizatori where (EmailUtilizator=@e)",con))
                {
                    cmd.Parameters.AddWithValue("e", email);
                    using(SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            rdr.Read();
                            Console.WriteLine(rdr.GetString(0));
                            
                            return false;
                        }
                        catch
                        {
                            return true;
                        }
                    }
                }
            }
        }
        public static UserModel GetFromEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from Utilizatori where (EmailUtilizator=@e)", con))
                {
                    cmd.Parameters.AddWithValue("e", email);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            rdr.Read();
                            Console.WriteLine(rdr.GetString(0));
                            return new UserModel
                            {
                                email = rdr.GetString(0),
                                Nume = rdr.GetString(1),
                                password = rdr.GetString(2)
                            };

                        }
                        catch
                        {
                            return new UserModel();
                            
                            
                        }
                    }
                }
            }
        }

        public static  bool CheckUser(UserModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from Utilizatori where (EmailUtilizator=@e and Parola=@p)",con))
                {
                    cmd.Parameters.AddWithValue("e", model.email.Trim());
                    cmd.Parameters.AddWithValue("p", model.password.Trim());
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            rdr.Read();
                            Console.WriteLine(rdr.GetValue(0).ToString());
                            userLogat = model;
                            userLogat.password = rdr.GetString(2);
                            return true ;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }
        private static void InsertUtilizator(UserModel model)
        {
            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Utilizatori values(@e,@n,@s)", con))
                {
                    cmd.Parameters.AddWithValue("e", model.email);
                    cmd.Parameters.AddWithValue("n", model.Nume);
                    cmd.Parameters.AddWithValue("s", model.password);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        private static void WriteUtilizator(UserModel model)
        {
            string line;
            using(StreamReader rdr = new StreamReader(Resources.utilizatoriString))
            {
                line = rdr.ReadToEnd();
                rdr.Close();
            }
            line += "\n" + model.email + ';' + model.Nume + ';' + model.password;
            using(StreamWriter wrt = new StreamWriter(Resources.utilizatoriString))
            {
                
                wrt.Write(line);
                wrt.Close();
            }
            
        }
        public static void GetGames(UserModel model)
        {
            using (SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from Rezultate where EmailUtilizator=@e", con))
                {
                    cmd.Parameters.AddWithValue("e", model.email);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            while (rdr.Read())
                            {
                                RezultateModel.Add(new RezultateModel { Id = rdr.GetInt32(0), Tip = rdr.GetInt32(1), EmailUser = rdr.GetString(2), Punctaj = rdr.GetInt32(3), Data = rdr.GetDateTime(4) });
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }

        }
    
    public static void GetAllGames()
    {
        using (SqlConnection con = new SqlConnection(Resources.connectionString))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("Select * from Rezultate", con))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    try
                    {
                        while (rdr.Read())
                        {
                                 AllResultModels.Add(new RezultateModel { Id = rdr.GetInt32(0), Tip = rdr.GetInt32(1), EmailUser = rdr.GetString(2), Punctaj = rdr.GetInt32(3), Data = rdr.GetDateTime(4) });
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

    }
    public static void InsertGames(RezultateModel model)
        {

            using(SqlConnection con = new SqlConnection(Resources.connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Insert into Rezultate values(@t,@e,@p,@d)",con))
                {
                    cmd.Parameters.AddWithValue("e", model.EmailUser);
                    cmd.Parameters.AddWithValue("t", model.Tip);
                    cmd.Parameters.AddWithValue("p", model.Punctaj);
                    cmd.Parameters.AddWithValue("d", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            string line;
            using (StreamReader rdr = new StreamReader(Resources.rezultateString))
            {
                line = rdr.ReadToEnd();
                rdr.Close();
            }
            line += "\n" + model.Tip + ';' + model.EmailUser + ';' + model.Punctaj + ';'+model.Data;
            using (StreamWriter wrt = new StreamWriter(Resources.rezultateString))
            {

                wrt.Write(line);
                wrt.Close();
            }
        }
    }
    
}
