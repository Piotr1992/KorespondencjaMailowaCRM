using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace KorespondencjaMailowaCRM
{

    public class SecureApp
	{
        private string SQLConnectionStringW = "";
        private string Wersja = "";
        private DateTime Data;
        public string Komunikat = "SQL ... Subquery returned more than 1 value. This is not permitted when the subquery follows =, !=, <, <= , >, >=";


        public SecureApp(string SqlConnectinString, string _Wersja, DateTime _Data)
        {
            SQLConnectionStringW = SqlConnectinString;
            Wersja = _Wersja;
            Data = _Data;
        }


        public string SprawdzWersje()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = SQLConnectionStringW;
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "select sys_wartosc from cdn.SystemCDN where SYS_ID = 3";
                    comm.Connection = conn;
                    if (comm.ExecuteScalar().ToString().StartsWith(Wersja))
                        return "";      
                    else
                    {
                        Komunikat = "Nieprawidłowa wersja programu CDN XL. Mechanizm działa dla wersji " + Wersja;
                        return Komunikat;
                    }
                }
            }
            catch (Exception e)
            {
                Komunikat = "Błąd sprawdzania wersji XL-a: " + e.Message;
                return Komunikat;
            }
        }


        public string SprawdzDate()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = SQLConnectionStringW;
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "select getdate()";
                    comm.Connection = conn;
                    if (Convert.ToDateTime(comm.ExecuteScalar()) > Data)
                    {
                        Komunikat = "Wersja testowa mechanizmu dobiegła końca.";
                        return Komunikat;
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                Komunikat = "Błąd sprawdzania daty XL-a: " + e.Message;
                return Komunikat;
            }
        }


        public string SprawdzKomunkat()
        {
            int rok_sql = 0;
            int miesiac_sql = 0;
            int dzien_sql = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(SQLConnectionStringW))
                {
                    SqlCommand comm = new SqlCommand();
                    comm.Connection = conn;

                    conn.Open();

                    comm.CommandText = "select year(convert(datetime,getdate(),121))";
                    rok_sql = Convert.ToInt32(comm.ExecuteScalar());

                    comm.CommandText = "select month(convert(datetime,getdate(),121))";
                    miesiac_sql = Convert.ToInt32(comm.ExecuteScalar());

                    comm.CommandText = "select day(convert(datetime,getdate(),121))";
                    dzien_sql = Convert.ToInt32(comm.ExecuteScalar());

                    comm.CommandText = "select count(*) from cdn.filtry where FIL_ProcID=-10000 and FIL_ListaID=1 and FIL_LP=1";
                    if (Convert.ToInt32(comm.ExecuteScalar()) == 1)
                    {
                        string DATAFILTR = "";
                        string KOMUNIKATFILTR = "";

                        comm.CommandText = "select rtrim(ltrim(FIL_FiltrSQL)) from cdn.filtry where FIL_ProcID=-10000 and FIL_ListaID=1 and FIL_LP=1";
                        DATAFILTR = Convert.ToString(comm.ExecuteScalar());

                        comm.CommandText = "select rtrim(ltrim(FIL_FiltrISAM)) from cdn.filtry where FIL_ProcID=-10000 and FIL_ListaID=1 and FIL_LP=1";
                        KOMUNIKATFILTR = Convert.ToString(comm.ExecuteScalar());

                        conn.Close();

                        if (DATAFILTR == "" || KOMUNIKATFILTR == "")
                        {
                            Komunikat = "Błąd ustawień konfiguracyjnych CDN.Filtry";
                            return Komunikat;
                        }
                        else
                        {
                            char[] chars = new char[Convert.FromBase64String(DATAFILTR).Length / sizeof(char)];
                            System.Buffer.BlockCopy(Convert.FromBase64String(DATAFILTR), 0, chars, 0, Convert.FromBase64String(DATAFILTR).Length);
                            DATAFILTR = new string(chars);

                            char[] chars2 = new char[Convert.FromBase64String(KOMUNIKATFILTR).Length / sizeof(char)];
                            System.Buffer.BlockCopy(Convert.FromBase64String(KOMUNIKATFILTR), 0, chars2, 0, Convert.FromBase64String(KOMUNIKATFILTR).Length);
                            KOMUNIKATFILTR = new string(chars2);

                            DateTime data_dzisiaj = new DateTime(rok_sql, miesiac_sql, dzien_sql, 0, 0, 0);
                            DateTime data_filtr = new DateTime(Convert.ToInt32(DATAFILTR.Substring(0, 4)), Convert.ToInt32(DATAFILTR.Substring(5, 2)), Convert.ToInt32(DATAFILTR.Substring(8, 2)), 0, 0, 0);

                            if (data_dzisiaj > data_filtr)
                            {
                                Komunikat = KOMUNIKATFILTR;
                                return Komunikat;
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {
                        if (conn != null) { conn.Close(); }
                        Komunikat = "Błąd ustawień konfiguracyjnych CDN.Filtry";
                        return Komunikat;
                    }
                }
            }
            catch (Exception ee)
            {
                Komunikat = "Error: " + ee.Message;
                return Komunikat;
            }
        }


        public string SprawdzWersjeOptima()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = SQLConnectionStringW;
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "select sys_wartosc from cdn.SystemCDN where SYS_ID = 3";
                    comm.Connection = conn;
                    if (comm.ExecuteScalar().ToString().Equals(Wersja))
                        return "";
                    else
                    {
                        Komunikat = "Nieprawidłowa wersja programu OPTIMA. Mechanizm działa dla wersji " + Wersja;
                        return Komunikat;
                    }
                }
            }
            catch (Exception e)
            {
                Komunikat = "Błąd sprawdzania wersji Optimy: " + e.Message;
                return Komunikat;
            }
        }


        public string SprawdzDateOptima()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = SQLConnectionStringW;
                    conn.Open();
                    SqlCommand comm = new SqlCommand();
                    comm.CommandText = "select getdate()";
                    comm.Connection = conn;
                    if (Convert.ToDateTime(comm.ExecuteScalar()) > Data)
                    {
                        Komunikat = "Wersja testowa mechanizmu dobiegła końca.";
                        return Komunikat;
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                Komunikat = "Błąd sprawdzania daty Optimy: " + e.Message;
                return Komunikat;
            }
        }

	}
}
