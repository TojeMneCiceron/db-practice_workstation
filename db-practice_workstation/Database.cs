using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace arm
{
    class Database
    {
        private static readonly string sConnStr = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "vvet",
            Username = "postgres",
            Password = "postgres"
        }.ConnectionString;

        public static void Connect()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
            }
        }

        public static bool CheckLogin(string login)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT count(*) FROM users WHERE lower(login) = lower(@currentLogin)"
                })
                {
                    sCommand.Parameters.AddWithValue("@currentLogin", login);
                    return (long)sCommand.ExecuteScalar() <= 0;
                }
            }
        }

        public static void NewUserReg(string login, string password)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO users (login, password_hash) VALUES (@login, @password)"
                })
                {
                    string salt = EditAndHash.Salt();
                    sCommand.Parameters.AddWithValue("@login", login);
                    sCommand.Parameters.AddWithValue("@password", EditAndHash.Hash(password, salt));
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void NewUserAdm(string login, string password, DateTime date, ref string password_hash, ref int id, int role_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO users (login, password_hash, role_id, registration_date) VALUES (@login, @password, @role, @date) RETURNING id"
                })
                {
                    string salt = EditAndHash.Salt();
                    password_hash = EditAndHash.Hash(password, salt);
                    sCommand.Parameters.AddWithValue("@login", login);
                    sCommand.Parameters.AddWithValue("@password", password_hash);
                    sCommand.Parameters.AddWithValue("@date", date);
                    sCommand.Parameters.AddWithValue("@role", role_id);
                    id = (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static int Authorize(string login, string password)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT * FROM users WHERE lower(login) = lower(@currentLogin)"
                })
                {
                    sCommand.Parameters.AddWithValue("@currentLogin", login);
                    using (var reader = sCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return 0;
                        reader.Read();
                        string dbhash = (string)reader["password_hash"];
                        int role = (int)reader["role_id"];
                        return EditAndHash.Verify(password, dbhash) ? role : 0;
                    }
                }
            }

        }

        public static void EditUser(string newLogin, string oldLogin, DateTime newDate, int role_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE users SET login = @newLogin, role_id = @newRole, registration_date = @newDate WHERE lower(login) = lower(@oldLogin)"
                })
                {
                    sCommand.Parameters.AddWithValue("@newLogin", newLogin);
                    sCommand.Parameters.AddWithValue("@newDate", newDate);
                    sCommand.Parameters.AddWithValue("@oldLogin", oldLogin);
                    sCommand.Parameters.AddWithValue("@newRole", role_id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void EditUser(string newLogin, string oldLogin, string newPassword, DateTime newDate, ref string password_hash, int role_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE users SET login = @newLogin, password_hash = @newPassword, role_id = @newRole, registration_date = @newDate WHERE lower(login) = lower(@oldLogin)"
                })
                {
                    password_hash = EditAndHash.Hash(newPassword, EditAndHash.Salt());
                    sCommand.Parameters.AddWithValue("@newLogin", newLogin);
                    sCommand.Parameters.AddWithValue("@newPassword", password_hash);
                    sCommand.Parameters.AddWithValue("@newDate", newDate);
                    sCommand.Parameters.AddWithValue("@oldLogin", oldLogin);
                    sCommand.Parameters.AddWithValue("@newRole", role_id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void lvUsers(ref ListView lv)
        {
            lv.Columns.Add("Логин");
            lv.Columns.Add("Пароль");
            lv.Columns.Add("Роль");
            lv.Columns.Add("Дата регистрации");
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        SELECT users.id as u_id, login, password_hash, roles.id as r_id, roles.name as r_name, registration_date
                        FROM users
                        JOIN roles ON users.role_id = roles.id
                        ORDER BY u_id;"
                };
                using (var reader = sCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ListViewItem lvi = new ListViewItem(new[]
                        {
                            (string) reader["login"],
                            (string) reader["password_hash"],
                            (string) reader["r_name"],
                            ((DateTime) reader["registration_date"]).ToLongDateString()
                        })
                        {
                            Tag = Tuple.Create((int)reader["u_id"], (DateTime)reader["registration_date"], (int)reader["r_id"])
                        };
                        lv.Items.Add(lvi);
                    }
                }
            }
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public static void DeleteUsers(ref ListView lv)
        {
            if (lv.SelectedItems.Count == 0)
                return;
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"
                        DELETE
                        FROM users
                        WHERE id = ANY(@ids);"
                };
                var ids = lv.SelectedItems.Cast<ListViewItem>().Select(item => ((Tuple<int, DateTime, int>)item.Tag).Item1).ToArray();
                sCommand.Parameters.AddWithValue("@ids", ids);
                sCommand.ExecuteNonQuery();
                foreach (ListViewItem selectedItem in lv.SelectedItems)
                {
                    lv.Items.Remove(selectedItem);
                }
            };
        }

        public static DataTable Roles()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id, name FROM roles;"
                };
                DataTable tRoles = new DataTable();
                tRoles.Load(sCommand.ExecuteReader());
                return tRoles;
            }
        }

        public static void InitializeDgvProcedures(ref DataGridView dgvProcedures)
        {
            dgvProcedures.Rows.Clear();
            dgvProcedures.Columns.Clear();
            dgvProcedures.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                Visible = false
            });
            var procedureNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = @"Название процедуры"
            };
            var costColumn = new DataGridViewTextBoxColumn
            {
                Name = "cost",
                HeaderText = @"Стоимость"
            };
            dgvProcedures.Columns.AddRange(procedureNameColumn, costColumn);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = "SELECT * FROM procedures ORDER BY id";
                    sCommand.Connection = sConn;
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var dataDict = new Dictionary<string, object>();
                        foreach (var columnsName in new[] { "name", "cost" })
                        {
                            dataDict[columnsName] = reader[columnsName];
                        }

                        var rowIdx = dgvProcedures.Rows.Add(reader["id"], reader["name"], reader["cost"]);
                        dgvProcedures.Rows[rowIdx].Tag = dataDict;
                    }
                }
            }
        }

        public static int InsertProcedure(string name, int cost)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO procedures (name, cost) VALUES (@name, @cost) RETURNING id"
                })
                {

                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@cost", cost);
                    return (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static void UpdateProcedure(string name, int cost, int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE procedures SET name = @name, cost = @cost WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@cost", cost);
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteProcedure(int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"DELETE FROM procedures WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckProcedure(int? id, string name)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id FROM procedures WHERE name = @name"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    var f_id = (int?)sCommand.ExecuteScalar();

                    return !f_id.HasValue || f_id == id;
                }
            }
        }

        public static void InitializeDgvSpecies(ref DataGridView dgvSpecies)
        {
            dgvSpecies.Rows.Clear();
            dgvSpecies.Columns.Clear();
            dgvSpecies.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                Visible = false
            });
            var speciesNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = @"Название вида"
            };
            var isEndangeredColumn = new DataGridViewCheckBoxColumn
            {
                Name = "is_endangered",
                HeaderText = @"Вымирающий вид",
            };
            var avgLifeExpectancyNumber = new DataGridViewTextBoxColumn
            {
                Name = "average_life_expectancy",
                HeaderText = @"Средняя продолжительность жизни"
            };
            dgvSpecies.Columns.AddRange(speciesNameColumn, isEndangeredColumn, avgLifeExpectancyNumber);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = "SELECT * FROM species ORDER BY id";
                    sCommand.Connection = sConn;
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var dataDict = new Dictionary<string, object>();
                        foreach (var columnsName in new[] { "name", "is_endangered", "average_life_expectancy" })
                        {
                            dataDict[columnsName] = reader[columnsName].ToString();
                        }
                        var rowIdx = dgvSpecies.Rows.Add(reader["id"], reader["name"], reader["is_endangered"], reader["average_life_expectancy"]);
                        dgvSpecies.Rows[rowIdx].Tag = dataDict;
                    }
                }
            }
        }

        public static int InsertSpecies(string name, bool is_en, int avg_l_e)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO species (name, is_endangered, average_life_expectancy) VALUES (@name, @is_en, @avg_l_e) RETURNING id"
                })
                {

                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@is_en", is_en);
                    sCommand.Parameters.AddWithValue("@avg_l_e", avg_l_e);
                    return (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static void UpdateSpecies(string name, bool is_en, int avg_l_e, int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE species SET name = @name, is_endangered = @is_en, average_life_expectancy = @avg_l_e WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@is_en", is_en);
                    sCommand.Parameters.AddWithValue("@avg_l_e", avg_l_e);
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteSpecies(int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"DELETE FROM species WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckSpecies(int? id, string name)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id FROM species WHERE name = @name"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    var f_id = (int?)sCommand.ExecuteScalar();

                    return !f_id.HasValue || f_id == id;
                }
            }
        }

        public static DataTable Species()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as species_id, name as species_name FROM species ORDER BY id;"
                };
                DataTable tSpecies = new DataTable();
                tSpecies.Load(sCommand.ExecuteReader());
                return tSpecies;
            }
        }

        public static DataTable Owners()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as owner_id, name as owner_name FROM owners ORDER BY id;"
                };
                DataTable tOwners = new DataTable();
                tOwners.Load(sCommand.ExecuteReader());
                return tOwners;
            }
        }

        public static void InitializeDgvPets(ref DataGridView dgvPets)
        {
            dgvPets.Rows.Clear();
            dgvPets.Columns.Clear();
            dgvPets.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                Visible = false
            });
            var petNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = @"Имя питомца"
            };
            var ageColumn = new DataGridViewTextBoxColumn
            {
                Name = "age",
                HeaderText = @"Возраст"
            };
            var speciesColumn = new DataGridViewComboBoxColumn
            {
                Name = "species_id",
                HeaderText = @"Вид",
                ValueMember = "species_id",
                DisplayMember = "species_name",
                DataSource = AddNew_Option(Species())
            };
            var ownerColumn = new DataGridViewComboBoxColumn
            {
                Name = "owner_id",
                HeaderText = @"Владелец",
                ValueMember = "owner_id",
                DisplayMember = "owner_name",
                DataSource = AddNew_Option(Owners())
            };
            dgvPets.Columns.AddRange(petNameColumn, ageColumn, speciesColumn, ownerColumn);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = "SELECT * FROM pets ORDER BY id";
                    sCommand.Connection = sConn;
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var dataDict = new Dictionary<string, object>();
                        foreach (var columnsName in new[] { "name", "age" })
                        {
                            dataDict[columnsName] = reader[columnsName];
                        }

                        var rowIdx = dgvPets.Rows.Add(reader["id"], reader["name"], reader["age"], reader["species_id"], reader["owner_id"]);
                        dgvPets.Rows[rowIdx].Tag = Tuple.Create(dataDict, (int)reader["species_id"], (int)reader["owner_id"]);
                    }
                }
            }
        }

        public static int InsertPet(string name, int age, int species_id, int owner_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO pets (name, age, species_id, owner_id) VALUES (@name, @age, @s_id, @o_id) RETURNING id"
                })
                {

                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@age", age);
                    sCommand.Parameters.AddWithValue("@s_id", species_id);
                    sCommand.Parameters.AddWithValue("@o_id", owner_id);
                    return (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static void UpdatePet(string name, int age, int species_id, int owner_id, int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE pets SET name = @name, age = @age, species_id = @s_id, owner_id = @o_id WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@age", age);
                    sCommand.Parameters.AddWithValue("@s_id", species_id);
                    sCommand.Parameters.AddWithValue("@o_id", owner_id);
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void DeletePet(int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"DELETE FROM pets WHERE id = @id"
                })
                {
                    sCommand.Parameters.AddWithValue("@id", id);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckPet(int? id, string name, int age, int species_id, int owner_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id FROM pets WHERE name = @name AND age = @age AND species_id = @s_id AND owner_id = @o_id"
                })
                {
                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@age", age);
                    sCommand.Parameters.AddWithValue("@s_id", species_id);
                    sCommand.Parameters.AddWithValue("@o_id", owner_id);
                    var f_id = (int?)sCommand.ExecuteScalar();

                    return !f_id.HasValue || f_id == id;
                }
            }

        }

        public static DataTable Vets()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as vet_id, name as vet_name FROM vets ORDER BY id;"
                };
                DataTable tVets = new DataTable();
                tVets.Load(sCommand.ExecuteReader());
                return tVets;
            }
        }

        public static DataTable Pets()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as pet_id, name as pet_name FROM pets ORDER BY id;"
                };
                DataTable tPets = new DataTable();
                tPets.Load(sCommand.ExecuteReader());
                return tPets;
            }
        }

        public static DataTable Procedures()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as procedure_id, name as procedure_name FROM procedures ORDER BY id;"
                };
                DataTable tProcedures = new DataTable();
                tProcedures.Load(sCommand.ExecuteReader());
                return tProcedures;
            }
        }
        public static void InitializeDgvAppointments(ref DataGridView dgvAppointments)
        {
            dgvAppointments.Rows.Clear();
            dgvAppointments.Columns.Clear();
            var vetColumn = new DataGridViewComboBoxColumn
            {
                Name = "vets",
                HeaderText = @"Ветеринар",
                ValueMember = "vet_id",
                DisplayMember = "vet_name",
                DataSource = AddNew_Option(Vets())
            };
            var petColumn = new DataGridViewComboBoxColumn
            {
                Name = "pets",
                HeaderText = @"Питомец",
                ValueMember = "pet_id",
                DisplayMember = "pet_name",
                DataSource = AddNew_Option(Pets())
            };
            var procedureColumn = new DataGridViewComboBoxColumn
            {
                Name = "procedures",
                HeaderText = @"Процедура",
                ValueMember = "procedure_id",
                DisplayMember = "procedure_name",
                DataSource = AddNew_Option(Procedures())
            };
            var dateColumn = new CalendarColumn
            {
                Name = "appointment_date",
                HeaderText = @"Дата приема"
            };

            var timeColumn = new DataGridViewTextBoxColumn
            {
                Name = "appointment_time",
                HeaderText = @"Время приема"
            };
            dgvAppointments.Columns.AddRange(vetColumn, petColumn, procedureColumn, dateColumn, timeColumn);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = "SELECT * FROM appointments";
                    sCommand.Connection = sConn;
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        
                        var rowIdx = dgvAppointments.Rows.Add(reader["vet_id"], reader["pet_id"], reader["procedure_id"], reader["appointment_date"], reader["appointment_time"]);
                        dgvAppointments.Rows[rowIdx].Tag = Tuple.Create((int)reader["vet_id"], (int)reader["pet_id"], (int)reader["procedure_id"], (DateTime)reader["appointment_date"]);
                    }
                }
            }
        }

        public static bool FindName(string name)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = @"SELECT count(*)
                                            FROM appointments
                                            JOIN pets ON appointments.pet_id = pets.id
                                            WHERE name LIKE @name";
                    sCommand.Connection = sConn;
                    sCommand.Parameters.AddWithValue("@name", name);
                    return (long)sCommand.ExecuteScalar() > 0;
                }
            }
        }

        public static void InitializeDgvNameSearch(ref DataGridView dgvNameSearch, string name)
        {
            dgvNameSearch.Rows.Clear();
            dgvNameSearch.Columns.Clear();
            dgvNameSearch.ReadOnly = true;
            var vetColumn = new DataGridViewTextBoxColumn
            {
                Name = "vets",
                HeaderText = @"Ветеринар"
            };
            var petColumn = new DataGridViewTextBoxColumn
            {
                Name = "pets",
                HeaderText = @"Питомец"
            };
            var procedureColumn = new DataGridViewTextBoxColumn
            {
                Name = "procedures",
                HeaderText = @"Процедура"
            };
            var dateColumn = new CalendarColumn
            {
                Name = "appointment_date",
                HeaderText = @"Дата приема"
            };
            var timeColumn = new DataGridViewTextBoxColumn
            {
                Name = "appointment_time",
                HeaderText = @"Время приема"
            };
            dgvNameSearch.Columns.AddRange(vetColumn, petColumn, procedureColumn, dateColumn, timeColumn);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = @"SELECT vets.name as vet_name,
                                                   pets.name as pet_name,
                                                   procedures.name as proc_name,
                                                   appointment_date,
                                                   appointment_time
                                            FROM appointments
                                            JOIN pets ON appointments.pet_id = pets.id
                                            JOIN procedures ON appointments.procedure_id = procedures.id
                                            JOIN vets ON appointments.vet_id = vets.id
                                            WHERE pets.name LIKE @name";
                    sCommand.Connection = sConn;
                    sCommand.Parameters.AddWithValue("@name", name);
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var rowIdx = dgvNameSearch.Rows.Add(reader["vet_name"], reader["pet_name"], reader["proc_name"], reader["appointment_date"], reader["appointment_time"]);
                    }
                }
            }
        }

        public static bool FindSpecies(int id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = @"SELECT count(*)
                                            FROM pets
                                            WHERE species_id = @s_id";
                    sCommand.Connection = sConn;
                    sCommand.Parameters.AddWithValue("@s_id", id);
                    return (long)sCommand.ExecuteScalar() > 0;
                }
            }
        }

        public static void InitializeDgvSpeciesSearch(ref DataGridView dgvSpeciesSearch, int id)
        {
            dgvSpeciesSearch.Rows.Clear();
            dgvSpeciesSearch.Columns.Clear();
            dgvSpeciesSearch.ReadOnly = true;
            var petNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "name",
                HeaderText = @"Имя питомца"
            };
            var ageColumn = new DataGridViewTextBoxColumn
            {
                Name = "age",
                HeaderText = @"Возраст"
            };
            var ownerColumn = new DataGridViewTextBoxColumn
            {
                Name = "owner",
                HeaderText = @"Владелец"
            };
            dgvSpeciesSearch.Columns.AddRange(petNameColumn, ageColumn, ownerColumn);
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand())
                {
                    sCommand.CommandText = @"SELECT pets.name as pet_name, age, owners.name as owner_name
                                            FROM pets
                                            JOIN owners ON pets.owner_id = owners.id
                                            WHERE species_id = @s_id";
                    sCommand.Connection = sConn;
                    sCommand.Parameters.AddWithValue("s_id", id);
                    var reader = sCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var rowIdx = dgvSpeciesSearch.Rows.Add(reader["pet_name"], reader["age"], reader["owner_name"]);
                    }
                }
            }
        }

        public static int InsertVet(string name, int cat_id, int we, string phone, int vc_id)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO vets (name, category_id, work_experience, phone_number, vet_clinic_id) VALUES (@name, @cat_id, @we, @phone, @vc_id) RETURNING id"
                })
                {

                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@cat_id", cat_id);
                    sCommand.Parameters.AddWithValue("@we", we);
                    sCommand.Parameters.AddWithValue("@phone", phone);
                    sCommand.Parameters.AddWithValue("@vc_id", vc_id);
                    return (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static int InsertOwner(string name, string phone)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO owners (name, phone_number) VALUES (@name, @phone) RETURNING id"
                })
                {

                    sCommand.Parameters.AddWithValue("@name", name);
                    sCommand.Parameters.AddWithValue("@phone", phone);
                    return (int)sCommand.ExecuteScalar();
                }
            }
        }

        public static bool CheckOwnerPhone(string phone)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT count(*) FROM owners WHERE phone_number = @phone"
                })
                {
                    sCommand.Parameters.AddWithValue("@phone", phone);
                    return (long)sCommand.ExecuteScalar() <= 0;
                }
            }
        }

        public static bool CheckVetPhone(string phone)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT count(*) FROM vets WHERE phone_number = @phone"
                })
                {
                    sCommand.Parameters.AddWithValue("@phone", phone);
                    return (long)sCommand.ExecuteScalar() <= 0;
                }
            }
        }

        public static DataTable VetClinics()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as vc_id, address as vc_address FROM vet_clinics ORDER BY id;"
                };
                DataTable tVC = new DataTable();
                tVC.Load(sCommand.ExecuteReader());
                return tVC;
            }
        }

        public static DataTable Categories()
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT id as category_id, name as category_name FROM categories ORDER BY id;"
                };
                DataTable tCategories = new DataTable();
                tCategories.Load(sCommand.ExecuteReader());
                return tCategories;
            }
        }

        public static void InsertAppointment(int vet_id, int pet_id, int proc_id, DateTime date, string time)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"INSERT INTO appointments VALUES (@vet, @pet, @proc, @date, @time)"
                })
                {

                    sCommand.Parameters.AddWithValue("@vet", vet_id);
                    sCommand.Parameters.AddWithValue("@pet", pet_id);
                    sCommand.Parameters.AddWithValue("@proc", proc_id);
                    sCommand.Parameters.AddWithValue("@date", date);
                    sCommand.Parameters.AddWithValue("@time", time);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateAppointment(int vet_id, int pet_id, int proc_id, DateTime date, string time, int old_vet_id, int old_pet_id, int old_proc_id, DateTime old_date)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE appointments SET vet_id = @vet_id, pet_id = @pet_id, procedure_id = @proc_id, appointment_date = @date, appointment_time = @time WHERE vet_id = @old_vet_id AND pet_id = @old_pet_id AND procedure_id = @old_proc_id AND appointment_date = @old_date"
                })
                {
                    sCommand.Parameters.AddWithValue("@vet_id", vet_id);
                    sCommand.Parameters.AddWithValue("@pet_id", pet_id);
                    sCommand.Parameters.AddWithValue("@proc_id", proc_id);
                    sCommand.Parameters.AddWithValue("@date", date);
                    sCommand.Parameters.AddWithValue("@time", time);
                    sCommand.Parameters.AddWithValue("@old_vet_id", old_vet_id);
                    sCommand.Parameters.AddWithValue("@old_pet_id", old_pet_id);
                    sCommand.Parameters.AddWithValue("@old_proc_id", old_proc_id);
                    sCommand.Parameters.AddWithValue("@old_date", old_date);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateAppointment(int vet_id, int pet_id, int proc_id, DateTime date, string time)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"UPDATE appointments SET appointment_time = @time WHERE vet_id = @vet_id AND pet_id = @pet_id AND procedure_id = @proc_id AND appointment_date = @date"
                })
                {
                    sCommand.Parameters.AddWithValue("@vet_id", vet_id);
                    sCommand.Parameters.AddWithValue("@pet_id", pet_id);
                    sCommand.Parameters.AddWithValue("@proc_id", proc_id);
                    sCommand.Parameters.AddWithValue("@date", date);
                    sCommand.Parameters.AddWithValue("@time", time);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteAppointment(int vet_id, int pet_id, int proc_id, DateTime date)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"DELETE FROM appointments WHERE vet_id = @vet_id AND pet_id = @pet_id AND procedure_id = @proc_id AND appointment_date = @date"
                })
                {
                    sCommand.Parameters.AddWithValue("@vet_id", vet_id);
                    sCommand.Parameters.AddWithValue("@pet_id", pet_id);
                    sCommand.Parameters.AddWithValue("@proc_id", proc_id);
                    sCommand.Parameters.AddWithValue("@date", date);
                    sCommand.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckAppointment(int vet_id, int pet_id, int proc_id, DateTime date)
        {
            using (var sConn = new NpgsqlConnection(sConnStr))
            {
                sConn.Open();
                using (var sCommand = new NpgsqlCommand
                {
                    Connection = sConn,
                    CommandText = @"SELECT count(*) FROM appointments WHERE vet_id = @vet_id AND pet_id = @pet_id AND procedure_id = @proc_id AND appointment_date::date = @date"
                })
                {
                    sCommand.Parameters.AddWithValue("@vet_id", vet_id);
                    sCommand.Parameters.AddWithValue("@pet_id", pet_id);
                    sCommand.Parameters.AddWithValue("@proc_id", proc_id);
                    sCommand.Parameters.AddWithValue("@date", date.Date);
                    return (long)sCommand.ExecuteScalar() <= 0;
                }
            }
        }

        public static DataTable AddNew_Option(DataTable tab)
        {
            tab.Rows.Add(-1, "--Добавить--");
            return tab;
        }
    }
}
