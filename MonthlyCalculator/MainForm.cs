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
    public partial class MainForm : Form
    {
        private Form LoginForm;
        private SqlConnection sqlconnection;
        private SqlCommand sqlcommand;
        private SqlDataReader SqlDR;
        private List<int> PersonsIDsList;
        private List<string> OfficersNamesList;
        private List<int> OfficersIDsList;



        public MainForm()
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


        public void UpdateMonthsYears()
        {
            Months.Items.Clear();
            Months1.Items.Clear();
            sqlcommand.CommandText = "select distinct Month from dbo.MONTHS";
            SqlDR = sqlcommand.ExecuteReader();
            while (SqlDR.Read())
            {
                Months.Items.Add(SqlDR[0].ToString());
                Months1.Items.Add(SqlDR[0].ToString());
            }
            SqlDR.Close();
            Years.Items.Clear();
            Years1.Items.Clear();
            sqlcommand.CommandText = "select distinct Year from dbo.MONTHS";
            SqlDR = sqlcommand.ExecuteReader();
            while (SqlDR.Read())
            {
                Years.Items.Add(SqlDR[0].ToString());
                Years1.Items.Add(SqlDR[0].ToString());
            }
            SqlDR.Close();
        }


        
        public void RefreshSalaryEdits()
        {
            UpdateTotal();
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
            float[] Sum = new float[8];
            Sum[0] = dataGridView2.Rows.Count;
            for (int i = 1; i < 6; i++)
            {
                Sum[i] = 0;
                for (int j = 0; j < dataGridView2.Rows.Count; j++)
                {
                    Sum[i] += float.Parse(dataGridView2[i, j].Value.ToString());
                }
            }
            Sum[6] = 0;
            for (int j = 0; j < dataGridView2.Rows.Count; j++)
            {
                DataGridViewCheckBoxCell Cell = dataGridView2[6, j] as DataGridViewCheckBoxCell;
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


        public void UpDateMeezMonthlyPayed()
        {
            float PayedCost = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell Cell = dataGridView1[44, i] as DataGridViewCheckBoxCell;
                if (Cell.Value.ToString() == "True")
                {
                    PayedCost += float.Parse(dataGridView1[43, i].Value.ToString());
                }
            }
            PayedMeezBox.Text = PayedCost.ToString();
            RestMeezBox.Text = (float.Parse(MeezTotalBox.Text.ToString()) - PayedCost).ToString();
        }


        public void UpDateMeezMonthly(float TotalCost)
        {
            sqlcommand.CommandText = "Select MM_PREVGOODS,MM_NEXTGOODS,MM_BILLS,MM_MISCELL FROM dbo.MEEZMONTHLY  WHERE MM_MONTH = '" + Months1.SelectedItem + "' and MM_YEAR = '" + Years1.SelectedItem + "'";
            SqlDR = sqlcommand.ExecuteReader();
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
            Percentage = Math.Round(Percentage, 2);
            PercentageBox.Text = Percentage.ToString();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[42, i].Value = Math.Round(Percentage * float.Parse(dataGridView1[41, i].Value.ToString()), 2);
                dataGridView1[43, i].Value = float.Parse(dataGridView1[41, i].Value.ToString()) + float.Parse(dataGridView1[42, i].Value.ToString());
                TotalMeezCost += float.Parse(dataGridView1[43, i].Value.ToString());
                DataGridViewCheckBoxCell Cell = dataGridView1[44, i] as DataGridViewCheckBoxCell;
                if (Cell.Value.ToString() == "True")
                {
                    PayedCost += float.Parse(dataGridView1[43, i].Value.ToString());
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
            sqlcommand.CommandText = "select distinct OFFICERSMEEZMONTHLY.P_ID,PERSONS.P_NAME from OFFICERSMEEZMONTHLY,PERSONS where PERSONS.P_ID=OFFICERSMEEZMONTHLY.P_ID and OFFICERSMEEZMONTHLY.OMM_MONTH = '" + Months1.Text + "' and OFFICERSMEEZMONTHLY.OMM_YEAR = '" + Years1.Text + "'";
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
            float TotalCost = 0;
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
            if (Months1.SelectedItem != null && Years1.SelectedItem != null)
            {
                UpDateMeezMonthly(TotalCost);
            }
        }

        
        private void Months_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSalary();
        }

        private void Years_SelectedIndexChanged(object sender, EventArgs e)
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

        private void Quit_Click(object sender, EventArgs e)
        {
            sqlcommand.Cancel();
            SqlDR.Close();
            sqlconnection.Close();
            this.Close();
            GetLoginForm().Show();
        }

        private void Months1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMeez();
        }

        private void Years1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMeez();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpDateMeezMonthlyPayed();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RefreshSalaryEdits();
        }
    }
}
