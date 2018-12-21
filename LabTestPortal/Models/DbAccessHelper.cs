using LabTestPortal.CustomClass;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LabTestPortal.Models
{
    public class DbAccessHelper
    {
        private static readonly string conStr = ConfigurationManager.AppSettings["ConnectionString"];

        public class Response
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public IEnumerable<Object> ResponseObject { get; set; }
        }

        public static List<State> GetStateList()
        {
            List<State> states = new List<State>();
            SqlDataReader sqlDataReader;
           
            using (SqlConnection conn = new SqlConnection(conStr))
            {
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = new SqlCommand("uspStatesList", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                conn.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load(sqlDataReader);
                states = dataTable.DataTableToModelList<State>();
                states.Insert(0, new State { state_id = null, state_code = null });
                conn.Close();
            }

            return states;
        }

        public static Response PersonUpsert(Person person)
        {
            Response response = new Response();
            response.IsSuccess = false;

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = new SqlCommand("uspPersonUpsert", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@person_id", person.person_id);
                sqlCommand.Parameters.AddWithValue("@first_name", person.first_name);
                sqlCommand.Parameters.AddWithValue("@last_name", person.last_name);
                sqlCommand.Parameters.AddWithValue("@state_id", person.state_id.Value);
                sqlCommand.Parameters.AddWithValue("@gender", person.gender);
                sqlCommand.Parameters.AddWithValue("@dob", person.dob);
                sqlCommand.Parameters.AddWithValue("@id", 0).Direction = ParameterDirection.Output;

                conn.Open();
                sqlCommand.ExecuteNonQuery();
                int newPersonId = (int)sqlCommand.Parameters["@id"].Value;
                conn.Close();

                if (newPersonId > 0)
                {
                    var results = PersonSearch(new Person { person_id = newPersonId });
                    response.IsSuccess = true;
                    response.ResponseObject = results;
                }
            }

            return response;
        }

        public static List<Person> PersonSearch(Person person)
        {
            List<Person> persons = new List<Person>();
            SqlDataReader sqlDataReader;

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = new SqlCommand("uspPersonSearch", conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.Parameters.AddWithValue("@person_id", person.person_id);
                sqlCommand.Parameters.AddWithValue("@first_name", person.first_name);
                sqlCommand.Parameters.AddWithValue("@last_name", person.last_name);
                sqlCommand.Parameters.AddWithValue("@state_id", person.state_id);
                sqlCommand.Parameters.AddWithValue("@gender", person.gender);
                sqlCommand.Parameters.AddWithValue("@dob", person.dob);

                conn.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                dataTable.Load(sqlDataReader);
                persons = dataTable.DataTableToModelList<Person>();
                conn.Close();
            }

            return persons;
        }
    }
}