﻿using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }
        public User CurrentUser;

        public class Changer<T> 
            where T: OP, new()
        {
            public AdminForm owner;
            private EntityTypes type;
            public Changer(EntityTypes _type)
            {
                type = _type;
            }
            public void Add()
            {
                T op = new T();
                op.Owner = owner;
                op.CurrentUser = owner.CurrentUser;
                op.Show();
            }
            public void Change(DataGridView dgv, int idColomn)
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[idColomn, index].Value.ToString(), out id);
                if (!ok) return;
                T op = new T();
                op.Id = id;
                op.CurrentUser = owner.CurrentUser;
                op.Owner = owner;
                op.Show();
                op.Change(Find(id));
                op.FormClosed +=(object obj, FormClosedEventArgs arg)=> dgv.Refresh(); 
            }
            public object Find(int Id)
            {
                switch (type)
                {
                    case EntityTypes.User:
                        return Operations.FindUser(Id);
                    case EntityTypes.City:
                        return Operations.FindCity(Id);
                    case EntityTypes.Street:
                        return Operations.FindStreet(Id);
                    case EntityTypes.House:
                        return Operations.FindHouse(Id);
                    case EntityTypes.Address:
                        return Operations.FindAddress(Id);
                    case EntityTypes.Order:
                        return Operations.FindOrder(Id);
                    case EntityTypes.OrderEntry:
                        return Operations.FindOrderEntry(Id);
                    case EntityTypes.Status:
                        return Operations.FindStatus(Id);
                    case EntityTypes.Meter:
                        return Operations.FindMeter(Id);
                    case EntityTypes.MeterType:
                        return Operations.FindMeterType(Id);
                    case EntityTypes.Stavka:
                        return Operations.FindStavka(Id);
                    case EntityTypes.Person:
                        return Operations.FindPerson(Id);
                    case EntityTypes.Customer:
                        return Operations.FindCustomer(Id);
                    case EntityTypes.Company:
                        return Operations.FindCompany(Id);
                    default:
                        return null;
                }
            }

        }

        private void AddCityButton_Click(object sender, EventArgs e)
        {
            SimpleView CityForm = new SimpleView();
            Operations.cont.CitySet.Load();
            CityForm.Text = "Города";
            CityForm.Source = Operations.cont.CitySet.Local.ToBindingList();
            Changer<OPCity> changer = new Changer<OPCity>(EntityTypes.City);
            CityForm.CurrentEntity = EntityTypes.City;
            changer.owner = this;
            CityForm.Add += changer.Add;
            CityForm.Change +=(DataGridView dgv)=> changer.Change(dgv,Program.FindTitle(dgv,"Идентификационный номер"));
            CityForm.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveCity(id,out string s);
                MessageBox.Show(s);
            };
            CityForm.SetButtonNames("Добавить город", "Удалить город", "Изменить город");
            CityForm.CurrentUser = CurrentUser;
            CityForm.Show();
        }

        private void AddStreetButton_Click(object sender, EventArgs e)
        {
            SimpleView StreetForm = new SimpleView();
            Operations.cont.StreetSet.Load();
            StreetForm.Text = "Улицы";
            StreetForm.CurrentEntity = EntityTypes.Street;
            StreetForm.Source = Operations.cont.StreetSet.Local.ToBindingList();
            Changer<OPStreet> changer = new Changer<OPStreet>(EntityTypes.Street);
            changer.owner = this;
            StreetForm.Add += changer.Add;
            StreetForm.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            StreetForm.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveStreet(id, out string s);
                MessageBox.Show(s);
            };
            StreetForm.SetButtonNames("Добавить улицу", "Удалить улицу", "Изменить улицу");
            StreetForm.CurrentUser = CurrentUser;
            StreetForm.Show();
        }


        private void UsersButton_Click(object sender, EventArgs e)
        {
            SimpleView UserForm = new SimpleView();
            Operations.cont.UserSet.Load();
            UserForm.Text = "Пользователи";
            UserForm.CurrentEntity = EntityTypes.User;
            UserForm.Source = Operations.cont.UserSet.Local.ToBindingList();
            Changer<OPUser> changer = new Changer<OPUser>(EntityTypes.User);
            changer.owner = this;
            UserForm.Add += changer.Add;
            UserForm.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            UserForm.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                if (CurrentUser.Id == id)
                {
                    MessageBox.Show("Невозможно удалить текущего пользователя");
                    return;
                }
                Operations.RemoveUser(id, out string s);
                MessageBox.Show(s);
            };
            UserForm.SetButtonNames("Добавить пользователя", "Удалить пользователя", "Изменить пользователя");
            UserForm.CurrentUser = CurrentUser;
            UserForm.Show();
        }

        private void AddressButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.AddressSet.Load();
            simpleView.Text = "Адреса";
            simpleView.CurrentEntity = EntityTypes.Address;
            simpleView.Source = Operations.cont.AddressSet.Local.ToBindingList();
            Changer<OPAddress> changer = new Changer<OPAddress>(EntityTypes.Address);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveAddress(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить адрес", "Удалить адрес", "Изменить адрес");
            simpleView.CurrentUser = CurrentUser;
            simpleView.Show();
        }

        private void MeterButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.MeterSet.Load();
            simpleView.Text = "Приборы учёта";
            simpleView.Source = Operations.cont.MeterSet.Local.ToBindingList();
            simpleView.CurrentEntity = EntityTypes.Meter;
            Changer<OPMeter> changer = new Changer<OPMeter>(EntityTypes.Meter);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveMeter(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить прибор учёта", "Удалить прибор учёта", "Изменить прибор учёта"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();
        }

        private void MeterTypeButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.MeterTypeSet.Load();
            simpleView.Text = "Типы приборов учёта";
            simpleView.CurrentEntity = EntityTypes.MeterType;
            simpleView.Source = Operations.cont.MeterTypeSet.Local.ToBindingList();
            Changer<OPMeterType> changer = new Changer<OPMeterType>(EntityTypes.MeterType);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveMeterType(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить тип приборов учёта", "Удалить тип приборов учёта", "Изменить тип приборов учёта"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();
        }

        private void OrderButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.OrderSet.Load();
            simpleView.Text = "Заказы";
            simpleView.CurrentEntity = EntityTypes.Order;
            simpleView.Source = Operations.cont.OrderSet.Local.ToBindingList();
            Changer<OPOrder> changer = new Changer<OPOrder>(EntityTypes.Order);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveOrder(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить заказ", "Удалить заказ", "Изменить заказ"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }

        private void OrderEntryButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.OrderEntrySet.Load();
            simpleView.Text = "Заказные позиции";
            simpleView.CurrentEntity = EntityTypes.OrderEntry;
            simpleView.Source = Operations.cont.OrderEntrySet.Local.ToBindingList();
            Changer<OPOrderEntry> changer = new Changer<OPOrderEntry>(EntityTypes.OrderEntry);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveOrderEntry(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить заказную позицию", "Удалить заказную позицию", "Изменить заказную позицию"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }

        private void StatusButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.StatusSet.Load();
            simpleView.Text = "Статусы заказных позиций";
            simpleView.CurrentEntity = EntityTypes.Status;
            simpleView.Source = Operations.cont.StatusSet.Local.ToBindingList();
            Changer<OPStatus> changer = new Changer<OPStatus>(EntityTypes.Status);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveStatus(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить статус заказа", "Удалить статус заказа", "Изменить статус заказа"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }

        private void CustomerButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            simpleView.Text = "Заказчики";
            Operations.cont.CustomerSet.Load();
            simpleView.CurrentEntity = EntityTypes.Customer;
            simpleView.Source = Operations.cont.CustomerSet.Local.ToBindingList();
            Changer<OPCustomer> changer = new Changer<OPCustomer>(EntityTypes.Customer);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                if (Operations.FindCustomer(id) is Company)
                {
                    Operations.RemoveCompany(id, out string s);
                    MessageBox.Show(s);
                }
                else
                {
                    Operations.RemoveCustomer(id, out string s);
                    MessageBox.Show(s);
                }
            };
            simpleView.SetButtonNames("Добавить заказчика", "Удалить заказчика", "Изменить заказчика"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }
        
        private void PersonButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.PersonSet.Load();
            simpleView.Text = "Работники";
            simpleView.CurrentEntity = EntityTypes.Person;
            simpleView.Source = Operations.cont.PersonSet.Local.ToBindingList();
            Changer<OPPerson> changer = new Changer<OPPerson>(EntityTypes.Person);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemovePerson(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить работника", "Удалить работника", "Изменить работника"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }

        private void StavkaButton_Click(object sender, EventArgs e)
        {
            SimpleView simpleView = new SimpleView();
            Operations.cont.StavkaSet.Load();
            simpleView.Text = "Ставки";
            simpleView.CurrentEntity = EntityTypes.Stavka;
            simpleView.Source = Operations.cont.StavkaSet.Local.ToBindingList();
            Changer<OPStavka> changer = new Changer<OPStavka>(EntityTypes.Stavka);
            changer.owner = this;
            simpleView.Add += changer.Add;
            simpleView.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            simpleView.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveStavka(id, out string s);
                MessageBox.Show(s);
            };
            simpleView.SetButtonNames("Добавить ставку", "Удалить ставку", "Изменить ставку"); simpleView.CurrentUser = CurrentUser;
            simpleView.Show();

        }

        private void HouseButton_Click(object sender, EventArgs e)
        {
            SimpleView HouseForm = new SimpleView();
            Operations.cont.HouseSet.Load();
            HouseForm.Text = "Дома";
            HouseForm.CurrentEntity = EntityTypes.House;
            HouseForm.Source = Operations.cont.HouseSet.Local.ToBindingList();
            Changer<OPHouse> changer = new Changer<OPHouse>(EntityTypes.House);
            changer.owner = this;
            HouseForm.Add += changer.Add;
            HouseForm.Change += (DataGridView dgv) => changer.Change(dgv, Program.FindTitle(dgv, "Идентификационный номер"));
            HouseForm.Remove += (DataGridView dgv) =>
            {
                int index = dgv.SelectedRows[0].Index;
                int id = 0;
                bool ok = int.TryParse(dgv[Program.FindTitle(dgv, "Идентификационный номер"), index].Value.ToString(), out id);
                if (!ok) return;
                Operations.RemoveAddress(id, out string s);
                MessageBox.Show(s);
            };
            HouseForm.SetButtonNames("Добавить дом", "Удалить дом", "Изменить дом"); HouseForm.CurrentUser = CurrentUser;
            HouseForm.Show();

        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            Text = CurrentUser.ToString();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    XMLQuery.Serialize(saveFileDialog1.OpenFile());
                    MessageBox.Show("База данных сохранена");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    XMLQuery.Deserialize(openFileDialog1.OpenFile());
                    MessageBox.Show("Загрузка завершена");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
