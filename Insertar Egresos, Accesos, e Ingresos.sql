/*INSERTAR EL ADMINISTRADOR*/
INSERT INTO Empleado( 'Nombre_Usuario','Tipo_Empleado','P_Nombre','S_Nombre','P_Apellido','S_Apellido','Password')
	VALUES( 'LilDwarf', 'Administrador (DBA)', 'Benjamin', '', 'Barahona', 'Henriquez', 'bbh753s');

/*INSERTAR LOS MODULOS DEL SISTEMA*/	
INSERT INTO Modulo VALUES('Examenes', 'Modulo para insertar los examenes patologicos y Medicos');
INSERT INTO Modulo VALUES('Control de Usuarios', 'Modulo para control de usuarios con permisos al sistema');
INSERT INTO Modulo VALUES('Contabilidad', 'Modulo para desplegar datos de ingresos y egresos');
INSERT INTO Modulo VALUES('Consultas', 'Modulo para hacer consultas a la Base de Datos');
/*AHORA LE DAMOS PERMISOS AL DBA A LOS MODULOS*/
INSERT INTO Acceso VALUES('Examenes', 'LilDwarf');
INSERT INTO Acceso VALUES('Control de Usuarios', 'LilDwarf');
INSERT INTO Acceso VALUES('Contabilidad', 'LilDwarf');
INSERT INTO Acceso VALUES('Informes', 'LilDwarf');

/*TENEMOS QUE LLENAR LAS TABLAS DE CONTABILIDAD PARA CADA MES
		HACER UN PROC. ANIDADO QUE HAGA ESTO	*/
		
/*DICIEMBRE 2011*/
INSERT INTO Egreso VALUES('Empleados', '2011', '12', '0');
INSERT INTO Egreso VALUES('Servicios', '2011', '12', '0');
INSERT INTO Egreso VALUES('Transporte', '2011', '12', '0');
INSERT INTO Egreso VALUES('Papeleria', '2011', '12', '0');
INSERT INTO Egreso VALUES('Materiales', '2011', '12', '0');
INSERT INTO Egreso VALUES('Mantenimiento Local', '2011', '12', '0');
INSERT INTO Egreso VALUES('Mantenimiento Equipo', '2011', '12', '0');
INSERT INTO Egreso VALUES('Jardineria', '2011', '12', '0');
INSERT INTO Egreso VALUES('Otros', '2011', '12', '0');
/*ENERO 2012*/
INSERT INTO Egreso VALUES('Empleados', '2012', '1', '0');
INSERT INTO Egreso VALUES('Servicios', '2012', '1', '0');
INSERT INTO Egreso VALUES('Transporte', '2012', '1', '0');
INSERT INTO Egreso VALUES('Papeleria', '2012', '1', '0');
INSERT INTO Egreso VALUES('Materiales', '2012', '1', '0');
INSERT INTO Egreso VALUES('Mantenimiento Local', '2012', '1', '0');
INSERT INTO Egreso VALUES('Mantenimiento Equipo', '2012', '1', '0');
INSERT INTO Egreso VALUES('Jardineria', '2012', '1', '0');
INSERT INTO Egreso VALUES('Otros', '2012', '1', '0');

/*INGRESOS*/
INSERT INTO Ingreso VALUES('Ingresos por Examen', '2012', '1', '0');