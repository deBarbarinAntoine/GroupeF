namespace AppMeteo
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.Windows.Forms.Label cityLabel;
        private System.Windows.Forms.Label temperatureLabel;
        private System.Windows.Forms.PictureBox weatherIcon;
        private System.Windows.Forms.TextBox cityTextBox;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Panel forecastPanel;

        private void InitializeComponent()
        {
            this.cityLabel = new System.Windows.Forms.Label();
            this.temperatureLabel = new System.Windows.Forms.Label();
            this.weatherIcon = new System.Windows.Forms.PictureBox();
            this.cityTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();

            // Détails de la fenêtre
            this.ClientSize = new System.Drawing.Size(420, 650);
            this.Text = "Application Météo";
            this.BackColor = System.Drawing.Color.FromArgb(10, 46, 54);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            // Ville
            this.cityLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.cityLabel.ForeColor = System.Drawing.Color.White;
            this.cityLabel.Location = new System.Drawing.Point(20, 20);
            this.cityLabel.Size = new System.Drawing.Size(360, 40);
            this.cityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Temperature
            this.temperatureLabel.Font = new System.Drawing.Font("Segoe UI", 48F);
            this.temperatureLabel.ForeColor = System.Drawing.Color.White;
            this.temperatureLabel.Location = new System.Drawing.Point(20, 70);
            this.temperatureLabel.Size = new System.Drawing.Size(360, 100);
            this.temperatureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Description
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.descriptionLabel.ForeColor = System.Drawing.Color.White;
            this.descriptionLabel.Location = new System.Drawing.Point(20, 180);
            this.descriptionLabel.Size = new System.Drawing.Size(360, 30);
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Icone de la météo
            this.weatherIcon.Location = new System.Drawing.Point(150, 220);
            this.weatherIcon.Size = new System.Drawing.Size(100, 100);
            this.weatherIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // Input de la ville
            this.cityTextBox.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.cityTextBox.Location = new System.Drawing.Point(20, 350);
            this.cityTextBox.Size = new System.Drawing.Size(260, 32);
            this.cityTextBox.PlaceholderText = "Entrez une ville";

            // Bouton rechercher
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.BackColor = System.Drawing.Color.White;
            this.searchButton.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.searchButton.FlatAppearance.BorderSize = 1;
            this.searchButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.searchButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.searchButton.Text = "Rechercher";
            this.searchButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.searchButton.Location = new System.Drawing.Point(290, 350);
            this.searchButton.Size = new System.Drawing.Size(110, 32);

            // Style supplémentaire des bords du bouton
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, 20, 20, 180, 90);
            path.AddArc(this.searchButton.Width - 20, 0, 20, 20, 270, 90);
            path.AddArc(this.searchButton.Width - 20, this.searchButton.Height - 20, 20, 20, 0, 90);
            path.AddArc(0, this.searchButton.Height - 20, 20, 20, 90, 90);
            path.CloseFigure();
            this.searchButton.Region = new System.Drawing.Region(path);

            // Hover du bouton
            this.searchButton.MouseEnter += (s, e) =>
            {
                this.searchButton.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            };
            this.searchButton.MouseLeave += (s, e) =>
            {
                this.searchButton.BackColor = System.Drawing.Color.White;
            };


            // Bouton rafraichir
            this.refreshButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshButton.BackColor = System.Drawing.Color.White;
            this.refreshButton.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.refreshButton.FlatAppearance.BorderSize = 1;
            this.refreshButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.refreshButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.refreshButton.Text = "Rafraîchir";
            this.refreshButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refreshButton.Location = new System.Drawing.Point(150, 400);
            this.refreshButton.Size = new System.Drawing.Size(100, 32);

            // Style supplémentaire des bords du bouton
            System.Drawing.Drawing2D.GraphicsPath refreshPath = new System.Drawing.Drawing2D.GraphicsPath();
            refreshPath.AddArc(0, 0, 20, 20, 180, 90);
            refreshPath.AddArc(this.refreshButton.Width - 20, 0, 20, 20, 270, 90);
            refreshPath.AddArc(this.refreshButton.Width - 20, this.refreshButton.Height - 20, 20, 20, 0, 90);
            refreshPath.AddArc(0, this.refreshButton.Height - 20, 20, 20, 90, 90);
            refreshPath.CloseFigure();
            this.refreshButton.Region = new System.Drawing.Region(refreshPath);

            // Hover du bouton
            this.refreshButton.MouseEnter += (s, e) =>
            {
                this.refreshButton.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            };
            this.refreshButton.MouseLeave += (s, e) =>
            {
                this.refreshButton.BackColor = System.Drawing.Color.White;
            };

           this.forecastPanel = new System.Windows.Forms.Panel();
            this.forecastPanel.Location = new System.Drawing.Point(20, 460);
            this.forecastPanel.Size = new System.Drawing.Size(380, 150);
            this.forecastPanel.BackColor = System.Drawing.Color.FromArgb(10, 46, 54);
            
            this.Controls.Add(this.cityLabel);
            this.Controls.Add(this.temperatureLabel);
            this.Controls.Add(this.weatherIcon);
            this.Controls.Add(this.cityTextBox);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.forecastPanel);
        }
    }
}
