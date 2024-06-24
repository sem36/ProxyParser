using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using xNet;

namespace ProxyParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.BackColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            groupBox1.ForeColor = Color.FromArgb(33, 33, 33);
            groupBox1.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            richTextBox1.BackColor = Color.FromArgb(240, 240, 240);
            richTextBox1.BorderStyle = BorderStyle.None;

            groupBox2.ForeColor = Color.FromArgb(33, 33, 33);
            groupBox2.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            richTextBox2.BackColor = Color.FromArgb(240, 240, 240);
            richTextBox2.BorderStyle = BorderStyle.None;

            StartBtn.BackColor = Color.FromArgb(33, 150, 243);
            StartBtn.ForeColor = Color.White;
            StartBtn.FlatStyle = FlatStyle.Flat;
            StartBtn.FlatAppearance.BorderSize = 0;
            StartBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            SaveBtn.BackColor = Color.FromArgb(76, 175, 80);
            SaveBtn.ForeColor = Color.White;
            SaveBtn.FlatStyle = FlatStyle.Flat;
            SaveBtn.FlatAppearance.BorderSize = 0;
            SaveBtn.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            groupBox3.ForeColor = Color.FromArgb(33, 33, 33);
            groupBox3.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            groupBox4.ForeColor = Color.FromArgb(33, 33, 33);
            groupBox4.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            InfoLbl.ForeColor = Color.FromArgb(33, 33, 33);
        }
        private void GetProxy()
        {
            HttpRequest data = new HttpRequest();
            int count = richTextBox2.Lines.Count();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    string response = data.Get(richTextBox2.Lines[i]).ToString();
                    RegularDegeneration(response);
                }
                catch (Exception)
                {
                    InfoLbl.Text = "Ошибка обработки запроса к сайту.";
                }

            }
        }
        private void RegularDegeneration(string response)
        {
            Regex regex = new Regex(@"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}:\d{1,5}");
            MatchCollection matches = regex.Matches(response);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    richTextBox1.Text += match.Value + "\n";
                }
            }
        }
        private void StartBtn_Click(object sender, EventArgs e)
        {
            InfoLbl.Text = "Парсинг начат...";
            try
            {
                GetProxy();
                InfoLbl.Text = $"Успешно найдено {richTextBox1.Lines.Count()} прокси.";
            }
            catch (Exception)
            {

                InfoLbl.Text = "Ошибка получения прокси.";
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files|*.txt";
            saveFileDialog.Title = "Save a Text File";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    File.WriteAllText(filePath, richTextBox1.Text);
                    MessageBox.Show("Файл успешно сохранен", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
