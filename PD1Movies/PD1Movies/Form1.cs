using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;

namespace PD1Movies
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        public Form1()
        {

            InitializeComponent();
            Connection();
        }

        public void Connection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PD1Movies"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State != ConnectionState.Open)
                Close();
            connection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Movies WHERE", connection);
            SqlParameter imax = new SqlParameter();
            imax.ParameterName = "@Imax";
            imax.SqlDbType = SqlDbType.Bit;
            if (imaxCheckBox.Checked)
                imax.Value = 1;
            else
                imax.Value = 0;
            command.CommandText += " Imax3D=@Imax";
            command.Parameters.Add(imax);
            SqlParameter title = new SqlParameter();
            title.ParameterName = "@Title";
            title.SqlDbType = SqlDbType.VarChar;
            if (titleTextBox.Text.Length > 0)
            {
                title.Value = titleTextBox.Text;
                command.CommandText += " AND CHARINDEX(@Title, Title)>0";
                command.Parameters.Add(title);
            }
            SqlParameter minBudget = new SqlParameter();
            minBudget.ParameterName = "@MinBudget";
            minBudget.SqlDbType = SqlDbType.Int;
            if (minBudgetTextBox.Text.Length > 0)
            {
                minBudget.Value = Convert.ToInt64(minBudgetTextBox.Text);
                command.CommandText += " AND Budget>=@MinBudget";
                command.Parameters.Add(minBudget);
            }
            SqlParameter maxBudget = new SqlParameter();
            maxBudget.ParameterName = "@MaxBudget";
            maxBudget.SqlDbType = SqlDbType.Int;
            if (maxBudgetTextBox.Text.Length > 0)
            {
                maxBudget.Value = Convert.ToInt64(maxBudgetTextBox.Text);
                command.CommandText += " AND Budget<=@MaxBudget";
                command.Parameters.Add(maxBudget);
            }
            SqlParameter minRating = new SqlParameter();
            minRating.ParameterName = "@MinRating";
            minRating.SqlDbType = SqlDbType.Float;
            if (minRatingTextBox.Text.Length > 0)
            {
                minRating.Value = Convert.ToDouble(minRatingTextBox.Text);
                command.CommandText += " AND AvgRating>=@MinRating";
                command.Parameters.Add(minRating);
            }
            SqlParameter maxRating = new SqlParameter();
            maxRating.ParameterName = "@MaxRating";
            maxRating.SqlDbType = SqlDbType.Float;
            if (maxRatingTextBox.Text.Length > 0)
            {
                maxRating.Value = Convert.ToDouble(maxRatingTextBox.Text);
                command.CommandText += " AND AvgRating<=@MaxRating";
                command.Parameters.Add(maxRating);
            }
            SqlParameter minDate = new SqlParameter();
            minDate.ParameterName = "@MinDate";
            minDate.SqlDbType = SqlDbType.Date;
            minDate.Value = fromDateTimePicker.Value;
            command.CommandText += " AND ReleaseDate>=@MinDate";
            command.Parameters.Add(minDate);
            SqlParameter maxDate = new SqlParameter();
            maxDate.ParameterName = "@MaxDate";
            maxDate.SqlDbType = SqlDbType.DateTime;
            maxDate.Value = toDateTimePicker.Value;
            command.CommandText += " AND ReleaseDate<=@MaxDate";
            command.Parameters.Add(maxDate);
            connection.Open();
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable dateTable = new DataTable();
            data.Fill(dateTable);
            connection.Close();
            dataGridView.DataSource = dateTable;
            dataGridView.Columns["ID"].Visible = false;
            dataGridView.Columns["ReleaseDate"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dataGridView.Columns["Budget"].DefaultCellStyle.Format = ".";
        }

        private void dataGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            SqlCommand command = new SqlCommand("DELETE FROM Movies WHERE ID=@ID", connection);
            SqlParameter id = new SqlParameter();
            id.ParameterName = "@ID";
            id.SqlDbType = SqlDbType.Int;
            id.Value = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value);
            command.Parameters.Add(id);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand command;
            if (dataGridView.CurrentRow.Cells["ID"].Value != DBNull.Value)
                command = new SqlCommand("UPDATE Movies SET Title=@Title, ReleaseDate=@ReleaseDate, Budget=@Budget, AvgRating=@Rating, Imax3D=@Imax, TicketsSold=@Tickets WHERE ID=@ID", connection);
            else
            {
                command = new SqlCommand("INSERT INTO Movies(Title, ReleaseDate, Budget, AvgRating, Imax3D, TicketsSold) VALUES (@Title, @ReleaseDate, @Budget, @Rating, @Imax, @Tickets)", connection);
                foreach (DataGridViewCell cell in dataGridView.CurrentRow.Cells)
                {
                    string columnName = dataGridView.Columns[cell.ColumnIndex].Name;
                    if (cell.Value == DBNull.Value && !columnName.Equals("ID") && !columnName.Equals("Imax3D"))
                        return;
                }
            }
            SqlParameter title = new SqlParameter();
            title.ParameterName = "@Title";
            title.SqlDbType = SqlDbType.VarChar;
            title.Value = dataGridView.CurrentRow.Cells["Title"].Value;
            command.Parameters.Add(title);
            SqlParameter releaseDate = new SqlParameter();
            releaseDate.ParameterName = "@ReleaseDate";
            releaseDate.SqlDbType = SqlDbType.DateTime;
            releaseDate.Value = dataGridView.CurrentRow.Cells["ReleaseDate"].Value;
            command.Parameters.Add(releaseDate);
            SqlParameter budget = new SqlParameter();
            budget.ParameterName = "@Budget";
            budget.SqlDbType = SqlDbType.Int;
            budget.Value = dataGridView.CurrentRow.Cells["Budget"].Value;
            command.Parameters.Add(budget);
            SqlParameter rating = new SqlParameter();
            rating.ParameterName = "@Rating";
            rating.SqlDbType = SqlDbType.Float;
            rating.Value = dataGridView.CurrentRow.Cells["AvgRating"].Value;
            command.Parameters.Add(rating);
            SqlParameter imax = new SqlParameter();
            imax.ParameterName = "@Imax";
            imax.SqlDbType = SqlDbType.Bit;
            if (dataGridView.CurrentRow.Cells["Imax3D"].Value == DBNull.Value)
                imax.Value = 0;
            else
                imax.Value = dataGridView.CurrentRow.Cells["Imax3D"].Value;
            command.Parameters.Add(imax);
            SqlParameter tickets = new SqlParameter();
            tickets.ParameterName = "@Tickets";
            tickets.SqlDbType = SqlDbType.Int;
            tickets.Value = dataGridView.CurrentRow.Cells["TicketsSold"].Value;
            command.Parameters.Add(tickets);
            if (dataGridView.CurrentRow.Cells["ID"].Value != DBNull.Value)
            {
                SqlParameter id = new SqlParameter();
                id.ParameterName = "@ID";
                id.SqlDbType = SqlDbType.Int;
                id.Value = Convert.ToInt32(dataGridView.CurrentRow.Cells["ID"].Value);
                command.Parameters.Add(id);
            }
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            if (dataGridView.CurrentRow.Cells["ID"].Value == DBNull.Value)
            {
                command = new SqlCommand("SELECT ID FROM Movies WHERE ID=(SELECT max(ID) FROM Movies)", connection);
                connection.Open();
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable dateTable = new DataTable();
                data.Fill(dateTable);
                connection.Close();
                dataGridView.CurrentRow.Cells["ID"].Value = dateTable.Rows[0][0];
            }
        }
    }
}
