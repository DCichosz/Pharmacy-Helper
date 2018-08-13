using System;
using System.Data.SqlClient;
using System.Reflection.Metadata.Ecma335;

namespace Pharmacy
{
    public class Order : ActiveRecord
    {
        public override int ID { get; set; }
        public int? PrescriptionID { get; set; }
        public int MedicineID { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }

        public Order()
        {

        }

        public Order(int? prescriptionId, int medicineId, DateTime date, int amount)
        {
            PrescriptionID = prescriptionId;
            MedicineID = medicineId;
            Date = DateTime.Now;
            Amount = amount;
        }

        public Order(int? prescriptionId, int medicineId, int amount)
        {
            PrescriptionID = prescriptionId;
            MedicineID = medicineId;
            Amount = amount;
        }

        /// <summary>
        /// Check amount from orders
        /// </summary>
        /// <param name="id"> Order's id </param>
        /// <returns></returns>
        static int CheckOrderAmount(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT Amount FROM Orders WHERE ID=@id";
                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };
                    sqlCommand.Parameters.Add(sqlIdParam);
                    int amount = (int)sqlCommand.ExecuteScalar();
                    return amount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// Check medicine's id from order
        /// </summary>
        /// <param name="id"> Order's id </param>
        /// <returns> Medicine's id </returns>
        static int CheckMedId(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT MedicineID FROM Orders WHERE ID=@id";
                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };
                    sqlCommand.Parameters.Add(sqlIdParam);
                    int medID = (int)sqlCommand.ExecuteScalar();
                    return medID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static int CheckMedicineStock(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT Amount FROM Medicines WHERE MedicineID=@id";
                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };
                    sqlCommand.Parameters.Add(sqlIdParam);
                    int stock = (int)sqlCommand.ExecuteScalar();
                    return stock;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// Save order with prescription in Database
        /// </summary>
        public override void Save()
        {
            do
            {
                try
                {
                    Console.WriteLine("Prescription ID: ");
                    int presID = Int32.Parse(Console.ReadLine());
                    Console.Clear();
                    ShowPrescription(presID);
                    Console.WriteLine("MedicineID: ");
                    int medID = Int32.Parse(Console.ReadLine());
                    Console.WriteLine($"Avaiable stock: {CheckMedicineStock(medID)}");
                    Console.WriteLine("Amount: ");
                    int amount = Int32.Parse(Console.ReadLine());
                    if (amount > CheckMedicineStock(medID))
                    {
                        throw new OutOfMemoryException("Order could not be realised becuase the amount is to high.");
                    }
                    Order ord = new Order(presID, medID, Date, amount);


                    using (var connection = ActiveRecord.Open())
                    {
                        var sqlCommand = new SqlCommand();
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText =
                            @"INSERT INTO Orders (PrescriptionID, MedicineID, Date, Amount)
			                             VALUES (@PrescriptionID, @MedicineID, @Date, @Amount); 
                            UPDATE Medicines SET Amount = Amount - @Amount WHERE MedicineID = @MedicineID;";

                        var sqlPrescriptionIDParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = ord.PrescriptionID,
                            ParameterName = "@PrescriptionID"
                        };

                        var sqlMedicineIdParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = ord.MedicineID,
                            ParameterName = "@MedicineID"
                        };

                        var sqlDateParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.DateTime,
                            Value = ord.Date,
                            ParameterName = "@Date"
                        };

                        var sqlAmountParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = ord.Amount,
                            ParameterName = "@Amount"
                        };

                        sqlCommand.Parameters.Add(sqlPrescriptionIDParam);
                        sqlCommand.Parameters.Add(sqlMedicineIdParam);
                        sqlCommand.Parameters.Add(sqlDateParam);
                        sqlCommand.Parameters.Add(sqlAmountParam);
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
        /// Display prescription from Database
        /// </summary>
        /// <param name="id"> Prescription's id </param>
        public void ShowPrescription(int id)
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT * FROM Prescriptions WHERE PrescriptionID = @id ";


                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };
                    sqlCommand.Parameters.Add(sqlIdParam);
                    var data = sqlCommand.ExecuteReader();

