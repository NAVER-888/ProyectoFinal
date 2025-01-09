namespace ProyectoFinal
{
    partial class FrmReportVentas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.ventaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.bDProyectoFinalVentas = new ProyectoFinal.BDProyectoFinalVentas();
            this.detalleVentasBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.detalleVentasTableAdapter = new ProyectoFinal.BDProyectoFinalVentasTableAdapters.DetalleVentasTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ventaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bDProyectoFinalVentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detalleVentasBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ventaBindingSource
            // 
            this.ventaBindingSource.DataSource = typeof(CapaDatos.Venta);
            this.ventaBindingSource.CurrentChanged += new System.EventHandler(this.ventaBindingSource_CurrentChanged);
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "VentasDataSet";
            reportDataSource1.Value = this.detalleVentasBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "ProyectoFinal.Ventas.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(1182, 578);
            this.reportViewer1.TabIndex = 0;
            // 
            // bDProyectoFinalVentas
            // 
            this.bDProyectoFinalVentas.DataSetName = "BDProyectoFinalVentas";
            this.bDProyectoFinalVentas.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // detalleVentasBindingSource
            // 
            this.detalleVentasBindingSource.DataMember = "DetalleVentas";
            this.detalleVentasBindingSource.DataSource = this.bDProyectoFinalVentas;
            // 
            // detalleVentasTableAdapter
            // 
            this.detalleVentasTableAdapter.ClearBeforeFill = true;
            // 
            // FrmReportVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 578);
            this.Controls.Add(this.reportViewer1);
            this.Name = "FrmReportVentas";
            this.Text = "FrmReportVentas";
            this.Load += new System.EventHandler(this.FrmReportVentas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ventaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bDProyectoFinalVentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detalleVentasBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ventaBindingSource;
        private BDProyectoFinalVentas bDProyectoFinalVentas;
        private System.Windows.Forms.BindingSource detalleVentasBindingSource;
        private BDProyectoFinalVentasTableAdapters.DetalleVentasTableAdapter detalleVentasTableAdapter;
    }
}