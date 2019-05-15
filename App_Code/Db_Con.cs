using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Class1
/// </summary>

public class Db_Con
{
    public Db_Con()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\inetpub\wwwroot\E_Chat\App_Data\e_chat.mdf;Integrated Security=True;User Instance=True");
    //Data Source=.\SQLEXPRESS;AttachDbFilename=C:\inetpub\wwwroot\E_Chat\App_Data\e_chat.mdf;Integrated Security=True;User Instance=True
    
    //C:\Users\srf\Desktop\Shijn_project_13-0519\DB\E_Chat\App_Data\e_chat.mdf
    //SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\Users\srf\Desktop\Shijn_project_13-0519\DB\E_Chat\App_Data\e_chat.mdf;Integrated Security=True;User Instance=True");

    //C:\inetpub\wwwroot\E_Chat\App_Data\e_chat.mdf
    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=C:\inetpub\wwwroot\E_Chat\App_Data\e_chat.mdf;Integrated Security=True;User Instance=True");

    public void execute(SqlCommand cmd)
    {
        cmd.Connection = con;
        try
        {
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch
        {

        }
        finally
        {
            con.Close();

        }

    }

    public DataTable getdata(SqlCommand cmd)
    {
        cmd.Connection = con;
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds.Tables[0];

    }

    public int maxid(SqlCommand cmd)
    {
        cmd.Connection = con;
        int i;
        try
        {
            con.Open();
            i = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
        }
        catch
        {
            i = 1;

        }
        finally
        {
            con.Close();
        }
        return i;
    }
}