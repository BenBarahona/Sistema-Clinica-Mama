using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Collections;
using System.Data;

namespace SD
{
    public class Correo
    {
        SmtpClient server;
        MailMessage mensage;
        Attachment adjunto, adjunto2;
        string correo, password, host;
        int puerto;

        public Correo()
        {
            correo = "adds.cosecol@gmail.com"; password = "sqlafiliado";
            server = new SmtpClient();
            mensage = new MailMessage();
            this.Establecer_Iniciales();
        }//Sobre carga Constructor

        public Boolean enviarCorreoModificar(String valores)
        {
            String usuario = valores.Split('&')[1];
            bool msg = true;
            string url = "http://DANY-PC:1809/ProyectoSCA_NavigationTestPage.aspx#/ModificarPerfil", tema = "Solicitud: Modificar Perfil de: " + usuario, mensage = "";

            mensage = "Buenas Administrador.\n\nLe informamos que el usuario: " + usuario + " del Sistema SCA quieren modificar sus datos.\n"
                + "Para ver y aceptar los cambios siga este link: " + url + "\n\nAtte.\nSistema SCA";
            do
            {
                msg = this.Enviar_Correo("adds.cosecol@gmail.com;", tema, mensage);
            } while (msg == false);

            return msg;
        }

        public Boolean enviarCorreoCapitalizar(String ruta1, String ruta2)
        {
            bool msg = true, msg2 = true;
            string tema = "Capitalización Normal", mensage = "";

            mensage = "Buenas Administrador.\n\nLe informamos que la capitalización de cierre de mes correspondiente a: "
                + DateTime.Now.Month + "/" + DateTime.Now.Year + " del Sistema SCA se ah realizado.\n"
                + "En el presente estan 2 adjuntos para ver la capitalizacion realizada.\n\nAtte.\nSistema SCA";
            do
            {
                msg = this.Enviar_Correo("adds.cosecol@gmail.com;", tema, mensage, ruta1, ruta2);
            } while (msg == false);

            return msg;
        }

        private void Establecer_Iniciales()
        {
            if (correo.Contains("@hotmail.com"))
            {
                host = "smtp.live.com";
                puerto = 587;
                server.EnableSsl = true;
            }
            else if (correo.Contains("@yahoo.com"))
            {
                puerto = 25;
                host = "out.izymail.com";
                server.EnableSsl = false;
            }
            else if (correo.Contains("@gmail.com") || correo.Contains("@unitec.edu"))
            {
                host = "smtp.gmail.com";
                puerto = 587;
                server.EnableSsl = true;
            }

            server.Host = this.host;
            server.Port = this.puerto;
            server.Credentials = new System.Net.NetworkCredential(correo, password);
        }//Establece q servidor, puerto, eh inicia sessio.

        private Boolean Enviar_Correo(string correo_a_enviar, string tema, string mensaje)
        {
            string[] a;
            a = correo_a_enviar.Split(';');

            try
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (!a[i].Equals(""))
                    {
                        mensage.From = new System.Net.Mail.MailAddress(this.correo);
                        mensage.To.Add(a[i]);
                        mensage.Subject = tema;
                        mensage.Body = mensaje;
                    }
                }

                server.Send(mensage);
                return true;
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message.ToString());
                return false;
            }
        }//Enviar correo tema

        private Boolean Enviar_Correo(string correo_a_enviar, string tema, string mensaje, string ruta, string ruta2)
        {
            string[] a;
            a = correo_a_enviar.Split(';');

            try
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (!a[i].Equals(""))
                    {
                        adjunto = new System.Net.Mail.Attachment(ruta);
                        adjunto2 = new System.Net.Mail.Attachment(ruta2);
                        mensage.From = new System.Net.Mail.MailAddress(this.correo);
                        mensage.To.Add(a[i]);
                        mensage.Subject = tema;
                        mensage.Body = mensaje;
                        mensage.Attachments.Add(adjunto);
                        mensage.Attachments.Add(adjunto2);
                    }
                }

                server.Send(mensage);
                return true;
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.Message.ToString());
                return false;
            }
        }//Enviar correo completo
    }
}