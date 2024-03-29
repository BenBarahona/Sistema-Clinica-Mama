USE [OdessaPatLab_DB]
GO
/****** Object:  Table [dbo].[Contabilidad]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contabilidad](
	[Nombre_contable] [nvarchar](50) NOT NULL,
	[Anio_contable] [int] NOT NULL,
	[Mes_contable] [int] NOT NULL,
 CONSTRAINT [PK_Contabilidad] PRIMARY KEY CLUSTERED 
(
	[Nombre_contable] ASC,
	[Anio_contable] ASC,
	[Mes_contable] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleado]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleado](
	[Nombre_Usuario] [nvarchar](50) NOT NULL,
	[Tipo_Empleado] [nvarchar](50) NULL,
	[P_Nombre] [nvarchar](50) NULL,
	[S_Nombre] [nvarchar](50) NULL,
	[P_Apellido] [nvarchar](50) NULL,
	[S_Apellido] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Correo] [nvarchar](50) NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[Nombre_Usuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paciente]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paciente](
	[ID_paciente] [int] IDENTITY(1,1) NOT NULL,
	[DIU] [bit] NULL,
	[FUR] [nvarchar](50) NULL,
	[FUP] [nvarchar](50) NULL,
	[P_Nombre] [nvarchar](50) NULL,
	[S_Nombre] [nvarchar](50) NULL,
	[P_Apellido] [nvarchar](50) NULL,
	[S_Apellido] [nvarchar](50) NULL,
	[Edad] [int] NULL,
	[Expediente] [nvarchar](50) NULL,
	[Anticonceptivos_Orales] [bit] NULL,
 CONSTRAINT [PK_Paciente] PRIMARY KEY CLUSTERED 
(
	[ID_paciente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modulo]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modulo](
	[Nombre_Modulo] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](500) NULL,
 CONSTRAINT [PK_Modulo] PRIMARY KEY CLUSTERED 
(
	[Nombre_Modulo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medico]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medico](
	[ID_Medico] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[telefono] [nvarchar](50) NULL,
	[celular] [nvarchar](50) NULL,
	[direccion] [nvarchar](255) NULL,
	[compania] [nvarchar](50) NULL,
	[numeroColegiatura] [nvarchar](50) NULL,
 CONSTRAINT [PK_Medico] PRIMARY KEY CLUSTERED 
(
	[ID_Medico] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[Material_Enviado] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[Material_Enviado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingreso]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingreso](
	[Nombre_contable] [nvarchar](50) NOT NULL,
	[Anio_contable] [int] NOT NULL,
	[Mes_contable] [int] NOT NULL,
	[Total_Ingreso] [int] NULL,
 CONSTRAINT [PK_Ingreso] PRIMARY KEY CLUSTERED 
(
	[Nombre_contable] ASC,
	[Anio_contable] ASC,
	[Mes_contable] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Acceso]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acceso](
	[Nombre_Modulo] [nvarchar](50) NOT NULL,
	[Nombre_Usuario] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Acceso] PRIMARY KEY CLUSTERED 
(
	[Nombre_Modulo] ASC,
	[Nombre_Usuario] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Egreso]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Egreso](
	[Nombre_contable] [nvarchar](50) NOT NULL,
	[Anio_contable] [int] NOT NULL,
	[Mes_contable] [int] NOT NULL,
	[Total_Egreso] [int] NULL,
 CONSTRAINT [PK_Egreso] PRIMARY KEY CLUSTERED 
(
	[Nombre_contable] ASC,
	[Anio_contable] ASC,
	[Mes_contable] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gasto]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gasto](
	[Nombre_contable] [nvarchar](50) NOT NULL,
	[Anio_contable] [int] NOT NULL,
	[Mes_contable] [int] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[Fecha_Exacta] [date] NULL,
	[Valor] [int] NULL,
 CONSTRAINT [PK_Gasto] PRIMARY KEY CLUSTERED 
(
	[Nombre_contable] ASC,
	[Anio_contable] ASC,
	[Mes_contable] ASC,
	[Descripcion] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Examen]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Examen](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Fecha_Examen] [date] NULL,
	[Precio] [int] NULL,
	[Diagnostico] [nvarchar](max) NULL,
	[Diagnostico_Medico] [nvarchar](500) NULL,
	[Registrado_por_empleado] [nvarchar](50) NULL,
	[ID_Paciente] [int] NULL,
	[ID_Medico] [int] NULL,
	[Nombre_contable] [nvarchar](50) NULL,
	[Anio_contable] [int] NULL,
	[Mes_contable] [int] NULL,
	[Fecha_Informe] [nvarchar](100) NULL,
	[Fecha] [date] NULL,
	[Fecha_Recibido] [nvarchar](100) NULL,
 CONSTRAINT [PK_Examen] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citologia]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citologia](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Comentario] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Citologia] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Biopsia]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Biopsia](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Descripcion_Macroscopica] [nvarchar](max) NULL,
	[Descripcion_Microscopica] [nvarchar](max) NULL,
	[Codificacion] [nvarchar](50) NULL,
 CONSTRAINT [PK_Biopsia] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialEnviadoBiopsia]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialEnviadoBiopsia](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Material_Enviado] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_MaterialEnviadoBiopsia] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC,
	[Material_Enviado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[No_Ginecologica]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[No_Ginecologica](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Descripcion_Macroscopica] [nvarchar](max) NULL,
	[Descripcion_Microscopica] [nvarchar](max) NULL,
 CONSTRAINT [PK_No_Ginecologica] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ginecologica]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ginecologica](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Inflamacion] [nvarchar](10) NULL,
	[Calidad_del_Frotis_Causa] [nvarchar](50) NULL,
	[Calidad_del_Frotis_Adecuado] [bit] NULL,
	[Anticonceptivos_Orales] [bit] NULL,
	[Candida_SP] [bit] NULL,
	[Agentes_Infecciosos_Gardnerella] [bit] NULL,
	[Agentes_Infecciosos_Vaginosis] [bit] NULL,
	[Agentes_Infecciosos_Herpes] [bit] NULL,
	[Agentes_Infecciosos_Tricomonas] [bit] NULL,
	[Agentes_Infecciosos_Otro] [nvarchar](100) NULL,
	[Evaluacion_Hormonal_Basales] [int] NULL,
	[Evaluacion_Hormonal_Intermedias] [int] NULL,
	[Evaluacion_Hormonal_Superficiales] [int] NULL,
	[Recomendaciones_Colposcopia] [bit] NULL,
	[Recomendaciones_Repetir] [int] NULL,
	[Recomendaciones_Otra] [nvarchar](255) NULL,
	[Recomendaciones_Biopsia] [bit] NULL,
	[Recomendaciones__tratamiento] [bit] NULL,
	[Diagnostico_NIC_I] [bit] NULL,
	[Diagnostico_NIC_II] [bit] NULL,
	[Diagnostico_NIC_III] [bit] NULL,
	[Celula_Atipica] [bit] NULL,
	[LesionEscamosa_BajoGrado] [bit] NULL,
	[LesionEscamosa_AltoGrado] [bit] NULL,
	[Origen_muestra] [nvarchar](500) NULL,
	[Diagnostico_Negativo_por_malignidad] [bit] NULL,
	[Diagnostico_Infecciones_VPH] [bit] NULL,
	[Diagnostico_Glandular] [bit] NULL,
	[Diagnostico_Escamosa] [bit] NULL,
	[Diagnostico_Adenocarcinoma] [bit] NULL,
	[Diagnostico_Carcinoma_Celula] [bit] NULL,
 CONSTRAINT [PK_Ginecologica] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialEnviadoCitologia]    Script Date: 02/12/2012 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialEnviadoCitologia](
	[Codigo_Examen] [nvarchar](10) NOT NULL,
	[Material_Enviado] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_MaterialEnviadoCitologia] PRIMARY KEY CLUSTERED 
(
	[Codigo_Examen] ASC,
	[Material_Enviado] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Ingreso_Contabilidad]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Ingreso]  WITH CHECK ADD  CONSTRAINT [FK_Ingreso_Contabilidad] FOREIGN KEY([Nombre_contable], [Anio_contable], [Mes_contable])
REFERENCES [dbo].[Contabilidad] ([Nombre_contable], [Anio_contable], [Mes_contable])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ingreso] CHECK CONSTRAINT [FK_Ingreso_Contabilidad]
GO
/****** Object:  ForeignKey [FK_Acceso_Empleado]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Acceso]  WITH CHECK ADD  CONSTRAINT [FK_Acceso_Empleado] FOREIGN KEY([Nombre_Usuario])
REFERENCES [dbo].[Empleado] ([Nombre_Usuario])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Acceso] CHECK CONSTRAINT [FK_Acceso_Empleado]
GO
/****** Object:  ForeignKey [FK_Acceso_Modulo]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Acceso]  WITH CHECK ADD  CONSTRAINT [FK_Acceso_Modulo] FOREIGN KEY([Nombre_Modulo])
REFERENCES [dbo].[Modulo] ([Nombre_Modulo])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Acceso] CHECK CONSTRAINT [FK_Acceso_Modulo]
GO
/****** Object:  ForeignKey [FK_Egreso_Contabilidad]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Egreso]  WITH CHECK ADD  CONSTRAINT [FK_Egreso_Contabilidad] FOREIGN KEY([Nombre_contable], [Anio_contable], [Mes_contable])
REFERENCES [dbo].[Contabilidad] ([Nombre_contable], [Anio_contable], [Mes_contable])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Egreso] CHECK CONSTRAINT [FK_Egreso_Contabilidad]
GO
/****** Object:  ForeignKey [FK_Gasto_Egreso]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Gasto]  WITH CHECK ADD  CONSTRAINT [FK_Gasto_Egreso] FOREIGN KEY([Nombre_contable], [Anio_contable], [Mes_contable])
REFERENCES [dbo].[Egreso] ([Nombre_contable], [Anio_contable], [Mes_contable])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Gasto] CHECK CONSTRAINT [FK_Gasto_Egreso]
GO
/****** Object:  ForeignKey [FK_Examen_Empleado]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Examen]  WITH CHECK ADD  CONSTRAINT [FK_Examen_Empleado] FOREIGN KEY([Registrado_por_empleado])
REFERENCES [dbo].[Empleado] ([Nombre_Usuario])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Examen] CHECK CONSTRAINT [FK_Examen_Empleado]
GO
/****** Object:  ForeignKey [FK_Examen_Ingreso]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Examen]  WITH CHECK ADD  CONSTRAINT [FK_Examen_Ingreso] FOREIGN KEY([Nombre_contable], [Anio_contable], [Mes_contable])
REFERENCES [dbo].[Ingreso] ([Nombre_contable], [Anio_contable], [Mes_contable])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Examen] CHECK CONSTRAINT [FK_Examen_Ingreso]
GO
/****** Object:  ForeignKey [FK_Examen_Medico]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Examen]  WITH CHECK ADD  CONSTRAINT [FK_Examen_Medico] FOREIGN KEY([ID_Medico])
REFERENCES [dbo].[Medico] ([ID_Medico])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Examen] CHECK CONSTRAINT [FK_Examen_Medico]
GO
/****** Object:  ForeignKey [FK_Examen_Paciente]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Examen]  WITH CHECK ADD  CONSTRAINT [FK_Examen_Paciente] FOREIGN KEY([ID_Paciente])
REFERENCES [dbo].[Paciente] ([ID_paciente])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Examen] CHECK CONSTRAINT [FK_Examen_Paciente]
GO
/****** Object:  ForeignKey [FK_Citologia_Examen]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Citologia]  WITH CHECK ADD  CONSTRAINT [FK_Citologia_Examen] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[Examen] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Citologia] CHECK CONSTRAINT [FK_Citologia_Examen]
GO
/****** Object:  ForeignKey [FK_Biopsia_Examen]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Biopsia]  WITH CHECK ADD  CONSTRAINT [FK_Biopsia_Examen] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[Examen] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Biopsia] CHECK CONSTRAINT [FK_Biopsia_Examen]
GO
/****** Object:  ForeignKey [FK_MaterialEnviadoBiopsia_Biopsia]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[MaterialEnviadoBiopsia]  WITH CHECK ADD  CONSTRAINT [FK_MaterialEnviadoBiopsia_Biopsia] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[Biopsia] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaterialEnviadoBiopsia] CHECK CONSTRAINT [FK_MaterialEnviadoBiopsia_Biopsia]
GO
/****** Object:  ForeignKey [FK_MaterialEnviadoBiopsia_Material]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[MaterialEnviadoBiopsia]  WITH CHECK ADD  CONSTRAINT [FK_MaterialEnviadoBiopsia_Material] FOREIGN KEY([Material_Enviado])
REFERENCES [dbo].[Material] ([Material_Enviado])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaterialEnviadoBiopsia] CHECK CONSTRAINT [FK_MaterialEnviadoBiopsia_Material]
GO
/****** Object:  ForeignKey [FK_No_Ginecologica_Citologia]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[No_Ginecologica]  WITH CHECK ADD  CONSTRAINT [FK_No_Ginecologica_Citologia] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[Citologia] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[No_Ginecologica] CHECK CONSTRAINT [FK_No_Ginecologica_Citologia]
GO
/****** Object:  ForeignKey [FK_Ginecologica_Citologia]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[Ginecologica]  WITH CHECK ADD  CONSTRAINT [FK_Ginecologica_Citologia] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[Citologia] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ginecologica] CHECK CONSTRAINT [FK_Ginecologica_Citologia]
GO
/****** Object:  ForeignKey [FK_MaterialEnviadoCitologia_Material]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[MaterialEnviadoCitologia]  WITH CHECK ADD  CONSTRAINT [FK_MaterialEnviadoCitologia_Material] FOREIGN KEY([Material_Enviado])
REFERENCES [dbo].[Material] ([Material_Enviado])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaterialEnviadoCitologia] CHECK CONSTRAINT [FK_MaterialEnviadoCitologia_Material]
GO
/****** Object:  ForeignKey [FK_MaterialEnviadoCitologia_No_Ginecologica]    Script Date: 02/12/2012 19:29:35 ******/
ALTER TABLE [dbo].[MaterialEnviadoCitologia]  WITH CHECK ADD  CONSTRAINT [FK_MaterialEnviadoCitologia_No_Ginecologica] FOREIGN KEY([Codigo_Examen])
REFERENCES [dbo].[No_Ginecologica] ([Codigo_Examen])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MaterialEnviadoCitologia] CHECK CONSTRAINT [FK_MaterialEnviadoCitologia_No_Ginecologica]
GO
s


