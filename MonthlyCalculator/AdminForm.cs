using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonthlyCalculator
{
    public partial class AdminForm : Form
    {
        private Form LoginForm;
        private SqlConnection sqlconnection;
        private SqlCommand sqlcommand;
        private SqlDataReader SqlDR;
        private List<int> PersonsIDsList;
        private List<string> OfficersNamesList;
        private List<int> OfficersIDsList;


        public AdminForm()
        {
            InitializeComponent();
            DatabseInitialization();
            FormInitializiation();
        }


        public void FormInitializiation()
        {
            PersonsIDsList = new List<int>();
            OfficersNamesList = new List<string>();
            OfficersIDsList = new List<int>();
            UpdateMonthsYears();
        }


        public void DatabseInitialization()
        {
            sqlconnection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=MonthlyCalculator;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            sqlcommand = new SqlCommand();
            sqlcommand.Connection = sqlconnection;
            sqlconnection.Open();
        }

        public void SetLoginForm(Form Login)
        {
            LoginForm = Login;
        }

        public Form GetLoginForm()
        {
            return LoginForm;
        }


        public void AddNewMonth()
        {
            sqlcommand.CommandText = "INSERT INTO [dbo].[MONTHS] VALUES ('" + Month.Text + "','" + Year.Text + "') ";
            sqlcommand.CommandText += "INSERT INTO MEEZMONTHLY VALUES ('" + Month.Text + "','" + Year.Text + "','0','0','0','0') ";
            sqlcommand.CommandText += "INSERT INTO MEEZ VALUES ('" + Month.Text + "','" + Year.Text + "','0') ";
            sqlcommand.CommandText += "INSERT INTO UNITDISCOUNT VALUES ('" + Month.Text + "','" + Year.Text + "','0')";
            sqlcommand.ExecuteNonQuery();
            UpdateMonthsYears();
            MessageBox.Show("تمت اضافة شهر  " + Month.Text + " الى قاعدة البيانات");
        }


        public void RemoveMonth()
        {
            sqlcommand.CommandText = "delete from MEEZMONTHLY where MEEZMONTHLY.MM_MONTH = '" + Month.Text + "' and MEEZMONTHLY.MM_YEAR = '" + Year.Text + "' ";
            sqlcommand.CommandText += "delete from MEEZ where MEEZ.M1_MONTH = '" + Month.Text + "' and MEEZ.M1_YEAR = '" + Year.Text + "' ";
            sqlcommand.CommandText += "delete from UNITDISCOUNT where UNITDISCOUNT.UD_MONTH = '" + Month.Text + "' and UNITDISCOUNT.UD_YEAR = '" + Year.Text + "' ";
            sqlcommand.CommandText += "delete from MONTHS where MONTHS.MONTH = ' " + Month.Text + "' and MONTHS.YEAR = '" + Year.Text + "' ";
            sqlcommand.ExecuteNonQuery();
            MessageBox.Show("تم حذف شهر " + Month.Text + " سنة " +Year.Text + " من قاعدة البيانات");
            UpdateMonthsYears();
        }


        public void UpdateMonthsYears()
        {
            Months.Items.Clear();
            Months1.Items.Clear();
            Months2.Items.Clear();
            sqlcommand.CommandText = "select distinct Month from dbo.MONTHS";
            SqlDR = sqlcommand.ExecuteReader();
            while (SqlDR.Read())
            {
                Months.Items.Add(SqlDR[0].ToString());
                Months1.Items.Add(SqlDR[0].ToString());
                Months2.Items.Add(SqlDR[0].ToString());
            }
            SqlDR.Close();
            Years.Items.Clear();
            Years1.Items.Clear();
            Years2.Items.Clear();
            sqlcommand.CommandText = "select distinct Year from dbo.MONTHS";
            SqlDR = sqlcommand.ExecuteReader();
            while (SqlDR.Read())
            {
                Years.Items.Add(SqlDR[0].ToString());
                Years1.Items.Add(SqlDR[0].ToString());
                Years2.Items.Add(SqlDR[0].ToString());
            }
            SqlDR.Close();
        }


        public void AddToMonth()
        {
            try
            {
                String P_ID;
                sqlcommand.CommandText = "Select P_ID from PERSONS Where P_NAME = N'" + NotInList.SelectedItem.ToString() + "'";
                SqlDR = sqlcommand.ExecuteReader();
                SqlDR.Read();
                P_ID = SqlDR[0].ToString();
                SqlDR.Close();
                sqlcommand.CommandText = "INSERT INTO SALARY VALUES ('" + Months2.Text + "','" + Years2.Text + "','" + P_ID + "', '0' , '0') ";
                sqlcommand.CommandText += "INSERT INTO MARKET VALUES ('" + Months2.Text + "','" + Years2.Text + "','" + P_ID + "','0') ";
                if (Officers1.Checked)
                {
                    sqlcommand.CommandText += "INSERT INTO OFFICERSMEEZMONTHLY VALUES ('" + Months2.Text + "','" + Years2.Text + "','" + P_ID + "','0','0') ";
                    for (int i = 0; i < 40; i++)
                    {
                        sqlcommand.CommandText += "INSERT INTO OFFICERSMEEZDAILY VALUES ('" + (i + 1) + "','" + Months2.Text + "','" + Years2.Text + "','" + P_ID + "','0')";
                    }
                }
                sqlcommand.ExecuteNonQuery();
                UpDateAllPersons();
                UpdateSalary();
                UpdateMeez();
                if (NotInList.Items.Count > 0)
                {
                    NotInList.SelectedIndex = 0;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("خطأ لم يتم تحديد فرد");
            }
        }

        public void RemoveFromMonth()
        {
            try
            {
                String P_ID;
                sqlcommand.CommandText = "Select P_ID from PERSONS Where P_NAME = N'" + InList.SelectedItem.ToString() + "'";
                SqlDR = sqlcommand.ExecuteReader();
                SqlDR.Read();
                P_ID = SqlDR[0].ToString();
                SqlDR.Close();
                sqlcommand.CommandText = "delete from SALARY where SALARY.P_ID = '" + P_ID + "' and SALARY.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "'";
                sqlcommand.CommandText += "delete from MARKET where MARKET.P_ID = '" + P_ID + "' and MARKET.MAR_MONTH = '" + Months2.Text + "' and MARKET.MAR_YEAR = '" + Years2.Text + "'";
                if (Officers1.Checked)
                {
                    sqlcommand.CommandText += "delete from OFFICERSMEEZMONTHLY where OFFICERSMEEZMONTHLY.P_ID = '" + P_ID + "' and OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months2.Text + "' and OFFICERSMEEZMONTHLY.OMM_YEAR= '" + Years2.Text + "'";
                    sqlcommand.CommandText += "delete from OFFICERSMEEZDAILY where OFFICERSMEEZDAILY.P_ID = '" + P_ID + "' and OFFICERSMEEZDAILY.M_MONTH = '" + Months2.Text + "' and OFFICERSMEEZDAILY.M_YEAR= '" + Years2.Text + "'";
                }
                sqlcommand.ExecuteNonQuery();
                UpDateAllPersons();
                UpdateSalary();
                UpdateMeez();
                if (InList.Items.Count > 0)
                {
                    InList.SelectedIndex = 0;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("خطأ لم يتم تحديد فرد");
            }
        }


        public void UpDateAllPersons()
        {
            InList.Items.Clear();
            NotInList.Items.Clear();
            if (Officers1.Checked)
            {
                sqlcommand.CommandText = "Select P_Name from PERSONS where P_NAME not in (select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' )  and PERSONS.P_Type = 'O'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    NotInList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
                sqlcommand.CommandText = "select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' and PERSONS.P_Type = 'O'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    InList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
            }
            else if (SubOfficers1.Checked)
            {
                sqlcommand.CommandText = "Select P_Name from PERSONS where P_NAME not in (select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' )  and PERSONS.P_Type = 'R'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    NotInList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
                sqlcommand.CommandText = "select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' and PERSONS.P_Type = 'R'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    InList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
            }
            else if (Soliders1.Checked)
            {
                sqlcommand.CommandText = "Select P_Name from PERSONS where P_NAME not in (select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' )  and PERSONS.P_Type = 'S'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    NotInList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
                sqlcommand.CommandText = "select P_Name from Persons,Salary where SALARY.P_ID =PERSONS.P_ID and  Salary.SA_MONTH = '" + Months2.Text + "' and Salary.SA_YEAR = '" + Years2.Text + "' and PERSONS.P_Type = 'S'";
                SqlDR = sqlcommand.ExecuteReader();
                while (SqlDR.Read())
                {
                    InList.Items.Add(SqlDR[0].ToString());
                }
                SqlDR.Close();
            }
        }


        public void AddNewPerson()
        {
            if (Officer.Checked)
            {
                sqlcommand.CommandText = "INSERT INTO [dbo].[PERSONS] VALUES ('O',N'" + PersonName.Text + "')";
            }
            else if (SubOfficer.Checked)
            {
                sqlcommand.CommandText = "INSERT INTO [dbo].[PERSONS] VALUES ('R',N'" + PersonName.Text + "')";
            }
            else if (Solider.Checked)
            {
                sqlcommand.CommandText = "INSERT INTO [dbo].[PERSONS] VALUES ('S',N'" + PersonName.Text + "')";
            }
            sqlcommand.ExecuteNonQuery();
            MessageBox.Show("تمت اضافة " + PersonName.Text+" قاعدة البيانات");
            PersonName.Clear();
            UpDateAllPersons();
        }


        public void RemovePerson()
        {
            String P_ID;
            sqlcommand.CommandText = "Select P_ID from PERSONS Where P_NAME = N'" + PersonName.Text + "'";
            SqlDR = sqlcommand.ExecuteReader();
            SqlDR.Read();
            P_ID = SqlDR[0].ToString();
            SqlDR.Close();
            sqlcommand.CommandText = "delete from PERSONS where PERSONS.P_ID = ' " + P_ID + "'";
            sqlcommand.ExecuteNonQuery();
            MessageBox.Show("تم حذف  " + PersonName.Text+ " من قاعدة البيانات");
            UpDateAllPersons();
        }


        public void SaveSalaryEdits()
        {
            for (int i = 0; i < dataGridView2.Rows.Count;i++)
            {
                sqlcommand.CommandText = "UPDATE dbo.SALARY SET SALARY.SA_SALARY = '" + dataGridView2[1, i].Value + "' WHERE SALARY.P_ID = '" + PersonsIDsList.ElementAt(i) + "' and  SALARY.SA_MONTH = '" + Months.SelectedItem + "' and SALARY.SA_YEAR = '" + Years.SelectedItem + "' ";
                sqlcommand.CommandText += "UPDATE dbo.SALARY SET SALARY.SA_SIGN = '" + dataGridView2[6, i].Value + "' WHERE SALARY.P_ID = '" + PersonsIDsList.ElementAt(i) + "' and  SALARY.SA_MONTH = '" + Months.SelectedItem + "' and SALARY.SA_YEAR = '" + Years.SelectedItem + "' ";
                sqlcommand.CommandText += "UPDATE dbo.MARKET SET MARKET.MAR_COST = '" + dataGridView2[2, i].Value + "' WHERE MARKET.P_ID = '" + PersonsIDsList.ElementAt(i) + "' and  MARKET.MAR_MONTH = '" + Months.SelectedItem + "' and MARKET.MAR_YEAR = '" + Years.SelectedItem + "'";
                sqlcommand.ExecuteNonQuery();
            }
            UpdateSalary();
        }

        
        public void UpdateSalary()
        {
            if (radioButton1.Checked)
            {
                sqlcommand.CommandText = "Select PERSONS.P_ID,PERSONS.P_NAME,Salary.SA_SALARY,MARKET.MAR_COST,OFFICERSMEEZMONTHLY.OMM_COST as Meez,(MARKET.MAR_COST+OFFICERSMEEZMONTHLY.OMM_COST) as Cut ,(Salary.SA_SALARY-(MARKET.MAR_COST+OFFICERSMEEZMONTHLY.OMM_COST)) as Net,Salary.SA_SIGN from PERSONS,SALARY,MARKET,OFFICERSMEEZMONTHLY where PERSONS.P_ID = SALARY.P_ID  and PERSONS.P_ID = MARKET.P_ID and PERSONS.P_ID = OFFICERSMEEZMONTHLY.P_ID and SALARY.SA_MONTH = '" + Months.Text + "' and SALARY.SA_YEAR = '" + Years.Text + "' and MARKET.MAR_MONTH = '" + Months.Text + "' and MARKET.MAR_YEAR = '" + Years.Text + "' and OFFICERSMEEZMONTHLY.OMM_MONTH ='" + Months.Text + "' and OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years.Text + "' and PERSONS.P_TYPE = 'O' ";
            }
            else if (radioButton2.Checked)
            {
                sqlcommand.CommandText = "Select PERSONS.P_ID,PERSONS.P_NAME,Salary.SA_SALARY,MARKET.MAR_COST,MEEZ.M1_COST as Meez,(MARKET.MAR_COST+MEEZ.M1_COST) as Cut ,(Salary.SA_SALARY-(MARKET.MAR_COST+MEEZ.M1_COST)) as Net ,Salary.SA_SIGN from PERSONS,SALARY,MARKET,MEEZ where PERSONS.P_ID = SALARY.P_ID  and PERSONS.P_ID = MARKET.P_ID and SALARY.SA_MONTH = '" + Months.Text + "' and SALARY.SA_YEAR = '" + Years.Text + "' and MARKET.MAR_MONTH = '" + Months.Text + "' and MARKET.MAR_YEAR = '" + Years.Text + "' and MEEZ.M1_MONTH ='" + Months.Text + "' and MEEZ.M1_YEAR = '" + Years.Text + "' and PERSONS.P_TYPE = 'R'";
            }
            else if (radioButton3.Checked)
            {
                sqlcommand.CommandText = "Select PERSONS.P_ID,PERSONS.P_NAME,Salary.SA_SALARY,MARKET.MAR_COST,MEEZ.M1_COST as Meez,(MARKET.MAR_COST+MEEZ.M1_COST) as Cut ,(Salary.SA_SALARY-(MARKET.MAR_COST+MEEZ.M1_COST)) as Net,Salary.SA_SIGN from PERSONS,SALARY,MARKET,MEEZ where PERSONS.P_ID = SALARY.P_ID  and PERSONS.P_ID = MARKET.P_ID and SALARY.SA_MONTH = '" + Months.Text + "' and SALARY.SA_YEAR = '" + Years.Text + "' and MARKET.MAR_MONTH = '" + Months.Text + "' and MARKET.MAR_YEAR = '" + Years.Text + "' and MEEZ.M1_MONTH ='" + Months.Text + "' and MEEZ.M1_YEAR = '" + Years.Text + "' and PERSONS.P_TYPE = 'S'";
            }
            else
            {
                String S = "Select PERSONS.P_ID,PERSONS.P_NAME,Salary.SA_SALARY,MARKET.MAR_COST,OFFICERSMEEZMONTHLY.OMM_COST as Meez,(MARKET.MAR_COST+OFFICERSMEEZMONTHLY.OMM_COST) as Cut ,(Salary.SA_SALARY-(MARKET.MAR_COST+OFFICERSMEEZMONTHLY.OMM_COST)) as Net ,Salary.SA_SIGN from PERSONS,SALARY,MARKET,OFFICERSMEEZMONTHLY where PERSONS.P_ID = SALARY.P_ID  and PERSONS.P_ID = MARKET.P_ID and PERSONS.P_ID = OFFICERSMEEZMONTHLY.P_ID and SALARY.SA_MONTH = '" + Months.Text + "' and SALARY.SA_YEAR = '" + Years.Text + "' and MARKET.MAR_MONTH = '" + Months.Text + "' and MARKET.MAR_YEAR = '" + Years.Text + "' and OFFICERSMEEZMONTHLY.OMM_MONTH ='" + Months.Text + "' and OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years.Text + "' and PERSONS.P_TYPE = 'O' Union ";
                S += "Select PERSONS.P_ID,PERSONS.P_NAME,Salary.SA_SALARY,MARKET.MAR_COST,MEEZ.M1_COST as Meez,(MARKET.MAR_COST+MEEZ.M1_COST) as Cut ,(Salary.SA_SALARY-(MARKET.MAR_COST+MEEZ.M1_COST)) as Net ,Salary.SA_SIGN from PERSONS,SALARY,MARKET,MEEZ where PERSONS.P_ID = SALARY.P_ID  and PERSONS.P_ID = MARKET.P_ID and SALARY.SA_MONTH = '" + Months.Text + "' and SALARY.SA_YEAR = '" + Years.Text + "' and MARKET.MAR_MONTH = '" + Months.Text + "' and MARKET.MAR_YEAR = '" + Years.Text + "' and MEEZ.M1_MONTH ='" + Months.Text + "' and MEEZ.M1_YEAR = '" + Years.Text + "' and PERSONS.P_TYPE != 'O'";
                sqlcommand.CommandText = S;
            }
            SqlDR = sqlcommand.ExecuteReader();
            int i = 0;
            dataGridView2.Rows.Clear();
            PersonsIDsList.Clear();
            while (SqlDR.Read())
            {
                dataGridView2.Rows.Add(1);
                dataGridView2[0, i].Value = SqlDR[1].ToString();
                dataGridView2[1, i].Value = SqlDR[2].ToString();
                dataGridView2[2, i].Value = SqlDR[3].ToString();
                dataGridView2[3, i].Value = SqlDR[4].ToString();
                dataGridView2[4, i].Value = SqlDR[5].ToString();
                dataGridView2[5, i].Value = SqlDR[6].ToString();
                dataGridView2[6, i].Value = SqlDR[7].ToString();
                PersonsIDsList.Add(Int32.Parse(SqlDR[0].ToString()));
                i++;
            }
            SqlDR.Close();
            UpdateTotal();
        }


        public void UpdateTotal()
        {
            float [] Sum = new float [8];
            Sum[0]=dataGridView2.Rows.Count;
            for(int i=1;i<6;i++)
            {
                Sum[i]=0;
                for (int j = 0; j < dataGridView2.Rows.Count;j++)
                {
                    Sum[i] += float.Parse(dataGridView2[i, j].Value.ToString());
                }
            }
            Sum[6] = 0;
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                DataGridViewCheckBoxCell Cell = dataGridView2[6,j] as DataGridViewCheckBoxCell;
                if (Cell.Value.ToString() == "True")
                {
                    Sum[6] += float.Parse(dataGridView2[5, j].Value.ToString());
                }
            }
            Sum[7] = Sum[5] - Sum[6];
            CountBox.Text = Sum[0].ToString();
            SalaryBox.Text = Sum[1].ToString();
            MarketBox.Text = Sum[2].ToString();
            MeezBox.Text = Sum[3].ToString();
            CutBox.Text = Sum[4].ToString();
            NetBox.Text = Sum[5].ToString();
            PayedBox.Text = Sum[6].ToString();
            RestBox.Text = Sum[7].ToString();
        }

        public void SaveMeezEdits()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j <= 40; j++)
                {
                   sqlcommand.CommandText = "UPDATE OFFICERSMEEZDAILY SET OFFICERSMEEZDAILY.M_COST = '" + dataGridView1[j, i].Value.ToString() + "' WHERE OFFICERSMEEZDAILY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' AND OFFICERSMEEZDAILY.M_DAY = '" + j + "' AND OFFICERSMEEZDAILY.M_MONTH = '" + Months1.SelectedItem + "' AND OFFICERSMEEZDAILY.M_YEAR = '" + Years1.SelectedItem + "'";
                   sqlcommand.CommandText += "UPDATE dbo.OFFICERSMEEZMONTHLY SET OMM_SIGN = '" + dataGridView1[44, i].Value.ToString() + "' WHERE OFFICERSMEEZMONTHLY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' AND OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months1.SelectedItem + "' AND OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years1.SelectedItem + "'";
                   sqlcommand.ExecuteNonQuery();
                }
             }
            SaveMeezMonthly();
            UpdateMeez();
        }


        
        public void SaveMeezMonthly()
        {
            try
            {
                sqlcommand.CommandText = "INSERT INTO dbo.MEEZMONTHLY VALUES ('" + Months1.SelectedItem + "','" + Years1.SelectedItem + "','" + PrevGoodsBox.Text + "','" + NextGoodsBox.Text + "','" + BillsBox.Text + "','" + MiscellBox.Text + "')";
                sqlcommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                sqlcommand.CommandText = "UPDATE dbo.MEEZMONTHLY SET MM_PREVGOODS = '"+PrevGoodsBox.Text+"',MM_NEXTGOODS = '"+NextGoodsBox.Text+"',MM_BILLS = '"+BillsBox.Text+"',MM_MISCELL= '"+MiscellBox.Text+"' WHERE MM_MONTH = '"+Months1.SelectedItem+"' and MM_YEAR = '"+Years1.SelectedItem+"'";
                sqlcommand.ExecuteNonQuery();
            }
        }


        public void UpDateMeezMonthly(float TotalCost)
        {
            sqlcommand.CommandText = "Select MM_PREVGOODS,MM_NEXTGOODS,MM_BILLS,MM_MISCELL FROM dbo.MEEZMONTHLY  WHERE MM_MONTH = '"+Months1.SelectedItem+"' and MM_YEAR = '"+Years1.SelectedItem+"'";
            SqlDR =sqlcommand.ExecuteReader();
            SqlDR.Read();
            PrevGoodsBox.Text = SqlDR[0].ToString();
            NextGoodsBox.Text = SqlDR[1].ToString();
            BillsBox.Text = SqlDR[2].ToString();
            MiscellBox.Text = SqlDR[3].ToString();
            SqlDR.Close();
            double Percentage;
            float TotalMeezCost = 0;
            float PayedCost = 0;
            Percentage = float.Parse(MiscellBox.Text) / TotalCost;
            Percentage = Math.Round(Percentage,2);
            PercentageBox.Text = Percentage.ToString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[42, i].Value = Math.Round(Percentage * float.Parse(dataGridView1[41, i].Value.ToString()),2);
                dataGridView1[43, i].Value = float.Parse(dataGridView1[41, i].Value.ToString()) + float.Parse(dataGridView1[42, i].Value.ToString());
                TotalMeezCost += float.Parse(dataGridView1[43, i].Value.ToString());
                DataGridViewCheckBoxCell Cell = dataGridView1[44, i] as DataGridViewCheckBoxCell;
                if (Cell.Value.ToString() == "True")
                {
                    float F;
                    if (float.TryParse(dataGridView1[43, i].Value.ToString(), out F))
                    {
                        PayedCost += F;
                    }
                    else
                    {
                        F = 0;
                    }
                }
                try
                {
                    sqlcommand.CommandText = "INSERT INTO OFFICERSMEEZMONTHLY VALUES ('" + Months1.SelectedItem + "','" + Years1.SelectedItem + "','" + OfficersIDsList.ElementAt(i) + "','" + dataGridView1[43, i].Value.ToString() + "','" + dataGridView1[44, i].Value.ToString() + "')";
                    sqlcommand.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    sqlcommand.CommandText = "UPDATE OFFICERSMEEZMONTHLY SET OFFICERSMEEZMONTHLY.OMM_COST = '" + dataGridView1[43, i].Value.ToString() + "' WHERE OFFICERSMEEZMONTHLY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' AND OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months1.SelectedItem + "' AND OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years1.SelectedItem + "' ";
                    sqlcommand.CommandText += "UPDATE OFFICERSMEEZMONTHLY SET OFFICERSMEEZMONTHLY.OMM_SIGN = '" + dataGridView1[44, i].Value.ToString() + "' WHERE OFFICERSMEEZMONTHLY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' AND OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months1.SelectedItem + "' AND OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years1.SelectedItem + "'";
                    sqlcommand.ExecuteNonQuery();
                }
            }
            MeezTotalBox.Text = TotalMeezCost.ToString();
            TotalBox.Text = (TotalMeezCost - (float.Parse(BillsBox.Text.ToString()) + float.Parse(PrevGoodsBox.Text.ToString()) - float.Parse(NextGoodsBox.Text.ToString()))).ToString();
            PayedMeezBox.Text = PayedCost.ToString();
            RestMeezBox.Text = (TotalMeezCost - PayedCost).ToString();
        }



        public void UpdateMeez()
        {
            dataGridView1.Rows.Clear();
            OfficersNamesList.Clear();
            OfficersIDsList.Clear();
            sqlcommand.CommandText = "select distinct OFFICERSMEEZMONTHLY.P_ID,PERSONS.P_NAME from OFFICERSMEEZMONTHLY,PERSONS where PERSONS.P_ID=OFFICERSMEEZMONTHLY.P_ID and OFFICERSMEEZMONTHLY.OMM_MONTH = '"+Months1.Text+"' and OFFICERSMEEZMONTHLY.OMM_YEAR = '"+Years1.Text+"'";
            SqlDR = sqlcommand.ExecuteReader();
            while (SqlDR.Read())
            {
                OfficersNamesList.Add(SqlDR[1].ToString());
                OfficersIDsList.Add(Int32.Parse(SqlDR[0].ToString()));
            }
            SqlDR.Close();
            if (OfficersIDsList.Count > 0)
            {
            dataGridView1.Rows.Add(OfficersIDsList.Count);
            }
            int j;
            float Sum;
            float TotalCost=0;
            for (int i = 0; i < OfficersIDsList.Count; i++)
            {
                sqlcommand.CommandText = "Select OFFICERSMEEZDAILY.M_COST from OFFICERSMEEZDAILY where OFFICERSMEEZDAILY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' and OFFICERSMEEZDAILY.M_MONTH = '" + Months1.SelectedItem + "' and OFFICERSMEEZDAILY.M_YEAR = '" + Years1.SelectedItem + "' Order by M_DAY";
                SqlDR = sqlcommand.ExecuteReader();
                j = 1;
                Sum = 0;
                dataGridView1[0, i].Value = OfficersNamesList.ElementAt(i);
                while (SqlDR.Read())
                {
                    dataGridView1[j++, i].Value = SqlDR["M_Cost"].ToString();
                    Sum += float.Parse(SqlDR["M_Cost"].ToString());
                }
                SqlDR.Close();
                sqlcommand.CommandText = "SELECT OMM_SIGN FROM dbo.OFFICERSMEEZMONTHLY where OFFICERSMEEZMONTHLY.P_ID = '" + OfficersIDsList.ElementAt(i) + "' and OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months1.SelectedItem + "' and OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years1.SelectedItem + "'";
                SqlDR = sqlcommand.ExecuteReader();
                SqlDR.Read();
                dataGridView1[44, i].Value = SqlDR["OMM_SIGN"].ToString();
                dataGridView1[41, i].Value = Sum;
                TotalCost += Sum;
                SqlDR.Close();
            }
            if( Months1.SelectedItem != null && Years1.SelectedItem != null)
            {
                UpDateMeezMonthly(TotalCost);
            }
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            sqlcommand.Cancel();
            SqlDR.Close();
            sqlconnection.Close();
            this.Close();
            GetLoginForm().Show();
        }

        private void Years_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void Months_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void Months1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMeez();
        }

        private void Years1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMeez();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddNewPerson();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNewMonth();
        }

        private void Years2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void Months2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddToMonth();
        }

        private void Officers1_CheckedChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void SubOfficers1_CheckedChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void Soliders1_CheckedChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RemoveFromMonth();
        }

        private void All1_CheckedChanged(object sender, EventArgs e)
        {
            UpDateAllPersons();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RemoveMonth();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RemovePerson();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveSalaryEdits();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveMeezEdits();
        }

    }
}
