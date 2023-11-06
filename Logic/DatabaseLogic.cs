using Microsoft.Data.Sqlite;

namespace Zeebe_sample.Logic
{
    public class DatabaseLogic
    {
        public static SqliteConnection GetDBConnection()
        {
            return new SqliteConnection("Data Source=database.db");
        }

        public static bool CheckUser(string id)
        {
                var UserExists = false;
                var connection = DatabaseLogic.GetDBConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
            SELECT name
            FROM users
            WHERE id = $id
            ";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = command.ExecuteReader())
                {
                    UserExists = reader.HasRows;
                    /* while (reader.Read())                  // может потом тут будет еще логика
                     {
                         var name = reader.GetString(0);
                     } */
                      reader.Close();
                }
                connection.Close();
                return UserExists;
        }

        public static string? CheckFlight(string city, int? date)
        {
            var connection = DatabaseLogic.GetDBConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT city, week_day, price
            FROM flights
            WHERE city = $city AND week_day = $week_day
            ";
            command.Parameters.AddWithValue("$city", city);
            command.Parameters.AddWithValue("$week_day", date);
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    string price = reader.GetString(2);  // если нашли рейс, то возвращаем цену, если не нашли, то возвращаем null
                    reader.Close();
                    return price;
                } else
                {
                    reader.Close();
                    connection.Close();
                    return null;
                }
            }
           
        }

        public static bool CheckAccount(string user_id, string price)
        {
            bool Approve = false;
            var connection = DatabaseLogic.GetDBConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
            @"
            SELECT id
            FROM accounts
            WHERE user_id = $user_id AND amount >= $price
            ";
            command.Parameters.AddWithValue("$user_id", user_id);
            command.Parameters.AddWithValue("$price", price);
            using (var reader = command.ExecuteReader())
            {
                Approve = reader.HasRows;
                reader.Close();
            }
            connection.Close();
            return Approve;
        }
    }
}
