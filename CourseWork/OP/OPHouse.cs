﻿using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class OPHouse : CourseWork.OP
    {
        public OPHouse()
        {
            InitializeComponent();
        }

        private void OPHouse_Load(object sender, EventArgs e)
        {
            Operations.cont.StreetSet.Load();
            dataGridView1.DataSource = Operations.cont.StreetSet.Local.ToBindingList();
            Program.HideColumns(ref dataGridView1, EntityTypes.Street,CurrentUser);
        }
        protected override void Act()
        {
            if (!Checker.IsHouseNumber(textBox1.Text))
            {
                MessageBox.Show("Неверный номер дома");
                return;
            }
            int index = dataGridView1.SelectedRows[0].Index;
            int id = 0;
            bool ok = int.TryParse(dataGridView1[Program.FindTitle(dataGridView1,"Id"), index].Value.ToString(), out id);
            if (!ok) return;
            if (ActionMode == ActionMode.Add)
            {
                if (Operations.AddHouse(textBox1.Text, Operations.FindStreet(id), out string Res))
                    Close();
                MessageBox.Show(Res);
            }else
            {
                if (Operations.ChangeHouse(Id,textBox1.Text, Operations.FindStreet(id), out string Res))
                    Close();
                MessageBox.Show(Res);
            }
        }
        public override void Change(object obj)
        {
            Text = "Изменение дома ";
            if (obj is House h)
            {
                textBox1.Text = h.Number;
                Program.SelectId(ref dataGridView1, h.Street.Id);
                Text += h + " id:" + h.Id;
            }
            ActionMode = ActionMode.Change;
            button1.Text = "Внести изменение";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Act();
        }
    }
}
