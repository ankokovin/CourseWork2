﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class OPUser : CourseWork.OP
    {
        public OPUser()
        {
            InitializeComponent();
        }

        private void OPUser1_Load(object sender, EventArgs e)
        {
            UserTypeComboBox.DataSource = Enum.GetValues(typeof(UserType));
        }
        protected override void Act()
        {
            if (ActionMode == ActionMode.Add)
            {
                Enum.TryParse(UserTypeComboBox.SelectedValue.ToString(), out UserType userType);
                if (Operations.AddUser(userType, LoginTextBox.Text, PasswordTextBox.Text,out string s))
                {
                    Close();
                }
                MessageBox.Show(s);
            }
            else
            {
                Enum.TryParse(UserTypeComboBox.SelectedValue.ToString(), out UserType userType);
                if (Operations.ChangeUser(Id,userType, LoginTextBox.Text, PasswordTextBox.Text, out string s))
                {
                    Close();
                }
                MessageBox.Show(s);
            }
        }
        public override void Change(Object obj)
        {
            Text = "Изменение пользователя ";
            if (obj is User user)
            {
                LoginTextBox.Text = user.Login;
                PasswordTextBox.Text = user.Password;
                Text += user + " id:" + user.Id;
                UserTypeComboBox.SelectedValue = user.UserType;
            }
            ActionMode = ActionMode.Change;
            button1.Text = "Изменить пользователя";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Act();
        }
    }
}
