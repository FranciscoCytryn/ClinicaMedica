using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;

namespace ClinicaMedica
{
    public partial class Informes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuario = Session["usuario"];

                if (usuario == null || !Seguridad.SesionActiva(usuario) || !(Seguridad.EsAdmin(usuario) || Seguridad.EsRecepcionista(usuario)))
                {
                    Response.Redirect("Login.aspx");
                    Response.End();
                    return;
                }

                pnlFiltros.Visible = false;
                pnlResultados.Visible = false;
                btnDescargarPDF.Visible = false;
            }
            else
            {
                btnDescargarPDF.Visible = InformeGenerado;
            }
        }
        public bool InformeGenerado { get; set; } = false;


        protected void ddlTipoInforme_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFiltros.Visible = !string.IsNullOrEmpty(ddlTipoInforme.SelectedValue);
            pnlResultados.Visible = false; 
        }

        protected void btnGenerarInforme_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaInicio = DateTime.Parse(txtFechaInicio.Text);
                DateTime fechaFin = DateTime.Parse(txtFechaFin.Text);

                if (fechaInicio > fechaFin)
                {
                    MostrarError("La fecha de inicio no puede ser mayor que la fecha de fin.");
                    return;
                }

                // Determinar qué informe generar según el valor seleccionado en el DropDownList
                switch (ddlTipoInforme.SelectedValue)
                {
                    case "TurnosPorMedico":
                        GenerarInformeMedicosConTurnosCerrados(fechaInicio, fechaFin);
                        break;
                    case "PacientesAtendidos":
                        GenerarInformePacientesAtendidos(fechaInicio, fechaFin);
                        break;
                    default:
                        MostrarError("Seleccione un tipo de informe válido.");
                        break;
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al generar el informe: {ex.Message}");
            }
        }


        private void GenerarInformePacientesAtendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            var resultados = pacienteNegocio.ObtenerPacientesAtendidos(fechaInicio, fechaFin);

            gvResultados.DataSource = resultados;
            gvResultados.DataBind();
            pnlResultados.Visible = true;
            InformeGenerado = true;
            btnDescargarPDF.Visible = true;
        }

        private void GenerarInformeMedicosConTurnosCerrados(DateTime fechaInicio, DateTime fechaFin)
        {
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            var resultados = turnoNegocio.ObtenerInformeMedicosConTurnosCerrados(fechaInicio, fechaFin);

            gvResultados.DataSource = resultados;
            gvResultados.DataBind();
            pnlResultados.Visible = true;
            InformeGenerado = true;
            btnDescargarPDF.Visible = true;
        }

        private void MostrarError(string mensaje)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showerror", $"alert('{mensaje}');", true);
        }

        protected void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvResultados.Rows.Count == 0)
                {
                    MostrarError("No hay datos para generar el PDF.");
                    return;
                }

                int columnCount = gvResultados.Rows[0].Cells.Count;
                if (columnCount == 0)
                {
                    MostrarError("El GridView no tiene columnas definidas.");
                    return;
                }

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Informe KO")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16));

                    var table = new iText.Layout.Element.Table(UnitValue.CreatePercentArray(columnCount)).UseAllAvailableWidth();

                    foreach (TableCell cell in gvResultados.HeaderRow.Cells)
                    {
                        table.AddHeaderCell(cell.Text);
                    }

                    foreach (GridViewRow row in gvResultados.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            string cellText = cell.Text;
                            if (string.IsNullOrEmpty(cellText))
                            {
                                cellText = " ";  
                            }
                            table.AddCell(cellText);
                        }
                    }

                    document.Add(table);

                    document.Close();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=InformeGenerado.pdf");

                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al generar el archivo PDF: {ex.Message}");
            }
        }
    }
}