/*INSERTAR EL ADMINISTRADOR*/
INSERT INTO Empleado VALUES( 'LilDwarf', 'Administrador (DBA)', 'Benjamin', '', 'Barahona', 'Henriquez', 'bbh753s', 'supbanjo01@hotmail.com');

/*INSERTAR LOS MODULOS DEL SISTEMA*/	
INSERT INTO Modulo VALUES('Examenes', 'Modulo para insertar los examenes patologicos y Medicos');
INSERT INTO Modulo VALUES('Control De Usuarios', 'Modulo para control de usuarios con permisos al sistema');
INSERT INTO Modulo VALUES('Ingresos', 'Modulo para desplegar datos de ingresos');
INSERT INTO Modulo VALUES('Egresos', 'Modulo para desplegar datos de egresos (Gastos)');
INSERT INTO Modulo VALUES('Consultas', 'Modulo para hacer consultas a la Base de Datos');
/*AHORA LE DAMOS PERMISOS AL DBA A LOS MODULOS*/
INSERT INTO Acceso VALUES('Examenes', 'LilDwarf');
INSERT INTO Acceso VALUES('Control de Usuarios', 'LilDwarf');
INSERT INTO Acceso VALUES('Ingresos', 'LilDwarf');
INSERT INTO Acceso VALUES('Egresos', 'LilDwarf');
INSERT INTO Acceso VALUES('Consultas', 'LilDwarf');

