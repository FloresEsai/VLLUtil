using System;
using System.Data.SqlClient;
using System.Windows.Forms;
/*
    
    VLL Utility TODO:
    - need to dynamically change the input label boxes to match the current table
    - need to make sure the sql queries can handle single quotes
    - optimize initializeForm, setTextBoxProperties, and clearInputFields
    - update add button to include the size limits, and duplicates from update button
 
 */
namespace VLLUtility
{
    public partial class MainForm : Form
    {
        // Declare class-level controls
        private ComboBox tableComboBox;
        private ListView recordListView;
        private TextBox txtField1, txtField2, txtField3, txtField4, txtField5, txtField6;
        private Button btnAdd, btnUpdate, btnDelete;
        private Label statusLabel;

        // Declare Labels for input fields
        private Label lblUPC, lblAddress, lblLegalDesc, lblOwnerAddr, lblFullName, lblOwnerLoc;

        // SQL connection string
        private string connectionString = "Server=MAXIMUS\\DEV;Database=vll_lookupdb;User Id=tdsusr;Password=tdspwd;";
        private SqlConnection sqlConnection;

        public MainForm()
        {
            InitializeForm(); // Initialize form components
            TestSqlConnection(); // Test the SQL connection
        }

        // Method to test the SQL connection
        private void TestSqlConnection()
        {
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                MessageBox.Show("Database connection successful!");
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to the database: " + ex.Message);
            }
        }

        // Refactored InitializeForm method
        private void InitializeForm()
        {
            this.Text = "Village of Los Lunas Lookup Utility";
            this.Size = new System.Drawing.Size(800, 600);

            // Initialize ComboBox for table selection
            InitializeComboBox();

            // Initialize ListView for displaying records
            InitializeListView();

            // Initialize TextBoxes and Labels
            InitializeInputFields();
            InitializeButtons();
            InitializeStatusLabel();

            // Set default table selection
            tableComboBox.SelectedIndex = 0;
        }

        private void InitializeComboBox()
        {
            this.tableComboBox = new ComboBox
            {
                Location = new System.Drawing.Point(20, 20)
            };
            this.tableComboBox.Items.AddRange(new object[] { "vcodes", "vofficers", "vprops" });
            this.tableComboBox.SelectedIndexChanged += new EventHandler(TableComboBox_SelectedIndexChanged);
            this.Controls.Add(tableComboBox);
        }

        private void InitializeListView()
        {
            this.recordListView = new ListView
            {
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(750, 200),
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false
            };
            this.recordListView.DoubleClick += new EventHandler(RecordListView_DoubleClick);
            this.Controls.Add(recordListView);
        }

        private void InitializeButtons()
        {
            // Configure buttons for adding, updating, and deleting
            this.btnAdd = CreateButton("Add", new System.Drawing.Point(20, 420), AddButton_Click);
            this.btnUpdate = CreateButton("Update", new System.Drawing.Point(120, 420), UpdateButton_Click);
            this.btnDelete = CreateButton("Delete", new System.Drawing.Point(220, 420), DeleteButton_Click);

            this.Controls.AddRange(new Control[] { btnAdd, btnUpdate, btnDelete });
        }

