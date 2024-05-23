using MyStore_G5.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyStore_G5
{
    public partial class StaffManagement : Page
    {
        private MyStoreContext con = new MyStoreContext();
        public StaffManagement()
        {
            InitializeComponent();
            LoadStaff();
        }

        private void LoadStaff()
        {
            try
            {
                if (con == null || con.Staffs == null)
                {
                    MessageBox.Show("Error: Database connection not properly configured.");
                    return;
                }

                // Xóa danh sách nhân viên hiện có
                staffListView.Items.Clear();

                // Truy vấn danh sách nhân viên từ cơ sở dữ liệu
                var staffList = con.Staffs.ToList();

                // Hiển thị danh sách nhân viên trong ListView
                foreach (var staff in staffList)
                {
                    staffListView.Items.Add(staff);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading staff: {ex.Message}");
            }
        }


        private void staffListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Mở một cửa sổ hộp thoại để thêm một nhân viên mới
                AddStaffDialog addStaffDialog = new AddStaffDialog();

                // Hiển thị cửa sổ hộp thoại dưới dạng cửa sổ modal
                bool? result = addStaffDialog.ShowDialog();

                // Nếu người dùng nhấp vào nút "Thêm" trong hộp thoại và kết quả của hộp thoại là true
                if (result == true)
                {
                    // Lấy chi tiết của nhân viên mới từ hộp thoại
                    MyStore_G5.Models.Staff newStaff = addStaffDialog.GetStaffDetails(); // Giả sử có một phương thức trong hộp thoại để lấy chi tiết nhân viên
                    if (newStaff != null)
                    {
                        con.Staffs.Add(newStaff);
                        con.SaveChanges();
                    }
                    // Tải lại danh sách nhân viên sau khi thêm nhân viên mới
                    LoadStaff();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a new staff: {ex.Message}");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected staff from the ListView
                MyStore_G5.Models.Staff selectedStaff = staffListView.SelectedItem as MyStore_G5.Models.Staff;

                // Check if a staff is selected
                if (selectedStaff == null)
                {
                    MessageBox.Show("Please select a staff to update.");
                    return;
                }

                // Open a dialog to edit the staff details
                EditStaffDialog editStaffDialog = new EditStaffDialog(selectedStaff);

                // Display the dialog as a modal window
                bool? result = editStaffDialog.ShowDialog();

                // If the user clicks "Update" in the dialog and the result is true
                if (result == true)
                {
                    // Get the updated staff details from the dialog
                    MyStore_G5.Models.Staff updatedStaff = editStaffDialog.GetUpdatedStaffDetails();

                    // Update the staff in the database
                    selectedStaff.Name = updatedStaff.Name;
                    selectedStaff.Password = updatedStaff.Password;
                    selectedStaff.Role = updatedStaff.Role;

                    con.SaveChanges();

                    // Reload the staff list after updating
                    LoadStaff();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the staff: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the selected staff from the ListView
                MyStore_G5.Models.Staff selectedStaff = staffListView.SelectedItem as MyStore_G5.Models.Staff;

                // Check if a staff is selected
                if (selectedStaff == null)
                {
                    MessageBox.Show("Please select a staff to delete.");
                    return;
                }

                // Confirm deletion with the user
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this staff?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // If user confirms deletion
                if (result == MessageBoxResult.Yes)
                {
                    // Remove the staff from the database
                    con.Staffs.Remove(selectedStaff);
                    con.SaveChanges();

                    // Reload the staff list after deletion
                    LoadStaff();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the staff: {ex.Message}");
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchText = searchTextBox.Text.Trim();

                // Perform the search based on the entered text
                var searchResults = con.Staffs.Where(staff => staff.Name.Contains(searchText)).ToList();

                // Clear the existing items in the ListView
                staffListView.Items.Clear();

                // Add the search results to the ListView
                foreach (var result in searchResults)
                {
                    staffListView.Items.Add(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching: {ex.Message}");
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
