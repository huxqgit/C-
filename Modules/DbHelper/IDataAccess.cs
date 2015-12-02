using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DbHelper
{
    public interface IDataAccess : IDisposable
    {
        IDbConnection Connection { get; }
        IDbCommand Command { get; }
        IDbDataAdapter DataAdapter { get; }

        string ConnectionString { get; set; }

        void Open();
        void Colse();        

        void ExecuteNonQuery(string sql);

        object ExecuteScalar(string sql);

        IDataReader ExecuteReader(string sql);       
    }
}
