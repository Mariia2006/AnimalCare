using AnimalCare.Core;
using ClosedXML.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimalCare
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ReportViewModel viewModel || viewModel.CareJobs == null || !viewModel.CareJobs.Any())
            {
                MessageBox.Show("There is no data to export.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = $"CareReport_{viewModel.OwnerSurname}.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using var workbook = new XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Care Jobs");

                    worksheet.Cell(1, 1).Value = viewModel.OwnerSurname;
                    worksheet.Cell(2, 1).Value = "Species";
                    worksheet.Cell(2, 2).Value = "Name";
                    worksheet.Cell(2, 3).Value = "Birth Year";
                    worksheet.Cell(2, 4).Value = "Gender";
                    worksheet.Cell(2, 5).Value = "Job Type";
                    worksheet.Cell(2, 6).Value = "Cost (UAH)";
                    worksheet.Cell(2, 7).Value = "Date";

                    int row = 3;
                    foreach (var job in viewModel.CareJobs)
                    {
                        worksheet.Cell(row, 1).Value = job.Animal.Species;
                        worksheet.Cell(row, 2).Value = job.Animal.Name;
                        worksheet.Cell(row, 3).Value = job.Animal.BirthYear;
                        worksheet.Cell(row, 4).Value = job.Animal.IsMale ? "M" : "F";
                        worksheet.Cell(row, 5).Value = job.JobType.ToString();
                        worksheet.Cell(row, 6).Value = job.Cost;
                        worksheet.Cell(row, 7).Value = job.JobDate.ToString("dd.MM.yyyy");
                        row++;
                    }

                    worksheet.Cell(row, 6).Value = viewModel.TotalCost;
                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);

                    MessageBox.Show("Excel file saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Browser_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not ReportViewModel viewModel || viewModel.CareJobs == null || !viewModel.CareJobs.Any())
            {
                MessageBox.Show("There is no data to export.");
                return;
            }

            var htmlBuilder = new StringBuilder();
            htmlBuilder.AppendLine("<html><head><meta charset='UTF-8'><title>Care Report</title></head><body>");
            htmlBuilder.AppendLine($"<h2>User report: {viewModel.OwnerSurname}</h2>");
            htmlBuilder.AppendLine("<table border='1' cellspacing='0' cellpadding='5'>");
            htmlBuilder.AppendLine("<tr><th>Species</th><th>Name</th><th>Birth Year</th><th>Gender</th><th>Job Type</th><th>Cost (UAH)</th><th>Date</th></tr>");

            foreach (var job in viewModel.CareJobs)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{job.Animal.Species}</td>");
                htmlBuilder.AppendLine($"<td>{job.Animal.Name}</td>");
                htmlBuilder.AppendLine($"<td>{job.Animal.BirthYear}</td>");
                htmlBuilder.AppendLine($"<td>{(job.Animal.IsMale ? "M" : "F")}</td>");
                htmlBuilder.AppendLine($"<td>{job.JobType}</td>");
                htmlBuilder.AppendLine($"<td>{job.Cost} ₴</td>");
                htmlBuilder.AppendLine($"<td>{job.JobDate:dd.MM.yyyy}</td>");
                htmlBuilder.AppendLine("</tr>");
            }

            htmlBuilder.AppendLine("</table>");
            htmlBuilder.AppendLine($"<p><strong>Total cost: {viewModel.TotalCost} ₴</strong></p>");
            htmlBuilder.AppendLine("</body></html>");

            string filePath = Path.Combine(Path.GetTempPath(), "CareReport.html");
            File.WriteAllText(filePath, htmlBuilder.ToString(), Encoding.UTF8);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }
    }
}
