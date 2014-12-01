using Eventizer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;
using Eventizer.Models;
using System.Data.SqlClient;

namespace Eventizer.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        public ActionResult Index()
        {
            MiniEmployeesList o = Employee.GetEmployeeDetails();
            ViewBag.EmployeeNames = o.Names;
            ViewBag.EmployeeIds = o.Ids;

            return View();
        }

        public bool TestConnectionString()
        {

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(
                        @"Data Source=199.193.116.14,1433;Network Library=DBMSSOCN;
                        Initial Catalog=eventizer;User ID=umayr;Password=c3ab2k11;"))
            {
                try
                {
                    conn.Open();

                    return (conn.State == System.Data.ConnectionState.Open);
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool CreateDatabase()
        {
            try
            {
                SqlConnection connection = new SqlConnection(@"server=(localdb)\v11.0");
                using (connection)
                {
                    connection.Open();

                    string sql = string.Format(@"

CREATE DATABASE [eventizer_web]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eventizer_web', FILENAME = N'{0}\eventizer_web.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'eventizer_web_log', FILENAME = N'{0}\eventizer_web_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [eventizer_web] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eventizer_web].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [eventizer_web] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [eventizer_web] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [eventizer_web] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [eventizer_web] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [eventizer_web] SET ARITHABORT OFF 
GO
ALTER DATABASE [eventizer_web] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [eventizer_web] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [eventizer_web] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [eventizer_web] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [eventizer_web] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [eventizer_web] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [eventizer_web] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [eventizer_web] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [eventizer_web] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [eventizer_web] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [eventizer_web] SET  DISABLE_BROKER 
GO
ALTER DATABASE [eventizer_web] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [eventizer_web] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [eventizer_web] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [eventizer_web] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [eventizer_web] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [eventizer_web] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [eventizer_web] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [eventizer_web] SET RECOVERY FULL 
GO
ALTER DATABASE [eventizer_web] SET  MULTI_USER 
GO
ALTER DATABASE [eventizer_web] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [eventizer_web] SET DB_CHAINING OFF 
GO
ALTER DATABASE [eventizer_web] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [eventizer_web] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'eventizer_web', N'ON'
GO
USE [eventizer_web]
GO
/****** Object:  StoredProcedure [dbo].[usp_add_asset]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_add_asset_to_subtask]    Script Date: 6/17/2014 3:36:48 AM ******/
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

    insert into subtask_assets values(@subtask_id, @asset_id, @quantity);
END


GO
/****** Object:  StoredProcedure [dbo].[usp_add_event]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_add_subtask]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_add_subtask_to_task]    Script Date: 6/17/2014 3:36:48 AM ******/
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
	
	SET NOCOUNT OFF;

    insert into task_subtasks values(@task_id,@subtask_id);
END


GO
/****** Object:  StoredProcedure [dbo].[usp_add_task]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_add_task_to_event]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_get_asset_by_id]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch an asset w.r.t its ID
-- =============================================
create PROCEDURE [dbo].[usp_get_asset_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	select * from assets where id = @id;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_get_employee_by_email]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_get_employee_by_id]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_get_event_by_id]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_get_subtask_by_id]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Umayr Shahid
-- Create date: 6/11/2014
-- Description:	Fetch a subtask w.r.t its ID
-- =============================================
CREATE PROCEDURE [dbo].[usp_get_subtask_by_id] 
	-- Add the parameters for the stored procedure here
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	select * from vw_all_subtasks where id = @id;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_get_task_by_id]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_login_employee]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  StoredProcedure [dbo].[usp_mark_complete]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[usp_mark_complete]
@id int,
@type int

AS
BEGIN
	SET NOCOUNT OFF;
	if @type = 0
	update events set status = 1 where id = @id;
	else if @type = 1
	update tasks set status = 1 where id = @id;
	else if @type = 2
	update subtasks set status = 1 where id = @id;
END


GO
/****** Object:  StoredProcedure [dbo].[usp_register_employee]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[assets]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[emp_basic]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[emp_details]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[event_tasks]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[events]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[subtask_assets]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[subtasks]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  Table [dbo].[task_subtasks]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task_subtasks](
	[task_id] [int] NOT NULL,
	[subtask_id] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tasks]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  View [dbo].[vw_all_employees]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  View [dbo].[vw_employees_names]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_employees_names]
AS
SELECT        name, id
FROM            dbo.vw_all_employees


GO
/****** Object:  View [dbo].[vw_all_assets_names]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create view [dbo].[vw_all_assets_names]
as
select id, name from assets

GO
/****** Object:  View [dbo].[vw_all_events]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  View [dbo].[vw_all_subtasks]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  View [dbo].[vw_all_tasks]    Script Date: 6/17/2014 3:36:48 AM ******/
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
/****** Object:  View [dbo].[vw_fetch_feeds]    Script Date: 6/17/2014 3:36:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[vw_fetch_feeds]
as
select id, name, [description], created_by, date_created, deadline,[status], -1 as assigned_to, 'Event' as [type] from events
union all
select id, name, [description], created_by, date_created, deadline,[status], assigned_to,'Task' as [type] from tasks
union all
select id, name, [description], created_by, date_created, deadline,[status], assigned_to,'Subtask' as [type] from subtasks


GO
SET IDENTITY_INSERT [dbo].[assets] ON 

GO
INSERT [dbo].[assets] ([id], [name], [type], [created_on], [created_by]) VALUES (1, N'Paper', N'Stationery', CAST(0x0000A34D0011C1FA AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[assets] OFF
GO
SET IDENTITY_INSERT [dbo].[emp_basic] ON 

GO
INSERT [dbo].[emp_basic] ([id], [name], [email], [password]) VALUES (1, N'Umayr Shahid', N'umayrr@hotmail.co.uk', N'3fd1a68cbd3dd7a289db04662c730afd0e9fb175')
GO
SET IDENTITY_INSERT [dbo].[emp_basic] OFF
GO
INSERT [dbo].[emp_details] ([id], [phone], [designation], [manager_id], [timestamp]) VALUES (N'1', 3458933015, N'Manager', 0, CAST(0x0000A34D000EB749 AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[event_tasks] ON 

GO
INSERT [dbo].[event_tasks] ([id], [event_id], [task_id]) VALUES (1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[event_tasks] OFF
GO
SET IDENTITY_INSERT [dbo].[events] ON 

GO
INSERT [dbo].[events] ([id], [name], [description], [created_by], [date_created], [deadline], [status]) VALUES (1, N'Birthday Ceremony', N'Aliquam at ipsum nisl. Suspendisse feugiat, velit nec tincidunt pharetra, purus dolor ullamcorper dui, in iaculis lorem ipsum nec orci. Duis dictum eget augue vel consectetur. Integer ultricies rhoncus sem at ullamcorper. Pellentesque dictum laoreet sapien quis mattis. Proin consequat purus risus, non bibendum metus mollis quis. Nulla at ipsum enim.', 1, CAST(0x0000A34D0010D704 AS DateTime), CAST(0x0000A35900000000 AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[events] OFF
GO
INSERT [dbo].[subtask_assets] ([subtask_id], [asset_id], [quantity]) VALUES (1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[subtasks] ON 

GO
INSERT [dbo].[subtasks] ([id], [name], [date_created], [created_by], [description], [labours_required], [deadline], [status], [assigned_to]) VALUES (1, N'Get a list of famous bakeries', CAST(0x0000A34D0011F32E AS DateTime), 1, N'Interdum et malesuada fames ac ante ipsum primis in faucibus. Integer in faucibus risus. Aliquam laoreet, arcu sit amet adipiscing adipiscing, ligula eros ultrices justo, quis dictum libero mauris nec massa. Mauris interdum eros eu sagittis tempor.', 1, CAST(0x0000A34E00000000 AS DateTime), 0, 1)
GO
SET IDENTITY_INSERT [dbo].[subtasks] OFF
GO
INSERT [dbo].[task_subtasks] ([task_id], [subtask_id]) VALUES (1, 1)
GO
SET IDENTITY_INSERT [dbo].[tasks] ON 

GO
INSERT [dbo].[tasks] ([id], [name], [description], [created_by], [date_created], [deadline], [status], [assigned_to]) VALUES (1, N'Order Budapestlangd', N'Mauris ac consequat arcu. Donec adipiscing tortor varius odio dictum porttitor. Sed eget ultrices velit. Curabitur semper nec mauris fermentum pellentesque. Donec in semper libero. Nullam eu sollicitudin nulla. Proin nunc tortor, consequat id sapien quis, hendrerit commodo metus. Etiam eget nisl id mi laoreet rhoncus. Nullam gravida lectus purus, eu aliquam libero facilisis ac.', 1, CAST(0x0000A34D001134ED AS DateTime), CAST(0x0000A35000000000 AS DateTime), 0, 1)
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
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""emp_basic""
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = ""emp_details""
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
      Begin ParameterDefaults = """"
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
         Configuration = ""(H (1[40] 4[20] 2[20] 3) )""
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = ""(H (1 [50] 4 [25] 3))""
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = ""(H (1 [50] 2 [25] 3))""
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = ""(H (4 [30] 2 [40] 3))""
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = ""(H (1 [56] 3))""
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = ""(H (2 [66] 3))""
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = ""(H (4 [50] 3))""
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = ""(V (3))""
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = ""(H (1[56] 4[18] 2) )""
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = ""(H (1 [75] 4))""
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = ""(H (1[66] 2) )""
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = ""(H (4 [60] 2))""
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = ""(H (1) )""
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = ""(V (4))""
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = ""(V (2))""
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = ""vw_all_employees""
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
      Begin ParameterDefaults = """"
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
ALTER DATABASE [eventizer_web] SET  READ_WRITE 
GO
",
                        @"C:"
                    );

                    SqlCommand command = new SqlCommand(sql, connection);
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
