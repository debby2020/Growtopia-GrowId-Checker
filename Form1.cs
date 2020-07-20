using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace growidchecker
{
    public partial class Form1 : Form
    {
  
        public ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("www.growtopiagame.com/account");
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Left;
            browser.Visible = true;
            browser.LoadingStateChanged += OnLoadingStateChangeds;
        }
        bool fourlettercheck = false;
        string currenAcc;
        string textpath = "";
        int foundcount = 0;
        int badatt = 0;
        int erroratt = 0;
        private async void OnLoadingStateChangeds(object sender, LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                if(fourlettercheck == true)
                {
                    string HTMSL = await browser.GetSourceAsync();
                    if (HTMSL.Contains("Unable to locate an account"))
                    {
                        if(checkBox2.Checked)
                        {
                            using (StreamWriter w = File.AppendText(textpath))
                            {
                                w.WriteLine(currenAcc);
                            }
                            foundcount += 1;
    
                        }
                        else
                        {
                            MessageBox.Show("available acc: " + currenAcc);
                            foundcount += 1;
                            found.Text = foundcount.ToString();
                        }
                    }
                    else if (HTMSL.Contains("That email doesn't match"))
                    {
                        badatt += 1;
  
                    }
                    else
                    {
                        erroratt += 1;
                    }
                }
                else
                {
                    string HTML = await browser.GetSourceAsync();
                    if (HTML.Contains("Unable to locate an account"))
                    {
                        MessageBox.Show("available acc");
                    }
                    else if (HTML.Contains("That email doesn't match"))
                    {
                        MessageBox.Show("unavailable acc");
                    }
                    else
                    {
                        MessageBox.Show("Ready To Use");
                    }
                }


            }
        }
        public void enable()
        {
            button1.Text = "Test";
            button1.Enabled = true;
        }
        public Form1()
        {
            InitializeComponent();
            InitBrowser();
        }
        Random r = new Random();
        private string RandomString(int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789"; //0123456789
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var c = pool[r.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    if(textBox1.TextLength > 3)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("document.getElementById('name').value=\"" + textBox1.Text + "\"");
                        builder.AppendLine("document.getElementById('email').value=\"" + RandomString(9) + "@gmail.com" + "\"");
                        builder.AppendLine("document.forms[0].submit()");
                        browser.ExecuteScriptAsync(builder.ToString());
                    }
                    else
                    {
                        MessageBox.Show("username need > 3");
                    }

                }
                else
                {
                    MessageBox.Show("Empty username");
                }
            }
            catch
            {
                MessageBox.Show("Please wait ready to use message.");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            MessageBox.Show("please wait ready to use message");
            enable();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        if (textBox1.TextLength > 3)
                        {
                            StringBuilder builder = new StringBuilder();
                            builder.AppendLine("document.getElementById('name').value=\"" + textBox1.Text + "\"");
                            builder.AppendLine("document.getElementById('email').value=\"" + RandomString(9) + "@gmail.com" + "\"");
                            builder.AppendLine("document.forms[0].submit()");
                            browser.ExecuteScriptAsync(builder.ToString());
                        }
                        else
                        {
                            MessageBox.Show("username need > 3");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Empty username");
                    }
                }
                catch
                {
                    MessageBox.Show("Please wait ready to use message.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fourletter.Interval = Convert.ToInt32(numericUpDown2.Value);
            fourletter.Start();
            textBox1.Text = "Checking:";
            textBox1.Enabled = false;
        }

        private void fourletter_Tick(object sender, EventArgs e)
        {
            StringBuilder builder1 = new StringBuilder();
            builder1.Append("abcdefghijklmnopqrstuvwxyz");
            if (checkBox1.Checked)
            {
                builder1.Append("0123456789");
            }
            var chars = builder1.ToString();//
            int output = 0;
            decimal std = numericUpDown1.Value;
            output += Convert.ToInt32(Convert.ToString(std));
            var stringChars = new char[output];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("document.getElementById('name').value=\"" + finalString + "\"");
            builder.AppendLine("document.getElementById('email').value=\"" + RandomString(9) + "@gmail.com" + "\"");
            builder.AppendLine("document.forms[0].submit()");
            browser.ExecuteScriptAsync(builder.ToString());
            fourlettercheck = true;
            currenAcc = finalString;
            textBox1.Text = "Checking:"+currenAcc;
            badattempt.Text = badatt.ToString();
            found.Text = foundcount.ToString();
            error.Text = erroratt.ToString();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == true)
            {
                OpenFileDialog theDialog = new OpenFileDialog();
                theDialog.Title = "Open Text File";
                theDialog.Filter = "TXT files|*.txt";
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                theDialog.InitialDirectory = desktop;
                if (theDialog.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show("Selected.");
                    StreamWriter stream = new StreamWriter(theDialog.FileName);
                    stream.WriteLine("## debby#2020");
                    stream.Close();
                    textpath = theDialog.FileName;
                }
                }
        }
    }
}
