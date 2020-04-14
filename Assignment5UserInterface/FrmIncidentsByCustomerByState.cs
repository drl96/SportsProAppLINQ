using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment5UserInterface
{
    public partial class FrmIncidentsByCustomerByState : Form
    {
        public FrmIncidentsByCustomerByState()
        {
            InitializeComponent();
        }
        Assignment5LINQtoSQLDataContext myContext = new Assignment5LINQtoSQLDataContext();


        private void FrmIncidentsByCustomerByState_Load(object sender, EventArgs e)
        {
            //var statesQry1 = from state in myContext.States
            //                 select new { state.StateCode, state.StateName };

            //stateNameComboBox.DataSource = statesQry1;
            //stateNameComboBox.SelectedIndex = -1;  

            //-----------------------------------------------------------------------------------------------------

            var statesQry1 = from state in myContext.States
                             orderby state.StateName
                             select state;
            stateBindingSource.DataSource = statesQry1;
            stateNameComboBox.SelectedIndex = -1;
        }



        //populate dgv with list of customers that reside in the state selected in the ComboBox "statecode"
        //"linq query across relationships section???"
        private void stateNameComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //customerBindingSource.Clear();
            //string stateCode = (string)stateNameComboBox.SelectedValue;


            //var customersQry1 =
            //    from customer in myContext.Customers
            //    where customer.State == stateCode
            //    let Phone = customer.Phone ?? "null"
            //    let Email = customer.Email ?? "null"              
            //    select new { customer.CustomerID, customer.Name, customer.Address, customer.City, customer.State, customer.ZipCode, Phone, Email };

            //customerBindingSource.DataSource = customersQry1;

            //-----------------------------------------------------------------------------------------------------

            //customerBindingSource.Clear();
            //incidentBindingSource.Clear();
            customerDataGridView.Rows.Clear();
            incidentDataGridView.Rows.Clear();

            string stateCode = (string)stateNameComboBox.SelectedValue;

            //the "selectState" query finds the full state entity for the selected state name valuemember from the combobox
            var selectState =
                (from state in myContext.States
                 where state.StateCode == stateCode
                 select state).Single();
            //.single is used because we know the query is returning a single value (similar to execute scalar??)

            customerBindingSource.DataSource = selectState.Customers;
            //because linq knows the relationship of states to customers, 
            //it can use the "selectState.Customers" statement to find the customers associated with the full state
            //entity that was selected in the selectState query
            //I believe this is the "linq query across relationships"
        }



        //populate the second dgv with all incidents associated with customer represented by the dgv selection
        private void customerDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            //incidentBindingSource.Clear();

            //if (customerDataGridView.Rows.Count > 0)
            //{
            //    int rowIndex = customerDataGridView.CurrentCell.RowIndex;

            //    int custID = (int)customerDataGridView.Rows[rowIndex].Cells[0].Value;

            //    var incidentsQry1 = from incident in myContext.Incidents
            //                        where incident.CustomerID == custID
            //                        select new { incident.IncidentID, incident.CustomerID, incident.ProductCode, incident.Title, incident.Description, incident.DateOpened, incident.DateClosed, incident.TechID };

            //    incidentBindingSource.DataSource = incidentsQry1;
            //}

            //-----------------------------------------------------------------------------------------------------

            if (customerDataGridView.Rows.Count > 0)
            {
                int rowIndex = customerDataGridView.CurrentCell.RowIndex;
                int custID = (int)customerDataGridView.Rows[rowIndex].Cells[0].Value;

                //the selectCustomer query the full customer entity for CustomerID in the selected row of the customers dgv
                var selectCustomer = (from customer in myContext.Customers
                                      where customer.CustomerID == custID
                                      select customer).Single();
                //.single is used because we know the query is returning a single value (similar to execute scalar??)

                incidentBindingSource.DataSource = selectCustomer.Incidents;
                //because linq knows the relationship of customers to incidents, 
                //it can use the "selectCustomer.Incidents" statement to find the incidents associated with the full customer
                //entity that was selected in the selectCustomer query
                //I believe this is the "linq query across relationships"

            }

        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
