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
    public partial class OPOrderEntry : CourseWork.OP
    {
        public OPOrderEntry()
        {
            InitializeComponent();
        }

        private void OPOrderEntry_Load(object sender, EventArgs e)
        {

            Operations.cont.OrderSet.Load();
            dataGridView1.DataSource = Operations.cont.OrderSet.Local.ToBindingList();
            Operations.cont.MeterSet.Load();
            dataGridView2.DataSource = Operations.cont.MeterSet.Local.ToBindingList();
            Operations.cont.StatusSet.Load();
            dataGridView3.DataSource = Operations.cont.StatusSet.Local.ToBindingList();
        }

        protected override void Act()
        {
            int ind1 = dataGridView1.SelectedRows[0].Index;
            int id1 = 0;
            bool ok = int.TryParse(dataGridView1[Program.FindTitle(dataGridView1, "Id"), ind1].Value.ToString(), out id1);
            if (!ok) return;
            int ind2 = dataGridView2.SelectedRows[0].Index;
            int id2 = 0;
            ok = int.TryParse(dataGridView2[Program.FindTitle(dataGridView2, "Id"), ind2].Value.ToString(), out id2);
            if (!ok) return;
            int ind3 = dataGridView3.SelectedRows[0].Index;
            int id3 = 0;
            ok = int.TryParse(dataGridView3[Program.FindTitle(dataGridView3, "Id"), ind3].Value.ToString(), out id3);
            if (!ok) return;
            if (ActionMode == ActionMode.Add)
            {
                if (Operations.AddOrderEntry(Operations.FindOrder(id1), dateTimePicker1.Value,
                    dateTimePicker2.Value, textBox1.Text, Operations.FindMeter(id2),
                    Operations.FindStatus(id3), out string Res))
                    Close();
                MessageBox.Show(Res);
            }else
            {
                if (Operations.ChangeOrderEntry(Id,Operations.FindOrder(id1), dateTimePicker1.Value,
                    dateTimePicker2.Value, textBox1.Text, Operations.FindMeter(id2), Operations.FindStatus(id3), out string Res))
                    Close();
                MessageBox.Show(Res);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Act();
        }

        public override void Change(object obj)
        {
            ActionMode = ActionMode.Change;
            button1.Text = "Изменить заказную позицию";
            if (obj is OrderEntry ordEnt)
            {
                dateTimePicker1.Value = ordEnt.StartTime;
                dateTimePicker2.Value = ordEnt.EndTime;
                Program.SelectId(ref dataGridView1, ordEnt.Order.Id);
                Program.SelectId(ref dataGridView2, ordEnt.Meter.Id);
                Program.SelectId(ref dataGridView3, ordEnt.Status.Id);
            }
        }
    }
}
