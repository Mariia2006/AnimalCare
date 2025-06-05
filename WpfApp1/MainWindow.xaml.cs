using System.ComponentModel;
using System.Text.Json;
using System.Windows;
using AnimalCare.Core;

namespace AnimalCare
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            CareListBox.ItemsSource = viewModel.CareList;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Care someCare = new Care();
            var window = new CareEditor(someCare);
            if (window.ShowDialog() == true)
            {
                viewModel.AddCare(window.EditedCare);
                CareListBox.Items.Refresh();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (CareListBox.SelectedItem is Care selectedCare)
            {
                var clone = JsonSerializer.Deserialize<Care>(JsonSerializer.Serialize(selectedCare));
                var window = new CareEditor(clone);
                if (window.ShowDialog() == true)
                {
                    viewModel.EditCare(CareListBox.SelectedIndex, window.EditedCare);
                    CareListBox.Items.Refresh();
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            viewModel.SaveChanges();
        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            if (CareListBox.SelectedItem is Care selectedCare)
            {
                var reportWindow = new ReportWindow
                {
                    DataContext = new ReportViewModel(selectedCare)
                };
                reportWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть запис для перегляду звіту.");
            }
        }
    }
}