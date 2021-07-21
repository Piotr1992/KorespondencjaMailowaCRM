
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------

//                  WERSJĘ DODATKU z 2018.2 do 2019.3 PODNOSIŁ Piotr Szyperek W DNIU 11-05-2020

//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------
//-----------------------------------------------------------------------------------------------------------



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using cdn_api;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

using System.Web;

namespace KorespondencjaMailowaCRM
{
    public partial class Form1 : Form
    {
        private List<String> mTmpDirectoris = new List<string>();
        private String mConnectionString = "";
        String mPath = "";
        int mSesja = -999;
        int mWersja = 20193;
        cdn_api.XLLoginInfo_20193 mLoginInfo = new cdn_api.XLLoginInfo_20193();
        public string mArsg;

        bool mvalue;

        public Form1(string[] arsg)
        {
            foreach (string s in arsg)
                mArsg += s;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String mBazaXML = "baza.xml";
            String mRaportXL = "Raport.xml";

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            log("");

            if (!System.IO.File.Exists(mBazaXML))
            {
                dsProperties.Settings.Rows.Clear();
                dsProperties.Settings.Rows.Add();
                dsProperties.Settings.Rows[0]["FirmaXL"] = "KrynicaVit_105";
                dsProperties.Settings.Rows[0]["LoginXL"] = "ADMIN";
                dsProperties.Settings.Rows[0]["HasloXL"] = "haslo";
                dsProperties.Settings.Rows[0]["LoginSQL"] = "sa";
                dsProperties.Settings.Rows[0]["HasloSQL"] = "sa";
                dsProperties.Settings.Rows[0]["WlaczEmail"] = "True";
                dsProperties.Settings.Rows[0]["ZamknijZadanie"] = "True";
                dsProperties.Settings.Rows[0]["EmailAdres"] = "krzysztof.jagiello@t2s.pl";
                dsProperties.Settings.Rows[0]["EmailHaslo"] = "haslo";
                dsProperties.Settings.Rows[0]["EmailSMTPSerwer"] = "win2008.nt2s.local";
                dsProperties.Settings.Rows[0]["EmailSMTPPort"] = 25;
                dsProperties.Settings.Rows[0]["EmailSMTPSSL"] = "False";
                dsProperties.Settings.Rows[0]["EmailSMTPLogin"] = "krzysztof.jagiello@t2s.pl";
                dsProperties.Settings.WriteXml(mBazaXML);
                log("Utworzono czystą bazę konfiguracyjną w postaci pliku " + mBazaXML + ".");
            }
            if (!System.IO.File.Exists(mRaportXL))
            {
                dsProperties.Raporty.Rows.Clear();
                dsProperties.Raporty.Rows.Add();
                dsProperties.Raporty.Rows[0]["GIDTyp"] = "0";
                dsProperties.Raporty.Rows[0]["TypWydruku"] = "NADCHODZĄCY TERMIN";
                dsProperties.Raporty.Rows[0]["Zrodlo"] = "0";
                dsProperties.Raporty.Rows[0]["WydrukID"] = "283";
                dsProperties.Raporty.Rows[0]["FormatID"] = "1";

                dsProperties.Raporty.Rows.Add();
                dsProperties.Raporty.Rows[1]["GIDTyp"] = "1";
                dsProperties.Raporty.Rows[1]["TypWydruku"] = "ZALEGŁY TERMIN";
                dsProperties.Raporty.Rows[1]["Zrodlo"] = "1";
                dsProperties.Raporty.Rows[1]["WydrukID"] = "30";
                dsProperties.Raporty.Rows[1]["FormatID"] = "1";

                dsProperties.Raporty.WriteXml(mRaportXL);
                log("Utworzono czystą bazę konfiguracyjną w postaci pliku " + mRaportXL + ".");
            }

            dsProperties.Clear();

            dsProperties.Raporty.ReadXml(mRaportXL);

            if (System.IO.File.Exists(mBazaXML))
            {
                dsProperties.Settings.ReadXml(mBazaXML);

                mLoginInfo.Wersja = mWersja;
                mLoginInfo.Winieta = -1;
                mLoginInfo.TrybNaprawy = 1;
                mLoginInfo.TrybWsadowy = 1;
                mLoginInfo.ProgramID = "KorespondencjaMailowaCRM " + Environment.UserName;
                mLoginInfo.OpeIdent = dsProperties.Settings.Rows[0]["LoginXL"].ToString();
                mLoginInfo.OpeHaslo = dsProperties.Settings.Rows[0]["HasloXL"].ToString();
                mLoginInfo.Baza = dsProperties.Settings.Rows[0]["FirmaXL"].ToString();
                int _Error = cdn_api.cdn_api.XLLogin(mLoginInfo, ref mSesja);
                log("Wczytano bazę konfiguracyjną i zalogowano się z komunikatem " + _Error.ToString());


                if (_Error == 0)
                {
                    cdn_api.XLPolaczenieInfo_20193 PolaczenieInfo = new XLPolaczenieInfo_20193();
                    PolaczenieInfo.Wersja = mWersja;
                    if (cdn_api.cdn_api.XLPolaczenie(PolaczenieInfo) == 0)
                    {

                        mConnectionString = "Data Source=" + PolaczenieInfo.Serwer + ";Initial Catalog=" + PolaczenieInfo.Baza + ";User Id=Hydra;Password=Hydra;";

                    }

                    string WersjaXL = "2019.3";
                    DateTime DataXL = new DateTime(2015, 1, 25);
                    SecureApp spr = new SecureApp(mConnectionString, WersjaXL, DataXL);
                    if (spr.SprawdzWersje() != "")
                    {
                        
                        log(spr.Komunikat);
                        return;

                    }                    

                    try
                    {
                        using (SqlConnection _Con = new SqlConnection(mConnectionString))
                        {
                            dataGridView1.DataSource = null;
                            dataGridView1.Columns.Clear();

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandTimeout = 3600;
                                cmd.CommandText = "cdn.T2S_KorespondencjaMailowaCRM";
                                cmd.Connection = _Con;
                                using (SqlDataAdapter _da = new SqlDataAdapter(cmd))
                                {
                                    dataGridView1.Rows.Clear();
                                    dataGridView1.Columns.Clear();
                                    DataTable _dt = new DataTable();
                                    _Con.Open();
                                    _da.Fill(_dt);
                                    _Con.Close();

                                    dataGridView1.DataSource = _dt;
                                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                                    {
                                        if (dataGridView1.Columns[i].Name != "Zazn")
                                            dataGridView1.Columns[i].ReadOnly = true;
                                    }

                                    for (int i = 0; i < dataGridView1.ColumnCount; i++)
                                    {
                                        if (dataGridView1.Columns[i].Name.Substring(0, 1) == "_")
                                            dataGridView1.Columns[i].Visible = false;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log("BŁĄD --> Błąd akcji odczytu danych z SQL:" + ex.Message);
                    }

                }
                else
                {
                    log("Błąd logowania: " + _Error.ToString());
                }

                if (mArsg == "b")
                    DrukujButton_Click(null, null);
            }
            if (mArsg == "b")
                this.Close();

        }

        private void DrukujButton_Click(object sender, EventArgs e)
        {
            Drukuj.Enabled = false;
            try
            {

                mPath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.Length - 29);

                String _Path = mPath;
                TimeSpan ts = DateTime.Now - DateTime.MinValue;
                if (_Path.Substring(_Path.Length - 1) != @"\")
                    _Path += @"\";
                _Path += (ts.Ticks).ToString();
                System.IO.Directory.CreateDirectory(_Path);

                mPath = _Path;

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    if (String.IsNullOrEmpty(mArsg) && Convert.ToBoolean(dataGridView1["Zazn", i].Value) == false)
                    {
                        continue;
                    }

                    log("Tworzenie pliku pdf: " + dataGridView1["Obiekt", i].Value.ToString());

                    System.Web.Mail.MailMessage wiadomoscWEB = null;
                    
                    try
                    {

                        string _tmp = _Wydrukuj(_Path
                            , (dataGridView1["Obiekt", i].Value.ToString() + ".pdf").Replace(@"/", "_")
                            , Convert.ToInt32(dataGridView1["GIDTyp", i].Value)
                            , dataGridView1["CKE_Kod", i].Value.ToString()
                            , dataGridView1["FiltrSQL", i].Value.ToString());                        

                        if (dsProperties.Settings.Rows[0]["WlaczEmail"].ToString() == "True" && System.IO.File.Exists(_tmp))
                        {

                            log("Wysyłanie pliku pdf: " + dataGridView1["Obiekt", i].Value.ToString() + " na adres " + dataGridView1["EMail", i].Value.ToString());

                            if (dataGridView1["EMail", i].Value.ToString() == "")
                            {
                                log("BŁĄD --> Nie znaleziono adresu email.");
                                continue;
                            }

                            wiadomoscWEB = new System.Web.Mail.MailMessage();

                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", dsProperties.Settings.Rows[0]["EmailSMTPSerwer"].ToString());
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", Convert.ToInt32(dsProperties.Settings.Rows[0]["EmailSMTPPort"]));
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", Convert.ToBoolean(dsProperties.Settings.Rows[0]["EmailSMTPSSL"]));
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", dsProperties.Settings.Rows[0]["EmailAdres"].ToString());
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", dsProperties.Settings.Rows[0]["EmailHaslo"].ToString());
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", 2);
                            wiadomoscWEB.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", 1);
                            
                            wiadomoscWEB.BodyFormat = System.Web.Mail.MailFormat.Html;
                            wiadomoscWEB.From = dsProperties.Settings.Rows[0]["EmailAdres"].ToString();
                            wiadomoscWEB.To = dataGridView1["EMail", i].Value.ToString();
                            wiadomoscWEB.Subject = dataGridView1["TematMaila", i].Value.ToString();
                            wiadomoscWEB.Body = dataGridView1["TrescMaila", i].Value.ToString();

                            System.Web.Mail.MailAttachment attachment = new System.Web.Mail.MailAttachment(_tmp);
                            wiadomoscWEB.Attachments.Add(attachment);

                            System.Web.Mail.SmtpMail.Send(wiadomoscWEB);

                            mTmpDirectoris.Add(_tmp);

                        }

                        if (dsProperties.Settings.Rows[0]["ZamknijZadanie"].ToString() == "True" && System.IO.File.Exists(_tmp))
                        {
                            log("Zamykanie zadania kontrahenta: " + dataGridView1["Knt_Akronim", i].Value.ToString());

                            _Update(Convert.ToInt32(dataGridView1["SsE_GIDNumer", i].Value), Convert.ToInt32(dataGridView1["SsE_GIDLp", i].Value));
                        }
                    }
                    catch (SmtpException exception)
                    {
                        log("BŁĄD --> Klient SMTP wysłał wyjątek: " + exception.Message);                        
                    }
                    catch (Exception exception2)
                    {
                        log("BŁĄD --> " + exception2.Message);                        
                    }
                }
            }
            catch (Exception ex)
            {
                log("BŁĄD --> " + ex.Message + " " + ex.HelpLink);
            }
        }

        private String _Wydrukuj(String pPath, String Nazwa, Int32 GIDTyp, string Typ, String FiltrSQL = "(1=1)")
        {
            cdn_api.XLWydrukInfo_20193 _w = new XLWydrukInfo_20193();
            if (mConnectionString != "")
            {
                try
                {
                    _w.Wersja = mWersja;
                    _w.FiltrSQL = FiltrSQL;
                    foreach (DataSet1.RaportyRow rr in dsProperties.Raporty)
                        if (rr.TypWydruku == Typ)
                        {
                            _w.Zrodlo = rr.Zrodlo;
                            _w.Wydruk = rr.WydrukID;
                            _w.Format = rr.FormatID;
                        }

                    _w.Urzadzenie = 2;
                    _w.DrukujDoPliku = 1;
                    _w.PlikDocelowy = pPath + @"\" + Nazwa;

                    Int32 _error = cdn_api.cdn_api.XLWykonajPodanyWydruk(_w);
                    if (_error != 0)
                        log("BŁĄD --> Błąd druku " + _error.ToString());
                }
                catch (Exception ee)
                {
                    log("BŁĄD --> " + ee.Message);
                }
            }
            return _w.PlikDocelowy;
        }

        private void _Update(Int32 pGIDNumer, Int32 pGIDLp)
        {
            try
            {
                using (SqlConnection _Con = new SqlConnection(mConnectionString))
                {
                    using (SqlCommand _Com = new SqlCommand(

                        @"UPDATE CDN.SrsElem
                        SET              
                        SsE_FlagaStatusu =1, /*1:zamkniete; 0:otwarte*/
                        SsE_OpeWNumer =(Select top 1 Ope_GIDNumer from CDN.OpeKarty Where Ope_Ident=@OpeIdent), /*operator wykonujacy zadnie*/
                        SsE_DataWykonania = DATEDIFF( ss, '01/01/1990 00:00:00',  GETDATE()), /*data wykonania*/
                        SsE_Opis = 'Zamknięte automatycznie' /*opis zadania*/
                        WHERE SsE_GIDNumer=@GIDNumer and SsE_GIDLp=@GIDLp"
                        , _Con))
                    {

                        _Com.Parameters.Clear();
                        _Com.Parameters.AddWithValue("@GIDNumer", pGIDNumer);
                        _Com.Parameters.AddWithValue("@GIDLp", pGIDLp);
                        _Com.Parameters.AddWithValue("@OpeIdent", mLoginInfo.OpeIdent);
                        _Con.Open();
                        _Com.ExecuteNonQuery();
                        _Con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log("BŁĄD --> " + ex.Message);
            }
        }

        void _KlientSmtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string userState = (string)e.UserState;

            if (e.Error != null)
            {
                log("BŁĄD --> Wystąpił błąd (przy wysyłce maila na adres " + userState.ToString() + "): " + e.Error.ToString());
            }
            else
            {
                log("Wiadomość została wysłana na adres " + userState.ToString() + ".");
            }
        }

        void log(String logMessage)
        {
            using (StreamWriter w = File.AppendText("Logi.log"))
                w.WriteLine("{0} {1}: {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), logMessage);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (dsProperties.Settings.Rows[0]["WlaczEmail"].ToString() == "True")
                {
                    foreach (string _tmp in mTmpDirectoris)
                    {
                        try
                        {
                            if (System.IO.File.Exists(_tmp))
                                System.IO.File.Delete(_tmp);
                        }
                        catch (Exception ex)
                        {
                            log("BŁĄD --> Błąd usunięcia plików tymczasowych (" + _tmp + "):" + ex.Message);
                        }
                    }
                    System.IO.Directory.Delete(mPath);
                }
                cdn_api.cdn_api.XLLogout(mSesja);
            }
            catch { }

            cdn_api.cdn_api.XLLogout(mSesja);
        }

        private void btnZaznaczAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1["Zazn", i].Value = true;
            }
        }
        private void btnOdznaczAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1["Zazn", i].Value = false;
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1)
            {
                
                mvalue = Convert.ToBoolean(dataGridView1["Zazn", e.RowIndex].Value);
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    dataGridView1.SelectedRows[i].Cells["Zazn"].Value = !mvalue;                
                }
                
            }

        }



    }
}
