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

namespace Minesweeper.Pages
{
    /// <summary>
    /// Interaction logic for LossPage.xaml
    /// </summary>
    public partial class LossPage : Page
    {
        public LossPage()
        {
            InitializeComponent();
        }

        private void PlayAgainButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new GamePage());
        }
    }
}
