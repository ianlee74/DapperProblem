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
            var clientSurvey = p.GetResultsAsync("NoDdmConnection").Result;
            Console.WriteLine("Id = {0}", clientSurvey.Id);
            Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);

            Console.WriteLine();
            Console.WriteLine("*** Get Incorrect results (Id == SurveyId) ***");
            clientSurvey = p.GetResultsAsync("DdmConnection").Result;
            Console.WriteLine("Id = {0}", clientSurvey.Id);
            Console.WriteLine("SurveyId = {0}", clientSurvey.SurveyId);

            Console.ReadLine();
        }

        public async Task<ClientSurvey> GetResultsAsync(string connectionStringName)
        {
            const string sql =
                @"select cs.*
                from dbo.ClientSurveys cs
                join dbo.Surveys s on s.Id = cs.SurveyId
                where cs.ClientId = 1
                and isnull(cs.EndDate, '9999-1-1') > getdate() 
                and isnull(cs.StartDate, '2000-1-1') < getdate() 
                and s.IsActive = 1;";

            using (var cnn = await OpenConnectionAsync(connectionStringName))
            {
                var surveys = (await cnn.QueryAsync<ClientSurvey>(sql)).ToList();
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
