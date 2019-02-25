using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2
{
    class Properties
    {
        private static int CurrentId = 1;
        public int Id;
        internal string Title;
        public int Price;
        internal string Address;
        public string PropertyType;
        public bool Sold;

        public void Creat(string T, string Ad, string Pt)
        {
            Id = CurrentId;
            CurrentId += 1;
            Title = T;
            Address = Ad;
            PropertyType = Pt;
            Sold = false;
        }
        public void Correct()
        {
            CurrentId -= 1;
        }
        public void UpdateTitle(string T)
        {
            Title = T;
            Console.WriteLine("Title Updated:");
            this.write();
        }
        public void write()
        {
            Console.WriteLine(Id + "\t" + PropertyType + "\t" + Title + "\t" + Address + "\t" + Price);

        }
        public bool Sell()
        {
            try
            {
                if (Sold == true)
                {
                    throw new ArgumentException("Unable to complete purchase, property with ID " + Id + " has already been sold \n");
                }
                else
                {
                    Sold = true;
                    return true;
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Unable to complete purchase, property with ID " + Id + " has already been sold \n");
                return false;
            }

        }
    }


    class Appartment : Properties
    {
        private int NbOfRooms;


        public Appartment(string T, string Ad, int Nbr)
        {
            Creat(T, Ad, "Appartment");
            NbOfRooms = Nbr;
            Price = Nbr * 15000;
        }
        public void Add()
        {
            SqlConnection conn = new SqlConnection(
            "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"insert into Apartments (Id, Title, Address, NbrOfRooms,Price) values ('" + Id + "','" + Title + "','" + Address + "','" + NbOfRooms + "','" + Price + "')", conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();

                }
            }
        }
        public string GetAp()
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;
            string output = "";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Apartments", conn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    output += "||" + rdr[0] + "  " + rdr[1] + "  " + rdr[2] + "  " + rdr[3] + "  " + rdr[4] + "||";

                }
                Correct();
                return output;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public string GetAp(int H, int L, string Ad, int Nr)
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;
            string output = "";
            List<Appartment> Ap = new List<Appartment>();


            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Apartments", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Appartment P = new Appartment(Convert.ToString(rdr[1]), Convert.ToString(rdr[2]), Convert.ToInt32(rdr[3]));
                    P.Id = Convert.ToInt32(rdr[0]);
                    if (P.Price < H && P.Price > L)
                        Ap.Add(P);
                    Correct();


                }
                List<Appartment> SortedList = Ap.OrderBy(o => o.Price).ToList();
                Ap.Clear();
                foreach (Appartment P in SortedList)
                {
                    if (Ad != "none" && P.Address == Ad)
                        Ap.Add(P);
                    if (Nr != 0 && P.NbOfRooms == Nr)
                        Ap.Add(P);
                    if (Nr == 0 && Ad == "none")
                        Ap.Add(P);
                }
                foreach (Appartment P in Ap)
                {
                    output += "||" + P.Id + "  " + P.Title + "  " + P.Address + "  " + P.NbOfRooms + "  " + P.Price + "||";
                }
                Correct();
                return output;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public string Delete(int id)
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                string deleteString = @"delete from Apartments where Id = '" + id + "'";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = deleteString;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                Correct();
                return "Done";
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        public string Update(int id,string Title)
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                string updateString = @"update Apartments set Title = '" + Title + "' where Id = '" + Id + "'";
                SqlCommand cmd = new SqlCommand(updateString);
                cmd.Connection = conn;
                Correct();
                return "Done";
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

    }
    class Buyer
    {
        public int Id;
        private string Name;
        private int Credit;
        private int Property = 0;

        public Buyer(int ID, String N, int C)
        {
            Id = ID;
            Name = N;
            Credit = C;
            Property = 0;
        }
        public delegate void PurchaseEventHandler(object source, Properties P, string N);
        public event PurchaseEventHandler PurchaseDone;

        public string Buy(int Bid, int Aid)
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Buyers where id=" + Bid + "", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Id = Convert.ToInt32(rdr[0]);
                    Name = Convert.ToString(rdr[1]);
                    Credit = Convert.ToInt32(rdr[2]);
                    Property = Convert.ToInt32(rdr[3]);
                }
                rdr.Close();
                cmd = new SqlCommand("select * from Apartments where id=" + Aid + "", conn);
                rdr = cmd.ExecuteReader();
                rdr.Read();
                if (Credit >= Convert.ToInt32(rdr[4]))
                {
                    Credit -= Convert.ToInt32(rdr[4]);
                    Property += 1;
                    string updateString = @"update Buyers set Credits = '" + Credit + "' where Id = '" + Id + "'";
                    cmd = new SqlCommand(updateString);
                    cmd.Connection = conn;
                    rdr.Close();
                    cmd.ExecuteNonQuery();
                    updateString = @"update Buyers set Prop = '" + Property + "' where Id = '" + Id + "'";
                    cmd = new SqlCommand(updateString);
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                    return "done";
                }
                return "Insufficient Fund";
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


        public string GetB()
        {
            SqlConnection conn = new SqlConnection(
                "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;
            string output = "";


            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Buyers", conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    output += "||" + rdr[0] + "   " + rdr[1] + "   " + rdr[2] + "   " + rdr[3] + "";
                }
                return output;
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }

        }
        protected virtual void OnPurchaseDone(Properties P)
        {
            if (PurchaseDone != null)
                PurchaseDone(this, P, Name);
        }
        public void Add()
        {

            SqlConnection conn = new SqlConnection(
            "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"insert into Buyers (Id,Name, Credits,Prop) values ('" + Id + "','" + Name + "','" + Credit + "','" + Property + "')", conn);
                cmd.ExecuteNonQuery();
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();

                }
            }
        }
        public string Apartment(int id)
        {
            SqlConnection conn = new SqlConnection(
                            "Data Source=(local);Initial Catalog=Project;Integrated Security=SSPI");
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("select * from Buyers", conn);
                rdr = cmd.ExecuteReader();

                rdr.Read();
                Appartment temp = new Appartment("temp", "temp", 2);
                temp.Correct();
                return temp.GetAp(Convert.ToInt32(rdr[2]),0, "none", 0);
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }

        }

    }
}