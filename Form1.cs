using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace bns_switcher
{
    public partial class Form1 : Form
    {
        private bool isReal = false;

        public Form1()
        {
            InitializeComponent();
            label2.BackColor = Color.FromArgb(50, 255, 255, 255);
            label3.BackColor = Color.FromArgb(50, 255, 255, 255);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checkReg();
        }

        private void checkReg()
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\plaync", true);

                if (key != null)
                {
                    var test_key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\plaync\BNS_KOR_TEST", true);

                    if(test_key != null)
                    {
                        label1.Text = "TEST";
                        test_key.Close();
                        isReal = false;
                    }
                    else
                    {
                        isReal = true;
                        label1.Text = "REAL";
                    }

         
                    key.Close();
                }

            }
            catch (Exception e)
            {
                label1.Text = "X";
            }


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            checkReg();
            if (isReal)
            {
                MessageBox.Show("이미 본서버입니다.");
                return;
            }

            changeReg(true);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            checkReg();
        
            if (!isReal)
            {
                MessageBox.Show("이미 테스트서버입니다.");
                return;
            }

            changeReg(false);
        }

        private void changeReg(bool real)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\plaync\BNS_KOR" + (real ? "_TEST" : ""), true);
                string dir = key.GetValue("BaseDir").ToString();

                if(dir == null)
                {
                    throw new Exception();
                }

                key.Close();

                RegistryKey plaync = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\plaync", RegistryKeyPermissionCheck.ReadWriteSubTree);
                plaync.DeleteSubKeyTree("BNS_KOR" + (real ? "_TEST" : ""));
                plaync.CreateSubKey("BNS_KOR" + (real ? "" : "_TEST"));
                plaync.Close();

                RegistryKey new_key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\plaync\BNS_KOR" + (real ? "" : "_TEST"), RegistryKeyPermissionCheck.ReadWriteSubTree);
                new_key.SetValue("BaseDir", dir);
                new_key.Close();

                checkReg();
            }
            catch(Exception e)
            {
                MessageBox.Show("오류가 발생했습니다. 관리자 권한이 없을 수 있습니다.");
            }

        }
    }
}
