using System;
using System.Windows.Forms;
using Order_Managment_Menu_2;
using stcokManagement;
using modifyStockLevel;
using User_Feedback;
using Order_Creation;
using Dealer_Information;
using System.Diagnostics;


namespace goToPage
{

    public class GoToPageFunction
    {
        private Form currentForm;

        public GoToPageFunction(Form form)
        {
            currentForm = form;
        }

        public void GoToOrderMenu()
        {
            Order_Menu orderMenu = new Order_Menu();
            orderMenu.Show();
            currentForm.Hide();
        }
        public void GoToStockMenu()
        {
            Stock_Menu stockMenu = new Stock_Menu();
            stockMenu.Show();
            currentForm.Hide();
        }
        public void GoToStockModify()
        {
            modifyStock modify = new modifyStock();
            modify.Show();
            currentForm.Hide();
        }
        public void GoToUserFeedback()
        {
            Feedback_Menu modify = new Feedback_Menu();
            modify.Show();
            currentForm.Hide();
        }
        public void GoToReport()
        {
            Report_Menu report_menu = new Report_Menu();
            report_menu.Show();
            currentForm.Hide();
        }
        public void Logout()
        {
            Login login = new Login();
            login.Show();
            currentForm.Close();
        }

        public void GoToCreateSellOrder()
        {
            OrderCreation a = new OrderCreation();
            a.Show();
            currentForm.Hide(); 
        }

        public void GoToDealerInfo()
        {
            Dealer_Info Dealer_Info = new Dealer_Info();
            Dealer_Info.Show();
            currentForm.Hide();
        }
        public void OpenCmd()
        {
            Process.Start("cmd.exe", "/c python C:\\Users\\a3791\\Desktop\\rp.py");
        }

    }
}