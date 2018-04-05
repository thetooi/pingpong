using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Pingpong01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists("pingpong.sqlite"))
            {
                SQLiteConnection.CreateFile("pingpong.sqlite");
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=pingpong.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "CREATE TABLE IF NOT EXISTS `num1` (`id` int(10) NOT NULL auto_increment,`num` numeric(9, 2),`hit` numeric(9, 2), PRIMARY KEY( `id` ));";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            }
            comboBox1.Items.Add("2");
            comboBox1.Items.Add("3");
            comboBox1.Items.Add("4");
            comboBox1.Items.Add("5");
            comboBox1.Items.Add("6");
            comboBox1.Items.Add("7");
            comboBox1.Items.Add("8");
            comboBox1.SelectedIndex = comboBox1.FindStringExact("5");
            Getdata();
            textBox1.Focus();
            button1.Enabled = false;
        }
        private void Getdata()
        {
            textBox2.Text = "";
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=pingpong.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "select * from num1 order by hit desc limit "+ comboBox1.Text +"";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBox2.Text += string.Join("-", reader["num"]);
            }
            textBox1.Focus();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                toolStripStatusLabel1.Text = "กรุณาใส่เลข2ตัวด้วยนะจ๊ะ";
            }
            else
            {
                int txtnum = Convert.ToInt32(textBox1.Text);
                var strnum = txtnum.ToString("D2");
                var arr = strnum.ToString().Select(t => int.Parse(t.ToString())).ToArray();
                int num = (arr[0] + arr[1]);
                int sum = (num % 10);

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=pingpong.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "SELECT count(num) from num1 where num = "+ sum + "";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                
                if (Convert.ToInt32(command.ExecuteScalar()) < 1)
                {
                    sql = $"insert into num1 (num, hit) values ('{sum}', 0)";

                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                    toolStripStatusLabel1.Text = "เพิ่มเรียบร้อบ["+ sum + "]";
                    textBox1.Text = "";
                    textBox1.Focus();
                    button1.Enabled = false;
                }
                else
                {
                    sql = $"UPDATE num1 SET hit = hit + 1 WHERE num = '{sum}'";

                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                    toolStripStatusLabel1.Text = "อัพเดตเรียบร้อย["+ sum + "]";
                    textBox1.Text = "";
                    textBox1.Focus();
                    button1.Enabled = false;
                }
                Getdata();

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 2)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Getdata();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.linkLabel1.LinkVisited = true;

            // Navigate to a URL.
            System.Diagnostics.Process.Start("http://pingpong999.blogspot.com/2018/04/500.html");
        }

        private void DelDBALLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ลบข้อมูลสถิติทั้งหมด?", "ลบสถิติทั้งหมด", MessageBoxButtons.YesNo,
        MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=pingpong.sqlite;Version=3;");
                m_dbConnection.Open();
                string sqlTrunc = "DELETE FROM num1";
                SQLiteCommand cmd = new SQLiteCommand(sqlTrunc, m_dbConnection);
                cmd.ExecuteNonQuery();
            }
            Getdata();
        }
    }
}
