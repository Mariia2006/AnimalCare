using AnimalCare.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

namespace AnimalCare
{
    public partial class CareEditor : Window
    {
        private CareEditorViewModel vm;

        public CareEditor(Care existingCare)
        {
            InitializeComponent();
            vm = new CareEditorViewModel(existingCare);
            DataContext = vm;
        }

        public Care EditedCare => vm.EditedCare;

        private void AddJobButton_Click(object sender, RoutedEventArgs e)
        {
            if (!vm.AddOrUpdateJob(out string error))
            {
                MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                JobsListBox.Items.Refresh();
            }
        }

        private void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(vm.OwnerSurname))
            {
                MessageBox.Show("Please enter the owner's last name.", "Validation error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            vm.EditedCare.OwnerSurname = vm.OwnerSurname;
            DialogResult = true;
            Close();
        }

        private void CancelAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void JobsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (JobsListBox.SelectedIndex >= 0)
            {
                var vm = (CareEditorViewModel)DataContext;
                vm.StartEditJob(JobsListBox.SelectedIndex);
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (vm.IsUnsavedJobEntryPresent())
            {
                var result = MessageBox.Show(
                    "You have entered job data but have not added it. Close window without saving?",
                    "Unsaved job data",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!vm.IsSaved)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes?",
                    "Confirm save",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    vm.SaveChanges();
                    DialogResult = true;
                }
                else
                {
                    DialogResult = false;
                }
            }
        }
    }
}