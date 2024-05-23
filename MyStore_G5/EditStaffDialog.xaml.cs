using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for EditStaffDialog.xaml
    /// </summary>
    public partial class EditStaffDialog : Window
    {
        private MyStore_G5.Models.Staff staffToUpdate;

        public EditStaffDialog(MyStore_G5.Models.Staff staff)
        {
            InitializeComponent();

            // Initialize the staffToUpdate field
            staffToUpdate = staff;

            // Set the initial values in the text boxes
            nameTextBox.Text = staffToUpdate.Name;
            passwordTextBox.Text = staffToUpdate.Password;

            // Find the ComboBoxItem corresponding to the staff's role
            ComboBoxItem selectedItem = cmbRole.Items.OfType<ComboBoxItem>()
                                                     .FirstOrDefault(item => item.Tag.ToString() == staffToUpdate.Role.ToString());

            // Select the ComboBoxItem
            if (selectedItem != null)
            {
                selectedItem.IsSelected = true;
            }
        }

        // Method to get the updated staff details
        public MyStore_G5.Models.Staff GetUpdatedStaffDetails()
        {
            // Update the staff details from the text boxes and combo box
            staffToUpdate.Name = nameTextBox.Text;
            staffToUpdate.Password = passwordTextBox.Text;

            // Retrieve the role from the selected ComboBoxItem's Tag property and parse it to an integer
            ComboBoxItem selectedItem = (ComboBoxItem)cmbRole.SelectedItem;
            staffToUpdate.Role = int.Parse(selectedItem.Tag.ToString());

            return staffToUpdate;
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {
                // Close the dialog with DialogResult set to true to indicate success
                DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
                // Close the dialog with DialogResult set to false to indicate cancellation
                DialogResult = false;
        }
    }
    
}
