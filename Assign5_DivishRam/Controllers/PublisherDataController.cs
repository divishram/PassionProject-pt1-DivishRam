using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlogProject.Models;
using MySql.Data.MySqlClient;
using System.Web.Http.Cors;
using System.Diagnostics;

namespace BlogProject.Controllers
{
    public class PublisherDataController: ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private GameDbContext Game = new GameDbContext();
        
        //This Controller Will  access all the publishers on the database. non-deterministic.
        /// <summary>
        /// Returns a list of publishers from the system
        /// </summary>
        /// A list of publishers (name, founder, city, country)
        /// </returns>
        [HttpGet]
        [Route("api/Publisher/ListPublishers/{SearchKey?}")]
        public IEnumerable<string> ListPublishers(string SearchKey = null)
        {
            //Create an instance of a connection
            MySqlConnection Conn = Game.AccessDatabase();
            try
            {

                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Select * from Publishers where lower(publishername) like lower(@key) or lower(founder) like lower(@key) or lower(concat(publishername ' ', founder)) like lower(@key) or lower(country) like lower(@key) or lower(city)";
                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Create an empty list of Publishers
                List<String> Publishers = new List<Publishers>{ };

                //Loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int Id = Convert.ToInt32(ResultSet["id"]);
                    string PublisherName = ResultSet["publishername"].ToString();
                    string Founder = ResultSet["founder"].ToString();
                    string Country = ResultSet["country"].ToString();
                    string City = ResultSet["city"].ToString();


                    Publishers NewPublishers = new Publishers;

                    NewPublishers.Id = Id;
                    NewPublishers.PublisherName = PublisherName;
                    NewPublisher.Founder = Founder;
                    NewPublisher.Country = Country;
                    NewPublisher.City = City;

                    //Add the publisher info to the List
                    Publishers.Add(NewPublishers);
                }

            }
            //try-catch statements to debug
            catch (MySqlException ex)
            {
                //Catches error on MySql
                Debug.WriteLine(ex);
                throw new ApplicationException("DB issue", ex);
            }
            catch (Exception ex)
            {
                //catches generic issues
                Debug.WriteLine(ex);
                throw new ApplicationException("Server issue", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();
            }

            //Return the final list of game reviews
            return Publishers;
        }

        
        ///<summary>
        ///Find game review Publisher with specified id
        ///</summary>
        ///<param name="id">The id of the Publisher within the id column in the SQL file</param>
        ///<example>
        ///GET: api/GameData/FindPublisher/1
        ///</example>
        ///<returns>
        ///Nintendo
        ///</returns>
        ///<example>
        ///GET: api/GameData/FindPublisher/2
        ///<returns>
        ///Microsoft
        ///</returns>
        ///<example>
        ///GET: api/GameData/FindPublisher/3
        ///</example>
        ///<returns>
        ///Blizzard
        ///</returns>
        
        [HttpGet]
        [Route("api/GameData/FindPublisher/{id}")]
        public Publisher FindPublisher(int id)
        {
            Publisher NewPublishers = new Publisher();
            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();

                //db query
                MySql cmd = Conn.CreateCommand();

                //query
                cmd.CommandText = "Select * from Publishers where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //results in variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int Id = Convert.ToInt32(ResultSet["id"]);
                    string PublisherName = ResultSet["publishername"].ToString();
                    string Founder = ResultSet["founder"].ToString();
                    string Country = ResultSet["country"].ToString();
                    string City = ResultSet["city"].ToString();


                    Publishers NewPublishers = new Publishers;

                    NewPublishers.Id = Id;
                    NewPublishers.PublisherName = PublisherName;
                    NewPublisher.Founder = Founder;
                    NewPublisher.Country = Country;
                    NewPublisher.City = City;

                }
                //check logic to see if it validates
                if (!NewPublishers.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("The publisher not available", ex);
            }
            
            catch (MySqlException ex)
            {
                //MySql problems
                Debug.WriteLine(ex);
                throw new ApplicationException("DB issue", ex);
            }

            catch (Exception ex)
            {
                //generic issues
                Debug.WriteLine(ex);
                throw new ApplicationException("Server problem");
            }

            finally
            {
                //close connection
                Conn.Close();
            }

            return NewPublishers;

        }

        /// <summary>
        /// Deletes publishers
        /// </summary>
        /// <param name="id">The ID of the Publisher.</param>
        /// <example>POST /api/GameData/DeletePublisher/1</example>
        /// <example>POST /api/GameData/DeletePublisher/2</example>
        /// <example>POST /api/GameData/DeletePublisher/3</example>
        [HttpPost]
        public void DeletePublisher(int id)
        {
            try
            {
                //open connection btw server and db
                Conn.Open();

                //new command for db
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from Publishers where id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare()
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                //catches MySQL issues
                Debug.WriteLine(ex);
                throw new ApplicationException("Issues on db", ex);
            }

            catch (Exception ex)
            {
                //catches generic issues
                Debug.WriteLine(ex);
                throw new ApplicationException("Server issue", ex);
            }

            finally
            {
                Conn.Close();
            }


        }

        /// <summary>
        /// Adds an Publisher to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewPublisher">An object with fields that map to the columns of the publisher's table. </param>
        /// <example>
        /// POST api/GameData/AddPublisher 
        /// {
        ///	"Publisher Name": "Insomnaic Games",
        ///	"Founder": "Ted Price,
        ///	"Country": "US"
        /// }
        /// </example>
        [HttpPost]
        public void AddPublisher([FromBody] Publisher NewPublishers)
        {
            //check logic to see if fields are valid
            if (!NewPublishers.IsValid()) throw new ApplicationException("Invalid fields entered");

            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();

                 //SQL QUERY
                cmd.CommandText = "insert into Publishers (publishername, found, country) values (@publishername, @founder, @country)";
                cmd.Parameters.AddWithValue("@title", NewPublishers.PublisherName);
                cmd.Parameters.AddWithValue("@rating", NewPublishers.Founder);
                cmd.Parameters.AddWithValue("@summary", NewPublishers.Country);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Conn.Close();
            }
                catch (MySqlException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("Db issue.", ex);
            }
            catch (Exception ex)
            {
                
                Debug.Write(ex);
                throw new ApplicationException("Server issue.", ex);
            }
            finally
            {
                //Close the connection between the MySQL Database and the WebServer
                Conn.Close();
            }
        }

        /// <summary>
        /// Updates a Publisher on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="PublisherInfo">An object with fields that map to the columns of the Publisher's table.</param>
        /// <example>
        /// POST api/PublisherData/UpdatePublisher/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"Country":"CA"
        /// }
        /// </example>
        [HttpPost]
        public void UpdatePublisher(int id, [FromBody] Publisher PublisherInfo)
        {
              //Exit method if model fields are not included.
            if (!PublisherInfo.IsValid()) throw new ApplicationException("Invalid logic");

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Publishers SET publishername=@publishername, founder=@founder WHERE id=@id";
                cmd.Parameters.AddWithValue("@title", PublisherInfo.PublisherName);
                cmd.Parameters.AddWithValue("@rating", PublisherInfo.Founder);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }

            catch (MySqlException ex)
            {
                
                Debug.WriteLine(ex);
                throw new ApplicationException("DB Issue.", ex);
            }
            catch (Exception ex)
            {
                
                Debug.Write(ex);
                throw new ApplicationException("Issue with server", ex);
            }
            finally
            {

                Conn.Close();

            }
        }
    }
}
