USE [msdb]
GO

/****** Object:  Job [CrearContabilidadMensual]    Script Date: 01/15/2012 13:52:29 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 01/15/2012 13:52:29 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'CrearContabilidadMensual', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'OptimusPrime\Ben', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [InsertingContabilidad]    Script Date: 01/15/2012 13:52:29 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'InsertingContabilidad', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=3, 
		@retry_interval=10, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'INSERT INTO Contabilidad VALUES(''Ingresos por Examen'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Empleados'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Servicios'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Transporte'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Papeleria'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Materiales'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Mantenimiento Local'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Mantenimiento Equipo'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Jardineria'', YEAR(GETDATE()), MONTH(GETDATE()));
INSERT INTO Contabilidad VALUES(''Otros'', YEAR(GETDATE()), MONTH(GETDATE()));

INSERT INTO Egreso VALUES(''Empleados'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Servicios'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Transporte'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Papeleria'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Materiales'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Mantenimiento Local'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Mantenimiento Equipo'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Jardineria'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');
INSERT INTO Egreso VALUES(''Otros'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');

INSERT INTO Ingreso VALUES(''Ingresos por Examen'', YEAR(GETDATE()), MONTH(GETDATE()), ''0'');', 
		@database_name=N'OdessaPatLab_DB', 
		@flags=4
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Insert Contabilidades Schedule', 
		@enabled=1, 
		@freq_type=16, 
		@freq_interval=1, 
		@freq_subday_type=1, 
		@freq_subday_interval=0, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=1, 
		@active_start_date=20120110, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'9bb90da7-8aa8-4d4a-86f5-9e57eef5bb37'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


