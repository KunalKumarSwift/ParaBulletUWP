using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ParaBullet
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void CreateBulletPoints_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve selected bullet type, extra space preference, and input text
            string selectedBullet = (bulletComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            bool addExtraSpace = extraSpaceCheckBox.IsChecked ?? false;
            string inputText = inputTextBox.Text;

            // Create bullet points based on the selected bullet type
            List<string> bulletPoints = CreateBulletPoints(selectedBullet, addExtraSpace, inputText);

            // Populate the outputTextBox with the generated list
            outputTextBox.Text = string.Join(Environment.NewLine, bulletPoints);
        }

        private List<string> CreateBulletPoints(string selectedBullet, bool addExtraSpace, string inputText)
        {
            List<string> bulletPoints = new List<string>();

            // Split the input text into paragraphs
            string[] paragraphs = inputText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            int listItemNumber = 1; // Initialize the list item number

            foreach (string paragraph in paragraphs)
            {
                StringBuilder bulletPoint = new StringBuilder();

                // Check if the selected bullet type is "Numbered List"
                if (selectedBullet == "Numbered List")
                {
                    // Add numbered list item (e.g., "1.")
                    bulletPoint.Append(listItemNumber + ". ");
                    listItemNumber++; // Increment the list item number for the next paragraph
                }
                else
                {
                    // Add the selected bullet type (e.g., "•")
                    bulletPoint.Append(selectedBullet + " ");
                }

                // Add paragraph text
                bulletPoint.Append(paragraph);

                // Add extra space if requested
                if (addExtraSpace)
                {
                    bulletPoint.Append("\n\n");
                }

                // Add the bullet point to the list
                bulletPoints.Add(bulletPoint.ToString());
            }

            return bulletPoints;
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            // Copy the bullet points to the clipboard
            var dataPackage = new DataPackage();
            dataPackage.SetText(outputTextBox.Text);

            Clipboard.SetContent(dataPackage);
        }

        private void ClearText_Click(object sender, RoutedEventArgs e)
        {
            // Clear input text and generated list
            inputTextBox.Text = string.Empty;
            outputTextBox.Text = string.Empty;
        }

        private void ShareList_Click(object sender, RoutedEventArgs e)
        {
            // Create a DataTransferManager instance
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

            // Register a DataRequested event handler to handle sharing requests
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;

            // Start the sharing process
            DataTransferManager.ShowShareUI();
        }

        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            // Set the data for sharing
            DataPackage requestData = e.Request.Data;
            requestData.SetText(outputTextBox.Text);

            // Provide a title for the shared content (optional)
            requestData.Properties.Title = "Shared Bullet List";

            // Unregister the DataRequested event handler
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested -= DataTransferManager_DataRequested;
        }

    }
}