/*TENEMOS QUE LLENAR LAS TABLAS DE CONTABILIDAD PARA CADA MES
		HACER UN PROC. ANIDADO QUE HAGA ESTO	*/

/*DICIEMBRE 2011*/
INSERT INTO Contabilidad VALUES('Empleados', '2011', '12');
INSERT INTO Contabilidad VALUES('Servicios', '2011', '12');
INSERT INTO Contabilidad VALUES('Transporte', '2011', '12');
INSERT INTO Contabilidad VALUES('Papeleria', '2011', '12');
INSERT INTO Contabilidad VALUES('Materiales', '2011', '12');
INSERT INTO Contabilidad VALUES('Mantenimiento Local', '2011', '12');
INSERT INTO Contabilidad VALUES('Mantenimiento Equipo', '2011', '12');
INSERT INTO Contabilidad VALUES('Jardineria', '2011', '12');
INSERT INTO Contabilidad VALUES('Otros', '2011', '12');
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
INSERT INTO Contabilidad VALUES('Empleados', '2012', '1');
INSERT INTO Contabilidad VALUES('Servicios', '2012', '1');
INSERT INTO Contabilidad VALUES('Transporte', '2012', '1');
INSERT INTO Contabilidad VALUES('Papeleria', '2012', '1');
INSERT INTO Contabilidad VALUES('Materiales', '2012', '1');
INSERT INTO Contabilidad VALUES('Mantenimiento Local', '2012', '1');
INSERT INTO Contabilidad VALUES('Mantenimiento Equipo', '2012', '1');
INSERT INTO Contabilidad VALUES('Jardineria', '2012', '1');
INSERT INTO Contabilidad VALUES('Otros', '2012', '1');
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
INSERT INTO Contabilidad VALUES('Ingresos por Examen', '2012', '1');
INSERT INTO Ingreso VALUES('Ingresos por Examen', '2012', '1', '0');