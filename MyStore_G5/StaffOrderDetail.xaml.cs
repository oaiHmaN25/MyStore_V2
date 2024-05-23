using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace MyStore_G5
{
    /// <summary>
    /// Interaction logic for StaffOrderDetail.xaml
    /// </summary>
    public partial class StaffOrderDetail : Window
    {
        private readonly int orderId;

        public StaffOrderDetail(int orderId)
        {
            InitializeComponent();
            this.orderId = orderId;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            // Truy xuất dữ liệu order details từ cơ sở dữ liệu
            List<OrderDetail> orderDetails = RetrieveOrderDetailsFromDatabase(orderId);

            // Gán danh sách order details cho ItemsSource của ListView
            listView.ItemsSource = orderDetails;

            // Đăng ký sự kiện Loaded của listView
            listView.Loaded += ListView_Loaded;
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            // Sau khi listView đã hoàn thành việc tạo và hiển thị mục OrderDetail, tính toán tổng giá trị
            UpdateTotalPrice();
        }
        private List<OrderDetail> RetrieveOrderDetailsFromDatabase(int orderId)
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            using (var context = new MyStoreContext())
            {
                var orderDetailsFromDb = context.OrderDetails
                                                  .Include(od => od.Product)
                                                  .Where(od => od.OrderId == orderId)
                                                  .ToList();

                foreach (var orderDetail in orderDetailsFromDb)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        OrderDetailId = orderDetail.OrderDetailId,
                        OrderId = orderDetail.OrderId,
                        ProductId = orderDetail.ProductId,
                        ProductName = orderDetail.Product != null ? orderDetail.Product.ProductName : "Unknown",
                        Quantity = orderDetail.Quantity,
                        UnitPrice = orderDetail.UnitPrice
                    });
                }
            }
            return orderDetails;
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            OrderDetail selectedOrderDetail = (sender as FrameworkElement)?.DataContext as OrderDetail;

            if (selectedOrderDetail != null)
            {
                // Pass the selected OrderDetail to the EditOrderDetail window
                EditOrderDetail editOrder = new EditOrderDetail(selectedOrderDetail);
                editOrder.ShowDialog();
            }

        }

        private void AddOrderDetailButton_Click(object sender, RoutedEventArgs e)
        {
            var addOrderDetailWindow = new StaffAddOrderDetail(orderId);
            if (addOrderDetailWindow.ShowDialog() == true)
            {
                // Reload the order details after adding a new one
                LoadOrderDetails();
            }
        }
        private void UpdateTotalPrice()
        {
            int totalPrice = 0;

            // Lặp qua các mục trong ListView và tính tổng giá trị
            foreach (OrderDetail orderDetail in listView.Items)
            {
                totalPrice += orderDetail.TotalPrice;
            }

            // Cập nhật TextBlock với tổng giá trị
            totalPriceTextBlock.Text = "Total Price: " + totalPrice.ToString();
        }
    }


    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice => Quantity * UnitPrice;
    }
}
