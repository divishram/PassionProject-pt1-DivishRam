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
    public class GameDataController: ApiController
    {
        // The database context class which allows us to access our MySQL Database.
        private GameDbContext Game = new GameDbContext();
        
        //This Controller Will  access game review articles on the database. non-deterministic.
        /// <summary>
        /// Returns a list of game articles from the system
        /// </summary>
        /// <example>GET api/GameData/ListGames</example>
        /// <returns>
        /// A list of articles (first names and last names)
        /// </returns>
        [HttpGet]
        [Route("api/GameData/ListArticles/{SearchKey?}")]
        public IEnumerable<string> ListArticles(string SearchKey = null)
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
                cmd.CommandText = "Select * from Articles where lower(title) like lower(@key) or lower(rating) like lower(@key) or lower(concat(title ' ', rating)) like lower(@key) or lower(releaseyear) like lower(@key) or lower(publishername)";
                cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
                cmd.Prepare();

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Create an empty list of Articles
                List<String> Articles = new List<Articles>{ };

                //Loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int Id = Convert.ToInt32(ResultSet["id"]);
                    string Title = ResultSet["title"].ToString();
                    float Rating = Convert.ToInt32(ResultSet["rating"]);
                    int ReleaseYear = Convert.ToInt32(ResultSet["releaseyear"]);
                    string PublisherName = ResultSet["publishername"].ToString();
                    string Summary = ResultSet["summary"].ToString();
                
                    Articles NewArticles = new Articles;

                    NewArticles.Id = Id;
                    NewArticles.Title = Title;
                    NewArticles.Rating = Rating;
                    NewArticles.ReleaseYear = ReleaseYear;
                    NewArticles.PublisherName = PublisherName;
                    NewArticles.Summary = Summary;

                    //Add the article info to the List
                    Articles.Add(NewArticles);
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
            return Articles;
        }

        
        ///<summary>
        ///Find game review article with specified id
        ///</summary>
        ///<param name="id">The id of the article within the id column in the SQL file</param>
        ///<example>
        ///GET: api/GameData/FindArticle/1
        ///</example>
        ///<returns>
        ///Ratchet and Clank: Rift Apart Review
        ///</returns>
        ///<example>
        ///GET: api/GameData/FindArticle/2
        ///<returns>
        ///Call of Duty: Vanguard Review
        ///</returns>
        ///<example>
        ///GET: api/GameData/FindArticle/3
        ///</example>
        ///<returns>
        ///Far Cry 6: Review
        ///</returns>
        
        [HttpGet]
        [Route("api/GameData/FindArticle/{id}")]
        public Article FindArticle(int id)
        {
            Article NewArticles = new Article();
            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();

                //db query
                MySql cmd = Conn.CreateCommand();

                //query
                cmd.CommandText = "Select * from Articles where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                //results in variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    int Id = Convert.ToInt32(ResultSet["id"]);
                    string Title = ResultSet["title"].ToString();
                    float Rating = Convert.ToInt32(ResultSet["rating"]);
                    int ReleaseYear = Convert.ToInt32(ResultSet["releaseyear"]);
                    string PublisherName = ResultSet["publishername"].ToString();
                    string Summary = ResultSet["summary"].ToString();

                    NewArticles.Id = Id;
                    NewArticles.Title = Title;
                    NewArticles.Rating = Rating;
                    NewArticles.ReleaseYear = ReleaseYear;
                    NewArticles.PublisherName = PublisherName;
                    NewArticles.Summary = Summary;

                }
                //check logic to see if it validates
                if (!NewArticles.IsValid()) throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (HttpResponseException ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException("The game review article is not available", ex);
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

            return NewArticles;

        }

        /// <summary>
        /// Deletes game review article
        /// </summary>
        /// <param name="id">The ID of the Article.</param>
        /// <example>POST /api/GameData/DeleteArticle/1</example>
        /// <example>POST /api/GameData/DeleteArticle/2</example>
        /// <example>POST /api/GameData/DeleteArticle/3</example>
        [HttpPost]
        public void DeleteArticle(int id)
        {
            try
            {
                //open connection btw server and db
                Conn.Open();

                //new command for db
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "Delete from Articles where id=@id";
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
        /// Adds an Article to the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="NewArticle">An object with fields that map to the columns of the Article's table. </param>
        /// <example>
        /// POST api/GameData/AddArticle 
        /// {
        ///	"Title":"Spider-man: Miles Morales",
        ///	"Rating": 9,
        ///	"Summary": "Very good game to play".
        /// }
        /// </example>
        [HttpPost]
        public void AddArticle([FromBody] Article NewArticle)
        {
            //check logic to see if fields are valid
            if (!NewArticles.IsValid()) throw new ApplicationException("Invalid fields entered");

            try
            {
                Conn.Open();
                MySqlCommand cmd = Conn.CreateCommand();

                 //SQL QUERY
                cmd.CommandText = "insert into Articles (title, rating, summary) values (@title,@rating, @summary)";
                cmd.Parameters.AddWithValue("@title", NewArticles.Title);
                cmd.Parameters.AddWithValue("@rating", NewArticles.Rating);
                cmd.Parameters.AddWithValue("@summary", NewArticles.Summary);

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
        /// Updates an Article on the MySQL Database. Non-Deterministic.
        /// </summary>
        /// <param name="ArticleInfo">An object with fields that map to the columns of the Article's table.</param>
        /// <example>
        /// POST api/ArticleData/UpdateArticle/208 
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"ArticleTitle":"My Sound Adventure in Italy",
        ///	"ArticleBody":"I really enjoyed Italy. The food was amazing!",
        /// }
        /// </example>
        [HttpPost]
        public void UpdateArticle(int id, [FromBody] Article ArticleInfo)
        {
              //Exit method if model fields are not included.
            if (!ArticleInfo.IsValid()) throw new ApplicationException("Invalid logic");

            try
            {
                //Open the connection between the web server and database
                Conn.Open();

                //Establish a new command (query) for our database
                MySqlCommand cmd = Conn.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "UPDATE Articles SET title=@title, rating=@rating WHERE id=@articleid";
                cmd.Parameters.AddWithValue("@title", ArticleInfo.Title);
                cmd.Parameters.AddWithValue("@rating", ArticleInfo.Rating);
                cmd.Parameters.AddWithValue("@ArticleId", Id);
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
