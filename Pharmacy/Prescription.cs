using System;
using System.Data.SqlClient;

namespace Pharmacy
{
    public class Prescription : ActiveRecord
    {
        public string CustomerName { get; set; }
        public string PESEL { get; set; }
        public int PrescriptionNumber { get; set; }
        public override int ID { get; set; }

        public Prescription()
        {

        }

        public Prescription(string customerName, string pesel, int prescriptionNumber)
        {
            CustomerName = customerName;
            PESEL = pesel;
            PrescriptionNumber = prescriptionNumber;
        }


        /// <summary>
        /// Save prescription in Database
        /// </summary>
        public override void Save()
        {
            do
            {
                try
                {
                    Console.WriteLine("Customer Name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("PESEL: ");
                    string pesel = Console.ReadLine();
                    Console.WriteLine("Prescription number: ");
                    int presNumber = Int32.Parse(Console.ReadLine());

                    Prescription pres = new Prescription(name, pesel, presNumber);

                    using (var connection = ActiveRecord.Open())
                    {
                        var sqlCommand = new SqlCommand();
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText =
                            @"INSERT INTO Prescriptions (CustomerName, PESEL, PrescriptionNumber)
			                             VALUES (@CustomerName, @PESEL, @PrescriptionNumber);";

                        var sqlCustomerNameParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.AnsiString,
                            Value = pres.CustomerName,
                            ParameterName = "@CustomerName"
                        };

                        var sqlPeselParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.AnsiString,
                            Value = pres.PESEL,
                            ParameterName = "@PESEL"
                        };

                        var sqlPrescriptionNumberParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = pres.PrescriptionNumber,
                            ParameterName = "@PrescriptionNumber"
                        };

                        sqlCommand.Parameters.Add(sqlCustomerNameParam);
                        sqlCommand.Parameters.Add(sqlPeselParam);
                        sqlCommand.Parameters.Add(sqlPrescriptionNumberParam);

                        sqlCommand.ExecuteNonQuery();
                        ConsoleEx.WriteLine("Successfully added", ConsoleColor.Green);
                        Console.WriteLine("Do you want to add another? [yes/no]");
                        string ans = Console.ReadLine();
                        if (ans.ToLower() == "no")
                        {
                            break;
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (true);
        }


        /// <summary>
        /// Reload prescription in Database
        /// </summary>
        /// <param name="id"> Prescription's id </param>
        public override void Reload(int id)
        {
            try
            {
                Console.WriteLine("Customer Name: ");
                string name = Console.ReadLine();
                Console.WriteLine("PESEL: ");
                string pesel = Console.ReadLine();
                Console.WriteLine("Prescription number: ");
                int presNumber = Int32.Parse(Console.ReadLine());

                Prescription pres = new Prescription(name, pesel, presNumber);

                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"UPDATE Prescriptions SET CustomerName = @CustomerName, PESEL = @PESEL, PrescriptionNumber = @PrescriptionNumber WHERE PrescriptionID = @id;";

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };

                    var sqlCustomerNameParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.AnsiString,
                        Value = pres.CustomerName,
                        ParameterName = "@CustomerName"
                    };

                    var sqlPeselParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.AnsiString,
                        Value = pres.PESEL,
                        ParameterName = "@PESEL"
                    };

                    var sqlPrescriptionNumberParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = pres.PrescriptionNumber,
                        ParameterName = "@PrescriptionNumber"
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlCustomerNameParam);
                    sqlCommand.Parameters.Add(sqlPeselParam);
                    sqlCommand.Parameters.Add(sqlPrescriptionNumberParam);

                    sqlCommand.ExecuteNonQuery();
                    ConsoleEx.WriteLine("Successfully edited", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        /// <summary>
        /// Remove prescription from Database
        /// </summary>
        /// <param name="id"> Prescription's id </param>
        public override void Remove(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"DELETE FROM Prescriptions WHERE PrescriptionID = @id;";

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.ExecuteNonQuery();
                    ConsoleEx.WriteLine("Successfully removed", ConsoleColor.Green);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }


        /// <summary>
        /// Show all prescriptions in console
        /// </summary>
        public override void ShowAll()
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT * FROM Prescriptions";
                    var data = sqlCommand.ExecuteReader();

                    while (data.HasRows && data.Read())
                    {
                        Console.WriteLine(
                            $"ID: {data["PrescriptionID"].ToString().PadRight(4)} " +
                            $"| Customer Name: {data["CustomerName"].ToString().PadRight(20)} " +
                            $"| PESEL: {data["PESEL"]} " +
                            $"| Prescription Number: {data["PrescriptionNumber"]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }


        /// <summary>
        /// Menu for prescriptions
        /// </summary>
        public static void Choice()
        {
            ConsoleEx.WriteLine("Avaiable commands for Prescriptions:\n1. Show all (show)\n2. Add prescription (add)\n3. Edit presctription (edit)\n4. Remove prescription (rem)\n5. Go to previous menu (exit)",ConsoleColor.Magenta);
            Prescription pres = new Prescription();
            string choice = Console.ReadLine();
            if (choice == "1" || choice.ToLower() == "show")
            {
                Console.Clear();
                pres.ShowAll();
            }
            else if (choice == "2" || choice.ToLower() == "add")
            {
                Console.Clear();
                pres.Save();
            }
            else if (choice == "3" || choice == "edit")
            {
                Console.Clear();
                pres.ShowAll();
                Console.Write("Write Prescription's ID to ");
                ConsoleEx.Write("Edit: ",ConsoleColor.Cyan);
                int id = Int32.Parse(Console.ReadLine());
                pres.Reload(id);
            }
            else if (choice == "4" || choice.ToLower() == "remove")
            {
                Console.Clear();
                Console.Write("Write Prescripion's ID to ");
                ConsoleEx.Write("Remove: ", ConsoleColor.Red);
                int id = Int32.Parse(Console.ReadLine());
                pres.Remove(id);
            }
            else if (choice == "5" || choice.ToLower() == "exit")
            {
                Console.Clear();
                return;
            }
        }
    }
}