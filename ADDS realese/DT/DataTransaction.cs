using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DT
{

    /*DT REALESE 1.0.0.0 BY: KOTARO*/
    /*Esta version soporta lo que es el registro de aportaciones y el Log*/
    public class DataTransaction
    {
        /*Esta es la clase DT (data transaction), esta clase se encarga totalmente de los queries a la base de datos
         *debe ser el unico elemento que tenga acceso a la base de datos, no se puede aceder a la base de datos sin pasar
         *por el DT, el DT proporciona todo los métodos necesarios para la inserción, eliminación, actualización y
         *recuperación de los datos en la base de datos COSECOL. El DT se comunica por medio dle protocolo DK*/

        /*VARIABLES DEL DT*/
        /*Esta es la variable donde se ingresan los Queries para ser ejecutados en la BD*/
        public static SqlCommand Query;
        /*Esta es la variable que se encarga de leer datos de la BD*/
        public static SqlDataReader DataReader; 
        /*Esta es la variable que se encaraga de la conexión a la BD*/
        public static SqlConnection Connection; 
        /*Esta variable almacena la información necesaria para conectarse a la BD, debe ser completamente privada!!*/
        public static string ConnectionString = "Server=DANY-PC;" +
                                                "Database=SCA;" +
                                                "User ID=sa;" +
                                                "Password=sql; " +
                                                "Trusted_Connection=False",
            //"Data Source=DEVPC-02\\SQLEXPRESS;Initial Catalog=SCA;Integrated Security=True",
        /*Esta es la variable donde escribimos todos los queries*/
                              QueryString = "";
        
/*********************************************************************************************************************************/
/***************************************************KOTARO************************************************************************/
/*********************************************************************************************************************************/

        /*por problemas del protocolo HTTP que envia paquetes a cada rato, la referencia a la conexion siempre se perdera
         *por lo tanto en cada metodo debemos de crear una conexion nueva y desconectarnos para ello simplemente debemos
         *de llamar antes de cada query al metodo CONECTAR, El standard para enviar datos es enviarlos separados por , 
         *pero para resibir y regresar objetos mas grandes como un afiliado o un empleado se utiliza lo que es el formato DK
         *que por motivos de segurdad no estan especificados en esta libreria, para utilización de estos metodos se requiere
         *conocer el protocolo DK.*/

        /*INSERTS A LA BASE DE DATOS*/
        public static Boolean InsertarEmpleado(String EmpleadoDK)
        {
            Boolean esadmin = Convert.ToBoolean(EmpleadoDK.Split('&')[2]);
            EmpleadoDK = EmpleadoDK.Split('&')[0] + '&' + EmpleadoDK.Split('&')[1]; 
            String tempDK = EmpleadoDK;
            EmpleadoDK = traductor.DK_Empleado(EmpleadoDK);
            string responsable = EmpleadoDK.Split('&')[1];
            EmpleadoDK = EmpleadoDK.Split('&')[0];
            /*Para Insertar Empleado se utiliza el Protocolo DK*/
            /*separamos los datos del empleado tal como lo especifica el Protocolo DK*/
            string[] datos = EmpleadoDK.Split(';'),
                     datos_personales = datos[0].Split(','),
                     otros_datos = datos[3].Split(',');
            /*PERSONA*/
                /*Agregamos a la transaccion el Query para insertar la persona que sera el empleado*/
            QueryString = "INSERT INTO Persona VALUES ('"+ datos_personales[1] + "','" + datos_personales[2] + "','" +
                datos_personales[3] + "','" + datos_personales[4] + "','" + datos_personales[0] +"','" + datos_personales[5] +
                "','" + datos_personales[6] + "','" + datos_personales[7] + "','" + datos_personales[8] + "' , GETDATE() , " +
                "GETDATE() , '" + responsable + "' , '" + responsable + "') ";
            /*TELEFONOS*/
                /*agregamos al Query todos los queries necesarios para insertar los telefonos del empleado*/
            string[] telefonos = datos[1].Split(',');
            for (int i = 0; i < telefonos.Length; i++)
                QueryString += " INSERT INTO Telefono_Personal VALUES" +
                        "((SELECT Persona.ID_Persona FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] +
                        "'),'" + telefonos[i] + "') ";
            /*CELULARES*/
                /*agregamos todos los celulares del empleado*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " INSERT INTO Celular VALUES" +
                      "((SELECT Persona.ID_Persona FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "')" +
                      ",'" + celulares[i] + "' ) ";
            /*USUARIO*/
                /*agregamos al Query como insertar un usuario, con el password encryptado*/
            QueryString += " INSERT INTO Usuario VALUES('" +
                otros_datos[0] + "' , '" + otros_datos[1] + "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') ";
            /*ROL*/
                /*ahora asignamos un rol a nuestro usuario, todos los roles de un empleado*/
                QueryString += " INSERT INTO Desempenia VALUES((SELECT Usuario.ID_Usuario FROM Usuario WHERE " +
                    "Usuario.Correo_Electronico = '" + otros_datos[0] + "'),(SELECT " + "Rol.ID_Rol FROM Rol WHERE " +
                    "Rol.Nombre = '" + (esadmin?"Administrador":"Empleado") + "'), GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') ";
            /*EMPLEADO*/
                /*ahora insertamos al empleado*/
            QueryString += " INSERT INTO Empleado VALUES ( (CONVERT(date,GETDATE())) ,(SELECT Persona.ID_Persona FROM Persona " +
					"WHERE Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Usuario.ID_Usuario FROM Usuario WHERE " +
                    " Usuario.Correo_Electronico = '" + otros_datos[0] + "'),'" + otros_datos[2] + "', 'TRUE' ,GETDATE() , GETDATE() , '"
                    + responsable + "' , '" + responsable + "') ";
            QueryString += Log.Insercion(tempDK, "Empleado");
            /*ya que el Query esta listo, Conectamos y ejecutamos!*/
            if( EjecutarComando() ) 
            { 
                FinalizarComandoCorrectamente(); 
                return true; 
            } else 
                return false;
        }
        public static Boolean   InsertarAfiliado(String AfiliadoDK)
        {
            Boolean YaExistiaLaPersona = false;

            String tempDK = AfiliadoDK;
            AfiliadoDK = traductor.DK_Afiliado(AfiliadoDK);
            string responsable = AfiliadoDK.Split('&')[1];
            AfiliadoDK = AfiliadoDK.Split('&')[0];
            /*Para insertar un afiliado usamos el Protocolo DK*/
            /*obtener datos de afiliado*/
            string[] datos = AfiliadoDK.Split(';');
            string[] datos_personales = datos[0].Split(',');
            /*****************************OCUPAMOS VERIFICAR SI YA EXISTE EL AFILIADO**************************************/
            QueryString = "SELECT COUNT(*) FROM Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona AND Persona.No_Identidad = '" + datos_personales[0] + "'";
            if( EjecutarComando() )
                if( DataReader.Read() )
                    if (DataReader.GetValue(0).ToString().Trim() != "0")
                    {
                        FinalizarComandoCorrectamente();
                        return false;
                    }
            FinalizarComandoCorrectamente();
            /******************************VERIFICAMOS SI YA EXISTE LA PERSONA********************************************/
            QueryString = "SELECT COUNT(*) FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "'";
            if (EjecutarComando())
                if (DataReader.Read())
                    YaExistiaLaPersona = (DataReader.GetValue(0).ToString().Trim() != "0");
            FinalizarComandoCorrectamente();
            /*******************************FIN VERIFICACIONES RARAS********************************************************/
            QueryString = "";
            if (!YaExistiaLaPersona)
            {
                /*query de persona*/
                QueryString = "INSERT INTO Persona VALUES('" + datos_personales[1] + "','" + datos_personales[2] + "','" + datos_personales[3] +
                  "','" + datos_personales[4] + "','" + datos_personales[0] + "','" + datos_personales[5] + "','" + datos_personales[6] + "','" +
                  datos_personales[7] + "','" + datos_personales[8] + "' , GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') ";
            } 
             /*telefonos*/
            string[] telefonos = datos[1].Split(',');
            for( int i = 0; i < telefonos.Length ; i++)
            QueryString += " INSERT INTO Telefono_Personal VALUES"+
                    "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '"+datos_personales[0]+
                    "'),'" + telefonos[i] + "' )";
            /*celulares*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " INSERT INTO Celular VALUES" +
                      "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" +datos_personales[0]+ "')"+
                      ",'" + celulares[i] + "' )";
        
            /*correo electronico y una nueva cuenta de usuario*/
            string[] datos_usuario = datos[3].Split(',');
            QueryString += " INSERT INTO Usuario VALUES('" + datos_usuario[0] +
                "','" + datos_usuario[1] + "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')  " +
                " INSERT INTO Cuenta VALUES( 0 , GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')";
            /*ahora asignamos un rol a nuestro usuario*/
            QueryString += " INSERT INTO Desempenia VALUES((SELECT Usuario.ID_Usuario FROM Usuario WHERE Usuario.Correo_Electronico = '"
                + datos_usuario[0] + "'),(SELECT Rol.ID_Rol FROM Rol WHERE Rol.Nombre = 'Afiliado'), GETDATE() , GETDATE() , '" 
                + responsable + "' , '" + responsable + "') ";
            /*ahora viene lo bueno... insertar el afiliado"*/
            string[] datos_laborales = datos[4].Split(',');
            QueryString += " INSERT INTO Afiliado VALUES((SELECT Persona.ID_Persona FROM Persona WHERE" +
                " Persona.No_Identidad = '" + datos_personales[0] + "'), GETDATE() ,(SELECT " + 
                " Usuario.ID_Usuario FROM Usuario WHERE Usuario.Correo_Electronico = '" + datos_usuario[0] + "') ," +
                " (SELECT MAX(Cuenta.Num_Certificado) FROM Cuenta), (SELECT Ocupacion.ID_Ocupacion FROM Ocupacion WHERE" +
                " Ocupacion.Ocupacion = '" + datos[5] + "' AND Ocupacion.Valido = 'TRUE' ) ,'" + datos_laborales[0] + "','" + datos_laborales[1] + "','" + datos_laborales[2] +
                "','"+datos_laborales[3]+"','" + datos_laborales[4] + "','" + datos_laborales[5] + 
                "','Activo', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') ";
            /*beneficiario de contingencia*/
            string[] beneficiario_contingencia = datos[6].Split(',');
            QueryString += " IF (SELECT COUNT(*) FROM Persona WHERE No_Identidad = '" + beneficiario_contingencia[0] + "' ) = 0 " + 
                "INSERT INTO Persona VALUES('" + beneficiario_contingencia[1] +
                "','" + beneficiario_contingencia[2] + "','" + beneficiario_contingencia[3] + "','" + beneficiario_contingencia[4]
                + "','" + beneficiario_contingencia[0] + "','" + beneficiario_contingencia[5] + "','" +
                beneficiario_contingencia[6] + "','" + beneficiario_contingencia[7] + "','" + beneficiario_contingencia[8] +
                "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')  INSERT INTO Beneficiario VALUES"  +
                " ((SELECT Persona.ID_Persona FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_contingencia[0] + "'),(SELECT ID_Afiliado FROM Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_contingencia[9] + "'), 'TRUE' ,GETDATE() , GETDATE() , '" + 
                responsable + "' , '" + responsable + "') INSERT INTO Beneficiario_Contingencia VALUES((SELECT MAX(Beneficiario.ID_Beneficiario)" +
                " FROM Beneficiario,Persona WHERE Beneficiario.ID_Persona = Persona.ID_Persona and Persona.No_Identidad = '"
                + beneficiario_contingencia[0] + "'),'TRUE', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')";
            /*beneficiarios normales*/
            for (int i = 7; i < datos.Length; i++)/*Aqui empieza desde 7 porque despues de septimo ; empiezan los Beneficiarios normales*/
            {
                string[] beneficiario_normal = datos[i].Split(',');
                QueryString += " IF (SELECT COUNT(*) FROM Persona WHERE No_Identidad = '" + beneficiario_normal[0]  + "' ) = 0 " +
                    " INSERT INTO Persona VALUES('" + beneficiario_normal[1] +
                "','" + beneficiario_normal[2] + "','" + beneficiario_normal[3] + "','" + beneficiario_normal[4] + "','"
                + beneficiario_normal[0] +
                "','" + beneficiario_normal[5] + "','" + beneficiario_normal[6] + "','" + beneficiario_normal[7] + "','"
                + beneficiario_normal[8] + "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') " + 
                " INSERT INTO Beneficiario VALUES ((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_normal[0] + "'),(SELECT ID_Afiliado FROm Afiliado,Persona WHERE Afiliado.ID_Persona = " + 
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_normal[9] + "'), 'TRUE' ,GETDATE() , GETDATE() , '" + 
                responsable + "' , '" + responsable + "')  INSERT INTO Beneficiario_Normal VALUES((SELECT MAX(Beneficiario.ID_Beneficiario) " +
                "FROM Beneficiario,Persona WHERE Beneficiario.ID_Persona = Persona.ID_Persona and Persona.No_Identidad = '" +
                beneficiario_normal[0] + "')," + beneficiario_normal[10] + "," + beneficiario_normal[11] + ", GETDATE() , GETDATE() , '"
                + responsable + "' , '" + responsable + "') ";
            }
            /*por ultimo asignamos las aportaciones obligatorias*/
            String QueryAfiliado = QueryString;
            QueryAfiliado += DataTransaction.AsignarAportacionesO(datos_personales[0],responsable);
            QueryString = QueryAfiliado + Log.Insercion(tempDK, "Afiliado");
            if( EjecutarComando() ) { FinalizarComandoCorrectamente();
            return true;
            } else return false;
        }
        public static Boolean InsertarOcupacion(String OcupacionDK)
        {
            String tempDK = OcupacionDK;
            OcupacionDK = traductor.DK_ocupacion(OcupacionDK);
            string[] Usuario_datos = OcupacionDK.Split('&');
            OcupacionDK = Usuario_datos[0];
            /*primero verificaremos que no halla otra ocupacion valido con el mismo nombre*/
            QueryString = "SELECT COUNT(*) FROM Ocupacion WHERE Ocupacion.Ocupacion = '" + OcupacionDK
                + "' AND Ocupacion.Valido = 'TRUE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        return false;
                    }
            FinalizarComandoCorrectamente();
            /*verificaremos si ya existe uno*/
            QueryString = "SELECT COUNT(*) FROM Ocupacion WHERE Ocupacion.Ocupacion = '" + OcupacionDK
                + "' AND Ocupacion.Valido = 'FALSE'";

            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        /*tenemos que actualizar el valor */
                        QueryString = " UPDATE Ocupacion  SET Valido = 'TRUE' , Fecha_Modificacion = GETDATE() ," +
                             "Usuario_Responsable = '" + Usuario_datos[1] + "' WHERE Ocupacion.Ocupacion = '" + OcupacionDK + "';";
                    }
                    else
                    {
                        FinalizarComandoCorrectamente();
                        /*ahora ya estamos seguros de poder insertar el valor*/
                        QueryString = "INSERT INTO Ocupacion VALUES( '" + OcupacionDK + "' , 'TRUE' , " +
                            " GETDATE() , GETDATE() , '" + Usuario_datos[1] + "' , '" + Usuario_datos[1] + "' );";
    
                    }
            DataTransaction.QueryString += Log.Insercion(tempDK, "Ocupacion");

            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean InsertarMotivo(String MotivoDK)
        {
            String tempDK = MotivoDK;
            MotivoDK = traductor.DK_Motivo(MotivoDK);
            string responsable = MotivoDK.Split('&')[1];
            MotivoDK = MotivoDK.Split('&')[0];
            /*primero verificaremos que no halla otro motivo valido con el mismo nombre*/
            QueryString = "SELECT COUNT(*) FROM Motivo WHERE Motivo.Motivo = '" + MotivoDK.Split('&')[0]
                + "' AND Motivo.Valido = 'TRUE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        return false;
                    }
            FinalizarComandoCorrectamente();
            /*verificaremos si ya existe uno falso*/
            QueryString = "SELECT COUNT(*) FROM Motivo WHERE Motivo.Motivo = '" + MotivoDK
                + "' AND Motivo.Valido = 'FALSE'";

            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        /*tenemos que actualizar el valor */
                        QueryString = "UPDATE Motivo SET Valido = 'TRUE' , Fecha_Modificacion = GETDATE() ," +
                             "Usuario_Responsable = '" + responsable + "' WHERE Motivo.Motivo = '" + MotivoDK + "';";
                    }
                    else
                    {
                        FinalizarComandoCorrectamente();
                        /*ahora ya estamos seguros de poder insertar el interes*/
                        QueryString = "INSERT INTO Motivo VALUES( '" + MotivoDK + "' , 'TRUE' , " +
                            " GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "' );";

                    }

            QueryString += Log.Insercion(tempDK, "Motivo");
            if( EjecutarComando() ) { 
                FinalizarComandoCorrectamente(); 
                return true; } else return false;
        }
        public static Boolean InsertarParentesco(String ParentescoDK)
        {
            String tempDK = ParentescoDK;
            ParentescoDK = traductor.DK_Parentesco(ParentescoDK);
            string responsable = ParentescoDK.Split('&')[1];
            ParentescoDK = ParentescoDK.Split('&')[0];
            /*primero verificaremos que no halla otrp parentesco valido con el mismo nombre*/
            QueryString = "SELECT COUNT(*) FROM Parentesco WHERE Parentesco.Parentesco = '" + ParentescoDK
                + "' AND Valido = 'TRUE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        return false;
                    }
            FinalizarComandoCorrectamente();
            /*verificaremos si ya existe uno*/
            QueryString = "SELECT COUNT(*) FROM Parentesco WHERE Parentesco.Parentesco = '" + ParentescoDK
                + "' AND Valido = 'FALSE'";

            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        /*tenemos que actualizar el valor */
                        QueryString = "UPDATE Parentesco SET Valido = 'TRUE' , Fecha_Modificacion = GETDATE() ," +
                             "Usuario_Responsable = '" + responsable + "' WHERE Parentesco.Parentesco = '" + ParentescoDK + "';";
                    }
                    else
                    {
                        FinalizarComandoCorrectamente();
                        /*ahora ya estamos seguros de poder insertar el valor*/
                        QueryString = "INSERT INTO Parentesco VALUES( '" + ParentescoDK + "' , 'TRUE' , " +
                            " GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "' );";

                    }

            QueryString += Log.Insercion(tempDK, "Parentesco");
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean InsertarNuevoMonto(String MontoDK)
        {
            String tempDK = MontoDK;
            string responsable = MontoDK.Split('&')[1];
            MontoDK = MontoDK.Split('&')[0];
            QueryString = "INSERT INTO Monto VALUES(" + MontoDK + ",GETDATE(),GETDATE(),'" + responsable + "','" 
                + responsable +"')";
            QueryString += Log.Insercion(tempDK, "Monto");
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean InsertarNuevoLimite(String LimiteDK)
        {
            String tempDK = LimiteDK;
            string responsable = LimiteDK.Split('&')[1];
            LimiteDK = LimiteDK.Split('&')[0];
            QueryString = "INSERT INTO Limite VALUES(" + LimiteDK + ",GETDATE(),GETDATE(),'" + responsable + "','"
                + responsable + "')";
            QueryString += Log.Insercion(tempDK, "Limite");
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean InsertarInteres(String InteresDK)
        {
            String tempDK = InteresDK;
            string responsable = InteresDK.Split('&')[1];
            InteresDK = InteresDK.Split('&')[0];
            /*Separamos los intereses segun el protocolo DK*/
            String[] datos_interes = InteresDK.Split(',');
            /*primero verificaremos que no halla otro interes valido con el mismo nombre, pues
             * la base de datos debe poder insertar varios con el mismo nombre pero solo 1 puede ser valido*/
            QueryString = "SELECT COUNT(*) FROM Interes WHERE Interes.Nombre = '" + datos_interes[0] 
                + "' AND Interes.Valido = 'TRUE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        return false; /*pues existe por lomenos uno que es valido con el mismo nombre*/
                    }
            FinalizarComandoCorrectamente();
            /*verificamos si existe uno invalido*/
            QueryString = "SELECT COUNT(*) FROM Interes WHERE Interes.Nombre = '" + datos_interes[0]
                + "' AND Interes.Valido = 'FALSE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        QueryString = " UPDATE Interes  SET Valido = 'TRUE' , Fecha_Modificacion = GETDATE() ," +
                             "Usuario_Responsable = '" + responsable + "' WHERE Interes.Nombre = '"
                             + datos_interes[0] + "' ";
                    }
                    else
                    {
                        /*ahora ya estamos seguros de poder insertar el interes*/
                        QueryString = "INSERT INTO Interes VALUES( '" + datos_interes[0] +
                            "', " + datos_interes[1] + " , 'TRUE' , '" + datos_interes[2] + "' , '" +
                            datos_interes[3] + "' , '" + datos_interes[4] + "' , '" + datos_interes[5] + "' , " + datos_interes[6] + " , " +
                            " GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') ";
                        QueryString += Log.Insercion(tempDK, "Interes");
                    }
            
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean SolicitarModificacionDePerfil(String Persona_DK)
        {
            String tempDK = Persona_DK;
            String num_Identidad = Persona_DK.Split('&')[2];
            QueryString = "SELECT COUNT(*) FROM Persona_Temp,Persona WHERE Persona.ID_Persona = Persona_Temp.ID_Persona " +
                          "AND Persona.No_Identidad = '" + num_Identidad.Split('~')[0] + "' AND Persona_Temp.Valido = 'TRUE'";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) != 0)
                    {
                        FinalizarComandoCorrectamente();
                        return false; /*retornamos pues existe por lomenos uno que es valido con el mismo nombre*/
                    }
            FinalizarComandoCorrectamente();
            /*ahora estamos listos para hacer una nueva modificación*/

            if (Persona_DK.Split('&').Length == 4)
                Persona_DK = Persona_DK.Split('&')[0] + '&' + Persona_DK.Split('&')[1] + '~' + Persona_DK.Split('&')[3];
            else
                Persona_DK = Persona_DK.Split('&')[0] + '&' + Persona_DK.Split('&')[1] + '~' + num_Identidad.Split('~')[1];

            QueryString = "INSERT INTO Persona_Temp VALUES((SELECT Persona.ID_Persona FROM " +
                "Persona WHERE No_Identidad = '" + num_Identidad.Split('~')[0] + "'),'TRUE' , '" + Persona_DK + "'); ";
            QueryString += Log.Insercion(tempDK, "Persona_Temp");
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }

        /*UPDATES A LA BASE DE DATOS*/
        public static Boolean ModificarPerfil(String correo_electronico)
        {
            String No_Identidad = "";
            QueryString = " IF ((SELECT COUNT(*) FROM Afiliado,Usuario WHERE Afiliado.ID_Usuario = Usuario.ID_Usuario " +
               "AND Usuario.Correo_Electronico = '" + correo_electronico  + 
               "') = 0) ( SELECT Persona.No_Identidad FROm Persona,Empleado," +
               "Usuario WHERE Persona.ID_Persona = Empleado.ID_Persona AND Empleado.ID_Usuario = Usuario.ID_Usuario " +
               "AND Usuario.Correo_Electronico = '" + correo_electronico  +
               "') ELSE ( SELECT Persona.No_Identidad FROm Persona,Afiliado," +
               "Usuario WHERE Persona.ID_Persona = Afiliado.ID_Persona AND Afiliado.ID_Usuario = Usuario.ID_Usuario " +
               "AND Usuario.Correo_Electronico = '" + correo_electronico  + "') ";
            if (EjecutarComando())
                if (DataReader.Read())
                    No_Identidad = DataReader.GetValue(0).ToString().Trim();
            if (No_Identidad == "")
                return false;
            String StringDK = getPersonaTemporal(No_Identidad);
            if (StringDK.CompareTo("") == 0)
                return false;
            int ID = Convert.ToInt32(StringDK.Split('&')[2]);
            StringDK = StringDK.Split('&')[0] + '&' + StringDK.Split('&')[1];

            String[] DK_Split = StringDK.Split('~');
            if (Generador.getTipoDK(StringDK) == 2)
                return ModificarPerfilEmpleado(DK_Split[0], ID, Convert.ToBoolean(DK_Split[1]));
            else
                return ModificarPerfilAfiliado(DK_Split[0], ID, DK_Split[1]);
          
       
        }
        public static Boolean AsignarAportacionesO()
        {
            String[] Ids = DataTransaction.getIDsAfiliadosActivos();
            Fecha fechaActual = DataTransaction.getFechaActual();
            String monto_actual = DataTransaction.getMontoActual(),
                id_monto = DataTransaction.getIDMontoActual();
            if (monto_actual.Length == 0 || id_monto.Length == 0 || Ids == null)
                return false;
            QueryString = "";
            for (int i = 0; i < Ids.Length; i++)
            {
                int Mes = fechaActual.Mes;
                while (Mes != 13)
                {
                    QueryString += " INSERT INTO Aportacion VALUES( " + Ids[i].ToString() + " , 'TRUE' , " + monto_actual + 
                        "  , 'Aportacion Obligatoria' , GETDATE() , GETDATE() ,'SCA Timer' , 'SCA Timer' , NULL ) " +
                        " INSERT INTO Aportacion_Obligatoria VALUES( (SELECT MAX(Aportacion.ID_Aportacion) FROM Aportacion)," +
                        " '" + fechaActual.Anio + '-' + Mes + '-' + getDiaFinal(Mes) + "' , " + id_monto + 
                        " , 'False' , GETDATE() ,GETDATE() , 'SCA Timer' , 'SCA Timer' )";
                    Mes++;
                }
            }
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static String AsignarAportacionesO(String num_Identidad,String Responsable)
        {
            String QueryReturn = "";
            Fecha fechaActual = DataTransaction.getFechaActual();
            String monto_actual = DataTransaction.getMontoActual(),
                id_monto = DataTransaction.getIDMontoActual();
            if (monto_actual.Length == 0 || id_monto.Length == 0 )
                return " SOLO ESTOY ARRUINANDO EL QUERY";
            QueryString = "";
     
                int Mes = fechaActual.Mes;
                while (Mes != 13)
                {
                    QueryReturn += " INSERT INTO Aportacion VALUES( (SELECT ID_Afiliado FROM Afiliado, Persona " +
                        " WHERE Persona.ID_Persona = Afiliado.ID_Persona AND Persona.No_Identidad = '" + num_Identidad +
                        "')  , 'TRUE' , " + monto_actual + "  , 'Aportacion Obligatoria' , GETDATE() , GETDATE() ," +
                        "'" + Responsable + "' , '" + Responsable + "' , NULL ) INSERT INTO Aportacion_Obligatoria " +
                        "VALUES( (SELECT MAX(Aportacion.ID_Aportacion) FROM Aportacion), '" + fechaActual.Anio + '-' + 
                        Mes.ToString() + '-' + getDiaFinal(Mes) + "' ," + id_monto + " , 'False' , GETDATE() ,GETDATE() ," +
                        " '" + Responsable + "' , '" + Responsable + "' ) ";
                    Mes++;
                }
                return QueryReturn;
          
        }
        public static Boolean AsignarAportacionOE(String AportacionOE_DK)
        {
            String tempDK = AportacionOE_DK,responsable = AportacionOE_DK.Split('&')[1];
            String[] datos = AportacionOE_DK.Split('&')[0].Split(','),
                     Ids = DataTransaction.getIDsAfiliadosActivos();
            if (Ids.Length == 0)
                return false;
            QueryString = "";
            for (int i = 0; i < Ids.Length; i++)
                QueryString += " INSERT INTO Aportacion VALUES( " + Ids[i].ToString() + " , 'TRUE' , " + datos[0] +
                        "  , '" + datos[4] + "' , GETDATE() , GETDATE() ,'" + responsable + "' , '" + responsable + "' , NULL ) " +
                        " INSERT INTO Aportacion_Obligatoria_Especial VALUES( (SELECT MAX(Aportacion.ID_Aportacion) FROM Aportacion)," +
                        " '" + datos[1] + "-" + datos[2] + "-" + getDiaFinal(Convert.ToInt32(datos[2])) +
                        "' , 'FALSE', (SELECT Motivo.ID_Motivo FROM Motivo WHERE Motivo.Motivo = '" +
                        datos[3] + "') , GETDATE() ,GETDATE() , '" + responsable + "' , '" + responsable + "' ) ";
            QueryString += Log.Insercion(tempDK, "Aportacion_Obligatoria_Especial");
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean ModificarOcupacion(String OcupacionMod_DK)
        {
            String tempDK = OcupacionMod_DK,
                   responsable = OcupacionMod_DK.Split('&')[1];
            OcupacionMod_DK = OcupacionMod_DK.Split('&')[0];
            OcupacionMod_DK = OcupacionMod_DK.Split(',')[0] + ',' + traductor.DK_ocupacion(OcupacionMod_DK.Split(',')[1]);
            QueryString = Generador.UpdateParametrizable(0,OcupacionMod_DK.Split(',')[0],
                OcupacionMod_DK.Split(',')[1],responsable);
            QueryString += Log.Modificacion(OcupacionMod_DK.Split(',')[1] + "&" + responsable,
                                            "Ocupacion", OcupacionMod_DK.Split(',')[0]);
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean ModificarParentesco(String ParentescoMod_DK)
        {
            String tempDK = ParentescoMod_DK,
                   responsable = ParentescoMod_DK.Split('&')[1];
            ParentescoMod_DK = ParentescoMod_DK.Split('&')[0];
            ParentescoMod_DK = ParentescoMod_DK.Split(',')[0] + ',' + traductor.DK_Parentesco(ParentescoMod_DK.Split(',')[1]);
            QueryString = Generador.UpdateParametrizable(1, ParentescoMod_DK.Split(',')[0],
                ParentescoMod_DK.Split(',')[1], responsable);
            QueryString += Log.Modificacion(ParentescoMod_DK.Split(',')[1] + "&" + responsable,
                                            "Parentesco", ParentescoMod_DK.Split(',')[0]);
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean ModificarMotivo(String MotivoMod_DK)
        {
            String tempDK = MotivoMod_DK,
                   responsable = MotivoMod_DK.Split('&')[1];
            MotivoMod_DK = MotivoMod_DK.Split('&')[0];
            MotivoMod_DK = MotivoMod_DK.Split(',')[0] + ',' + traductor.DK_Motivo(MotivoMod_DK.Split(',')[1]);
            QueryString = Generador.UpdateParametrizable(2, MotivoMod_DK.Split(',')[0],
                MotivoMod_DK.Split(',')[1], responsable);
            QueryString += Log.Modificacion(MotivoMod_DK.Split(',')[1] + "&" + responsable,
                                            "Motivo", MotivoMod_DK.Split(',')[0]);
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean ModificarInteres(String InteresMod_DK)
        {
            String tempDK = InteresMod_DK,responsable = InteresMod_DK.Split('&')[1];
            String[] datos = InteresMod_DK.Split('&')[0].Split(',');
            String DKAnterior = DataTransaction.getInteres(datos[0]);
            QueryString = "UPDATE Interes SET Nombre = '" + datos[1] + "', Taza = " + datos[2] + ", Aplica_Obligatoria = '" + 
                datos[3] + "', Aplica_Voluntaria = '" + datos[4] + "',Aplica_Obligatoria_Especial = '" + datos[5] +
                "', Aplica_Voluntaria_ArribaDe = '" + datos[6] + "', ArribaDe = " + datos[7] + " WHERE Nombre = '" + datos[0] 
                + "' ";
            String[] temp = tempDK.Split(',');
            tempDK = "";
            for (int i = 1; i < temp.Length; i++)
                tempDK += temp[i] + ",";
            tempDK = tempDK.Substring(0, tempDK.Length - 1);
            QueryString += Log.Modificacion(tempDK, "Interes", DKAnterior);
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean RechazarPerfil(String correo)
        {
            QueryString = "IF ((SELECT COUNT(*) FROM Afiliado,Usuario WHERE Afiliado.ID_Usuario = Usuario.ID_Usuario " +
               "AND Usuario.Correo_Electronico = '" + correo + "') != 0 ) UPDATE Persona_Temp SET Valido = 'FALSE'" +
               " WHERE Persona_Temp.ID_Persona = (SELECT Persona.ID_Persona FROM Persona,Afiliado,Usuario WHERE " +
               "Usuario.ID_Usuario = Afiliado.ID_Usuario AND Persona.ID_Persona = Afiliado.ID_Persona AND " +
               "Usuario.Correo_Electronico = '" + correo + "') ELSE UPDATE Persona_Temp SET Valido = 'FALSE' " +
               "WHERE Persona_Temp.ID_Persona = (SELECT Persona.ID_Persona FROM Persona,Empleado,Usuario WHERE " +
               "Usuario.ID_Usuario = Empleado.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona AND " +
               "Usuario.Correo_Electronico = '" + correo + "') ";
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
                return false;
        }
        public static String getPerfilNuevo(String Correo)
        {
            String[] datos = DataTransaction.getIDAfiliadoOEmpleado(Correo);
            QueryString = " SELECT String_DK FROM Persona_Temp WHERE Persona_Temp.ID_Persona = "
                + datos[1] + " AND Valido = 'TRUE' ";
            String stringdk = "";
            if (EjecutarComando())
                if (DataReader.Read())
                    stringdk = DataReader.GetValue(0).ToString().Trim();
            if (stringdk == "")
                return "";
            stringdk = stringdk.Split('&')[0] + "~" + stringdk.Split('&')[1].Split('~')[1]; 
            switch (datos[0])
            {
                case "Empleado":
                    return datos[0] + "&" + stringdk;
                case "Afiliado":
                    return datos[0] + "&0," + stringdk;
                default:
                    return "";
            }

        }

        /*METODOS PRIVADOS DEL DT*/
        public static String[] getIDAfiliadoOEmpleado(String Correo)
        {
            String[] datos = new String[2];
            QueryString = "IF(( SELECT COUNT(*) FROM Empleado,Usuario WHERE Usuario.ID_Usuario = Empleado.ID_Usuario" +
                " AND Usuario.Correo_Electronico = '" + Correo + "' ) = 0) SELECT 'Afiliado' AS Tipo,Persona.ID_Persona" +
                " FROM Persona,Afiliado,Usuario WHERE Persona.ID_Persona = Afiliado.ID_Persona AND Afiliado.ID_Usuario = " +
                "Usuario.ID_Usuario AND Usuario.Correo_Electronico = '" + Correo + "' ELSE SELECT 'Empleado' " +
                "AS Tipo,Persona.ID_Persona FROM Persona,Empleado,Usuario WHERE Persona.ID_Persona = Empleado.ID_Persona " +
                "AND Empleado.ID_Usuario = Usuario.ID_Usuario AND Usuario.Correo_Electronico = '" + Correo + "' ";
            if( EjecutarComando() )
                if (DataReader.Read())
                {
                    datos[0] = DataReader.GetValue(0).ToString().Trim();
                    datos[1] = DataReader.GetValue(1).ToString().Trim();
                    return datos;
                }
            return null;
        }
        public static Boolean ModificarPerfilAfiliado(String Afiliado_DK, int ID , String Estado)
        {
            /*arreglamos el DK antes empezar a modificar*/
            Afiliado_DK = traductor.DK_Afiliado(Afiliado_DK);
            /*obtenemos al usuario responsable de la modificacion... deberia ser siempre el admin*/
            string responsable = Afiliado_DK.Split('&')[1];
            /*obtenemos el resto de los datos sin el responsable*/
            Afiliado_DK = Afiliado_DK.Split('&')[0];
            string[] datos = Afiliado_DK.Split(';');
            /*empezamos modificando los datos personales*/
            string[] datos_personales = datos[0].Split(',');
            QueryString = "UPDATE Persona SET No_Identidad = '"+datos_personales[0]+"', P_Nombre = '"+ datos_personales[1]
                +"',S_Nombre = '" + datos_personales[2] +"',P_Apellido = '" + datos_personales[3]+ "'," +
                 "S_Apellido = '"+ datos_personales[4] +"',Fecha_Nacimiento = '" +datos_personales[5] +
                 "',Estado_Civil = '"+ datos_personales[6] +"',Genero = '" + datos_personales[7] +
                 "',Direccion = '" + datos_personales[8] +"', Fecha_Modificacion = GETDATE(), Usuario_Responsable = '"  + 
                 responsable + "' WHERE Persona.ID_Persona = " + ID ;
            /*eliminemos primero todos los telefonos y celulares*/
            QueryString += " DELETE FROM Telefono_Personal WHERE Id_Persona = " + ID +
                         " DELETE FROM Telefono_Personal WHERE Id_Persona = " + ID;
            /*telefonos*/
            string[] telefonos = datos[1].Split(',');
            for (int i = 0; i < telefonos.Length; i++)
                QueryString += " INSERT INTO Telefono_Personal VALUES" +
                        "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] +
                        "'),'" + telefonos[i] + "' )";
            /*celulares*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " INSERT INTO Celular VALUES" +
                      "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "')" +
                      ",'" + celulares[i] + "' )";
            /*modificamos el usuario*/
            string correo = datos[3].Split(',')[0], pass = datos[3].Split(',')[1];
            QueryString += "UPDATE Usuario SET Correo_Electronico = '" + correo + "', Pass = '" + pass + "', Fecha_Modificacion = GETDATE(),"
	               + "Usuario_Responsable = '" + responsable + "' WHERE ID_Usuario = " + 
                   DataTransaction.getIDUsuarioAfiliado( datos_personales[0] );
            string[] datos_laborales = datos[4].Split(',');
            /*ahora modificamos el imbecil afiliado*/
            QueryString += " UPDATE Afiliado SET ID_Ocupacion = (SELECT ID_Ocupacion FROM Ocupacion WHERE Ocupacion.Ocupacion" 
                + " = '" + datos[5] + "'), Empresa_Nombre = '" + datos_laborales[0] + "',Empresa_Telefono = '" + datos_laborales[1] +
                "',Empresa_Direccion = '" + datos_laborales[2] + "', " + "Empresa_Tiempo_Laboracion = '" + datos_laborales[3] + 
                "',Empresa_Dpto = '" + datos_laborales[4] + "',Fecha_Modificacion = GETDATE(),"
                + " Usuario_Responsable = '" + responsable + "' , Estado = '" + Estado + 
                "'  WHERE Afiliado.ID_Persona = " + ID;
            /*borramos todos los beneficiarios*/
            QueryString += " UPDATE Beneficiario SET Valido = 'False' ,"  +
              "Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable + "' WHERE Beneficiario.ID_Afiliado" + 
                  " = (SELECT ID_Afiliado FROM Afiliado WHERE Afiliado.ID_Persona = " + ID + ")";
            /*ahora a lo caretudo metemos los beneficiarios nuevamente*/
            /*beneficiario de contingencia*/
            string[] beneficiario_contingencia = datos[6].Split(',');
            QueryString += " IF (SELECT COUNT(*) FROM Persona WHERE No_Identidad = '" + beneficiario_contingencia[0] + "' ) = 0 " +
                "INSERT INTO Persona VALUES('" + beneficiario_contingencia[1] +
                "','" + beneficiario_contingencia[2] + "','" + beneficiario_contingencia[3] + "','" + beneficiario_contingencia[4]
                + "','" + beneficiario_contingencia[0] + "','" + beneficiario_contingencia[5] + "','" +
                beneficiario_contingencia[6] + "','" + beneficiario_contingencia[7] + "','" + beneficiario_contingencia[8] +
                "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')  INSERT INTO Beneficiario VALUES" +
                " ((SELECT Persona.ID_Persona FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_contingencia[0] + "'),(SELECT ID_Afiliado FROM Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_contingencia[9] + "'), 'TRUE' ,GETDATE() , GETDATE() , '" +
                responsable + "' , '" + responsable + "') INSERT INTO Beneficiario_Contingencia VALUES((SELECT MAX(Beneficiario.ID_Beneficiario)" +
                " FROM Beneficiario,Persona WHERE Beneficiario.ID_Persona = Persona.ID_Persona and Persona.No_Identidad = '"
                + beneficiario_contingencia[0] + "'),'TRUE', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "')";
            /*beneficiarios normales*/
            for (int i = 7; i < datos.Length; i++)/*Aqui empieza desde 7 porque despues de septimo ; empiezan los Beneficiarios normales*/
            {
                string[] beneficiario_normal = datos[i].Split(',');
                QueryString += " IF (SELECT COUNT(*) FROM Persona WHERE No_Identidad = '" + beneficiario_normal[0] + "' ) = 0 " +
                    " INSERT INTO Persona VALUES('" + beneficiario_normal[1] +
                "','" + beneficiario_normal[2] + "','" + beneficiario_normal[3] + "','" + beneficiario_normal[4] + "','"
                + beneficiario_normal[0] +
                "','" + beneficiario_normal[5] + "','" + beneficiario_normal[6] + "','" + beneficiario_normal[7] + "','"
                + beneficiario_normal[8] + "', GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "') " +
                " INSERT INTO Beneficiario VALUES ((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" +
                beneficiario_normal[0] + "'),(SELECT ID_Afiliado FROm Afiliado,Persona WHERE Afiliado.ID_Persona = " +
                "Persona.ID_Persona and Persona.No_Identidad = '" + datos_personales[0] + "'),(SELECT Parentesco.ID_Parentesco" +
                " FROM Parentesco WHERE Parentesco.Parentesco = '" + beneficiario_normal[9] + "'), 'TRUE' ,GETDATE() , GETDATE() , '" +
                responsable + "' , '" + responsable + "')  INSERT INTO Beneficiario_Normal VALUES((SELECT MAX(Beneficiario.ID_Beneficiario) " +
                "FROM Beneficiario,Persona WHERE Beneficiario.ID_Persona = Persona.ID_Persona and Persona.No_Identidad = '" +
                beneficiario_normal[0] + "')," + beneficiario_normal[10] + "," + beneficiario_normal[11] + ", GETDATE() , GETDATE() , '"
                + responsable + "' , '" + responsable + "') ";
            }

            /*eliminamos el temporal*/
            QueryString += " UPDATE Persona_Temp SET Valido = 'False' WHERE Persona_Temp.ID_Persona = " + ID;
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; }
            else
            {
                QueryString = " UPDATE Persona_Temp SET Valido = 'False' WHERE Persona_Temp.ID_Persona = " + ID;
                EjecutarComando();
                FinalizarComandoCorrectamente();
                return false;
            }
        }
        public static Boolean ModificarPerfilEmpleado(String Empleado_DK, int ID, Boolean EsAdmin)
        {
            /*arreglamos el DK antes empezar a modificar*/
            Empleado_DK = traductor.DK_Empleado(Empleado_DK);
            /*obtenemos al usuario responsable de la modificacion... deberia ser siempre el admin*/
            string responsable = Empleado_DK.Split('&')[1];

            /**Modificando ROL**/
            QueryString = "DELETE FROM Desempenia WHERE ID_Usuario = (SELECT Usuario.ID_Usuario FROM Usuario,Persona," +
                "Empleado WHERE Usuario.ID_Usuario = Empleado.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona " +
                " AND Persona.ID_Persona = " + ID.ToString() + " ) IF( '" + EsAdmin.ToString() + "' = 'TRUE' ) INSERT INTO Desempenia " +
                "VALUES( (SELECT Usuario.ID_Usuario FROM Usuario,Persona,Empleado WHERE Usuario.ID_Usuario = " +
                "Empleado.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona AND Persona.ID_Persona = " + ID.ToString() +
                " ) , (SELECT ID_Rol FROM Rol WHERE Rol.Nombre = 'Administrador') , GETDATE() , GETDATE() , '" + responsable
                + "' , '" + responsable + "' ) " +
                " ELSE INSERT INTO Desempenia VALUES( (SELECT Usuario.ID_Usuario FROM Usuario,Persona,Empleado " +
                " WHERE Usuario.ID_Usuario = Empleado.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona " +
                " AND Persona.ID_Persona = " + ID.ToString() + " ) , (SELECT ID_Rol FROM Rol WHERE Rol.Nombre = 'Empleado') ," +
                " GETDATE() , GETDATE() , '" + responsable + "' , '" + responsable + "' ) ";
            /**Modificando ROL**/

            if (EjecutarComando())
                FinalizarComandoCorrectamente();
            
            /*obtenemos el resto de los datos sin el responsable*/
            Empleado_DK = Empleado_DK.Split('&')[0];
            string[] datos = Empleado_DK.Split(';');
            /*empezamos modificando los datos personales*/
            string[] datos_personales = datos[0].Split(',');
            QueryString = "UPDATE Persona SET No_Identidad = '" + datos_personales[0] + "', P_Nombre = '" + datos_personales[1]
                + "',S_Nombre = '" + datos_personales[2] + "',P_Apellido = '" + datos_personales[3] + "'," +
                 "S_Apellido = '" + datos_personales[4] + "',Fecha_Nacimiento = '" + datos_personales[5] +
                 "',Estado_Civil = '" + datos_personales[6] + "',Genero = '" + datos_personales[7] +
                 "',Direccion = '" + datos_personales[8] + "', Fecha_Modificacion = GETDATE(), Usuario_Responsable = '" +
                 responsable + "' WHERE Persona.ID_Persona = " + ID;
            /*eliminemos primero todos los telefonos y celulares*/
            QueryString += " DELETE FROM Telefono_Personal WHERE Id_Persona = " + ID +
                         " DELETE FROM Telefono_Personal WHERE Id_Persona = " + ID;
            /*telefonos*/
            string[] telefonos = datos[1].Split(',');
            for (int i = 0; i < telefonos.Length; i++)
                QueryString += " INSERT INTO Telefono_Personal VALUES" +
                        "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] +
                        "'),'" + telefonos[i] + "' )";
            /*celulares*/
            string[] celulares = datos[2].Split(',');
            for (int i = 0; i < celulares.Length; i++)
                QueryString += " INSERT INTO Celular VALUES" +
                      "((SELECT [Persona].[ID_Persona] FROM Persona WHERE Persona.No_Identidad = '" + datos_personales[0] + "')" +
                      ",'" + celulares[i] + "' )";
            /*modificamos el usuario*/
            string correo = datos[3].Split(',')[0], pass = datos[3].Split(',')[1], puesto = datos[3].Split(',')[2];
            QueryString += "UPDATE Usuario SET Correo_Electronico = '" + correo + "', Pass = '" + pass + "', Fecha_Modificacion = GETDATE(),"
                   + "Usuario_Responsable = '" + responsable + "' WHERE ID_Usuario = " + DataTransaction.getIDUsuarioEmpleado(datos_personales[0]);
            /*modificamos al empleado*/
            QueryString += "UPDATE Empleado SET Puesto = '" + puesto + "',Fecha_Modificacion = GETDATE()," +
                        "Usuario_Responsable = '" + responsable + "'WHERE ID_Persona = " + ID;
            /*eliminamos el temporal*/
            QueryString += " UPDATE Persona_Temp SET Valido = 'False' WHERE Persona_Temp.ID_Persona = " + ID;
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static int getIDPersona(string num_identidad)
        {
            QueryString = "SELECT Persona.ID_Persona FROM Persona WHERE Persona.No_Identidad = '" + num_identidad + "' ";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    int res = Convert.ToInt32(DataReader.GetValue(0));
                    DataTransaction.FinalizarComandoCorrectamente();
                    return res;
                }
            return -1;
        }
        public static int getIDUsuarioAfiliado(string num_identidad)
        {
            QueryString = "SELECT Usuario.ID_Usuario FROM Usuario,Persona,Afiliado WHERE Afiliado.ID_Usuario = " +
                " Usuario.ID_Usuario AND Persona.ID_Persona = Afiliado.ID_Persona AND Persona.No_Identidad = '" + num_identidad +
                "' ";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    int id = Convert.ToInt32(DataReader.GetValue(0));
                    FinalizarComandoCorrectamente();
                    return id;
                }
            return -1;
        }
        public static int getIDUsuarioEmpleado(string num_identidad)
        {
            QueryString = "SELECT Usuario.ID_Usuario FROM Usuario,Persona,Empleado WHERE Empleado.ID_Usuario = " +
                " Usuario.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona AND Persona.No_Identidad = '" + num_identidad +
                "' ";
            if (EjecutarComando())
                while (DataReader.Read())
                    return Convert.ToInt32(DataReader.GetValue(0));
            return -1;
        }
        public static String getPersonaTemporal(String Num_Identidad)
        {
            QueryString = "SELECT String_DK,Persona_Temp.ID_Persona FROM Persona_Temp,Persona WHERE Persona.No_Identidad = '"
                + Num_Identidad + "' AND Persona_Temp.ID_Persona = Persona.ID_Persona AND Persona_Temp.Valido = 'TRUE'";
            String Persona_DK = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    if (DataReader.HasRows)
                        Persona_DK = DataReader.GetValue(0).ToString() + '&' + DataReader.GetValue(1).ToString();
            return Persona_DK;
        }
        public static String getRol(String correo)
        {
            QueryString = "SELECT Rol.Nombre FROm Rol,Desempenia,Usuario WHERE Rol.ID_Rol = Desempenia.ID_Rol AND " +
                "Desempenia.ID_Usuario = Usuario.ID_Usuario AND Usuario.Correo_Electronico = '" + correo + "'";
            if (EjecutarComando())
                if (DataReader.Read())
                    return DataReader.GetValue(0).ToString().Trim();
            return "";
        }
        public static String getPermisos(String correo)
        {
            QueryString = "SELECT Permiso.Nombre FROM Usuario,Rol,Permiso,Tiene,Desempenia WHERE Usuario.ID_Usuario = "
               + " Desempenia.ID_Usuario AND Desempenia.ID_Rol = Rol.ID_Rol AND Rol.ID_Rol = Tiene.ID_Rol AND "
               + " Tiene.ID_Permiso = Permiso.ID_Permiso AND Usuario.Correo_Electronico = '" + correo + "'";
            string result = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        result += DataReader.GetValue(i).ToString().Trim() + ',';
            if (result.Length > 0)
                return result.Substring(0, result.Length - 1);
            return result;
        }
        public static String[] getIDsAfiliadosActivos()
        {
            string resultado = "";
            QueryString = "SELECT Afiliado.ID_Afiliado FROM Afiliado WHERE Afiliado.Estado = 'Activo'";
            if (EjecutarComando())
                while (DataReader.Read())
                    resultado += DataReader.GetValue(0) + ",";
            if (resultado.CompareTo("") == 0)
                return null;
            return resultado.Substring(0, resultado.Length - 1).Split(',');
            
        }
        public static String[] getNumCertificadosAfiliadosActivos()
        {
            string resultado = "";
            QueryString = "SELECT Afiliado.Num_Certificado FROM Afiliado WHERE Afiliado.Estado = 'Activo'";
            if (EjecutarComando())
                while (DataReader.Read())
                    resultado += DataReader.GetValue(0) + ",";
            if (resultado.CompareTo("") == 0)
                return null;
            return resultado.Substring(0, resultado.Length - 1).Split(',');

        }
        public static Fecha getFechaActual()
        {
            QueryString = "SELECT GETDATE()";
            if (EjecutarComando())
                while (DataReader.Read())
                    return new Fecha(DataReader.GetValue(0).ToString());
            return null;
        }
        public static String getIDMontoActual()
        {
            QueryString = " SELECT MAX(ID_Monto) FROM Monto ";
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            if (monto.Length == 0)
                return "";
            return monto.Substring(0, monto.Length - 1);
             
        }
        public static String getIDAfiliadoBusqueda(String ControlPagoDK)
        {
            string[] datos = ControlPagoDK.Split(';');
            string id = "";
            /*numero de identidad*/
            if (datos[2].CompareTo("") != 0)
            {
                QueryString = "SELECT Afiliado.ID_Afiliado FROM Afiliado,Persona WHERE Persona.ID_Persona = " +
                              "Afiliado.ID_Persona AND Persona.No_Identidad = '" + datos[2] + "'";
            }
            else
            {
                if (datos[0].CompareTo("") != 0)
                {
                    QueryString = "SELECT Afiliado.ID_Afiliado FROM Afiliado,Cuenta WHERE Afiliado.Num_Certificado = " +
                            "Cuenta.Num_Certificado AND Cuenta.Num_Certificado = " + datos[0];
                }
                else
                {
                    string[] nombres = datos[1].Split(',');
                    QueryString = "SELECT ID_Afiliado FROM Persona,Afiliado WHERE Afiliado.ID_Persona = Persona.ID_Persona ";
                    if( nombres[0].CompareTo("") != 0 )
                        QueryString += " AND P_Nombre = '" + nombres[0] + "' ";
                    if( nombres[1].CompareTo("") != 0 )
                        QueryString += " AND S_Nombre = '" + nombres[1] + "' ";
                    if( nombres[2].CompareTo("") != 0 )
                        QueryString += " AND P_Apellido = '" + nombres[2] + "' ";
                    if( nombres[3].CompareTo("") != 0 )
                        QueryString += " AND S_Apellido = '" + nombres[3] + "' ";
                }
            }

            if (EjecutarComando())
                while (DataReader.Read())
                    id += DataReader.GetValue(0).ToString().Trim() + ",";
            if (id.Length == 0)
                return "";
            return id.Substring(0, id.Length - 1);
        }
        public static String getIDAfiliado(String num_identidad)
        {
            QueryString = "SELECT Afiliado.ID_Afiliado FROM Afiliado,Persona WHERE Persona.ID_Persona = " + 
                "Afiliado.ID_Persona AND Persona.No_Identidad = '" + num_identidad + "'";
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            DataTransaction.FinalizarComandoCorrectamente();
            if (monto.Length == 0)
                return "";
            return monto.Substring(0, monto.Length - 1);
        }
        public static String getEstadoAfiliado(String Correo)
        {
            QueryString = "SELECT Estado FROm Afiliado,Usuario WHERE Afiliado.ID_Usuario = Usuario.ID_Usuario " +
                 "AND Usuario.Correo_Electronico = '" + Correo + "'";
            if (EjecutarComando())
                if (DataReader.Read())
                {
                    String datos = DataReader.GetValue(0).ToString().Trim();
                    FinalizarComandoCorrectamente();
                    return datos;
                }
            return "";
        }
        public static String getNumCertificadoAfiliado(String ID_Afiliado)
        {
            QueryString = "SELECT Afiliado.Num_Certificado FROM Afiliado WHERE Afiliado.ID_Afiliado = " + ID_Afiliado;
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            DataTransaction.FinalizarComandoCorrectamente();
            if (monto.Length == 0)
                return "";
            return monto.Substring(0, monto.Length - 1);
        }
        public static String getIDLimitectual()
        {
            QueryString = " SELECT MAX(ID_Limite) FROM Limite ";
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            DataTransaction.FinalizarComandoCorrectamente();
            if (monto.Length == 0)
                return "";
            return monto.Substring(0, monto.Length - 1);
        }
        public static String getMontoObligatoria(String ID_Aportacion)
        {
            QueryString = "SELECT Aportacion.Monto FROM Aportacion WHERE Aportacion.ID_Aportacion =  " + ID_Aportacion;
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            DataTransaction.FinalizarComandoCorrectamente();
            if (monto.Length == 0)
                return "";
            return monto.Substring(0, monto.Length - 1);
        }
        public static String PagarObligatorias(String ID_Afiliado, String[] ID_Aportaciones, String responsable)
        {
            String QueryAportaciones = "",
                   numCertificado = DataTransaction.getNumCertificadoAfiliado(ID_Afiliado);
            for (int i = 0; i < ID_Aportaciones.Length; i++)
            {
                String ID = ID_Aportaciones[i];
                String MontoAportacion = DataTransaction.getMontoObligatoria(ID);
                switch (DataTransaction.getTipoAportacion(ID))
                {
                    case 1:
                        /*normal*/
                        QueryAportaciones += " UPDATE Cuenta SET Saldo = (Cuenta.Saldo + " + MontoAportacion + "), " +
                                    " Fecha_Modificacion = GETDATE(), Usuario_Responsable = '" + responsable + "' "+
                                    " WHERE Cuenta.Num_Certificado =  " + numCertificado + " UPDATE Aportacion_Obligatoria" +
                                    " SET Cancelado = 'TRUE', Fecha_Modificacion = GETDATE(),Usuario_Responsable = '" +
                                    responsable + "' WHERE ID_Aportacion = " + ID + " UPDATE Aportacion SET Fecha_Realizacion" +
                                    " = GETDATE() , Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable +
                                    "' WHERE ID_Aportacion = " + ID + " ";
                        break;

                    case 2:
                        /*especial*/
                        QueryAportaciones += " UPDATE Cuenta SET Saldo = (Cuenta.Saldo + " + MontoAportacion + "), " +
                                    " Fecha_Modificacion = GETDATE(), Usuario_Responsable = '" + responsable + "' " +
                                    " WHERE Cuenta.Num_Certificado =  " + numCertificado + " UPDATE Aportacion_Obligatoria_Especial" +
                                    " SET Cancelado = 'TRUE', Fecha_Modificacion = GETDATE(),Usuario_Responsable = '" +
                                    responsable + "' WHERE ID_Aportacion = " + ID + " UPDATE Aportacion SET Fecha_Realizacion" +
                                    " = GETDATE() , Fecha_Modificacion = GETDATE() , Usuario_Responsable = '" + responsable + 
                                    "' WHERE ID_Aportacion = " + ID + " ";
                        break;
                    default:
                            QueryString += " ESTO SOLO ES PARA JODER EL QUERY Y QUE NO SE EJECUTE ";
                        break;
                }
            }
            return QueryAportaciones;
        }
        public static int getTipoAportacion(String ID_Aportacion)
        {
            QueryString = "SELECT COUNT(*) FROM Aportacion,Aportacion_Obligatoria WHERE Aportacion.ID_Aportacion" +
                    "= Aportacion_Obligatoria.ID_Aportacion AND Aportacion.ID_Aportacion = " + ID_Aportacion;
            if (EjecutarComando())
                while (DataReader.Read())
                    if (Convert.ToInt32(DataReader.GetValue(0).ToString()) != 0)
                        return 1;
            DataTransaction.FinalizarComandoCorrectamente();
            QueryString = "SELECT COUNT(*) FROM Aportacion,Aportacion_Obligatoria_Especial WHERE Aportacion.ID_Aportacion" +
                    "= Aportacion_Obligatoria_Especial.ID_Aportacion AND Aportacion.ID_Aportacion = " + ID_Aportacion;
            if (EjecutarComando())
                while (DataReader.Read())
                    if (Convert.ToInt32(DataReader.GetValue(0).ToString()) != 0)
                        return 2;
            DataTransaction.FinalizarComandoCorrectamente();
            return 0;

        }
        public static String getMontoAportacionesObligatoriasAfiliado(String numCertificado)
        {
            String result = "";
            QueryString = "SELECT Monto.Monto FROM Aportacion,Aportacion_Obligatoria,Afiliado,Monto " +
                "WHERE Aportacion.ID_Aportacion = Aportacion_Obligatoria.ID_Aportacion AND Aportacion_Obligatoria.Cancelado " +
                "= 'TRUE' AND Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND Afiliado.Num_Certificado = " + numCertificado +
                " AND ( Aportacion.Fecha_Realizacion <= GETDATE() AND Aportacion.Fecha_Realizacion > " +
                "(SELECT MAX(Fecha) FROM LOG WHERE Actividad = 'CAPITALIZACION')) AND  Aportacion_Obligatoria.ID_Monto = " +
                "Monto.ID_Monto";
            if (EjecutarComando())
                while (DataReader.Read())
                    result += DataReader.GetValue(0).ToString() + ",";
            FinalizarComandoCorrectamente();
            if (result.Length == 0)
                return "0";
            return result.Substring(0, result.Length - 1);

        }
        public static String getMontoAportacionesObligatoriasEspecialesAfiliado(String numCertificado)
        {
            String result = "";
            QueryString = "SELECT Aportacion.Monto FROM Aportacion,Aportacion_Obligatoria_Especial,Afiliado " +
                "WHERE Aportacion.ID_Aportacion = Aportacion_Obligatoria_Especial.ID_Aportacion AND " +
                "Aportacion_Obligatoria_Especial.Cancelado = 'TRUE' AND Afiliado.ID_Afiliado = Aportacion.ID_Afiliado" +
                " AND Afiliado.Num_Certificado = " + numCertificado + " AND ( Aportacion.Fecha_Realizacion " +
                "<= GETDATE() AND Aportacion.Fecha_Realizacion > (SELECT MAX(Fecha) FROM LOG WHERE " +
                "Actividad = 'CAPITALIZACION'))";
            if (EjecutarComando())
                while (DataReader.Read())
                    result += DataReader.GetValue(0).ToString() + ",";
            FinalizarComandoCorrectamente();
            if (result.Length == 0)
                return "0";
            return result.Substring(0, result.Length - 1);

        }
        public static String getMontoVoluntariaAfiliado(String numCertificado)
        {
            String result = "";
            QueryString = "SELECT Aportacion.Monto FROM Aportacion,Aportacion_Voluntaria,Afiliado WHERE Aportacion" +
                ".ID_Aportacion = Aportacion_Voluntaria.ID_Aportacion AND Aportacion_Voluntaria.Aceptaba = 'TRUE' " +
                "AND Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND Afiliado.Num_Certificado = " + numCertificado +
                " AND ( Aportacion.Fecha_Realizacion " +
                "<= GETDATE() AND Aportacion.Fecha_Realizacion > (SELECT MAX(Fecha) FROM LOG WHERE " +
                "Actividad = 'CAPITALIZACION'))";
            if (EjecutarComando())
                while (DataReader.Read())
                    result += DataReader.GetValue(0).ToString() + ",";
            FinalizarComandoCorrectamente();
            if (result.Length == 0)
                return "0";
            return result.Substring(0, result.Length - 1);
        }
        public static List<String[]> getAportacionesObligatoriasACapitalizar()
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Monto.Monto," +
	            "Aportacion_Obligatoria.Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo " +
                " FROM Afiliado,Aportacion,Aportacion_Obligatoria,Monto WHERE Afiliado.ID_Afiliado = " +
                "Aportacion.ID_Afiliado AND Aportacion_Obligatoria.Cancelado = 'TRUE' AND Aportacion.ID_Aportacion "  +
                "= Aportacion_Obligatoria.ID_Aportacion AND ( Aportacion.Fecha_Realizacion <= GETDATE() AND " +
                "Aportacion.Fecha_Realizacion > (SELECT MAX(Fecha) FROM LOG WHERE Actividad = 'CAPITALIZACION')) " +
                "AND Monto.ID_Monto = Aportacion_Obligatoria.ID_Monto";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getAportacionesObligatoriasACapitalizar(String Desde,String Hasta)
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Monto.Monto," +
                "Aportacion_Obligatoria.Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo " +
                " FROM Afiliado,Aportacion,Aportacion_Obligatoria,Monto WHERE Afiliado.ID_Afiliado = " +
                "Aportacion.ID_Afiliado AND Aportacion_Obligatoria.Cancelado = 'TRUE' AND Aportacion.ID_Aportacion " +
                "= Aportacion_Obligatoria.ID_Aportacion AND ( Aportacion.Fecha_Realizacion <= '" + Hasta + "' AND " +
                "Aportacion.Fecha_Realizacion >= '" + Desde + "' ) AND Monto.ID_Monto = Aportacion_Obligatoria.ID_Monto";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getAportacionesObligatoriasEspecialesACapitalizar()
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Aportacion.Monto, " +
                    " Aportacion_Obligatoria_Especial.Fecha_Plazo,Aportacion.Fecha_Realizacion, " +
                    " Motivo.Motivo FROM Afiliado,Aportacion,Aportacion_Obligatoria_Especial,Motivo " +
                    " WHERE Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND " +
                    " Aportacion_Obligatoria_Especial.ID_Motivo = Motivo.ID_Motivo AND " +
                    " Aportacion_Obligatoria_Especial.Cancelado = 'TRUE' AND " +
                    " Aportacion.ID_Aportacion = Aportacion_Obligatoria_Especial.ID_Aportacion AND " +
                    " (Aportacion.Fecha_Realizacion <= GETDATE() AND Aportacion.Fecha_Realizacion > " +
                    " (SELECT MAX(Fecha)FROM LOG WHERE Actividad = 'CAPITALIZACION')) ";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getAportacionesObligatoriasEspecialesACapitalizar(String Desde, String Hasta)
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Monto.Monto," +
                      "Aportacion_Obligatoria_Especial.Fecha_Plazo,Aportacion.Fecha_Realizacion,Motivo.Motivo " +
                      "FROM Afiliado,Aportacion,Aportacion_Obligatoria_Especial,Motivo,Monto " +
                      "WHERE Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND Aportacion_Obligatoria_Especial.ID_Motivo " +
                      "= Motivo.ID_Motivo AND Aportacion_Obligatoria_Especial.Cancelado = 'TRUE' AND " +
                      "Aportacion.ID_Aportacion = Aportacion_Obligatoria_Especial.ID_Aportacion AND ( " +
                      "Aportacion.Fecha_Realizacion <= '" + Hasta + "' AND Monto.ID_Monto = Monto.ID_Monto AND" +
                      " Aportacion.Fecha_Realizacion > '" + Desde + "')";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getAportacionesVoluntariaACapitalizar()
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Aportacion.Monto," +
                    "'N/A' AS Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo FROM Afiliado," +
                    "Aportacion,Aportacion_Voluntaria WHERE Afiliado.ID_Afiliado = Aportacion.ID_Afiliado" +
                    " AND Aportacion_Voluntaria.Aceptaba = 'TRUE' AND Aportacion.ID_Aportacion = " +
                    "Aportacion_Voluntaria.ID_Aportacion AND ( Aportacion.Fecha_Realizacion <= GETDATE() AND " +
                    "Aportacion.Fecha_Realizacion > (SELECT MAX(Fecha) FROM LOG WHERE Actividad = 'CAPITALIZACION'))";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getAportacionesVoluntariaACapitalizar(String Desde,String Hasta)
        {
            List<String[]> Consulta = new List<String[]>();
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Descripcion,Aportacion.Monto," +
                    "'N/A' AS Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo FROM Afiliado," +
                    "Aportacion,Aportacion_Voluntaria WHERE Afiliado.ID_Afiliado = Aportacion.ID_Afiliado" +
                    " AND Aportacion_Voluntaria.Aceptaba = 'TRUE' AND Aportacion.ID_Aportacion = " +
                    "Aportacion_Voluntaria.ID_Aportacion AND ( Aportacion.Fecha_Realizacion <= '" + Hasta + "' AND " +
                    "Aportacion.Fecha_Realizacion >= '" + Desde + "')";
            if (!EjecutarComando())
                return null;
            while (DataReader.Read())
            {
                String[] temp = new String[6];
                for (int i = 0; i < temp.Length; i++)
                    temp[i] = DataReader.GetValue(i).ToString().Trim();
                Consulta.Add(temp);
            }
            return Consulta;
        }
        public static List<String[]> getSaldosAfiliadosActivos()
        {
            QueryString = "SELECT Cuenta.Num_Certificado,Saldo FROM Afiliado,Cuenta WHERE Afiliado.Num_Certificado " +
                "= Cuenta.Num_Certificado AND Afiliado.Estado = 'Activo' ";
            List<String[]> resultado = new List<String[]>();
            if( EjecutarComando() )
                while(DataReader.Read() )
                {
                    String[] tupla = new String[2];
                    tupla[0] = DataReader.GetValue(0).ToString().Trim();
                    tupla[1] = DataReader.GetValue(1).ToString().Trim();
                    resultado.Add(tupla);
                }
            return resultado;

        }
        public static Boolean EsMorosa(String fecha)
        {
            QueryString = "IF ( '" + fecha + "' < GETDATE()) SELECT 'TRUE' ELSE  SELECT 'FALSE'";
            if (EjecutarComando())
                if (DataReader.Read())
                {
                    bool  result = (DataReader.GetValue(0).ToString().Trim() == "TRUE");
                    FinalizarComandoCorrectamente();
                    return result;
                }
            return false;

        }
        /*FIN METODOS PRIVADOS DEL DT*/

        /*GETTERS DE LA PARAMETRIZACIÓN*/
        public static String getTodasOcupaciones()
        {
            /*obtener las ocupaciones no va en formato DK, solo va separado por , */
            QueryString = "SELECT Ocupacion.Ocupacion,Ocupacion.Valido FROM Ocupacion";
            String ocupaciones = "";
            /*nos conectamos, leemos y añadimos al string*/
            if ( EjecutarComando() )
                while (DataReader.Read())
                    ocupaciones += DataReader.GetString(0).Trim() + "," + DataReader.GetValue(1).ToString() + ";";
            DataTransaction.FinalizarComandoCorrectamente();
            /*eliminamos la ultima , si es que leimos algo*/
            if (ocupaciones.Length >= 1)
                return ocupaciones.Substring(0, ocupaciones.Length - 1);
            return ocupaciones;
        }
        public static String getTodosMotivos()
        {
            QueryString = "SELECT Motivo.Motivo,Motivo.Valido FROM Motivo";
            String motivos = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    motivos += DataReader.GetString(0).Trim() + "," + DataReader.GetValue(1).ToString() + ";";
            if (motivos.Length >= 1)
                return motivos.Substring(0, motivos.Length - 1);
            return motivos;
        }
        public static String getTodosParentescos()
        {
            QueryString = "SELECT Parentesco.Parentesco,Parentesco.Valido FROM Parentesco";
            String parentescos = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    parentescos += DataReader.GetString(0).Trim() + "," + DataReader.GetValue(1).ToString() + ";";
            if (parentescos.Length >= 1)
                return parentescos.Substring(0, parentescos.Length - 1);
            return parentescos;
        }
        public static String getMontoActual()
        {
            QueryString = "SELECT Monto.Monto FROM Monto WHERE Monto.ID_Monto = " +
                "(SELECT MAX(Monto.ID_Monto) FROM Monto)";
            String monto = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    monto += DataReader.GetValue(0).ToString().Trim() + ",";
            if (monto.Length == 0)
                return "0";
            return monto.Substring(0, monto.Length - 1);
        }
        public static String getLimiteActual()
        {
            QueryString = "SELECT Limite.Cantidad FROM Limite WHERE Limite.ID_Limite =" +
                   "(SELECT MAX(Limite.ID_Limite ) FROM Limite);";
            String limite = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    limite += DataReader.GetValue(0).ToString().Trim() + ",";
            if (limite.Length == 0)
                return "0";
            return limite.Substring(0, limite.Length - 1);
        }
        public static String getTodosIntereses()
        {
            QueryString = "SELECT Nombre,Taza,Aplica_Obligatoria,Aplica_Voluntaria,"+ 
                "Aplica_Obligatoria_Especial,Aplica_Voluntaria_ArribaDe,ArribaDe,Valido FROM Interes  ";
            String intereses = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String temp = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        temp += DataReader.GetValue(i).ToString().Trim() + ",";
                    intereses += temp.Substring(0, temp.Length - 1) + ";";
                }
            if (intereses.Length == 0)
                return "";
            return intereses.Substring(0, intereses.Length - 1);
        }
        public static String getInteres(String nombre_interes)
        {
            String Interes = "";
            QueryString = "SELECT Nombre,Taza,Aplica_Obligatoria,Aplica_Voluntaria,Aplica_Obligatoria_Especial," +
                    "Aplica_Voluntaria_ArribaDe,ArribaDe,Valido FROM Interes WHERE Interes.Nombre = '" + nombre_interes + "';";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Interes += DataReader.GetValue(i).ToString().Trim() + ",";
            if (Interes.CompareTo("") == 0)
                return "";
            return Interes.Substring(0, Interes.Length - 1);
        }
        public static String getAOE()
        {
            String[] ids = DataTransaction.getIDsAfiliadosActivos();
            if (ids == null)
                return "";
            String id_x = ids[0], Consulta = "";
            if (id_x.Length == 0)
                return "";
            QueryString = "SELECT Monto,YEAR(Fecha_Plazo),MONTH(Fecha_Plazo),Motivo,Descripcion FROM " +
                "Aportacion_Obligatoria_Especial,Aportacion,Motivo WHERE Aportacion.ID_Aportacion = " +
                "Aportacion_Obligatoria_Especial.ID_Aportacion AND Fecha_Plazo >= GETDATE() AND Motivo.ID_Motivo = " +
                "Aportacion_Obligatoria_Especial.ID_Motivo AND ID_Afiliado = " + id_x;
            if (EjecutarComando())
                while( DataReader.Read() )
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Consulta += DataReader.GetValue(i).ToString().Trim() + ',';
                    Consulta = Consulta.Substring(0, Consulta.Length - 1) + ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
            
        }

        /*DELETES DE LA BASE DE DATOS*/
        public static Boolean DeshabilitarOcupacion(String OcupacionDK)
        {
            string ocupacion = OcupacionDK.Split('&')[0], responsable =  OcupacionDK.Split('&')[1];
            QueryString = "UPDATE Ocupacion  SET Valido = 'FALSE' , Fecha_Modificacion = GETDATE() ," +
                    "Usuario_Responsable = '" + responsable + "' WHERE Ocupacion.Ocupacion = '" + ocupacion + "'";
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean DeshabilitarMotivo(String MotivoDK)
        {
            string motivo = MotivoDK.Split('&')[0], responsable = MotivoDK.Split('&')[1];
            QueryString = "UPDATE Motivo  SET Valido = 'FALSE' , Fecha_Modificacion = GETDATE() ," +
                    "Usuario_Responsable = '" + responsable + "' WHERE Motivo.Motivo = '" + motivo + "'";
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean DeshabilitarParentesco(String ParentescoDK)
        {
            string parentesco = ParentescoDK.Split('&')[0], responsable = ParentescoDK.Split('&')[1];
            QueryString = "UPDATE Parentesco  SET Valido = 'FALSE' , Fecha_Modificacion = GETDATE() ," +
                    "Usuario_Responsable = '" + responsable + "' WHERE Parentesco.Parentesco = '" + parentesco + "'";
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static Boolean DeshabilitarInteres(String InteresDK)
        {
            string interes = InteresDK.Split('&')[0], responsable = InteresDK.Split('&')[1];
            QueryString = "UPDATE Interes SET Interes.Valido = 'FALSE' , Fecha_Modificacion = GETDATE() ," +
                "Usuario_Responsable = '" + responsable + "' WHERE Interes.Nombre = '"+ interes + "';";
            if( EjecutarComando() ) { FinalizarComandoCorrectamente(); return true; } else return false;
        }

        /*CONSULTAS A LA BASE DE DATOS*/
        public static String getPerfilAfiliado(String correo)
        {
            /*primero generamos y tomamos los datos importantes de persona*/
            QueryString = "SELECT Num_Certificado,Persona.No_Identidad,P_Nombre,S_Nombre,P_Apellido,S_Apellido,Fecha_Nacimiento," +
                "Estado_Civil,Genero,Direccion,Empresa_Nombre,Empresa_Telefono,Empresa_Direccion,Empresa_Tiempo_Laboracion" +
                ",Empresa_Dpto,Lugar_Nacimiento,Ocupacion.Ocupacion FROM Afiliado,Persona,Ocupacion,Usuario WHERE " +
                "Persona.ID_Persona = Afiliado.ID_Persona AND Ocupacion.ID_Ocupacion = Afiliado.ID_Ocupacion AND " +
                "Usuario.ID_Usuario = Afiliado.ID_Usuario AND Usuario.Correo_Electronico = '" + correo + "'";
            String AfiliadoDK = "", temp = "", Num_Identidad = "", Num_Certificado = "";
            /*del resultado del query solo tomaremos los valores antes dle telefono y dejaremos
             * los demas en temp para ser utilizados mas tarde*/
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        if (i <= 9)
                        {
                            if (i != 6)
                                AfiliadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
                            else
                                AfiliadoDK += convertirFechaBD(((DataReader.GetValue(i).ToString().Trim()).Split(' '))[0]) + ",";
                        }
                        else
                            temp += DataReader.GetValue(i).ToString().Trim() + ",";
                    }
                }

            /*verificamos si obtuvimos algo, sino devemos terminar*/
            if (AfiliadoDK.CompareTo("") == 0)
                return AfiliadoDK;
            Num_Certificado = AfiliadoDK.Split(',')[0];
            Num_Identidad = AfiliadoDK.Split(',')[1];
            AfiliadoDK = AfiliadoDK.Substring(0, AfiliadoDK.Length - 1);
            /*ahora obtendremos los telefonos*/
            QueryString = "SELECT Telefono_Personal.Telefono FROM Telefono_Personal,Persona WHERE Persona.ID_Persona = " +
                "Telefono_Personal.ID_Persona AND Persona.No_Identidad = '" + Num_Identidad + "'";
            String telefonos = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    telefonos += DataReader.GetValue(0).ToString().Trim() + ",";
            if( telefonos != "" )
                AfiliadoDK += ";" + telefonos.Substring(0, telefonos.Length - 1);
            else
                AfiliadoDK += ";" + telefonos;
            /*ahora obtendremos los celulares*/
            QueryString = "SELECT Celular.Celular FROM Celular,Persona WHERE Persona.ID_Persona = Celular.ID_Persona AND " +
                        "Persona.No_Identidad = '" + Num_Identidad + "'";
            String celulares = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    celulares += DataReader.GetValue(0).ToString().Trim() + ",";
            if (telefonos != "")
                AfiliadoDK += ";" + celulares.Substring(0, celulares.Length - 1);
            else
                AfiliadoDK += ";" + celulares;
            /*ahora obtendremos el correo electronico y el password*/
            String datos_usuario = "";
            QueryString = "SELECT Correo_Electronico,Pass FROM Usuario,Afiliado,Persona WHERE Persona.ID_Persona = " +
                "Afiliado.ID_Persona AND Afiliado.ID_Usuario = Usuario.ID_Usuario AND Persona.No_Identidad = '" +
                Num_Identidad + "'";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        datos_usuario += DataReader.GetValue(i).ToString().Trim() + ",";
            AfiliadoDK += ";" + datos_usuario.Substring(0, datos_usuario.Length - 1) + ";";
            /*ahora es tiempo de pegar el resto de los datos del primer query*/
            String[] los_demas_datos = temp.Split(',');
            for (int i = 0; i < 6; i++)
            {
                if (i != 3)
                    AfiliadoDK += los_demas_datos[i] + ",";
                else
                    AfiliadoDK += convertirFechaBD((los_demas_datos[i].Split(' '))[0]) + ",";
            }
            /*ahora metemos la Ocupacion*/
            AfiliadoDK = AfiliadoDK.Substring(0, AfiliadoDK.Length - 1) + ";" + los_demas_datos[6] + ";";
            /*ahora conseguimos al beneficiario de contingencia*/
            QueryString = "SELECT Persona.No_Identidad,P_Nombre,S_Nombre,P_Apellido,S_Apellido,Fecha_Nacimiento," +
                "Estado_Civil,Genero,Direccion,Parentesco FROM Beneficiario,Beneficiario_Contingencia,Persona,Parentesco" +
                " WHERE Beneficiario.ID_Beneficiario = Beneficiario_Contingencia.ID_Beneficiario AND Persona.ID_Persona = " +
                "Beneficiario.ID_Persona AND Parentesco.ID_Parentesco = Beneficiario.ID_Parentesco AND Beneficiario.ID_Afiliado" +
                " = (SELECT ID_Afiliado FROM Afiliado,Persona WHERE Persona.ID_Persona = Afiliado.ID_Persona AND " +
                "Persona.No_Identidad = '" + Num_Identidad + "' AND Beneficiario.Valido = 'TRUE')";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        if (i != 5)
                            AfiliadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
                        else
                            AfiliadoDK += convertirFechaBD((DataReader.GetValue(i).ToString().Trim()).Split(' ')[0]) + ",";
                    }
                }
            /*arreglamos para antes de agregar beneficiarios normales*/
            AfiliadoDK = AfiliadoDK.Substring(0, AfiliadoDK.Length - 1);
            /*ahora insertamos a todos los beneficiarios normales*/
            QueryString = "SELECT Persona.No_Identidad,P_Nombre,S_Nombre,P_Apellido,S_Apellido,Fecha_Nacimiento," +
                "Estado_Civil,Genero,Direccion,Parentesco,Porcentaje_Seguro,Procentaje_Aportaciones FROM Beneficiario," +
                "Beneficiario_Normal,Persona,Parentesco WHERE Beneficiario.ID_Beneficiario = Beneficiario_Normal.ID_Beneficiario" +
                " AND Persona.ID_Persona = Beneficiario.ID_Persona AND Parentesco.ID_Parentesco = Beneficiario.ID_Parentesco " +
                "AND Beneficiario.ID_Afiliado = (SELECT ID_Afiliado FROM Afiliado,Persona WHERE Persona.ID_Persona = " +
                "Afiliado.ID_Persona AND Persona.No_Identidad = '" + Num_Identidad + "' AND Beneficiario.Valido = 'TRUE')";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String Benficiario = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        if (i != 5)
                            Benficiario += DataReader.GetValue(i).ToString().Trim() + ",";
                        else
                            Benficiario += convertirFechaBD((DataReader.GetValue(i).ToString().Trim()).Split(' ')[0]) + ",";
                    }
                    AfiliadoDK += ";" + Benficiario.Substring(0, Benficiario.Length - 1);
                }

            return AfiliadoDK +"~" + getEstadoAfiliado(correo);
        }
        public static String getPerfilEmpleado(String correo)
        {
            /*datos personales del empleado*/
            QueryString = "SELECT No_Identidad,P_Nombre,S_Nombre,P_Apellido,S_Apellido,Fecha_Nacimiento,Estado_Civil,Genero," +
                "Direccion FROM Persona,Empleado,Usuario WHERE Empleado.ID_Persona = Persona.ID_Persona AND " +
                "Usuario.ID_Usuario = Empleado.ID_Usuario AND Usuario.Correo_Electronico = '" + correo + "'";
            String EmpleadoDK = "",Num_Identidad = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        if (i != 5)
                            EmpleadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
                        else
                            EmpleadoDK += convertirFechaBD(((DataReader.GetValue(i).ToString().Trim()).Split(' '))[0]) + ",";
                    }
            /*sino obtuvimos nada terminamos el proceso*/
            if (EmpleadoDK.CompareTo("") == 0)
                return EmpleadoDK;
            Num_Identidad = EmpleadoDK.Split(',')[0];
            /*obtenemos los telefonos*/
            EmpleadoDK = EmpleadoDK.Substring(0, EmpleadoDK.Length - 1) + ";";
            QueryString = "SELECT Telefono FROM Telefono_Personal,Persona WHERE Persona.ID_Persona = Telefono_Personal.ID_Persona"
                + " AND Persona.No_Identidad = '" + Num_Identidad + "';";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        EmpleadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
            EmpleadoDK = EmpleadoDK.Substring(0, EmpleadoDK.Length - 1) + ";";
            /*obtenemos los celulares*/
            QueryString = "SELECT Celular FROM Celular,Persona WHERE Persona.ID_Persona = Celular.ID_Persona" +
                " AND Persona.No_Identidad = '" + Num_Identidad + "';";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        EmpleadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
            EmpleadoDK = EmpleadoDK.Substring(0, EmpleadoDK.Length - 1) + ";";
            /*obtenemos el usuario y el resto de datos personales*/
            QueryString = "SELECT Usuario.Correo_Electronico,Pass,Empleado.Puesto FROM Empleado,Persona,Usuario WHERE" +
                " Usuario.ID_Usuario = Empleado.ID_Usuario AND Persona.ID_Persona = Empleado.ID_Persona AND " + 
                "Persona.No_Identidad = '" + Num_Identidad + "';";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        EmpleadoDK += DataReader.GetValue(i).ToString().Trim() + ",";
            EmpleadoDK = EmpleadoDK.Substring(0, EmpleadoDK.Length - 1) ;
            return EmpleadoDK + "~" + getRol(correo);
        }
        public static String getAportacionesObligatorias(String Num_Certificado,String fecha_Desde,String fecha_hasta)
        {
            QueryString = "SELECT Aportacion.monto,Aportacion.Descripcion,Aportacion_Obligatoria.Fecha_Plazo," +
                "Aportacion.Fecha_Realizacion FROM Aportacion_Obligatoria,Aportacion,Afiliado WHERE Aportacion_Obligatoria" +
                ".ID_Aportacion = Aportacion.ID_Aportacion AND Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND " +
                "Afiliado.Num_Certificado = " + Num_Certificado + " AND Aportacion.Fecha_Realizacion >= '" + fecha_Desde
                + "' AND Aportacion.Fecha_Realizacion <= '" + fecha_hasta + "'";
            String Consulta = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        String dato = DataReader.GetValue(i).ToString().Trim();
                        if (i == 2 || i == 3)
                        {
                            if (dato.Length != 0)
                                Consulta += dato.Split(' ')[0] + ',';
                            else
                                Consulta += dato + ',';
                        }
                        else
                            Consulta += dato + ',';
                        
                    }
                    Consulta += ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String getAportacionesObligatoriasEspeciales(String Num_Certificado, String fecha_Desde, String fecha_hasta)
        {
            QueryString = "SELECT Aportacion.monto,Aportacion.Descripcion,Aportacion_Obligatoria_Especial.Fecha_Plazo," +
                    "Aportacion.Fecha_Realizacion,Motivo.Motivo FROM Aportacion_Obligatoria_Especial,Aportacion,Afiliado," +
                    "Motivo WHERE Aportacion_Obligatoria_Especial.ID_Aportacion = Aportacion.ID_Aportacion AND " +
                    "Afiliado.ID_Afiliado = Aportacion.ID_Afiliado AND Motivo.ID_Motivo = Aportacion_Obligatoria_Especial" +
                    ".ID_Motivo AND Afiliado.Num_Certificado = " + Num_Certificado + " AND Aportacion.Fecha_Realizacion >= '"
                    + fecha_Desde + "' AND Aportacion.Fecha_Realizacion <= '" + fecha_hasta + "'";
            String Consulta = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        String dato = DataReader.GetValue(i).ToString().Trim();
                        if (i == 2 || i == 3)
                        {
                            if (dato.Length != 0)
                                Consulta += dato.Split(' ')[0] + ',';
                            else
                                Consulta += dato + ',';
                        }
                        else
                            Consulta += dato + ',';

                    }
                    Consulta = Consulta.Substring(0, Consulta.Length - 1) + ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String getAportacionesVoluntarias(String Num_Certificado, String fecha_Desde, String fecha_hasta)
        {
            QueryString = "SELECT Aportacion.monto,Aportacion.Descripcion,'',Aportacion.Fecha_Realizacion,'' " +
                "FROM Aportacion_Voluntaria,Aportacion,Afiliado WHERE Aportacion_Voluntaria.ID_Aportacion = " +
                "Aportacion.ID_Aportacion AND Afiliado.ID_Afiliado = Aportacion.ID_Afiliado  AND Afiliado.Num_Certificado = "
                + Num_Certificado + " AND Aportacion.Fecha_Realizacion >= '" + fecha_Desde + 
                "' AND Aportacion.Fecha_Realizacion <= '" + fecha_hasta + "'";
            String Consulta = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        String dato = DataReader.GetValue(i).ToString().Trim();
                        if (i == 2 || i == 3)
                        {
                            if (dato.Length != 0)
                                Consulta += dato.Split(' ')[0] + ',';
                            else
                                Consulta += dato + ',';
                        }
                        else
                            Consulta += dato + ',';

                    }
                    Consulta = Consulta.Substring(0, Consulta.Length - 1) + ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String getSiguienteNumeroDeCertificado()
        {
            QueryString = "SELECT (MAX(Num_Certificado)+1) FROM Afiliado";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String valor = DataReader.GetValue(0).ToString().Trim();
                    if (valor.Length == 0)
                        return "1";
                    return valor;
                }
            return "ERROR";

        }

        /*CONSULTAS PARA EL MANEJO DEL SISTEMA*/
        public static String getLogin(String LoginDK) 
        {
            String Correo = LoginDK.Split(',')[0], Pass = LoginDK.Split(',')[1]; 
            QueryString = "SELECT COUNT(*) FROM Usuario WHERE Usuario.Correo_Electronico = '" + Correo + 
                "' AND Usuario.Pass = '"  + Pass + "' ";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) == 0)
                        return "";
            /*ocupamos saber si es afiliado o empleado*/
            QueryString = "SELECT COUNT(*) FROM Usuario,Empleado WHERE Empleado.ID_Usuario = Usuario.ID_Usuario AND " +
                          "Usuario.Correo_Electronico = '" + Correo + "' ";
            if (EjecutarComando())
                if (DataReader.Read())
                    if ((int)DataReader.GetValue(0) == 0)
                    {
                        /*Es afiliado*/
                        QueryString = "SELECT P_Nombre,S_Nombre,P_Apellido,S_Apellido FROM Usuario,Persona,Afiliado" +
                                      " WHERE Usuario.Correo_Electronico = '" + Correo + "' AND Afiliado.ID_Persona = " +
                                      "Persona.ID_Persona AND Afiliado.ID_Usuario = Usuario.ID_Usuario";
                    }
                    else
                    {
                        /*es empleado*/
                        QueryString = "SELECT P_Nombre,S_Nombre,P_Apellido,S_Apellido FROM Usuario,Persona,Empleado" +
                                      " WHERE Usuario.Correo_Electronico = '" + Correo + "' AND Empleado.ID_Persona = " +
                                      "Persona.ID_Persona AND Empleado.ID_Usuario = Usuario.ID_Usuario";
                    }
            String Nombres = "";
            if (EjecutarComando())
                if (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Nombres += DataReader.GetValue(i).ToString().Trim() + ",";
            if (Nombres.Length == 0)
                return "";
            return Nombres.Substring(0,Nombres.Length-1) + ';' + getPermisos(Correo) + ';' + getRol(Correo);

        }
        public static String getControlDePago(String ControlPago_DK)
        {
            String id = DataTransaction.getIDAfiliadoBusqueda(ControlPago_DK);
            QueryString = "SELECT Num_Certificado,P_Nombre,S_Nombre,P_Apellido,S_Apellido,No_Identidad FROM Persona,Afiliado"
                          + " WHERE Persona.ID_Persona = Afiliado.ID_Persona AND Afiliado.ID_Afiliado = " + id;
            String resultado = "";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        if( i == 0 || i == 4 )
                            resultado += DataReader.GetValue(i).ToString().Trim() + ";";
                        else
                            resultado += DataReader.GetValue(i).ToString().Trim() + ",";
            if (resultado.Length == 0)
                return "";
            resultado = resultado.Substring(0,resultado.Length -1);
            resultado += "&" + DataTransaction.gettodasLasAportacionesObligatorias(id);
            return resultado;
        } 
        public static String gettodasLasAportacionesObligatorias(String Id_Afiliado)
        {
            QueryString = " SELECT Aportacion.ID_Aportacion,Aportacion_Obligatoria.Fecha_Plazo,Aportacion.Monto  " +
                "FROM Aportacion,Aportacion_Obligatoria WHERE Aportacion.ID_Aportacion = Aportacion_Obligatoria." +
                "ID_Aportacion AND Aportacion.ID_Afiliado = " + Id_Afiliado + " AND Aportacion_Obligatoria.Cancelado = 'False'";
            string resultado  = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                    {
                        if (i == 1)
                            resultado += DataTransaction.convertirFechaBD(DataReader.GetValue(i).ToString().Trim()) + ",";
                        else
                            resultado += DataReader.GetValue(i).ToString().Trim() + ",";
                    }
                    resultado = resultado + ";";
                }
            DataTransaction.FinalizarComandoCorrectamente();
            QueryString = "SELECT Aportacion.ID_Aportacion,Aportacion_Obligatoria_Especial.Fecha_Plazo," +
                    "Aportacion.Monto,Motivo.Motivo FROM Aportacion,Aportacion_Obligatoria_Especial,Motivo WHERE " +
                    "Aportacion.ID_Aportacion = Aportacion_Obligatoria_Especial.ID_Aportacion AND " +
                    "Aportacion_Obligatoria_Especial.ID_Motivo = Motivo.ID_Motivo AND Aportacion.ID_Afiliado = " + Id_Afiliado
                    + " AND Aportacion_Obligatoria_Especial.Cancelado = 'False'";
          
                if (EjecutarComando())
                    while (DataReader.Read())
                    {
                        for (int i = 0; i < DataReader.FieldCount; i++)
                        {
                            if (i == 1)
                                resultado += DataTransaction.convertirFechaBD(DataReader.GetValue(i).ToString().Trim()) + ",";
                            else
                                resultado += DataReader.GetValue(i).ToString().Trim() + ",";
                        }
                        resultado = resultado.Substring(0,resultado.Length-1) + ";";
                    }
                DataTransaction.FinalizarComandoCorrectamente();
            if( resultado.Length != 0 )
            return resultado.Substring(0,resultado.Length-1);
            return "";
        }
        public static Boolean PagarAportaciones(String AportacionesID_DK)
        {
            string tempDK = AportacionesID_DK;
            String responsable = AportacionesID_DK.Split('&')[1];
            AportacionesID_DK = AportacionesID_DK.Split('&')[0];
            String[] datos = AportacionesID_DK.Split(';');
            /*primero obtenemos el ID del afiliado*/
            String ID_Afiliado = DataTransaction.getIDAfiliado(datos[0]);
            if (ID_Afiliado.Length == 0)
                return false;
            if( datos[1].Length != 0 )
             QueryString += DataTransaction.PagarObligatorias(ID_Afiliado,datos[1].Split(','),responsable);
            for (int i = 2; i < datos.Length; i++)
                if( datos[i].Length != 0 )
                    QueryString += Generador.InsertarAportacionVoluntaria(datos[0] + "," + datos[i] + "&" + responsable);

            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static String getAfiliadosPAGAR()
        {
            QueryString = " SELECT Afiliado.Num_Certificado,Persona.P_Nombre,Persona.S_Nombre,Persona.P_Apellido," +
                "Persona.S_Apellido FROM Afiliado,Persona  WHERE Afiliado.Estado = 'Activo' AND Afiliado.ID_Persona" +
                " = Persona.ID_Persona ";
            String Consulta = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Consulta += DataReader.GetValue(i).ToString().Trim() + ",";
                    Consulta = Consulta.Substring(0, Consulta.Length - 1) + ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String BuscarAfiliadoConsultas(String Criterios_DK)
        {
            QueryString = "SELECT DISTINCT Afiliado.Num_Certificado,Persona.P_Nombre,Persona.S_Nombre," +
	            "Persona.P_Apellido,Persona.S_Apellido,Persona.No_Identidad,Usuario.Correo_Electronico" +
                " FROM Afiliado,Persona,Usuario,Telefono_Personal,Celular,Ocupacion  WHERE Afiliado.ID_Persona " +
                "= Persona.ID_Persona AND Usuario.ID_Usuario = Afiliado.ID_Usuario AND Telefono_Personal.ID_Persona = " +
                "Persona.ID_Persona AND Celular.ID_Persona = Persona.ID_Persona AND Afiliado.ID_Ocupacion = " +
                "Ocupacion.ID_Ocupacion ";
            String[] Criterios = Criterios_DK.Split(',');
            if (Criterios[0].Length != 0)
                QueryString += " AND Afiliado.Num_Certificado =  " + Criterios[0];
            if (Criterios[1].Length != 0)
                QueryString += " AND Persona.P_Nombre = '" + Criterios[1] + "' ";
            if (Criterios[2].Length != 0)
                QueryString += " AND Persona.S_Nombre = '" + Criterios[2] + "' ";
            if (Criterios[3].Length != 0)
                QueryString += " AND Persona.P_Apellido = '" + Criterios[3] + "' ";
            if (Criterios[4].Length != 0)
                QueryString += " AND Persona.S_Apellido = '" + Criterios[4] + "' ";
            if (Criterios[5].Length != 0)
                QueryString += " AND Persona.No_Identidad = '" + Criterios[5] + "' ";
            if (Criterios[6].Length != 0)
                QueryString += " AND Telefono_Personal.Telefono  = '" + Criterios[6] + "' ";
            if (Criterios[7].Length != 0)
                QueryString += " AND Celular.Celular  = '" + Criterios[7] + "' ";
            if (Criterios[8].Length != 0)
                QueryString += " AND Ocupacion.Ocupacion  = '" + Criterios[8] + "' ";
            if (Criterios[9].Length != 0)
                QueryString += " AND Persona.Genero = '" + Criterios[9] + "' ";
            if (Criterios[10].Length != 0)
                QueryString += " AND Usuario.Correo_Electronico = '" + Criterios[10] + "' ";
            if (Criterios[11].Length != 0)
                QueryString += " AND Persona.Estado_Civil = '" + Criterios[11] + "' ";
            if (Criterios[12].Length != 0)
                QueryString += " AND Afiliado.Estado = '" + Criterios[12] + "' ";
            if (Criterios[13].Length != 0)
                QueryString += " AND Afiliado.Empresa_Nombre = '" + Criterios[13] + "' ";
            String Consulta = "";
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Consulta += DataReader.GetValue(i).ToString().Trim() + ",";
                    Consulta = Consulta.Substring(0, Consulta.Length - 1) + ';';
                }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String ConsultarEstadoDeCuenta(String EstadoCuenta_DK)
        {
            String num_certificado = EstadoCuenta_DK.Split(',')[0], desde = EstadoCuenta_DK.Split(',')[1],
                hasta = EstadoCuenta_DK.Split(',')[2], EstadoCuenta = "";
            String EstadoCuentaO = DataTransaction.getAportacionesObligatorias(num_certificado,desde,hasta),
            EstadoCuentaOE = DataTransaction.getAportacionesObligatoriasEspeciales(num_certificado, desde, hasta),
            EstadoCuentaV = DataTransaction.getAportacionesVoluntarias(num_certificado, desde, hasta);

            EstadoCuenta += EstadoCuentaO;
            if (EstadoCuentaO.Length != 0 && EstadoCuentaOE.Length != 0)
                EstadoCuenta += ';' + EstadoCuentaOE;
            else
                EstadoCuenta += EstadoCuentaOE;

            if (EstadoCuentaOE.Length != 0 && EstadoCuentaV.Length != 0)
                EstadoCuenta += ';' + EstadoCuentaV;
            else
            {
                if (EstadoCuentaO.Length != 0 && EstadoCuentaV.Length != 0)
                    EstadoCuenta += ';' + EstadoCuentaV;
                else
                    EstadoCuenta += EstadoCuentaV;
            }
            
            return EstadoCuenta;
        }
        public static String getTodosLosPermisos()
        {
            bool[,] getPermisos = new bool[3, 6];
            List<string> query_resultado = new List<string>();
            QueryString = "SELECT Rol.Nombre,Permiso.Nombre FROM Rol,Tiene,Permiso WHERE Rol.ID_Rol = Tiene.ID_Rol AND " + 
                "Permiso.ID_Permiso = Tiene.ID_Permiso";
            if (EjecutarComando())
                while (DataReader.Read())
                    query_resultado.Add( (DataReader.GetValue(0).ToString().Trim() + "," 
                        + DataReader.GetValue(1).ToString().Trim()) );
            foreach( String tupla in query_resultado )
            {
                String rol = tupla.Split(',')[0], permiso = tupla.Split(',')[1];
                int i = -1, j = -1;
                switch (rol)
                {
                    case "Administrador":
                            getPermisos = Generador.AsignarPermiso(0, permiso, getPermisos);
                        break;
                    case "Empleado":
                            getPermisos = Generador.AsignarPermiso(1, permiso, getPermisos);
                        break;
                    case "Afiliado":
                            getPermisos = Generador.AsignarPermiso(2, permiso, getPermisos);
                        break;
                }
            }
            return Generador.PermisosToString(getPermisos);
        }
        public static Boolean ModificarPermisos(String PermisosDK)
        {
            String responsable = PermisosDK.Split('&')[1];
            PermisosDK = PermisosDK.Split('&')[0];
            QueryString = " DELETE FROM Tiene ";
            String[] roles = PermisosDK.Split(';');
            QueryString += Generador.getQueryPermisoRol("Administrador", roles[0].Split(','), responsable);
            QueryString += Generador.getQueryPermisoRol("Empleado", roles[1].Split(','), responsable);
            QueryString += Generador.getQueryPermisoRol("Afiliado", roles[2].Split(','), responsable);
            if (EjecutarComando()) { FinalizarComandoCorrectamente(); return true; } else return false;
        }
        public static String getPersonaTemporales()
        {
            String Consulta = "";
            QueryString = "SELECT Usuario.Correo_Electronico FROM Persona_Temp,Usuario,Persona,Afiliado " +
                "WHERE Persona.ID_Persona = Persona_Temp.ID_Persona AND Persona_Temp.Valido = 'TRUE' AND " +
                "Afiliado.ID_Persona = Persona.ID_Persona AND Afiliado.ID_Usuario = Usuario.ID_Usuario";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Consulta += DataReader.GetValue(i).ToString().Trim() + ",";
            QueryString = "SELECT Usuario.Correo_Electronico FROm Persona_Temp,Usuario,Persona,Empleado " +
                "WHERE Persona.ID_Persona = Persona_Temp.ID_Persona AND Persona_Temp.Valido = 'TRUE' AND " +
                "Empleado.ID_Persona = Persona.ID_Persona AND Empleado.ID_Usuario = Usuario.ID_Usuario";
            if (EjecutarComando())
                while (DataReader.Read())
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        Consulta += DataReader.GetValue(i).ToString().Trim() + ",";
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }

        /*HORRIBLES METODOS DE CAPITALIZACION*/
        public static String getAportacionesACapitalizar()
        {
            List<String[]> O = getAportacionesObligatoriasACapitalizar(),
                           OE = getAportacionesObligatoriasEspecialesACapitalizar(), 
                           V = getAportacionesVoluntariaACapitalizar();
            /*obtenemos todos los intereses*/
            String Intereses = DataTransaction.getTodosIntereses();
            if (Intereses.Length == 0)
                return "";
            String[] intereses_DK = Intereses.Split(';');
            Interes[] intereses = new Interes[intereses_DK.Length];
            for (int i = 0; i < intereses_DK.Length; i++)
                intereses[i] = new Interes(intereses_DK[i]);
            String Consulta = "";
            
            for (int i = 0; i < intereses.Length; i++)
            {
                if (intereses[i].nombre != "Interes de Mora" && intereses[i].Valido )
                {
                    if (intereses[i].Obligatoria)
                        foreach (String[] aportacion in O)
                            Consulta += Generador.getTuplaTodasLasAportaciones(aportacion, intereses[i]);

                    if (intereses[i].Obligatoria_Especial)
                        foreach (String[] aportacion in OE)
                            Consulta += Generador.getTuplaTodasLasAportaciones(aportacion, intereses[i]);

                    if (intereses[i].Voluntaria)
                        foreach (String[] aportacion in V)
                            Consulta += Generador.getTuplaTodasLasAportaciones(aportacion, intereses[i]);

                    if (intereses[i].VoluntariaArribaDe != 0)
                        foreach (String[] aportacion in V)
                            if( Convert.ToDouble( aportacion[2] ) >= intereses[i].VoluntariaArribaDe)
                                Consulta += Generador.getTuplaTodasLasAportaciones(aportacion, intereses[i]);
                }
            }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0, Consulta.Length - 1);
        }
        public static String getSaldosACapitalizar()
        {
            List<String[]> tabla = new List<String[]>(),
                           afiliados = DataTransaction.getSaldosAfiliadosActivos();
            String aportaciones_s = DataTransaction.getAportacionesACapitalizar();
            String[] aportaciones = aportaciones_s.Split(';');
            String Consulta = "";
            foreach (String[] afiliado in afiliados)
            {
                List<String> aportaciones_afiliado = Generador.filtrarAportaciones(aportaciones, afiliado[0]);
                float Total_Interes = Generador.getTotalInteres(aportaciones_afiliado),
                    Saldo_actual = ((float)Convert.ToDouble(afiliado[1])), 
                    Nuevo_Saldo = Saldo_actual + Total_Interes;
                Consulta += afiliado[0] + ',' + Saldo_actual.ToString() + ',' + Total_Interes.ToString() + ',' +
                    Nuevo_Saldo.ToString() + ';';
            }
            if (Consulta.Length == 0)
                return "";
            return Consulta.Substring(0,Consulta.Length-1);
        }
        public static Boolean CapitalizarExtraordinariamente(String responsable)
        {
            String QueryCapitalizar = "", Intereses = DataTransaction.getTodosIntereses();
            if (Intereses.Length == 0)
                return false;
            String[] intereses_DK = Intereses.Split(';');
            Interes[] intereses = new Interes[intereses_DK.Length];
            for (int i = 0; i < intereses_DK.Length; i++)
                intereses[i] = new Interes(intereses_DK[i]);
            String[] numCertificados = DataTransaction.getNumCertificadosAfiliadosActivos();
            for (int j = 0; j < numCertificados.Length; j++)
            {
                /*para cada uno de los afiliados activos*/
                String montos_obligatorios =
                    DataTransaction.getMontoAportacionesObligatoriasAfiliado(numCertificados[j]),
                       montos_obligatorios_especiales =
                    DataTransaction.getMontoAportacionesObligatoriasEspecialesAfiliado(numCertificados[j]),
                       montos_voluntarias =
                    DataTransaction.getMontoVoluntariaAfiliado(numCertificados[j]);
                String[] monto_O = montos_obligatorios.Split(','),
                         monto_OE = montos_obligatorios_especiales.Split(','),
                         monto_V = montos_voluntarias.Split(',');
                int[] i_monto_O = new int[monto_O.Length],
                      i_monto_OE = new int[monto_OE.Length],
                      i_monto_V = new int[monto_V.Length];
                for (int i = 0; i < monto_O.Length; i++)
                    i_monto_O[i] = Convert.ToInt32(monto_O[i]);
                for (int i = 0; i < monto_OE.Length; i++)
                    i_monto_OE[i] = Convert.ToInt32(monto_OE[i]);
                for (int i = 0; i < monto_V.Length; i++)
                    i_monto_V[i] = Convert.ToInt32(monto_V[i]);

                for (int i = 0; i < intereses.Length; i++)
                {
                    if (intereses[i].nombre != "Interes de Mora" && intereses[i].Valido)
                    {
                        if (intereses[i].Obligatoria)
                            for (int x = 0; x < i_monto_O.Length; x++)
                                if (i_monto_O[x] != 0)
                                    QueryCapitalizar += Generador.SumarACuenta(numCertificados[j],
                                        intereses[i].Ganancia(i_monto_O[x]), responsable);

                        if (intereses[i].Obligatoria_Especial)
                            for (int x = 0; x < i_monto_OE.Length; x++)
                                if (i_monto_OE[x] != 0)
                                    QueryCapitalizar += Generador.SumarACuenta(numCertificados[j],
                                        intereses[i].Ganancia(i_monto_OE[x]), responsable);

                        if (intereses[i].Voluntaria)
                            for (int x = 0; x < i_monto_V.Length; x++)
                                if (i_monto_V[x] != 0)
                                    QueryCapitalizar += Generador.SumarACuenta(numCertificados[j],
                                        intereses[i].Ganancia(i_monto_V[x]), responsable);

                        if (intereses[i].VoluntariaArribaDe != 0)
                            for (int x = 0; x < i_monto_V.Length; x++)
                                if (i_monto_V[x] >= intereses[i].VoluntariaArribaDe && i_monto_V[x] != 0)
                                    QueryCapitalizar += Generador.SumarACuenta(numCertificados[j],
                                        intereses[i].Ganancia(i_monto_V[x]), responsable);
                    }
                }
            }
            QueryCapitalizar += Log.Capitalizacion(responsable);
            QueryString = QueryCapitalizar;
            if (EjecutarComando())
            {
                FinalizarComandoCorrectamente();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void AsignarMora()
        {
            String[] certificados = DataTransaction.getNumCertificadosAfiliadosActivos();
            Interes interes_mora = new Interes( DataTransaction.getInteres( "Interes de Mora" ));
            String QueryMoroso = "";
            for( int i = 0 ; i < certificados.Length; i++ )
            {
               String datos = DataTransaction.getControlDePago(certificados[i] + ";;"),
                   aportaciones = datos.Split('&')[1];
               if (aportaciones.Length == 0)
                   i = certificados.Length;
               else
               {
                   List<String> aportaciones_morosas = Generador.filtrarAportacionesMorosas(aportaciones.Split(';'));
                   foreach (String aportacion_morosa in aportaciones_morosas)
                       QueryMoroso += Generador.AumentarAportacion(
                           interes_mora.Ganancia((float)Convert.ToDouble(aportacion_morosa.Split(',')[2])).ToString(),
                           aportacion_morosa.Split(',')[0]);
               }
               
            }
            if (QueryMoroso == "")
                return;
            QueryString = QueryMoroso;
            if (EjecutarComando())
                FinalizarComandoCorrectamente();
        }

        /*REPORTES alfin...*/
        public static String ReporteTodasAportaciones(String ReporteAportacionesDK)
        {
            String Desde = ReporteAportacionesDK.Split(';')[2].Split(',')[0],
                Hasta = ReporteAportacionesDK.Split(';')[2].Split(',')[1];
            List<String> O = null, OE = null, V = null, Num_Certificados = new List<String>(),
                total = new List<String>();
            String[] num_cert_array = ReporteAportacionesDK.Split(';')[0].Split(',');
            foreach (string tupla in num_cert_array)
                Num_Certificados.Add(tupla);
            switch (ReporteAportacionesDK.Split(';')[1])
            {
                case "T":
                    O = DataTransaction.getAportacionesO(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in O)
                        total.Add(tupla);
                    OE = DataTransaction.getAportacionesOE(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in OE)
                        total.Add(tupla);
                    V = DataTransaction.getAportacionesV(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in V)
                        total.Add(tupla);
                    break;
                case "O":
                    O = DataTransaction.getAportacionesO(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in O)
                        total.Add(tupla);
                    break;
                case "V":
                    V = DataTransaction.getAportacionesV(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in V)
                        total.Add(tupla);
                    break;
                case "E":
                    OE = DataTransaction.getAportacionesOE(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in OE)
                        total.Add(tupla);
                    break;
                case "Y":
                    O = DataTransaction.getAportacionesO(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in O)
                        total.Add(tupla);
                    OE = DataTransaction.getAportacionesOE(Num_Certificados, Desde, Hasta);
                    foreach (String tupla in OE)
                        total.Add(tupla);
                    break;
            }
            String Reporte = "";
            foreach (String tupla in total)
                Reporte += tupla + ";";
            if( Reporte.Length == 0 )
                return "";
            return Reporte.Substring(0, Reporte.Length - 1);
        }
        public static String ReporteTodosSaldos(String ListaAfiliados)
        {
            String[] num_certificados = ListaAfiliados.Split(',');
            if( num_certificados.Length == 0 )
                return "";
            QueryString = "SELECT Afiliado.Num_Certificado,Persona.P_Nombre,Persona.S_Nombre,Persona.P_Apellido," +
                "Persona.S_Apellido,Cuenta.Saldo FROM Persona,Afiliado,Cuenta WHERE Persona.ID_Persona = " +
                "Afiliado.ID_Persona AND Afiliado.Num_Certificado = Cuenta.Num_Certificado AND (Afiliado.Num_Certificado = " +
                num_certificados[0] + " ";
            for (int i = 1; i < num_certificados.Length; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados[i] + " ";
            QueryString += " ) ORDER BY Afiliado.Num_Certificado";
            List<String[]> datos_afilidos = new List<String[]>();
            if (EjecutarComando())
            {
                while (DataReader.Read())
                {
                    String[] tupla = new String[6];
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla[i] = DataReader.GetValue(i).ToString().Trim();
                    datos_afilidos.Add(tupla);
                }
            }
            else return "";

            List<String> O = DataTransaction.getAportacionesO(num_certificados),
                         OE = DataTransaction.getAportacionesOE(num_certificados),
                         V = DataTransaction.getAportacionesV(num_certificados);
            
            String RESULT = "";
            foreach (String[] Afiliado in datos_afilidos)
            {
                List<String> afiliado_O = Generador.filtrarAportaciones(O, Afiliado[0]),
                             afiliado_OE = Generador.filtrarAportaciones(OE,Afiliado[0]),
                             afiliado_V = Generador.filtrarAportaciones(V, Afiliado[0]);

                for (int i = 0; i < Afiliado.Length - 1; i++)
                    RESULT += Afiliado[i] + ",";
                float monto_O = (Generador.getTotalAportaciones(afiliado_O) + Generador.getTotalAportaciones(afiliado_OE)),
                    monto_V = Generador.getTotalAportaciones(afiliado_V);
                RESULT += monto_O + "," + monto_V + "," + ((Convert.ToDouble(Afiliado[5])) - (monto_O+monto_V)) + 
                    "," + Afiliado[5] + ";";
            }
            if (RESULT.Length == 0)
                return "";
            return RESULT.Substring(0,RESULT.Length-1);
        }

        private static List<String> getAportacionesO(List<String> num_certificados, String Fecha_Desde, String Fecha_Hasta)
        {
            if (num_certificados.Count == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Monto.Monto,Aportacion.Descripcion,Aportacion_Obligatoria.Fecha_Plazo"+
                ",Aportacion.Fecha_Realizacion,'N/A' AS Motivo,Aportacion_Obligatoria.Cancelado FROM Afiliado,Monto,Aportacion,"+
                "Aportacion_Obligatoria WHERE Aportacion.ID_Afiliado = Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = " +
                "Aportacion_Obligatoria.ID_Aportacion AND Aportacion_Obligatoria.ID_Monto = Monto.ID_Monto AND ( "+
                "Afiliado.Num_Certificado = " + num_certificados.ElementAt(0) + " ";
            for (int i = 1; i < num_certificados.Count; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados.ElementAt(i) + " ";
            QueryString += ") AND (Aportacion.Fecha_Realizacion >= '" + Fecha_Desde +"' AND Aportacion.Fecha_Realizacion " +
                "<= '" + Fecha_Hasta + "' ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }
        private static List<String> getAportacionesOE(List<String> num_certificados, String Fecha_Desde, String Fecha_Hasta)
        {
            if (num_certificados.Count == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Monto,Aportacion.Descripcion," +
                   "Aportacion_Obligatoria_Especial.Fecha_Plazo,Aportacion.Fecha_Realizacion,Motivo.Motivo," +
                   "Aportacion_Obligatoria_Especial.Cancelado FROM Afiliado,Motivo,Aportacion,Aportacion_Obligatoria_Especial" +
                   " WHERE Aportacion.ID_Afiliado = Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = " +
                   "Aportacion_Obligatoria_Especial.ID_Aportacion AND Aportacion_Obligatoria_Especial.ID_Motivo = " +
                   "Motivo.ID_Motivo AND ( Afiliado.Num_Certificado = " + num_certificados.ElementAt(0) + " ";
            for (int i = 1; i < num_certificados.Count; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados.ElementAt(i) + " ";
            QueryString += " ) AND (Aportacion.Fecha_Realizacion >= '" + Fecha_Desde + "' AND Aportacion.Fecha_Realizacion " +
                "<= '" + Fecha_Hasta + "' ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }
        private static List<String> getAportacionesV(List<String> num_certificados, String Fecha_Desde, String Fecha_Hasta)
        {
            if (num_certificados.Count == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Monto,Aportacion.Descripcion," +
                  "'N/A' AS Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo,'N/A' AS Cancelado" +
                  " FROM Afiliado,Aportacion,Aportacion_Voluntaria WHERE Aportacion.ID_Afiliado = " +
                  "Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = Aportacion_Voluntaria.ID_Aportacion AND" +
                  " (Afiliado.Num_Certificado =  " + num_certificados.ElementAt(0) + " ";
            for (int i = 1; i < num_certificados.Count; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados.ElementAt(i) + " ";
            QueryString += " ) AND (Aportacion.Fecha_Realizacion >= '" + Fecha_Desde + "' AND Aportacion.Fecha_Realizacion " +
                "<= '" + Fecha_Hasta + "' ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }

        private static List<String> getAportacionesO(String[] num_certificados)
        {
            if (num_certificados.Length == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Monto.Monto,Aportacion.Descripcion,Aportacion_Obligatoria.Fecha_Plazo" +
                ",Aportacion.Fecha_Realizacion,'N/A' AS Motivo,Aportacion_Obligatoria.Cancelado FROM Afiliado,Monto,Aportacion," +
                "Aportacion_Obligatoria WHERE Aportacion.ID_Afiliado = Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = " +
                "Aportacion_Obligatoria.ID_Aportacion AND Aportacion_Obligatoria.ID_Monto = Monto.ID_Monto AND ( " +
                "Afiliado.Num_Certificado = " + num_certificados[0] + " ";
            for (int i = 1; i < num_certificados.Length; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados[i] + " ";
            QueryString += ") AND (Aportacion.Fecha_Realizacion <= GETDATE() ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }
        private static List<String> getAportacionesOE(String[] num_certificados)
        {
            if (num_certificados.Length == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Monto,Aportacion.Descripcion," +
                   "Aportacion_Obligatoria_Especial.Fecha_Plazo,Aportacion.Fecha_Realizacion,Motivo.Motivo," +
                   "Aportacion_Obligatoria_Especial.Cancelado FROM Afiliado,Motivo,Aportacion,Aportacion_Obligatoria_Especial" +
                   " WHERE Aportacion.ID_Afiliado = Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = " +
                   "Aportacion_Obligatoria_Especial.ID_Aportacion AND Aportacion_Obligatoria_Especial.ID_Motivo = " +
                   "Motivo.ID_Motivo AND ( Afiliado.Num_Certificado = " + num_certificados[0] + " ";
            for (int i = 1; i < num_certificados.Length; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados[i] + " ";
            QueryString += " ) AND (Aportacion.Fecha_Realizacion <= GETDATE() ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }
        private static List<String> getAportacionesV(String[] num_certificados)
        {
            if (num_certificados.Length == 0)
                return null;
            QueryString = "SELECT Afiliado.Num_Certificado,Aportacion.Monto,Aportacion.Descripcion," +
                  "'N/A' AS Fecha_Plazo,Aportacion.Fecha_Realizacion,'N/A' AS Motivo,'N/A' AS Cancelado" +
                  " FROM Afiliado,Aportacion,Aportacion_Voluntaria WHERE Aportacion.ID_Afiliado = " +
                  "Afiliado.ID_Afiliado AND Aportacion.ID_Aportacion = Aportacion_Voluntaria.ID_Aportacion AND" +
                  " (Afiliado.Num_Certificado =  " + num_certificados[0] + " ";
            for (int i = 1; i < num_certificados.Length; i++)
                QueryString += " OR Afiliado.Num_Certificado = " + num_certificados[i] + " ";
            QueryString += " ) AND (Aportacion.Fecha_Realizacion <= GETDATE() ) ORDER BY Afiliado.Num_Certificado ";
            List<String> aportaciones = new List<String>();
            if (EjecutarComando())
                while (DataReader.Read())
                {
                    String tupla = "";
                    for (int i = 0; i < DataReader.FieldCount; i++)
                        tupla += DataReader.GetValue(i).ToString().Trim() + ",";
                    aportaciones.Add(tupla.Substring(0, tupla.Length - 1));

                }
            return aportaciones;
        }

        /*METODOS ESTANDARD DEL DT*/
        /*El metodo que debe retornar todo método para saber si todo se inserto en la base de datos, o para saber
         *si un select o update tubo efecto etc.*/
        public static bool EjecutarComando()
        {
            try
            {
                DataTransaction.Connection = new SqlConnection(DataTransaction.ConnectionString);
                DataTransaction.Connection.Open();
                SqlTransaction transaccion = Connection.BeginTransaction();
                DataTransaction.Query = Connection.CreateCommand();
                DataTransaction.Query.CommandType = System.Data.CommandType.Text;
                DataTransaction.Query.Transaction = transaccion;
                DataTransaction.Query.CommandText = QueryString;
                DataTransaction.DataReader = DataTransaction.Query.ExecuteReader();
                return true;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                try
                {
                    DataTransaction.Query.Transaction.Rollback();
                    DataTransaction.Connection.Close();
                    DataTransaction.Connection.Dispose();
                    Console.WriteLine("ERROR: TRANSACTION ROLLED BACK\n" + ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: TRANSACTION NOT ROLLED BACK\n" + e.Message);
                }

                DataTransaction.DataReader = null;
                DataTransaction.Connection = null;
                DataTransaction.Query = null;
                DataTransaction.QueryString = "";
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: NO SE QUE PASO...\n" + ex.Message);
                return false;
            }
        }
        /*Comete y desconecta de la base de datos*/
        public static void FinalizarComandoCorrectamente()
        {
                DataTransaction.DataReader.Dispose();
                DataTransaction.DataReader.Close();
                DataTransaction.Query.Transaction.Commit();
                DataTransaction.DataReader.Dispose();
                DataTransaction.DataReader.Close();
                DataTransaction.Query.Connection.Dispose();
                DataTransaction.Query.Dispose();
                DataTransaction.Query.Connection.Close();
                DataTransaction.Connection.Dispose();
                DataTransaction.Connection.Close();
                DataTransaction.DataReader = null;
                DataTransaction.Connection = null;
                DataTransaction.Query = null;
                DataTransaction.QueryString = "";
        }
          /*Convierte la fecha del sistema a formato de BD con el formato: AÑO-MES-DIA*/
        public static string convertirFechaBD(string fecha)
        {
            string[] fecha_corregida = fecha.Split('/');
            fecha = "";

            fecha += fecha_corregida[2].Split(' ')[0] + "-";
            fecha += fecha_corregida[0] + "-";
            fecha += fecha_corregida[1];

            return fecha;
        }
        /*regresa el ultimo dia del mes*/
        public static String getDiaFinal(int Mes)
        {
            switch (Mes)
            {
                case 2:
                    return "28";
                case 4:
                    return "30";
                case 6:
                    return "30";
                case 9:
                    return "30";
                case 11:
                    return "30";
                default:
                    return "31";
            }
        }
/*********************************************************************************************************************************/
/***********************************************FIN-KOTARO************************************************************************/
/*********************************************************************************************************************************/
    }
}
