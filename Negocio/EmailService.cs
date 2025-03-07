using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl = true;

        public EmailService()
        {
            _smtpUsername = "cytryn.francisco@gmail.com";
            _smtpPassword = "uncgynvinnmzezyk";
;
        }

        public void EnviarCorreo(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    smtpClient.EnableSsl = _enableSsl;

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(_smtpUsername),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = true  
                    };
                    mensaje.To.Add(destinatario);
                    smtpClient.Send(mensaje);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo electrónico", ex);
            }
        }

        public string CrearTemplateTurnoReprogramado(string nombrePaciente, DateTime nuevaFecha, TimeSpan nuevaHora, string observaciones)
        {
            return $@"
                <h1>¡Su turno ha sido reprogramado!</h1>
                <p>Estimado/a {nombrePaciente}, le informamos que su turno ha sido reprogramado. A continuación, le presentamos los detalles de su nuevo turno:</p>
                <ul>
                    <li><strong>Fecha:</strong> {nuevaFecha:dd/MM/yyyy}</li>
                    <li><strong>Hora:</strong> {nuevaHora.ToString(@"hh\:mm")}</li>
                    <li><strong>Observaciones:</strong> {observaciones}</li>
                </ul>
                <p>Por favor, asegúrese de asistir puntualmente.</p>
                <p>Saludos,<br>Clínica KO</p>";
        }

        public string CrearAsuntoTurnoReprogramado()
        {
            return "Turno Reprogramado";
        }

        public string CrearTemplateAltaPaciente(string nombrePaciente)
        {
            return $@"
                <h1>¡Bienvenido/a a Clínica KO!</h1>
                <p>Estimado/a {nombrePaciente}, le damos la bienvenida a Clínica KO.</p>
                <p>Usted recibirá la mejor atención médica, aunque le duela!.</p>
                <p>Ante cualquier consulta, no dude en comunicarse con nosotros.</p>
                <p>Saludos,<br>Clínica KO</p>";
        }

        public string CrearAsuntoAltaPaciente()
        {
            return "Bienvenido/a a Clínica KO";
        }

        public string CrearTemplateConfirmacionTurno(string nombrePaciente, DateTime fecha, TimeSpan hora, string especialidad, string medico, string observaciones)
        {
            return $@"
                <h1>¡Su turno ha sido confirmado!</h1>
                <p>Estimado/a {nombrePaciente}, le informamos que su turno ha sido confirmado. A continuación, le presentamos los detalles de su nuevo turno:</p>
                <ul>
                    <li><strong>Fecha:</strong> {fecha:dd/MM/yyyy}</li>
                    <li><strong>Hora:</strong> {hora.ToString(@"hh\:mm")}</li>
                    <li><strong>Especialidad:</strong> {especialidad}</li>
                    <li><strong>Médico:</strong> {medico}</li>
                    <li><strong>Observaciones:</strong> {observaciones}</li>
                </ul>
                <p>Por favor, asegúrese de asistir puntualmente.</p>
                <p>Saludos,<br>Clínica KO</p>";
        }
        public string CrearAsuntoConfirmacionTurno()
        {
            return "Confirmación de Turno";
        }

        public string CrearTemplateCancelacionTurno(string nombrePaciente, DateTime fecha, TimeSpan hora, string observaciones)
        {
            return $@"
                <h1>¡Su turno ha sido cancelado!</h1>
                <p>Estimado/a {nombrePaciente}, lamentamos informarle que su turno ha sido cancelado. A continuación, le presentamos los detalles del mismo:</p>
                <ul>
                    <li><strong>Fecha:</strong> {fecha:dd/MM/yyyy}</li>
                    <li><strong>Hora:</strong> {hora.ToString(@"hh\:mm")}</li>
                    <li><strong>Observaciones:</strong> {observaciones}</li>
                </ul>
                <p>Por favor, póngase en contacto si necesita programar un nuevo turno.</p>
                <p>Saludos,<br>Clínica KO</p>";
        }

        public string CrearAsuntoCancelacionTurno()
        {
            return "Cancelación de Turno";
        }

        public string CrearTemplateAltaMedico(string nombreMedico, string email, string especialidades, string horaEntrada, string horaSalida)
        {
            string especialidadesFormateadas = string.Join(", ", especialidades);

            return $@"
                <h1>¡Bienvenido/a a Clínica KO!</h1>
                <p>Estimado/a {nombreMedico}, le damos la bienvenida a nuestra clínica.</p>
                <p>Su registro ha sido exitoso. A continuación, encontrará los detalles de su alta:</p>
                <ul>
                    <li><strong>Email:</strong> {email}</li>
                    <li><strong>Especialidades:</strong> {especialidadesFormateadas}</li>
                    <li><strong>Horario de Trabajo:</strong> {horaEntrada} - {horaSalida}</li>
                </ul>
                <p>Por favor, asegúrese de revisar sus datos y ponerse en contacto con nosotros si necesita realizar alguna modificación.</p>
                <p>Saludos,<br>Clínica KO</p>";
        }
        public string CrearAsuntoAltaMedico()
        {
            return "Bienvenido/a a Clínica KO";
        }
    }
}

