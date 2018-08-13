using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pharmacy
{


    public class Medicine : ActiveRecord
    {
        public override int ID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public bool WithPrescription { get; set; }

        public Medicine(string name, string manufacturer, decimal price, int amount, bool withPrescription)
        {
            Name = name;
            Manufacturer = manufacturer;
            Price = price;
            Amount = amount;
            WithPrescription = withPrescription;
        }

        public Medicine()
        {

        }

        /// <summary>
        /// Save medicine to Database
        /// </summary>
        public override void Save()
        {
            do
            {
                try
                {
                    Console.WriteLine("Medicine Name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Manufacturer: ");
                    string manufacturer = Console.ReadLine();
                    Console.WriteLine("Price: ");
                    decimal price = Decimal.Parse(Console.ReadLine());
                    Console.WriteLine("Amount: ");
                    int amount = Int32.Parse(Console.ReadLine());
                    Console.WriteLine("With Prescription?(True/False):");
                    bool withPrescription = bool.Parse(Console.ReadLine());

                    Medicine med = new Medicine(name, manufacturer, price, amount, withPrescription);


                    using (var connection = ActiveRecord.Open())
                    {
                        var sqlCommand = new SqlCommand();
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText =
                            @"INSERT INTO Medicines (Name, Manufacturer, Price, Amount, WithPrescription)
			                             VALUES (@Name, @Manufacturer, @Price, @Amount, @WithPrescription);";

                        var sqlNameParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.AnsiString,
                            Value = med.Name,
                            ParameterName = "@Name"
                        };

                        var sqlManufacturerParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.AnsiString,
                            Value = med.Manufacturer,
                            ParameterName = "@Manufacturer"
                        };

                        var sqlPriceParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Decimal,
                            Value = med.Price,
                            ParameterName = "@Price"
                        };

                        var sqlAmountParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = med.Amount,
                            ParameterName = "@Amount"
                        };

                        var sqlWithPrescriptionParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Boolean,
                            Value = med.WithPrescription,
                            ParameterName = "@WithPrescription"
                        };

                        sqlCommand.Parameters.Add(sqlNameParam);
                        sqlCommand.Parameters.Add(sqlManufacturerParam);
                        sqlCommand.Parameters.Add(sqlPriceParam);
                        sqlCommand.Parameters.Add(sqlAmountParam);
                        sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

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
        /// Change medicine from Database
        /// </summary>
        /// <param name="id"> Medicine's id </param>
        public override void Reload(int id)
        {
            try
            {

                Console.WriteLine("Medicine Name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Manufacturer: ");
                string manufacturer = Console.ReadLine();
                Console.WriteLine("Price: ");
                decimal price = Decimal.Parse(Console.ReadLine());
                Console.WriteLine("Amount: ");
                int amount = Int32.Parse(Console.ReadLine());
                Console.WriteLine("With Prescription?(True/False): ");
                bool withPrescription = bool.Parse(Console.ReadLine());

                var med = new Medicine(name, manufacturer, price, amount, withPrescription);

                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"UPDATE Medicines SET Name = @Name, Manufacturer = @Manufacturer, Price = @Price, Amount = @Amount, WithPrescription = @WithPrescription
			                     WHERE MedicineID = @id;";

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };

                    var sqlNameParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.AnsiString,
                        Value = med.Name,
                        ParameterName = "@Name"
                    };

                    var sqlManufacturerParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.AnsiString,
                        Value = med.Manufacturer,
                        ParameterName = "@Manufacturer"
                    };

                    var sqlPriceParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Decimal,
                        Value = med.Price,
                        ParameterName = "@Price"
                    };

                    var sqlAmountParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = med.Amount,
                        ParameterName = "@Amount"
                    };

                    var sqlWithPrescriptionParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Boolean,
                        Value = med.WithPrescription,
                        ParameterName = "@WithPrescription"
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlNameParam);
                    sqlCommand.Parameters.Add(sqlManufacturerParam);
                    sqlCommand.Parameters.Add(sqlPriceParam);
                    sqlCommand.Parameters.Add(sqlAmountParam);
                    sqlCommand.Parameters.Add(sqlWithPrescriptionParam);

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
        /// Remove medicine from Database
        /// </summary>
        /// <param name="id"> Medicine's id </param>
        public override void Remove(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"DELETE FROM Medicines WHERE MedicineID = @id;";

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
        /// Stock change on medicine
        /// </summary>
        /// <param name="amount"> Stock amount </param>
        /// <param name="medId"> Medicine's id </param>
        public void ChangeStock(int amount, int medId)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"UPDATE Medicines SET Amount = @Amount WHERE MedicineID = @MedicineID;";

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = medId,
                        ParameterName = "@MedicineID"
                    };

                    var sqlAmountParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = amount,
                        ParameterName = "@Amount"
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlAmountParam);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Show all medicines in console
        /// </summary>
        public override void ShowAll()
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT * FROM Medicines";
                    var data = sqlCommand.ExecuteReader();

                    while (data.HasRows && data.Read())
                    {
                        Console.WriteLine(
                            $"ID: {data["MedicineID"].ToString().PadRight(4)} " +
                            $"| Name: {data["Name"].ToString().PadRight(20)} " +
                            $"| Manufacturer: {data["Manufacturer"].ToString().PadRight(20)} " +
                            $"| Price: {data["Price"].ToString().PadRight(6)} " +
                            $"| Amount: {data["Amount"].ToString().PadRight(6)} " +
                            $"| WithPrescription: {data["WithPrescription"]}");
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
        /// Check if medicine is with prescription
        /// </summary>
        /// <param name="medId"> Medicine's id </param>
        /// <returns> bool:true/false </returns>
        public bool CheckWithPrescription(int medId)
        {
            bool result = false;
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT WithPrescription FROM Medicines WHERE MedicineID=@id";
                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = medId,
                        ParameterName = "@id"
                    };
                    sqlCommand.Parameters.Add(sqlIdParam);
                    result = (bool)sqlCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Menu for medicines
        /// </summary>
        public static void Choice()
        {
            ConsoleEx.WriteLine("Avaiable commands for Medicines:\n1. Show all (show)\n2. Add medicine (add)\n3. Edit medicine (edit)\n4. Remove medicine (rem)\n5. Go to previous menu (exit)", ConsoleColor.Yellow);
            Medicine med = new Medicine();
            string choice = Console.ReadLine();
            if (choice == "1" || choice.ToLower() == "show")
            {
                Console.Clear();
                med.ShowAll();
            }
            else if (choice == "2" || choice.ToLower() == "add")
            {
                Console.Clear();
                med.Save();
            }
            else if (choice == "3" || choice == "edit")
            {
                Console.Clear();
                Console.Write("Write Medicine's ID to ");
                ConsoleEx.Write("Edit: ", ConsoleColor.Cyan);
                int id = Int32.Parse(Console.ReadLine());
                med.Reload(id);
            }
            else if (choice == "4" || choice.ToLower() == "remove")
            {
                Console.Clear();
                Console.Write("Write Medicine's ID to ");
                ConsoleEx.Write("Remove: ", ConsoleColor.Red);
                int id = Int32.Parse(Console.ReadLine());
                med.Remove(id);
            }
            else if (choice == "5" || choice.ToLower() == "exit")
            {
                Console.Clear();
                return;
            }
        }
    }
}