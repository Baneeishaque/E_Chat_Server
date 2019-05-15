using System;
using System.Collections.Generic;

using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/// <summary>
/// Summary description for WebService
/// </summary>

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]

public class WebService : System.Web.Services.WebService
{
    //public static String pathip = "http://192.168.43.162/E_Chat";
    public static String pathip = "http://192.168.43.89/E_Chat";

    Db_Con db = new Db_Con();

    public WebService()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public String new_insert(String name, String phone, String photo)
    {
        String pic = "NULL", type = "user", s = "";
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();
        cmd.CommandText = "select max(user_id)from TLB_login";
        int id = db.maxid(cmd);
        cmd.CommandText = "insert into TLB_login values('" + id + "','" + name + "','" + phone + "','" + type + "')";
        cmd2.CommandText = "insert into TLB_profile values('" + id + "','" + name + "','" + phone + "','" + photo + "')";
        try
        {
            db.execute(cmd);
            db.execute(cmd2);
            s = id.ToString();
        }
        catch
        {
            s = "ERROR";
        }
        return s;
    }

    [WebMethod]
    public String chat_insert(String sid, String rid, String msg_cont, String msg_ext)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select max(chat_id)from TLB_chat";
        int id = db.maxid(cmd);
        cmd.CommandText = "insert into TLB_chat values('" + id + "','" + sid + "','" + rid + "','" + System.DateTime.Now.ToShortTimeString() + "','" + System.DateTime.Now.ToShortDateString() + "','" + msg_cont + "','" + msg_ext + "')";
        try
        {
            db.execute(cmd);
            s = s + "SUCCES";
        }
        catch
        {
            s = "ERROR";
        }
        return s;
    }

    [WebMethod]
    public String user_details()
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from TLB_profile ";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow d in dt.Rows)
            {
                s = s + d[0].ToString() + '#' + d[1].ToString() + '#' + d[2].ToString() + '#' + d[3].ToString() + '@';
            }
        }
        return s;
    }

    [WebMethod]
    public String user_id(String phone)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_id from TLB_login where user_phone='" + phone + "' ";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            s = s + dt.Rows[0][0].ToString();
        }
        return s;
    }

    [WebMethod]
    public String match_phone(String uid)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_phone from TLB_login where user_id='" + uid + "' ";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            s = s + dt.Rows[0][0].ToString();
        }
        return s;
    }

    [WebMethod]
    public String contact_number(String phone)
    {
        // string ph = phone.Trim('');
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from TLB_profile where user_phone='" + phone + "' ";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {

            string path = pathip + dt.Rows[0][3].ToString().Trim('~');

            s = dt.Rows[0][0].ToString() + '#' + dt.Rows[0][1].ToString() + '#' + dt.Rows[0][2].ToString() + '#' + path;


        }
        else
        {

            s = "error";
        }
        return s;

    }

    [WebMethod]
    public String all_user()
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_id from TLB_profile ";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {


            foreach (DataRow d in dt.Rows)
            {
                s = s + d[0].ToString() + "#";
            }




        }
        return s;
    }

    [WebMethod]
    public String user_profile(String user_id)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from TLB_profile where user_id='" + user_id + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow d in dt.Rows)
            {
                string path = pathip + d[3].ToString().Trim('~');
                s = d[0].ToString() + '#' + d[1].ToString() + '#' + d[2].ToString() + '#' + path;
            }

        }
        return s;
    }

    [WebMethod]
    public String account_delete(String phone)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_id from TLB_profile where user_phone='" + phone + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            int id = Convert.ToInt32(dt.Rows[0][0].ToString());
            cmd.CommandText = "delete from TLB_profile where user_id='" + id + "'";
            db.execute(cmd);
            cmd.CommandText = "delete from TLB_login where user_id='" + id + "'";
            db.execute(cmd);
            s = "SUCCES";
        }
        else
        {
            s = "ERROR";
        }

        return s;
    }

    [WebMethod]
    public String account_change(String old_no, String new_no)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_id from TLB_profile where user_phone='" + old_no + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            int id = Convert.ToInt32(dt.Rows[0][0].ToString());
            cmd.CommandText = "update TLB_profile set user_phone='" + new_no + "' where user_id='" + id + "'";
            db.execute(cmd);
            cmd.CommandText = "update TLB_login set user_phone='" + new_no + "' where user_id='" + id + "'";
            db.execute(cmd);
            s = "SUCCES";
        }
        else
        {
            s = "ERROR";
        }

        return s;

    }

    [WebMethod]
    public String profile_update(String user_id, String user_phone, String user_name, String user_pic)
    {
        String s = "";

        byte[] img2 = Convert.FromBase64String(user_pic);
        Image img3 = byteArrayToImage(img2);
        string fpath = Server.MapPath("~/profile/" + user_id + ".jpg");
        string fnm = "~/profile/" + user_id + ".jpg";
        img3.Save(fpath);

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from TLB_profile where user_id='" + user_id + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            cmd.CommandText = "update TLB_profile set user_phone='" + user_phone + "', user_name='" + user_name + "',user_pic='" + fnm + "' where user_id='" + user_id + "'";
            db.execute(cmd);
            cmd.CommandText = "update TLB_login set user_phone='" + user_phone + "', user_name='" + user_name + "' where user_id='" + user_id + "'";
            db.execute(cmd);
            s = "SUCCES";
        }
        else
        {
            s = "ERROR";
        }

        return s;

    }

    [WebMethod]
    public String chat_list_view(String user_id)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "SELECT  distinct c.receiver_id ,p.user_name,o.status,p.user_pic  FROM TLB_chat c,TLB_profile p,TLB_online o WHERE   p.user_id=c.receiver_id and c.receiver_id=o.user_id and c.send_id='" + user_id + "'";
        // AND (TLB_online.status = 'online')
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow d in dt.Rows)
            {
                string path = pathip + d[3].ToString().Trim('~');
                s = s + d[0].ToString() + '#' + d[1].ToString() + '#' + d[2].ToString() + '#' + path + "@";//+ '#' + d[5].ToString() + '#' + d[6].ToString() + '@';
            }

        }
        else
        {

            s = "error";
        }
        return s;
    }

    [WebMethod]
    public String chat_select_person(String uid, String toid)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select * from TLB_chat where ((send_id='" + uid + "') or (send_id='" + toid + "')) and ((receiver_id='" + toid + "') or (receiver_id='" + uid + "')) order by date desc";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow d in dt.Rows)
            {
                s = s + d[0].ToString() + '#' + d[1].ToString() + '#' + d[2].ToString() + '#' + d[3].ToString() + '#' + d[4].ToString() + '#' + d[5].ToString() + '#' + d[6].ToString() + '@';
            }
        }
        return s;
    }

    [WebMethod]
    public String profile_pic(string uid)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select user_pic from TLB_profile where user_id='" + uid + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            string path = pathip + dt.Rows[0][0].ToString().Trim('~');
            s = s + path;
        }
        return s;
    }

    [WebMethod]
    public String status(string toid)
    {
        String d, t, s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select max(date),max(time) from TLB_emotion where user_id='" + toid + "'";
        DataTable dt1 = db.getdata(cmd);
        d = dt1.Rows[0][0].ToString();
        t = dt1.Rows[0][1].ToString();
        cmd.CommandText = "select e.emotion_pic,e.emotion_label,e.time_delay,p.user_name from TLB_emotion e,TLB_profile p where p.user_id=e.user_id and e.user_id='" + toid + "'and e.date='" + d + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            string path = pathip + dt.Rows[0][0].ToString().Trim('~');
            s = s + path + '#' + dt.Rows[0][1].ToString() + '#' + dt.Rows[0][2].ToString() + '#' + dt.Rows[0][3].ToString() + '#';
        }
        s = s + d + '#' + t + '#';
        return s;
    }

    [WebMethod]
    public String set_status(String uid)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select max(user_id)from TLB_emotion";
        int id = db.maxid(cmd);
        cmd.CommandText = "insert into TLB_emotion values ('" + id + "','" + "~/Test/neutral.png" + "','" + "neutral" + "','" + System.DateTime.Now.ToShortTimeString() + "','" + System.DateTime.Now.ToShortDateString() + "','" + uid + "','" + 24 + "')";
        try
        {
            db.execute(cmd);
            s = s + "SUCCES";
        }
        catch
        {
            s = "error";
        }
        return s;
    }

    [WebMethod]
    public String update_status(String uid, String img)
    {
        String lab = "", d, t, s = "", pic = "";
        //
        byte[] img2 = Convert.FromBase64String(img);
        Image img3 = byteArrayToImage(img2);
        string fpath = Server.MapPath("~/emotion_pic/" + uid + "2" + ".jpg");
        string fnm = "~/emotion_pic/" + uid + "2" + ".jpg";
        img3.Save(fpath);

        Image imgPhoto = Image.FromFile(@"C:/inetpub/wwwroot/E_Chat/emotion_pic/" + uid + "2" + ".jpg");
        Bitmap image = ResizeImage(imgPhoto, 350, 350);
        image.Save(@"D:/EMOTDATASET/dataset/test/new.jpg");
        //copyFiles("C:/inetpub/wwwroot/E_Chat/emotion_pic","D:/EMOTDATASET/dataset/test");

        lab = emotion(fnm);

        File.Delete(@"D:\EMOTDATASET\dataset\test\new.jpg");
        // 

        if (lab == "happy")
        {
            pic = "~/Test/happy.png";
        }
        else if (lab == "sad")
        {
            pic = "~/Test/sad.png";
        }
        else if (lab == "surprise")
        {
            pic = "~/Test/surprise.png";
        }
        else if (lab == "fear")
        {
            pic = "~/Test/fear.png";
        }
        else if (lab == "anger")
        {
            pic = "~/Test/angry.png";
        }
        else if (lab == "disgust")
        {
            pic = "~/Test/disgust.png";
        }
        else if (lab == "contempt")
        {
            pic = "~/Test/contempt.png";
        }
        else
        {
            pic = "~/Test/neutral.png";
        }

        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select max(date),max(time)  from TLB_emotion where user_id='" + uid + "'";
        DataTable dt1 = db.getdata(cmd);
        d = dt1.Rows[0][0].ToString();
        t = dt1.Rows[0][1].ToString();

        cmd.CommandText = "update TLB_emotion set emotion_pic='" + pic + "', emotion_label='" + lab + "' where user_id='" + uid + "'";
        try
        {
            db.execute(cmd);
            s = s + "SUCCES";

        }
        catch
        {
            s = "error";
        }
        finally { File.Delete(@"C:/inetpub/wwwroot/E_Chat/emotion_pic/" + uid + ".jpg"); }
        return s;
    }

    [WebMethod]
    public String set_privacy(String uid, String privacy)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "update TLB_privacy set privacy='" + privacy + "' where user_id='" + uid + "'";
        try
        {
            db.execute(cmd);
            s = s + "SUCCES";
        }
        catch
        {
            s = "ERROR";
        }
        return s;
    }

    [WebMethod]
    public String select_privacy(String uid)
    {
        String s = "";
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "select privacy from TLB_privacy where user_id='" + uid + "'";
        DataTable dt = db.getdata(cmd);
        if (dt.Rows.Count > 0)
        {
            s = s + dt.Rows[0][0].ToString();
        }

        return s;
    }

    private void exealg(string nam)
    {

        ProcessStartInfo start = new ProcessStartInfo();
        start.Arguments = nam; //ok
        start.UseShellExecute = false;
        start.FileName = @"C:\Users\Shijin\Anaconda3\python.exe";
        start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
        start.RedirectStandardError = true;
        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
            }

            process.WaitForExit();
        }
    }

    [WebMethod]
    public String emotion(String img)
    {
        String s = "";
        //byte[] img2 = Convert.FromBase64String(img);
        //Image img3 = byteArrayToImage(img2);
        //string fpath = Server.MapPath("D:/EMOTDATASET/dataset/test/emo"+ ".jpg");
        //string fnm = "D:/EMOTDATASET/dataset/test/emo".jpg";
        //img3.Save(fpath);
        StreamWriter sr1 = new StreamWriter(@"D:\EMOTDATASET\input.txt");
        sr1.Write("Hello");
        sr1.Close();

        StreamWriter sr2 = new StreamWriter(@"D:\EMOTDATASET\input1.txt");
        sr2.Write("Hello");
        sr2.Close();

        while (true)
        {

            if (File.Exists(@"D:\EMOTDATASET\outp.txt"))
            {
                //  exealg(@"D:\py prjct\predict.py");
                System.Threading.Thread.Sleep(2000);
                StreamReader sr = new StreamReader(@"D:\EMOTDATASET\outp.txt");
                s = sr.ReadToEnd();
                sr.Close();

                ///////////////////////

                StreamWriter sr3 = new StreamWriter(@"D:\EMOTDATASET\outp1.txt");
                sr3.Write(s);
                sr3.Close();
                //////////////////////

                File.Delete(@"D:\EMOTDATASET\outp.txt");
                break;
            }
        }
        return s;

    }

    public void copyFiles(String s, String d)
    {


        DirectoryInfo dis = new DirectoryInfo(s);
        DirectoryInfo did = new DirectoryInfo(d);
        foreach (FileInfo fi in dis.GetFiles())
        {
            fi.CopyTo(Path.Combine(did.ToString(), fi.Name), true);
        }
        //String[] files = System.IO.Directory.GetFiles(s);
        //foreach (String file in files)
        //{
        //    System.IO.File.Copy(s, d);
        //}

    }

    public static Bitmap ResizeImage(Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }
        return destImage;
    }

    public Image byteArrayToImage(byte[] byteArrayIn)
    {

        MemoryStream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        Bitmap bmp = (Bitmap)returnImage;
        return returnImage;
    }
}