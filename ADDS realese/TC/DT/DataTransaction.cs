using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DT
{
    public class DataTransaction
    {
        private SqlCommand Query;
        private SqlDataReader DataReader;
        private SqlConnection Connection;
        private string ConnectionString = "Server=DANY-PC;Database=COSECOL; User ID=CosecolUser; Password=sqlafiliado1; " +
                                  "Trusted_Connection=False", QueryString = "";

        /*********************************************************************************************************************************/
        /***************************************************KOTARO************************************************************************/
        /*********************************************************************************************************************************/

        /*por problemas del protocolo HTTP que envia paquetes a cada rato, la referencia a la conexion siempre se perdera
         *por lo tanto en cada metodo debemos de crear una conexion nueva y desconectarnos para ello simplemente debemos
         *de llamar antes de cada query al metodo CONECTAR*/

        /*setters*/
        public Boolean InsertarEmpleado(String EmpleadoDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN INSERTAR_EMPLEADO ";
            string[] datos = EmpleadoDK.Split(';'), datos_personales = datos[0].Split(','), otros_datos = datos[3].Split(',');

            /*insertar la persona*/
            QueryString += "INSERT INTO Persona VALUES ('" + datos_personales[1] + "','" + datos_personales[2] + "','" +
                datos_personales[3] + "','" + datos_personales[4] + "','" + datos_personales[0] + "','" + datos_personales[5] +
                "','" + datos_personales[6] + "','" + datos_personales[7] + "','" + datos_personales[8] + "'); ";
            /*telefonos*/
            string[] telefonos = datos[1].Split(',');
            for (int i = 0; i < telefonos.Length; i++)
                QueryString += " INSERT INTO [COSECOL].[dbo].[Telefono_Personal]([ID_Persona],[Telefono]) VALUES" +
                        "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] +
                        "'),'" + telefonos[i] + "'); ";
            /*celulares*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " INSERT INTO [COSECOL].[dbo].[Celular]([ID_Persona],[Celular]) VALUES" +
                      "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "')" +
                      ",'" + celulares[i] + "'); ";
            /*nueva cuenta de usuario*/
            QueryString += " INSERT INTO [COSECOL].[dbo].[Usuario]([Correo_Electronico]) VALUES('" + otros_datos[0] + "'); ";
            /*ahora asignamos un rol a nuestro usuario*/
            QueryString += " INSERT INTO Desempenia VALUES((SELECT [COSECOL].[dbo].[Usuario].ID_Usuario FROM [COSECOL].[dbo]."
                + "[Usuario] WHERE [COSECOL].[dbo].[Usuario].Correo_Electronico = '" + otros_datos[0] + "'),(SELECT "
                + "Rol.ID_Rol FROM Rol WHERE Rol.Nombre = 'Empleado')); ";
            /*ahora insertamos al empleado*/
            QueryString += " INSERT INTO Empleado VALUES ( (CONVERT(date,GETDATE())) ,(SELECT Persona.ID_Persona FROM Persona " +
                    "WHERE Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Usuario.ID_Usuario FROM Usuario WHERE " +
                    " Usuario.Correo_Electronico = '" + otros_datos[0] + "'),'" + otros_datos[1] + "'); ";
            /*roolback por si las de hule*/
            QueryString += " if ( @@ERROR != 0 ) ROLLBACK TRAN INSERTAR_EMPLEADO; else COMMIT TRAN INSERTAR_EMPLEADO;";
            return EjecutarComando();
        }
        public Boolean InsertarAfiliado(String AfiliadoDK)
        {
            Conectar();
            /*obtener datos de afiliado*/
            string[] datos = AfiliadoDK.Split(';');
            string[] datos_personales = datos[0].Split(',');

            QueryString = "BEGIN TRAN INSERTAR_AFILIADO ";

            /*query de persona*/
            QueryString += "INSERT INTO [COSECOL].[dbo].[Persona] ([P_Nombre],[S_Nombre],[P_Apellido],[S_Apellido]," +
              "[No_Identidad],[Fecha_Nacimiento],[Estado_Civil],[Genero],[Direccion]) VALUES('" + datos_personales[1] +
              "','" + datos_personales[2] + "','" + datos_personales[3] + "','" + datos_personales[4] + "','" + datos_personales[0] +
              "','" + datos_personales[5] + "','" + datos_personales[6] + "','" + datos_personales[7] + "','" + datos_personales[8] + "') ";
            /*telefonos*/
            string[] telefonos = datos[1].Split(',');
            for (int i = 0; i < telefonos.Length; i++)
                QueryString += " ; INSERT INTO [COSECOL].[dbo].[Telefono_Personal]([ID_Persona],[Telefono]) VALUES" +
                        "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] +
                        "'),'" + telefonos[i] + "')";
            /*celulares*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " ; INSERT INTO [COSECOL].[dbo].[Celular]([ID_Persona],[Celular]) VALUES" +
                      "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "')" +
                      ",'" + celulares[i] + "')";
            /*correo electronico y una nueva cuenta de usuario*/
            QueryString += "; INSERT INTO [COSECOL].[dbo].[Usuario]([Correo_Electronico]) VALUES('" + datos[3] + "') ; " +
                "INSERT INTO [COSECOL].[dbo].[Cuenta] ([Saldo],[Fecha_Creacion]) VALUES( 0 , Convert( date , GETDATE()))";
            /*ahora asignamos un rol a nuestro usuario*/
            QueryString += " ; INSERT INTO Desempenia VALUES((SELECT [COSECOL].[dbo].[Usuario].ID_Usuario FROM [COSECOL].[dbo]."
                + "[Usuario] WHERE [COSECOL].[dbo].[Usuario].Correo_Electronico = '" + datos[3] + "'),(SELECT "
                + "Rol.ID_Rol FROM Rol WHERE Rol.Nombre = 'Afiliado')) ";
            /*ahora viene lo bueno... insertar el afiliado"*/
            string[] datos_laborales = datos[4].Split(',');
            QueryString += " ;INSERT INTO [COSECOL].[dbo].[Afiliado]([ID_Persona],[Fecha_Ingreso],[ID_Usuario],[Num_Certificado]" +
                ",[Empresa_Nombre],[Empresa_Telefono],[Empresa_Direccion],[Empresa_Tiempo_Laboracion],[Empresa_Dpto]," +
                "[ID_Ocupacion],[Lugar_Nacimiento],[Estado]) VALUES((SELECT [Persona].[ID_Persona] FROM Persona WHERE" +
                " Persona.No_Identidad = '" + datos_personales[0] + "'),Convert( date , GETDATE()),(SELECT " +
                "[COSECOL].[dbo].[Usuario].ID_Usuario FROM [COSECOL].[dbo].[Usuario] WHERE [COSECOL].[dbo].[Usuario]." +
                "Correo_Electronico = '" + datos[3] + "') , (SELECT MAX(Cuenta.Num_Certificado) FROM Cuenta)," +
                "'" + datos_laborales[0] + "','" + datos_laborales[1] + "','" + datos_laborales[2] + "','" + datos_laborales[3] +
                "','" + datos_laborales[4] + "',(SELECT Ocupacion.ID_Ocupacion FROM Ocupacion WHERE Ocupacion.Ocupacion = '"
                + datos[5] + "' AND Ocupacion.Valido = 'TRUE' ),'" + datos_laborales[5] + "','Activo')";
            /*beneficiario de contingencia*/
            string[] beneficiario_contingencia = datos[6].Split(',');
            QueryString += " ; INSERT INTO [COSECOL].[dbo].[Persona] ([P_Nombre],[S_Nombre],[P_Apellido],[S_Apellido]," +
                "[No_Identidad],[Fecha_Nacimiento],[Estado_Civil],[Genero],[Direccion]) VALUES('" + beneficiario_contingencia[1] +
                "','" + beneficiario_contingencia[2] + "','" + beneficiario_contingencia[3] + "','" + beneficiario_contingencia[4]
                + "','" + beneficiario_contingencia[0] + "','" + beneficiario_contingencia[5] + "','" +
                beneficiario_contingencia[6] + "','" + beneficiario_contingencia[7] + "','" + beneficiario_contingencia[8] +
                "') ; INSERT INTO [COSECOL].[dbo].[Beneficiario] ([ID_Persona],[ID_Afiliado],[ID_Parentesco])"
                + "VALUES ((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_contingencia[0] + "'),(SELECT ID_Afiliado FROm Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_contingencia[9] + "')) ; " +
                "INSERT INTO [COSECOL].[dbo].[Beneficiario_Contingencia] ([ID_Beneficiario],[Activo]) " +
                "VALUES((SELECT Beneficiario.ID_Beneficiario FROM Beneficiario,Persona "
                + "WHERE Beneficiario.ID_Persona = Persona.ID_Persona and Persona.No_Identidad = '"
                + beneficiario_contingencia[0] + "'),'TRUE')";
            /*beneficiarios normales*/
            for (int i = 7; i < datos.Length; i++)
            {
                string[] beneficiario_normal = datos[i].Split(',');
                QueryString += " ; INSERT INTO [COSECOL].[dbo].[Persona] ([P_Nombre],[S_Nombre],[P_Apellido],[S_Apellido]," +
                "[No_Identidad],[Fecha_Nacimiento],[Estado_Civil],[Genero],[Direccion]) VALUES('" + beneficiario_normal[1] +
                "','" + beneficiario_normal[2] + "','" + beneficiario_normal[3] + "','" + beneficiario_normal[4] + "','"
                + beneficiario_normal[0] +
                "','" + beneficiario_normal[5] + "','" + beneficiario_normal[6] + "','" + beneficiario_normal[7] + "','"
                + beneficiario_normal[8] + "') ; INSERT INTO [COSECOL].[dbo].[Beneficiario] ([ID_Persona],[ID_Afiliado]," +
                "[ID_Parentesco]) VALUES ((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_normal[0] + "'),(SELECT ID_Afiliado FROm Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_normal[9] + "')) ; INSERT INTO" +
                " [COSECOL].[dbo].[Beneficiario_Normal] ([ID_Beneficiario],[Porcentaje_Seguro],[Procentaje_Aportaciones])" +
                " VALUES((SELECT Beneficiario.ID_Beneficiario FROM Beneficiario,Persona WHERE Beneficiario.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + beneficiario_normal[0] + "')," + beneficiario_normal[10] + ","
                + beneficiario_normal[11] + ")";
            }

            /*rollback por si acaso*/
            QueryString += "; if ( @@ERROR != 0 ) ROLLBACK TRAN INSERTAR_AFILIADO; else COMMIT TRAN INSERTAR_AFILIADO;";
            return EjecutarComando();
        }

        public Boolean InsertarOcupacion(String OcupacionDK)
        {
            Conectar();

            /*primero verificaremos que no halla otro interes valido con el mismo nombre, pues
             * la base de datos debe poder insertar varios con el mismo nombre pero solo 1 puede ser valido*/

            QueryString = "SELECT COUNT(*) FROM Ocupacion WHERE Ocupacion.Ocupacion = '" + OcupacionDK
                + "' AND Ocupacion.Valido = 'TRUE'";


            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                        return false; /*pues existe por lomenos uno que es valido con el mismo nombre*/

            /*ahora ya estamos seguros de poder insertar el interes*/

            Conectar();

            QueryString = "BEGIN TRAN INSERTAR_OCUPACION INSERT INTO Ocupacion VALUES ('" + OcupacionDK + "','TRUE');" +
                            "if ( @@ERROR != 0 ) ROLLBACK TRAN INSERTAR_OCUPACION; else COMMIT TRAN INSERTAR_OCUPACION;";
            return EjecutarComando();
        }
        public Boolean InsertarMotivo(String MotivoDK)
        {
            Conectar();

            /*primero verificaremos que no halla otro interes valido con el mismo nombre, pues
             * la base de datos debe poder insertar varios con el mismo nombre pero solo 1 puede ser valido*/

            QueryString = "SELECT COUNT(*) FROM Motivo WHERE Motivo.Motivo = '" + MotivoDK
                + "' AND Motivo.Valido = 'TRUE'";


            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                        return false; /*pues existe por lomenos uno que es valido con el mismo nombre*/

            /*ahora ya estamos seguros de poder insertar el interes*/

            Conectar();

            QueryString = "BEGIN TRAN INSERTAR_MOTIVO INSERT INTO Motivo VALUES ('" + MotivoDK + "' , 'TRUE');" +
                            " if ( @@ERROR != 0 ) ROLLBACK TRAN INSERTAR_MOTIVO; else COMMIT TRAN INSERTAR_MOTIVO;";
            return EjecutarComando();
        }
        public Boolean InsertarParentesco(String ParentescoDK)
        {
            Conectar();

            /*primero verificaremos que no halla otro interes valido con el mismo nombre, pues
             * la base de datos debe poder insertar varios con el mismo nombre pero solo 1 puede ser valido*/

            QueryString = "SELECT COUNT(*) FROM Parentesco WHERE Parentesco.Parentesco = '" + ParentescoDK
                + "' AND Parentesco.Valido = 'TRUE'";


            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                        return false; /*pues existe por lomenos uno que es valido con el mismo nombre*/

            /*ahora ya estamos seguros de poder insertar el interes*/

            Conectar();

            QueryString = "BEGIN TRAN INSERTAR_PARENTESCO INSERT INTO Parentesco VALUES ('" + ParentescoDK + "' , 'TRUE');" +
                            "if ( @@ERROR != 0 ) ROLLBACK TRAN INSERTAR_PARENTESCO; else COMMIT TRAN INSERTAR_PARENTESCO;";
            return EjecutarComando();
        }
        public Boolean InsertarNuevoMonto(String MontoDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN INSERTAR_MONTO; INSERT INTO Monto VALUES(" + MontoDK + ",CONVERT(date,GETDATE()));" +
                            " if @@ERROR != 0 ROLLBACK TRAN INSERTAR_MONTO; else COMMIT TRAN INSERTAR_MONTO;";
            return EjecutarComando();
        }
        public Boolean InsertarNuevoLimite(String LimiteDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN INSERTAR_LIMITE; INSERT INTO Limite VALUES(" + LimiteDK + ",CONVERT(date,GETDATE()));" +
                           "if @@ERROR != 0 ROLLBACK TRAN INSERTAR_LIMITE; else COMMIT TRAN INSERTAR_LIMITE;";
            return EjecutarComando();
        }
        public Boolean InsertarInteres(String InteresDK)
        {
            Conectar();
            String[] datos_interes = InteresDK.Split(',');

            /*primero verificaremos que no halla otro interes valido con el mismo nombre, pues
             * la base de datos debe poder insertar varios con el mismo nombre pero solo 1 puede ser valido*/

            QueryString = "SELECT COUNT(*) FROM Interes WHERE Interes.Nombre = '" + datos_interes[0]
                + "' AND Interes.Valido = 'TRUE'";


            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                        return false; /*pues existe por lomenos uno que es valido con el mismo nombre*/

            /*ahora ya estamos seguros de poder insertar el interes*/

            Conectar();

            QueryString = "BEGIN TRAN INSERTAR_INTERES INSERT INTO Interes VALUES( '" + datos_interes[0] + "',(CONVERT(date,GETDATE())) , " +
                datos_interes[1] + " , 'TRUE' , '" + datos_interes[2] + "' , '" + datos_interes[3] + "' , '" +
                datos_interes[4] + "' , '" + datos_interes[5] + "' , " + datos_interes[6] + " ); " +
                "if @@ERROR != 0 ROLLBACK TRAN INSERTAR_INTERES; else COMMIT TRAN INSERTAR_INTERES;";


            return EjecutarComando();
        }

        /*getters*/
        public String getTodasOcupaciones()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_OCUPACIONES SELECT Ocupacion.Ocupacion FROM Ocupacion WHERE Ocupacion.Valido = 'TRUE';" +
                    "if ( @@ERROR != 0 ) ROLLBACK TRAN GET_OCUPACIONES; else COMMIT TRAN GET_OCUPACIONES;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    ocupaciones += DataReader.GetString(0).Trim() + ",";
            if (ocupaciones.Length >= 1)
                return ocupaciones.Substring(0, ocupaciones.Length - 1);
            return ocupaciones;
        }
        public String getTodosMotivos()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_MOTIVOS SELECT Motivo.Motivo FROM Motivo WHERE Motivo.Valido = 'TRUE';" +
                    "if ( @@ERROR != 0 ) ROLLBACK TRAN GET_MOTIVOS; else COMMIT TRAN GET_MOTIVOS;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    ocupaciones += DataReader.GetString(0).Trim() + ",";
            if (ocupaciones.Length >= 1)
                return ocupaciones.Substring(0, ocupaciones.Length - 1);
            return ocupaciones;
        }
        public String getTodosParentescos()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_PARENTESCOS SELECT Parentesco.Parentesco FROM Parentesco WHERE Parentesco.valido = " +
                    "'TRUE'; if ( @@ERROR != 0 ) ROLLBACK TRAN GET_PARENTESCOS; else COMMIT TRAN GET_PARENTESCOS;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    ocupaciones += DataReader.GetString(0).Trim() + ",";
            if (ocupaciones.Length >= 1)
                return ocupaciones.Substring(0, ocupaciones.Length - 1);
            return ocupaciones;
        }
        public String getMontoActual()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_MONTO; SELECT Monto.Monto FROM Monto WHERE Monto.ID_Monto = " +
                "(SELECT MAX(Monto.ID_Monto) FROM Monto); if @@ERROR != 0  ROLLBACK TRAN GET_MONTO; else COMMIT TRAN GET_MONTO;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    ocupaciones += DataReader.GetValue(0).ToString().Trim() + ",";
            return ocupaciones.Substring(0, ocupaciones.Length - 1);
        }
        public String getLimiteActual()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_LIMITE; SELECT Limite.Cantidad FROM Limite WHERE Limite.ID_Limite =" +
                   "(SELECT MAX(Limite.ID_Limite ) FROM Limite); if @@ERROR != 0 ROLLBACK TRAN GET_LIMITE;" +
                   "else COMMIT TRAN GET_LIMITE;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    ocupaciones += DataReader.GetValue(0).ToString().Trim() + ",";
            return ocupaciones.Substring(0, ocupaciones.Length - 1);
        }
        public String getTodosIntereses()
        {
            Conectar();
            QueryString = "BEGIN TRAN GET_INTERESES; SELECT Nombre,Taza,Aplica_Obligatoria,Aplica_Voluntaria," +
                "Aplica_Obligatoria_Especial,Aplica_Voluntaria_ArribaDe,ArribaDe FROM Interes WHERE Interes.Valido = 'TRUE';" +
                "if @@ERROR != 0 ROLLBACK TRAN GET_INTERESES; else COMMIT TRAN GET_INTERESES;";
            String ocupaciones = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String temp = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        temp += DataReader.GetValue(i).ToString().Trim() + ",";
                    ocupaciones += temp.Substring(0, temp.Length - 1) + ";";
                }
            return ocupaciones.Substring(0, ocupaciones.Length - 1);
        }

        /*deletes*/
        public Boolean DeleteOcupacion(String OcupacionDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN ELIMINAR_OCUPACION UPDATE Ocupacion  SET Ocupacion.Valido = 'FALSE' WHERE Ocupacion.Ocupacion = '"
                + OcupacionDK + "'; if ( @@ERROR != 0 ) ROLLBACK TRAN ELIMINAR_OCUPACION; else COMMIT TRAN ELIMINAR_OCUPACION;";
            return EjecutarComando();
        }
        public Boolean DeleteMotivo(String MotivoDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN ELIMINAR_MOTIVO UPDATE Motivo SET Motivo.Valido = 'FALSE' WHERE Motivo.Motivo = '" + MotivoDK
                + "'; if ( @@ERROR != 0 ) ROLLBACK TRAN ELIMINAR_MOTIVO; else COMMIT TRAN ELIMINAR_MOTIVO;";
            return EjecutarComando();
        }
        public Boolean DeleteParentesco(String ParentescoDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN ELIMINAR_PARENTESCO UPDATE Parentesco SET Parentesco.Valido = 'FALSE' WHERE " +
                "Parentesco.Parentesco = '" + ParentescoDK + "'; if ( @@ERROR != 0 ) ROLLBACK TRAN ELIMINAR_PARENTESCO;" +
                "else COMMIT TRAN ELIMINAR_PARENTESCO;";
            return EjecutarComando();
        }
        public Boolean DeleteInteres(String InteresDK)
        {
            Conectar();
            QueryString = "BEGIN TRAN ELIMINAR_INTERES UPDATE Interes SET Interes.Valido = 'FALSE' WHERE Interes.Nombre = '"
                + InteresDK + "'; if ( @@ERROR != 0 ) ROLLBACK TRAN ELIMINAR_INTERES; else COMMIT TRAN ELIMINAR_INTERES;";
            return EjecutarComando();
        }

        /*El metodo que debe retornar todo método para saber si todo se inserto en la base de datos, o para saber
         *si un select o update tubo efecto etc.*/
        public bool EjecutarComando()
        {
            try
            {
                Query = new SqlCommand(QueryString);
                Query.Connection = Connection;
                DataReader = Query.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        /*Conecta a la Base de Datos con el ConnectionString y deja abierta la conexión*/
        public void Conectar()
        {
            Connection = new SqlConnection();
            Connection.ConnectionString = ConnectionString;
            Connection.Open();
        }

        /*********************************************************************************************************************************/
        /***********************************************FIN-KOTARO************************************************************************/
        /*********************************************************************************************************************************/

    }
}
