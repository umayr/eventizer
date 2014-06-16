USE [master]
GO
/****** Object:  Database [eventizer]    Script Date: 6/14/2014 8:56:26 PM ******/
CREATE DATABASE [eventizer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eventizer', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\eventizer.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'eventizer_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\eventizer_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [eventizer] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eventizer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [eventizer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [eventizer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [eventizer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [eventizer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [eventizer] SET ARITHABORT OFF 
GO
ALTER DATABASE [eventizer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [eventizer] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [eventizer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [eventizer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [eventizer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [eventizer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [eventizer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [eventizer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [eventizer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [eventizer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [eventizer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [eventizer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [eventizer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [eventizer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [eventizer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [eventizer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [eventizer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [eventizer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [eventizer] SET RECOVERY FULL 
GO
ALTER DATABASE [eventizer] SET  MULTI_USER 
GO
ALTER DATABASE [eventizer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [eventizer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [eventizer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [eventizer] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'eventizer', N'ON'
GO
USE [eventizer]
GO
/****** Object:  StoredProcedure [dbo].[usp_add_asset]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a new asset
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_asset] 
	@name varchar(50),
	@type varchar(50),
	@created_by int
AS
BEGIN
	
	SET NOCOUNT OFF;

    insert into assets values(@name,@type,getdate(), @created_by);
END

GO
/****** Object:  StoredProcedure [dbo].[usp_add_asset_to_subtask]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a existing asset to a subtask
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_asset_to_subtask]
	@subtask_id int,
	@asset_id int,
	@quantity int
AS
BEGIN
	
	SET NOCOUNT ON;

    insert into subtasks_assets values(@subtask_id, @asset_id, @quantity);
END

GO
/****** Object:  StoredProcedure [dbo].[usp_add_event]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a new Event
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_event]
	@name varchar(50),
	@created_by	int,
	@description text,
	@deadline datetime
AS
BEGIN
	
	SET NOCOUNT OFF;

    insert into events values(@name,@description,@created_by,GETDATE(),@deadline,default);
	select SCOPE_IDENTITY() as id; 
END

GO
/****** Object:  StoredProcedure [dbo].[usp_add_subtask]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a new Subtask
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_subtask] 
	@name varchar(50),
	@created_by	int,
	@description text,
	@labours_required int,
	@deadline datetime,
	@assigned_to int
AS
BEGIN
	
	SET NOCOUNT OFF;

    insert into subtasks values(@name,GETDATE(),@created_by,@description,@labours_required,@deadline,default,@assigned_to);
	select SCOPE_IDENTITY() as id; 
END

GO
/****** Object:  StoredProcedure [dbo].[usp_add_subtask_to_task]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a existing subtask to a task
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_subtask_to_task]
	@subtask_id int,
	@task_id int
AS
BEGIN
	
	SET NOCOUNT ON;

    insert into task_subtasks values(@task_id,@subtask_id);
END

GO
/****** Object:  StoredProcedure [dbo].[usp_add_task]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a new Task
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_task] 
	@name varchar(50),
	@created_by	int,
	@description text,
	@deadline datetime,
	@assigned_to int
AS
BEGIN
	
	SET NOCOUNT OFF;
    insert into tasks values(@name,@description,@created_by,GETDATE(),@deadline,default,@assigned_to);
	select SCOPE_IDENTITY() as id; 
END


GO
/****** Object:  StoredProcedure [dbo].[usp_add_task_to_event]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/13/2014
-- Description:	Add a existing task to an event
-- =============================================
CREATE PROCEDURE [dbo].[usp_add_task_to_event]
	@task_id int,
	@event_id int
AS
BEGIN
	
	SET NOCOUNT OFF;

    insert into event_tasks values(@event_id,@task_id);
END

GO
/****** Object:  StoredProcedure [dbo].[usp_get_employee_by_email]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch an employee w.r.t his/her ID
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_employee_by_email] 
	-- Add the parameters for the stored procedure here
	@email varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from vw_all_employees where email = @email
END

GO
/****** Object:  StoredProcedure [dbo].[usp_get_employee_by_id]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch an employee w.r.t his/her ID
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_employee_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from vw_all_employees where id = @id
END

GO
/****** Object:  StoredProcedure [dbo].[usp_get_event_by_id]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch an event w.r.t its ID
-- =============================================
create PROCEDURE [dbo].[usp_get_event_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	select * from vw_all_events where id = @id;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_get_subtask_by_id]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch a subtask w.r.t its ID
-- =============================================
Create PROCEDURE [dbo].[usp_get_subtask_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	select * from subtasks where id = @id;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_get_task_by_id]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch a tasl w.r.t its ID
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_task_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	select * from vw_all_tasks where id = @id;
END

GO
/****** Object:  StoredProcedure [dbo].[usp_login_employee]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Procedure to login employee.
-- =============================================
CREATE PROCEDURE [dbo].[usp_login_employee]
	
	@email varchar(50),
	@password varchar(200)

AS
BEGIN
	SET NOCOUNT ON;
	declare @return int;
	if (select password from emp_basic where email = @email) = @password
	begin
		set @return = 1;
		return @return;
	end
	else
	begin
		set @return = 0;
		return @return;
	end	
END

GO
/****** Object:  StoredProcedure [dbo].[usp_register_employee]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Procedure to register employee.
-- =============================================
CREATE PROCEDURE [dbo].[usp_register_employee]
	@name varchar(50),
	@email varchar(50),
	@password varchar(200),
	@phone bigint,
	@designation varchar(50),
	@manager_id int
AS
BEGIN
	SET NOCOUNT OFF;
	Declare @id int;

    insert into emp_basic values(@name, @email, @password);
	select @id = (select id from emp_basic where email = @email);
	insert into emp_details values(@id, @phone, @designation, @manager_id, SYSDATETIME());

END

GO
/****** Object:  Table [dbo].[assets]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[assets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[type] [varchar](50) NOT NULL,
	[created_on] [datetime] NOT NULL,
	[created_by] [int] NOT NULL,
 CONSTRAINT [PK_assets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[emp_basic]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[emp_basic](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[password] [varchar](200) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[emp_details]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[emp_details](
	[id] [varchar](20) NOT NULL,
	[phone] [bigint] NOT NULL,
	[designation] [varchar](50) NOT NULL,
	[manager_id] [int] NOT NULL,
	[timestamp] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[event_tasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[event_tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[event_id] [int] NOT NULL,
	[task_id] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[events]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[events](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [text] NOT NULL,
	[created_by] [int] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[deadline] [datetime] NOT NULL,
	[status] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[subtask_assets]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[subtask_assets](
	[subtask_id] [int] NOT NULL,
	[asset_id] [int] NOT NULL,
	[quantity] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[subtasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[subtasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[created_by] [int] NOT NULL,
	[description] [text] NOT NULL,
	[labours_required] [int] NOT NULL,
	[deadline] [datetime] NOT NULL,
	[status] [bit] NOT NULL,
	[assigned_to] [int] NOT NULL,
 CONSTRAINT [PK_subtasks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[task_subtasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task_subtasks](
	[task_id] [int] NOT NULL,
	[subtask_id] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tasks](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[description] [text] NOT NULL,
	[created_by] [int] NOT NULL,
	[date_created] [datetime] NOT NULL,
	[deadline] [datetime] NOT NULL,
	[status] [bit] NULL,
	[assigned_to] [int] NOT NULL,
 CONSTRAINT [PK_tasks] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vw_all_employees]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_all_employees]
AS
SELECT        dbo.emp_basic.id, dbo.emp_basic.name, dbo.emp_basic.email, dbo.emp_basic.password, dbo.emp_details.phone, dbo.emp_details.designation, 
                         dbo.emp_details.manager_id
FROM            dbo.emp_basic INNER JOIN
                         dbo.emp_details ON dbo.emp_basic.id = dbo.emp_details.id

GO
/****** Object:  View [dbo].[vw_employees_names]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_employees_names]
AS
SELECT        name, id
FROM            dbo.vw_all_employees

GO
/****** Object:  View [dbo].[vw_all_events]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE view [dbo].[vw_all_events]
as

SELECT DISTINCT events.id
	,name
	,cast(description AS NVARCHAR(1000)) as description
	,created_by
	,date_created
	,deadline
	,status
	,ISNULL((
		SELECT cast(task_id AS VARCHAR(5)) + ','
		FROM event_tasks
		INNER JOIN events e ON events.id = event_tasks.event_id
		WHERE event_tasks.event_id = e.id
		FOR XML PATH('')
		),'0,') AS tasks
FROM events
LEFT JOIN event_tasks ON events.id = event_tasks.event_id






GO
/****** Object:  View [dbo].[vw_all_subtasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE view [dbo].[vw_all_subtasks]
as
SELECT DISTINCT subtasks.id
	,name
	,cast(description AS NVARCHAR(1000)) as description
	,created_by
	,date_created
	,deadline
	,status
	,labours_required
	,assigned_to
	,ISNULL((
		SELECT cast(subtask_id AS VARCHAR(5)) + ','
		FROM subtask_assets
		INNER JOIN subtasks S ON subtasks.id = subtask_assets.subtask_id
		WHERE subtask_assets.subtask_id = S.id
		FOR XML PATH('')
		),'0,') AS assets
FROM subtasks
LEFT JOIN subtask_assets ON subtasks.id = subtask_assets.subtask_id






GO
/****** Object:  View [dbo].[vw_all_tasks]    Script Date: 6/14/2014 8:56:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[vw_all_tasks]
as
SELECT DISTINCT tasks.id
	,name
	,cast(description AS NVARCHAR(1000)) as description
	,created_by
	,date_created
	,deadline
	,status
	,assigned_to
	,ISNULL((
		SELECT cast(subtask_id AS VARCHAR(5)) + ','
		FROM task_subtasks
		INNER JOIN tasks T ON tasks.id = task_subtasks.task_id
		WHERE task_subtasks.task_id = T.id
		FOR XML PATH('')
		),'0,') AS subtasks
FROM tasks
LEFT JOIN task_subtasks ON tasks.id = task_subtasks.task_id





GO
SET IDENTITY_INSERT [dbo].[emp_basic] ON 

GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (1, N'Munizaza', N'mini-za@lol.io', N'4d13fcc6eda389d4d679602171e11593eadae9b9')
GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (2, N'Umayr Shahid', N'umayr@lol.io', N'3fd1a68cbd3dd7a289db04662c730afd0e9fb175')
GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (3, N'Zahra Shahzad', N'zararara@lol.io', N'403926033d001b5279df37cbbe5287b7c7c267fa')
GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (4, N'Bashira', N'bash@lol.io', N'da39a3ee5e6b4b0d3255bfef95601890afd80709')
GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (5, N'Lalarukh', N'lala@lol.io', N'7e59bef51f489818fc6410f6d275765e93ec960d')
GO
SET IDENTITY_INSERT [dbo].[emp_basic] OFF
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'1', 3458966352, N'Manager', 0, CAST(0x0000A347001E650D AS DateTime))
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'2', 3458933015, N'Manager', 0, CAST(0x0000A347001F299E AS DateTime))
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'3', 3465995600, N'Supervisor', 2, CAST(0x0000A34700202F6A AS DateTime))
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'4', 0, N'Supervisor', 1, CAST(0x0000A3470044B675 AS DateTime))
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'5', 3458955252, N'Supervisor', 2, CAST(0x0000A347004687F7 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[event_tasks] ON 

GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (1, 1, 1)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (2, 1, 3)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (3, 2, 3)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (4, 3, 1)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (5, 3, 2)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (6, 3, 3)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (7, 10, 8)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (8, 10, 9)
GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (9, 6, 10)
GO
SET IDENTITY_INSERT [dbo].[event_tasks] OFF
GO
SET IDENTITY_INSERT [dbo].[events] ON 

GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (1, N'Zahra`s wedding', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut.', 5, CAST(0x0000A34900D7C844 AS DateTime), CAST(0x0000A34900000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (2, N'Health Conference', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut.', 5, CAST(0x0000A34900D7D1DC AS DateTime), CAST(0x0000A38600000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (3, N'Biology Seminar', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut.', 5, CAST(0x0000A34900D82AEE AS DateTime), CAST(0x0000A34F00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (4, N'Birthday Ceremony', N'Vestibulum ut vestibulum quam. Sed arcu purus, consequat at ipsum eu, ullamcorper placerat lectus. Mauris scelerisque dolor in justo mattis iaculis. Duis gravida massa consequat ante congue hendrerit. Nunc id massa lacinia massa congue eleifend. Nam ultrices mauris', 5, CAST(0x0000A34A00151A58 AS DateTime), CAST(0x0000A35A00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (5, N'Beach Party', N'Aliquam erat volutpat. Donec feugiat velit velit, vitae ornare felis suscipit tempus. In cursus tristique nisl sed faucibus. Curabitur eget nisl ut diam adipiscing fringilla. Maecenas tristique, turpis non semper tincidunt, orci libero hendrerit risus, nec volutpat erat nisi et ante.', 5, CAST(0x0000A34A00448C23 AS DateTime), CAST(0x0000A35A00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (6, N'Pool Party', N'Nulla placerat tortor metus, sed bibendum ipsum feugiat a. Mauris velit turpis, bibendum sed vestibulum sed, tempor in massa. Aliquam erat volutpat. Fusce sagittis id dui eu iaculis. Morbi at dictum nibh. Aliquam vulputate nisi ligula, eget vestibulum magna dapibus sit amet. Integer non mi lacus. Vivamus euismod quam erat. Cras sed sem sed lectus vestibulum convallis eu condimentum purus.', 5, CAST(0x0000A34A0047D26E AS DateTime), CAST(0x0000A34C00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (10, N'xbox360 Spree', N'Mauris velit ligula, consequat id euismod ac, consequat ac lacus. Mauris tempor est sit amet massa tempus tempus. Fusce ultrices metus sapien, nec ultrices est porttitor non. Aenean consectetur malesuada dolor, at dictum orci elementum et. Phasellus vitae suscipit nisl, nec aliquam mi. Donec egestas metus a nisl egestas, in gravida augue dictum. Aenean in neque at est mollis iaculis.', 5, CAST(0x0000A34A00499BAC AS DateTime), CAST(0x0000A34C00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (12, N'Orgy', N'Curabitur consequat semper nisl eu adipiscing. Pellentesque iaculis sit amet elit at dictum. Aenean dapibus et dui eu bibendum. Duis sagittis laoreet sapien, non imperdiet ipsum rutrum a. Ut mi diam, consequat a pretium id, ullamcorper et nisi. Aenean commodo tellus ut consequat varius.', 5, CAST(0x0000A34A004AA8F4 AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (13, N'Threesome', N'Nam id posuere nisl. Vestibulum ac sapien ultricies mi laoreet placerat id ac nisi. Aliquam pellentesque libero scelerisque massa congue, non mattis augue dapibus. In est turpis, dapibus a mattis a, aliquam quis enim. Nullam eleifend sodales luctus. Sed bibendum tempus est.', 5, CAST(0x0000A34A004BA718 AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0)
GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (11, N'Asthema', N'Slacerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum.', 2, CAST(0x0000A34A004A5912 AS DateTime), CAST(0x0000A35500000000 AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[events] OFF
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (3, 1, 100)
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (3, 2, 50)
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (1, 3, 10)
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (1, 1, 10)
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (2, 2, 5)
GO
SET IDENTITY_INSERT [dbo].[subtasks] ON 

GO
INSERT [dbo].[subtasks] ([id], [name], [date_created], [created_by], [description], [labours_required], [deadline], [status], [assigned_to]) VALUES (1, N'Clean Dishes', CAST(0x0000A349014A9B6F AS DateTime), 1, N'Adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum.', 10, CAST(0x0000A34B00000000 AS DateTime), 0, 5)
GO
INSERT [dbo].[subtasks] ([id], [name], [date_created], [created_by], [description], [labours_required], [deadline], [status], [assigned_to]) VALUES (2, N'Distribute Cards', CAST(0x0000A349014AC6A2 AS DateTime), 3, N'Sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum.', 4, CAST(0x0000A34B00000000 AS DateTime), 0, 4)
GO
INSERT [dbo].[subtasks] ([id], [name], [date_created], [created_by], [description], [labours_required], [deadline], [status], [assigned_to]) VALUES (3, N'Get Food', CAST(0x0000A349014B7A8A AS DateTime), 4, N'Sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum.', 15, CAST(0x0000A35500000000 AS DateTime), 0, 1)
GO
INSERT [dbo].[subtasks] ([id], [name], [date_created], [created_by], [description], [labours_required], [deadline], [status], [assigned_to]) VALUES (4, N'Asthema', CAST(0x0000A349014DEC5A AS DateTime), 2, N'Slacerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum.', 15, CAST(0x0000A35500000000 AS DateTime), 0, 2)
GO
SET IDENTITY_INSERT [dbo].[subtasks] OFF
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (1, 3)
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (1, 2)
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (2, 3)
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (2, 1)
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (3, 3)
GO
SET IDENTITY_INSERT [dbo].[tasks] ON 

GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (1, N'Catering Arrangements', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.', 3, CAST(0x0000A349012D3EEA AS DateTime), CAST(0x0000A35300D7C844 AS DateTime), 0, 5)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (2, N'Music Band', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus.', 3, CAST(0x0000A349012D5AA0 AS DateTime), CAST(0x0000A349013EC540 AS DateTime), 0, 4)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (3, N'Repairing old assets', N'Lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut.', 2, CAST(0x0000A349012D9C09 AS DateTime), CAST(0x0000A34C00D7C844 AS DateTime), 0, 5)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (4, N'test task', N'some description', 1, CAST(0x0000A34A001C65DF AS DateTime), CAST(0x0000A34A00000000 AS DateTime), 0, 3)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (5, N'test task II', N'some description', 1, CAST(0x0000A34A001C8144 AS DateTime), CAST(0x0000A34A00000000 AS DateTime), 0, 3)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (6, N'Find a girl', N'Ut bibendum, felis a tincidunt vehicula, mauris elit suscipit odio, vitae hendrerit eros dui ac ligula. Mauris ut neque nisi. Vestibulum imperdiet nunc vel risus vestibulum laoreet. Nam at orci ac ante luctus tristique. Cras vulputate rutrum fermentum. Proin lacinia placerat dignissim. Phasellus hendrerit mi et viverra eleifend.', 5, CAST(0x0000A34A00FBA30D AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0, 0)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (7, N'Get one controller', N'Ut bibendum, felis a tincidunt vehicula, mauris elit suscipit odio, vitae hendrerit eros dui ac ligula. Mauris ut neque nisi. Vestibulum imperdiet nunc vel risus vestibulum laoreet. Nam at orci ac ante luctus tristique. Cras vulputate rutrum fermentum. Proin lacinia placerat dignissim. Phasellus hendrerit mi et viverra eleifend.', 5, CAST(0x0000A34A00FC46B4 AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0, 0)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (8, N'Buy Sprinter Cell', N'Ut bibendum, felis a tincidunt vehicula, mauris elit suscipit odio, vitae hendrerit eros dui ac ligula. Mauris ut neque nisi. Vestibulum imperdiet nunc vel risus vestibulum laoreet. Nam at orci ac ante luctus tristique. Cras vulputate rutrum fermentum. Proin lacinia placerat dignissim. Phasellus hendrerit mi et viverra eleifend.', 5, CAST(0x0000A34A00FCF097 AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0, 0)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (9, N'Some crazy stuff', N'Morbi diam erat, vehicula ut aliquam et, imperdiet ut metus. Curabitur sollicitudin massa sed risus dapibus feugiat. Phasellus condimentum lacus a mauris varius, sit amet posuere nulla convallis. Nam dolor lorem, fringilla luctus malesuada et, volutpat auctor arcu. Aliquam orci ligula,', 5, CAST(0x0000A34A00FD8D55 AS DateTime), CAST(0x0000A34C00000000 AS DateTime), 0, 0)
GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (10, N'Swimming Custumes', N'Nam facilisis massa ac eleifend venenatis. Integer vehicula velit rutrum, viverra nulla ut, varius neque. Curabitur tristique posuere neque, id euismod dolor dignissim eu. Aliquam hendrerit pharetra consequat. Morbi at arcu et nisi fermentum dapibus. Curabitur sapien est, pellentesque vel risus eu', 5, CAST(0x0000A34A0102E2FA AS DateTime), CAST(0x0000A34B00000000 AS DateTime), 0, 2)
GO
SET IDENTITY_INSERT [dbo].[tasks] OFF
GO
ALTER TABLE [dbo].[events] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[subtasks] ADD  DEFAULT ((0)) FOR [status]
GO
ALTER TABLE [dbo].[tasks] ADD  DEFAULT ((0)) FOR [status]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "emp_basic"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "emp_details"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 135
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_all_employees'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_all_employees'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vw_all_employees"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_employees_names'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_employees_names'
GO
USE [master]
GO
ALTER DATABASE [eventizer] SET  READ_WRITE 
GO
