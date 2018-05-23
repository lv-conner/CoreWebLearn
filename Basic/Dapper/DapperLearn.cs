using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Data.Common;
using System.Data.SqlClient;
namespace Basic
{
    public class DapperLearn
    {
        public static void Learn()
        {
            using (var conn = new SqlConnection("Data Source=PRCNMG1L0311;initial catalog=LFEDISystemInfo; Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True"))
            {
                var list =  conn.Query<RM>("SELECT [RMiden],[Miden],[isEnable] FROM [dbo].[RoleMenu] where [isEnable]=@isEnable", new { isEnable = 1 });
            }
        }
    }


    public class RM
    {
        public string RIden { get; set; }
        public string MIden { get; set; }
        public bool IsEnable { get; set; }
    }
}
