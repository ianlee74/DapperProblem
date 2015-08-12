using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperProblem.Models;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;

namespace DapperProblem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            Console.WriteLine("*** Get correct results (Id != SurveyId) ***");
            var clientSurvey = p.GetCorrectResultsAsync(1).Result;
            Console.WriteLine("Id = {0}", clientSurvey.Id);
            Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);

            Console.WriteLine();
            Console.WriteLine("*** Get Incorrect results (Id == SurveyId) ***");            
            clientSurvey = p.GetIncorrectResultsAsync(1).Result;
            Console.WriteLine("Id = {0}", clientSurvey.Id);
            Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);

            Console.ReadLine();
        }

        public async Task<ClientSurvey> GetCorrectResultsAsync(int clientId)
        {
            const string sql =
                @"select *
                  from dbo.ClientSurveys cs
                  where cs.ClientId = @clientId
                    and isnull(cs.EndDate, '9999-1-1') > getdate() 
                    and isnull(cs.StartDate, '2000-1-1') < getdate()
                    and exists (select * from dbo.Surveys s where s.Id = cs.SurveyId and s.IsActive = 1);";

            using (var cnn = await OpenConnectionAsync("NoDdmConnection"))
            {
                var surveys = (await cnn.QueryAsync<ClientSurvey>(sql, new { ClientId = clientId })).ToList();
                var clientSurvey = surveys.First();
                return clientSurvey;
            }
        }

        public async Task<ClientSurvey> GetIncorrectResultsAsync(int clientId)
        {      
            const string sql =
                @"select cs.*
                  from dbo.ClientSurveys cs
                  join dbo.Surveys s on s.Id = cs.SurveyId
                  where cs.ClientId = @clientId
                    and isnull(cs.EndDate, '9999-1-1') > getdate() 
                    and isnull(cs.StartDate, '2000-1-1') < getdate() 
                    and s.IsActive = 1;";

            using (var cnn = await OpenConnectionAsync("DdmConnection"))
            {
                var surveys = (await cnn.QueryAsync<ClientSurvey>(sql, new { ClientId = clientId })).ToList();
                var clientSurvey = surveys.First();
                return clientSurvey;
            }
        }

        protected async Task<SqlConnection> OpenConnectionAsync(string connectionStringName) 
        {
            var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString); 
            await cnn.OpenAsync(); 
            return cnn; 
        } 
    }
}
