using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace arm
{
    public partial class Tables : Form
    {
        int role;
        public Tables(int r)
        {
            InitializeComponent();
            //Database.Connect();

            role = r;

            if (role == 0)
                dgvAppointments.ReadOnly = dgvPets.ReadOnly = dgvProcedures.ReadOnly = dgvSpecies.ReadOnly = true;
            if (role != 1)
                tControl.TabPages.Remove(tpUsers);

            Database.lvUsers(ref lv);
            Database.InitializeDgvSpecies(ref dgvSpecies);
            Database.InitializeDgvProcedures(ref dgvProcedures);
            Database.InitializeDgvPets(ref dgvPets);
            Database.InitializeDgvAppointments(ref dgvAppointments);

            cmbSpeciesSearch.DisplayMember = "species_name";
            cmbSpeciesSearch.ValueMember = "species_id";
            cmbSpeciesSearch.DataSource = Database.Species();
            cmbSpeciesSearch.SelectedValue = -1;
        }

        private static readonly Regex timeRegex = new Regex("^(0[0-9]|1[0-9]|2[0-3]|[0-9]):[0-5][0-9]$");

        private bool CheckTime(string time)
        {
            if (time is null)
                return false;
            return timeRegex.IsMatch(time);        
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add aw = new Add();
            if (aw.ShowDialog() != DialogResult.OK)
                return;
            var lvi = new ListViewItem(new[]
            {
                aw.login,
                aw.password_hash,
                aw.role,
                (aw.date.ToLongDateString())
            })
            {
                Tag = Tuple.Create(aw.id, aw.date, aw.role_id)
            };
            lv.Items.Add(lvi);
        }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lv.SelectedItems.Count == 0)
                return;
            foreach (ListViewItem selectedItem in lv.SelectedItems)
            {
                var tag = (Tuple<int, DateTime, int>)selectedItem.Tag;
                Edit ew = new Edit(selectedItem.SubItems[0].Text, tag.Item2, tag.Item3);
                if (ew.ShowDialog() != DialogResult.OK)
                    continue;
                selectedItem.SubItems[0].Text = ew.newLogin;
                if (ew.password_hash != "")
                    selectedItem.SubItems[1].Text = ew.password_hash;
                selectedItem.SubItems[2].Text = ew.role;
                selectedItem.SubItems[3].Text = ew.date.ToLongDateString();
                selectedItem.Tag = Tuple.Create(tag.Item1, ew.date, ew.role_id);
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database.DeleteUsers(ref lv);
        }

        private void dgvSpecies_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvSpecies.Rows[e.RowIndex];
            if (dgvSpecies.IsCurrentRowDirty)
            {
                row.ErrorText = "";
                var cellsWithPotentialErrors = new[] { row.Cells["name"], row.Cells["average_life_expectancy"] };
                foreach (var cell in cellsWithPotentialErrors)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(cell.Value)))
                    {
                        row.ErrorText += $"Значение в столбце '{cell.OwningColumn.HeaderText}' не может быть пустым\n";
                        e.Cancel = true;
                    }
                }

                string name = EditAndHash.DeleteExtraSpaces((string)row.Cells["name"].Value);

                int avgle = 0;
                if (!e.Cancel && !int.TryParse((row.Cells["average_life_expectancy"].Value).ToString(), out avgle))
                {
                    row.ErrorText += $"В столбце '{row.Cells["average_life_expectancy"].OwningColumn.HeaderText}' должно быть число \n";
                    e.Cancel = true;
                }

                if (!e.Cancel && avgle < 0)
                {
                    row.ErrorText += $"В столбце '{row.Cells["average_life_expectancy"].OwningColumn.HeaderText}' должно быть положительное число \n";
                    e.Cancel = true;
                }

                if (!e.Cancel && !Database.CheckSpecies((int?)row.Cells["id"].Value, name))
                {
                    {
                        row.ErrorText = $"Такой вид уже существует";
                        e.Cancel = true;
                    }
                }

                if (!e.Cancel)
                {
                    var id = (int?)row.Cells["id"].Value;
                    if (id.HasValue)
                        Database.UpdateSpecies(name, Convert.ToBoolean(row.Cells["is_endangered"].Value), avgle /*Convert.ToInt32(row.Cells["average_life_expectancy"].Value)*/, id.Value);
                    else
                        row.Cells["id"].Value = Database.InsertSpecies(name, Convert.ToBoolean(row.Cells["is_endangered"].Value), avgle /*Convert.ToInt32(row.Cells["average_life_expectancy"].Value)*/);

                    var dataDict = new Dictionary<string, object>();
                    foreach (var columnsName in new[] { "name", "is_endangered", "average_life_expectancy" })
                    {
                        if (columnsName == "is_endangered")
                        {
                            bool val = row.Cells == null ? (bool)row.Cells[columnsName].Value == true : false;

                            dataDict[columnsName] = val.ToString();
                        }
                        else
                            dataDict[columnsName] = EditAndHash.DeleteExtraSpaces((row.Cells[columnsName].Value).ToString());
                    }
                    row.Tag = dataDict;


                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ErrorText = "";
                    }
                }
            }
        }

        private void dgvSpecies_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (role == 0)
            {
                e.Cancel = true;
                return;
            }

            var id = (int?)e.Row.Cells["id"].Value;
            if (id.HasValue)
                Database.DeleteSpecies(id.Value);
        }

        private void dgvSpecies_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvSpecies.Rows[e.RowIndex].IsNewRow)
            {
                dgvSpecies[e.ColumnIndex, e.RowIndex].ErrorText = "Значение изменено и пока не сохранено.";

                if (dgvSpecies.Rows[e.RowIndex].Tag != null)
                    dgvSpecies[e.ColumnIndex, e.RowIndex].ErrorText += "\nПредыдущее значение - " +
                                                                  ((Dictionary<string, object>)dgvSpecies.Rows[e.RowIndex]
                                                                      .Tag)[dgvSpecies.Columns[e.ColumnIndex].Name];
            }
        }

        private void dgvSpecies_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dgvSpecies.IsCurrentRowDirty)
            {
                dgvSpecies.CancelEdit();
                if (dgvSpecies.CurrentRow.Cells["id"].Value != null)
                {
                    dgvSpecies.CurrentRow.ErrorText = "";
                    foreach (var kvp in (Dictionary<string, object>)dgvSpecies.CurrentRow.Tag)
                    {
                        dgvSpecies.CurrentRow.Cells[kvp.Key].Value = kvp.Value;
                        dgvSpecies.CurrentRow.Cells[kvp.Key].ErrorText = "";
                    }
                }
                else
                {
                    dgvSpecies.Rows.Remove(dgvSpecies.CurrentRow);
                }
            }
        }

        private void dgvProcedures_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvProcedures.Rows[e.RowIndex].IsNewRow)
            {
                dgvProcedures[e.ColumnIndex, e.RowIndex].ErrorText = "Значение изменено и пока не сохранено.";

                if (dgvProcedures.Rows[e.RowIndex].Tag != null)
                    dgvProcedures[e.ColumnIndex, e.RowIndex].ErrorText += "\nПредыдущее значение - " +
                                                                  ((Dictionary<string, object>)dgvProcedures.Rows[e.RowIndex]
                                                                      .Tag)[dgvProcedures.Columns[e.ColumnIndex].Name];
            }
        }

        private void dgvProcedures_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dgvProcedures.IsCurrentRowDirty)
            {
                dgvProcedures.CancelEdit();
                if (dgvProcedures.CurrentRow.Cells["id"].Value != null)
                {
                    dgvProcedures.CurrentRow.ErrorText = "";
                    foreach (var kvp in (Dictionary<string, object>)dgvProcedures.CurrentRow.Tag)
                    {
                        dgvProcedures.CurrentRow.Cells[kvp.Key].Value = kvp.Value;
                        dgvProcedures.CurrentRow.Cells[kvp.Key].ErrorText = "";
                    }
                }
                else
                {
                    dgvProcedures.Rows.Remove(dgvProcedures.CurrentRow);
                }
            }
        }

        private void dgvProcedures_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvProcedures.Rows[e.RowIndex];
            if (dgvProcedures.IsCurrentRowDirty)
            {
                row.ErrorText = "";
                var cellsWithPotentialErrors = new[] { row.Cells["name"], row.Cells["cost"] };
                foreach (var cell in cellsWithPotentialErrors)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(cell.Value)))
                    {
                        row.ErrorText += $"Значение в столбце '{cell.OwningColumn.HeaderText}' не может быть пустым\n";
                        e.Cancel = true;
                    }
                }

                string name = EditAndHash.DeleteExtraSpaces((string)row.Cells["name"].Value);
                int cost = 0;
                if (!e.Cancel && !int.TryParse((row.Cells["cost"].Value).ToString(), out cost))
                {
                    row.ErrorText += $"В столбце '{row.Cells["cost"].OwningColumn.HeaderText}' должно быть число \n";
                    e.Cancel = true;
                }

                if (!e.Cancel && cost < 0)
                {
                    row.ErrorText += $"В столбце '{row.Cells["cost"].OwningColumn.HeaderText}' должно быть положительное число \n";
                    e.Cancel = true;
                }

                if (!e.Cancel && !Database.CheckProcedure((int?)row.Cells["id"].Value, name))
                {
                    {
                        row.ErrorText = $"Такая процедура уже существует";
                        e.Cancel = true;
                    }
                }

                if (!e.Cancel)
                {
                    var id = (int?)row.Cells["id"].Value;
                    if (id.HasValue)
                        Database.UpdateProcedure(name, cost, id.Value);
                    else
                        row.Cells["id"].Value = Database.InsertProcedure(name, cost);


                    var dataDict = new Dictionary<string, object>();
                    foreach (var columnsName in new[] { "name", "cost" })
                    {
                        dataDict[columnsName] = EditAndHash.DeleteExtraSpaces((row.Cells[columnsName].Value).ToString());
                    }
                    row.Tag = dataDict;


                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ErrorText = "";
                    }
                }
            }
        }

        private void dgvProcedures_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (role == 0)
            {
                e.Cancel = true;
                return;
            }

            var id = (int?)e.Row.Cells["id"].Value;
            if (id.HasValue)
                Database.DeleteProcedure(id.Value);
        }

        private void dgvPets_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvPets.Rows[e.RowIndex];
            if (dgvPets.IsCurrentRowDirty)
            {
                row.ErrorText = "";
                var cellsWithPotentialErrors = new[] { row.Cells["name"], row.Cells["age"] };
                foreach (var cell in cellsWithPotentialErrors)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(cell.Value)))
                    {
                        row.ErrorText += $"Значение в столбце '{cell.OwningColumn.HeaderText}' не может быть пустым\n";
                        e.Cancel = true;
                    }
                }

                string name = EditAndHash.DeleteExtraSpaces((string)row.Cells["name"].Value);
                int age = 0;
                if (!e.Cancel && !int.TryParse((row.Cells["age"].Value).ToString(), out age))
                {
                    row.ErrorText += $"В столбце '{row.Cells["age"].OwningColumn.HeaderText}' должно быть число \n";
                    e.Cancel = true;
                }

                if (!e.Cancel && age < 0)
                {
                    row.ErrorText += $"В столбце '{row.Cells["age"].OwningColumn.HeaderText}' должно быть положительное число \n";
                    e.Cancel = true;
                }

                var cellsWithPotentialCMBErrors = new[] { row.Cells["species_id"], row.Cells["owner_id"] };
                foreach (var cell in cellsWithPotentialCMBErrors)
                {
                    if (cell.Value == null)
                    {
                        row.ErrorText += $"Выберите значение в столбце '{cell.OwningColumn.HeaderText}' \n";
                        e.Cancel = true;
                    }
                }

                if (!e.Cancel && !Database.CheckPet((int?)row.Cells["id"].Value, name, age, Convert.ToInt32(row.Cells["species_id"].Value), Convert.ToInt32(row.Cells["owner_id"].Value)))
                {
                    row.ErrorText += "Такой питомец уже существует";
                    e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    var id = (int?)row.Cells["id"].Value;
                    if (id.HasValue)
                        Database.UpdatePet(name, age, Convert.ToInt32(row.Cells["species_id"].Value), Convert.ToInt32(row.Cells["owner_id"].Value), id.Value);
                    else
                        row.Cells["id"].Value = Database.InsertPet(name, age, Convert.ToInt32(row.Cells["species_id"].Value), Convert.ToInt32(row.Cells["owner_id"].Value));

                    var dataDict = new Dictionary<string, object>();
                    foreach (var columnsName in new[] { "name", "age" })
                    {
                        dataDict[columnsName] = EditAndHash.DeleteExtraSpaces((row.Cells[columnsName].Value).ToString());
                    }
                    row.Tag = Tuple.Create(dataDict, Convert.ToInt32(row.Cells["species_id"].Value), Convert.ToInt32(row.Cells["owner_id"].Value));


                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ErrorText = "";
                    }
                }
            }
        }

        private void dgvPets_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (role == 0)
            {
                e.Cancel = true;
                return;
            }

            var id = (int?)e.Row.Cells["id"].Value;
            if (id.HasValue)
                Database.DeletePet(id.Value);
        }

        private void dgvPets_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPets.CurrentCell.OwningColumn.HeaderText == "Вид")
            {
                var species = (int?)dgvPets.CurrentCell.Value;
                if (species.HasValue && species.Value == -1)
                {

                    AddSpecies addSpeciesW = new AddSpecies();
                    addSpeciesW.ShowDialog();
                    if (addSpeciesW.DialogResult == DialogResult.OK)
                    {
                        ((DataGridViewComboBoxColumn)dgvPets.Columns["species_id"]).DataSource = Database.AddNew_Option(Database.Species());
                        ((DataGridViewComboBoxCell)dgvPets.CurrentCell).Value = addSpeciesW.id;
                    }
                    else
                    {
                        var key = (Tuple<Dictionary<string, object>, int, int>)dgvPets.Rows[e.RowIndex].Tag;
                        if (!(key is null))
                            dgvPets.CurrentCell.Value = key.Item2;
                        else
                            dgvPets.CurrentCell.Value = null;
                    }
                }
                return;
            }
            else
            if (dgvPets.CurrentCell.OwningColumn.HeaderText == "Владелец")
            {
                var owner = (int?)dgvPets.CurrentCell.Value;
                if (owner.HasValue && owner.Value == -1)
                {

                    AddOwner addOwnerW = new AddOwner();
                    addOwnerW.ShowDialog();
                    if (addOwnerW.DialogResult == DialogResult.OK)
                    {
                        ((DataGridViewComboBoxColumn)dgvPets.Columns["owner_id"]).DataSource = Database.AddNew_Option(Database.Owners());
                        ((DataGridViewComboBoxCell)dgvPets.CurrentCell).Value = addOwnerW.id;
                    }
                    else
                    {
                        var key = (Tuple<Dictionary<string, object>, int, int>)dgvPets.Rows[e.RowIndex].Tag;
                        if (!(key is null))
                            dgvPets.CurrentCell.Value = key.Item3;
                        else
                            dgvPets.CurrentCell.Value = null;
                    }

                }
                return;
            }

            if (!dgvPets.Rows[e.RowIndex].IsNewRow)
            {
                dgvPets[e.ColumnIndex, e.RowIndex].ErrorText = "Значение изменено и пока не сохранено.";
                if (dgvPets.Rows[e.RowIndex].Tag != null)
                {
                    var key = (Tuple<Dictionary<string, object>, int, int>)dgvPets.Rows[e.RowIndex].Tag;
                    var dict = key.Item1;
                    dgvPets[e.ColumnIndex, e.RowIndex].ErrorText += "\nПредыдущее значение - " +
                                                                  (dict)[dgvPets.Columns[e.ColumnIndex].Name];
                }
            }

        }

        private void dgvPets_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dgvPets.IsCurrentRowDirty)
            {
                dgvPets.CancelEdit();
                if (dgvPets.CurrentRow.Cells["id"].Value != null)
                {
                    var key = (Tuple<Dictionary<string, object>, int, int>)dgvPets.Rows[dgvPets.CurrentCell.RowIndex].Tag;
                    var dict = key.Item1;

                    dgvPets.CurrentRow.ErrorText = "";
                    foreach (var kvp in dict)
                    {
                        dgvPets.CurrentRow.Cells[kvp.Key].Value = kvp.Value;
                        dgvPets.CurrentRow.Cells[kvp.Key].ErrorText = "";
                    }
                }
                else
                {
                    dgvPets.Rows.Remove(dgvPets.CurrentRow);
                }
            }
        }

        private void ClearNameSearch()
        {
            dgvNameSearch.Rows.Clear();
            dgvNameSearch.Columns.Clear();
        }

        private void ClearSpeciesSearch()
        {
            dgvSpeciesSearch.Rows.Clear();
            dgvSpeciesSearch.Columns.Clear();
        }

        private void bNameSearch_Click(object sender, EventArgs e)
        {
            string name = EditAndHash.DeleteExtraSpaces(tbNameSearch.Text);
            tbNameSearch.Text = name;
            name += "%";
            if (name != "%" && Database.FindName(name))
            {
                Database.InitializeDgvNameSearch(ref dgvNameSearch, name);
                lb404name.Visible = false;
            }
            else
            {
                lb404name.Visible = true;
                ClearNameSearch();
            }
        }

        private void bNameSearchCancel_Click(object sender, EventArgs e)
        {
            ClearNameSearch();
            lb404name.Visible = false;
            tbNameSearch.Text = "";
        }

        private void bSpeciesSearch_Click(object sender, EventArgs e)
        {
            int id = (int)cmbSpeciesSearch.SelectedValue;
            if (Database.FindSpecies(id))
            {
                Database.InitializeDgvSpeciesSearch(ref dgvSpeciesSearch, id);
                lb404species.Visible = false;
            }
            else
            {
                ClearSpeciesSearch();
                lb404species.Visible = true;
            }
        }

        private void bSpeciesSearchCancel_Click(object sender, EventArgs e)
        {
            ClearSpeciesSearch();
            lb404species.Visible = false;
            cmbSpeciesSearch.SelectedValue = -1;
        }

        private void cmbSpeciesSearch_SelectedValueChanged(object sender, EventArgs e)
        {
            bSpeciesSearch.Enabled = !(cmbSpeciesSearch.SelectedValue is null);
        }

        private void dgvAppointments_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            var row = dgvAppointments.Rows[e.RowIndex];
            if (dgvAppointments.IsCurrentRowDirty)
            {
                row.ErrorText = "";
                var cellsWithPotentialCMBErrors = new[] { row.Cells["vets"], row.Cells["pets"], row.Cells["procedures"] };
                foreach (var cell in cellsWithPotentialCMBErrors)
                {
                    if (cell.Value == null)
                    {
                        row.ErrorText += $"Выберите значение в столбце '{cell.OwningColumn.HeaderText}' \n";
                        e.Cancel = true;
                    }
                }
                var key = (Tuple<int, int, int, DateTime>)row.Tag;
                if (!e.Cancel && !Database.CheckAppointment(Convert.ToInt32(row.Cells["vets"].Value), Convert.ToInt32(row.Cells["pets"].Value), Convert.ToInt32(row.Cells["procedures"].Value), (DateTime)row.Cells["appointment_date"].Value) && key != null && !(key.Item1 == Convert.ToInt32(row.Cells["vets"].Value) && key.Item2 == Convert.ToInt32(row.Cells["pets"].Value) && key.Item3 == Convert.ToInt32(row.Cells["procedures"].Value) && key.Item4 == (DateTime)row.Cells["appointment_date"].Value))
                {
                    row.ErrorText = $"Такая запись на прием уже существует";
                    e.Cancel = true;
                }

                string time = EditAndHash.DeleteExtraSpaces((string)row.Cells["appointment_time"].Value);
                if (!e.Cancel && !timeRegex.IsMatch(time))
                {
                    row.ErrorText += $"В столбце '{row.Cells["appointment_time"].OwningColumn.HeaderText}' должно быть время в формате ЧЧ:ММ\n";
                    e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    //var key = (Tuple<int, int, int, DateTime>)row.Tag;
                    if (key != null)
                        if (key.Item1 != Convert.ToInt32(row.Cells["vets"].Value) || key.Item2 != Convert.ToInt32(row.Cells["pets"].Value) || key.Item3 != Convert.ToInt32(row.Cells["procedures"].Value) || key.Item4 != (DateTime)row.Cells["appointment_date"].Value)
                        Database.UpdateAppointment(
                            Convert.ToInt32(row.Cells["vets"].Value),
                            Convert.ToInt32(row.Cells["pets"].Value),
                            Convert.ToInt32(row.Cells["procedures"].Value),
                            (DateTime)row.Cells["appointment_date"].Value,
                            time,
                            key.Item1,
                            key.Item2,
                            key.Item3,
                            key.Item4
                            );
                        else
                        Database.UpdateAppointment(
                            key.Item1,
                            key.Item2,
                            key.Item3,
                            key.Item4,
                            time
                            );
                    else
                        Database.InsertAppointment(
                            Convert.ToInt32(row.Cells["vets"].Value),
                            Convert.ToInt32(row.Cells["pets"].Value), 
                            Convert.ToInt32(row.Cells["procedures"].Value),
                            (DateTime)row.Cells["appointment_date"].Value, 
                            time
                            );

                    row.Tag = Tuple.Create((int)row.Cells["vets"].Value, (int)row.Cells["pets"].Value, (int)row.Cells["procedures"].Value, (DateTime)row.Cells["appointment_date"].Value);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ErrorText = "";
                    }
                }
            }
        }

        private void dgvAppointments_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (role == 0)
            {
                e.Cancel = true;
                return;
            }

            Database.DeleteAppointment(Convert.ToInt32(e.Row.Cells["vets"].Value), Convert.ToInt32(e.Row.Cells["pets"].Value), Convert.ToInt32(e.Row.Cells["procedures"].Value), (DateTime)e.Row.Cells["appointment_date"].Value);
        }

        private void dgvAppointments_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && dgvAppointments.IsCurrentRowDirty)
            {
                dgvAppointments.CancelEdit();
                var key = (Tuple<int, int, int, DateTime>)dgvAppointments.CurrentRow.Tag;
                if (key != null)
                {
                    dgvAppointments.CurrentRow.ErrorText = "";

                    dgvAppointments.CurrentRow.Cells["vets"].Value = key.Item1;
                    dgvAppointments.CurrentRow.Cells["vets"].ErrorText = "";

                    dgvAppointments.CurrentRow.Cells["pets"].Value = key.Item2;
                    dgvAppointments.CurrentRow.Cells["pets"].ErrorText = "";

                    dgvAppointments.CurrentRow.Cells["procedures"].Value = key.Item3;
                    dgvAppointments.CurrentRow.Cells["procedures"].ErrorText = "";

                    dgvAppointments.CurrentRow.Cells["appointment_date"].Value = key.Item4;
                    dgvAppointments.CurrentRow.Cells["appointment_date"].ErrorText = "";
                }
                else
                {
                    dgvAppointments.Rows.Remove(dgvAppointments.CurrentRow);
                }
            }
        }
        

        private void dgvPets_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!(dgvPets.CurrentCell is DataGridViewComboBoxCell))
                return;

            if (dgvPets.IsCurrentCellDirty)
            {
                dgvPets.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }

        }

        private void dgvAppointments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!(dgvAppointments.CurrentCell is DataGridViewComboBoxCell))
                return;

            if (dgvAppointments.IsCurrentCellDirty)
            {
                dgvAppointments.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            
        }

        private void dgvAppointments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAppointments.CurrentCell.OwningColumn.HeaderText == "Ветеринар")
            {
                var vet = (int?)dgvAppointments.CurrentCell.Value;
                if (vet.HasValue && vet.Value == -1)
                {

                    AddVet addVetW = new AddVet();
                    addVetW.ShowDialog();
                    if (addVetW.DialogResult == DialogResult.OK)
                    {
                        ((DataGridViewComboBoxColumn)dgvAppointments.Columns["vets"]).DataSource = Database.AddNew_Option(Database.Vets());
                        ((DataGridViewComboBoxCell)dgvAppointments.CurrentCell).Value = addVetW.id;
                    }
                    else
                    {
                        var key = (Tuple<int, int, int, DateTime>)dgvAppointments.Rows[e.RowIndex].Tag;
                        if (!(key is null))
                            dgvAppointments.CurrentCell.Value = key.Item1;
                        else
                            dgvAppointments.CurrentCell.Value = null;
                    }
                }
            }
            else
            if (dgvAppointments.CurrentCell.OwningColumn.HeaderText == "Питомец")
            {
                var pet = (int?)dgvAppointments.CurrentCell.Value;
                if (pet.HasValue && pet.Value == -1)
                {

                    AddPet addPetW = new AddPet();
                    addPetW.ShowDialog();
                    if (addPetW.DialogResult == DialogResult.OK)
                    {
                        ((DataGridViewComboBoxColumn)dgvAppointments.Columns["pets"]).DataSource = Database.AddNew_Option(Database.Pets());
                        ((DataGridViewComboBoxCell)dgvAppointments.CurrentCell).Value = addPetW.id;
                    }
                    else
                    {
                        var key = (Tuple<int, int, int, DateTime>)dgvAppointments.Rows[e.RowIndex].Tag;
                        if (!(key is null))
                            dgvAppointments.CurrentCell.Value = key.Item2;
                        else
                            dgvAppointments.CurrentCell.Value = null;
                    }
                }
            }
            else
            if (dgvAppointments.CurrentCell.OwningColumn.HeaderText == "Процедура")
            {
                var proc = (int?)dgvAppointments.CurrentCell.Value;
                if (proc.HasValue && proc.Value == -1)
                {

                    AddProcedure addProcW = new AddProcedure();
                    addProcW.ShowDialog();
                    if (addProcW.DialogResult == DialogResult.OK)
                    {
                        ((DataGridViewComboBoxColumn)dgvAppointments.Columns["procedures"]).DataSource = Database.AddNew_Option(Database.Procedures());
                        ((DataGridViewComboBoxCell)dgvAppointments.CurrentCell).Value = addProcW.id;
                    }
                    else
                    {
                        var key = (Tuple<int, int, int, DateTime>)dgvAppointments.Rows[e.RowIndex].Tag;
                        if (!(key is null))
                            dgvAppointments.CurrentCell.Value = key.Item3;
                        else
                            dgvAppointments.CurrentCell.Value = null;
                    }
                }
            }
        }

        private void tControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Database.InitializeDgvSpecies(ref dgvSpecies);
            Database.InitializeDgvProcedures(ref dgvProcedures);
            Database.InitializeDgvPets(ref dgvPets);
            Database.InitializeDgvAppointments(ref dgvAppointments);
            cmbSpeciesSearch.DataSource = Database.Species();
        }

        private void dgvNameSearch_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {

            e.Cancel = true;
            return;

        }

        private void dgvSpeciesSearch_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
            return;
        }
    }
}