                    while (data.HasRows && data.Read())
                    {
                        Console.WriteLine(

                            $"Prescription ID: {data["PrescriptionID"].ToString().PadRight(4)} " +
                            $"| Customer Name: {data["CustomerName"].ToString().PadRight(10)} " +
                            $"| PESEL: {data["PESEL"]}");
                    }
                }
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Save Order without prescription in Database
        /// </summary>
        public void SaveWithoutPrescription()
        {
            do
            {
                try
                {
                    Console.WriteLine("MedicineID: ");
                    int medID = Int32.Parse(Console.ReadLine());
                    Console.WriteLine($"Avaiable stock: {CheckMedicineStock(medID)}");
                    Console.WriteLine("Amount: ");
                    int amount = Int32.Parse(Console.ReadLine());
                    if (amount > CheckMedicineStock(medID))
                    {
                        throw new OutOfMemoryException("Order could not be realised becuase the amount is to high.");
                    }
                    Order ord = new Order(null, medID, Date, amount);


                    using (var connection = ActiveRecord.Open())
                    {
                        var sqlCommand = new SqlCommand();
                        sqlCommand.Connection = connection;
                        sqlCommand.CommandText =
                            @"INSERT INTO Orders (MedicineID, Date, Amount)
			                             VALUES (@MedicineID, @Date, @Amount);
                            UPDATE Medicines SET Amount = Amount - @Amount WHERE MedicineID = @MedicineID;";

                        var sqlMedicineIdParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = ord.MedicineID,
                            ParameterName = "@MedicineID"
                        };

                        var sqlDateParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.DateTime,
                            Value = ord.Date,
                            ParameterName = "@Date"
                        };

                        var sqlAmountParam = new SqlParameter
                        {
                            DbType = System.Data.DbType.Int32,
                            Value = ord.Amount,
                            ParameterName = "@Amount"
                        };

                        sqlCommand.Parameters.Add(sqlMedicineIdParam);
                        sqlCommand.Parameters.Add(sqlDateParam);
                        sqlCommand.Parameters.Add(sqlAmountParam);
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
        /// Remove order from Database
        /// </summary>
        /// <param name="id"> Order's id </param>
        public override void Remove(int id)
        {
            int medId = CheckMedId(id);
            int amount = CheckMedicineStock(medId);
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText =
                        @"DELETE FROM Orders WHERE ID = @id;
                        UPDATE Medicines SET Amount = Amount + @Am WHERE MedicineID = @medId;";

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };

                    var sqlAmParam = new SqlParameter()
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = amount,
                        ParameterName = "@Am"
                    };

                    var sqlmedId = new SqlParameter()
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = medId,
                        ParameterName = "@medId"
                    };

                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlAmParam);
                    sqlCommand.Parameters.Add(sqlmedId);
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
        /// Reload order in Database
        /// </summary>
        /// <param name="id"> Order's id </param>
        public override void Reload(int id)
        {
            try
            {
                var sqlCommand = new SqlCommand();
                sqlCommand.CommandText = @"UPDATE Orders SET MedicineID = @MedicineID, Amount = @Amount WHERE ID = @id";
                int? presID = null;
                Console.WriteLine("MedicineID: ");
                int medID = Int32.Parse(Console.ReadLine());

                if (new Medicine().CheckWithPrescription(medID) == true)
                {
                    Console.WriteLine("Prescription ID: ");
                    presID = Int32.Parse(Console.ReadLine());
                    sqlCommand.CommandText =
                        @"UPDATE Orders SET PrescriptionID = @PrescriptionID, MedicineID = @MedicineID, Amount = @Amount WHERE ID = @id;";
                    var sqlPrescriptionIDParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = presID,
                        ParameterName = "@PrescriptionID"
                    };
                    sqlCommand.Parameters.Add(sqlPrescriptionIDParam);
                }

                Console.WriteLine("Amount: ");
                int amount = Int32.Parse(Console.ReadLine());
                Order ord = new Order(presID, medID, amount);


                int oldAmount = CheckOrderAmount(id);
                int currentAmount = CheckMedicineStock(medID);

                using (var connection = ActiveRecord.Open())
                {
                    sqlCommand.Connection = connection;
                    var sqlMedicineIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = ord.MedicineID,
                        ParameterName = "@MedicineID"
                    };

                    var sqlIdParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = id,
                        ParameterName = "@id"
                    };

                    var sqlAmountParam = new SqlParameter
                    {
                        DbType = System.Data.DbType.Int32,
                        Value = ord.Amount,
                        ParameterName = "@Amount"
                    };
                    sqlCommand.Parameters.Add(sqlMedicineIdParam);
                    sqlCommand.Parameters.Add(sqlIdParam);
                    sqlCommand.Parameters.Add(sqlAmountParam);
                    sqlCommand.ExecuteNonQuery();
                    ConsoleEx.WriteLine("Successfully edited", ConsoleColor.Green);
                }
                int correctAmount = currentAmount + (oldAmount - amount);
                new Medicine().ChangeStock(correctAmount,medID);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Show all orders in console
        /// </summary>
        public override void ShowAll()
        {
            try
            {
                using (var connection = ActiveRecord.Open())
                {
                    var sqlCommand = new SqlCommand();
                    sqlCommand.Connection = connection;
                    sqlCommand.CommandText = @"SELECT * FROM Orders";
                    var data = sqlCommand.ExecuteReader();

                    while (data.HasRows && data.Read())
                    {
                        Console.WriteLine(
                            $"ID: {data["ID"].ToString().PadRight(4)} " +
                            $"Prescription ID: {data["PrescriptionID"].ToString().PadRight(4)} " +
                            $"Medicine ID: {data["MedicineID"].ToString().PadRight(4)} " +
                            $"| Date: {data["Date"].ToString().PadRight(20)} " +
                            $"| Amount: {data["Amount"]} ");
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
        /// Menu for orders
        /// </summary>
        public static void Choice()
        {
            ConsoleEx.WriteLine("Avaiable commands for Orders:\n1. Show all (show)\n2. Add order (add)\n3. Edit order (edit)\n4. Remove order (rem)\n5. Go to previous menu (exit)", ConsoleColor.Yellow);
            Order ord = new Order();
            string choice = Console.ReadLine();
            if (choice == "1" || choice.ToLower() == "show")
            {
                Console.Clear();
                ord.ShowAll();
            }
            else if (choice == "2" || choice.ToLower() == "add")
            {
                Console.Clear();
                ConsoleEx.WriteLine("1. Add order with prescription (add)\n2. Add order without prescription (add2)\n3. Go to previous menu (exit", ConsoleColor.Cyan);
                string add = Console.ReadLine();
                if (add == "1" || add.ToLower() == "add")
                {
                    Console.Clear();
                    ord.Save();
                }
                else if (add == "2" || add.ToLower() == "add2")
                {
                    Console.Clear();
                    ord.SaveWithoutPrescription();
                }
                else if (add == "3" || add.ToLower() == "exit")
                {
                    Console.Clear();
                    return;
                }
            }
            else if (choice == "3" || choice == "edit")
            {
                Console.Clear();
                ord.ShowAll();
                Console.Clear();
                Console.Write("Write Order's ID to ");
                ConsoleEx.Write("Edit: ", ConsoleColor.Cyan);
                int id = Int32.Parse(Console.ReadLine());
                ord.Reload(id);
            }
            else if (choice == "4" || choice.ToLower() == "remove")
            {
                Console.Clear();
                Console.Write("Write Order's ID to ");
                ConsoleEx.Write("Remove: ", ConsoleColor.Red);
                int id = Int32.Parse(Console.ReadLine());
                ord.Remove(id);
            }
            else if (choice == "5" || choice.ToLower() == "exit")
            {
                Console.Clear();
                return;
            }
        }
    }
}