using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using FSharp.SpendingTracker.Api.Models;
using FSharp.SpendingTracker.Domain;
using FSharp.SpendingTracker.Data;

namespace FSharp.SpendingTracker.Api.Controllers {

    
    public abstract class BaseApiController : ApiController {

        protected readonly string ConnectionString = ConfigurationManager.ConnectionStrings["FSharp.SpendingTracker"].ConnectionString;

        protected void ExecuteInSqlConnection(Action<SqlConnection> actionToExecute) {
            using (var sqlConnection = new SqlConnection(this.ConnectionString)) {
                actionToExecute(sqlConnection);
            }
        }

        protected T ExecuteInSqlConnection<T>(Func<SqlConnection,T> actionToExecute) {
            using (var sqlConnection = new SqlConnection(this.ConnectionString)) {
                return actionToExecute(sqlConnection);
            }
        }

    }

}
