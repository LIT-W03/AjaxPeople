using System.Collections.Generic;
using System.Data.SqlClient;

namespace AjaxPeople.Data
{
    public class PersonDb
    {
        private string _connectionString;

        public PersonDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Person> GetPeople()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM People";
                connection.Open();
                List<Person> result = new List<Person>();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person
                    {
                        Id = (int)reader["Id"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Age = (int)reader["Age"]
                    };
                    result.Add(person);
                }

                return result;
            }
        }

        public void AddPerson(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO People (FirstName, LastName, Age) " +
                                  "VALUES (@firstName, @lastName, @age); SELECT @@Identity";
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@age", person.Age);
                connection.Open();
                person.Id = (int)(decimal)cmd.ExecuteScalar();
            }
        }

        public void Update(Person person)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE People SET FirstName = @firstName, " +
                                  "LastName = @lastName, " +
                                  "Age = @age " +
                                  "WHERE Id = @id";
                cmd.Parameters.AddWithValue("@firstName", person.FirstName);
                cmd.Parameters.AddWithValue("@lastName", person.LastName);
                cmd.Parameters.AddWithValue("@age", person.Age);
                cmd.Parameters.AddWithValue("@id", person.Id);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int personId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM People WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", personId);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public int GetPeopleCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT Count(*) FROM People";
                connection.Open();
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}