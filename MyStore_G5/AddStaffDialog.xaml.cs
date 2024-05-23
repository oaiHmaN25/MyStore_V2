using System;
using MyStore_G5.Models;
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
using System.Windows.Shapes;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for AddStaffDialog.xaml
    /// </summary>
    public partial class AddStaffDialog : Window
    {
        public AddStaffDialog()
        {
            InitializeComponent();
        }
        // Phương thức này trả về thông tin của nhân viên từ các điều khiển nhập liệu
        public MyStore_G5.Models.Staff GetStaffDetails()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cmbRole.SelectedItem;
            string role = selectedItem.Tag.ToString();
            // Tạo một đối tượng Staff mới từ thông tin được nhập vào các điều khiển
            MyStore_G5.Models.Staff newStaff = new MyStore_G5.Models.Staff
            {
                Name = txtStaffName.Text,
                Password = txtPassword.Text,
                
                Role = int.Parse(role)
            };

            return newStaff;
        }

        // Xử lý sự kiện khi người dùng nhấp vào nút "Thêm"
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra tính hợp lệ của thông tin nhân viên trước khi đóng cửa sổ
            if (IsValidStaff())
            {
                // Đóng cửa sổ hộp thoại
                this.DialogResult = true;
                this.Close();
            }
        }

        // Xử lý sự kiện khi người dùng nhấp vào nút "Hủy"
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Đóng cửa sổ hộp thoại mà không thêm nhân viên
            this.DialogResult = false;
            this.Close();
        }

        // Phương thức này kiểm tra tính hợp lệ của thông tin nhân viên
        private bool IsValidStaff()
        {
            // Kiểm tra xem các trường bắt buộc đã được nhập hay chưa
            if (string.IsNullOrWhiteSpace(txtStaffName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter complete information!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void txtStaffID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtStaffName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