        private Button CreateButton(string text, System.Drawing.Point location, EventHandler clickEvent)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = new System.Drawing.Size(80, 30)
            };

            // Subscribe to the Click event using +=
            button.Click += clickEvent;

            return button;
        }


        private void InitializeStatusLabel()
        {
            this.statusLabel = new Label
            {
                Location = new System.Drawing.Point(20, 470),
                Size = new System.Drawing.Size(750, 25)
            };
            this.Controls.Add(statusLabel);
        }


        private void InitializeInputFields()
        {
            // Initialize TextBoxes
            this.txtField1 = CreateTextBox(new System.Drawing.Point(120, 280));
            this.txtField2 = CreateTextBox(new System.Drawing.Point(120, 320));
            this.txtField3 = CreateTextBox(new System.Drawing.Point(120, 360));
            this.txtField4 = CreateTextBox(new System.Drawing.Point(460, 280));
            this.txtField5 = CreateTextBox(new System.Drawing.Point(460, 320));
            this.txtField6 = CreateTextBox(new System.Drawing.Point(460, 360));

            this.Controls.AddRange(new Control[] { txtField1, txtField2, txtField3, txtField4, txtField5, txtField6 });

            // Initialize Labels
            InitializeLabels();
        }

        private TextBox CreateTextBox(System.Drawing.Point location)
        {
            return new TextBox
            {
                Location = location,
                Size = new System.Drawing.Size(200, 25)
            };
        }

        private void InitializeLabels()
        {
            this.lblUPC = CreateLabel("UPC No | Officer:", new System.Drawing.Point(20, 280));
            this.lblAddress = CreateLabel("Violation | Address | Phone No:", new System.Drawing.Point(20, 320));
            this.lblLegalDesc = CreateLabel("Legal Description | Email:", new System.Drawing.Point(20, 360));
            this.lblOwnerAddr = CreateLabel("Owner Address | AD Account:", new System.Drawing.Point(360, 280));
            this.lblFullName = CreateLabel("Full Name | Title:", new System.Drawing.Point(360, 320));
            this.lblOwnerLoc = CreateLabel("Owner Location:", new System.Drawing.Point(360, 360));

            this.Controls.AddRange(new Control[] { lblUPC, lblAddress, lblLegalDesc, lblOwnerAddr, lblFullName, lblOwnerLoc });
        }

        private Label CreateLabel(string text, System.Drawing.Point location)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Size = new System.Drawing.Size(100, 25)
            };
        }

        private void ClearInputFields()
        {
            // Clears all text fields by iterating through an array of TextBox controls
            foreach (var textBox in new TextBox[] { txtField1, txtField2, txtField3, txtField4, txtField5, txtField6 })
            {
                textBox.Text = "";
            }
        }


        // Set Label properties for positioning and layout
        private void SetLabelProperties()
        {
            this.lblUPC.Text = "UPC No | Officer:";
            this.lblUPC.Location = new System.Drawing.Point(20, 280);
            this.lblUPC.Size = new System.Drawing.Size(100, 25);

            this.lblAddress.Text = "Violation | Address | Phone No:";
            this.lblAddress.Location = new System.Drawing.Point(20, 320);
            this.lblAddress.Size = new System.Drawing.Size(100, 25);

            this.lblLegalDesc.Text = "Legal Description | Email:";
            this.lblLegalDesc.Location = new System.Drawing.Point(20, 360);
            this.lblLegalDesc.Size = new System.Drawing.Size(100, 25);

            this.lblOwnerAddr.Text = "Owner Address | AD Account:";
            this.lblOwnerAddr.Location = new System.Drawing.Point(360, 280);
            this.lblOwnerAddr.Size = new System.Drawing.Size(100, 25);

            this.lblFullName.Text = "Full Name | Title:";
            this.lblFullName.Location = new System.Drawing.Point(360, 320);
            this.lblFullName.Size = new System.Drawing.Size(100, 25);

            this.lblOwnerLoc.Text = "Owner Location:";
            this.lblOwnerLoc.Location = new System.Drawing.Point(360, 360);
            this.lblOwnerLoc.Size = new System.Drawing.Size(100, 25);
        }

        // Event: ComboBox table selection change
        private void TableComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = this.tableComboBox.SelectedItem.ToString();
            UpdateListViewForSelectedTable(selectedTable);
            UpdateInputLabels(selectedTable);
        }

        // Helper method to update input labels based on the selected table
        private void UpdateInputLabels(string tableName)
        {
            switch (tableName)
            {
                case "vcodes":
                    lblUPC.Text = "Municipal Code:";
                    lblAddress.Text = "Violation Type:";
                    lblLegalDesc.Text = "Description:";
                    lblOwnerAddr.Visible = false; // Hide unused labels for this table
                    lblFullName.Visible = false;
                    lblOwnerLoc.Visible = false;
                    txtField4.Visible = false;
                    txtField5.Visible = false;
                    txtField6.Visible = false;
                    break;
                case "vofficers":
                    lblUPC.Text = "Officer:";
                    lblAddress.Text = "Phone No:";
                    lblLegalDesc.Text = "Email:";
                    lblOwnerAddr.Text = "AD Account:";
                    lblFullName.Text = "Title:";
                    lblOwnerLoc.Visible = false; // Hide unused label for this table
                    txtField6.Visible = false;
                    lblOwnerAddr.Visible = true;
                    lblFullName.Visible = true;
                    txtField4.Visible = true;
                    txtField5.Visible = true;
                    break;
                case "vprops":
                    lblUPC.Text = "UPC No:";
                    lblAddress.Text = "Address:";
                    lblLegalDesc.Text = "Legal Description:";
                    lblOwnerAddr.Text = "Owner Address:";
                    lblFullName.Text = "Full Name:";
                    lblOwnerLoc.Text = "Owner Location:";
                    lblOwnerAddr.Visible = true;
                    lblFullName.Visible = true;
                    lblOwnerLoc.Visible = true;
                    txtField4.Visible = true;
                    txtField5.Visible = true;
                    txtField6.Visible = true;
                    break;
            }
        }

        // Update the ListView columns dynamically based on table selection
        private void UpdateListViewForSelectedTable(string tableName)
        {
            // Clear the current ListView
            recordListView.Items.Clear();
            recordListView.Columns.Clear();

            // Dynamically update ListView columns based on the selected table
            switch (tableName)
            {
                case "vcodes":
                    recordListView.Columns.Add("Municipal Code", 100);
                    recordListView.Columns.Add("Violation Type", 100);
                    recordListView.Columns.Add("Description", 150);
                    break;
                case "vofficers":
                    recordListView.Columns.Add("Officer", 100);
                    recordListView.Columns.Add("Phone No", 100);
                    recordListView.Columns.Add("Email", 150);
                    recordListView.Columns.Add("AD Account", 100);
                    recordListView.Columns.Add("Title", 100);
                    break;
                case "vprops":
                    recordListView.Columns.Add("UPC No", 100);
                    recordListView.Columns.Add("Address", 150);
                    recordListView.Columns.Add("Legal Description", 150);
                    recordListView.Columns.Add("Owner Address", 150);
                    recordListView.Columns.Add("Full Name", 100);
                    recordListView.Columns.Add("Owner Location", 100);
                    break;
            }

            // Load data from the database into the ListView
            LoadData();
        }

        // Load data from the database into the ListView
        private void LoadData()
        {
            try
            {
                // Clear input fields to ensure no old data remains
                ClearInputFields();

                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    string selectedTable = this.tableComboBox.SelectedItem.ToString();
                    string query = $"SELECT * FROM {selectedTable}";

                    using (SqlCommand command = new SqlCommand(query, sqlConnection))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        recordListView.Items.Clear();

                        // Load data from the database into the ListView
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader[0].ToString()); // First column
                            for (int i = 1; i < reader.FieldCount; i++)
                            {
                                item.SubItems.Add(reader[i].ToString());
                            }
                            recordListView.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }



        private void RecordListView_DoubleClick(object sender, EventArgs e)
        {
            if (this.recordListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = this.recordListView.SelectedItems[0];
                txtField1.Text = selectedItem.SubItems[0].Text;
                txtField2.Text = selectedItem.SubItems[1].Text;
                txtField3.Text = selectedItem.SubItems[2].Text;

                // Only set fields if they exist to prevent crashes
                if (selectedItem.SubItems.Count > 3)
                {
                    txtField4.Text = selectedItem.SubItems[3].Text;
                }
                if (selectedItem.SubItems.Count > 4)
                {
                    txtField5.Text = selectedItem.SubItems[4].Text;
                }
                if (selectedItem.SubItems.Count > 5)
                {
                    txtField6.Text = selectedItem.SubItems[5].Text;
                }
            }
        }

        // Event: Add button clicked - Insert new record into the currently selected table
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtField1.Text))
            {
                MessageBox.Show("Please enter a value for the required fields.");
                return;
            }

            string selectedTable = tableComboBox.SelectedItem.ToString();
            string insertQuery = "";
            string checkDuplicateQuery = "";
            string keyField = "";
            int rowSizeLimit = 0;
            int rowSize = txtField1.Text.Length + txtField2.Text.Length + txtField3.Text.Length;

            // Phone number validation regex
            string phoneNumberPattern = @"^\(\d{3}\) \d{3}-\d{4}$";

            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Build the check for duplicate key query and set size limits based on the selected table
                    switch (selectedTable)
                    {
                        case "vcodes":
                            keyField = "upc";
                            rowSizeLimit = 1430;
                            checkDuplicateQuery = "SELECT COUNT(*) FROM vcodes WHERE upc = @upc";
                            insertQuery = "INSERT INTO vcodes (upc, citation, cdesc) VALUES (@upc, @citation, @cdesc)";
                            break;

                        case "vofficers":
                            keyField = "officer";
                            rowSizeLimit = 295;

                            // Validate phone number format for vofficers
                            if (!System.Text.RegularExpressions.Regex.IsMatch(txtField2.Text, phoneNumberPattern))
                            {
                                MessageBox.Show("Invalid phone number format. Please use the format (xxx) xxx-xxxx.");
                                return;
                            }

                            checkDuplicateQuery = "SELECT COUNT(*) FROM vofficers WHERE officer = @officer";
                            insertQuery = "INSERT INTO vofficers (officer, ophone, oemail, adusrname, otitle) VALUES (@officer, @ophone, @oemail, @adusrname, @otitle)";
                            rowSize += txtField4.Text.Length + txtField5.Text.Length;
                            break;

                        case "vprops":
                            keyField = "upc";
                            rowSizeLimit = 1350;
                            checkDuplicateQuery = "SELECT COUNT(*) FROM vprops WHERE upc = @upc";
                            insertQuery = "INSERT INTO vprops (upc, addr, legaldesc, owneraddr, fullname, owrloc) VALUES (@upc, @addr, @legaldesc, @owneraddr, @fullname, @owrloc)";
                            rowSize += txtField4.Text.Length + txtField5.Text.Length + txtField6.Text.Length;
                            break;

                        default:
                            MessageBox.Show("Unknown table selected.");
                            return;
                    }

                    // Check for duplicates
                    SqlCommand checkDuplicateCommand = new SqlCommand(checkDuplicateQuery, sqlConnection);
                    checkDuplicateCommand.Parameters.AddWithValue($"@{keyField}", txtField1.Text);
                    int recordCount = (int)checkDuplicateCommand.ExecuteScalar();

                    if (recordCount > 0)
                    {
                        MessageBox.Show($"Error: A record with the same {keyField} already exists. Please enter a unique value.");
                        return;
                    }

                    // Check row size limit
                    if (rowSize > rowSizeLimit)
                    {
                        MessageBox.Show($"Error: The total row size exceeds the {rowSizeLimit}-byte limit for {selectedTable}. Please reduce the input size.");
                        return;
                    }

                    // Proceed with inserting the new record if no duplicates and row size is valid
                    SqlCommand command = new SqlCommand(insertQuery, sqlConnection);

                    // Add parameters based on the selected table
                    switch (selectedTable)
                    {
                        case "vcodes":
                            command.Parameters.AddWithValue("@upc", txtField1.Text);
                            command.Parameters.AddWithValue("@citation", txtField2.Text);
                            command.Parameters.AddWithValue("@cdesc", txtField3.Text);
                            break;

                        case "vofficers":
                            command.Parameters.AddWithValue("@officer", txtField1.Text);
                            command.Parameters.AddWithValue("@ophone", txtField2.Text);
                            command.Parameters.AddWithValue("@oemail", txtField3.Text);
                            command.Parameters.AddWithValue("@adusrname", txtField4.Text);
                            command.Parameters.AddWithValue("@otitle", txtField5.Text);
                            break;

                        case "vprops":
                            command.Parameters.AddWithValue("@upc", txtField1.Text);
                            command.Parameters.AddWithValue("@addr", txtField2.Text);
                            command.Parameters.AddWithValue("@legaldesc", txtField3.Text);
                            command.Parameters.AddWithValue("@owneraddr", txtField4.Text);
                            command.Parameters.AddWithValue("@fullname", txtField5.Text);
                            command.Parameters.AddWithValue("@owrloc", txtField6.Text);
                            break;
                    }

                    command.ExecuteNonQuery();
                    MessageBox.Show("Record added successfully!");
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding record: " + ex.Message);
            }
        }

        // Event: Update button clicked - Update the selected record
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (this.recordListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            string selectedTable = tableComboBox.SelectedItem.ToString();
            string updateQuery = "";
            ListViewItem selectedItem = this.recordListView.SelectedItems[0]; // Get the selected record
            int rowSize = 0;

            // Phone number validation regex for vofficers
            string phoneNumberPattern = @"^\(\d{3}\) \d{3}-\d{4}$";

            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Check if the user is attempting to change the key field (Municipal Code, Officer, UPC No)
                    switch (selectedTable)
                    {
                        case "vcodes":
                            if (txtField1.Text != selectedItem.SubItems[0].Text)  // Key field for vcodes is Municipal Code (UPC)
                            {
                                MessageBox.Show("Error: Municipal Code (UPC No) cannot be changed. Please create a new record if necessary.");
                                return;  // Stop the update process if the key field is being changed
                            }
                            rowSize = txtField1.Text.Length + txtField2.Text.Length + txtField3.Text.Length;
                            if (rowSize > 1430)
                            {
                                MessageBox.Show("Error: The total row size exceeds the 1430-byte limit for vcodes. Please reduce the input size.");
                                return; // Stop the update operation if the row size exceeds the limit
                            }
                            updateQuery = "UPDATE vcodes SET citation = @citation, cdesc = @cdesc WHERE upc = @upc";
                            break;

                        case "vofficers":
                            if (txtField1.Text != selectedItem.SubItems[0].Text)  // Key field for vofficers is Officer
                            {
                                MessageBox.Show("Error: Officer field cannot be changed. Please create a new record if necessary.");
                                return;  // Stop the update process if the key field is being changed
                            }

                            // Validate phone number format for vofficers
                            if (!System.Text.RegularExpressions.Regex.IsMatch(txtField2.Text, phoneNumberPattern))
                            {
                                MessageBox.Show("Invalid phone number format. Please use the format (xxx) xxx-xxxx.");
                                return;  // Stop if the phone number format is invalid
                            }

                            rowSize = txtField1.Text.Length + txtField2.Text.Length + txtField3.Text.Length + txtField4.Text.Length + txtField5.Text.Length;
                            if (rowSize > 295)
                            {
                                MessageBox.Show("Error: The total row size exceeds the 295-byte limit for vofficers. Please reduce the input size.");
                                return; // Stop the update operation if the row size exceeds the limit
                            }
                            updateQuery = "UPDATE vofficers SET ophone = @ophone, oemail = @oemail, adusrname = @adusrname, otitle = @otitle WHERE officer = @officer";
                            break;

                        case "vprops":
                            if (txtField1.Text != selectedItem.SubItems[0].Text)  // Key field for vprops is UPC
                            {
                                MessageBox.Show("Error: UPC No cannot be changed. Please create a new record if necessary.");
                                return;  // Stop the update process if the key field is being changed
                            }

                            rowSize = txtField1.Text.Length + txtField2.Text.Length + txtField3.Text.Length + txtField4.Text.Length + txtField5.Text.Length + txtField6.Text.Length;
                            if (rowSize > 1350)
                            {
                                MessageBox.Show("Error: The total row size exceeds the 1350-byte limit for vprops. Please reduce the input size.");
                                return; // Stop the update operation if the row size exceeds the limit
                            }
                            updateQuery = "UPDATE vprops SET addr = @addr, legaldesc = @legaldesc, owneraddr = @owneraddr, fullname = @fullname, owrloc = @owrloc WHERE upc = @upc";
                            break;

                        default:
                            MessageBox.Show("Unknown table selected.");
                            return;
                    }

                    SqlCommand command = new SqlCommand(updateQuery, sqlConnection);

                    // Add parameters based on the selected table
                    switch (selectedTable)
                    {
                        case "vcodes":
                            command.Parameters.AddWithValue("@upc", txtField1.Text);
                            command.Parameters.AddWithValue("@citation", txtField2.Text);
                            command.Parameters.AddWithValue("@cdesc", txtField3.Text);
                            break;

                        case "vofficers":
                            command.Parameters.AddWithValue("@officer", txtField1.Text);  // Officer
                            command.Parameters.AddWithValue("@ophone", txtField2.Text);   // Phone No
                            command.Parameters.AddWithValue("@oemail", txtField3.Text);   // Email
                            command.Parameters.AddWithValue("@adusrname", txtField4.Text); // AD Account
                            command.Parameters.AddWithValue("@otitle", txtField5.Text);    // Title
                            break;

                        case "vprops":
                            command.Parameters.AddWithValue("@upc", txtField1.Text);       // UPC No
                            command.Parameters.AddWithValue("@addr", txtField2.Text);      // Address
                            command.Parameters.AddWithValue("@legaldesc", txtField3.Text); // Legal Description
                            command.Parameters.AddWithValue("@owneraddr", txtField4.Text); // Owner Address
                            command.Parameters.AddWithValue("@fullname", txtField5.Text);  // Full Name
                            command.Parameters.AddWithValue("@owrloc", txtField6.Text);    // Owner Location
                            break;
                    }

                    command.ExecuteNonQuery();
                    MessageBox.Show("Record updated successfully!");
                    LoadData(); // Refresh ListView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating record: " + ex.Message);
            }
        }

        // Event: Delete button clicked - Delete the selected record
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (recordListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string selectedTable = tableComboBox.SelectedItem.ToString();
                string keyField = selectedTable == "vcodes" || selectedTable == "vprops" ? "upc" : "officer";
                string deleteQuery = $"DELETE FROM {selectedTable} WHERE {keyField} = @{keyField}";

                try
                {
                    using (sqlConnection = new SqlConnection(connectionString))
                    {
                        sqlConnection.Open();
                        SqlCommand command = new SqlCommand(deleteQuery, sqlConnection);
                        command.Parameters.AddWithValue($"@{keyField}", txtField1.Text);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Record deleted successfully!");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting record: " + ex.Message);
                }
            }
        }
    }
}