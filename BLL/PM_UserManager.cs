namespace BLL
{
    using DAL;
    using System;
    using System.Data;

    public class PM_UserManager
    {
        private static BLL.PM_UserManager _instance = null;

        private PM_UserManager()
        {
        }

        public string GetUserIdByUserName(string userName)
        {
            return SqlHelper.GetScalar(string.Format(" select USER_ID from  [dbo].[PM_USER] where USER_NAME='{0}' ", userName));
        }

        public string GetUserNames()
        {
            string str = string.Empty;
            string str2 = "  select distinct USER_NAME from  [dbo].[PM_USER]  ";
            DataTable table = SqlHelper.GetTable(str2);
            if ((table != null) && (table.Rows.Count > 0))
            {
                foreach (DataRow row in table.Rows)
                {
                    str = str + row["USER_NAME"] + ",";
                }
            }
            return str.TrimEnd(new char[] { ',' });
        }

        public static BLL.PM_UserManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BLL.PM_UserManager();
                }
                return _instance;
            }
        }
    }
}
