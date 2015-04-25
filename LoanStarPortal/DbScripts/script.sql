USE [LoanStar]
GO
/****** Object:  StoredProcedure [dbo].[GetTaskNoteList]    Script Date: 06/05/2007 11:16:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskNoteList]
	@TaskID  int
AS

	select * from TaskNote
	where TaskID = @TaskID
GO
/****** Object:  Table [dbo].[LoanOfficer]    Script Date: 06/05/2007 11:27:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanOfficer](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_LoanOfficer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 06/05/2007 11:32:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Role_StatusId]  DEFAULT ((1)),
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocumentTemplate]    Script Date: 06/05/2007 11:22:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentTemplate](
	[id] [int] NOT NULL,
	[PDFFileName] [varchar](100) NOT NULL CONSTRAINT [DF_DocumentTemplate_PDFFileName]  DEFAULT (''),
	[MappingData] [text] NOT NULL CONSTRAINT [DF_DocumentTemplate_XMLFileName]  DEFAULT (''),
 CONSTRAINT [PK_DocumentTemplate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[testDrop]    Script Date: 06/05/2007 11:36:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[testDrop](
	[id] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentPackage]    Script Date: 06/05/2007 11:22:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentPackage](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_DocumentPackage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TransferPercentage]    Script Date: 06/05/2007 11:36:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TransferPercentage](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_PercentageTransfer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteDocTemplateField]    Script Date: 06/05/2007 11:14:17 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[DeleteDocTemplateField]
	@ID	int
AS

DELETE FROM DocTemplateField
WHERE ID = @ID

select @@rowcount
GO
/****** Object:  Table [dbo].[RuleAlert]    Script Date: 06/05/2007 11:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleAlert](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [varchar](256) NOT NULL,
 CONSTRAINT [PK_RuleAlert] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ServiceYourLoan]    Script Date: 06/05/2007 11:35:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServiceYourLoan](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ServiceYourLoan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserRoleByUser]    Script Date: 06/05/2007 11:14:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserRoleByUser]
	@userid     int
AS
	DELETE FROM UserRole
	WHERE Userid=@userid
GO
/****** Object:  Table [dbo].[TaskDifficulty]    Script Date: 06/05/2007 11:36:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskDifficulty](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_TaskDifficulty] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HowManyAssigns]    Script Date: 06/05/2007 11:23:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HowManyAssigns](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_HowManyAssigns] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserRoleByRole]    Script Date: 06/05/2007 11:14:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserRoleByRole]
	@roleid     int
AS
	DELETE FROM UserRole
	WHERE Roleid=@Roleid
GO
/****** Object:  StoredProcedure [dbo].[SaveRoleTemplate]    Script Date: 06/05/2007 11:17:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRoleTemplate]
	@id     int
	,@name  varchar(50)
AS
DECLARE @cnt int
	BEGIN TRANSACTION tr
	SELECT @cnt = COUNT(*) FROM Roletemplate
	WHERE @id<>id AND @name=[name]
	IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- already exists
	END 
	IF @id > 0 BEGIN
		UPDATE RoleTemplate
		SET [Name]=@name
		WHERE id=@id
		IF @@ERROR<>0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
		RETURN
	END ELSE BEGIN
		INSERT INTO RoleTemplate(
			[Name]
		) VALUES (
			@name
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT scope_identity()
		RETURN
	END
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  Table [dbo].[RuleData]    Script Date: 06/05/2007 11:34:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FieldId] [int] NOT NULL,
	[FieldValue] [varchar](256) NOT NULL,
 CONSTRAINT [PK_RuleData] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LogicalOperation]    Script Date: 06/05/2007 11:27:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogicalOperation](
	[id] [smallint] NOT NULL,
	[Name] [varchar](10) NOT NULL CONSTRAINT [DF_LogicalOperation_Name]  DEFAULT (''),
 CONSTRAINT [PK_LogicalOperation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetCheckListById]    Script Date: 06/05/2007 11:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCheckListById]
	@id			int
AS
	SELECT pf.[id],pf.[Name]
		,CONVERT(bit, CASE WHEN cl.[id] IS NULL THEN 0 ELSE 1 END) AS Selected
		,ISNULL(cl.Question,'') AS Question
		,ISNULL(cl.cbYes,0) AS cbYes
		,ISNULL(cl.cbNo,0) AS cbNo
		,ISNULL(cl.cbDontknow,0) AS cbDontKnow
		,ISNULL(cl.cbToFollow,0) AS cbToFollow 
	FROM ProfileStatus pf
	LEFT OUTER JOIN vwRulechecklist cl ON cl.Profilestatusid=pf.[Id] AND cl.ID=@id
	WHERE pf.Id>0
GO
/****** Object:  Table [dbo].[RuleCheckList]    Script Date: 06/05/2007 11:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleCheckList](
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_RuleCheckList] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskType]    Script Date: 06/05/2007 11:36:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [varchar](100) NOT NULL,
	[IsSelectable] [bit] NOT NULL CONSTRAINT [DF_TaskType_IsSelectable]  DEFAULT ((1)),
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_TaskType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetFieldList]    Script Date: 06/05/2007 11:15:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFieldList]
	@fieldid		int
	,@fieldtypeid	int
AS
	SELECT id,[Description] AS FieldName FROM MortgageProfileField
	WHERE id <> @fieldid AND ValueTypeId=@fieldtypeid
GO
/****** Object:  Table [dbo].[Status]    Script Date: 06/05/2007 11:35:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[id] [int] NOT NULL,
	[StatusName] [varchar](10) NOT NULL,
 CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[AddRoleStatus]    Script Date: 06/05/2007 11:13:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRoleStatus]
	@roleid	int
	,@initstatusid	int
	,@finalstatusid int
AS
	INSERT INTO RoleProfileStatus(
		roleId
		,ProfileInitStatusId
		,ProfileFinalStatusId
	) VALUES (
		@roleid
		,@initstatusid
		,@finalstatusid		
	)
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[EditMortgageProfile]    Script Date: 06/05/2007 11:14:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditMortgageProfile](
 @id   INT,
 @BorrowerInfoId   INT,
 @PropertyInfoId   INT,
 @ServicerId   INT,
 @VendorId   INT,
 @WholesaleLenderId   INT,
 @CurProfileStatusID   INT,
 @CompanyID   INT
)
AS

DECLARE @Result  INT

IF(@id <= 0)
 BEGIN
 INSERT INTO MortgageProfile
  (BorrowerInfoId, PropertyInfoId, ServicerId, VendorId, WholesaleLenderId, CurProfileStatusID, CompanyID) 
 VALUES
  (@BorrowerInfoId, @PropertyInfoId, @ServicerId, @VendorId, @WholesaleLenderId, @CurProfileStatusID, @CompanyID) 
 SET @Result = @@IDENTITY
 END
ELSE
 BEGIN
  UPDATE  MortgageProfile SET
  BorrowerInfoId = @BorrowerInfoId,
  PropertyInfoId = @PropertyInfoId,
  ServicerId = @ServicerId,
  VendorId = @VendorId,
  WholesaleLenderId = @WholesaleLenderId,
  CurProfileStatusID = @CurProfileStatusID,
  CompanyID = @CompanyID
  WHERE  ID = @id
  SET @Result = @@rowcount
 END

SELECT @Result
GO
/****** Object:  StoredProcedure [dbo].[ArchiveUnarchiveDocTemplate]    Script Date: 06/05/2007 11:14:01 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ArchiveUnarchiveDocTemplate] (
	@id		INT,
	@archive	BIT
)
AS
UPDATE DocTemplate SET Archived = @archive WHERE ID = @id
GO
/****** Object:  Table [dbo].[DocumentGroup]    Script Date: 06/05/2007 11:22:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentGroup](
	[id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_DocumentGroup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceType]    Script Date: 06/05/2007 11:26:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_InvoiceType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteRoleStatus]    Script Date: 06/05/2007 11:14:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRoleStatus]
	@id  int
AS
	DELETE FROM RoleProfileStatus
	WHERE id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[DeleteRoleTemplate]    Script Date: 06/05/2007 11:14:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRoleTemplate]
	@id int
AS
	DELETE FROM RoleTemplate
	WHERE  id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  Table [dbo].[PropertyInfo]    Script Date: 06/05/2007 11:32:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyInfo](
	[id] [int] NOT NULL,
	[TotalLienPayoffs] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_PropertyInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetAdminList]    Script Date: 06/05/2007 11:14:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAdminList]
	@orderby varchar(100) = ''
	,@where varchar(1000) = ''
AS
DECLARE @sql nvarchar(2000)
	IF @orderby = '' AND @where = ''
		SELECT * FROM vwAdmin
	ELSE BEGIN
		SET @sql = 'SELECT * FROM vwAdmin '
		IF @where <> ''
			SET @sql = @sql + @where + ' '
		IF @orderby <> ''
			SET @sql = @sql + @orderby
		EXEC sp_executesql @sql
	END
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageBorrowers]    Script Date: 06/05/2007 11:15:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageBorrowers](
	@MortgageProfileID	int
)
AS
 --SELECT b.* FROM Borrower b INNER JOIN MortgageProfileBorrower mpb ON b.id = mpb.BorrowerID WHERE mpb.MortgageProfileID = @MortgageProfileID
	SELECT * FROM Borrower 
	WHERE MortgageId=@MortgageProfileID
GO
/****** Object:  Table [dbo].[CompareOperation]    Script Date: 06/05/2007 11:20:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CompareOperation](
	[Id] [smallint] NOT NULL,
	[Name] [varchar](10) NOT NULL,
 CONSTRAINT [PK_RuleCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TaskStatus]    Script Date: 06/05/2007 11:36:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetConditionList]    Script Date: 06/05/2007 11:15:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionList]
	@mortgageID  int
AS

	select c.*
	from Condition c
	where MortgageID = @mortgageID
GO
/****** Object:  StoredProcedure [dbo].[SaveInvoice]    Script Date: 06/05/2007 11:17:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveInvoice]
	@id	int,
	@MortgageID int,
	@TypeID	int,
	@ProviderID int,
	@IN bit,
	@Invoice money,
	@PMT varchar(50),
	@CreatedBy int
AS
DECLARE @newid int
	BEGIN TRANSACTION tr
	if exists(select id from Invoice where id=@id)
	 begin
		update Invoice set
			MortgageID = @MortgageID,
			TypeID = @TypeID,
			ProviderID = @ProviderID,
			[IN] = @IN,
			Invoice = @Invoice,
			PMT = @PMT
		where id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
	 end
	else
	 begin
		insert into Invoice(
			MortgageID,
			TypeID,
			ProviderID,
			[IN],
			Invoice,
			PMT,
			CreatedBy
		)values(
			@MortgageID,
			@TypeID,
			@ProviderID,
			@IN,
			@Invoice,
			@PMT,
			@CreatedBy
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		COMMIT TRANSACTION Tr
		SELECT @newid
	 end

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetFieldInGroup]    Script Date: 06/05/2007 11:15:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetFieldInGroup]
	@groupid 	int
AS
	SELECT id,[description] FROM MortgageProfileField
	WHERE FieldGroupId=@groupid
GO
/****** Object:  StoredProcedure [dbo].[GetInvoiceProviderList]    Script Date: 06/05/2007 11:15:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInvoiceProviderList]
AS

	select * from InvoiceProvider
	order by DisplayOrder
GO
/****** Object:  Table [dbo].[InvoiceProvider]    Script Date: 06/05/2007 11:26:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InvoiceProvider](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_InvoiceProvider] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[test]    Script Date: 06/05/2007 11:36:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[test](
	[fint] [int] NOT NULL,
	[fdate] [datetime] NOT NULL,
	[fstring] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetRole]    Script Date: 06/05/2007 11:16:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRole]
	@roleid		int
AS
	SELECT * FROM Role
	WHERE id=@roleid
GO
/****** Object:  Table [dbo].[ConditionCategory]    Script Date: 06/05/2007 11:20:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConditionCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ConditionCategory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Control]    Script Date: 06/05/2007 11:21:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Control](
	[id] [int] NOT NULL,
	[ControlName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Controls] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MortgageProfileFieldType]    Script Date: 06/05/2007 11:29:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MortgageProfileFieldType](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MortgageProfileFieldType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetRoleTemplate]    Script Date: 06/05/2007 11:16:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleTemplate]
	@roleid		int
AS
	SELECT * FROM RoleTemplate
	WHERE id=@roleid
GO
/****** Object:  StoredProcedure [dbo].[GetRoleTemplateList]    Script Date: 06/05/2007 11:16:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleTemplateList]
	@all bit = 0
	,@asc bit = 1
AS
	IF @all=1
		SELECT * FROM RoleTemplate 
		WHERE DisplayOrder>=0
		ORDER BY DisplayOrder
--		IF @asc=1		
			--SELECT * FROM RoleTemplate 
--			ORDER BY [Name] ASC
--		ELSE
--			SELECT * FROM RoleTemplate 
--			ORDER BY [Name] DESC
	ELSE
		IF @asc=1		
		SELECT * FROM RoleTemplate 
		WHERE id>0 AND DisplayOrder>=0
		ORDER BY [DisplayOrder]

--			SELECT * FROM RoleTemplate 
--			WHERE id>0
--			ORDER BY [Name] ASC
--		ELSE
--			SELECT * FROM RoleTemplate 
--			WHERE id>0
--			ORDER BY [Name] DESC
GO
/****** Object:  Table [dbo].[ConditionType]    Script Date: 06/05/2007 11:21:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConditionType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ConditionType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetBorrowerById]    Script Date: 06/05/2007 11:15:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetBorrowerById](
	@id		INT
)
AS
	SELECT * FROM Borrower WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[EditBorrower]    Script Date: 06/05/2007 11:14:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditBorrower](
	@id			INT,
	@FirstName		VARCHAR(50),
	@LastName		VARCHAR(100),
	@Sex			BIT,
	@Address1		VARCHAR(256),
	@Address2		VARCHAR(256),
	@Address3		VARCHAR(256),
	@City			VARCHAR(50),
	@StateId		INT,
	@Zip			VARCHAR(12),
	@DateOfBirth		SMALLDATETIME,
	@PhoneNumber		VARCHAR(20),
	@SSN			VARCHAR(12),
	@archived		BIT
)
AS

DECLARE @Result 	INT

IF(@id <= 0)
 BEGIN
	INSERT INTO Borrower
		(FirstName, LastName, Sex, Address1, Address2, Address3, City, StateId, Zip, DateOfBirth, PhoneNumber, SSN, archived) 
	VALUES
		(@FirstName, @LastName, @Sex, @Address1, @Address2, @Address3, @City, @StateId, @Zip, @DateOfBirth, @PhoneNumber, @SSN, @archived) 
	SET @Result = @@IDENTITY
 END
ELSE
	BEGIN
	 UPDATE  Borrower SET
		FirstName = @FirstName,
		LastName = @LastName,
		Sex = @Sex,
		Address1 = @Address1,
		Address2 = @Address2,
		Address3 = @Address3,
		City = @City,
		StateId = @StateId,
		 Zip = @Zip,
		DateOfBirth = @DateOfBirth,
		PhoneNumber = @PhoneNumber,
		SSN = @SSN,
		archived = @archived
	 WHERE  ID = @id
	 SET @Result = @@rowcount
	END

SELECT @Result
GO
/****** Object:  StoredProcedure [dbo].[GetUserRoles]    Script Date: 06/05/2007 11:16:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserRoles]
	@userid  int
AS
	SELECT * FROM Role r
	INNER JOIN UserRole ur ON ur.roleid=r.id
	WHERE ur.userid=@userid
GO
/****** Object:  StoredProcedure [dbo].[GetConditionStatusList]    Script Date: 06/05/2007 11:15:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionStatusList]
AS

	select * from ConditionStatus
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetPayoffList]    Script Date: 06/05/2007 11:16:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPayoffList]
	@MortgageID  int
AS

	select p.*,
		datediff(day, getdate(),ExpDate) as RemDays
	 from Payoff p
	where MortgageID = @MortgageID
GO
/****** Object:  Table [dbo].[RoleProfileStatus]    Script Date: 06/05/2007 11:32:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleProfileStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Roleid] [int] NOT NULL,
	[ProfileInitStatusId] [int] NOT NULL,
	[ProfileFinalStatusId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SaveFilledDocsInfo]    Script Date: 06/05/2007 11:17:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[SaveFilledDocsInfo](
	@filledDIXml	ntext
)
AS

DECLARE @ResultID 	INT
SET		@ResultID = 0

DECLARE @idXmlDoc  int
EXEC sp_xml_preparedocument @idXmlDoc OUTPUT, @filledDIXml
IF @@ERROR <> 0 
	GOTO Finish

SELECT	*
INTO	#MPPackage
FROM	OPENXML (@idXmlDoc, 'Root/item', 1)
		WITH	(
				MortgageID	int '@MortgageID', 
				[FileName]	nvarchar(100) '@FileName', 
				Title		nvarchar(100) '@Title', 
				PackageName	nvarchar(100) '@PackageName'
				)

EXEC sp_xml_removedocument @idXmlDoc

BEGIN TRANSACTION tr

	UPDATE	MortgageProfilePackage
	SET		[FileName] = sourceMPP.[FileName] 
	FROM	MortgageProfilePackage destMPP
			INNER JOIN #MPPackage sourceMPP ON	sourceMPP.MortgageID = destMPP.MortgageID AND 
												sourceMPP.Title = destMPP.Title AND 
												sourceMPP.PackageName = destMPP.PackageName 
	SET @ResultID =	@@rowcount
	IF @@ERROR <> 0 
		GOTO ErrorHandler

	DELETE	#MPPackage
	FROM	MortgageProfilePackage destMPP
			INNER JOIN #MPPackage sourceMPP ON	sourceMPP.MortgageID = destMPP.MortgageID AND 
												sourceMPP.Title = destMPP.Title AND 
												sourceMPP.PackageName = destMPP.PackageName 

	INSERT	INTO MortgageProfilePackage
	SELECT	mpp.*, GETDATE() UploadDate
	FROM	#MPPackage mpp 

	DECLARE @tmpResultID int
	SET		@tmpResultID = @@rowcount

	IF @@ERROR <> 0 
		GOTO ErrorHandler
	IF @ResultID = 0
		SET @ResultID =	@tmpResultID

COMMIT TRANSACTION tr
GOTO FullFinish


ErrorHandler:
ROLLBACK TRANSACTION tr
SET		@ResultID = 0

FullFinish:
DROP TABLE #MPPackage
Finish:
SELECT @ResultID
GO
/****** Object:  Table [dbo].[ProfileStatus]    Script Date: 06/05/2007 11:31:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProfileStatus](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_ProfileStatus_Name]  DEFAULT (''),
 CONSTRAINT [PK_ProfileStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageRoleList]    Script Date: 06/05/2007 11:16:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageRoleList]
AS
	SELECT * FROM RoleTemplate
	WHERE CanBeAssignedToMortgage=1
	ORDER BY DisplayOrder
GO
/****** Object:  Table [dbo].[MortgageProfileBorrower]    Script Date: 06/05/2007 11:29:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfileBorrower](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileID] [int] NOT NULL,
	[BorrowerID] [int] NOT NULL,
 CONSTRAINT [PK_MortgageProfileBorrower] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_MortgageProfileBorrower] UNIQUE NONCLUSTERED 
(
	[BorrowerID] ASC,
	[MortgageProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskInfoSource]    Script Date: 06/05/2007 11:36:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskInfoSource](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_TaskInfoSource_DisplayOrder]  DEFAULT ((1)),
 CONSTRAINT [PK_TaskInfoSource] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetDocTemplateList]    Script Date: 06/05/2007 11:15:16 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Stored Procedure dbo.GetDocTemplateList    Script Date: 3/14/2007 2:32:09 PM ******/


/****** Object:  Stored Procedure dbo.GetDocTemplateList    Script Date: 3/13/2007 2:48:49 PM ******/


CREATE    PROCEDURE [dbo].[GetDocTemplateList] (
	@OrderBy	varchar(500)='',
	@WhereClause	varchar(500)=''
) AS

DECLARE @SQLString nvarchar(1000)
SET @SQLString =	N'SELECT docTempl.ID, docTempl.Title, docTempl.Archived, docTemplVer.UploadDate ' + 
			N'FROM	DocTemplate docTempl ' + 
			N'	INNER JOIN DocTemplateVersion docTemplVer ON docTemplVer.DocTemplateID = docTempl.ID ' + 
			N'	INNER JOIN ' + 
			N'		( ' + 
			N'		SELECT docTemplGr.ID DTID, MAX(docTemplVerGr.UploadDate) DTLastUplDate ' + 
			N'		FROM	 DocTemplate docTemplGr ' + 
			N'			 INNER JOIN DocTemplateVersion docTemplVerGr ON docTemplVerGr.DocTemplateID = docTemplGr.ID ' + 
			N'		GROUP BY docTemplGr.ID ' + 
			N'		) AS LastDocTempl ON LastDocTempl.DTID = docTempl.ID AND LastDocTempl.DTLastUplDate = docTemplVer.UploadDate ' 

IF LEN(@WhereClause) > 0
	SET @SQLString = @SQLString + @WhereClause + ' '
IF LEN(@OrderBy) > 0
	SET @SQLString = @SQLString + @OrderBy

EXEC sp_executesql @SQLString

set ANSI_NULLS ON
set QUOTED_IDENTIFIER OFF
GO
/****** Object:  Table [dbo].[DocTemplate]    Script Date: 06/05/2007 11:21:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocTemplate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Archived] [bit] NOT NULL,
 CONSTRAINT [PK_DocTemplate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InterestRate]    Script Date: 06/05/2007 11:25:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InterestRate](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_InterestRate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskRecurrence]    Script Date: 06/05/2007 11:36:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskRecurrence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_TaskRecurrence_DisplayOrder]  DEFAULT ((1)),
 CONSTRAINT [PK_TaskRecurrence] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Markupamount]    Script Date: 06/05/2007 11:28:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Markupamount](
	[id] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Markup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 06/05/2007 11:35:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sex](
	[id] [int] NOT NULL,
	[Name] [varchar](20) NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TitleHeld]    Script Date: 06/05/2007 11:36:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TitleHeld](
	[id] [int] NOT NULL,
	[Name] [varchar](256) NOT NULL CONSTRAINT [DF_TitleHeld_Name]  DEFAULT (''),
 CONSTRAINT [PK_TitleHeld] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Salutation]    Script Date: 06/05/2007 11:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Salutation](
	[id] [int] NOT NULL,
	[Description] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Salutation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RoleTemplateProfileStatus]    Script Date: 06/05/2007 11:33:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleTemplateProfileStatus](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Roleid] [int] NOT NULL,
	[ProfileInitStatusId] [int] NOT NULL,
	[ProfileFinalStatusId] [int] NOT NULL,
 CONSTRAINT [PK_RoleTemplateProfileStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCompany]    Script Date: 06/05/2007 11:14:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCompany]
	@companyid int
AS
	UPDATE Company
	SET StatusId=3
	WHERE Id=@companyid
	SELECT @@ROWCOUNT
GO
/****** Object:  Table [dbo].[EventType]    Script Date: 06/05/2007 11:23:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_EventType_DisplayOrder]  DEFAULT ((1)),
	[IsSelectable] [bit] NOT NULL CONSTRAINT [DF_EventType_IsSelectable]  DEFAULT ((1)),
 CONSTRAINT [PK_EventType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SaveHoliday]    Script Date: 06/05/2007 11:17:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveHoliday]
	@id			int
	,@name		varchar(100)
	,@day		smalldatetime
	,@companyid	int
AS
DECLARE @cnt int
	SELECT @cnt=COUNT(*) FROM Holiday WHERE id<>@id AND [Description]=@name
	IF @cnt > 0 BEGIN
		SELECT -1
		RETURN;
	END
	IF @id > 0 BEGIN
		UPDATE Holiday
		SET [Description]=@name
		,Holidaydate=@day
		WHERE id=@id
		SELECT @id
	END ELSE BEGIN
		INSERT INTO Holiday(
			[Description]
			,Holidaydate
			,CompanyId
		) VALUES (
			@name
			,@day
			,@companyid
		)
		SELECT scope_identity()
	END
GO
/****** Object:  Table [dbo].[UnderwritingCondition]    Script Date: 06/05/2007 11:36:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnderwritingCondition](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_UnderwritingCondition] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Holiday]    Script Date: 06/05/2007 11:23:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Holiday](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Holidaydate] [smalldatetime] NOT NULL,
	[Description] [varchar](100) NOT NULL CONSTRAINT [DF_Holiday_Description]  DEFAULT (''),
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[State]    Script Date: 06/05/2007 11:35:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[State](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Abbreviation] [char](10) NOT NULL CONSTRAINT [DF_States_FullName]  DEFAULT (''),
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_States_Abbriviation]  DEFAULT (''),
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetCompanyList]    Script Date: 06/05/2007 11:15:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompanyList]
	@all	bit = 1
AS
	IF @all=1
		SELECT * FROM Company
		WHERE StatusId!=3
		ORDER BY Company
	ELSE
		SELECT * FROM Company
		WHERE StatusId!=3 AND ID > 0
		ORDER BY Company
GO
/****** Object:  Table [dbo].[Factor]    Script Date: 06/05/2007 11:23:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factor](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_Factor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetListCompany]    Script Date: 06/05/2007 11:15:46 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetListCompany]
	@orderby varchar(100) = ''
	,@where varchar(1000) = ''
AS
DECLARE @sql nvarchar(2000)
	IF @orderby = '' AND @where = ''
		SELECT * FROM Company
		WHERE StatusId!=3 and ID>0
	ELSE BEGIN
		SET @sql = 'SELECT * FROM Company '
		IF @where <> ''
			SET @sql = @sql + @where + ' AND StatusId!=3 AND Id>0'
		ELSE
			SET @sql = @sql + 'WHERE StatusId!=3 and ID>0'
		IF @orderby <> ''
			SET @sql = @sql + @orderby
		EXEC sp_executesql @sql
	END
GO
/****** Object:  StoredProcedure [dbo].[GetDictionaryValues]    Script Date: 06/05/2007 11:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDictionaryValues]
	@table	varchar(50)
AS
DECLARE @sql nvarchar(4000)
	SET @sql = 'SELECT * FROM '+@table + ' WHERE Id>0'
	EXEC sp_executesql @sql
GO
/****** Object:  Table [dbo].[RuleCategory]    Script Date: 06/05/2007 11:33:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleCategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_RuleCategory_Name]  DEFAULT (''),
 CONSTRAINT [PK_RuleCategory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HUDFactors]    Script Date: 06/05/2007 11:25:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HUDFactors](
	[Age] [float] NULL,
	[4] [float] NULL,
	[4,125] [float] NULL,
	[4,25] [float] NULL,
	[4,375] [float] NULL,
	[4,5] [float] NULL,
	[4,625] [float] NULL,
	[4,75] [float] NULL,
	[4,875] [float] NULL,
	[5] [float] NULL,
	[5,125] [float] NULL,
	[5,25] [float] NULL,
	[5,375] [float] NULL,
	[5,5] [float] NULL,
	[5,625] [float] NULL,
	[5,75] [float] NULL,
	[5,875] [float] NULL,
	[6] [float] NULL,
	[6,125] [float] NULL,
	[6,25] [float] NULL,
	[6,375] [float] NULL,
	[6,5] [float] NULL,
	[6,625] [float] NULL,
	[6,75] [float] NULL,
	[6,875] [float] NULL,
	[7] [float] NULL,
	[7,125] [float] NULL,
	[7,25] [float] NULL,
	[7,375] [float] NULL,
	[7,5] [float] NULL,
	[7,625] [float] NULL,
	[7,75] [float] NULL,
	[7,875] [float] NULL,
	[8] [float] NULL,
	[8,125] [float] NULL,
	[8,25] [float] NULL,
	[8,375] [float] NULL,
	[8,5] [float] NULL,
	[8,625] [float] NULL,
	[8,75] [float] NULL,
	[8,875] [float] NULL,
	[9] [float] NULL,
	[9,125] [float] NULL,
	[9,25] [float] NULL,
	[9,375] [float] NULL,
	[9,5] [float] NULL,
	[9,625] [float] NULL,
	[9,75] [float] NULL,
	[9,875] [float] NULL,
	[10] [float] NULL,
	[10,125] [float] NULL,
	[10,25] [float] NULL,
	[10,375] [float] NULL,
	[10,5] [float] NULL,
	[10,625] [float] NULL,
	[10,75] [float] NULL,
	[10,875] [float] NULL,
	[11] [float] NULL,
	[11,125] [float] NULL,
	[11,25] [float] NULL,
	[11,375] [float] NULL,
	[11,5] [float] NULL,
	[11,625] [float] NULL,
	[11,75] [float] NULL,
	[11,875] [float] NULL,
	[12] [float] NULL,
	[12,125] [float] NULL,
	[12,25] [float] NULL,
	[12,375] [float] NULL,
	[12,5] [float] NULL,
	[12,625] [float] NULL,
	[12,75] [float] NULL,
	[12,875] [float] NULL,
	[13] [float] NULL,
	[13,125] [float] NULL,
	[13,25] [float] NULL,
	[13,375] [float] NULL,
	[13,5] [float] NULL,
	[13,625] [float] NULL,
	[13,75] [float] NULL,
	[13,875] [float] NULL,
	[14] [float] NULL,
	[14,125] [float] NULL,
	[14,25] [float] NULL,
	[14,375] [float] NULL,
	[14,5] [float] NULL,
	[14,625] [float] NULL,
	[14,75] [float] NULL,
	[14,875] [float] NULL,
	[15] [float] NULL,
	[15,125] [float] NULL,
	[15,25] [float] NULL,
	[15,375] [float] NULL,
	[15,5] [float] NULL,
	[15,625] [float] NULL,
	[15,75] [float] NULL,
	[15,875] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfileUser]    Script Date: 06/05/2007 11:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfileUser](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileID] [int] NOT NULL,
	[UserRoleID] [int] NOT NULL,
	[ProfileStatusID] [int] NOT NULL,
	[OperationStartDate] [datetime] NOT NULL CONSTRAINT [DF_MortgageProfileUser_OperationStartDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_MortgageProfileUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HUDLendingLimit]    Script Date: 06/05/2007 11:25:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HUDLendingLimit](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_HUDLendingLimit] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SaveCondition]    Script Date: 06/05/2007 11:17:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCondition]
	@id	int,
	@MortgageID int,
	@Title varchar(100),
	@Details varchar(250),
	@SignOffLevelID int,
	@TypeID int,
	@CategoryID int,
	@StatusID int,
	@Notes varchar(250),
	@CreatedBy int
AS
DECLARE @newid int
	BEGIN TRANSACTION tr
	if exists(select id from Condition where id=@id)
	 begin
		update Condition set
			MortgageID = @MortgageID,
			Title = @Title,
			Details = @Details,
			SignOffLevelID = @SignOffLevelID,
			TypeID = @TypeID,
			CategoryID = @CategoryID,
			StatusID = @StatusID,
			Notes = @Notes,
			CreatedBy = @CreatedBy
		where id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
	 end
	else
	 begin
		insert into Condition(
			MortgageID,
			Title,
			Details,
			SignOffLevelID,
			TypeID,
			CategoryID,
			StatusID,
			Notes,
			CreatedBy
		)values(
			@MortgageID,
			@Title,
			@Details,
			@SignOffLevelID,
			@TypeID,
			@CategoryID,
			@StatusID,
			@Notes,
			@CreatedBy
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		COMMIT TRANSACTION Tr
		SELECT @newid
	 end

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  Table [dbo].[PaymentPlan]    Script Date: 06/05/2007 11:31:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentPlan](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_PaymentPlan] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConditionStatus]    Script Date: 06/05/2007 11:21:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConditionStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ConditionStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FieldGroup]    Script Date: 06/05/2007 11:23:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FieldGroup](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_FieldGroup_Name]  DEFAULT (''),
 CONSTRAINT [PK_FieldGroup] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CorrespondentLender]    Script Date: 06/05/2007 11:21:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CorrespondentLender](
	[id] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_CorrespondentLender_Name]  DEFAULT (''),
	[CountryId] [int] NOT NULL,
	[StateId] [int] NOT NULL,
	[City] [varchar](50) NOT NULL CONSTRAINT [DF_CorrespondentLender_City]  DEFAULT (''),
	[Address1] [varchar](256) NOT NULL CONSTRAINT [DF_CorrespondentLender_Address1]  DEFAULT (''),
	[Address2] [varchar](256) NOT NULL CONSTRAINT [DF_CorrespondentLender_Address2]  DEFAULT (''),
	[Zip] [varchar](12) NOT NULL CONSTRAINT [DF_CorrespondentLender_Zip]  DEFAULT (''),
	[IDCode] [varchar](20) NOT NULL CONSTRAINT [DF_CorrespondentLender_IDCode]  DEFAULT (''),
 CONSTRAINT [PK_CorrespondentLender] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteRule]    Script Date: 06/05/2007 11:14:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRule]
	@ruleid int
AS
	DELETE FROM [Rule] WHERE Id=@ruleid
	SELECT @@ROWCOUNT
GO
/****** Object:  Table [dbo].[MailStatus]    Script Date: 06/05/2007 11:28:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MailStatus](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MailStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VendorInvoice]    Script Date: 06/05/2007 11:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorInvoice](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_VendorInvoice] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[LoadRuleById]    Script Date: 06/05/2007 11:17:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[LoadRuleById]
	@ruleid		int
AS
	SELECT * FROM [Rule]
	WHERE id=@ruleid
GO
/****** Object:  Table [dbo].[RuleObjectType]    Script Date: 06/05/2007 11:34:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleObjectType](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RuleObjectType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[GetRuleProductList]    Script Date: 06/05/2007 11:16:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleProductList]
	@ruleid		int
AS
	SELECT p.Id,p.Name,
	CASE WHEN rp.id IS NULL THEN 0
		 ELSE 1 END Selected
	FROM Product p
	LEFT OUTER JOIN RuleProduct rp ON rp.ProductId=p.Id AND rp.ruleid=@ruleid
	WHERE p.id >0
GO
/****** Object:  Table [dbo].[VendorOrder]    Script Date: 06/05/2007 11:37:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorOrder](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_VendorOrder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleAction]    Script Date: 06/05/2007 11:33:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleAction](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[LoadFieldById]    Script Date: 06/05/2007 11:17:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[LoadFieldById]
	@id		int
AS
	SELECT * FROM MortgageProfileField
	WHERE id=@id
GO
/****** Object:  StoredProcedure [dbo].[DeleteRuleUnit]    Script Date: 06/05/2007 11:14:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRuleUnit]
	@id	int
AS
	DELETE FROM RuleUnit
	WHERE Id = @id
	SELECT @@ROWCOUNT
GO
/****** Object:  Table [dbo].[MartialStatus]    Script Date: 06/05/2007 11:28:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MartialStatus](
	[id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
 CONSTRAINT [PK_MartialStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConditionSignOffLevel]    Script Date: 06/05/2007 11:20:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConditionSignOffLevel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_ConditionSignOffLevel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payoff]    Script Date: 06/05/2007 11:31:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payoff](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[Creditor] [varchar](50) NOT NULL,
	[Ordered] [bit] NOT NULL CONSTRAINT [DF_Payoff_Ordered]  DEFAULT ((0)),
	[IN] [bit] NOT NULL,
	[Amount] [money] NOT NULL,
	[Perdiem] [varchar](50) NOT NULL,
	[ExpDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Payoff_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_Payoff] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Creditor]    Script Date: 06/05/2007 11:21:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Creditor](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_Creditor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetDictionaryList]    Script Date: 06/05/2007 11:15:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDictionaryList]
	@table	varchar(50)
	,@field	varchar(50)
AS
DECLARE @sql nvarchar(4000)
	SET @sql = 'SELECT * FROM '+@table + ' ORDER BY ' + @field
	EXEC sp_executesql @sql
GO
/****** Object:  StoredProcedure [dbo].[GetTaskNote]    Script Date: 06/05/2007 11:16:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskNote]
	@ID  int
AS

	select * from TaskNote
	where ID = @ID
GO
/****** Object:  Table [dbo].[RoleTemplate]    Script Date: 06/05/2007 11:32:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RoleTemplate](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_RoleTemplate_Name]  DEFAULT (''),
	[DisplayOrder] [int] NULL,
	[CanBeAssignedToMortgage] [bit] NULL,
	[Abbriviation] [nchar](10) NOT NULL,
	[ParentRoleId] [int] NULL,
 CONSTRAINT [PK_RoleTemplate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Servicer]    Script Date: 06/05/2007 11:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Servicer](
	[id] [int] NOT NULL,
	[ABANumber] [varchar](50) NOT NULL CONSTRAINT [DF_Servicer_ABANumber]  DEFAULT (''),
	[Servicernumber] [varchar](50) NOT NULL CONSTRAINT [DF_Servicer_Servicernumber]  DEFAULT (''),
 CONSTRAINT [PK_Servicer] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RBYNValues]    Script Date: 06/05/2007 11:32:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RBYNValues](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RBYNValues] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ControlType]    Script Date: 06/05/2007 11:21:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ControlType](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[Name] [varchar](50) NOT NULL CONSTRAINT [DF_ControlType_Name]  DEFAULT (''),
 CONSTRAINT [PK_ControlType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 06/05/2007 11:37:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_Vendor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleCondition]    Script Date: 06/05/2007 11:34:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleCondition](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Detail] [varchar](256) NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_RuleCondition] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WholesaleLender]    Script Date: 06/05/2007 11:37:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WholesaleLender](
	[id] [int] NOT NULL,
 CONSTRAINT [PK_WholesaleLender] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageHeaderBorrower]    Script Date: 06/05/2007 11:15:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageHeaderBorrower](
	@CompanyID	int, 
	@mortgageid	INT
)AS
CREATE TABLE #tmp (MPID INT, YBID INT, YBFirstName varchar(50), YBLastName varchar(50), MortgageProfileUserID int, StatusID int, UserID int)

INSERT #tmp EXEC [dbo].[GetMortgageHeaders] @CompanyID
select YBID from #tmp WHERE MPID = @mortgageid
DROP table #tmp
GO
/****** Object:  StoredProcedure [dbo].[GetProductById]    Script Date: 06/05/2007 11:16:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProductById]
	@id		int	
AS
	SELECT * FROM Product WHERE Id=@id
GO
/****** Object:  Table [dbo].[ControlAccess]    Script Date: 06/05/2007 11:21:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ControlAccess](
	[id] [int] NOT NULL,
	[ControlName] [varchar](50) NOT NULL,
	[MortgageStatusId] [int] NOT NULL,
 CONSTRAINT [PK_ControlAccess] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FieldRestriction]    Script Date: 06/05/2007 11:23:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldRestriction](
	[id] [int] NOT NULL,
	[FieldId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[IsVisible] [bit] NOT NULL CONSTRAINT [DF_FieldRestriction_IsVisible]  DEFAULT (1),
	[IsEditible] [bit] NOT NULL CONSTRAINT [DF_FieldRestriction_IsEditible]  DEFAULT (0),
 CONSTRAINT [PK_FieldRestriction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 06/05/2007 11:22:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Document](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[TemplateId] [int] NOT NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocTemplateField]    Script Date: 06/05/2007 11:22:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocTemplateField](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocTemplateVerID] [int] NOT NULL,
	[FieldID] [int] NOT NULL,
	[PDFFieldName] [nvarchar](100) NOT NULL,
	[Type] [int] NOT NULL,
	[DataFormatID] [int] NULL,
	[GroupIndex] [int] NOT NULL CONSTRAINT [DF_DocTemplateField_GroupIndex]  DEFAULT ((0))
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LenderSpecificField]    Script Date: 06/05/2007 11:27:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LenderSpecificField](
	[CompanyId] [int] NOT NULL,
	[Pastthreeyearservice] [bit] NOT NULL,
	[HowManyAssignsId] [int] NOT NULL,
	[IncludesAssignmentsId] [int] NOT NULL,
	[PercentAssignment] [real] NOT NULL CONSTRAINT [DF_LenderSpecificField_PercentAssignment]  DEFAULT ((0)),
	[SponsoreAgentCode] [varchar](50) NOT NULL CONSTRAINT [DF_Table_1_SponsoreAgentCode]  DEFAULT (''),
	[BlankIncludesId] [int] NOT NULL,
	[WeAreAble] [bit] NOT NULL,
	[WeAssignServicing] [bit] NOT NULL,
	[WeDontServiceMortgageLoans] [bit] NOT NULL,
	[WeMayAssign] [bit] NOT NULL,
	[YouWillBeInformed] [bit] NOT NULL,
	[TransferedPercentageId] [int] NOT NULL,
	[ServiceYourLoanId] [int] NOT NULL,
	[Address1] [varchar](256) NOT NULL CONSTRAINT [DF_LenderSpecificField_Address1]  DEFAULT (''),
	[Address2] [varchar](256) NOT NULL CONSTRAINT [DF_LenderSpecificField_Address2]  DEFAULT (''),
	[City] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_City]  DEFAULT (''),
	[Name] [varchar](256) NOT NULL CONSTRAINT [DF_LenderSpecificField_Name]  DEFAULT (''),
	[PhoneNumber] [varchar](20) NOT NULL CONSTRAINT [DF_LenderSpecificField_PhoneNumber]  DEFAULT (''),
	[StateId] [int] NOT NULL,
	[Zip] [varchar](10) NOT NULL CONSTRAINT [DF_LenderSpecificField_Zip]  DEFAULT (''),
	[Year1] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_Year1]  DEFAULT (''),
	[Year1Percentage] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_Year1Percentage]  DEFAULT (''),
	[Year2] [varchar](50) NOT NULL,
	[Year2Percentage] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_Year2Percentage]  DEFAULT (''),
	[Year3] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_Year3]  DEFAULT (''),
	[Year3Percentage] [varchar](50) NOT NULL CONSTRAINT [DF_LenderSpecificField_Year3Percentage]  DEFAULT (''),
 CONSTRAINT [PK_LenderSpecificField_1] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MortgageProfileAlert]    Script Date: 06/05/2007 11:28:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfileAlert](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileId] [int] NOT NULL,
	[RuleAlertId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_MortgageAlert_IsActive]  DEFAULT ((1)),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_MortgageAlert_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_MortgageAlert] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfileEvent]    Script Date: 06/05/2007 11:29:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfileEvent](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileId] [int] NOT NULL,
	[RuleEventId] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_MortgageProfileEvent_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_MortgageProfileEvent] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskNote]    Script Date: 06/05/2007 11:36:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TaskNote](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaskID] [int] NULL,
	[MortgageId] [int] NOT NULL,
	[Note] [varchar](max) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_TaskNote_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_TaskNote] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConditionTask]    Script Date: 06/05/2007 11:21:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConditionTask](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ConditionID] [int] NOT NULL,
	[TaskID] [int] NOT NULL,
 CONSTRAINT [PK_ConditionTask] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleTemplateField]    Script Date: 06/05/2007 11:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleTemplateField](
	[RoleId] [int] NOT NULL,
	[FieldId] [int] NOT NULL,
	[ProfileStatusId] [int] NOT NULL CONSTRAINT [DF_RoleTemplateField_ProfileStatusId]  DEFAULT ((1)),
 CONSTRAINT [PK_RoleTemplateField] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[FieldId] ASC,
	[ProfileStatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleField]    Script Date: 06/05/2007 11:32:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleField](
	[RoleId] [int] NOT NULL,
	[FieldId] [int] NOT NULL,
	[Companyid] [int] NOT NULL,
	[ProfileStatusid] [int] NOT NULL CONSTRAINT [DF_RoleField_ProfileStatusid]  DEFAULT ((1)),
 CONSTRAINT [PK_RoleField] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[FieldId] ASC,
	[Companyid] ASC,
	[ProfileStatusid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 06/05/2007 11:35:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Task](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[StatusID] [int] NULL,
	[Title] [nvarchar](30) NOT NULL,
	[AssignedTo] [int] NULL,
	[Description] [nvarchar](max) NOT NULL,
	[ScheduleDate] [datetime] NULL,
	[CompleteDate] [datetime] NULL,
	[RecurrenceID] [int] NULL,
	[InfoSourceID] [int] NOT NULL,
	[DifficultyID] [int] NOT NULL,
	[EstimatedAttempts] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Task_Created]  DEFAULT (getdate()),
	[RuleTaskId] [int] NULL CONSTRAINT [DF_Task_RuleTaskId]  DEFAULT ((0)),
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RuleTask]    Script Date: 06/05/2007 11:34:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleTask](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Description] [varchar](512) NOT NULL,
	[TaskTypeId] [int] NOT NULL,
	[TaskInfoSourceId] [int] NOT NULL,
	[TaskDifficultyId] [int] NOT NULL,
 CONSTRAINT [PK_RuleTask] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RuleUnit]    Script Date: 06/05/2007 11:35:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleUnit](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[RuleId] [int] NOT NULL,
	[Sequence] [int] NOT NULL,
	[LogicalOpId] [smallint] NOT NULL,
	[FieldId] [int] NOT NULL,
	[CompareOpId] [smallint] NOT NULL,
	[DataValue] [varchar](256) NOT NULL,
	[RefId] [int] NOT NULL,
	[LogicalNot] [bit] NOT NULL CONSTRAINT [DF_RuleUnit_LogicalNot]  DEFAULT ((0)),
	[LiteralValue] [bit] NOT NULL CONSTRAINT [DF_RuleUnit_LiteralValue]  DEFAULT ((1)),
 CONSTRAINT [PK_RuleUnit_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RuleCheckListItem]    Script Date: 06/05/2007 11:33:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleCheckListItem](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CheckListId] [int] NOT NULL,
	[Question] [varchar](256) NOT NULL,
	[ProfileStatusId] [int] NOT NULL,
	[cbYes] [bit] NOT NULL,
	[cbNo] [bit] NOT NULL,
	[cbDontKnow] [bit] NOT NULL,
	[cbToFollow] [bit] NOT NULL,
 CONSTRAINT [PK_RuleCheckListItem] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 06/05/2007 11:37:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](256) NOT NULL,
	[Password] [varchar](16) NOT NULL,
	[UserStatusId] [int] NOT NULL CONSTRAINT [DF_User_UserStatusId]  DEFAULT (1),
	[CompanyId] [int] NOT NULL CONSTRAINT [DF_User_CompanyId]  DEFAULT (1),
	[FirstName] [varchar](20) NOT NULL CONSTRAINT [DF_User_FirstName]  DEFAULT (''),
	[LastName] [varchar](20) NOT NULL CONSTRAINT [DF_User_LastName]  DEFAULT (''),
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Company]    Script Date: 06/05/2007 11:20:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Company](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[Company] [varchar](100) NOT NULL,
	[LogoImage] [varchar](100) NOT NULL CONSTRAINT [DF_Company_LogoImage]  DEFAULT (''),
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Company_StatusId]  DEFAULT ((1)),
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 06/05/2007 11:31:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL CONSTRAINT [DF_Product_Name]  DEFAULT (''),
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Product_StatusId]  DEFAULT ((1)),
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rule]    Script Date: 06/05/2007 11:33:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rule](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[CompanyId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[StatusId] [int] NOT NULL CONSTRAINT [DF_Rule_StatusId]  DEFAULT ((1)),
	[ParentRuleId] [int] NULL,
 CONSTRAINT [PK_Rule] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DocTemplateRelation]    Script Date: 06/05/2007 11:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocTemplateRelation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocTemplateID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[IsAppPackage] [bit] NOT NULL,
	[IsClosingPackage] [bit] NOT NULL,
	[IsMiscPackage] [bit] NOT NULL,
 CONSTRAINT [PK_DocTemplateRelation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 06/05/2007 11:26:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Invoice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[TypeID] [int] NOT NULL,
	[ProviderID] [int] NOT NULL,
	[IN] [bit] NOT NULL,
	[Invoice] [money] NOT NULL,
	[PMT] [varchar](50) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Invoice_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PropertyCreditor]    Script Date: 06/05/2007 11:32:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyCreditor](
	[id] [int] NOT NULL,
	[PropertyInfoId] [int] NOT NULL,
	[CreditorId] [int] NOT NULL,
 CONSTRAINT [PK_PropertyCreditor] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mail]    Script Date: 06/05/2007 11:27:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Mail](
	[ID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[MorgageID] [int] NULL,
	[MailStatusID] [int] NOT NULL,
	[From] [varchar](500) NOT NULL,
	[To] [varchar](500) NOT NULL,
	[CC] [varchar](500) NOT NULL,
	[Bcc] [varchar](500) NOT NULL,
	[Subject] [varchar](256) NOT NULL,
	[Body] [varchar](1024) NOT NULL,
	[IsBodyHtml] [bit] NOT NULL CONSTRAINT [DF_Mail_IsBodyHtml]  DEFAULT ((1)),
	[Priority] [int] NOT NULL CONSTRAINT [DF_Mail_Priority]  DEFAULT ((0)),
	[Date] [datetime] NOT NULL CONSTRAINT [DF_Mail_Date]  DEFAULT (getdate()),
	[SubjectPageCode] [int] NOT NULL CONSTRAINT [DF_Mail_SubjectPageCode]  DEFAULT ((65001)),
	[BodyPageCode] [int] NOT NULL CONSTRAINT [DF_Mail_TransferEncoding]  DEFAULT ((65001)),
 CONSTRAINT [PK_Mail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MortgageCheckList]    Script Date: 06/05/2007 11:28:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageCheckList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileId] [int] NOT NULL,
	[CheckListItemId] [int] NOT NULL,
	[cbYes] [bit] NULL,
	[cbNo] [bit] NULL,
	[cbDontknow] [bit] NULL,
	[cbToFollow] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfileCheckList]    Script Date: 06/05/2007 11:29:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfileCheckList](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageProfileId] [int] NOT NULL,
	[CheckListItemId] [int] NOT NULL,
	[cbYes] [bit] NULL,
	[cbNo] [bit] NULL,
	[cbDontknow] [bit] NULL,
	[cbToFollow] [bit] NULL,
	[StatusId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfilePackage]    Script Date: 06/05/2007 11:30:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfilePackage](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[PackageName] [nvarchar](100) NOT NULL,
	[UploadDate] [datetime] NOT NULL CONSTRAINT [DF_MortgageProfilePackage_UploadDate_1]  DEFAULT (getdate()),
 CONSTRAINT [PK_MortgageProfilePackage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_MortgageProfilePackage] UNIQUE NONCLUSTERED 
(
	[MortgageID] ASC,
	[Title] ASC,
	[PackageName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Borrower]    Script Date: 06/05/2007 11:19:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Borrower](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[MiddleInitial] [varchar](50) NOT NULL CONSTRAINT [DF_Borrower_MiddleInitial]  DEFAULT (''),
	[DateOfBirth] [datetime] NULL,
	[Address1] [varchar](256) NOT NULL,
	[Address2] [varchar](256) NULL,
	[City] [varchar](50) NOT NULL,
	[StateId] [int] NOT NULL,
	[Zip] [varchar](12) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[SexId] [int] NOT NULL,
	[SSN] [varchar](12) NOT NULL,
	[archived] [bit] NULL CONSTRAINT [DF_Borrower_archived]  DEFAULT ((0)),
	[SalutationId] [int] NOT NULL,
	[MartialStatusId] [int] NOT NULL,
	[AKANames] [varchar](100) NOT NULL CONSTRAINT [DF_Borrower_AKANames]  DEFAULT (''),
	[ActualAge] [int] NOT NULL CONSTRAINT [DF_Borrower_ActualAge]  DEFAULT ((0)),
	[NearestAge] [int] NOT NULL CONSTRAINT [DF_Borrower_NearestAge]  DEFAULT ((0)),
	[YearsAtPresentAddress] [int] NOT NULL CONSTRAINT [DF_Borrower_YearsAtPresentAddress]  DEFAULT ((0)),
	[MonthlyIncome] [money] NOT NULL CONSTRAINT [DF_Borrower_MonthlyIncome]  DEFAULT ((0)),
	[RealEstateAssets] [money] NOT NULL CONSTRAINT [DF_Borrower_RealEstateAssets]  DEFAULT ((0)),
	[AvailableAssets] [money] NOT NULL CONSTRAINT [DF_Borrower_AvailableAssets]  DEFAULT ((0)),
	[DifferentMailingAddress] [bit] NOT NULL CONSTRAINT [DF_Borrower_DifferentMailingAddress]  DEFAULT ((0)),
	[UsePOA] [bit] NOT NULL CONSTRAINT [DF_Borrower_UsePOA]  DEFAULT ((0)),
	[DecJudments] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecJudments]  DEFAULT ((0)),
	[DecBuncruptcy] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecBuncruptcy]  DEFAULT ((0)),
	[DecLawsuit] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecLawsuit]  DEFAULT ((0)),
	[DecFedDebt] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecFedDebt]  DEFAULT ((0)),
	[DecPrimaryres] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecPrimaryres]  DEFAULT ((0)),
	[DecEndorser] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecEndorser]  DEFAULT ((0)),
	[DecUSCitizen] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecUSCitizen]  DEFAULT ((1)),
	[DecPermanentRes] [bit] NOT NULL CONSTRAINT [DF_Borrower_DecPermanentRes]  DEFAULT ((1)),
	[HDMAHide] [bit] NOT NULL CONSTRAINT [DF_Borrower_HDMAHide]  DEFAULT ((1)),
	[HDMARace] [varchar](50) NOT NULL CONSTRAINT [DF_Borrower_HDMARace]  DEFAULT (''),
	[HDMAEthnicity] [varchar](50) NOT NULL CONSTRAINT [DF_Borrower_HDMAEthnicity]  DEFAULT (''),
	[PoaDurable] [bit] NULL,
	[PoaEncumbering] [bit] NULL,
	[PoaRevocable] [bit] NULL,
	[PoaIncapacitated] [bit] NULL,
	[PoaExecutionDate] [datetime] NULL,
 CONSTRAINT [PK_Borrower] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MortgageRoleAssignment]    Script Date: 06/05/2007 11:31:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageRoleAssignment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_MortgageRoleAssignment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Alert]    Script Date: 06/05/2007 11:18:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Alert](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Alert_IsActive]  DEFAULT ((1)),
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Alert_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_Alert] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompanyStructure]    Script Date: 06/05/2007 11:20:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyStructure](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[RoleTemplateId] [int] NOT NULL CONSTRAINT [DF_CompanyPosition_PositionTitle]  DEFAULT (''),
	[ParentId] [int] NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_CompanyPosition] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 06/05/2007 11:37:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[roleId] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_UserRole] UNIQUE NONCLUSTERED 
(
	[roleId] ASC,
	[userId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Condition]    Script Date: 06/05/2007 11:20:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Condition](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[Title] [varchar](100) NOT NULL,
	[Details] [varchar](250) NOT NULL,
	[SignOffLevelID] [int] NOT NULL CONSTRAINT [DF_Condition_SignOffLevelID]  DEFAULT ((1)),
	[TypeID] [int] NOT NULL CONSTRAINT [DF_Condition_TypeID]  DEFAULT ((1)),
	[CategoryID] [int] NOT NULL CONSTRAINT [DF_Condition_CategoryID]  DEFAULT ((1)),
	[StatusID] [int] NOT NULL CONSTRAINT [DF_Condition_StatusID]  DEFAULT ((1)),
	[Notes] [varchar](250) NULL,
	[CreatedBy] [int] NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Condition_Created]  DEFAULT (getdate()),
	[RuleConditionId] [int] NULL CONSTRAINT [DF_Condition_RuleConditionId]  DEFAULT ((0)),
 CONSTRAINT [PK_Condition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CompareFieldOpForbidden]    Script Date: 06/05/2007 11:20:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompareFieldOpForbidden](
	[CompareOpId] [smallint] NOT NULL,
	[ValueTypeId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfile]    Script Date: 06/05/2007 11:28:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageProfile](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[PropertyInfoId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_PropertyInfoId]  DEFAULT ((0)),
	[ServicerId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_ServicerId]  DEFAULT ((0)),
	[VendorId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_VendorId]  DEFAULT ((0)),
	[WholesaleLenderId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_WholesaleLenderId]  DEFAULT ((0)),
	[CurProfileStatusID] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_CurProfileStatusID_1]  DEFAULT ((1)),
	[CompanyID] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_MortgageProfile_Created]  DEFAULT (getdate()),
	[ProductId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_ProductId]  DEFAULT ((0)),
	[PropertyId] [int] NOT NULL CONSTRAINT [DF_MortgageProfile_PropertyId]  DEFAULT ((0)),
 CONSTRAINT [PK_Mortgage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProfileField]    Script Date: 06/05/2007 11:29:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MortgageProfileField](
	[id] [int] IDENTITY(0,1) NOT NULL,
	[TableName] [varchar](50) NOT NULL CONSTRAINT [DF_Field_TableName]  DEFAULT (''),
	[FieldName] [varchar](50) NOT NULL CONSTRAINT [DF_Field_FieldName]  DEFAULT (''),
	[Description] [varchar](100) NOT NULL CONSTRAINT [DF_Field_Description]  DEFAULT (''),
	[ValueTypeId] [int] NOT NULL CONSTRAINT [DF_Field_ValueType]  DEFAULT ((0)),
	[DefaultValue] [varchar](100) NOT NULL CONSTRAINT [DF_Fields_DefaultValue]  DEFAULT (''),
	[ControlTypeId] [int] NOT NULL CONSTRAINT [DF_Field_ControlTypeId]  DEFAULT ((0)),
	[DisplayOrder] [int] NOT NULL CONSTRAINT [DF_Field_DisplayOrder]  DEFAULT ((0)),
	[UsedInRules] [bit] NOT NULL CONSTRAINT [DF_Field_UsedInRules]  DEFAULT ((1)),
	[FieldGroupId] [int] NOT NULL,
	[DictionaryField] [bit] NOT NULL CONSTRAINT [DF_MortgageProfileField_DictionaryField]  DEFAULT ((0)),
	[PropertyName] [varchar](256) NOT NULL,
	[ValidationMessage] [varchar](100) NOT NULL CONSTRAINT [DF_MortgageProfileField_ValidationMessage]  DEFAULT (''),
 CONSTRAINT [PK_Fields] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RoleTemplateProfileStatusView]    Script Date: 06/05/2007 11:33:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleTemplateProfileStatusView](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleTemplateID] [int] NOT NULL,
	[ProfileStatusID] [int] NOT NULL,
 CONSTRAINT [PK_RoleTemplateProfileStatusView_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_RoleTemplateProfileStatusView] UNIQUE NONCLUSTERED 
(
	[ProfileStatusID] ASC,
	[RoleTemplateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageStatusHistory]    Script Date: 06/05/2007 11:31:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MortgageStatusHistory](
	[MortgageId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_MortgageStatusHistory_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_MortgageStatusHistory_1] PRIMARY KEY CLUSTERED 
(
	[MortgageId] ASC,
	[StatusId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocTemplateVersion]    Script Date: 06/05/2007 11:22:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocTemplateVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DocTemplateID] [int] NOT NULL,
	[Version] [nvarchar](100) NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[UploadDate] [datetime] NOT NULL CONSTRAINT [DF_DocTemplateVersion_UploadDate]  DEFAULT (getdate()),
	[IsCurrent] [bit] NOT NULL CONSTRAINT [DF_DocTemplateVersion_IsCurrent]  DEFAULT ((1)),
 CONSTRAINT [PK_DocTemplateVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MortgageProperty]    Script Date: 06/05/2007 11:31:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MortgageProperty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Address1] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_Address1]  DEFAULT (''),
	[Address2] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_Address2]  DEFAULT (''),
	[City] [varchar](50) NOT NULL CONSTRAINT [DF_MortgageProperty_City]  DEFAULT (''),
	[StateId] [int] NOT NULL,
	[Zip] [varchar](12) NOT NULL CONSTRAINT [DF_MortgageProperty_Zip]  DEFAULT (''),
	[County] [varchar](50) NOT NULL CONSTRAINT [DF_MortgageProperty_County]  DEFAULT (''),
	[Hazard] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_Hazard]  DEFAULT ((0)),
	[HazDwelling] [int] NOT NULL CONSTRAINT [DF_MortgageProperty_HazDwelling]  DEFAULT ((0)),
	[HazStart] [datetime] NULL,
	[HazExp] [datetime] NULL,
	[HazPremium] [money] NOT NULL CONSTRAINT [DF_MortgageProperty_HazPremium]  DEFAULT ((0)),
	[HazMtgeeClauseOK] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_HazMtgeeClauseOK]  DEFAULT ((0)),
	[HazAllBorrOnPolicy] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_HazAllBorrOnPolicy]  DEFAULT ((0)),
	[HazAgencyName] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_HazAgencyName]  DEFAULT (''),
	[HazAgentName] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_HazAgentName]  DEFAULT (''),
	[HazAgencyPhone] [varchar](20) NOT NULL CONSTRAINT [DF_MortgageProperty_HazAgencyPhone]  DEFAULT (''),
	[HazAgencyFax] [varchar](20) NOT NULL CONSTRAINT [DF_MortgageProperty_HazAgencyFax]  DEFAULT (''),
	[Flood] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_Flood]  DEFAULT ((0)),
	[FldDwelling] [int] NOT NULL CONSTRAINT [DF_MortgageProperty_FldDwelling]  DEFAULT ((0)),
	[FldStart] [datetime] NULL,
	[FldExp] [datetime] NULL,
	[FldPremium] [money] NOT NULL CONSTRAINT [DF_MortgageProperty_FldPremium]  DEFAULT ((0)),
	[FldMtgeeClauseOK] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_FldMtgeeClauseOK]  DEFAULT ((0)),
	[FldAllBorrOnPolicy] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_FldAllBorrOnPolicy]  DEFAULT ((0)),
	[FldAgencyName] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_FldAgencyName]  DEFAULT (''),
	[FldAgentName] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_FldAgentName]  DEFAULT (''),
	[FldAgencyPhone] [varchar](20) NOT NULL,
	[FldAgencyFax] [varchar](20) NOT NULL CONSTRAINT [DF_MortgageProperty_FldAgencyFax]  DEFAULT (''),
	[SPTitleHeldId] [int] NOT NULL CONSTRAINT [DF_MortgageProperty_SPTitleHeldAs]  DEFAULT ((0)),
	[SPTitleIsHeldInTheseNames] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_SPTitleIsHeldInTheseNames]  DEFAULT (''),
	[SPHeldInTrust] [bit] NOT NULL CONSTRAINT [DF_MortgageProperty_SPHeldInTrust]  DEFAULT ((0)),
	[LOTitle] [varchar](256) NOT NULL CONSTRAINT [DF_MortgageProperty_LOTitle]  DEFAULT (''),
	[LOFax] [varchar](20) NOT NULL CONSTRAINT [DF_MortgageProperty_LOFax]  DEFAULT (''),
 CONSTRAINT [PK_MortgageProperty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RuleEvent]    Script Date: 06/05/2007 11:34:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RuleEvent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EventTypeID] [int] NOT NULL,
	[Message] [varchar](256) NOT NULL,
 CONSTRAINT [PK_RuleEvent] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Event]    Script Date: 06/05/2007 11:22:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MortgageID] [int] NOT NULL,
	[TypeID] [int] NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[Created] [datetime] NOT NULL CONSTRAINT [DF_Event_Created]  DEFAULT (getdate()),
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FieldGroupIndex]    Script Date: 06/05/2007 11:23:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FieldGroupIndex](
	[ID] [int] NOT NULL,
	[FieldGroupID] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FieldGroupIndex] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RuleObject]    Script Date: 06/05/2007 11:34:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleObject](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Ruleid] [int] NOT NULL,
	[ObjectId] [int] NOT NULL,
	[ObjectTypeid] [int] NOT NULL,
	[RuleActionId] [int] NOT NULL,
	[ParentId] [int] NULL,
 CONSTRAINT [PK_RuleObject] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RuleProduct]    Script Date: 06/05/2007 11:34:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleProduct](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Ruleid] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_RuleProduct] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MailAttachment]    Script Date: 06/05/2007 11:28:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailAttachment](
	[ID] [int] NOT NULL,
	[MailID] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[ContentType] [nvarchar](100) NOT NULL,
	[NamePageCode] [int] NOT NULL CONSTRAINT [DF_Attachment_NameCodePage]  DEFAULT ((65001)),
	[TransferEncoding] [int] NOT NULL CONSTRAINT [DF_Attachment_TransferEncoding]  DEFAULT ((3)),
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetRoleList]    Script Date: 06/05/2007 11:16:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetRoleList]
	@all 		bit = 0
	,@asc 		bit = 1
	,@companyid int
AS
	IF @all=1
		IF @asc=1		
			SELECT * FROM [Role] 
			WHERE CompanyId=@companyId
			ORDER BY [Name] ASC
		ELSE
			SELECT * FROM [Role] 
			WHERE CompanyId=@companyId
			ORDER BY [Name] DESC
	ELSE
		IF @asc=1		
			SELECT * FROM [Role]
			WHERE id>0 AND CompanyId=@companyId
			ORDER BY [Name] ASC
		ELSE
			SELECT * FROM [Role] 
			WHERE id>0 AND CompanyId=@companyId
			ORDER BY [Name] DESC
GO
/****** Object:  View [dbo].[vwActiveRole]    Script Date: 06/05/2007 11:37:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  VIEW [dbo].[vwActiveRole]
AS
SELECT * FROM [Role] WHERE Statusid=1
GO
/****** Object:  StoredProcedure [dbo].[CopyAllRoleTemplateForCompany_]    Script Date: 06/05/2007 11:14:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyAllRoleTemplateForCompany_]
	@companyid			int
AS
DECLARE @newid		int
DECLARE @roleid		int
DECLARE @rolename 	varchar(50)
DECLARE  cur CURSOR FAST_FORWARD FOR
	SELECT [Id],[Name] FROM RoleTemplate	
	WHERE [Id]>0

	OPEN cur
	FETCH NEXT FROM cur INTO @roleid,@rolename
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO Role(
			[Name]
			,CompanyId
		) VALUES (
			@rolename
			,@companyid
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		INSERT INTO RoleProfileStatus
		SELECT @newid,ProfileInitStatusId,ProfileFinalStatusId
		FROM RoleTemplateProfileStatus
		WHERE Roleid=@roleid
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO RoleField
		SELECT @newid,FieldId
		FROM RoleTemplateField
		WHERE Roleid=@roleid
		IF @@ERROR <> 0 GOTO ErrorHandler
		FETCH NEXT FROM cur INTO @roleid,@rolename
	END
	CLOSE cur
	DEALLOCATE cur
	RETURN 0;
ErrorHandler:
	CLOSE cur
	DEALLOCATE cur
	RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[GetEditableFields]    Script Date: 06/05/2007 11:15:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEditableFields]
	@userid		int
AS
	SELECT DISTINCT rf.FieldId 
	FROM [Role] r
	INNER JOIN RoleField rf ON rf.roleid=r.id
	INNER JOIN UserRole ur ON ur.roleid=r.id and ur.userid=@userid
GO
/****** Object:  StoredProcedure [dbo].[DeleteAllCompanyRole]    Script Date: 06/05/2007 11:14:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAllCompanyRole]
	@companyid     int
AS
	UPDATE [Role]
	SET StatusId = 3
	WHERE Companyid=@companyid
	DELETE ur FROM UserRole ur 
	INNER JOIN [Role] r ON r.id=ur.roleid
	WHERE r.companyId=@companyid
GO
/****** Object:  StoredProcedure [dbo].[SaveRole]    Script Date: 06/05/2007 11:17:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRole]
	@id				int
	,@name			varchar(50)
	,@templateid	int
	,@companyid		int
AS
DECLARE @newid int
DECLARE @cnt int
	BEGIN TRANSACTION tr
	SELECT @cnt = COUNT(*) FROM [Role]
	WHERE @id<>id AND @name=[name] AND @companyid = companyid
	IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- already exists
		RETURN
	END 
	IF @id > 0 BEGIN
		UPDATE Role
		SET [Name]=@name
		WHERE id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
		RETURN
	END ELSE BEGIN
		INSERT INTO Role(
			[Name]
			,CompanyId
		) VALUES (
			@name
			,@companyid
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		INSERT INTO RoleProfileStatus
		SELECT @newid,ProfileInitStatusId,ProfileFinalStatusId
		FROM RoleTemplateProfileStatus
		WHERE Roleid=@templateid
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO RoleField
		SELECT @newid,FieldId
		FROM RoleTemplateField
		WHERE Roleid=@templateid
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @newid
		RETURN
	END

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[DeleteRole]    Script Date: 06/05/2007 11:14:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRole]
	@roleid int
AS
	BEGIN TRANSACTION tr
	UPDATE [Role]
	SET StatusId=3
	WHERE Id=@roleid
	IF @@ERROR<>0 GOTO ErrorHandler
	exec dbo.DeleteUserRoleByRole
	IF @@ERROR<>0 GOTO ErrorHandler
	COMMIT TRANSACTION Tr
	SELECT 1
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  View [dbo].[vwMortgageProfileAlert]    Script Date: 06/05/2007 11:37:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwMortgageProfileAlert]
AS
	SELECT mpa.Id,mpa.MortgageProfileId AS MortgageId
		,ra.Message AS [Description]
		,mpa.IsActive
		,mpa.Created 
	from MortgageProfileAlert mpa
	INNER JOIN RuleAlert ra on ra.id=mpa.rulealertid
GO
/****** Object:  StoredProcedure [dbo].[SaveMPAlerts]    Script Date: 06/05/2007 11:17:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPAlerts]
	@mpid		int
	,@xml		ntext
AS
DECLARE @idoc  int
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 
		RETURN
	BEGIN TRANSACTION Tr
	DELETE FROM MortgageProfileAlert 
	WHERE MortgageProfileId=@mpid 
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF DATALENGTH(@xml) > 0 BEGIN
		INSERT INTO MortgageProfileAlert (
			MortgageProfileId
			,RuleAlertId
		) SELECT
			@mpid
			,ro.ObjectId
			FROM OPENXML (@idoc,'Root/item',1)
			WITH (
				ItemId		int	'@id'	
			) s
		INNER JOIN RuleObject ro ON ro.ruleid=s.ItemId AND ro.ObjectTypeId=6
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetAlert]    Script Date: 06/05/2007 11:14:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlert]
	@id	int
AS
	SELECT mpa.Id,mpa.MortgageProfileId AS MortgageId
		,ra.Message AS [Description]
		,mpa.IsActive
		,mpa.Created 
	from MortgageProfileAlert mpa
	INNER JOIN RuleAlert ra on ra.id=mpa.rulealertid
	WHERE mpa.id=@id
GO
/****** Object:  StoredProcedure [dbo].[UpdateMPAlertRules]    Script Date: 06/05/2007 11:18:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateMPAlertRules]
	@MortgageProfileId	int,
	@xml				ntext
AS

DECLARE @ids  int
EXEC sp_xml_preparedocument @ids OUTPUT, @xml

IF Datalength(@xml)>0
	BEGIN
		insert into MortgageProfileAlert (MortgageProfileId, RuleAlertId)
		SELECT @MortgageProfileId,ro.ObjectId
		FROM RuleObject ro
		INNER JOIN RuleAlert a ON a.Id=ro.ObjectId
		WHERE ro.objecttypeid=6 AND
		ro.ruleid IN (SELECT 
				id
			FROM OPENXML (@ids,'Root/item',1)
			WITH (
				Id     int '@id'
			) s)
		AND ro.ObjectId NOT IN (SELECT mpa.RuleAlertID
		from MortgageProfileAlert mpa
		INNER JOIN RuleAlert ra on ra.id=mpa.rulealertid
		WHERE mpa.MortgageProfileId = @MortgageProfileId)
	END

EXEC sp_xml_removedocument @ids
GO
/****** Object:  StoredProcedure [dbo].[GetFieldsByDocTemplateVerIDs]    Script Date: 06/05/2007 11:15:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[GetFieldsByDocTemplateVerIDs](
	@idXmlDoc		INT 
)
AS

SELECT DISTINCT	docTemplField.[ID], docTemplField.DocTemplateVerID, docTemplField.FieldID, docTemplField.PDFFieldName, 
	docTemplField.Type as TypeID, docTemplField.DataFormatID, mpField.[Description] as MPFiledName, 
	docTemplField.GroupIndex, 
	[Type] = 
	CASE
		WHEN docTemplField.Type = 1 THEN 'Boolean' 
		ELSE 'String' 
	END 
FROM	(
		SELECT	*
		FROM	OPENXML (@idXmlDoc, 'Root/item', 1)
				WITH	(
						ID	int '@id' 
						)
		) AS docTemplate
		INNER JOIN DocTemplateVersion dtVersion ON  dtVersion.DocTemplateID = docTemplate.ID AND dtVersion.IsCurrent = 1
		INNER JOIN DocTemplateField docTemplField ON docTemplField.DocTemplateVerID = dtVersion.[ID]
		INNER JOIN MortgageProfileField mpField ON mpField.[id] = docTemplField.FieldID
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageProfileFieldByDTIDs]    Script Date: 06/05/2007 11:16:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageProfileFieldByDTIDs]
	@dtIDsXml	ntext
AS

DECLARE @idXmlDoc  int
EXEC sp_xml_preparedocument @idXmlDoc OUTPUT, @dtIDsXml
IF @@ERROR <> 0 
	RETURN

SELECT	DISTINCT mpf.*, mpft.[Name]
FROM	(
		SELECT	*
		FROM	OPENXML (@idXmlDoc, 'Root/item', 1)
				WITH	(
						ID	int '@id' 
						)
		) AS docTemplate
		INNER JOIN DocTemplateVersion dtVersion ON  dtVersion.DocTemplateID = docTemplate.ID AND dtVersion.IsCurrent = 1
		INNER JOIN DocTemplateField docTemplField ON docTemplField.DocTemplateVerID = dtVersion.[ID]
		INNER JOIN MortgageProfileField mpf ON mpf.[id] = docTemplField.FieldID
		INNER JOIN MortgageProfileFieldType mpft ON mpf.ValueTypeId = mpft.id

EXEC sp_xml_removedocument @idXmlDoc
GO
/****** Object:  StoredProcedure [dbo].[EditDocTemplateVersion]    Script Date: 06/05/2007 11:14:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditDocTemplateVersion](
 @id   INT,
 @doctemplateid  INT,
 @version  NVARCHAR(100),
 @filename  NVARCHAR(100),
 @iscurrent  BIT
)
AS

DECLARE @Result  INT

IF(@id <= 0)
 BEGIN
-- UPDATE DocTemplateVersion SET IsCurrent = 0 WHERE DocTemplateID = @doctemplateid

 INSERT INTO DocTemplateVersion
  (DocTemplateID, Version, [FileName], IsCurrent) 
 VALUES
  (@doctemplateid, @version, @filename, @iscurrent) 
 SET @Result = @@IDENTITY
 END
ELSE
 BEGIN
  IF(@iscurrent=1)
   BEGIN
    UPDATE DocTemplateVersion SET IsCurrent = 0 WHERE DocTemplateID = @doctemplateid
   END
  IF (LEN(@FileName)<=0)
   BEGIN
     UPDATE  DocTemplateVersion SET
     Version = @version,
     IsCurrent  = @iscurrent
     WHERE  ID = @id
   END
  ELSE
   BEGIN
     UPDATE  DocTemplateVersion SET
     Version = @version,
     [FileName] = @filename,
     IsCurrent  = @iscurrent
     WHERE  ID = @id
   END
  SET @Result = @@rowcount
 END

SELECT @Result
GO
/****** Object:  StoredProcedure [dbo].[SetDocTemplateVersionCurrent]    Script Date: 06/05/2007 11:18:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[SetDocTemplateVersionCurrent](
 @id				INT,
 @doctemplateid		INT
)
AS

IF(@id > 0)
 BEGIN
 UPDATE DocTemplateVersion SET IsCurrent = 0 WHERE DocTemplateID = @doctemplateid

 UPDATE DocTemplateVersion SET IsCurrent = 1 where id=@id
 END
GO
/****** Object:  StoredProcedure [dbo].[GetDocTemplateVersionList]    Script Date: 06/05/2007 11:15:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDocTemplateVersionList]
 @id  int
AS
 SELECT * FROM DocTemplateVersion WHERE DocTemplateID=@id order by Version
GO
/****** Object:  StoredProcedure [dbo].[SaveMPStatusHistory]    Script Date: 06/05/2007 11:17:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPStatusHistory](
	@id			int
	,@statusId	int
)
AS
	INSERT INTO MortgageStatusHistory(
		MortgageId
		,StatusId
	) VALUES (
		@id
		,@StatusId
	)
	RETURN @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetMPDates]    Script Date: 06/05/2007 11:16:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMPDates]
	@mortgageId	int
AS
	SELECT ps.Name+' Date' AS StatusName
	,CASE WHEN mh.Created IS NULL THEN 'N/A'ELSE Convert(varchar(20),mh.Created,101) END AS ImportantDate			
	FROM profilestatus ps
	LEFT OUTER JOIN mortgagestatushistory mh ON mh.StatusId = ps.Id AND mh.MortgageId=@mortgageId
	WHERE ps.Id>0 
	ORDER BY ps.Id
GO
/****** Object:  StoredProcedure [dbo].[GetLenderSpecificFieldsByCompanyId]    Script Date: 06/05/2007 11:15:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLenderSpecificFieldsByCompanyId](
	@CompanyID   INT
)
AS
	SELECT lsf.*
	,s.Name AS StateName
	,syl.Name AS ServiceYourLoan
	,hma.Name AS HowManyAssigns
	,rbyn1.Name AS IncludeAssignments
	,rbyn2.Name AS BlankIncludes
	,tp.Name AS TransferPercentage
	FROM LenderSpecificField lsf
	INNER JOIN [State] s ON s.Id=lsf.StateId
	INNER JOIN ServiceYourLoan syl ON syl.Id=lsf.ServiceYourLoanId
	INNER JOIN HowManyAssigns hma ON hma.Id=lsf.HowManyAssignsId
	INNER JOIN RBYNValues rbyn1 ON rbyn1.Id=lsf.IncludesAssignmentsId
	INNER JOIN RBYNValues rbyn2 ON rbyn2.Id=lsf.BlankIncludesId
	INNER JOIN TransferPercentage tp ON tp.Id=lsf.TransferedPercentageId
	WHERE CompanyId=@CompanyId
GO
/****** Object:  StoredProcedure [dbo].[GetRuleAlertList]    Script Date: 06/05/2007 11:16:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleAlertList]
	@ruleid		int
AS
		SELECT ro.[Id] AS [Id],a.Message,'Alert' AS Type, 0 AS EventTypeId
		FROM RuleObject ro
		INNER JOIN RuleAlert a ON a.Id=ro.ObjectId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=6
		ORDER BY Id
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleAlert]    Script Date: 06/05/2007 11:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleAlert]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @Message varchar(256)
DECLARE @oldid int
DECLARE @newid int
DECLARE @res   int
	SET @res = 0

	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id,rc.Message
	FROM rulealert rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=6
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid,@Message

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO rulealert (
			[Message]
		) VALUES (
			@Message
		)
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		SET @newid=scope_identity()
		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=6 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid,@Message

	END

StopCursor:
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[DeleteRuleObjectByParent]    Script Date: 06/05/2007 11:14:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRuleObjectByParent]
	@parentid	int
	,@ruleid	int
AS
DECLARE @objtypeid  int
DECLARE @objid  int
DECLARE @cnt int	
	BEGIN TRANSACTION tr
	SELECT @objtypeid=ObjectTypeid,@objid=ObjectId FROM RuleObject WHERE ruleid=@ruleid AND parentId=@parentId
	DELETE FROM RuleObject WHERE ruleid=@ruleid AND parentId=@parentId
	SET @cnt=@@ROWCOUNT
	IF @cnt = 0 BEGIN
		COMMIT TRANSACTION Tr
		SELECT @cnt
		RETURN
	END
	IF @objtypeid = 2 BEGIN
		DELETE FROM RuleCondition WHERE id=@objid		
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 3 BEGIN
		DELETE FROM RuleTask WHERE id=@objid		
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 4 BEGIN
		DELETE FROM DocTemplateRelation WHERE ID=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 5 BEGIN
		DELETE FROM RuleCheckList WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 6 BEGIN
		DELETE FROM RuleAlert WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 7 BEGIN
		DELETE FROM RuleEvent WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 8 BEGIN
		DELETE FROM RuleData WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	COMMIT TRANSACTION Tr
	SELECT @cnt
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[DeleteRuleObject]    Script Date: 06/05/2007 11:14:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRuleObject]
	@id			int
AS
DECLARE @objtypeid  int
DECLARE @objid  int
DECLARE @cnt int	
	BEGIN TRANSACTION tr
	SELECT @objtypeid=ObjectTypeid,@objid=ObjectId FROM RuleObject WHERE id=@id
	DELETE FROM RuleObject WHERE id=@id
	SET @cnt=@@ROWCOUNT
	IF @cnt = 0 BEGIN
		COMMIT TRANSACTION Tr
		SELECT @cnt
		RETURN
	END
	IF @objtypeid = 2 BEGIN
		DELETE FROM RuleCondition WHERE id=@objid		
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 3 BEGIN
		DELETE FROM RuleTask WHERE id=@objid		
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 4 BEGIN
		DELETE FROM DocTemplateRelation WHERE ID=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 5 BEGIN
		DELETE FROM RuleCheckList WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 6 BEGIN
		DELETE FROM RuleAlert WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 7 BEGIN
		DELETE FROM RuleEvent WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	IF @objtypeid = 8 BEGIN
		DELETE FROM RuleData WHERE id=@objid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	COMMIT TRANSACTION Tr
	SELECT @cnt
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleEvent]    Script Date: 06/05/2007 11:14:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleEvent]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @Message varchar(256)
DECLARE @EventTypeID int
DECLARE @oldid int
DECLARE @newid int
DECLARE @res	int
	SET @res = 0

	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id,rc.Message,rc.EventTypeId
	FROM ruleevent rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=7
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid,@Message,@EventTypeId

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO ruleevent (
			[Message]
			,EventTypeId
		) VALUES (
			@Message
			,@EventTypeId
		)
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		SET @newid=scope_identity()
		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=7 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid,@Message,@EventTypeId

	END

StopCursor:
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  View [dbo].[vwMortgageProfileEvent]    Script Date: 06/05/2007 11:37:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwMortgageProfileEvent]
AS
	SELECT mpe.Id,mpe.MortgageProfileId AS MortgageId
		,re.Message AS [Description]
		,et.[Name]
		,mpe.Created 
	FROM MortgageProfileEvent mpe
	INNER JOIN RuleEvent re on re.id=mpe.ruleeventid
	INNER JOIN EventType et on et.id=re.eventtypeid
GO
/****** Object:  StoredProcedure [dbo].[UpdateMPTaskRules]    Script Date: 06/05/2007 11:18:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateMPTaskRules]
	@MortgageProfileId	int,
	@xml				ntext
AS
DECLARE @ids  int


 IF Datalength(@xml)>0
	EXEC sp_xml_preparedocument @ids OUTPUT, @xml

	BEGIN
		insert into Task (MortgageID, RuleTaskId, Title, [Description], InfoSourceID, DifficultyID)
		SELECT @MortgageProfileId, ro.ObjectId, rt.Title, rt.Description, rt.TaskInfoSourceId, rt.TaskDifficultyId
		FROM RuleObject ro
		INNER JOIN RuleTask rt ON rt.Id=ro.ObjectId
		WHERE ro.objecttypeid=3 AND
		ro.ruleid IN (SELECT 
				id
			FROM OPENXML (@ids,'Root/item',1)
			WITH (
				Id     int '@id'
			) s)
		AND ro.ObjectId NOT IN (SELECT t.RuleTaskId
		from Task t
		INNER JOIN RuleTask rt on rt.id=t.RuleTaskId
		WHERE t.MortgageID = @MortgageProfileId)
	END
 EXEC sp_xml_removedocument @ids
GO
/****** Object:  StoredProcedure [dbo].[GetFieldForRole]    Script Date: 06/05/2007 11:15:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetFieldForRole]
	@roleid 	int
	,@groupid	int
	,@statusid	int
	,@companyid int
AS
	SELECT f.id,f.[description],
	CASE WHEN rf.roleid IS NULL THEN 0
		 ELSE 1 END Selected
	FROM MortgageProfileField f
	LEFT OUTER JOIN RoleField rf ON rf.FieldId=f.Id AND rf.roleid=@roleid 
	AND rf.CompanyId=@companyid	AND rf.ProfileStatusId=@statusid
	WHERE f.FieldGroupId=@groupid
GO
/****** Object:  StoredProcedure [dbo].[GetFieldForRoleTemplate]    Script Date: 06/05/2007 11:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetFieldForRoleTemplate]
	@roleid 	int
	,@groupid	int
	,@statusid	int
AS
	SELECT f.id,f.[description],
	CASE WHEN rf.roleid IS NULL THEN 0
		 ELSE 1 END Selected
	FROM MortgageProfileField f
	LEFT OUTER JOIN RoleTemplateField rf ON rf.FieldId=f.Id 
	AND rf.roleid=@roleid AND rf.ProfileStatusId=@statusid
	WHERE f.FieldGroupId=@groupid
GO
/****** Object:  StoredProcedure [dbo].[GetRuleFieldDataList]    Script Date: 06/05/2007 11:16:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleFieldDataList]
AS
	SELECT ro.RuleId,mp.Id,mp.PropertyName,rd.FieldValue FROM RuleObject ro
	INNER JOIN RuleData rd on rd.id=ro.Objectid
	INNER JOIN MortgageProfileField mp ON mp.id=rd.FieldId
	WHERE ro.ObjectTypeId=8
	ORDER BY mp.id
GO
/****** Object:  StoredProcedure [dbo].[GetRuleDependantFieldList]    Script Date: 06/05/2007 11:16:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleDependantFieldList]
AS
	SELECT ro.RuleId AS RuleId,mp.PropertyName AS PropertyName
		,mp.id AS FieldId,'' AS FieldValue, ro.ObjectTypeId AS ObjectTypeID
	FROM RuleObject ro
	INNER JOIN MortgageProfileField mp ON mp.Id=ro.ObjectId
	WHERE objecttypeid=1
	UNION ALL 
	SELECT ro.RuleId AS RuleId,mp.PropertyName AS PropertyName
		,mp.id AS FieldId,rd.FieldValue AS FieldValue, ro.ObjectTypeId AS ObjectTypeID
	FROM RuleObject ro
	INNER JOIN RuleData rd on rd.id=ro.Objectid
	INNER JOIN MortgageProfileField mp ON mp.id=rd.FieldId
	WHERE ro.ObjectTypeId=8
	ORDER BY ro.ObjectTypeId,mp.id
GO
/****** Object:  StoredProcedure [dbo].[GetRuleFieldList]    Script Date: 06/05/2007 11:16:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleFieldList]
AS
	SELECT ro.RuleId,mp.PropertyName,mp.id 
	FROM ruleobject ro
	INNER JOIN MortgageProfileField mp ON mp.Id=ro.ObjectId
	WHERE objecttypeid=1
	ORDER BY mp.id
GO
/****** Object:  StoredProcedure [dbo].[GetObjectRule_]    Script Date: 06/05/2007 11:16:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetObjectRule_]
	@lenderid	int
	,@objecttypeid 	int
AS
	SELECT ru.Id,ru.RuleId,ru.[Sequence], ru.LogicalOpId, ru.CompareOpId, 
	CASE WHEN ru.RefId<0 
		THEN ru.DataValue 
		ELSE CASE WHEN LiteralValue=1
			THEN CAST(ru.RefId AS varchar(256)) 
			ELSE (SELECT PropertyName FROM MortgageProfileField WHERE id=ru.refId) END
	END AS PropertyValue

	,ru.LogicalNot
	,ru.LiteralValue
	,mpf.PropertyName
	,mpf.ValueTypeId AS PropertyType
	FROM Ruleunit ru
	INNER JOIN [Rule] r ON r.id=ruleid AND (r.CompanyId=@lenderid OR r.CompanyId=1) AND r.StatusId=1
	INNER JOIN MortgageProfileField mpf ON mpf.Id=ru.FieldId 
	WHERE EXISTS ( SELECT 1 FROM RuleObject ro WHERE ro.ruleid=ru.ruleid AND ro.objecttypeid=@objecttypeid)
	ORDER BY ru.Ruleid,ru.[Sequence]
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserFromCompanyStructure]    Script Date: 06/05/2007 11:14:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserFromCompanyStructure]
	@nodeid     int
AS
DECLARE @userid int
	BEGIN TRANSACTION Tr
	SELECT @userId=UserId FROM CompanyStructure WHERE Id=@nodeId
	IF @@ROWCOUNT = 0  GOTO ErrorHandler
	UPDATE [User] SET UserStatusId = 3 WHERE Id=@userId
	IF @@ERROR<>0 GOTO ErrorHandler
	exec dbo.DeleteUserRoleByUser @userId
	IF @@ERROR<>0 GOTO ErrorHandler
	DELETE FROM CompanyStructure WHERE ParentId=@nodeId
	DELETE FROM CompanyStructure WHERE Id=@nodeId
	IF @@ERROR<>0 GOTO ErrorHandler
	COMMIT TRANSACTION Tr
	SELECT @@ROWCOUNT
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 06/05/2007 11:14:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUser]
	@userid     int
AS
	BEGIN TRANSACTION Tr
	UPDATE [User]
	SET UserStatusId = 3
	WHERE id=@userid
	IF @@ERROR<>0 GOTO ErrorHandler
	exec dbo.DeleteUserRoleByUser
	IF @@ERROR<>0 GOTO ErrorHandler
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetEvent_]    Script Date: 06/05/2007 11:15:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEvent_]
	@id	int
AS
	
	select e.*, et.Name as EventType from [Event] e 
	inner join EventType et on et.ID = e.TypeID
	where e.id=@id
GO
/****** Object:  StoredProcedure [dbo].[GetMessageBoard_]    Script Date: 06/05/2007 11:15:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMessageBoard_]
	@MortgageID int,
	@ReportedDate datetime
AS
BEGIN
	declare @MessageBoard table (ID varchar(15), Title varchar(50), MessageType varchar(20), Created datetime, IsHighLight bit)
	insert into @MessageBoard
	select 'T&'+ cast(t.ID as varchar(13)) as ID,
			t.Title,
			'Task' as MessageType,
			DateAdd(day,datediff(day, ScheduleDate, @ReportedDate),ScheduleDate) as Created,
			0 as IsHighLight
	from Task t
	where 
		t.MortgageID = @MortgageID and
		((t.RecurrenceID = 6 and DATEDIFF(day, ScheduleDate, @ReportedDate)=0) --Never
		or (DATEDIFF(day,ScheduleDate, @ReportedDate)>=0 and (StatusID = 1 or DATEDIFF(day,CompleteDate,@ReportedDate)<=0) and
			(t.RecurrenceID = 1 --daily
			or (t.RecurrenceID = 2 and DATEDIFF(day,ScheduleDate, @ReportedDate)%2=0 )--every other day
			or (t.RecurrenceID = 3 and DATEPART(Week,ScheduleDate)=DATEPART(Week,@ReportedDate))--Once a week
			or (t.RecurrenceID = 4 and DATEPART(Week,ScheduleDate)=DATEPART(Week,@ReportedDate) and DATEDIFF(Week,ScheduleDate,@ReportedDate)%2=0)--Every other week
			or (t.RecurrenceID = 5 and DATEPART(Month,ScheduleDate)=DATEPART(Month,@ReportedDate))--Once a month
			)
		   )
		 )


	insert into @MessageBoard
	select 'E&'+ cast(e.ID as varchar(13)) as ID,
			et.Name + '(' + e.Description+')' as Title,
			'Event' as MessageType,
			Created,
			0 as IsHighLight
	from [Event] e
	inner join EventType et on et.ID=e.TypeID
	where 
		MortgageID = @MortgageID and
		DATEDIFF(day, Created, @ReportedDate)=0

	insert into @MessageBoard
	select 'A&'+ cast(a.ID as varchar(13)) as ID,
			a.Description as Title,
			'Alert' as MessageType,
			Created,
			a.IsActive as HighLight
	from Alert a
	where 
		MortgageID = @MortgageID and
		DATEDIFF(day, Created, @ReportedDate)=0

	insert into @MessageBoard
	select 'N&'+ cast(n.ID as varchar(13)) as ID,
			n.Note as Title,
			'Note' as MessageType,
			Created,
			0 as HighLight
	from TaskNote n
	where 
		MortgageID = @MortgageID and
		TaskId is null and
		DATEDIFF(day, Created, @ReportedDate)=0

--insert here all other items

	select * from @MessageBoard
	order by Created
END
GO
/****** Object:  StoredProcedure [dbo].[GetTaskDifficultyList]    Script Date: 06/05/2007 11:16:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskDifficultyList]
AS

	select * from TaskDifficulty
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetMPDocTemplateList]    Script Date: 06/05/2007 11:16:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMPDocTemplateList]
	@posRuleXml	ntext
AS

DECLARE @idoc  int
EXEC sp_xml_preparedocument @idoc OUTPUT, @posRuleXml
IF @@ERROR <> 0 
	RETURN

SELECT	positiveRule.[ID] RUID, ro.[ID] ROID, dtRelation.[ID] DTRelID, 
		dt.Title DTTitle, dt.ID DTID, 
		dtRelation.IsAppPackage, dtRelation.IsClosingPackage, dtRelation.IsMiscPackage, 
		dtRelation.GroupID 
FROM	(
		SELECT	*
		FROM	OPENXML (@idoc, 'Root/item', 1)
				WITH	(
						ID	int '@id' 
						)
		) AS positiveRule 
		INNER JOIN RuleObject ro ON ro.Ruleid = positiveRule.ID 
		INNER JOIN DocTemplateRelation dtRelation ON dtRelation.[ID] = ro.ObjectId AND ro.ObjectTypeid = 4 
		INNER JOIN DocTemplate dt ON dt.[ID] = dtRelation.DocTemplateID 

EXEC sp_xml_removedocument @idoc
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleDocument]    Script Date: 06/05/2007 11:14:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleDocument]
	@srcruleid	int, 
	@dstruleid	int
AS

DECLARE	@DocTemplateID		INT
DECLARE	@GroupID			INT
DECLARE	@IsAppPackage		BIT
DECLARE	@IsClosingPackage	BIT
DECLARE	@IsMiscPackage		BIT

DECLARE	@oldid int
DECLARE	@newid int
DECLARE	@res	int
SET		@res = 0

DECLARE cur CURSOR FAST_FORWARD FOR
SELECT	dtr.[ID], dtr.DocTemplateID, dtr.GroupID, dtr.IsAppPackage, dtr.IsClosingPackage, dtr.IsMiscPackage 
FROM	DocTemplateRelation dtr
		INNER JOIN RuleObject ro ON ro.ObjectId = dtr.[ID] AND ro.ObjectTypeid = 4
WHERE	ro.Ruleid = @dstruleid

OPEN cur

FETCH NEXT FROM cur
INTO @oldid, @DocTemplateID, @GroupID, @IsAppPackage, @IsClosingPackage, @IsMiscPackage 

WHILE @@FETCH_STATUS = 0
BEGIN
	INSERT	INTO DocTemplateRelation
			(
			DocTemplateID, 
			GroupID, 
			IsAppPackage, 
			IsClosingPackage, 
			IsMiscPackage 
			)
			VALUES
			(
			@DocTemplateID, 
			@GroupID, 
			@IsAppPackage, 
			@IsClosingPackage, 
			@IsMiscPackage 
			)

	SET	@res = @@ERROR
	IF @res <> 0 
		GOTO StopCursor

	SET	@newid = scope_identity()
	
	UPDATE	RuleObject 
	SET		ObjectId = @newid
	WHERE	RuleId = @dstruleid AND ObjectTypeId = 4 AND ObjectId = @oldid


	SET	@res = @@ERROR
	IF @res <> 0 
		GOTO StopCursor

	FETCH NEXT FROM cur
	INTO @oldid, @DocTemplateID, @GroupID, @IsAppPackage, @IsClosingPackage, @IsMiscPackage 
END


StopCursor:
CLOSE cur
DEALLOCATE cur
RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[GetCompanyStructure]    Script Date: 06/05/2007 11:15:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompanyStructure]
	@companyid	int
AS
	IF EXISTS ( SELECT  1 FROM CompanyStructure WHERE CompanyId=@companyId) BEGIN
			SELECT cs.Id
				,cs.ParentId
				,rt.Abbriviation 
				,cs.RoleTemplateId AS RoleId
				,'('+RTRIM(u.FirstName)+' '+RTRIM(u.LastName)+')' AS UserFullName
				,cs.UserId
			FROM CompanyStructure cs
			INNER JOIN roletemplate rt on rt.id=cs.roletemplateid
			INNER JOIN [User] u ON u.id=cs.UserId
			WHERE cs.CompanyId=@companyid
			ORDER BY cs.ParentId,rt.Abbriviation
	END ELSE BEGIN
			SELECT -1 AS Id
			, NULL AS ParentId
			, Abbriviation=(SELECT RTRIM([Abbriviation])FROM RoleTemplate WHERE Id=18)
			, 18 AS Roleid
			,'(N/A)' AS UserFullName
			, -1 AS UserId
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveUserInCompanyStructrue]    Script Date: 06/05/2007 11:18:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveUserInCompanyStructrue]
	@id        int
	,@parentId	int	
	,@roleId	int
	,@userid	int
	,@login		varchar(256)
	,@password	varchar(16)
	,@firstname	varchar(20)
	,@lastname	varchar(20)
	,@companyid	int
	,@xml		text
AS
DECLARE @cnt int
DECLARE @idnew int
DECLARE @idoc  int	

	BEGIN TRANSACTION Tr
	SELECT @cnt = COUNT(*) FROM [User]
	WHERE @userid<>id AND @login=[login] AND UserStatusId=1
	IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION Tr
		SELECT -1   -- login already exists
		RETURN
	END 
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF (@userid < 0) BEGIN
		INSERT INTO [User](
			Login
			,[Password]
			,FirstName
			,LastName
			,CompanyId
		) VALUES (
			@login
			,@password
			,@firstname
			,@lastname
			,@companyid
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		SET @idnew = scope_identity()
		DELETE FROM UserRole
		WHERE userid=@userid
		IF @@ERROR<>0 GOTO ErrorHandler
		INSERT INTO UserRole(
			userId
			,roleId	
		)
		SELECT
			@idnew
			,s.RoleId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			RoleId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc

	END ELSE BEGIN
		SET @idnew = @userid
		UPDATE [User]
		SET Login=@login
		,[Password]=@password
		,Firstname=@firstname
		,LastName=@lastname
		,CompanyId=@companyid
		WHERE id=@userid
		IF @@ERROR<>0 GOTO ErrorHandler
		DELETE FROM UserRole
		WHERE userid=@userid
		IF @@ERROR<>0 GOTO ErrorHandler
		INSERT INTO UserRole(
			userId
			,roleId	
		)
		SELECT
			@userid
			,s.RoleId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			RoleId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
		IF @@ERROR<>0 GOTO ErrorHandler

	END	
	IF @id < 0 BEGIN
		INSERT INTO CompanyStructure (
			CompanyId
			,RoleTemplateId
			,ParentId
			,UserId
		) VALUES (
			@companyId
			,@roleId
			,@parentId
			,@idnew
		) 
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @idnew = scope_identity()
	END ELSE BEGIN
		UPDATE CompanyStructure
		SET RoleTemplateId=@roleId
			,ParentId=@parentId
			,UserId=@idnew
		WHERE Id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @idnew = @id
	END

	COMMIT TRANSACTION Tr
	SELECT @idnew
	RETURN
	
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageHeadersInDefault]    Script Date: 06/05/2007 11:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        PROCEDURE [dbo].[GetMortgageHeadersInDefault](
	@CompanyID	int, 
	@UserID		int = 0
)AS

SELECT	borr.FirstName, borr.LastName, borr.[id], borr.DateOfBirth, MPByUser.[ID] MortgageProfileID, 
		MPByUser.MortgageProfileUserID, 
		MPByUser.StatusID,  @UserID AS UserID 
INTO	#MPBorrower
FROM	(
		SELECT	DISTINCT	mProf.id ID, mProf.CurProfileStatusID StatusID, CAST(ISNULL(mrAssign.[id], 0) AS BIT) MortgageProfileUserID 
		FROM	MortgageProfile mProf
				LEFT JOIN MortgageRoleAssignment mrAssign ON mrAssign.MortgageId = mProf.[id] AND (@UserID <= 0 OR @UserID = mrAssign.UserId)
				LEFT JOIN [User] us ON us.[id] = mrAssign.UserId AND us.CompanyId = @CompanyID
--				LEFT JOIN MortgageProfileUser mProfUser ON mProfUser.MortgageProfileID = mProf.[id]
--				LEFT JOIN UserRole usRole ON usRole.[ID] = mProfUser.UserRoleID AND (@UserID <= 0 OR @UserID = usRole.userId)
--				LEFT JOIN [User] us ON us.[id] = usRole.userId AND us.CompanyId = @CompanyID 
		WHERE	mProf.CompanyID = @CompanyID 
		) AS MPByUser 
		LEFT JOIN Borrower borr ON borr.MortgageID = MPByUser.[ID]
--		LEFT JOIN MortgageProfileBorrower mpBorr ON mpBorr.MortgageProfileID = MPByUser.[ID]
--		LEFT JOIN Borrower borr ON borr.[id] = mpBorr.BorrowerID

SELECT	mpBorr.MortgageProfileID MPID, mpBorr.StatusID, mpBorr.UserID, 
		mpBorr.MortgageProfileUserID, 
		ISNULL(mpBorr.[id], 0) YBID, ISNULL(mpBorr.FirstName, 'N/A') YBFirstName, ISNULL(mpBorr.LastName, 'N/A') YBLastName 
FROM	#MPBorrower mpBorr
WHERE	mpBorr.[id] IS NULL OR
		mpBorr.[id] = 
		(
		SELECT	TOP 1 mpBorrIDs.[id]
		FROM	(
			SELECT	mpBorrInside1.[id], mpBorrInside1.DateOfBirth
			FROM	#MPBorrower mpBorrInside1
			WHERE	mpBorrInside1.MortgageProfileID = mpBorr.MortgageProfileID
			) AS mpBorrIDs
				INNER JOIN 
					(
					SELECT	MAX(mpBorrInside2.DateOfBirth) MaxDateOfBirth
					FROM	#MPBorrower mpBorrInside2
					WHERE	mpBorrInside2.MortgageProfileID = mpBorr.MortgageProfileID
					) AS mpBorrAge
						ON mpBorrAge.MaxDateOfBirth = mpBorrIDs.DateOfBirth
		)


DROP TABLE #MPBorrower
GO
/****** Object:  StoredProcedure [dbo].[GetMPAssignedUser]    Script Date: 06/05/2007 11:16:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMPAssignedUser]
	@mid	int
AS
	SELECT * FROM MortgageRoleAssignment
	WHERE MortgageId=@mid
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageHeaders]    Script Date: 06/05/2007 11:15:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE        PROCEDURE [dbo].[GetMortgageHeaders](
	@CompanyID	int, 
	@UserID		int = 0
)AS

SELECT	borr.FirstName, borr.LastName, borr.[id], borr.DateOfBirth, MPByUser.[ID] MortgageProfileID, 
		MPByUser.MortgageProfileUserID, 
		MPByUser.StatusID,  @UserID AS UserID 
INTO	#MPBorrower
FROM	(
		SELECT	DISTINCT	mProf.id ID, mProf.CurProfileStatusID StatusID, CAST(mrAssign.[id] AS BIT) MortgageProfileUserID 
		FROM	MortgageProfile mProf
				INNER JOIN MortgageRoleAssignment mrAssign ON mrAssign.MortgageId = mProf.[id] AND (@UserID <= 0 OR @UserID = mrAssign.UserId)
				INNER JOIN [User] us ON us.[id] = mrAssign.UserId AND us.CompanyId = @CompanyID	
		WHERE	mProf.CompanyID = @CompanyID 
		) AS MPByUser
		INNER JOIN Borrower borr ON borr.MortgageID = MPByUser.[ID]
--FROM	Borrower borr
--	INNER JOIN MortgageProfileBorrower mpBorr ON mpBorr.BorrowerID = borr.[id] 
--	INNER JOIN 
--				(
--				SELECT	DISTINCT	mProf.id ID, mProf.CurProfileStatusID StatusID, CAST(mProfUser.[ID] AS BIT) MortgageProfileUserID 
--				FROM	MortgageProfile mProf
--						INNER JOIN MortgageProfileUser mProfUser ON mProfUser.MortgageProfileID = mProf.[id]
--						INNER JOIN UserRole usRole ON usRole.[ID] = mProfUser.UserRoleID AND (@UserID <= 0 OR @UserID = usRole.userId)
--						INNER JOIN [User] us ON us.[id] = usRole.userId AND us.CompanyId = @CompanyID
--				WHERE	mProf.CompanyID = @CompanyID 
--				) AS MPByUser ON MPByUser.[ID] = mpBorr.MortgageProfileID

SELECT	mpBorr.MortgageProfileID MPID, mpBorr.[id] YBID, mpBorr.FirstName YBFirstName, mpBorr.LastName YBLastName, 
		mpBorr.MortgageProfileUserID, 
		mpBorr.StatusID, mpBorr.UserID
FROM	#MPBorrower mpBorr
WHERE	mpBorr.[id] = 
		(
		SELECT	TOP 1 mpBorrIDs.[id]
		FROM	(
			SELECT	mpBorrInside1.[id], mpBorrInside1.DateOfBirth
			FROM	#MPBorrower mpBorrInside1
			WHERE	mpBorrInside1.MortgageProfileID = mpBorr.MortgageProfileID
			) AS mpBorrIDs
				INNER JOIN 
					(
					SELECT	MAX(mpBorrInside2.DateOfBirth) MaxDateOfBirth
					FROM	#MPBorrower mpBorrInside2
					WHERE	mpBorrInside2.MortgageProfileID = mpBorr.MortgageProfileID
					) AS mpBorrAge
						ON mpBorrAge.MaxDateOfBirth = mpBorrIDs.DateOfBirth
		)


DROP TABLE #MPBorrower
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleUnit]    Script Date: 06/05/2007 11:18:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleUnit]
	@id		int
	,@ruleid		int
	,@LogicalOpId	smallint
	,@FieldId	int
	,@CompareOpId	smallint
	,@DataValue	varchar(256)
	,@RefId	int
	,@LogicalNot	bit
	,@LiteralValue	bit
AS
DECLARE @seq int
	IF @id < 0 BEGIN
		BEGIN TRANSACTION Tr
		SELECT @seq = ISNULL(MAX([Sequence]),-1)+1 FROM RuleUnit WHERE RuleId=@ruleid
		INSERT INTO RuleUnit (
			RuleId
			,[Sequence]			
			,LogicalOpId
			,FieldId
			,CompareOpId
			,DataValue
			,RefId
			,LogicalNot
			,LiteralValue
		) VALUES (
			@ruleid
			,@seq
			,@LogicalOpId
			,@FieldId
			,@CompareOpId
			,@DataValue
			,@RefId
			,@LogicalNot
			,@LiteralValue
		)
		COMMIT TRANSACTION Tr
		SELECT scope_identity()
	END
GO
/****** Object:  StoredProcedure [dbo].[GetRuleUnitList]    Script Date: 06/05/2007 11:16:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleUnitList]
	@ruleid  int
AS
DECLARE @cnt int
	SELECT fieldId,DataValue,Refid,LiteralValue
	INTO #fielddata 
	FROM ruleunit 
	WHERE ruleid=@ruleid
	SELECT @cnt=count(*) FROM #fielddata
	WHERE refid>0
	IF @cnt>0 BEGIN
		DECLARE @table varchar(50)
		DECLARE @field varchar(50)
		DECLARE @refId int
		DECLARE @fieldId int
		DECLARE @sql nvarchar(2000)
		DECLARE  cur CURSOR FAST_FORWARD FOR
			SELECT mp.TableName,mp.FieldName,fd.refid,fd.FieldId FROM #fielddata fd
			INNER JOIN MortgageProfileField mp on mp.id=fd.fieldid
			WHERE fd.refid>0 AND fd.LiteralValue=1
		OPEN cur
		FETCH NEXT FROM cur
		INTO @table,@field,@refid,@fieldId
		WHILE @@FETCH_STATUS = 0 BEGIN
			SET @sql = 'UPDATE #fielddata SET DataValue=(SELECT cast('+@field+' as varchar(256)) FROM '+@table + ' WHERE id='+CAST(@refid as varchar(10))+')'
			SET @sql = @sql + ' WHERE fieldid='+CAST(@fieldId as varchar(10))
			EXEC sp_executesql @sql
			FETCH NEXT FROM cur
			INTO @table,@field,@refid,@fieldId
		END
		CLOSE cur
		DEALLOCATE cur
	END
	SELECT ru.Id,ru.[Sequence]
	,CASE WHEN ru.LogicalNot=1 THEN 'NOT' ELSE '' END AS LogicalNot
	,lo.[Name] LogicalOp, co.[Name] CompareOp
	,CASE WHEN ru.LiteralValue=1 THEN fd.DataValue ELSE '-' END AS DataValue
	,mp.[Description]
	,CASE WHEN ru.LiteralValue=1 THEN '-' ELSE fd.DataValue END AS FieldName
	,ru.LiteralValue as LiteralValue
	FROM RuleUnit ru
	INNER JOIN LogicalOperation lo on lo.id=ru.LogicalOpId
	INNER JOIN CompareOperation co on co.id=ru.CompareOpId
	INNER JOIN #fielddata fd on fd.fieldid=ru.fieldid
	INNER JOIN MortgageProfileField mp on mp.id=ru.fieldid
	WHERE ru.ruleid=@ruleId
	ORDER BY ru.[Sequence]
	DROP TABLE #fielddata
GO
/****** Object:  StoredProcedure [dbo].[SaveLenderSpecificFields]    Script Date: 06/05/2007 11:17:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveLenderSpecificFields](
	@CompanyID   INT
	,@Pastthreeyearservice bit
	,@HowManyAssignsId int
	,@IncludesAssignmentsId int
	,@PercentAssignment float
	,@SponsoreAgentCode varchar(50)
	,@BlankIncludesId int
	,@WeAreAble bit
	,@WeAssignServicing bit
	,@WeDontServiceMortgageLoans bit
	,@WeMayAssign bit
	,@YouWillBeInformed bit
	,@TransferedPercentageId int
	,@ServiceYourLoanId int
	,@Address1 varchar(256)
	,@Address2 varchar(256)
	,@City varchar(50)
	,@Name varchar(256)
	,@PhoneNumber varchar(20)
	,@StateId int
	,@Zip varchar(10)
	,@Year1 varchar(50)
	,@Year1Percentage varchar(50)
	,@Year2 varchar(50)
	,@Year2Percentage varchar(50)
	,@Year3 varchar(50)
	,@Year3Percentage varchar(50)
)
AS
	BEGIN TRANSACTION Tr
	IF EXISTS (SELECT * FROM LenderSpecificField WHERE CompanyId=@companyId) BEGIN
		UPDATE LenderSpecificField
		SET Pastthreeyearservice = @Pastthreeyearservice
			,HowManyAssignsId=@HowManyAssignsId
			,IncludesAssignmentsId=@IncludesAssignmentsId
			,PercentAssignment=@PercentAssignment
			,SponsoreAgentCode=@SponsoreAgentCode
			,BlankIncludesId=@BlankIncludesId
			,WeAreAble=@WeAreAble
			,WeAssignServicing=@WeAssignServicing
			,WeDontServiceMortgageLoans=@WeDontServiceMortgageLoans
			,WeMayAssign=@WeMayAssign
			,YouWillBeInformed=@YouWillBeInformed
			,TransferedPercentageId=@TransferedPercentageId
			,ServiceYourLoanId=@ServiceYourLoanId
			,Address1=@Address1
			,Address2=@Address2
			,City=@City
			,[Name]=@Name
			,PhoneNumber=@PhoneNumber
			,StateId=@StateId
			,Zip=@Zip
			,Year1=@Year1
			,Year1Percentage=@Year1Percentage
			,Year2=@Year2
			,Year2Percentage=@Year2Percentage
			,Year3=@Year3
			,Year3Percentage=@Year3Percentage
		WHERE CompanyId=@companyId
	END
	ELSE BEGIN
		INSERT INTO LenderSpecificField (
			CompanyID
			,Pastthreeyearservice
			,HowManyAssignsId
			,IncludesAssignmentsId
			,PercentAssignment
			,SponsoreAgentCode
			,BlankIncludesId
			,WeAreAble
			,WeAssignServicing
			,WeDontServiceMortgageLoans
			,WeMayAssign
			,YouWillBeInformed
			,TransferedPercentageId
			,ServiceYourLoanId
			,Address1
			,Address2
			,City
			,[Name]
			,PhoneNumber
			,StateId
			,Zip
			,Year1
			,Year1Percentage
			,Year2
			,Year2Percentage
			,Year3
			,Year3Percentage
		) VALUES (
			@CompanyID
			,@Pastthreeyearservice
			,@HowManyAssignsId
			,@IncludesAssignmentsId
			,@PercentAssignment
			,@SponsoreAgentCode
			,@BlankIncludesId
			,@WeAreAble
			,@WeAssignServicing
			,@WeDontServiceMortgageLoans
			,@WeMayAssign
			,@YouWillBeInformed
			,@TransferedPercentageId
			,@ServiceYourLoanId
			,@Address1
			,@Address2
			,@City
			,@Name
			,@PhoneNumber
			,@StateId
			,@Zip
			,@Year1
			,@Year1Percentage
			,@Year2
			,@Year2Percentage
			,@Year3
			,@Year3Percentage
		)
	END
	COMMIT TRANSACTION Tr
	SELECT @Companyid
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetRuleDataList]    Script Date: 06/05/2007 11:16:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleDataList]
	@ruleid  int
AS
DECLARE @cnt int

	SELECT ro.id,mpf.[Description] AS FieldName,rd.FieldId,rd.FieldValue,mpf.dictionaryfield AS Ref
	INTO #fielddata
	FROM ruleobject ro
	INNER JOIN Ruledata rd on rd.id=ro.objectid
	INNER JOIN MortgageProfileField mpf ON mpf.[Id]=rd.FieldId
	WHERE ro.objecttypeid=8 AND ro.RuleId=@ruleid
	SELECT @cnt=count(*) FROM #fielddata
	WHERE ref>0
	IF @cnt>0 BEGIN
		DECLARE @table varchar(50)
		DECLARE @field varchar(50)
		DECLARE @refId varchar(256)
		DECLARE @fieldId int
		DECLARE @sql nvarchar(2000)
		DECLARE  cur CURSOR FAST_FORWARD FOR
			SELECT mp.TableName,mp.FieldName,fd.FieldValue,fd.FieldId FROM #fielddata fd
			INNER JOIN MortgageProfileField mp on mp.id=fd.fieldid
			WHERE fd.ref>0
		OPEN cur
		FETCH NEXT FROM cur
		INTO @table,@field,@refid,@fieldId
		WHILE @@FETCH_STATUS = 0 BEGIN
			SET @sql = 'UPDATE #fielddata SET Ref=0, FieldValue=(SELECT cast('+@field+' as varchar(256)) FROM '+@table + ' WHERE id='+RTRIM(@refid)+')'
			SET @sql = @sql + ' WHERE fieldid='+CAST(@fieldId as varchar(10))
			EXEC sp_executesql @sql
			FETCH NEXT FROM cur
			INTO @table,@field,@refid,@fieldId
		END
		CLOSE cur
		DEALLOCATE cur
	END
	SELECT [Id],FieldName,FieldValue FROM #fielddata
	DROP TABLE #fielddata
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleData]    Script Date: 06/05/2007 11:14:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleData]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @FieldValue varchar(256)
DECLARE @FieldId int
DECLARE @oldid int
DECLARE @newid int
DECLARE @res	int
	SET @res = 0

	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id,rc.FieldId,rc.Fieldvalue
	FROM ruledata rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=8
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid,@FieldId,@FieldValue

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO ruledata (
			FieldId
			,FieldValue
		) VALUES (
			@FieldId
			,@FieldValue
		)
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		SET @newid=scope_identity()
		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=8 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid,@FieldId,@FieldValue

	END

StopCursor:
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[GetLogicOperationList]    Script Date: 06/05/2007 11:15:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLogicOperationList]
AS
	SELECT * FROM LogicalOperation
	ORDER BY [Id]
GO
/****** Object:  StoredProcedure [dbo].[GetMPCheckList]    Script Date: 06/05/2007 11:16:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMPCheckList]
	@mpid		int
	,@mpstatusid int
	,@xml		ntext
AS
DECLARE @idoc  int
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 
		RETURN
	SELECT ro.ruleid ,rcli.Question,rcli.cbYes,rcli.cbNo,rcli.cbDontknow,rcli.cbTofollow
	,ISNULL(mpcl.cbYes,cast(0 as bit)) AS YesValue, ISNULL(mpcl.cbNo,cast(0 as bit)) AS NoValue
	,ISNULL(mpcl.cbDontknow,cast(0 as bit)) AS DontknowValue,ISNULL(mpcl.cbToFollow,cast(0 as bit)) AS TofollowValue
	,rcli.Id AS ItemId
	,rcli.ProfileStatusId AS StatusId
	FROM ruleobject ro
	INNER JOIN rulechecklist rcl ON rcl.id=ro.objectid
	INNER JOIN rulechecklistitem rcli ON rcli.checklistid=rcl.id 
	LEFT OUTER JOIN MortgageProfileCheckList mpcl ON mpcl.CheckListItemId = rcli.Id 
	WHERE ro.objecttypeid=5 AND 
	rcli.ProfileStatusID=@mpstatusid AND
	ro.ruleid IN (SELECT 
			id
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			Id     int '@id'
		) s)
	EXEC sp_xml_removedocument @idoc
GO
/****** Object:  View [dbo].[vwRuleCheckList]    Script Date: 06/05/2007 11:37:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  VIEW [dbo].[vwRuleCheckList]
AS
SELECT rcl.id
	,rcli.Question,rcli.ProfileStatusid,rcli.cbYes,rcli.cbNo,rcli.cbDontKnow,rcli.cbToFollow 
	,pf.[Name]
FROM rulechecklist rcl
INNER join rulechecklistitem rcli on rcli.checklistid=rcl.id
INNER join profilestatus pf on pf.id=rcli.profilestatusid
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleCheckList]    Script Date: 06/05/2007 11:14:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleCheckList]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @oldid int
DECLARE @newid int
DECLARE @res	int
	SET @res = 0	

	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id
	FROM rulechecklist rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=5
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid

	CREATE TABLE #checklistitem(
		[CheckListId] [int] NOT NULL,
		[Question] [varchar](256) NOT NULL,
		[ProfileStatusId] [int] NOT NULL,
		[cbYes] [bit] NOT NULL,
		[cbNo] [bit] NOT NULL,
		[cbDontKnow] [bit] NOT NULL,
		[cbToFollow] [bit] NOT NULL
	)

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO rulechecklist DEFAULT VALUES
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor
		SET @newid=scope_identity()
		INSERT #checklistitem 
		SELECT CheckListId,Question,ProfileStatusId,cbYes,cbNo,cbDontKnow,cbTofollow
		FROM rulechecklistitem WHERE CheckListId=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor
		UPDATE #checklistitem SET CheckListId=@newid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor
		INSERT rulechecklistitem 
		SELECT * FROM #checklistitem
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor
		DELETE FROM #checklistitem

		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=5 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid

	END

StopCursor:
	DROP TABLE #checklistitem
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[GetMPCheckListStatusList]    Script Date: 06/05/2007 11:16:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMPCheckListStatusList]
AS
BEGIN
	SELECT DISTINCT rcli.ProfileStatusId AS id
	FROM ruleobject ro
	INNER JOIN rulechecklist rcl ON rcl.id=ro.objectid
	INNER JOIN rulechecklistitem rcli ON rcli.checklistid=rcl.id 
	LEFT OUTER JOIN MortgageProfileCheckList mpcl ON mpcl.CheckListItemId = rcli.Id 
	WHERE ro.objecttypeid=5
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteRuleCheckList]    Script Date: 06/05/2007 11:14:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRuleCheckList]
	@ruleid		int
	,@objectid	int
AS
	BEGIN TRANSACTION tr
	DELETE FROM RuleCheckList WHERE id=@objectid
	IF @@ERROR <> 0 GOTO ErrorHandler
	DELETE FROM RuleObject WHERE RuleId=@ruleid AND ObjectID=@objectId
	IF @@ERROR <> 0 GOTO ErrorHandler
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -1
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetTaskTypeList]    Script Date: 06/05/2007 11:16:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskTypeList]
AS

	select t.*,
		case when t.ParentID is null then t.Name else pt.Name + ': ' + t.Name end as FullName
	from TaskType t
	left outer join TaskType pt on pt.ID = t.ParentID
	where t.IsSelectable = 1
	order by t.DisplayOrder, t.Name
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleTask]    Script Date: 06/05/2007 11:14:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleTask]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @Title varchar(100)
DECLARE @Description varchar(512)
DECLARE @TaskTypeId int
DECLARE @TaskInfoSourceId int
DECLARE @TaskDifficultyId int
DECLARE @oldid int
DECLARE @newid int
DECLARE @res	int
	SET @res = 0

	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id,rc.Title,rc.Description,rc.TaskTypeId,rc.TaskInfoSourceId,rc.TaskDifficultyId
	FROM ruletask rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=3
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid,@Title,@Description,@TaskTypeId,@TaskInfoSourceId,@TaskDifficultyId

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO ruletask (
			Title
			,[Description]
			,TaskTypeId
			,TaskInfoSourceId
			,TaskDifficultyId
		) VALUES (
			@Title
			,@Description
			,@TaskTypeId
			,@TaskInfoSourceId
			,@TaskDifficultyId
		)
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		SET @newid=scope_identity()
		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=3 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid,@Title,@Description,@TaskTypeId,@TaskInfoSourceId,@TaskDifficultyId

	END

StopCursor:
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[GetStatusList]    Script Date: 06/05/2007 11:16:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStatusList](
	@all	bit = 0
)
AS
	IF @all=0
		SELECT * FROM Status
		WHERE id<3
		ORDER BY id
	ELSE
		SELECT * FROM Status
		ORDER BY id
GO
/****** Object:  View [dbo].[vwUser]    Script Date: 06/05/2007 11:37:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwUser]
AS
SELECT     u.id, u.Login, u.Password, u.UserStatusId, u.CompanyId, u.FirstName, u.LastName, c.Company, s.StatusName
FROM         dbo.[User] AS u INNER JOIN
                      dbo.Company AS c ON c.id = u.CompanyId INNER JOIN
                      dbo.Status AS s ON s.id = u.UserStatusId AND u.UserStatusId <> 3
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
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 99
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 6
               Left = 416
               Bottom = 84
               Right = 567
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwUser'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwUser'
GO
/****** Object:  View [dbo].[vwAdmin]    Script Date: 06/05/2007 11:37:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwAdmin]
AS
SELECT     u.id, u.Login, u.Password, u.UserStatusId, u.CompanyId, u.FirstName, u.LastName, c.Company, s.StatusName
FROM         dbo.[User] AS u INNER JOIN
                      dbo.Company AS c ON c.id = u.CompanyId INNER JOIN
                      dbo.Status AS s ON s.id = u.UserStatusId INNER JOIN
                      dbo.UserRole AS ur ON ur.userId = u.id
WHERE     (ur.roleId = 1 OR
                      ur.roleId = 2) AND (u.UserStatusId <> 3)
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
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 114
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "c"
            Begin Extent = 
               Top = 6
               Left = 227
               Bottom = 99
               Right = 378
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ur"
            Begin Extent = 
               Top = 114
               Left = 38
               Bottom = 192
               Right = 189
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "s"
            Begin Extent = 
               Top = 6
               Left = 416
               Bottom = 84
               Right = 567
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
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwAdmin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vwAdmin'
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentGroupList]    Script Date: 06/05/2007 11:15:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDocumentGroupList]
AS
	SELECT [id] GroupID, [Name] GroupName FROM DocumentGroup
GO
/****** Object:  StoredProcedure [dbo].[GetInvoiceTypeList]    Script Date: 06/05/2007 11:15:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInvoiceTypeList]
AS

	select * from InvoiceType
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetInvoiceList]    Script Date: 06/05/2007 11:15:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInvoiceList]
	@MortgageID  int
AS

	select i.*, it.Name as Type, ip.Name as Provider from Invoice i
		inner join InvoiceType it on it.ID = i.TypeID
		inner join InvoiceProvider ip on ip.ID = i.ProviderID
	where MortgageID = @MortgageID
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageUserHeaders]    Script Date: 06/05/2007 11:16:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE          PROCEDURE [dbo].[GetMortgageUserHeaders](
	@CompanyID	int, 
	@RoleID		int = -1, 
	@UserID		int = -1 
)AS

SELECT	borr.FirstName, borr.LastName, borr.[id], borr.DateOfBirth, mProf.[id] MortgageProfileID, 
		mProf.CurProfileStatusID StatusID 
INTO	#MPBorrower
FROM	Borrower borr
		INNER JOIN MortgageProfile mProf ON mProf.[id] = borr.MortgageID
--		INNER JOIN MortgageProfileBorrower mpBorr ON mpBorr.BorrowerID = borr.[id] 
--		INNER JOIN MortgageProfile mProf ON mProf.[id] = mpBorr.MortgageProfileID
WHERE	mProf.CompanyID = @CompanyID

SELECT	mpBorr.MortgageProfileID MPID, mpBorr.[id] YBID, mpBorr.FirstName YBFirstName, mpBorr.LastName YBLastName, mpBorr.StatusID 
INTO	#MPYBorrower
FROM	#MPBorrower mpBorr
WHERE	mpBorr.[id] = 
		(
		SELECT	TOP 1 mpBorrIDs.[id]
		FROM	(
			SELECT	mpBorrInside1.[id], mpBorrInside1.DateOfBirth
			FROM	#MPBorrower mpBorrInside1
			WHERE	mpBorrInside1.MortgageProfileID = mpBorr.MortgageProfileID
			) AS mpBorrIDs
				INNER JOIN 
					(
					SELECT	MAX(mpBorrInside2.DateOfBirth) MaxDateOfBirth
					FROM	#MPBorrower mpBorrInside2
					WHERE	mpBorrInside2.MortgageProfileID = mpBorr.MortgageProfileID
					) AS mpBorrAge
						ON mpBorrAge.MaxDateOfBirth = mpBorrIDs.DateOfBirth
		)

SELECT	DISTINCT mpBorr.MPID, mpBorr.YBID, mpBorr.YBFirstName, mpBorr.YBLastName, mpBorr.StatusID, mrAssign.UserID, 
		CAST(mrAssign.[id] AS BIT) MortgageProfileUserID
FROM	#MPYBorrower mpBorr
		INNER JOIN MortgageRoleAssignment mrAssign ON	mrAssign.MortgageId = mpBorr.MPID AND 
														(@UserID <= 0 OR @UserID = mrAssign.UserId) AND 
														(@RoleID <= 0 OR @RoleID = mrAssign.RoleId)
		INNER JOIN [User] us ON us.[id] = mrAssign.UserId AND us.CompanyId = @CompanyID
--		INNER JOIN MortgageProfileUser mpUser ON mpUser.MortgageProfileID = mpBorr.MPID 
--		INNER JOIN UserRole usRole ON usRole.[ID] = mpUser.UserRoleID AND ( @RoleID < 0 OR @RoleID = usRole.roleId )
--		INNER JOIN [User] us ON us.[id] = usRole.userId AND us.CompanyId = @CompanyID 	


DROP TABLE #MPYBorrower 
DROP TABLE #MPBorrower
GO
/****** Object:  StoredProcedure [dbo].[SaveUserAndRoles]    Script Date: 06/05/2007 11:18:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveUserAndRoles]
	@id			int
	,@login		varchar(256)
	,@password	varchar(16)
	,@firstname	varchar(20)
	,@lastname	varchar(20)
	,@companyid	int
	,@xml		text
AS
DECLARE @cnt int
DECLARE @idnew int
DECLARE @idoc  int	

	BEGIN TRANSACTION Tr
	SELECT @cnt = COUNT(*) FROM [User]
	WHERE @id<>id AND @login=login
	IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- login already exists
	END 
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF (@id < 0) BEGIN
		INSERT INTO [User](
			Login
			,[Password]
			,FirstName
			,LastName
			,CompanyId
		) VALUES (
			@login
			,@password
			,@firstname
			,@lastname
			,@companyid
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		SET @idnew = scope_identity()
		INSERT INTO UserRole(
			userId
			,roleId	
		)
		SELECT
			@idnew
			,s.RoleId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			RoleId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
		COMMIT TRANSACTION Tr
		SELECT @idnew
		RETURN
	END ELSE BEGIN
		UPDATE [User]
		SET Login=@login
		,[Password]=@password
		,Firstname=@firstname
		,LastName=@lastname
		,CompanyId=@companyid
		WHERE id=@id
		IF @@ERROR<>0 GOTO ErrorHandler
		DELETE FROM UserRole
		WHERE userid=@id
		IF @@ERROR<>0 GOTO ErrorHandler
		INSERT INTO UserRole(
			userId
			,roleId	
		)
		SELECT
			@id
			,s.RoleId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			RoleId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
		IF @@ERROR<>0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
		RETURN
	END	
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[DeleteAllCompanyUser]    Script Date: 06/05/2007 11:14:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAllCompanyUser]
	@companyid     int
AS
	UPDATE [User]
	SET UserStatusId = 3
	WHERE Companyid=@companyid
	DELETE ur FROM UserRole ur 
	INNER JOIN [User] u ON u.id=ur.userid
	WHERE u.companyId=@companyid
GO
/****** Object:  StoredProcedure [dbo].[ChangeUserStatus]    Script Date: 06/05/2007 11:14:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeUserStatus]
	@userid     int
	,@statusid  int
AS
	UPDATE [User]
	SET UserStatusId = @statusId
	WHERE id=@userid
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetCompanyOperationUser]    Script Date: 06/05/2007 11:15:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompanyOperationUser]
	@companyId	int
AS
	SELECT  u.Id, RTRIM(u.FirstName)+' '+RTRIM(u.LastName) AS UserName
	,ur.Roleid
	FROM [user] u
	INNER JOIN userrole ur ON ur.userId=u.Id
	INNER JOIN RoleTemplate rt ON rt.id=ur.Roleid AND rt.CanBeAssignedToMortgage=1
	WHERE u.UserStatusId=1
	AND u.companyId=@companyId
	ORDER BY UserName
GO
/****** Object:  StoredProcedure [dbo].[SaveUserAndRole]    Script Date: 06/05/2007 11:18:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Returns:
	-1 - login already exists
    -2 - db error
*/
CREATE PROCEDURE [dbo].[SaveUserAndRole]
	@id			int
	,@login		varchar(256)
	,@password	varchar(16)
	,@firstname	varchar(20)
	,@lastname	varchar(20)
	,@companyid	int
	,@roleid	int
AS
DECLARE @cnt int
DECLARE @idnew int
	BEGIN TRANSACTION Tr
	SELECT @cnt = COUNT(*) FROM [User]
	WHERE @id<>id AND @login=login
	IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- login already exists
	END 
	IF (@id < 0) BEGIN
		INSERT INTO [User](
			Login
			,[Password]
			,FirstName
			,LastName
			,CompanyId
		) VALUES (
			@login
			,@password
			,@firstname
			,@lastname
			,@companyid
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		SET @idnew = scope_identity()
		INSERT INTO UserRole(
			userId
			,roleId	
		)VALUES(
			@idnew
			,@roleid
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @idnew
		RETURN
	END ELSE BEGIN
		UPDATE [User]
		SET Login=@login
		,[Password]=@password
		,Firstname=@firstname
		,LastName=@lastname
		,CompanyId=@companyid
		WHERE id=@id
		IF @@ERROR<>0 GOTO ErrorHandler
		DELETE FROM UserRole
		WHERE userid=@id
		IF @@ERROR<>0 GOTO ErrorHandler
		INSERT INTO UserRole(
			userId
			,roleId	
		)VALUES(
			@id
			,@roleid
		)
		IF @@ERROR<>0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
		RETURN
	END	
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[LoadUserByName]    Script Date: 06/05/2007 11:17:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Load all user assosiated data for enabled user
*/
CREATE  PROCEDURE [dbo].[LoadUserByName]
	@login		varchar(256)	
AS
DECLARE @userid int
	SELECT @userid=u.id FROM [User] u 
	WHERE u.Login=@login
	AND u.UserStatusId=1
	IF @@ROWCOUNT <> 0 BEGIN
		SELECT u.*,c.* FROM [User] u 
		INNER JOIN Company c ON c.id=u.companyid
		WHERE u.id=@userid
		exec dbo.GetUserRoles @userid
	END
GO
/****** Object:  StoredProcedure [dbo].[LoadUserById]    Script Date: 06/05/2007 11:17:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[LoadUserById]
	@userid		int
AS
	SELECT u.*,c.* FROM [User] u 
	INNER JOIN Company c ON c.id=u.companyid
	WHERE u.id=@userid
	IF @@ROWCOUNT <> 0 BEGIN
		exec dbo.GetUserRoles @userid
	END
GO
/****** Object:  StoredProcedure [dbo].[GetCompareOperationList]    Script Date: 06/05/2007 11:15:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCompareOperationList]
AS
	SELECT * FROM CompareOperation
	ORDER BY [Id]
GO
/****** Object:  StoredProcedure [dbo].[GetAllowedCompOpList]    Script Date: 06/05/2007 11:14:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllowedCompOpList]
	@valuetypeid	int
AS
	SELECT co.* FROM CompareOperation co
	WHERE co.id NOT IN (SELECT CompareOpId FROM CompareFieldOpForbidden WHERE ValueTypeId=@valuetypeid)
	ORDER BY [Id]
GO
/****** Object:  StoredProcedure [dbo].[GetConditionTaskList]    Script Date: 06/05/2007 11:15:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionTaskList]
	@mortgageID  int
AS

	select 'C'+cast(c.ID as varchar) as SID,
			'C' as [Type],
			c.Title,
			cs.Name as Status
	from Condition c
	inner join ConditionStatus cs on cs.ID = c.StatusID
	where MortgageID = @mortgageID
	union
	select 'T'+cast(t.ID as varchar) as SID,
			'T' as [Type],
			t.Title,
			ts.Name as Status
	from Task t
	left join ConditionTask ct on ct.TaskID = t.ID
	inner join TaskStatus ts on ts.ID = t.StatusID
	where MortgageID = @mortgageID and ct.ID is null
GO
/****** Object:  StoredProcedure [dbo].[GetTaskStatusList]    Script Date: 06/05/2007 11:16:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskStatusList]
AS

	select * from TaskStatus
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetTaskList]    Script Date: 06/05/2007 11:16:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskList]
	@mortgageID  int,
	@status nvarchar(50)
AS

	select t.*, ts.Name as Status from Task t
	inner join TaskStatus ts on ts.ID = t.StatusID
	where (@status is null or ts.Name = @status)
		and t.MortgageID = @mortgageID
GO
/****** Object:  StoredProcedure [dbo].[GetAlert_]    Script Date: 06/05/2007 11:14:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAlert_]
	@id	int
AS
	select * from Alert
	where id=@id
GO
/****** Object:  StoredProcedure [dbo].[GetLenderCompanyList]    Script Date: 06/05/2007 11:15:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetLenderCompanyList]
	@all	bit = 1
AS
	IF @all=1
		SELECT * FROM Company
		WHERE StatusId!=3 AND ID != 1
		ORDER BY Company
	ELSE
		SELECT * FROM Company
		WHERE StatusId!=3 AND ID > 1
		ORDER BY Company
GO
/****** Object:  StoredProcedure [dbo].[LoadCompanyById]    Script Date: 06/05/2007 11:17:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadCompanyById]
	@id			int
AS
	SELECT * FROM Company WHERE Id=@id
GO
/****** Object:  StoredProcedure [dbo].[GetConditionCategoryList]    Script Date: 06/05/2007 11:15:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionCategoryList]
AS

	select * from ConditionCategory
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetFieldInGroupWithSelectedType]    Script Date: 06/05/2007 11:15:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[GetFieldInGroupWithSelectedType]
	@groupid 	int
AS
	SELECT MPField.[id], MPField.[description], MPFieldType.[Name] type
	FROM MortgageProfileField MPField
		INNER JOIN MortgageProfileFieldType  MPFieldType ON MPFieldType.[id] = MPField.ValueTypeID
	WHERE FieldGroupId=@groupid AND MPField.ValueTypeID <> 0
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageProfileFieldById]    Script Date: 06/05/2007 11:16:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageProfileFieldById]
	@id		int
AS
	SELECT mpf.*, mpft.Name
	FROM MortgageProfileField mpf
	INNER JOIN MortgageProfileFieldType mpft ON mpf.ValueTypeId = mpft.id
	WHERE mpf.id = @id
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageProfileFieldsByGroup]    Script Date: 06/05/2007 11:16:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageProfileFieldsByGroup]
	@id		int
AS
	SELECT mpf.*, mpft.Name
	FROM MortgageProfileField mpf
	INNER JOIN MortgageProfileFieldType mpft ON mpf.ValueTypeId = mpft.id
	WHERE mpf.FieldGroupId = @id
GO
/****** Object:  StoredProcedure [dbo].[GetConditionTypeList]    Script Date: 06/05/2007 11:15:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionTypeList]
AS

	select * from ConditionType
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetMailByID]    Script Date: 06/05/2007 11:15:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMailByID](
	@MailID			int 
)
AS

SELECT	* 
FROM	Mail
WHERE	ID = @MailID

SELECT	*
FROM	MailAttachment
WHERE	MailID = @MailID
GO
/****** Object:  StoredProcedure [dbo].[CopyAllRoleTemplateForCompany]    Script Date: 06/05/2007 11:14:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyAllRoleTemplateForCompany]
	@companyid			int
AS
DECLARE @roleid		int
DECLARE  cur CURSOR FAST_FORWARD FOR
	SELECT [Id] FROM RoleTemplate	
	WHERE [Id]>0

	OPEN cur
	FETCH NEXT FROM cur INTO @roleid
	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO RoleProfileStatus
		SELECT @roleid,ProfileInitStatusId,ProfileFinalStatusId,@companyid
		FROM RoleTemplateProfileStatus
		WHERE Roleid=@roleid
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO RoleField
		SELECT @roleid,FieldId,@companyid
		FROM RoleTemplateField
		WHERE Roleid=@roleid
		IF @@ERROR <> 0 GOTO ErrorHandler
		FETCH NEXT FROM cur INTO @roleid
	END
	CLOSE cur
	DEALLOCATE cur
	RETURN 0;
ErrorHandler:
	CLOSE cur
	DEALLOCATE cur
	RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[SaveFieldForRoleTemplate]    Script Date: 06/05/2007 11:17:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveFieldForRoleTemplate]
	@roleid			int
	,@statusid		int
	,@xml 			text
AS
DECLARE @idoc  int
	BEGIN TRANSACTION Tr
	IF DATALENGTH(@xml)=0 BEGIN
		IF @statusid>0 BEGIN
			DELETE FROM RoleTemplateField WHERE Roleid=@roleid AND ProfileStatusId=@statusid			
		END ELSE BEGIN
			DELETE FROM RoleTemplateField WHERE Roleid=@roleid
		END
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT 1
		RETURN
	END
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF @statusid>0 BEGIN
		DELETE FROM RoleTemplateField WHERE Roleid=@roleid AND ProfileStatusId=@statusid
		IF @@ERROR <> 0 GOTO ErrorHandler

		INSERT INTO RoleTemplateField(
			RoleId
			,ProfileStatusId
			,FieldId
		)
		SELECT 
			@roleid
			,@statusId
			,s.FieldId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			FieldId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
	END ELSE BEGIN
		DELETE FROM RoleTemplateField WHERE Roleid=@roleid
		INSERT INTO RoleTemplateField(
			RoleId
			,ProfileStatusId
			,FieldId
		)
		SELECT 
			@roleid
			,ps.Id
			,s.FieldId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			FieldId		int '@id'
		) s, ProfileStatus ps
		WHERE ps.Id>0
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetStatusName]    Script Date: 06/05/2007 11:16:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStatusName]
	@id	int
AS
	select [Name] from ProfileStatus 
	where [id]=@id
GO
/****** Object:  StoredProcedure [dbo].[GetAllowedRoleTemplateStatus]    Script Date: 06/05/2007 11:15:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllowedRoleTemplateStatus]
	@roleid 	int
	,@statusid  	int = -1
AS
	IF @statusid < 0
		SELECT * FROM ProfileStatus
		ORDER BY [Name]
	ELSE
		SELECT * FROM ProfileStatus
		WHERE id <> @statusid
		AND id NOT IN (SELECT ProfileFinalStatusId FROM RoleTemplateProfileStatus WHERE Roleid=@roleid)
		ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetAllowedRoleStatus]    Script Date: 06/05/2007 11:14:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllowedRoleStatus]
	@roleid 	int
	,@statusid  int = -1
AS
	IF @statusid < 0
		SELECT * FROM ProfileStatus
		ORDER BY [Name]
	ELSE
		SELECT * FROM ProfileStatus
		WHERE id <> @statusid
		AND id NOT IN (SELECT ProfileFinalStatusId FROM RoleProfileStatus WHERE Roleid=@roleid)
		ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageProfileById]    Script Date: 06/05/2007 11:16:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageProfileById]
	@id		int
AS
	SELECT mp.*, ps.[Name] as StatusName, p.[Name] as ProductName FROM MortgageProfile mp
	INNER JOIN ProfileStatus ps on ps.[id] = mp.CurProfileStatusID
	INNER JOIN Product p on p.[id] = mp.ProductID
	--INNER JOIN MortgageProfileBorrower mpb on mpb.MortgageProfileID  = mp.[id]
	--INNER JOIN Borrower b on b.[id] = mpb.BorrowerID 
	WHERE mp.[id]=@id
GO
/****** Object:  StoredProcedure [dbo].[SaveFieldForRole]    Script Date: 06/05/2007 11:17:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveFieldForRole]
	@roleid			int
	,@statusid		int
	,@companyid		int
	,@xml 			text
AS
DECLARE @idoc  int
	BEGIN TRANSACTION Tr
	IF DATALENGTH(@xml)=0 BEGIN
		IF @statusid>0 BEGIN
			DELETE FROM RoleField WHERE Roleid=@roleid AND ProfileStatusId=@statusid AND CompanyId=@companyid
		END ELSE BEGIN
			DELETE FROM RoleField WHERE Roleid=@roleid AND CompanyId=@companyid
		END
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT 1
		RETURN
	END
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF @statusid>0 BEGIN
		DELETE FROM RoleField WHERE Roleid=@roleid AND ProfileStatusId=@statusid AND CompanyId=@companyid
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO RoleField(
			RoleId
			,CompanyId
			,ProfileStatusId
			,FieldId
		)
		SELECT 
			@roleid
			,@companyId
			,@statusid
			,s.FieldId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			FieldId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
	END ELSE BEGIN
		DELETE FROM RoleField WHERE Roleid=@roleid AND CompanyId=@companyid
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO RoleField(
			RoleId
			,CompanyId
			,ProfileStatusId
			,FieldId
		)
		SELECT 
			@roleid
			,@companyId
			,ps.Id
			,s.FieldId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			FieldId		int '@id'
		) s, ProfileStatus ps
		WHERE ps.Id>0
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetRoleTemplateStatusList]    Script Date: 06/05/2007 11:16:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleTemplateStatusList]
	@roleid  int
AS
	SELECT rt.*,p1.[Name] as ProfileInitStatus,p2.[Name] as ProfileFinalStatus FROM RoleTemplateProfileStatus rt
	INNER JOIN ProfileStatus p1 on p1.id=rt.ProfileInitStatusId
	INNER JOIN ProfileStatus p2 on p2.id=rt.ProfileFinalStatusId
	WHERE roleid=@roleid
GO
/****** Object:  StoredProcedure [dbo].[GetMpStatusList]    Script Date: 06/05/2007 11:16:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMpStatusList]
	@statusid  int = -1
AS
	IF @statusid < 0
		SELECT * FROM ProfileStatus
		ORDER BY [Name]
	ELSE
		SELECT * FROM ProfileStatus
		WHERE id <> @statusid
		ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetRoleStatusList]    Script Date: 06/05/2007 11:16:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleStatusList]
	@roleid  int
AS
	SELECT rt.*,p1.[Name] as ProfileInitStatus,p2.[Name] as ProfileFinalStatus FROM RoleProfileStatus rt
	INNER JOIN ProfileStatus p1 on p1.id=rt.ProfileInitStatusId
	INNER JOIN ProfileStatus p2 on p2.id=rt.ProfileFinalStatusId
	WHERE roleid=@roleid
GO
/****** Object:  StoredProcedure [dbo].[GetProfileStatusList]    Script Date: 06/05/2007 11:16:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProfileStatusList]
AS
	SELECT * FROM ProfileStatus WHERE Id>0
GO
/****** Object:  StoredProcedure [dbo].[GetTemplateStatusList]    Script Date: 06/05/2007 11:16:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTemplateStatusList]
	@roleid  int
AS
	SELECT rt.*,p1.[Name] as ProfileInitStatus,p2.[Name] as ProfileFinalStatus FROM RoleTemplateProfileStatus rt
	INNER JOIN ProfileStatus p1 on p1.id=rt.ProfileInitStatusId
	INNER JOIN ProfileStatus p2 on p2.id=rt.ProfileFinalStatusId
	WHERE roleid=@roleid
GO
/****** Object:  StoredProcedure [dbo].[EditMortgageProfileBorrower_]    Script Date: 06/05/2007 11:14:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditMortgageProfileBorrower_](
	@MortgageProfileID		INT,
	@BorrowerID			INT
)
AS

DELETE FROM MortgageProfileBorrower WHERE MortgageProfileID = @MortgageProfileID AND BorrowerID=@BorrowerID

INSERT INTO MortgageProfileBorrower
  (MortgageProfileID, BorrowerID) 
 VALUES
  (@MortgageProfileID, @BorrowerID)
GO
/****** Object:  StoredProcedure [dbo].[GetTaskInfoSourceList]    Script Date: 06/05/2007 11:16:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskInfoSourceList]
AS

	select * from TaskInfoSource
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetDocTemplateById]    Script Date: 06/05/2007 11:15:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[GetDocTemplateById](
	@id		INT
)
AS
	SELECT * FROM DocTemplate WHERE  ID=@id

/*
	SELECT d.ID, d.Title, d.Archived, v.ID as VersionID, v.Version, v.FileName, v.UploadDate, v.IsCurrent
	FROM DocTemplate  d
	INNER JOIN DocTemplateVersion v ON d.ID = v.DocTemplateID
	WHERE  d.ID=@id AND v.IsCurrent = 1
*/
GO
/****** Object:  StoredProcedure [dbo].[EditDocTemplate]    Script Date: 06/05/2007 11:14:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditDocTemplate](
 @id   INT,
 @title   NVARCHAR(100),
 @archived  BIT
)
AS

DECLARE @Result  INT

IF(@id <= 0)
 BEGIN
 INSERT INTO DocTemplate
  (Title, Archived) 
 VALUES
  (@title, @archived) 
 SET @Result = @@IDENTITY
 END
ELSE
 BEGIN
  UPDATE  DocTemplate SET
  Title = @title,
  Archived = @archived
  WHERE  ID = @id
  SET @Result = @@rowcount
 END

SELECT @Result
GO
/****** Object:  StoredProcedure [dbo].[SaveRule_]    Script Date: 06/05/2007 11:17:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRule_]
	@id				int
	,@name			varchar(50)
	,@companyid		int
	,@xml			ntext
	,@categoryid	int
	,@categoryname	varchar(100)
	,@startdate		datetime = null
	,@enddate		datetime = null
AS
DECLARE @cnt int
DECLARE @newid int
DECLARE @idoc  int
DECLARE @catid int

	BEGIN TRANSACTION tr
	SELECT @cnt = COUNT(*) FROM [Rule]
	WHERE @id<>id AND @name=[Name] AND CompanyId=@companyid
		IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- already exists
		RETURN
	END 	
	IF @categoryid < 0 BEGIN
		SELECT @catid=ISNULL([Id],-1) FROM RuleCategory WHERE [Name]=@categoryname
		IF @catid = -1 BEGIN
			INSERT INTO RuleCategory (
				[Name]
			) VALUES (
				@categoryname
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @catid=scope_identity()
		END
	END ELSE BEGIN
		SET @catid=@categoryid
	END
	IF @id > 0 BEGIN
		UPDATE [Rule]
		SET [Name]=@name
		,StartDate=@startdate
		,EndDate=@enddate
		,categoryId=@catid
		WHERE id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid=@id
	END ELSE BEGIN
		INSERT INTO [Rule](
			[Name]
			,StartDate
			,EndDate
			,CompanyId
			,CategoryId
		) VALUES (
			@name
			,@startdate
			,@enddate
			,@companyId
			,@catid
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid=scope_identity()
	END
	IF DATALENGTH(@xml) > 0 BEGIN
		DELETE FROM RuleProduct WHERE RuleId=@newid
		IF @@ERROR <> 0 GOTO ErrorHandler	
		EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
		IF @@ERROR <> 0 GOTO ErrorHandler	
		INSERT INTO RuleProduct(
			RuleId
			,ProductId
		)
		SELECT 
			@newid
			,s.ProductId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			ProductId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
	END
	COMMIT TRANSACTION Tr
	SELECT @newid
	RETURN 

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetTaskRecurrenceList]    Script Date: 06/05/2007 11:16:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTaskRecurrenceList]
AS

	select * from TaskRecurrence
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetCondition]    Script Date: 06/05/2007 11:15:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCondition]
	@id	int
AS
	select c.*,
	(select top 1 TaskID from ConditionTask where ConditionID=c.ID) as TaskID
	from Condition c
	where c.id=@id
GO
/****** Object:  StoredProcedure [dbo].[GetTask]    Script Date: 06/05/2007 11:16:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTask]
	@id	int
AS
	select t.*,
--		case when tt.ParentID is null then tt.Name else pt.Name + ': ' + tt.Name end as TaskType,
		(select top 1 ConditionID from ConditionTask where TaskID=t.ID) as ConditionID
	from Task t
--	inner join TaskType tt on tt.ID = t.TypeID
--	left outer join TaskType pt on pt.ID = tt.ParentID
	where t.id=@id
GO
/****** Object:  StoredProcedure [dbo].[SaveTask]    Script Date: 06/05/2007 11:18:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveTask]
	@id	int,
	@MortgageID int,
	@StatusID	int,
	@Title nvarchar(30),
	@AssignedTo int,
	@Description nvarchar(max),
	@ScheduleDate datetime,
	@RecurrenceID int,
	@InfoSourceID int,
	@DifficultyID int,
	@EstimatedAttempts varchar(50),
	@ConditionID int,
	@CreatedBy int
AS
DECLARE @newid int
	BEGIN TRANSACTION tr
	if exists(select id from Task where id=@id)
	 begin
		update Task set
			CompleteDate = getdate()
		where StatusID<>@StatusID and @StatusID=2
		update Task set
			MortgageID = @MortgageID,
			StatusID = @StatusID,
			Title = @Title,
			AssignedTo = @AssignedTo,
			Description = @Description,
			ScheduleDate = @ScheduleDate,
			RecurrenceID = @RecurrenceID,
			InfoSourceID = @InfoSourceID,
			DifficultyID = @DifficultyID,
			EstimatedAttempts = @EstimatedAttempts
		where id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		if @ConditionID is not null
			if exists (select id from ConditionTask where TaskId = @id)
				update ConditionTask set ConditionID=@ConditionID where TaskID=@id
			else
				insert into ConditionTask(ConditionID, TaskID)values(@ConditionID, @id)
		COMMIT TRANSACTION Tr
		SELECT @id
	 end
	else
	 begin
		insert into Task(
			MortgageID,
			StatusID,
			Title,
			AssignedTo,
			Description,
			ScheduleDate,
			RecurrenceID,
			InfoSourceID,
			DifficultyID,
			EstimatedAttempts,
			CreatedBy
		)values(
			@MortgageID,
			@StatusID,
			@Title,
			@AssignedTo,
			@Description,
			@ScheduleDate,
			@RecurrenceID,
			@InfoSourceID,
			@DifficultyID,
			@EstimatedAttempts,
			@CreatedBy
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		if @ConditionID is not null
			insert into ConditionTask(ConditionID, TaskID)values(@ConditionID, @newid)

		COMMIT TRANSACTION Tr
		SELECT @newid
	 end

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetPropertyById]    Script Date: 06/05/2007 11:16:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPropertyById]
	@id	int
AS
	SELECT mp.* ,s.Name AS StateName,t.Name AS SPTitleHeld
	FROM MortgageProperty mp
	INNER JOIN [State] s on s.id=mp.stateId
	INNER JOIN TitleHeld t on t.Id=mp.SpTitleHeldId
	WHERE mp.Id=@id
GO
/****** Object:  StoredProcedure [dbo].[DeleteRoleTemplateStatus]    Script Date: 06/05/2007 11:14:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteRoleTemplateStatus]
	@id  int
AS
	DELETE FROM RoleTemplateProfileStatus
	WHERE id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[AddRoleTemplateStatus]    Script Date: 06/05/2007 11:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddRoleTemplateStatus]
	@roleid	int
	,@initstatusid	int
	,@finalstatusid int
AS
	INSERT INTO RoleTemplateProfileStatus(
		roleId
		,ProfileInitStatusId
		,ProfileFinalStatusId
	) VALUES (
		@roleid
		,@initstatusid
		,@finalstatusid		
	)
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetEventTypeList]    Script Date: 06/05/2007 11:15:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEventTypeList]
AS
	SELECT [id],[name] FROM EventType ORDER BY DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[DeleteHoliday]    Script Date: 06/05/2007 11:14:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteHoliday]
	@id			int
AS
	DELETE FROM Holiday WHERE Id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetHolidayByYearAndCompany]    Script Date: 06/05/2007 11:15:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetHolidayByYearAndCompany]
	@year		int
	,@companyid	int
AS
DECLARE @cnt int
	IF @companyid = 1 BEGIN
		SELECT * FROM Holiday
		WHERE CompanyId=@companyId AND YEAR(HolidayDate)=@year
		ORDER BY HolidayDate
	END ELSE BEGIN
		SELECT @cnt = COUNT(*) 
		FROM Holiday 
		WHERE CompanyId=@companyId AND YEAR(HolidayDate)=@year
		IF @cnt = 0 BEGIN
			INSERT INTO Holiday
			SELECT HolidayDate,[Description],@companyid
			FROM Holiday
			WHERE CompanyId=1 AND YEAR(HolidayDate)=@year
		END
		SELECT * FROM Holiday
		WHERE CompanyId=@companyId AND YEAR(HolidayDate)=@year
		ORDER BY HolidayDate		
	END
GO
/****** Object:  StoredProcedure [dbo].[GetStateName]    Script Date: 06/05/2007 11:16:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStateName]
	@id	int
AS
	select [Name] from [State] 
	where [id]=@id
GO
/****** Object:  StoredProcedure [dbo].[GetStateCheckboxList]    Script Date: 06/05/2007 11:16:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStateCheckboxList]
AS
	SELECT * FROM State where id>0 ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetStateList]    Script Date: 06/05/2007 11:16:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStateList]
AS
	SELECT * FROM State ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[CalculateFieldGroupIndex]    Script Date: 06/05/2007 11:14:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalculateFieldGroupIndex]
AS

SELECT	fg.[id], fgInd.[ID] FGIndID, fgInd.[Count], [Name] = 
													CASE	WHEN fgInd.[Name] IS NULL 
																THEN fg.[Name] 
															ELSE 
																fgInd.[Name]
													END
INTO	#FGIndex 
FROM	FieldGroup fg
		LEFT JOIN FieldGroupIndex fgInd ON fgInd.FieldGroupID = fg.[id] 

DECLARE	@FGID		INT
DECLARE	@FGIndID	INT
DECLARE	@Count		INT
DECLARE	@Name		varchar(50)
DECLARE	@Index		INT

WHILE (SELECT COUNT(*) FROM #FGIndex) > 0
BEGIN
	SELECT	TOP 1 @FGID = fgIndShort.[id], @FGIndID = fgIndShort.FGIndID, @Count = fgIndShort.[Count], @Name = fgIndShort.[Name] 
	FROM	#FGIndex fgIndShort

	IF @Count IS NULL OR @Count < 0
		INSERT INTO #FGIndexDetailed
			( [id], FGIndID, [Index], [Name] )
			VALUES
			( @FGID, @FGIndID, @Count, @Name )
	ELSE
	BEGIN
		SET @Index = 0
		WHILE @Index < @Count
		BEGIN
			INSERT INTO #FGIndexDetailed
				( [id], FGIndID, [Index], [Name] )
				VALUES
				( @FGID, @FGIndID, @Index, @Name + CAST(@Index AS varchar(50)) )

			SET @Index = @Index + 1
		END
	END

	IF @FGIndID IS NULL
		DELETE	FROM #FGIndex
		WHERE	[id] = @FGID
	ELSE
		DELETE	FROM #FGIndex
		WHERE	FGIndID = @FGIndID
END

UPDATE	#FGIndexDetailed
SET		[Index] = ISNULL([Index], 0), 
		FGIndID = ISNULL(FGIndID, 0), 
		ComplexID = CASE	WHEN [Index] IS NULL
								THEN CAST([id] AS varchar(50)) + ',0'
							ELSE
								CAST([id] AS varchar(50)) + ',' + CAST([Index] AS varchar(50))
					END

DROP TABLE #FGIndex
GO
/****** Object:  StoredProcedure [dbo].[EditMortgageProfileUser]    Script Date: 06/05/2007 11:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EditMortgageProfileUser](
	@MortgageProfileID		INT,
	@UserRoleID			INT,
	@ProfileStatusID			INT
)
AS

INSERT INTO MortgageProfileUser
  (MortgageProfileID, UserRoleID, ProfileStatusID) 
 VALUES
  (@MortgageProfileID, @UserRoleID, @ProfileStatusID)
GO
/****** Object:  StoredProcedure [dbo].[GetRuleCategory]    Script Date: 06/05/2007 11:16:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleCategory]
AS
	SELECT * FROM RuleCategory
	ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetHUDFactors]    Script Date: 06/05/2007 11:15:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetHUDFactors]
AS

SELECT	*
--FROM	MortgageProfile
FROM	HUDFactors
WHERE	Age > 0
GO
/****** Object:  StoredProcedure [dbo].[GetFieldGroup]    Script Date: 06/05/2007 11:15:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFieldGroup]
	@all   bit = 0
AS
	IF @all = 1
		SELECT * FROM FieldGroup
		ORDER BY id
	ELSE
		SELECT * FROM FieldGroup
		WHERE id>0
		ORDER BY id
GO
/****** Object:  StoredProcedure [dbo].[GetUserRolesList]    Script Date: 06/05/2007 11:16:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserRolesList]
	@userid		int
	,@roleid	int
AS
	SELECT r.id,r.[Name],
	CASE WHEN ur.roleid IS NULL THEN 0
		ELSE 1 END Selected
	FROM [RoleTemplate] r
	LEFT OUTER JOIN UserRole ur ON ur.roleid=r.id AND ur.userId=@userid
	WHERE r.Id>1 AND r.Id<>@roleid
GO
/****** Object:  StoredProcedure [dbo].[GetRolesForUser]    Script Date: 06/05/2007 11:16:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetRolesForUser]
	@userid		int
AS
/*	
	SELECT r.id,r.[Name],
	CASE WHEN ur.roleid IS NULL THEN 0
		ELSE 1 END Selected
	FROM [Role] r
	LEFT OUTER JOIN UserRole ur ON ur.roleid=r.id AND ur.userId=@userid
	WHERE r.CompanyId=@companyid
*/
	SELECT r.id,r.[Name],
	CASE WHEN ur.roleid IS NULL THEN 0
		ELSE 1 END Selected
	FROM [RoleTemplate] r
	LEFT OUTER JOIN UserRole ur ON ur.roleid=r.id AND ur.userId=@userid
	WHERE r.Id>0
GO
/****** Object:  UserDefinedFunction [dbo].[udfGetFieldReadOnly]    Script Date: 06/05/2007 11:37:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[udfGetFieldReadOnly]
(
	@userid				int
	,@mprpofilestatusid	int
	,@fieldid			int
)
RETURNS bit
AS
BEGIN
	IF EXISTS (SELECT 1	FROM roleField rf INNER JOIN userRole ur ON ur.roleid=rf.roleid and ur.userid=@userid)
		RETURN 0
	RETURN 1
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteObjectFromRule]    Script Date: 06/05/2007 11:14:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteObjectFromRule]
	@id			int
AS
	DELETE FROM RuleObject WHERE id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetRuleConditionById]    Script Date: 06/05/2007 11:16:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetRuleConditionById]
	@id		int
AS
	SELECT rc.* FROM RuleCondition rc
	INNER JOIN RuleObject ro ON ro.ObjectId=rc.[Id]
	WHERE ro.[id]=@id
GO
/****** Object:  UserDefinedFunction [dbo].[udfGetRuleObject]    Script Date: 06/05/2007 11:37:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[udfGetRuleObject]
(
	@ruleid		int
	,@objecttypeid	int
)
RETURNS int
AS
BEGIN
DECLARE @cnt int
	IF @objecttypeid=0 BEGIN
		SELECT @cnt = COUNT(*) FROM RuleUnit ru
		INNER JOIN [Rule] r ON r.Id=ru.ruleid 
		WHERE r.id=@ruleid
	END ELSE BEGIN
		SELECT @cnt = COUNT(*) FROM RuleObject ro
		INNER JOIN RuleObjectType rot ON rot.Id=ro.ObjectTypeid
		WHERE ro.ruleid=@ruleid AND rot.Id=@objecttypeid
	END
	RETURN @cnt
END
GO
/****** Object:  StoredProcedure [dbo].[GetInvisibleField]    Script Date: 06/05/2007 11:15:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInvisibleField]
	@lenderid	int
AS
	SELECT DISTINCT r.id AS RuleId,mf.Id AS FieldId
	FROM [Rule] r 
	INNER JOIN RuleObject ro ON ro.ruleid=r.id 
	INNER JOIN MortgageProfileField mf on mf.id=ro.objectid
	WHERE ro.objecttypeid=1 AND r.companyid=1
	ORDER BY R.Id
GO
/****** Object:  StoredProcedure [dbo].[GetRuleAllowedObjectList]    Script Date: 06/05/2007 11:16:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleAllowedObjectList]
	@ruleid			int
	,@objecttypeid	int
	,@objectgroupid	int
AS
	IF @objecttypeid = 1 BEGIN
		SELECT MPField.Id,MPField.[description] 
		FROM MortgageProfileField MPField
		WHERE FieldGroupId=@objectgroupid AND MPField.Id NOT IN (SELECT objectid FROM RuleObject WHERE ruleid=@ruleid AND objecttypeid=@objecttypeid)
	END
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleCondition]    Script Date: 06/05/2007 11:14:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleCondition]
	@srcruleid	int
	,@dstruleid	int
AS
DECLARE @Title varchar(100)
DECLARE @Detail varchar(256)
DECLARE @Roleid int
DECLARE @oldid int
DECLARE @newid int
DECLARE @res	int
	SET @res = 0
	DECLARE cur CURSOR FAST_FORWARD FOR
	SELECT rc.Id,rc.Title,rc.detail,rc.roleid 
	FROM rulecondition rc
	INNER JOIN ruleobject ro ON ro.objectid=rc.id and ro.objecttypeid=2
	WHERE ro.ruleid=@dstruleid

	OPEN cur

	FETCH NEXT FROM cur
	INTO @oldid,@Title,@Detail,@Roleid

	WHILE @@FETCH_STATUS = 0 BEGIN
		INSERT INTO rulecondition (
			Title
			,Detail
			,Roleid
		) VALUES (
			@Title
			,@Detail
			,@RoleId
		)
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		SET @newid=scope_identity()
		UPDATE ruleobject SET objectid=@newid
		WHERE RuleId=@dstruleid AND ObjectTypeId=2 AND objectid=@oldid
		SET @res=@@ERROR
		IF @res <> 0 GOTO StopCursor

		FETCH NEXT FROM cur
		INTO @oldid,@Title,@Detail,@Roleid		
	END

StopCursor:
	CLOSE cur
	DEALLOCATE cur
	RETURN @res
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleObject]    Script Date: 06/05/2007 11:18:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleObject]
	@ruleid		int
	,@objectid	int
	,@objecttypeid	int
	,@actionid	int
AS
DECLARE @cnt int
	INSERT INTO RuleObject(
		Ruleid
		,ObjectId
		,ObjectTypeid
		,RuleActionId
	) VALUES (
		@ruleid
		,@objectid
		,@objecttypeid
		,@actionid
	)
	SET @cnt = @@ROWCOUNT
	SELECT @cnt
	RETURN @cnt
GO
/****** Object:  StoredProcedure [dbo].[SaveMPEvents]    Script Date: 06/05/2007 11:17:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPEvents]
	@mpid		int
	,@xml		ntext
AS
DECLARE @idoc  int
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 
		RETURN
	BEGIN TRANSACTION Tr
	DELETE FROM MortgageProfileEvent
	WHERE MortgageProfileId=@mpid 
	IF @@ERROR <> 0 GOTO ErrorHandler
	IF DATALENGTH(@xml) > 0 BEGIN
		INSERT INTO MortgageProfileEvent (
			MortgageProfileId
			,RuleEventId
		) SELECT
			@mpid
			,ro.ObjectId
			FROM OPENXML (@idoc,'Root/item',1)
			WITH (
				ItemId		int	'@id'	
			) s
		INNER JOIN RuleObject ro ON ro.ruleid=s.ItemId AND ro.ObjectTypeId=7
		IF @@ERROR <> 0 GOTO ErrorHandler
	END
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[UpdateMPConditionRules]    Script Date: 06/05/2007 11:18:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateMPConditionRules]
	@MortgageProfileId	int,
	@xml				ntext
AS
DECLARE @ids  int


DELETE FROM Condition WHERE RuleConditionId>0 AND MortgageID=@MortgageProfileId

 IF Datalength(@xml)>0
	BEGIN
	EXEC sp_xml_preparedocument @ids OUTPUT, @xml
		insert into Condition (MortgageID, RuleConditionId, Title, Details, SignOffLevelID)
		SELECT @MortgageProfileId, ro.ObjectId, rc.Title, rc.Detail, rc.RoleId
		FROM RuleObject ro
		INNER JOIN RuleCondition rc ON rc.Id=ro.ObjectId
		WHERE ro.objecttypeid=2 AND
		ro.ruleid IN (SELECT 
				id
			FROM OPENXML (@ids,'Root/item',1)
			WITH (
				Id     int '@id'
			) s)
		AND ro.ObjectId NOT IN (SELECT c.RuleConditionId
		from Condition c
		INNER JOIN RuleCondition rc on rc.id=c.RuleConditionId
		WHERE c.MortgageID = @MortgageProfileId)
	EXEC sp_xml_removedocument @ids
	END
GO
/****** Object:  View [dbo].[vwActiveRule]    Script Date: 06/05/2007 11:37:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwActiveRule]
AS
SELECT * FROM [Rule] 
WHERE StatusId=1 AND 
((StartDate IS NULL) AND (EndDate IS NULL)) OR
(StartDate <= GetDate()) AND (EndDate >= GetDate())
GO
/****** Object:  StoredProcedure [dbo].[SetRuleStatus]    Script Date: 06/05/2007 11:18:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetRuleStatus]
	@id			int
	,@statusid	int
AS
	UPDATE [Rule] SET StatusId=@statusid
	WHERE Id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  StoredProcedure [dbo].[GetRuleFieldActionList]    Script Date: 06/05/2007 11:16:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleFieldActionList]
AS
	SELECT * FROM RuleAction 
	WHERE ID > 0 AND ID < 3
	ORDER BY Id
GO
/****** Object:  StoredProcedure [dbo].[GetConditionSignOffLevelList]    Script Date: 06/05/2007 11:15:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetConditionSignOffLevelList]
AS

	select * from ConditionSignOffLevel
	order by DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[SavePayoff]    Script Date: 06/05/2007 11:17:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePayoff]
	@id	int,
	@MortgageID int,
	@Creditor	varchar(50),
	@Ordered bit,
	@IN bit,
	@Amount money,
	@Perdiem varchar(50),
	@ExpDate datetime,
	@CreatedBy int
AS
DECLARE @newid int
	BEGIN TRANSACTION tr
	if exists(select id from Payoff where id=@id)
	 begin
		update Payoff set
			MortgageID = @MortgageID,
			Creditor = @Creditor,
			Ordered = @Ordered,
			[IN] = @IN,
			Amount = @Amount,
			Perdiem = @Perdiem,
			ExpDate = @ExpDate
		where id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
	 end
	else
	 begin
		insert into Payoff(
			MortgageID,
			Creditor,
			Ordered,
			[IN],
			Amount,
			Perdiem,
			ExpDate,
			CreatedBy
		)values(
			@MortgageID,
			@Creditor,
			@Ordered,
			@IN,
			@Amount,
			@Perdiem,
			@ExpDate,
			@CreatedBy
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		COMMIT TRANSACTION Tr
		SELECT @newid
	 end
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[EditDocTemplateField]    Script Date: 06/05/2007 11:14:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[EditDocTemplateField](
	@ID			int, 
	@DocTemplateVerID	int, 
	@MPFieldID		int, 
	@FieldType		int, 
	@PDFFieldName		nvarchar(100), 
	@GroupIndex		int 
)
AS

DECLARE @ResultID 	INT

IF @ID <=  0 
	BEGIN
		INSERT INTO DocTemplateField
		(
		DocTemplateVerID, 
		FieldID, 
		PDFFieldName, 
		[Type], 
		DataFormatID, 
		GroupIndex
		)
	
		VALUES	
		(
		@DocTemplateVerID, 
		@MPFieldID, 
		@PDFFieldName, 
		@FieldType, 
		NULL, 
		@GroupIndex
		)
	
		SET @ResultID = SCOPE_IDENTITY()
	END
ELSE
	BEGIN
 	  	UPDATE DocTemplateField 
 	  	SET
   		DocTemplateVerID = @DocTemplateVerID, 
		FieldID = @MPFieldID, 
		PDFFieldName = @PDFFieldName, 
		[Type] = @FieldType, 
		DataFormatID = NULL, 
		GroupIndex = @GroupIndex 
 	  	WHERE ID = @ID
 	
 	  	SET @ResultID =  @@rowcount
 	END

SELECT @ResultID
GO
/****** Object:  StoredProcedure [dbo].[GetRoleTemplateStructure]    Script Date: 06/05/2007 11:16:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRoleTemplateStructure]
AS
	SELECT Id,[Name],Abbriviation,ISNULL(ParentRoleId,0) AS ParentId FROM Roletemplate WHERE displayorder>0 AND Id<>6 -- Operation manager
	ORDER BY ParentRoleId
GO
/****** Object:  StoredProcedure [dbo].[GetInboxMailsList]    Script Date: 06/05/2007 11:15:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetInboxMailsList](
	@UserID			int 
)
AS

SELECT	ml.ID, ml.[From], ml.Subject, ml.Date, ml.MailStatusID 
FROM	Mail ml 
WHERE	ml.MailStatusID = 1 OR ml.MailStatusID = 2
GO
/****** Object:  StoredProcedure [dbo].[SaveMPCheckList]    Script Date: 06/05/2007 11:17:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPCheckList]
	@mpid		int
	,@mpstatusid int
	,@xml		ntext
AS
DECLARE @idoc  int
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 
		RETURN
	BEGIN TRANSACTION Tr
	DELETE FROM MortgageProfileCheckList 
	WHERE MortgageProfileId=@mpid AND StatusId = @mpstatusid
	IF @@ERROR <> 0 GOTO ErrorHandler
	INSERT INTO MortgageProfileCheckList (
		MortgageProfileId
		,StatusId
		,CheckListItemId
		,cbYes
		,cbNo
		,cbDontknow
		,cbTofollow
	) SELECT
		@mpid
		,@mpstatusid
		,s.ItemId
		,s.cbYes
		,s.cbNo
		,s.cbDontknow
		,s.cbTofollow
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			ItemId		int	'@id'	
			,cbYes		bit	'@yes'
			,cbNo		bit  '@no'
			,cbDontKnow	bit  '@donotknow'
			,cbToFollow	bit  '@tofollow'
		) s
	IF @@ERROR <> 0 GOTO ErrorHandler
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[SaveProduct]    Script Date: 06/05/2007 11:17:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveProduct]
	@id	int
	,@name varchar(100)
AS
DECLARE @cnt int
DECLARE @newid int
	BEGIN TRANSACTION Tr	
		SELECT @cnt = COUNT(*) FROM Product
		WHERE @id<>id AND @name=[Name]
		IF @cnt > 0 BEGIN
			ROLLBACK TRANSACTION Tr
			SELECT -1   -- already exists
			RETURN
		END
		IF @id > 0 BEGIN
			UPDATE Product SET [Name]=@name WHERE ID=@id
			IF @@ERROR <> 0 GOTO ErrorHandler
			COMMIT TRANSACTION Tr
			SELECT @id
			RETURN
		END ELSE BEGIN
			INSERT INTO Product(
				[Name]
			) VALUES (
				@name
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @newid = scope_identity()
			COMMIT TRANSACTION Tr
			SELECT @newid
			RETURN
		END
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 06/05/2007 11:14:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteProduct]
	@id		int	
AS
	UPDATE Product SET StatusId=3 WHERE Id=@id
	SELECT @@ROWCOUNT
GO
/****** Object:  View [dbo].[vwActiveProduct]    Script Date: 06/05/2007 11:37:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  VIEW [dbo].[vwActiveProduct]
AS
SELECT * FROM Product WHERE Statusid=1
GO
/****** Object:  StoredProcedure [dbo].[SaveTaskNote]    Script Date: 06/05/2007 11:18:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveTaskNote]
	@id	int,
	@TaskID int,
	@MortgageID int,
	@Note nvarchar(max),
	@CreatedBy int
AS
DECLARE @newid int
	BEGIN TRANSACTION tr
	if exists(select id from TaskNote where id=@id)
	 begin
		update TaskNote set
			TaskID = @TaskID,
			MortgageID = @MortgageID,
			Note = @Note
		where id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
	 end
	else
	 begin
		insert into TaskNote(
			TaskID,
			MortgageID,
			Note,
			CreatedBy
		)values(
			@TaskID,
			@MortgageID,
			@Note,
			@CreatedBy
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		COMMIT TRANSACTION Tr
		SELECT @newid
	 end

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetDocTemplateVersionByIds]    Script Date: 06/05/2007 11:15:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[GetDocTemplateVersionByIds](
	@dtIDsXml	ntext
)
AS

DECLARE @idXmlDoc  int
EXEC sp_xml_preparedocument @idXmlDoc OUTPUT, @dtIDsXml
IF @@ERROR <> 0 
	RETURN

SELECT	DISTINCT dtVersion.*
FROM	(
		SELECT	*
		FROM	OPENXML (@idXmlDoc, 'Root/item', 1)
				WITH	(
						ID	int '@id' 
						)
		) AS docTemplate
		INNER JOIN DocTemplateVersion dtVersion ON  dtVersion.DocTemplateID = docTemplate.ID AND dtVersion.IsCurrent = 1

EXEC dbo.GetFieldsByDocTemplateVerIDs @idXmlDoc

EXEC sp_xml_removedocument @idXmlDoc
GO
/****** Object:  StoredProcedure [dbo].[GetProductList]    Script Date: 06/05/2007 11:16:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetProductList]
	@all  bit 
AS
	IF @all = 1
		SELECT * FROM vwActiveProduct ORDER BY [Name]
	ELSE
		SELECT * FROM vwActiveProduct WHERE ID > 0 ORDER BY [Name]
GO
/****** Object:  StoredProcedure [dbo].[GetFieldGroupIndex]    Script Date: 06/05/2007 11:15:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFieldGroupIndex]
AS

CREATE	TABLE #FGIndexDetailed ([id] int NOT NULL, ComplexID varchar(50), FGIndID int, [Index] int, [Name] varchar(50) NOT NULL) 
EXEC CalculateFieldGroupIndex

SELECT	fgIndDet.[Name], fgIndDet.ComplexID
FROM	#FGIndexDetailed fgIndDet
ORDER BY fgIndDet.[id], fgIndDet.FGIndID, fgIndDet.[Name]

DROP TABLE #FGIndexDetailed
GO
/****** Object:  StoredProcedure [dbo].[GetFieldsByDocTemplateVerID]    Script Date: 06/05/2007 11:15:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[GetFieldsByDocTemplateVerID](
	@DocTemplateVerID		INT 
)
AS

CREATE	TABLE #FGIndexDetailed ([id] int NOT NULL, ComplexID varchar(50), FGIndID int, [Index] int, [Name] varchar(50) NOT NULL) 
EXEC CalculateFieldGroupIndex

SELECT	docTemplField.[ID], docTemplField.DocTemplateVerID, docTemplField.FieldID, docTemplField.PDFFieldName, 
	docTemplField.Type as TypeID, docTemplField.DataFormatID, mpField.[Description] as MPFiledName, 
	docTemplField.GroupIndex, 
	fgIndDet.[Name] GroupName, fgIndDet.FGIndID, fgIndDet.[id] FGID, 
	[Type] = 
	CASE
		WHEN docTemplField.Type = 1 THEN 'Boolean' 
		ELSE 'String' 
	END 
FROM	DocTemplateField docTemplField
	INNER JOIN MortgageProfileField mpField ON mpField.[id] = docTemplField.FieldID
	INNER JOIN #FGIndexDetailed fgIndDet ON fgIndDet.[id] = mpField.FieldGroupId AND fgIndDet.[Index] = docTemplField.GroupIndex
WHERE	docTemplField.DocTemplateVerID = @DocTemplateVerID

DROP TABLE #FGIndexDetailed
GO
/****** Object:  StoredProcedure [dbo].[GetMessageBoard]    Script Date: 06/05/2007 11:15:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMessageBoard]
	@MortgageID int,
	@ReportedDate datetime,
	@UserID int,
	@NeedTasks bit = 1,
	@NeedAlerts bit = 1,
	@NeedEvents bit = 1,
	@NeedNotes bit = 1,
	@TaskID int = null
AS
BEGIN
	declare @MessageBoard table (ID varchar(15), Title varchar(50), MessageType varchar(20), MortgageID int, Created datetime, IsHighLight bit)
	if(@NeedTasks=1)
		insert into @MessageBoard
		select distinct 'T&'+ cast(t.ID as varchar(13)) as ID,
				t.Title,
				'Task' as MessageType,
				t.MortgageID,
				DateAdd(day,datediff(day, ScheduleDate, @ReportedDate),ScheduleDate) as Created,
				0 as IsHighLight
		from Task t
		inner join MortgageProfile mp on mp.ID = t.MortgageID
		inner join MortgageProfileUser mpu on mpu.MortgageProfileID = mp.ID
		where 
			(t.MortgageID = @MortgageID or
				(@MortgageID is null and t.MortgageID in (select MortgageID from MortgageRoleAssignment where UserId=@UserID))
			)
			and ((t.RecurrenceID = 6 and DATEDIFF(day, ScheduleDate, @ReportedDate)=0) --Never
			or (DATEDIFF(day,ScheduleDate, @ReportedDate)>=0 and (StatusID = 1 or DATEDIFF(day,CompleteDate,@ReportedDate)<=0) and
				(t.RecurrenceID = 1 --daily
				or (t.RecurrenceID = 2 and DATEDIFF(day,ScheduleDate, @ReportedDate)%2=0 )--every other day
				or (t.RecurrenceID = 3 and DATEPART(Week,ScheduleDate)=DATEPART(Week,@ReportedDate))--Once a week
				or (t.RecurrenceID = 4 and DATEPART(Week,ScheduleDate)=DATEPART(Week,@ReportedDate) and DATEDIFF(Week,ScheduleDate,@ReportedDate)%2=0)--Every other week
				or (t.RecurrenceID = 5 and DATEPART(Month,ScheduleDate)=DATEPART(Month,@ReportedDate))--Once a month
				)
			   )
			 )

	if(@NeedEvents=1)
		insert into @MessageBoard
		select distinct 'E&'+ cast(e.ID as varchar(13)) as ID,
				left(e.Name + '(' + e.Description+')',50) as Title,
				'Event' as MessageType,
				e.MortgageID,
				e.Created,
				0 as IsHighLight
		from vwMortgageProfileEvent e
		inner join MortgageProfile mp on mp.ID = e.MortgageID
		inner join MortgageProfileUser mpu on mpu.MortgageProfileID = mp.ID
		where 
			(e.MortgageID = @MortgageID or
				(@MortgageID is null and e.MortgageID in (select MortgageID from MortgageRoleAssignment where UserId=@UserID))
			) and
			DATEDIFF(day, e.Created, @ReportedDate)=0

	if(@NeedAlerts=1)
		insert into @MessageBoard
		select distinct 'A&'+ cast(a.ID as varchar(13)) as ID,
				left(a.Description, 50) as Title,
				'Alert' as MessageType,
				a.MortgageID,
				a.Created,
				a.IsActive as HighLight
		from vwMortgageProfileAlert a
		inner join MortgageProfile mp on mp.ID = a.MortgageID
		inner join MortgageProfileUser mpu on mpu.MortgageProfileID = mp.ID
		where 
			(a.MortgageID = @MortgageID or
				(@MortgageID is null and a.MortgageID in (select MortgageID from MortgageRoleAssignment where UserId=@UserID))
			) and
			DATEDIFF(day, a.Created, @ReportedDate)=0

	if(@NeedNotes=1)
		insert into @MessageBoard
		select distinct 'N&'+ cast(n.ID as varchar(13)) as ID,
				left(n.Note,50) as Title,
				'Note' as MessageType,
				n.MortgageID,
				n.Created,
				0 as HighLight
		from TaskNote n
		inner join MortgageProfile mp on mp.ID = n.MortgageID
		inner join MortgageProfileUser mpu on mpu.MortgageProfileID = mp.ID
		where 
			(n.MortgageID = @MortgageID or
				(@MortgageID is null and n.MortgageID in (select MortgageID from MortgageRoleAssignment where UserId=@UserID))
			) and
			(@TaskID is null or TaskId = @TaskID) and
			DATEDIFF(day, n.Created, @ReportedDate)=0

--insert here all other items
	select * from @MessageBoard
	order by Created DESC
END
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleEvent]    Script Date: 06/05/2007 11:18:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleEvent]
	@ruleid			int
	,@objectid		int
	,@message		varchar(256)
	,@eventtypeid	int
	,@isnew			bit
AS
DECLARE @newobjid 	int
DECLARE @cnt		int

	IF @isnew=1 BEGIN
		BEGIN TRANSACTION tr
		IF @objectid > 0 BEGIN
			DELETE ra FROM RuleAlert ra 
			INNER JOIN RuleObject ro ON ro.ObjectId=ra.[Id]
			WHERE ro.[Id]=@objectid
			IF @@ERROR <> 0 GOTO ErrorHandler
			DELETE FROM RuleObject WHERE Id=@objectid AND ObjectTypeId=6
			IF @@ERROR <> 0 GOTO ErrorHandler

		END
			INSERT INTO RuleEvent(
				Message
				,EventTypeid
			) VALUES (
				@message
				,@eventtypeid
			)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newobjid = scope_identity()
		EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,7,0
		IF @cnt <> 1 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @newobjid
		RETURN
ErrorHandler:
		ROLLBACK TRANSACTION Tr
		SELECT -2       
		RETURN
	END ELSE BEGIN
		UPDATE RuleEvent
		SET Message = @message
			,EventTypeId = @eventtypeid
		WHERE Id=(SELECT ObjectId FROM RuleObject ro WHERE ro.[id]=@objectid)
		SELECT @objectid
		RETURN
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleAlert]    Script Date: 06/05/2007 11:17:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleAlert]
	@ruleid		int
	,@objectid	int
	,@message	varchar(256)
	,@isnew	bit
AS
DECLARE @newobjid 	int
DECLARE @cnt		int
	IF @isnew=1 BEGIN
		BEGIN TRANSACTION tr
		IF @objectid > 0 BEGIN
			DELETE re FROM RuleEvent re 
			INNER JOIN RuleObject ro ON ro.Objectid=re.[Id]
			WHERE ro.[Id]=@objectid
			IF @@ERROR <> 0 GOTO ErrorHandler
			DELETE FROM RuleObject WHERE Id=@objectid AND ObjectTypeId=7
			IF @@ERROR <> 0 GOTO ErrorHandler
		END
		INSERT INTO RuleAlert (
			Message
		) VALUES (
			@message
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newobjid = scope_identity()
		EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,6,0
		IF @cnt <> 1 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @newobjid
		RETURN
ErrorHandler:
		ROLLBACK TRANSACTION Tr
		SELECT -2       
		RETURN
	END ELSE BEGIN
		UPDATE RuleAlert
		SET Message = @message
		WHERE Id=(SELECT ObjectId FROM RuleObject ro WHERE ro.[id]=@objectid)
		SELECT @objectid
		RETURN
	END
GO
/****** Object:  StoredProcedure [dbo].[GetRuleObjectList]    Script Date: 06/05/2007 11:16:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleObjectList]
	@ruleid		int
	,@objecttypeid	int
AS
	IF @objecttypeid=1 BEGIN
		SELECT ro.Id,mp.Description AS ObjectName, ra.[Name] AS ActionName 
		FROM RuleObject ro
		INNER JOIN MortgageProfileField mp ON mp.Id=ro.ObjectId
		INNER JOIN RuleAction ra ON ra.Id=ro.RuleActionId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=@objecttypeid
	END ELSE IF @objecttypeid=2 BEGIN
		SELECT ro.Id AS [Id]
			,rc.Title,rc.Detail,rc.RoleId
			,r.[Name] RoleName
		FROM RuleObject ro
		INNER JOIN RuleCondition rc ON rc.Id=ro.ObjectId
		INNER JOIN [RoleTemplate] r ON r.Id=rc.Roleid
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=@objecttypeid
	END ELSE IF @objecttypeid=3 BEGIN
		SELECT ro.Id AS [Id]
			,rt.Title,rt.[Description]
			,tt.[Name] AS TaskType
			,ti.[Name] AS InfoSource
			,td.[Name] AS TaskDifficulty
		FROM RuleObject ro
		INNER JOIN RuleTask rt ON rt.Id=ro.ObjectId
		INNER JOIN TaskType tt ON tt.id=rt.TaskTypeId
		INNER JOIN TaskInfosource ti ON ti.id=rt.TaskInfoSourceId
		INNER JOIN TaskDifficulty td ON td.id=rt.TaskDifficultyId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=@objecttypeid
	END ELSE IF @objecttypeid=4 BEGIN
		SELECT	ISNULL(ro.id, 0) ID, 
				dg.[Name] GroupName, 
				dtRelation.[ID] DTRelID, dtRelation.GroupID, dtRelation.IsAppPackage, 
				dtRelation.IsClosingPackage, dtRelation.IsMiscPackage, 
				dt.Title DTTitle, dt.ID DTID 
		FROM	DocTemplateRelation dtRelation
				INNER JOIN RuleObject ro ON ro.ObjectId = dtRelation.[ID] AND ro.objecttypeid = @objecttypeid 
				INNER JOIN DocumentGroup dg ON dg.[id] = dtRelation.GroupID 
				RIGHT JOIN DocTemplate dt ON dt.[ID] = dtRelation.DocTemplateID 
		WHERE	ro.ruleid IS NULL OR ro.ruleid = @ruleid 
	END ELSE IF @objecttypeid=5 BEGIN
		SELECT ro.ObjectId AS [Id]
		FROM RuleObject ro
		INNER JOIN RuleCheckList rcl ON rcl.Id=ro.ObjectId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=@objecttypeid
	END ELSE IF @objecttypeid=10 BEGIN
		SELECT ro.[Id] AS [Id] ,e.Message,'Event' AS Type, e.EventTypeId AS EventTypeId
		FROM RuleObject ro
		INNER JOIN RuleEvent e ON e.Id=ro.ObjectId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=7
		UNION
		SELECT ro.[Id] AS [Id],a.Message,'Alert' AS Type, 0 AS EventTypeId
		FROM RuleObject ro
		INNER JOIN RuleAlert a ON a.Id=ro.ObjectId
		WHERE ro.ruleid=@ruleid AND ro.objecttypeid=6
		ORDER BY Id		
	END ELSE IF @objecttypeid=8 BEGIN
		exec dbo.GetRuleDataList @ruleid
	END
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageCountInStatuses]    Script Date: 06/05/2007 11:15:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE       PROCEDURE [dbo].[GetMortgageCountInStatuses](
	@CompanyID	int, 
	@UserID		int = 0
)AS

SELECT	DISTINCT profStatus.* 
INTO	#ProfStatus
FROM	ProfileStatus profStatus
		INNER JOIN RoleTemplateProfileStatusView rtProfSt ON rtProfSt.ProfileStatusID = profStatus.[id] 
		INNER JOIN RoleTemplate rt ON rt.[id] = rtProfSt.RoleTemplateID 
		INNER JOIN UserRole ur ON ur.roleId = rt.[id] 
WHERE	ur.userId = @UserID

IF NOT EXISTS( SELECT * FROM #ProfStatus ) AND @UserID <= 0
	INSERT	INTO #ProfStatus
	SELECT	profStatus.* 
	FROM	ProfileStatus profStatus


SELECT	profStatus.[id] GroupID, profStatus.[Name] GroupName, ( 'StatusID = ' + CAST(profStatus.[id] AS nvarchar(50)) ) GroupFilter, ISNULL(mProfAvg.MPCount, 0) GroupCount
FROM	#ProfStatus profStatus
	LEFT JOIN 
			(
			SELECT	MPByUser.StatusID, COUNT(*) MPCount
			FROM
				(
				SELECT	DISTINCT	mProf.id ID, mProf.CurProfileStatusID StatusID 
				FROM	MortgageProfile mProf
						INNER JOIN MortgageRoleAssignment mrAssign ON mrAssign.MortgageId = mProf.[id] AND (@UserID <= 0 OR @UserID = mrAssign.UserId)
						INNER JOIN [User] us ON us.[id] = mrAssign.UserId AND us.CompanyId = @CompanyID
--						INNER JOIN MortgageProfileUser mProfUser ON mProfUser.MortgageProfileID = mProf.[id]
--						INNER JOIN UserRole usRole ON usRole.[ID] = mProfUser.UserRoleID AND (@UserID <= 0 OR @UserID = usRole.userId)
--						INNER JOIN [User] us ON us.[id] = usRole.userId AND us.CompanyId = @CompanyID 
				WHERE	mProf.CompanyID = @CompanyID
				) AS MPByUser
			GROUP BY MPByUser.StatusID
			) AS mProfAvg ON mProfAvg.StatusID = profStatus.[id]
WHERE	profStatus.[id] > 0
	
DROP TABLE #ProfStatus 


EXEC	GetMortgageHeaders @CompanyID, @UserID
GO
/****** Object:  StoredProcedure [dbo].[SaveMPStatus]    Script Date: 06/05/2007 11:17:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPStatus](
	@id		INT,
	@status	INT
)
AS
DECLARE @cnt int
	BEGIN TRANSACTION Tr
	EXEC @cnt = dbo.SaveMPStatusHistory @id,@status
	IF @cnt <> 1 GOTO ErrorHandler
	UPDATE MortgageProfile SET CurProfileStatusID=@status WHERE ID = @id
	IF @@ERROR <> 0 GOTO ErrorHandler
	COMMIT TRANSACTION Tr
	RETURN

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[InsertMortgageProfile]    Script Date: 06/05/2007 11:17:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertMortgageProfile]
	@companyId	int
AS
DECLARE @id int
	INSERT INTO MortgageProfile (
		CompanyId
	)
	VALUES(
		@companyId
	)
	SET @id = scope_identity()
	EXEC dbo.SaveMPStatusHistory @id,1
	RETURN @id
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageProfileFieldsForObject]    Script Date: 06/05/2007 11:16:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetMortgageProfileFieldsForObject]
	@objectname		varchar(100)
	,@userid		int
	,@mpstatusid	int
AS
	SELECT mpf.id,mpf.Description,mpf.ValueTypeId
		,mpf.TableName AS TableName
		,mpf.FieldName AS FieldName
		,mpf.ControlTypeId,mpf.DisplayOrder,mpf.ValidationMessage
		,replace(mpf.PropertyName,@objectname+'.','') AS PropertyName
		,dbo.udfGetFieldReadOnly(@userid,@mpstatusid,mpf.id) AS [ReadOnly]
	FROM MortgageProfileField mpf
	INNER JOIN MortgageProfileFieldType mpft ON mpf.ValueTypeId = mpft.id
	WHERE mpf.PropertyName LIKE @objectname+'%'
	AND mpf.DisplayOrder>0
	ORDER BY mpf.DisplayOrder
GO
/****** Object:  StoredProcedure [dbo].[GetObjectRule]    Script Date: 06/05/2007 11:16:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetObjectRule]
	@lenderid		int
	,@objecttypeid 	int
AS
	SELECT ru.Id,ru.RuleId,ru.[Sequence], ru.LogicalOpId, ru.CompareOpId, 
	CASE WHEN ru.RefId<0 
		THEN ru.DataValue 
		ELSE CASE WHEN LiteralValue=1
			THEN CAST(ru.RefId AS varchar(256)) 
			ELSE (SELECT PropertyName FROM MortgageProfileField WHERE id=ru.refId) END
	END AS PropertyValue
	,ru.LogicalNot
	,ru.LiteralValue
	,mpf.PropertyName
	,mpf.ValueTypeId AS PropertyType
	FROM Ruleunit ru
	INNER JOIN (
		SELECT r1.Id FROM vwActiveRule r1
		LEFT OUTER JOIN vwActiveRule r2 ON r2.ParentRuleId=r1.Id AND r2.CompanyId=@lenderId AND r2.StatusId=1
		WHERE r1.CompanyId=1 AND r2.Id IS NULL AND r1.StatusId=1
		UNION 
		SELECT r3.Id FROM vwActiveRule r3 WHERE r3.CompanyId=1 AND r3.StatusId=1
	) r	ON r.id=ru.ruleid
	INNER JOIN MortgageProfileField mpf ON mpf.Id=ru.FieldId 
	WHERE EXISTS ( SELECT 1 FROM RuleObject ro WHERE ro.ruleid=ru.ruleid AND ro.objecttypeid=@objecttypeid)
	ORDER BY ru.Ruleid,ru.[Sequence]
GO
/****** Object:  StoredProcedure [dbo].[EditDocTemplateRelation]    Script Date: 06/05/2007 11:14:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[EditDocTemplateRelation](
		@RuleID				int, 
		@ObjectID			int, 
		@DocTemplateID		int, 
		@GroupID			int, 
		@IsAppPackage		bit, 
		@IsClosingPackage	bit, 
		@IsMiscPackage		bit 
)
AS

DECLARE @ResultID 	INT
SET		@ResultID = 0

IF @ObjectID > 0
	BEGIN
		UPDATE	DocTemplateRelation
		SET		GroupID = @GroupID, 
				IsAppPackage = @IsAppPackage, 
				IsClosingPackage = @IsClosingPackage, 
				IsMiscPackage = @IsMiscPackage
		WHERE	ID = @ObjectID

		SET @ResultID =	@@rowcount
	END
ELSE
	BEGIN
		DECLARE	@CompanyID	int
		SET		@CompanyID =	(
								SELECT TOP 1 c.[id]
								FROM	[Rule] r
										INNER JOIN Company c ON c.[id] = r.CompanyId 
								WHERE	r.[id] = @RuleID
								)

		IF @CompanyID IS NULL OR @DocTemplateID <= 0
			GOTO Finish

		DECLARE	@DTRelationID	int
		SELECT	@DTRelationID = dtRelation.[ID]
		FROM	Company c
				INNER JOIN [Rule] r ON r.CompanyId = c.[id] 
				INNER JOIN RuleObject ro ON ro.Ruleid = r.[id] AND ro.ObjectTypeid = 4 
				INNER JOIN DocTemplateRelation dtRelation ON dtRelation.[ID] = ro.ObjectId AND ro.ObjectTypeid = 4 
		WHERE	dtRelation.DocTemplateID = @DocTemplateID

		BEGIN TRANSACTION tr
			IF @DTRelationID IS NOT NULL
				BEGIN
					DELETE	dtRelation
					FROM	DocTemplateRelation dtRelation
					WHERE	dtRelation.[ID] = @DTRelationID
					IF @@ERROR <> 0 
						GOTO ErrorHandler

					DELETE	ro
					FROM	RuleObject ro
					WHERE	ro.ObjectId = @DTRelationID AND ro.ObjectTypeid = 4 
					IF @@ERROR <> 0 
						GOTO ErrorHandler
				END

			INSERT INTO	DocTemplateRelation
			(
				DocTemplateID, 
				GroupID, 
				IsAppPackage, 
				IsClosingPackage, 
				IsMiscPackage
			)
			VALUES
			(
				@DocTemplateID, 
				@GroupID, 
				@IsAppPackage, 
				@IsClosingPackage, 
				@IsMiscPackage 
			)
			IF @@ERROR <> 0 
				GOTO ErrorHandler

			SET @ResultID = SCOPE_IDENTITY()

			DECLARE	@cnt	INT
			EXEC @cnt = dbo.SaveRuleObject @RuleID, @ResultID, 4, 0
			IF @cnt <> 1 
				GOTO ErrorHandler
		COMMIT TRANSACTION tr

		GOTO Finish

		ErrorHandler:
		ROLLBACK TRANSACTION tr
	END

Finish:
SELECT @ResultID
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageCountInDefault]    Script Date: 06/05/2007 11:15:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[GetMortgageCountInDefault](
	@CompanyID	int, 
	@UserID		int = 0 
)AS

DECLARE	@MPCount int
SET	@MPCount = 
		(
		SELECT	COUNT(*) 
		FROM
			(
			SELECT	DISTINCT	mProf.id ID, mProf.CurProfileStatusID StatusID 
			FROM	MortgageProfile mProf
					LEFT JOIN MortgageRoleAssignment mrAssign ON mrAssign.MortgageId = mProf.[id] AND (@UserID <= 0 OR @UserID = mrAssign.UserId)
					LEFT JOIN [User] us ON us.[id] = mrAssign.UserId AND us.CompanyId = @CompanyID
--					LEFT JOIN MortgageProfileUser mProfUser ON mProfUser.MortgageProfileID = mProf.[id]
--					LEFT JOIN UserRole usRole ON usRole.[ID] = mProfUser.UserRoleID
--					LEFT JOIN [User] us ON us.[id] = usRole.userId AND us.CompanyId = @CompanyID
			WHERE	mProf.CompanyID = @CompanyID 
			) AS MPByUser
		)

SELECT	0 AS GroupID, 'No Grouping' AS GroupName, ('UserID = ' + CAST(@UserID AS nvarchar(50))) AS GroupFilter, @MPCount AS GroupCount

EXEC	GetMortgageHeadersInDefault @CompanyID, @UserID
GO
/****** Object:  StoredProcedure [dbo].[GetMortgageCountInUsers]    Script Date: 06/05/2007 11:15:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE	PROCEDURE [dbo].[GetMortgageCountInUsers](
	@CompanyID	int, 
	@RoleID		int = -1, 
	@UserID		int = -1 
)AS

SELECT	DISTINCT us.[id] GroupID, (us.LastName + ' ' + us.FirstName) GroupName, ( 'UserID = ' + CAST(us.[id] AS nvarchar(50)) ) GroupFilter, ISNULL(mProfUserAvg.MPCount, 0) GroupCount
FROM	[User] us
	LEFT JOIN	
		  (
			SELECT	diffMP.UserID, COUNT(*) MPCount
			FROM 	
				(
				SELECT	DISTINCT mrAssign.UserID UserID, mProf.[id] MPID
				FROM	MortgageProfile mProf
					INNER JOIN MortgageRoleAssignment mrAssign ON	mrAssign.MortgageId = mProf.[id] AND 
																	(@UserID <= 0 OR @UserID = mrAssign.UserId) AND 
																	(@RoleID <= 0 OR @RoleID = mrAssign.RoleId)
--					INNER JOIN MortgageProfileUser mProfUser ON mProfUser.MortgageProfileID = mProf.[id]
--					INNER JOIN UserRole usRole ON usRole.[ID] = mProfUser.UserRoleID AND ( @RoleID < 0 OR @RoleID = usRole.roleId )
				WHERE	mProf.CompanyID = @CompanyID
				) AS diffMP
			GROUP BY diffMP.UserID 
		  ) AS mProfUserAvg ON mProfUserAvg.UserID = us.[id]
	INNER JOIN UserRole usRole ON usRole.userId = us.[id] AND ( @RoleID < 0 OR @RoleID = usRole.roleId )
WHERE	(@UserID <= 0 OR @UserID = us.[id]) AND us.CompanyId = @CompanyID

EXEC	GetMortgageUserHeaders @CompanyID, @RoleID, @UserID
GO
/****** Object:  StoredProcedure [dbo].[CopyRuleForLender]    Script Date: 06/05/2007 11:14:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CopyRuleForLender]
	@srcruleid	int
	,@companyid	int
AS	
DECLARE @cnt int
DECLARE @dstruleid int
DECLARE @res int
	IF @companyId=1 BEGIN
		RETURN @srcruleid
	END
-- check if already exist
	SELECT @cnt = COUNT(*) FROM [Rule]
	WHERE  [Name]=(SELECT [Name] FROM [Rule] WHERE id=@srcruleid AND CompanyId=1) AND CompanyId=@companyid
	IF @cnt > 0 BEGIN
		RETURN -1
	END
	BEGIN TRANSACTION Tr
-- add new rule for company
		INSERT INTO [Rule]
		SELECT [Name],StartDate,EndDate,@companyId,CategoryId,StatusId,@srcruleid
		FROM [Rule] WHERE id=@srcruleid
		IF @@ROWCOUNT=0 GOTO ErrorHandler
		SET @dstruleid = scope_identity()
-- copy all ruleobjects records
		INSERT INTO RuleObject 
		SELECT @dstruleid,ObjectId,ObjectTypeId,RuleActionId,id
		FROM RuleObject WHERE RuleId=@srcruleid
		IF @@ERROR<>0 GOTO ErrorHandler
-- copy all ruleproduct records
		INSERT INTO RuleProduct
		SELECT @dstruleid,ProductId
		FROM RuleProduct WHERE RuleId=@srcruleid
		IF @@ERROR<>0 GOTO ErrorHandler
-- copy all ruleunit records
		INSERT INTO RuleUnit
		SELECT @dstruleid,Sequence,LogicalOpId,FieldId,CompareOPId,DataValue,RefId,LogicalNot,LiteralValue
		FROM RuleUnit WHERE RuleId=@srcruleid
		IF @@ERROR<>0 GOTO ErrorHandler
-- copy Condition objects
		exec @res=dbo.CopyRuleCondition @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy Document objects
		exec @res=dbo.CopyRuleDocument @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy Task objects
		exec @res=dbo.CopyRuleTask @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy CheckList objects
		exec @res=dbo.CopyRuleCheckList @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy Alert objects
		exec @res=dbo.CopyRuleAlert @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy Event objects
		exec @res=dbo.CopyRuleEvent @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler
-- copy Data objects
		exec @res=dbo.CopyRuleData @srcruleid,@dstruleid
		IF @res<>0 GOTO ErrorHandler

		COMMIT TRANSACTION Tr
		RETURN @dstruleid

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	RETURN -2
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleData]    Script Date: 06/05/2007 11:18:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleData]
	@ruleid			int
	,@objectid		int
	,@fieldid		int
	,@fieldvalue	varchar(256)
AS
DECLARE @newobjid 	int
DECLARE @cnt		int
	
	IF @objectid <= 0 BEGIN
		BEGIN TRANSACTION tr
			INSERT INTO RuleData (
				FieldId
				,FieldValue
			) VALUES (
				@fieldid
				,@fieldvalue
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @newobjid = scope_identity()
			EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,8,0
			IF @cnt <> 1 GOTO ErrorHandler
			COMMIT TRANSACTION Tr
			SELECT @newobjid
			RETURN 
ErrorHandler:
			ROLLBACK TRANSACTION Tr
			SELECT -2       
			RETURN		
	END ELSE BEGIN
		UPDATE RuleData
		SET FieldId=@fieldid
			,FieldValue=@fieldvalue
		WHERE Id=(SELECT ObjectId FROM RuleObject ro WHERE ro.[id]=@objectid)
		SELECT @objectid
		RETURN
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleCheckList]    Script Date: 06/05/2007 11:17:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleCheckList]
	@ruleid		int
	,@objectid	int
	,@xml		ntext
AS
DECLARE @newobjid 	int
DECLARE @idoc  		int
DECLARE @cnt		int
	BEGIN TRANSACTION Tr
	IF @objectid < 0 BEGIN
		INSERT INTO RuleCheckList DEFAULT VALUES
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newobjid = scope_identity()
		EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,5,3
		IF @cnt <> 1 GOTO ErrorHandler
	END ELSE BEGIN
		DELETE FROM RuleCheckListItem WHERE CheckListId=@objectid
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newobjid = @objectid
	END
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	INSERT INTO RuleCheckListItem(
		CheckListId
		,Question
		,ProfileStatusId
		,cbYes
		,cbNo
		,cbDontKnow
		,cbToFollow
	) SELECT 
		@newobjid			
		,s.Question
		,s.ProfileStatusId
		,s.cbYes
		,s.cbNo
		,s.cbDontKnow
		,s.cbToFollow
	FROM OPENXML (@idoc,'Root/item',1)
	WITH (
		Question		   varchar(256) '@text'
		,ProfileStatusId	int '@id'
		,cbYes				bit  '@yes'
		,cbNo				bit  '@no'
		,cbDontKnow			bit  '@donotknow'
		,cbToFollow			bit  '@tofollow'
	) s
	IF @@ERROR <> 0 GOTO ErrorHandler
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT 1
	RETURN

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[SaveCompany]    Script Date: 06/05/2007 11:17:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCompany]
	@id			int
	,@name		varchar(100)
	,@logoimage	varchar(100)

AS
DECLARE @cnt int
DECLARE @newid int
DECLARE @res int
	BEGIN TRANSACTION tr
	SELECT @cnt = COUNT(*) FROM [Company]
	WHERE @id<>id AND @name=Company
		IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- already exists
		RETURN
	END 
	IF @id > 0 BEGIN
		UPDATE Company
		SET Company=@name
		,LogoImage=@logoimage
		WHERE id=@id
		IF @@ERROR <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @id
		RETURN			
	END ELSE BEGIN
		INSERT INTO Company(
			Company
			,LogoImage
		) VALUES (
			@name
			,@logoimage
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid = scope_identity()
		exec @res = dbo.CopyAllRoleTemplateForCompany @newid
		IF @res <> 0 GOTO ErrorHandler
		COMMIT TRANSACTION Tr
		SELECT @newid
		RETURN
	END
ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetEvent]    Script Date: 06/05/2007 11:15:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetEvent]
	@id	int
AS
	SELECT * FROM vwMortgageProfileEvent
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleTask]    Script Date: 06/05/2007 11:18:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleTask]
	@ruleid			int
	,@objectid		int
	,@title			varchar(100)
	,@description	varchar(512)
	,@typeid		int
	,@infosourceid	int
	,@difficultyid	int
AS
DECLARE @newobjid 	int
DECLARE @cnt		int

	IF @objectid < 0 BEGIN
		BEGIN TRANSACTION Tr
			INSERT INTO RuleTask (
				Title
				,[Description]
				,TaskTypeid
				,TaskInfoSourceid
				,TaskDifficultyid
			) VALUES (
				@title
				,@description
				,@typeid
				,@infosourceid
				,@difficultyid
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @newobjid = scope_identity()
			EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,3,0
			IF @cnt <> 1 GOTO ErrorHandler
			COMMIT TRANSACTION Tr
			SELECT @newobjid
			RETURN 
ErrorHandler:
			ROLLBACK TRANSACTION Tr
			SELECT -2       
			RETURN
	END ELSE BEGIN
		UPDATE RuleTask
		SET Title = @title
			,[Description]=@description
			,TaskTypeid=@typeid
			,TaskInfoSourceid=@infosourceid
			,TaskDifficultyid=@difficultyid
		WHERE Id=(SELECT ObjectId FROM RuleObject ro WHERE ro.[id]=@objectid)
		SELECT @objectid
		RETURN
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleCondition]    Script Date: 06/05/2007 11:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleCondition]
	@ruleid		int
	,@objectid	int
	,@title		varchar(100)
	,@detail	varchar(256)
	,@roleid	int
AS
DECLARE @newobjid 	int
DECLARE @cnt		int

	IF @objectid < 0 BEGIN
		BEGIN TRANSACTION Tr
			INSERT INTO RuleCondition (
				Title
				,Detail
				,RoleId
			) VALUES (
				@title
				,@detail
				,@roleid
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @newobjid = scope_identity()
			EXEC @cnt = dbo.SaveRuleObject @ruleid,@newobjid,2,0
			IF @cnt <> 1 GOTO ErrorHandler
			COMMIT TRANSACTION Tr
			SELECT @newobjid
			RETURN 
ErrorHandler:
			ROLLBACK TRANSACTION Tr
			SELECT -2       
			RETURN
	END ELSE BEGIN
		UPDATE RuleCondition
		SET Title = @title
		,Detail = @detail
		,RoleId = @roleid
		WHERE Id=(SELECT ObjectId FROM RuleObject ro WHERE ro.[id]=@objectid)
		SELECT @objectid
		RETURN
	END
GO
/****** Object:  View [dbo].[vwRuleList]    Script Date: 06/05/2007 11:37:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vwRuleList]
AS
SELECT r.[id],r.[Name],r.StartDate,r.EndDate,r.CompanyId AS CompanyId, r.StatusId AS StatusId
		,rc.[Name] AS Category
		,rc.[Id] AS CategoryId
		,s.StatusName AS Status
--		,ISNULL(rp.productid,0) AS ProductId
		,dbo.udfGetRuleObject(r.id,0) AS CodeUnit
		,dbo.udfGetRuleObject(r.id,1) AS Field
		,dbo.udfGetRuleObject(r.id,2) AS Condition
		,dbo.udfGetRuleObject(r.id,3) AS Task		
		,dbo.udfGetRuleObject(r.id,4) AS Document
		,dbo.udfGetRuleObject(r.id,5) AS Checklist
		,dbo.udfGetRuleObject(r.id,6)+dbo.udfGetRuleObject(r.id,7) AS Alert
		,dbo.udfGetRuleObject(r.id,8) AS Data
FROM [Rule] r
INNER JOIN RuleCategory rc ON rc.id = r.CategoryId
INNER JOIN Status s ON s.id=r.StatusId
--LEFT OUTER JOIN RuleProduct rp ON rp.ruleid=r.id
WHERE r.Id>0
GO
/****** Object:  StoredProcedure [dbo].[GetLenderUserList]    Script Date: 06/05/2007 11:15:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[GetLenderUserList]
	@companyId		int
	,@orderby varchar(100) = ''
	,@where varchar(1000) = ''

AS
DECLARE @sql nvarchar(2000)
	IF @orderby = '' AND @where = ''
		SELECT *, FirstName +' '+ LastName as FullName FROM vwUser WHERE CompanyId=@companyId
	ELSE BEGIN
	SET @sql = 'SELECT *, FirstName +'' ''+ LastName as FullName FROM vwUser '
		IF @where <> ''
			SET @sql = @sql + @where + ' ' + ' AND CompanyId='+rtrim(ltrim(cast(@companyId as varchar(5))))
		ELSE
			SET @sql = @sql + ' WHERE CompanyId=@companyId'
		IF @orderby <> ''
			SET @sql = @sql + @orderby
		
		EXEC sp_executesql @sql

	END
GO
/****** Object:  StoredProcedure [dbo].[GetDocTemplateVersionById]    Script Date: 06/05/2007 11:15:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    PROCEDURE [dbo].[GetDocTemplateVersionById](
	@id		INT
)
AS
	SELECT * FROM DocTemplateVersion WHERE  ID=@id

	EXEC dbo.GetFieldsByDocTemplateVerID @id
GO
/****** Object:  StoredProcedure [dbo].[SaveMPAssignedUser]    Script Date: 06/05/2007 11:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPAssignedUser]
	@mid			int
	,@companyId		int
	,@xml			ntext
AS
DECLARE @idoc  int
DECLARE @mpid  int

	BEGIN TRANSACTION Tr
	IF @mid > 0 BEGIN
		SET @mpid=@mid
	END ELSE BEGIN
		EXEC @mpid=dbo.InsertMortgageProfile @companyId
	END
	EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
	IF @@ERROR <> 0 GOTO ErrorHandler
	DELETE FROM MortgageRoleAssignment WHERE MortgageId=@mpid
	INSERT INTO MortgageRoleAssignment (
		MortgageId
		,RoleId
		,UserId
	) SELECT 
		@mpid
		,s.RoleId
		,s.UserId
	FROM OPENXML (@idoc,'Root/item',1)
	WITH (
		RoleId	int '@roleid'
		,UserId int '@userid'
	) s
	IF @@ERROR <> 0 GOTO ErrorHandler	
	EXEC sp_xml_removedocument @idoc
	COMMIT TRANSACTION Tr
	SELECT @mpid
	RETURN		

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[SaveMPProperty]    Script Date: 06/05/2007 11:17:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPProperty]
	@mid			int
	,@userId		int
	,@companyId		int
	,@updatedxml	ntext
	,@cntupdated    int
	,@insertedxml	ntext
	,@cntinserted   int
AS
DECLARE @idoc  int
DECLARE @mpid  int
DECLARE @propertyid int

	BEGIN TRANSACTION Tr
	IF @mid > 0 BEGIN
		SET @mpid=@mid
	END ELSE BEGIN
		EXEC @mpid=dbo.InsertMortgageProfile @companyId
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO MortgageRoleAssignment(
			MortgageId
			,RoleId
			,UserId
		)VALUES (
			@mpid
			,4
			,@userId
		)
	END
	IF @cntupdated > 0 BEGIN
		EXEC sp_xml_preparedocument @idoc OUTPUT, @updatedxml
		IF @@ERROR <> 0 GOTO ErrorHandler
		UPDATE b SET 
			b.Address1=s.Address1
			,b.Address2=s.Address2
			,b.City=s.City
			,b.StateId=s.StateId
			,b.Zip=s.Zip
			,b.County=s.County
			,b.Hazard=s.Hazard
			,b.HazDwelling=s.HazDwelling
			,b.HazStart=s.HazStart
			,b.HazExp=s.HazExp
			,b.HazPremium=s.HazPremium
			,b.HazMtgeeClauseOK=s.HazMtgeeClauseOK
			,b.HazAllBorrOnPolicy=s.HazAllBorrOnPolicy
			,b.HazAgencyName=s.HazAgencyName
			,b.HazAgentName=s.HazAgentName
			,b.HazAgencyPhone=s.HazAgencyPhone
			,b.HazAgencyFax=s.HazAgencyFax
			,b.Flood=s.Flood
			,b.FldDwelling=s.FldDwelling
			,b.FldStart=s.FldStart
			,b.FldExp=s.FldExp
			,b.FldPremium=s.FldPremium
			,b.FldMtgeeClauseOK=s.FldMtgeeClauseOK
			,b.FldAllBorrOnPolicy=s.FldAllBorrOnPolicy
			,b.FldAgencyName=s.FldAgencyName
			,b.FldAgentName=s.FldAgentName
			,b.FldAgencyPhone=s.FldAgencyPhone
			,b.FldAgencyFax=s.FldAgencyFax
			,b.SPTitleHeldId=s.SPTitleHeldId
			,b.SPTitleIsHeldInTheseNames=s.SPTitleIsHeldInTheseNames
			,b.SPHeldInTrust=s.SPHeldInTrust
			,b.LOTitle=s.LOTitle
			,b.LOFax=s.LOFax
		FROM MortgageProperty b
		INNER JOIN (SELECT 
			s.Id
			,s.Address1
			,s.Address2
			,s.City
			,s.StateId
			,s.Zip
			,s.County
			,s.Hazard
			,s.HazDwelling
			,s.HazStart
			,s.HazExp
			,s.HazPremium
			,s.HazMtgeeClauseOK
			,s.HazAllBorrOnPolicy
			,s.HazAgencyName
			,s.HazAgentName
			,s.HazAgencyPhone
			,s.HazAgencyFax
			,s.Flood
			,s.FldDwelling
			,s.FldStart
			,s.FldExp
			,s.FldPremium
			,s.FldMtgeeClauseOK
			,s.FldAllBorrOnPolicy
			,s.FldAgencyName
			,s.FldAgentName
			,s.FldAgencyPhone
			,s.FldAgencyFax
			,s.SPTitleHeldId
			,s.SPTitleIsHeldInTheseNames
			,s.SPHeldInTrust
			,s.LOTitle
			,s.LOFax
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			Id					int '@ID'
			,Address1			varchar(256) '@Address1'
			,Address2			varchar(256) '@Address2'
			,City				varchar(50) '@City'
			,StateId			int '@StateId'
			,Zip				varchar(12)	'@Zip'
			,County				varchar(50)	'@County'
			,Hazard				bit '@IsHazard'
			,HazDwelling		int '@HazDwelling'
			,HazStart			datetime '@HazStart'
			,HazExp				datetime '@HazExp'
			,HazPremium			money '@HazPremium'
			,HazMtgeeClauseOK	bit '@HazMtgeeClauseOK'
			,HazAllBorrOnPolicy bit '@HazAllBorrOnPolicy'
			,HazAgencyName		varchar(256) '@HazAgencyName'
			,HazAgentName		varchar(256) '@HazAgentName'
			,HazAgencyPhone		varchar(20) '@HazAgencyPhone'
			,HazAgencyFax		varchar(20) '@HazAgencyFax'
			,Flood				bit '@IsFlood' 
			,FldDwelling		int '@FldDwelling'
			,FldStart			datetime '@FldStart'
			,FldExp				datetime '@FldExp'
			,FldPremium			money '@FldPremium'
			,FldMtgeeClauseOK	bit '@FldMtgeeClauseOK'
			,FldAllBorrOnPolicy bit '@FldAllBorrOnPolicy'
			,FldAgencyName		varchar(256) '@FldAgencyName'
			,FldAgentName		varchar(256) '@FldAgentName'
			,FldAgencyPhone		varchar(20) '@FldAgencyPhone'
			,FldAgencyFax		varchar(20) '@FldAgencyFax'
			,SPTitleHeldId		int '@SPTitleHeldId'
			,SPTitleIsHeldInTheseNames varchar(256) '@SPTitleIsHeldInTheseNames'
			,SPHeldInTrust		bit '@SPHeldInTrust'
			,LOTitle			varchar(256) '@LOTitle'
			,LOFax				varchar(20)'@LOFax'
		)s) s ON s.Id=b.id
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
	END

	IF @cntinserted > 0 BEGIN
		EXEC sp_xml_preparedocument @idoc OUTPUT, @insertedxml
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO MortgageProperty (
			Address1
			,Address2
			,City
			,StateId
			,Zip
			,County
			,Hazard
			,HazDwelling
			,HazStart
			,HazExp
			,HazPremium
			,HazMtgeeClauseOK
			,HazAllBorrOnPolicy
			,HazAgencyName
			,HazAgentName
			,HazAgencyPhone
			,HazAgencyFax
			,Flood
			,FldDwelling
			,FldStart
			,FldExp
			,FldPremium
			,FldMtgeeClauseOK
			,FldAllBorrOnPolicy
			,FldAgencyName
			,FldAgentName
			,FldAgencyPhone
			,FldAgencyFax
			,SPTitleHeldId
			,SPTitleIsHeldInTheseNames
			,SPHeldInTrust
			,LOTitle
			,LOFax
		) SELECT 
			s.Address1
			,s.Address2
			,s.City
			,s.StateId
			,s.Zip
			,s.County
			,s.Hazard
			,s.HazDwelling
			,s.HazStart
			,s.HazExp
			,s.HazPremium
			,s.HazMtgeeClauseOK
			,s.HazAllBorrOnPolicy
			,s.HazAgencyName
			,s.HazAgentName
			,s.HazAgencyPhone
			,s.HazAgencyFax
			,s.Flood
			,s.FldDwelling
			,s.FldStart
			,s.FldExp
			,s.FldPremium
			,s.FldMtgeeClauseOK
			,s.FldAllBorrOnPolicy
			,s.FldAgencyName
			,s.FldAgentName
			,s.FldAgencyPhone
			,s.FldAgencyFax
			,s.SPTitleHeldId
			,s.SPTitleIsHeldInTheseNames
			,s.SPHeldInTrust
			,s.LOTitle
			,s.LOFax
			FROM OPENXML (@idoc,'Root/item',1)
			WITH (
			Id					int '@ID'
			,Address1			varchar(256) '@Address1'
			,Address2			varchar(256) '@Address2'
			,City				varchar(50) '@City'
			,StateId			int '@StateId'
			,Zip				varchar(12)	'@Zip'
			,County				varchar(50)	'@County'
			,Hazard				bit '@IsHazard'
			,HazDwelling		int '@HazDwelling'
			,HazStart			datetime '@HazStart'
			,HazExp				datetime '@HazExp'
			,HazPremium			money '@HazPremium'
			,HazMtgeeClauseOK	bit '@HazMtgeeClauseOK'
			,HazAllBorrOnPolicy bit '@HazAllBorrOnPolicy'
			,HazAgencyName		varchar(256) '@HazAgencyName'
			,HazAgentName		varchar(256) '@HazAgentName'
			,HazAgencyPhone		varchar(20) '@HazAgencyPhone'
			,HazAgencyFax		varchar(20) '@HazAgencyFax'
			,Flood				bit '@IsFlood' 
			,FldDwelling		int '@FldDwelling'
			,FldStart			datetime '@FldStart'
			,FldExp				datetime '@FldExp'
			,FldPremium			money '@FldPremium'
			,FldMtgeeClauseOK	bit '@FldMtgeeClauseOK'
			,FldAllBorrOnPolicy bit '@FldAllBorrOnPolicy'
			,FldAgencyName		varchar(256) '@FldAgencyName'
			,FldAgentName		varchar(256) '@FldAgentName'
			,FldAgencyPhone		varchar(20) '@FldAgencyPhone'
			,FldAgencyFax		varchar(20) '@FldAgencyFax'
			,SPTitleHeldId		int '@SPTitleHeldId'
			,SPTitleIsHeldInTheseNames varchar(256) '@SPTitleIsHeldInTheseNames'
			,SPHeldInTrust		bit '@SPHeldInTrust'
			,LOTitle			varchar(256) '@LOTitle'
			,LOFax				varchar(20)'@LOFax'
			) s
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @propertyid=scope_identity()
			UPDATE MortgageProfile
			SET PropertyId=@propertyid
			WHERE Id=@mpid
			IF @@ERROR <> 0 GOTO ErrorHandler			
	END
	COMMIT TRANSACTION Tr
	SELECT @mpid
	RETURN		

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[SaveMPBorrowers]    Script Date: 06/05/2007 11:17:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMPBorrowers]
	@mid			int
	,@userId		int
	,@companyId		int
	,@updatedxml	ntext
	,@cntupdated    int
	,@insertedxml	ntext
	,@cntinserted   int
AS
DECLARE @idoc  int
DECLARE @mpid  int
DECLARE @borrowerid int

	BEGIN TRANSACTION Tr
	IF @mid > 0 BEGIN
		SET @mpid=@mid
	END ELSE BEGIN
		EXEC @mpid=dbo.InsertMortgageProfile @companyId
		IF @@ERROR <> 0 GOTO ErrorHandler
		INSERT INTO MortgageRoleAssignment(
			MortgageId
			,RoleId
			,UserId
		)VALUES (
			@mpid
			,4
			,@userId
		)
	END
	IF @cntupdated > 0 BEGIN
		EXEC sp_xml_preparedocument @idoc OUTPUT, @updatedxml
		IF @@ERROR <> 0 GOTO ErrorHandler
		UPDATE b SET
			b.FirstName=s.FirstName
			,b.LastName=s.LastName
			,b.MiddleInitial=s.MiddleInitial
			,b.DateOfBirth=s.DateOfBirth
			,b.Address1=s.Address1
			,b.Address2=s.Address2
			,b.City=s.City
			,b.StateId=s.StateId
			,b.Zip=s.Zip
			,b.PhoneNumber=s.PhoneNumber
			,b.SexId=s.Sex
			,b.SSN=s.SSN
			,b.SalutationId=s.SalutationId
			,b.MartialStatusId=s.MartialStatusId
			,b.AkaNames=s.AkaNames
			,b.ActualAge=s.ActualAge
			,b.NearestAge=s.NearestAge
			,b.YearsAtPresentAddress=s.YearsAtPresentAddress
			,b.RealEstateAssets=s.RealEstateAssets
			,b.MonthlyIncome=s.MonthlyIncome
			,b.AvailableAssets=s.AvailableAssets
			,b.DifferentMailingAddress=s.DifferentMailingAddress
			,b.UsePOA=s.UsePOA
			,b.DecJudments=s.DecJudments
			,b.DecBuncruptcy=s.DecBuncruptcy
			,b.DecLawsuit=s.DecLawsuit
			,b.DecFedDebt=s.DecFedDebt
			,b.DecPrimaryres=s.DecPrimaryres
			,b.DecEndorser=s.DecEndorser
			,b.DecUSCitizen=s.DecUSCitizen
			,b.DecPermanentRes=s.DecPermanentRes
			,b.HDMAHide=s.HDMAHide
			,b.HDMARace=s.HDMARace
			,b.HDMAEthnicity=s.HDMAEthnicity
			,b.PoaDurable=s.PoaDurable
			,b.PoaEncumbering=s.PoaEncumbering
			,b.PoaRevocable=s.PoaRevocable
			,b.PoaIncapacitated=s.PoaIncapacitated
			,b.PoaExecutionDate=s.PoaExecutionDate
		FROM Borrower b
		INNER JOIN (SELECT 
			s.Id
			,s.FirstName
			,s.LastName
			,s.MiddleInitial
			,s.DateOfBirth
			,s.Address1
			,s.Address2
			,s.City
			,s.StateId
			,s.Zip
			,s.PhoneNumber
			,s.Sex
			,s.SSN
			,s.SalutationId
			,s.MartialStatusId
			,s.AkaNames
			,s.ActualAge
			,s.NearestAge
			,s.YearsAtPresentAddress
			,s.RealEstateAssets
			,s.MonthlyIncome
			,s.AvailableAssets
			,s.DifferentMailingAddress
			,s.UsePOA
			,s.DecJudments
			,s.DecBuncruptcy
			,s.DecLawsuit
			,s.DecFedDebt
			,s.DecPrimaryres
			,s.DecEndorser
			,s.DecUSCitizen
			,s.DecPermanentRes
			,s.HDMAHide
			,s.HDMARace
			,s.HDMAEthnicity
			,s.PoaDurable
			,s.PoaEncumbering
			,s.PoaRevocable
			,s.PoaIncapacitated
			,s.PoaExecutionDate
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			Id             int '@ID'
			,FirstName		varchar(100) '@FirstName'
			,LastName		varchar(100) '@LastName'
			,MiddleInitial	varchar(50) '@MiddleInitial'
			,DateOfBirth	datetime '@BirthDate'
			,Address1		varchar(256) '@Address1'
			,Address2		varchar(256) '@Address2'
			,City			varchar(50) '@City'
			,StateId		int '@StateID' 
			,Zip			varchar(12) '@Zip'
			,PhoneNumber	varchar(20) '@Phone'
			,Sex			int '@BorrowerSex'
			,SSN			varchar(12) '@SSN'
			,SalutationId   int '@SalutationId'
			,MartialStatusId int '@MartialStatusId'
			,AkaNames		varchar(100) '@AkaNames'
			,ActualAge		int	'@ActualAge'
			,NearestAge		int '@NearestAge'
			,YearsAtPresentAddress int '@YearsAtPresentAddress'
			,RealEstateAssets	money '@RealEstateAssets'
			,MonthlyIncome 	money '@MonthlyIncome'
			,AvailableAssets 	money '@AvailableAssets'
			,DifferentMailingAddress bit '@DifferentMailingAddress'
			,UsePOA bit '@UsePOA'
			,DecJudments bit '@DecJudments'
			,DecBuncruptcy bit '@DecBuncruptcy'
			,DecLawsuit bit '@DecLawsuit'
			,DecFedDebt bit '@DecFedDebt'
			,DecPrimaryres bit '@DecPrimaryres'
			,DecEndorser bit '@DecEndorser'
			,DecUSCitizen bit '@DecUSCitizen'
			,DecPermanentRes bit '@DecPermanentRes'
			,HDMAHide bit '@HDMAHide'
			,HDMARace varchar(50) '@HDMARace'
			,HDMAEthnicity varchar(50) '@HDMAEthnicity'			
			,PoaDurable bit '@PoaDurable'
			,PoaEncumbering bit '@PoaEncumbering'
			,PoaRevocable bit '@PoaRevocable'
			,PoaIncapacitated bit '@PoaIncapacitated'
			,PoaExecutionDate datetime '@PoaExecutionDate'
		)s) s ON s.Id=b.id
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
	END

	IF @cntinserted > 0 BEGIN
		EXEC sp_xml_preparedocument @idoc OUTPUT, @insertedxml
		IF @@ERROR <> 0 GOTO ErrorHandler
		IF @cntinserted = 1 BEGIN
			INSERT INTO Borrower (
				MortgageId
				,FirstName
				,LastName
				,MiddleInitial
				,DateOfBirth
				,Address1
				,Address2
				,City
				,StateId
				,Zip
				,PhoneNumber
				,SexId
				,SSN
				,SalutationId
				,MartialStatusId
				,AkaNames
				,ActualAge
				,NearestAge
				,YearsAtPresentAddress
				,RealEstateAssets
				,MonthlyIncome
				,AvailableAssets
				,DifferentMailingAddress
				,UsePOA
				,DecJudments
				,DecBuncruptcy
				,DecLawsuit
				,DecFedDebt
				,DecPrimaryres
				,DecEndorser
				,DecUSCitizen
				,DecPermanentRes
				,HDMAHide
				,HDMARace
				,HDMAEthnicity
				,PoaDurable
				,PoaEncumbering
				,PoaRevocable
				,PoaIncapacitated
				,PoaExecutionDate
			) SELECT 
				@mpid
				,s.FirstName
				,s.LastName
				,s.MiddleInitial
				,s.DateOfBirth
				,s.Address1
				,s.Address2
				,s.City
				,s.StateId
				,s.Zip
				,s.PhoneNumber
				,s.Sex
				,s.SSN
				,s.SalutationId
				,s.MartialStatusId
				,s.AkaNames
				,s.ActualAge
				,s.NearestAge
				,s.YearsAtPresentAddress
				,s.RealEstateAssets
				,s.MonthlyIncome
				,s.AvailableAssets
				,s.DifferentMailingAddress
				,s.UsePOA
				,s.DecJudments
				,s.DecBuncruptcy
				,s.DecLawsuit
				,s.DecFedDebt
				,s.DecPrimaryres
				,s.DecEndorser
				,s.DecUSCitizen
				,s.DecPermanentRes
				,s.HDMAHide
				,s.HDMARace
				,s.HDMAEthnicity
				,s.PoaDurable
				,s.PoaEncumbering
				,s.PoaRevocable
				,s.PoaIncapacitated
				,s.PoaExecutionDate
			FROM OPENXML (@idoc,'Root/item',1)
			WITH (
				FirstName		varchar(100) '@FirstName'
				,LastName		varchar(100) '@LastName'
				,MiddleInitial	varchar(50) '@MiddleInitial'
				,DateOfBirth	datetime '@BirthDate'
				,Address1		varchar(256) '@Address1'
				,Address2		varchar(256) '@Address2'
				,City			varchar(50) '@City'
				,StateId		int '@StateID' 
				,Zip			varchar(12) '@Zip'
				,PhoneNumber	varchar(20) '@Phone'
				,Sex			bit '@BorrowerSex'
				,SSN			varchar(12) '@SSN'
				,SalutationId   int '@SalutationId'				
				,MartialStatusId int '@MartialStatusId'
				,AkaNames		varchar(100) '@AkaNames'
				,ActualAge		int	'@ActualAge'
				,NearestAge		int '@NearestAge'
				,YearsAtPresentAddress int '@YearsAtPresentAddress'
				,RealEstateAssets	money '@RealEstateAssets'
				,MonthlyIncome 	money '@MonthlyIncome'
				,AvailableAssets 	money '@AvailableAssets'
				,DifferentMailingAddress bit '@DifferentMailingAddress'
				,UsePOA bit '@UsePOA'
				,DecJudments bit '@DecJudments'
				,DecBuncruptcy bit '@DecBuncruptcy'
				,DecLawsuit bit '@DecLawsuit'
				,DecFedDebt bit '@DecFedDebt'
				,DecPrimaryres bit '@DecPrimaryres'
				,DecEndorser bit '@DecEndorser'
				,DecUSCitizen bit '@DecUSCitizen'
				,DecPermanentRes bit '@DecPermanentRes'
				,HDMAHide bit '@HDMAHide'
				,HDMARace varchar(50) '@HDMARace'
				,HDMAEthnicity varchar(50) '@HDMAEthnicity'
				,PoaDurable bit '@PoaDurable'
				,PoaEncumbering bit '@PoaEncumbering'
				,PoaRevocable bit '@PoaRevocable'
				,PoaIncapacitated bit '@PoaIncapacitated'
				,PoaExecutionDate datetime '@PoaExecutionDate'
			) s
			IF @@ERROR <> 0 GOTO ErrorHandler
		END ELSE BEGIN
			CREATE TABLE #Borrower(
				[FirstName] [varchar](100) NOT NULL,
				[LastName] [varchar](100) NOT NULL,
				[MiddleInitial] [varchar](50) NOT NULL,
				[DateOfBirth] [datetime] NULL,
				[Address1] [varchar](256) NOT NULL,
				[Address2] [varchar](256) NULL,
				[City] [varchar](50) NOT NULL,
				[StateId] [int] NOT NULL,
				[Zip] [varchar](12) NOT NULL,
				[PhoneNumber] [varchar](20) NULL,
				[Sex] [int] NULL,
				[SSN] [varchar](12) NULL,
				[SalutationId] [int] NULL,
				[MartialStatusId] [int] NULL,
				[AkaNames] [varchar](100) NULL,
				[ActualAge] [int] NULL,
				[NearestAge] [int] NULL,
				[YearsAtPresentAddress] [int] NULL,
				[RealEstateAssets] [money] NULL,
				[MonthlyIncome] [money] NULL,
				[AvailableAssets] [money] NULL,
				[DifferentMailingAddress] [bit] NULL,
				[UsePOA] [bit] NULL,
				[DecJudments] [bit] NULL,
				[DecBuncruptcy] [bit] NULL,
				[DecLawsuit] [bit] NULL,
				[DecFedDebt] [bit] NULL,
				[DecPrimaryres] [bit] NULL,
				[DecEndorser] [bit] NULL,
				[DecUSCitizen] [bit] NULL,
				[DecPermanentRes] [bit] NULL,
				[HDMAHide] [bit] NULL,
				[HDMARace] [varchar](50) NULL,
				[HDMAEthnicity] [varchar](50) NULL,
				[PoaDurable] [bit] NULL,
				[PoaEncumbering] [bit] NULL,
				[PoaRevocable] [bit] NULL,
				[PoaIncapacitated] [bit] NULL,
				[PoaExecutionDate] [datetime] NULL,

			)
			INSERT INTO #Borrower (
				FirstName
				,LastName
				,MiddleInitial
				,DateOfBirth
				,Address1
				,Address2
				,City
				,StateId
				,Zip
				,PhoneNumber
				,Sex
				,SSN
				,SalutationId
				,MartialStatusId
				,AkaNames
				,ActualAge
				,NearestAge
				,YearsAtPresentAddress
				,RealEstateAssets
				,MonthlyIncome
				,AvailableAssets
				,DifferentMailingAddress
				,UsePOA
				,DecJudments
				,DecBuncruptcy
				,DecLawsuit
				,DecFedDebt
				,DecPrimaryres
				,DecEndorser
				,DecUSCitizen
				,DecPermanentRes
				,HDMAHide
				,HDMARace
				,HDMAEthnicity
				,PoaDurable
				,PoaEncumbering
				,PoaRevocable
				,PoaIncapacitated
				,PoaExecutionDate
			) SELECT 
				s.FirstName
				,s.LastName
				,s.MiddleInitial
				,s.DateOfBirth
				,s.Address1
				,s.Address2
				,s.City
				,s.StateId
				,s.Zip
				,s.PhoneNumber
				,s.Sex
				,s.SSN
				,s.SalutationId
				,s.MartialStatusId
				,s.AkaNames
				,s.ActualAge
				,s.NearestAge
				,s.YearsAtPresentAddress
				,s.RealEstateAssets
				,s.MonthlyIncome
				,s.AvailableAssets
				,s.DifferentMailingAddress
				,s.UsePOA
				,s.DecJudments
				,s.DecBuncruptcy
				,s.DecLawsuit
				,s.DecFedDebt
				,s.DecPrimaryres
				,s.DecEndorser
				,s.DecUSCitizen
				,s.DecPermanentRes
				,s.HDMAHide
				,s.HDMARace
				,s.HDMAEthnicity
				,s.PoaDurable
				,s.PoaEncumbering
				,s.PoaRevocable
				,s.PoaIncapacitated
				,s.PoaExecutionDate
			FROM OPENXML (@idoc,'Root/item',1)
			WITH (
				FirstName		varchar(100) '@FirstName'
				,LastName		varchar(100) '@LastName'
				,MiddleInitial	varchar(50) '@MiddleInitial'
				,DateOfBirth	datetime '@BirthDate'
				,Address1		varchar(256) '@Address1'
				,Address2		varchar(256) '@Address2'
				,City			varchar(50) '@City'
				,StateId		int '@StateID' 
				,Zip			varchar(12) '@Zip'
				,PhoneNumber	varchar(20) '@Phone'
				,Sex			bit '@BorrowerSex'
				,SSN			varchar(12) '@SSN'
				,SalutationId   int '@SalutationId'				
				,MartialStatusId int '@MartialStatusId'
				,AkaNames		varchar(100) '@AkaNames'
				,ActualAge		int	'@ActualAge'
				,NearestAge		int '@NearestAge'
				,YearsAtPresentAddress int '@YearsAtPresentAddress'
				,RealEstateAssets	money '@RealEstateAssets'
				,MonthlyIncome 	money '@MonthlyIncome'
				,AvailableAssets 	money '@AvailableAssets'
				,DifferentMailingAddress bit '@DifferentMailingAddress'
				,UsePOA bit '@UsePOA'
				,DecJudments bit '@DecJudments'
				,DecBuncruptcy bit '@DecBuncruptcy'
				,DecLawsuit bit '@DecLawsuit'
				,DecFedDebt bit '@DecFedDebt'
				,DecPrimaryres bit '@DecPrimaryres'
				,DecEndorser bit '@DecEndorser'
				,DecUSCitizen bit '@DecUSCitizen'
				,DecPermanentRes bit '@DecPermanentRes'
				,HDMAHide bit '@HDMAHide'
				,HDMARace varchar(50) '@HDMARace'
				,HDMAEthnicity varchar(50) '@HDMAEthnicity'
				,PoaDurable bit '@PoaDurable'
				,PoaEncumbering bit '@PoaEncumbering'
				,PoaRevocable bit '@PoaRevocable'
				,PoaIncapacitated bit '@PoaIncapacitated'
				,PoaExecutionDate datetime '@PoaExecutionDate'
			) s
			IF @@ERROR <> 0 GOTO ErrorHandler
			DECLARE @FirstName varchar(100)
			DECLARE @LastName varchar(100)			
			DECLARE @MiddleInitial varchar(50)
			DECLARE @DateOfBirth datetime			
			DECLARE @Address1 varchar(256)
			DECLARE @Address2 varchar(256)
			DECLARE @City varchar(50)
			DECLARE @StateId int 
			DECLARE @Zip varchar(12)
			DECLARE @PhoneNumber varchar(20)
			DECLARE @Sex int
			DECLARE @SSN varchar(12)
			DECLARE @SalutationId int
			DECLARE @MartialStatusId int
			DECLARE @AkaNames		varchar(100)
			DECLARE @ActualAge		int
			DECLARE @NearestAge		int
			DECLARE @YearsAtPresentAddress int
			DECLARE @RealEstateAssets	money
			DECLARE @MonthlyIncome 	money
			DECLARE @AvailableAssets 	money
			DECLARE @DifferentMailingAddress bit
			DECLARE @UsePOA bit
			DECLARE @DecJudments bit
			DECLARE @DecBuncruptcy bit
			DECLARE @DecLawsuit bit
			DECLARE @DecFedDebt bit
			DECLARE @DecPrimaryres bit
			DECLARE @DecEndorser bit
			DECLARE @DecUSCitizen bit
			DECLARE @DecPermanentRes bit
			DECLARE @HDMAHide bit
			DECLARE @HDMARace varchar(50)
			DECLARE @HDMAEthnicity varchar(50)
			DECLARE @PoaDurable bit
			DECLARE @PoaEncumbering bit
			DECLARE @PoaRevocable bit
			DECLARE @PoaIncapacitated bit
			DECLARE @PoaExecutionDate datetime


			DECLARE  cur CURSOR FAST_FORWARD FOR
			SELECT FirstName,LastName,MiddleInitial,DateOfBirth,Address1 
					,Address2,City,StateId,Zip,PhoneNumber,Sex,SSN,SalutationId
					,MartialStatusId,AkaNames,ActualAge,NearestAge,YearsAtPresentAddress
					,RealEstateAssets,MonthlyIncome,AvailableAssets
					,DifferentMailingAddress,UsePOA,DecJudments,DecBuncruptcy
					,DecLawsuit,DecFedDebt,DecPrimaryres,DecEndorser,DecUSCitizen
					,DecPermanentRes,HDMAHide,HDMARace,HDMAEthnicity
					,PoaDurable,PoaEncumbering,PoaRevocable,PoaIncapacitated,PoaExecutionDate

			FROM #Borrower
			OPEN cur
			FETCH NEXT FROM cur
			INTO @FirstName,@LastName,@MiddleInitial,@DateOfBirth,@Address1,@Address2
				,@City,@StateId,@Zip,@PhoneNumber,@Sex,@SSN,@SalutationId
				,@MartialStatusId,@AkaNames,@ActualAge,@NearestAge,@YearsAtPresentAddress
				,@RealEstateAssets,@MonthlyIncome,@AvailableAssets
				,@DifferentMailingAddress,@UsePOA,@DecJudments,@DecBuncruptcy
				,@DecLawsuit,@DecFedDebt,@DecPrimaryres,@DecEndorser,@DecUSCitizen
				,@DecPermanentRes,@HDMAHide,@HDMARace,@HDMAEthnicity
				,@PoaDurable,@PoaEncumbering,@PoaRevocable,@PoaIncapacitated,@PoaExecutionDate

			WHILE @@FETCH_STATUS = 0 BEGIN
				INSERT INTO Borrower (
					MortgageId
					,FirstName
					,LastName
					,MiddleInitial
					,DateOfBirth
					,Address1
					,Address2
					,City
					,StateId
					,Zip
					,PhoneNumber
					,SexId
					,SSN
					,SalutationId
					,MartialStatusId
					,AkaNames
					,ActualAge
					,NearestAge
					,YearsAtPresentAddress
					,RealEstateAssets
					,MonthlyIncome
					,AvailableAssets
					,DifferentMailingAddress
					,UsePOA
					,DecJudments
					,DecBuncruptcy
					,DecLawsuit
					,DecFedDebt
					,DecPrimaryres
					,DecEndorser
					,DecUSCitizen
					,DecPermanentRes
					,HDMAHide
					,HDMARace
					,HDMAEthnicity
					,PoaDurable
					,PoaEncumbering
					,PoaRevocable
					,PoaIncapacitated
					,PoaExecutionDate
					) VALUES (
					@mpid
					,@FirstName
					,@LastName
					,@MiddleInitial
					,@DateOfBirth
					,@Address1
					,@Address2
					,@City
					,@StateId
					,@Zip
					,@PhoneNumber
					,@Sex
					,@SSN
					,@SalutationId
					,@MartialStatusId
					,@AkaNames
					,@ActualAge
					,@NearestAge
					,@YearsAtPresentAddress
					,@RealEstateAssets
					,@MonthlyIncome
					,@AvailableAssets
					,@DifferentMailingAddress
					,@UsePOA
					,@DecJudments
					,@DecBuncruptcy
					,@DecLawsuit
					,@DecFedDebt
					,@DecPrimaryres
					,@DecEndorser
					,@DecUSCitizen
					,@DecPermanentRes
					,@HDMAHide
					,@HDMARace
					,@HDMAEthnicity
					,@PoaDurable
					,@PoaEncumbering
					,@PoaRevocable
					,@PoaIncapacitated
					,@PoaExecutionDate
					)
				IF @@ERROR <> 0 GOTO ErrorHandler			
				FETCH NEXT FROM cur
				INTO @FirstName,@LastName,@MiddleInitial,@DateOfBirth,@Address1,@Address2
					,@City,@StateId,@Zip,@PhoneNumber,@Sex,@SSN,@SalutationId
					,@MartialStatusId,@AkaNames,@ActualAge,@NearestAge,@YearsAtPresentAddress
					,@RealEstateAssets,@MonthlyIncome,@AvailableAssets
					,@DifferentMailingAddress,@UsePOA,@DecJudments,@DecBuncruptcy
					,@DecLawsuit,@DecFedDebt,@DecPrimaryres,@DecEndorser,@DecUSCitizen
					,@DecPermanentRes,@HDMAHide,@HDMARace,@HDMAEthnicity
					,@PoaDurable,@PoaEncumbering,@PoaRevocable,@PoaIncapacitated,@PoaExecutionDate
			END
			CLOSE cur
			DEALLOCATE cur
			DROP TABLE #borrower
		END		
		EXEC sp_xml_removedocument @idoc
	END
	COMMIT TRANSACTION Tr
	SELECT @mpid
	RETURN		

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[SaveRuleCopy]    Script Date: 06/05/2007 11:18:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRuleCopy]
	@id				int
	,@companyid		int
AS
DECLARE @newid int
BEGIN TRANSACTION Tr
	EXEC @newid=dbo.CopyRuleForLender @id,@companyId
	IF @newid < 0
		ROLLBACK TRANSACTION Tr	
	ELSE
		COMMIT TRANSACTION Tr
	SELECT @newid
GO
/****** Object:  StoredProcedure [dbo].[SaveRule]    Script Date: 06/05/2007 11:17:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveRule]
	@id				int
	,@copyFromRule	bit
	,@name			varchar(50)
	,@companyid		int
	,@xml			ntext
	,@categoryid	int
	,@categoryname	varchar(100)
	,@startdate		datetime = null
	,@enddate		datetime = null
AS
DECLARE @cnt int
DECLARE @newid int
DECLARE @idoc  int
DECLARE @catid int
	SET @newid = @id
	
	BEGIN TRANSACTION tr
	IF (@copyFromRule = 1) AND (@newid > 0) BEGIN
		EXEC @newid=dbo.CopyRuleForLender @id,@companyId
	END
	SELECT @cnt = COUNT(*) FROM [Rule]
	WHERE @newid<>id AND @name=[Name] AND CompanyId=@companyid
		IF @cnt > 0 BEGIN
		ROLLBACK TRANSACTION tr
		SELECT -1   -- already exists
		RETURN
	END 	
	IF @categoryid < 0 BEGIN
		SELECT @catid=ISNULL([Id],-1) FROM RuleCategory WHERE [Name]=@categoryname
		IF @catid = -1 BEGIN
			INSERT INTO RuleCategory (
				[Name]
			) VALUES (
				@categoryname
			)
			IF @@ERROR <> 0 GOTO ErrorHandler
			SET @catid=scope_identity()
		END
	END ELSE BEGIN
		SET @catid=@categoryid
	END
	IF @newid > 0 BEGIN
		UPDATE [Rule]
		SET [Name]=@name
		,StartDate=@startdate
		,EndDate=@enddate
		,categoryId=@catid
		WHERE id=@newid
		IF @@ERROR <> 0 GOTO ErrorHandler
	END ELSE BEGIN
		INSERT INTO [Rule](
			[Name]
			,StartDate
			,EndDate
			,CompanyId
			,CategoryId
		) VALUES (
			@name
			,@startdate
			,@enddate
			,@companyId
			,@catid
		)
		IF @@ERROR <> 0 GOTO ErrorHandler
		SET @newid=scope_identity()
	END
	IF DATALENGTH(@xml) > 0 BEGIN
		DELETE FROM RuleProduct WHERE RuleId=@newid
		IF @@ERROR <> 0 GOTO ErrorHandler	
		EXEC sp_xml_preparedocument @idoc OUTPUT, @xml
		IF @@ERROR <> 0 GOTO ErrorHandler	
		INSERT INTO RuleProduct(
			RuleId
			,ProductId
		)
		SELECT 
			@newid
			,s.ProductId
		FROM OPENXML (@idoc,'Root/item',1)
		WITH (
			ProductId		int '@id'
		) s
		IF @@ERROR <> 0 GOTO ErrorHandler
		EXEC sp_xml_removedocument @idoc
	END
	COMMIT TRANSACTION Tr
	SELECT @newid
	RETURN 

ErrorHandler:
	ROLLBACK TRANSACTION Tr
	SELECT -2       
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[GetRuleList]    Script Date: 06/05/2007 11:16:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRuleList]
	@companyid	int
	,@where varchar(1000) = ''
	,@orderby varchar(100) = ''
AS
DECLARE @sql nvarchar(4000)
	IF @companyid=1 BEGIN
		IF @orderby = '' AND @where = ''
			SELECT vrl.* , 0 AS LenderSpecific
			FROM vwRuleList vrl
			WHERE vrl.Companyid=@companyid
			ORDER BY vrl.[Name]
		ELSE BEGIN
			SET @sql = 'SELECT vrl.* , 0 AS LenderSpecific FROM vwRuleList vrl '
			IF @where <> '' 
				SET @sql = @sql + @where + ' AND vrl.Companyid='+rtrim(ltrim(cast(@companyId as varchar(5))))
			ELSE
				SET @sql = @sql + ' WHERE vrl.Companyid='+rtrim(ltrim(cast(@companyId as varchar(5)))) 
			IF @orderby <> ''
				SET @sql = @sql + ' ' + @orderby
			EXEC sp_executesql @sql	
		END
	END ELSE BEGIN
		IF @orderby = '' AND @where = ''
			SELECT vrl.* , 1 AS LenderSpecific
			FROM vwRuleList vrl
			WHERE vrl.Companyid=@companyid		
			UNION 
			SELECT vrl.* , 0 AS LenderSpecific
			FROM vwRuleList vrl
			WHERE vrl.Companyid=1
			ORDER BY [LenderSpecific],vrl.[Name]
		ELSE BEGIN
			SET @sql = 'SELECT vrl.* , 1 AS LenderSpecific FROM vwRuleList vrl '
			IF @where <> '' 
				SET @sql = @sql + @where + ' AND vrl.Companyid='+rtrim(ltrim(cast(@companyId as varchar(5))))
			ELSE
				SET @sql = @sql + ' WHERE vrl.Companyid='+rtrim(ltrim(cast(@companyId as varchar(5)))) 
			SET @sql = @sql + ' UNION ALL '
			SET @sql = @sql +' SELECT vrl.* , 0 AS LenderSpecific FROM vwRuleList vrl '
			IF @where <> '' 
				SET @sql = @sql + @where + ' AND vrl.Companyid=1'
			ELSE
				SET @sql = @sql + ' WHERE vrl.Companyid=1'
			IF @orderby <> ''
				SET @sql = @sql + ' ' + @orderby
			EXEC sp_executesql @sql	
		END
	END
GO
/****** Object:  Check [CK_FieldGroupIndex]    Script Date: 06/05/2007 11:23:19 ******/
ALTER TABLE [dbo].[FieldGroupIndex]  WITH CHECK ADD  CONSTRAINT [CK_FieldGroupIndex] CHECK  (([FieldGroupID]>(0)))
GO
ALTER TABLE [dbo].[FieldGroupIndex] CHECK CONSTRAINT [CK_FieldGroupIndex]
GO
/****** Object:  ForeignKey [FK_Alert_MortgageProfile]    Script Date: 06/05/2007 11:18:42 ******/
ALTER TABLE [dbo].[Alert]  WITH CHECK ADD  CONSTRAINT [FK_Alert_MortgageProfile] FOREIGN KEY([MortgageID])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[Alert] CHECK CONSTRAINT [FK_Alert_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_Borrower_MartialStatus]    Script Date: 06/05/2007 11:19:51 ******/
ALTER TABLE [dbo].[Borrower]  WITH CHECK ADD  CONSTRAINT [FK_Borrower_MartialStatus] FOREIGN KEY([MartialStatusId])
REFERENCES [dbo].[MartialStatus] ([id])
GO
ALTER TABLE [dbo].[Borrower] CHECK CONSTRAINT [FK_Borrower_MartialStatus]
GO
/****** Object:  ForeignKey [FK_Borrower_MortgageProfile]    Script Date: 06/05/2007 11:19:51 ******/
ALTER TABLE [dbo].[Borrower]  WITH CHECK ADD  CONSTRAINT [FK_Borrower_MortgageProfile] FOREIGN KEY([MortgageID])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[Borrower] CHECK CONSTRAINT [FK_Borrower_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_Borrower_Salutation]    Script Date: 06/05/2007 11:19:52 ******/
ALTER TABLE [dbo].[Borrower]  WITH CHECK ADD  CONSTRAINT [FK_Borrower_Salutation] FOREIGN KEY([SalutationId])
REFERENCES [dbo].[Salutation] ([id])
GO
ALTER TABLE [dbo].[Borrower] CHECK CONSTRAINT [FK_Borrower_Salutation]
GO
/****** Object:  ForeignKey [FK_Borrower_Sex]    Script Date: 06/05/2007 11:19:53 ******/
ALTER TABLE [dbo].[Borrower]  WITH CHECK ADD  CONSTRAINT [FK_Borrower_Sex] FOREIGN KEY([SexId])
REFERENCES [dbo].[Sex] ([id])
GO
ALTER TABLE [dbo].[Borrower] CHECK CONSTRAINT [FK_Borrower_Sex]
GO
/****** Object:  ForeignKey [FK_Company_Status]    Script Date: 06/05/2007 11:20:01 ******/
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_Company_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_Company_Status]
GO
/****** Object:  ForeignKey [FK_CompanyPosition_Company]    Script Date: 06/05/2007 11:20:10 ******/
ALTER TABLE [dbo].[CompanyStructure]  WITH CHECK ADD  CONSTRAINT [FK_CompanyPosition_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[CompanyStructure] CHECK CONSTRAINT [FK_CompanyPosition_Company]
GO
/****** Object:  ForeignKey [FK_CompanyPosition_RoleTemplate]    Script Date: 06/05/2007 11:20:11 ******/
ALTER TABLE [dbo].[CompanyStructure]  WITH CHECK ADD  CONSTRAINT [FK_CompanyPosition_RoleTemplate] FOREIGN KEY([RoleTemplateId])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[CompanyStructure] CHECK CONSTRAINT [FK_CompanyPosition_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_CompanyPosition_User]    Script Date: 06/05/2007 11:20:11 ******/
ALTER TABLE [dbo].[CompanyStructure]  WITH CHECK ADD  CONSTRAINT [FK_CompanyPosition_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[CompanyStructure] CHECK CONSTRAINT [FK_CompanyPosition_User]
GO
/****** Object:  ForeignKey [FK_CompanyStructure_CompanyStructure]    Script Date: 06/05/2007 11:20:12 ******/
ALTER TABLE [dbo].[CompanyStructure]  WITH CHECK ADD  CONSTRAINT [FK_CompanyStructure_CompanyStructure] FOREIGN KEY([ParentId])
REFERENCES [dbo].[CompanyStructure] ([id])
GO
ALTER TABLE [dbo].[CompanyStructure] CHECK CONSTRAINT [FK_CompanyStructure_CompanyStructure]
GO
/****** Object:  ForeignKey [FK_CompareFieldOpForbidden_CompareOperation]    Script Date: 06/05/2007 11:20:16 ******/
ALTER TABLE [dbo].[CompareFieldOpForbidden]  WITH CHECK ADD  CONSTRAINT [FK_CompareFieldOpForbidden_CompareOperation] FOREIGN KEY([CompareOpId])
REFERENCES [dbo].[CompareOperation] ([Id])
GO
ALTER TABLE [dbo].[CompareFieldOpForbidden] CHECK CONSTRAINT [FK_CompareFieldOpForbidden_CompareOperation]
GO
/****** Object:  ForeignKey [FK_CompareFieldOpForbidden_MortgageProfileFieldType]    Script Date: 06/05/2007 11:20:17 ******/
ALTER TABLE [dbo].[CompareFieldOpForbidden]  WITH CHECK ADD  CONSTRAINT [FK_CompareFieldOpForbidden_MortgageProfileFieldType] FOREIGN KEY([ValueTypeId])
REFERENCES [dbo].[MortgageProfileFieldType] ([id])
GO
ALTER TABLE [dbo].[CompareFieldOpForbidden] CHECK CONSTRAINT [FK_CompareFieldOpForbidden_MortgageProfileFieldType]
GO
/****** Object:  ForeignKey [FK_Condition_ConditionCategory]    Script Date: 06/05/2007 11:20:42 ******/
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD  CONSTRAINT [FK_Condition_ConditionCategory] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ConditionCategory] ([ID])
GO
ALTER TABLE [dbo].[Condition] CHECK CONSTRAINT [FK_Condition_ConditionCategory]
GO
/****** Object:  ForeignKey [FK_Condition_ConditionSignOffLevel]    Script Date: 06/05/2007 11:20:42 ******/
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD  CONSTRAINT [FK_Condition_ConditionSignOffLevel] FOREIGN KEY([SignOffLevelID])
REFERENCES [dbo].[ConditionSignOffLevel] ([ID])
GO
ALTER TABLE [dbo].[Condition] CHECK CONSTRAINT [FK_Condition_ConditionSignOffLevel]
GO
/****** Object:  ForeignKey [FK_Condition_ConditionStatus]    Script Date: 06/05/2007 11:20:43 ******/
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD  CONSTRAINT [FK_Condition_ConditionStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[ConditionStatus] ([ID])
GO
ALTER TABLE [dbo].[Condition] CHECK CONSTRAINT [FK_Condition_ConditionStatus]
GO
/****** Object:  ForeignKey [FK_Condition_ConditionType]    Script Date: 06/05/2007 11:20:44 ******/
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD  CONSTRAINT [FK_Condition_ConditionType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[ConditionType] ([ID])
GO
ALTER TABLE [dbo].[Condition] CHECK CONSTRAINT [FK_Condition_ConditionType]
GO
/****** Object:  ForeignKey [FK_Condition_User]    Script Date: 06/05/2007 11:20:44 ******/
ALTER TABLE [dbo].[Condition]  WITH CHECK ADD  CONSTRAINT [FK_Condition_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Condition] CHECK CONSTRAINT [FK_Condition_User]
GO
/****** Object:  ForeignKey [FK_ConditionTask_Condition]    Script Date: 06/05/2007 11:21:06 ******/
ALTER TABLE [dbo].[ConditionTask]  WITH CHECK ADD  CONSTRAINT [FK_ConditionTask_Condition] FOREIGN KEY([ConditionID])
REFERENCES [dbo].[Condition] ([ID])
GO
ALTER TABLE [dbo].[ConditionTask] CHECK CONSTRAINT [FK_ConditionTask_Condition]
GO
/****** Object:  ForeignKey [FK_ConditionTask_Task]    Script Date: 06/05/2007 11:21:07 ******/
ALTER TABLE [dbo].[ConditionTask]  WITH CHECK ADD  CONSTRAINT [FK_ConditionTask_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO
ALTER TABLE [dbo].[ConditionTask] CHECK CONSTRAINT [FK_ConditionTask_Task]
GO
/****** Object:  ForeignKey [FK_DocTemplateField_DocTemplateVersion]    Script Date: 06/05/2007 11:22:01 ******/
ALTER TABLE [dbo].[DocTemplateField]  WITH CHECK ADD  CONSTRAINT [FK_DocTemplateField_DocTemplateVersion] FOREIGN KEY([DocTemplateVerID])
REFERENCES [dbo].[DocTemplateVersion] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocTemplateField] CHECK CONSTRAINT [FK_DocTemplateField_DocTemplateVersion]
GO
/****** Object:  ForeignKey [FK_DocTemplateField_MortgageProfileField]    Script Date: 06/05/2007 11:22:02 ******/
ALTER TABLE [dbo].[DocTemplateField]  WITH CHECK ADD  CONSTRAINT [FK_DocTemplateField_MortgageProfileField] FOREIGN KEY([FieldID])
REFERENCES [dbo].[MortgageProfileField] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocTemplateField] CHECK CONSTRAINT [FK_DocTemplateField_MortgageProfileField]
GO
/****** Object:  ForeignKey [FK_DocTemplateRelation_DocTemplate]    Script Date: 06/05/2007 11:22:12 ******/
ALTER TABLE [dbo].[DocTemplateRelation]  WITH CHECK ADD  CONSTRAINT [FK_DocTemplateRelation_DocTemplate] FOREIGN KEY([DocTemplateID])
REFERENCES [dbo].[DocTemplate] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocTemplateRelation] CHECK CONSTRAINT [FK_DocTemplateRelation_DocTemplate]
GO
/****** Object:  ForeignKey [FK_DocTemplateRelation_DocumentGroup]    Script Date: 06/05/2007 11:22:13 ******/
ALTER TABLE [dbo].[DocTemplateRelation]  WITH CHECK ADD  CONSTRAINT [FK_DocTemplateRelation_DocumentGroup] FOREIGN KEY([GroupID])
REFERENCES [dbo].[DocumentGroup] ([id])
GO
ALTER TABLE [dbo].[DocTemplateRelation] CHECK CONSTRAINT [FK_DocTemplateRelation_DocumentGroup]
GO
/****** Object:  ForeignKey [FK_DocTemplateVersion_DocTemplate]    Script Date: 06/05/2007 11:22:24 ******/
ALTER TABLE [dbo].[DocTemplateVersion]  WITH CHECK ADD  CONSTRAINT [FK_DocTemplateVersion_DocTemplate] FOREIGN KEY([DocTemplateID])
REFERENCES [dbo].[DocTemplate] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DocTemplateVersion] CHECK CONSTRAINT [FK_DocTemplateVersion_DocTemplate]
GO
/****** Object:  ForeignKey [FK_Document_DocumentTemplate]    Script Date: 06/05/2007 11:22:30 ******/
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_DocumentTemplate] FOREIGN KEY([TemplateId])
REFERENCES [dbo].[DocumentTemplate] ([id])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_DocumentTemplate]
GO
/****** Object:  ForeignKey [FK_Event_EventType]    Script Date: 06/05/2007 11:22:54 ******/
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_EventType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[EventType] ([ID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_EventType]
GO
/****** Object:  ForeignKey [FK_EventType_EventType]    Script Date: 06/05/2007 11:23:03 ******/
ALTER TABLE [dbo].[EventType]  WITH CHECK ADD  CONSTRAINT [FK_EventType_EventType] FOREIGN KEY([ParentID])
REFERENCES [dbo].[EventType] ([ID])
GO
ALTER TABLE [dbo].[EventType] CHECK CONSTRAINT [FK_EventType_EventType]
GO
/****** Object:  ForeignKey [FK_FieldGroupIndex_FieldGroup]    Script Date: 06/05/2007 11:23:18 ******/
ALTER TABLE [dbo].[FieldGroupIndex]  WITH CHECK ADD  CONSTRAINT [FK_FieldGroupIndex_FieldGroup] FOREIGN KEY([FieldGroupID])
REFERENCES [dbo].[FieldGroup] ([id])
GO
ALTER TABLE [dbo].[FieldGroupIndex] CHECK CONSTRAINT [FK_FieldGroupIndex_FieldGroup]
GO
/****** Object:  ForeignKey [FK_FieldRestriction_Fields]    Script Date: 06/05/2007 11:23:28 ******/
ALTER TABLE [dbo].[FieldRestriction]  WITH NOCHECK ADD  CONSTRAINT [FK_FieldRestriction_Fields] FOREIGN KEY([FieldId])
REFERENCES [dbo].[MortgageProfileField] ([id])
GO
ALTER TABLE [dbo].[FieldRestriction] CHECK CONSTRAINT [FK_FieldRestriction_Fields]
GO
/****** Object:  ForeignKey [FK_FieldRestriction_Group]    Script Date: 06/05/2007 11:23:29 ******/
ALTER TABLE [dbo].[FieldRestriction]  WITH NOCHECK ADD  CONSTRAINT [FK_FieldRestriction_Group] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([id])
GO
ALTER TABLE [dbo].[FieldRestriction] CHECK CONSTRAINT [FK_FieldRestriction_Group]
GO
/****** Object:  ForeignKey [FK_Invoice_InvoiceProvider]    Script Date: 06/05/2007 11:26:17 ******/
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_InvoiceProvider] FOREIGN KEY([ProviderID])
REFERENCES [dbo].[InvoiceProvider] ([ID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_InvoiceProvider]
GO
/****** Object:  ForeignKey [FK_Invoice_InvoiceType]    Script Date: 06/05/2007 11:26:17 ******/
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_InvoiceType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[InvoiceType] ([ID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_InvoiceType]
GO
/****** Object:  ForeignKey [FK_Invoice_Mortgage]    Script Date: 06/05/2007 11:26:18 ******/
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Mortgage] FOREIGN KEY([MortgageID])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Mortgage]
GO
/****** Object:  ForeignKey [FK_Invoice_User]    Script Date: 06/05/2007 11:26:19 ******/
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_User]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_Company]    Script Date: 06/05/2007 11:27:13 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_Company]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_HowManyAssigns]    Script Date: 06/05/2007 11:27:14 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_HowManyAssigns] FOREIGN KEY([HowManyAssignsId])
REFERENCES [dbo].[HowManyAssigns] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_HowManyAssigns]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_RBYNValues]    Script Date: 06/05/2007 11:27:15 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_RBYNValues] FOREIGN KEY([IncludesAssignmentsId])
REFERENCES [dbo].[RBYNValues] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_RBYNValues]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_RBYNValues1]    Script Date: 06/05/2007 11:27:15 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_RBYNValues1] FOREIGN KEY([BlankIncludesId])
REFERENCES [dbo].[RBYNValues] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_RBYNValues1]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_ServiceYourLoan]    Script Date: 06/05/2007 11:27:16 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_ServiceYourLoan] FOREIGN KEY([ServiceYourLoanId])
REFERENCES [dbo].[ServiceYourLoan] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_ServiceYourLoan]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_State]    Script Date: 06/05/2007 11:27:17 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_State] FOREIGN KEY([StateId])
REFERENCES [dbo].[State] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_State]
GO
/****** Object:  ForeignKey [FK_LenderSpecificField_TransferPercentage]    Script Date: 06/05/2007 11:27:17 ******/
ALTER TABLE [dbo].[LenderSpecificField]  WITH CHECK ADD  CONSTRAINT [FK_LenderSpecificField_TransferPercentage] FOREIGN KEY([TransferedPercentageId])
REFERENCES [dbo].[TransferPercentage] ([id])
GO
ALTER TABLE [dbo].[LenderSpecificField] CHECK CONSTRAINT [FK_LenderSpecificField_TransferPercentage]
GO
/****** Object:  ForeignKey [FK_Mail_MailStatus]    Script Date: 06/05/2007 11:27:50 ******/
ALTER TABLE [dbo].[Mail]  WITH CHECK ADD  CONSTRAINT [FK_Mail_MailStatus] FOREIGN KEY([MailStatusID])
REFERENCES [dbo].[MailStatus] ([ID])
GO
ALTER TABLE [dbo].[Mail] CHECK CONSTRAINT [FK_Mail_MailStatus]
GO
/****** Object:  ForeignKey [FK_Mail_MortgageProfile]    Script Date: 06/05/2007 11:27:50 ******/
ALTER TABLE [dbo].[Mail]  WITH CHECK ADD  CONSTRAINT [FK_Mail_MortgageProfile] FOREIGN KEY([MorgageID])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[Mail] CHECK CONSTRAINT [FK_Mail_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_Mail_User]    Script Date: 06/05/2007 11:27:51 ******/
ALTER TABLE [dbo].[Mail]  WITH CHECK ADD  CONSTRAINT [FK_Mail_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Mail] CHECK CONSTRAINT [FK_Mail_User]
GO
/****** Object:  ForeignKey [FK_Attachment_Mail]    Script Date: 06/05/2007 11:28:03 ******/
ALTER TABLE [dbo].[MailAttachment]  WITH CHECK ADD  CONSTRAINT [FK_Attachment_Mail] FOREIGN KEY([MailID])
REFERENCES [dbo].[Mail] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MailAttachment] CHECK CONSTRAINT [FK_Attachment_Mail]
GO
/****** Object:  ForeignKey [FK_MortgageCheckList_MortgageProfile1]    Script Date: 06/05/2007 11:28:26 ******/
ALTER TABLE [dbo].[MortgageCheckList]  WITH CHECK ADD  CONSTRAINT [FK_MortgageCheckList_MortgageProfile1] FOREIGN KEY([MortgageProfileId])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageCheckList] CHECK CONSTRAINT [FK_MortgageCheckList_MortgageProfile1]
GO
/****** Object:  ForeignKey [FK_MortgageCheckList_RuleCheckListItem]    Script Date: 06/05/2007 11:28:27 ******/
ALTER TABLE [dbo].[MortgageCheckList]  WITH CHECK ADD  CONSTRAINT [FK_MortgageCheckList_RuleCheckListItem] FOREIGN KEY([CheckListItemId])
REFERENCES [dbo].[RuleCheckListItem] ([id])
GO
ALTER TABLE [dbo].[MortgageCheckList] CHECK CONSTRAINT [FK_MortgageCheckList_RuleCheckListItem]
GO
/****** Object:  ForeignKey [FK_MortgageProfile_Company]    Script Date: 06/05/2007 11:28:46 ******/
ALTER TABLE [dbo].[MortgageProfile]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfile_Company] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[MortgageProfile] CHECK CONSTRAINT [FK_MortgageProfile_Company]
GO
/****** Object:  ForeignKey [FK_MortgageProfile_MortgageProperty]    Script Date: 06/05/2007 11:28:47 ******/
ALTER TABLE [dbo].[MortgageProfile]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfile_MortgageProperty] FOREIGN KEY([PropertyId])
REFERENCES [dbo].[MortgageProperty] ([id])
GO
ALTER TABLE [dbo].[MortgageProfile] CHECK CONSTRAINT [FK_MortgageProfile_MortgageProperty]
GO
/****** Object:  ForeignKey [FK_MortgageProfileAlert_MortgageProfile]    Script Date: 06/05/2007 11:28:57 ******/
ALTER TABLE [dbo].[MortgageProfileAlert]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfileAlert_MortgageProfile] FOREIGN KEY([MortgageProfileId])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileAlert] CHECK CONSTRAINT [FK_MortgageProfileAlert_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_MortgageProfileAlert_RuleAlert]    Script Date: 06/05/2007 11:28:57 ******/
ALTER TABLE [dbo].[MortgageProfileAlert]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfileAlert_RuleAlert] FOREIGN KEY([RuleAlertId])
REFERENCES [dbo].[RuleAlert] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileAlert] CHECK CONSTRAINT [FK_MortgageProfileAlert_RuleAlert]
GO
/****** Object:  ForeignKey [FK_MortgageCheckList_MortgageProfile]    Script Date: 06/05/2007 11:29:15 ******/
ALTER TABLE [dbo].[MortgageProfileCheckList]  WITH CHECK ADD  CONSTRAINT [FK_MortgageCheckList_MortgageProfile] FOREIGN KEY([MortgageProfileId])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileCheckList] CHECK CONSTRAINT [FK_MortgageCheckList_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_MortgageProfileEvent_MortgageProfile]    Script Date: 06/05/2007 11:29:23 ******/
ALTER TABLE [dbo].[MortgageProfileEvent]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfileEvent_MortgageProfile] FOREIGN KEY([MortgageProfileId])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileEvent] CHECK CONSTRAINT [FK_MortgageProfileEvent_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_MortgageProfileEvent_RuleEvent]    Script Date: 06/05/2007 11:29:24 ******/
ALTER TABLE [dbo].[MortgageProfileEvent]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfileEvent_RuleEvent] FOREIGN KEY([RuleEventId])
REFERENCES [dbo].[RuleEvent] ([ID])
GO
ALTER TABLE [dbo].[MortgageProfileEvent] CHECK CONSTRAINT [FK_MortgageProfileEvent_RuleEvent]
GO
/****** Object:  ForeignKey [FK_Field_ControlType1]    Script Date: 06/05/2007 11:29:48 ******/
ALTER TABLE [dbo].[MortgageProfileField]  WITH CHECK ADD  CONSTRAINT [FK_Field_ControlType1] FOREIGN KEY([ControlTypeId])
REFERENCES [dbo].[ControlType] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileField] CHECK CONSTRAINT [FK_Field_ControlType1]
GO
/****** Object:  ForeignKey [FK_Field_FieldGroup]    Script Date: 06/05/2007 11:29:49 ******/
ALTER TABLE [dbo].[MortgageProfileField]  WITH CHECK ADD  CONSTRAINT [FK_Field_FieldGroup] FOREIGN KEY([FieldGroupId])
REFERENCES [dbo].[FieldGroup] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileField] CHECK CONSTRAINT [FK_Field_FieldGroup]
GO
/****** Object:  ForeignKey [FK_MortgageProfileField_MortgageProfileFieldType]    Script Date: 06/05/2007 11:29:49 ******/
ALTER TABLE [dbo].[MortgageProfileField]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfileField_MortgageProfileFieldType] FOREIGN KEY([ValueTypeId])
REFERENCES [dbo].[MortgageProfileFieldType] ([id])
GO
ALTER TABLE [dbo].[MortgageProfileField] CHECK CONSTRAINT [FK_MortgageProfileField_MortgageProfileFieldType]
GO
/****** Object:  ForeignKey [FK_MortgageProfilePackage_MortgageProfile]    Script Date: 06/05/2007 11:30:05 ******/
ALTER TABLE [dbo].[MortgageProfilePackage]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProfilePackage_MortgageProfile] FOREIGN KEY([MortgageID])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageProfilePackage] CHECK CONSTRAINT [FK_MortgageProfilePackage_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_MortgageProperty_TitleHeld]    Script Date: 06/05/2007 11:31:15 ******/
ALTER TABLE [dbo].[MortgageProperty]  WITH CHECK ADD  CONSTRAINT [FK_MortgageProperty_TitleHeld] FOREIGN KEY([SPTitleHeldId])
REFERENCES [dbo].[TitleHeld] ([id])
GO
ALTER TABLE [dbo].[MortgageProperty] CHECK CONSTRAINT [FK_MortgageProperty_TitleHeld]
GO
/****** Object:  ForeignKey [FK_MortgageRoleAssignment_MortgageProfile]    Script Date: 06/05/2007 11:31:22 ******/
ALTER TABLE [dbo].[MortgageRoleAssignment]  WITH CHECK ADD  CONSTRAINT [FK_MortgageRoleAssignment_MortgageProfile] FOREIGN KEY([MortgageId])
REFERENCES [dbo].[MortgageProfile] ([id])
GO
ALTER TABLE [dbo].[MortgageRoleAssignment] CHECK CONSTRAINT [FK_MortgageRoleAssignment_MortgageProfile]
GO
/****** Object:  ForeignKey [FK_MortgageRoleAssignment_RoleTemplate]    Script Date: 06/05/2007 11:31:23 ******/
ALTER TABLE [dbo].[MortgageRoleAssignment]  WITH CHECK ADD  CONSTRAINT [FK_MortgageRoleAssignment_RoleTemplate] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[MortgageRoleAssignment] CHECK CONSTRAINT [FK_MortgageRoleAssignment_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_MortgageRoleAssignment_User]    Script Date: 06/05/2007 11:31:24 ******/
ALTER TABLE [dbo].[MortgageRoleAssignment]  WITH CHECK ADD  CONSTRAINT [FK_MortgageRoleAssignment_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[MortgageRoleAssignment] CHECK CONSTRAINT [FK_MortgageRoleAssignment_User]
GO
/****** Object:  ForeignKey [FK_MortgageStatusHistory_ProfileStatus]    Script Date: 06/05/2007 11:31:30 ******/
ALTER TABLE [dbo].[MortgageStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_MortgageStatusHistory_ProfileStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ProfileStatus] ([id])
GO
ALTER TABLE [dbo].[MortgageStatusHistory] CHECK CONSTRAINT [FK_MortgageStatusHistory_ProfileStatus]
GO
/****** Object:  ForeignKey [FK_Product_Status]    Script Date: 06/05/2007 11:31:55 ******/
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Status]
GO
/****** Object:  ForeignKey [FK_PropertyCreditor_Creditor]    Script Date: 06/05/2007 11:32:06 ******/
ALTER TABLE [dbo].[PropertyCreditor]  WITH CHECK ADD  CONSTRAINT [FK_PropertyCreditor_Creditor] FOREIGN KEY([CreditorId])
REFERENCES [dbo].[Creditor] ([id])
GO
ALTER TABLE [dbo].[PropertyCreditor] CHECK CONSTRAINT [FK_PropertyCreditor_Creditor]
GO
/****** Object:  ForeignKey [FK_PropertyCreditor_PropertyInfo]    Script Date: 06/05/2007 11:32:06 ******/
ALTER TABLE [dbo].[PropertyCreditor]  WITH CHECK ADD  CONSTRAINT [FK_PropertyCreditor_PropertyInfo] FOREIGN KEY([PropertyInfoId])
REFERENCES [dbo].[PropertyInfo] ([id])
GO
ALTER TABLE [dbo].[PropertyCreditor] CHECK CONSTRAINT [FK_PropertyCreditor_PropertyInfo]
GO
/****** Object:  ForeignKey [FK_RoleField_Company]    Script Date: 06/05/2007 11:32:30 ******/
ALTER TABLE [dbo].[RoleField]  WITH CHECK ADD  CONSTRAINT [FK_RoleField_Company] FOREIGN KEY([Companyid])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[RoleField] CHECK CONSTRAINT [FK_RoleField_Company]
GO
/****** Object:  ForeignKey [FK_RoleField_MortgageProfileField]    Script Date: 06/05/2007 11:32:30 ******/
ALTER TABLE [dbo].[RoleField]  WITH CHECK ADD  CONSTRAINT [FK_RoleField_MortgageProfileField] FOREIGN KEY([FieldId])
REFERENCES [dbo].[MortgageProfileField] ([id])
GO
ALTER TABLE [dbo].[RoleField] CHECK CONSTRAINT [FK_RoleField_MortgageProfileField]
GO
/****** Object:  ForeignKey [FK_RoleField_ProfileStatus]    Script Date: 06/05/2007 11:32:31 ******/
ALTER TABLE [dbo].[RoleField]  WITH CHECK ADD  CONSTRAINT [FK_RoleField_ProfileStatus] FOREIGN KEY([ProfileStatusid])
REFERENCES [dbo].[ProfileStatus] ([id])
GO
ALTER TABLE [dbo].[RoleField] CHECK CONSTRAINT [FK_RoleField_ProfileStatus]
GO
/****** Object:  ForeignKey [FK_RoleField_RoleTemplate]    Script Date: 06/05/2007 11:32:32 ******/
ALTER TABLE [dbo].[RoleField]  WITH CHECK ADD  CONSTRAINT [FK_RoleField_RoleTemplate] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[RoleField] CHECK CONSTRAINT [FK_RoleField_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_RoleTemplate_RoleTemplate]    Script Date: 06/05/2007 11:32:50 ******/
ALTER TABLE [dbo].[RoleTemplate]  WITH CHECK ADD  CONSTRAINT [FK_RoleTemplate_RoleTemplate] FOREIGN KEY([ParentRoleId])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[RoleTemplate] CHECK CONSTRAINT [FK_RoleTemplate_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_RoleTemplateField_Field]    Script Date: 06/05/2007 11:32:56 ******/
ALTER TABLE [dbo].[RoleTemplateField]  WITH CHECK ADD  CONSTRAINT [FK_RoleTemplateField_Field] FOREIGN KEY([FieldId])
REFERENCES [dbo].[MortgageProfileField] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleTemplateField] CHECK CONSTRAINT [FK_RoleTemplateField_Field]
GO
/****** Object:  ForeignKey [FK_RoleTemplateField_ProfileStatus]    Script Date: 06/05/2007 11:32:57 ******/
ALTER TABLE [dbo].[RoleTemplateField]  WITH CHECK ADD  CONSTRAINT [FK_RoleTemplateField_ProfileStatus] FOREIGN KEY([ProfileStatusId])
REFERENCES [dbo].[ProfileStatus] ([id])
GO
ALTER TABLE [dbo].[RoleTemplateField] CHECK CONSTRAINT [FK_RoleTemplateField_ProfileStatus]
GO
/****** Object:  ForeignKey [FK_RoleTemplateField_RoleTemplate]    Script Date: 06/05/2007 11:32:58 ******/
ALTER TABLE [dbo].[RoleTemplateField]  WITH NOCHECK ADD  CONSTRAINT [FK_RoleTemplateField_RoleTemplate] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleTemplate] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleTemplateField] CHECK CONSTRAINT [FK_RoleTemplateField_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_RoleTemplateProfileStatusView_ProfileStatus]    Script Date: 06/05/2007 11:33:12 ******/
ALTER TABLE [dbo].[RoleTemplateProfileStatusView]  WITH CHECK ADD  CONSTRAINT [FK_RoleTemplateProfileStatusView_ProfileStatus] FOREIGN KEY([ProfileStatusID])
REFERENCES [dbo].[ProfileStatus] ([id])
GO
ALTER TABLE [dbo].[RoleTemplateProfileStatusView] CHECK CONSTRAINT [FK_RoleTemplateProfileStatusView_ProfileStatus]
GO
/****** Object:  ForeignKey [FK_RoleTemplateProfileStatusView_RoleTemplate]    Script Date: 06/05/2007 11:33:12 ******/
ALTER TABLE [dbo].[RoleTemplateProfileStatusView]  WITH CHECK ADD  CONSTRAINT [FK_RoleTemplateProfileStatusView_RoleTemplate] FOREIGN KEY([RoleTemplateID])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[RoleTemplateProfileStatusView] CHECK CONSTRAINT [FK_RoleTemplateProfileStatusView_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_Rule_Company]    Script Date: 06/05/2007 11:33:25 ******/
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_Company]
GO
/****** Object:  ForeignKey [FK_Rule_Rule]    Script Date: 06/05/2007 11:33:26 ******/
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Rule] FOREIGN KEY([ParentRuleId])
REFERENCES [dbo].[Rule] ([id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_Rule]
GO
/****** Object:  ForeignKey [FK_Rule_RuleCategory]    Script Date: 06/05/2007 11:33:26 ******/
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_RuleCategory] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[RuleCategory] ([id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_RuleCategory]
GO
/****** Object:  ForeignKey [FK_Rule_Status]    Script Date: 06/05/2007 11:33:27 ******/
ALTER TABLE [dbo].[Rule]  WITH CHECK ADD  CONSTRAINT [FK_Rule_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[Rule] CHECK CONSTRAINT [FK_Rule_Status]
GO
/****** Object:  ForeignKey [FK_RuleCheckListItem_RuleCheckList]    Script Date: 06/05/2007 11:33:54 ******/
ALTER TABLE [dbo].[RuleCheckListItem]  WITH CHECK ADD  CONSTRAINT [FK_RuleCheckListItem_RuleCheckList] FOREIGN KEY([CheckListId])
REFERENCES [dbo].[RuleCheckList] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleCheckListItem] CHECK CONSTRAINT [FK_RuleCheckListItem_RuleCheckList]
GO
/****** Object:  ForeignKey [FK_RuleEvent_EventType]    Script Date: 06/05/2007 11:34:13 ******/
ALTER TABLE [dbo].[RuleEvent]  WITH CHECK ADD  CONSTRAINT [FK_RuleEvent_EventType] FOREIGN KEY([EventTypeID])
REFERENCES [dbo].[EventType] ([ID])
GO
ALTER TABLE [dbo].[RuleEvent] CHECK CONSTRAINT [FK_RuleEvent_EventType]
GO
/****** Object:  ForeignKey [FK_RuleObject_Rule]    Script Date: 06/05/2007 11:34:23 ******/
ALTER TABLE [dbo].[RuleObject]  WITH CHECK ADD  CONSTRAINT [FK_RuleObject_Rule] FOREIGN KEY([Ruleid])
REFERENCES [dbo].[Rule] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleObject] CHECK CONSTRAINT [FK_RuleObject_Rule]
GO
/****** Object:  ForeignKey [FK_RuleObject_RuleObjectType]    Script Date: 06/05/2007 11:34:24 ******/
ALTER TABLE [dbo].[RuleObject]  WITH CHECK ADD  CONSTRAINT [FK_RuleObject_RuleObjectType] FOREIGN KEY([ObjectTypeid])
REFERENCES [dbo].[RuleObjectType] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleObject] CHECK CONSTRAINT [FK_RuleObject_RuleObjectType]
GO
/****** Object:  ForeignKey [FK_RuleProduct_Product]    Script Date: 06/05/2007 11:34:34 ******/
ALTER TABLE [dbo].[RuleProduct]  WITH CHECK ADD  CONSTRAINT [FK_RuleProduct_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleProduct] CHECK CONSTRAINT [FK_RuleProduct_Product]
GO
/****** Object:  ForeignKey [FK_RuleProduct_Rule]    Script Date: 06/05/2007 11:34:35 ******/
ALTER TABLE [dbo].[RuleProduct]  WITH CHECK ADD  CONSTRAINT [FK_RuleProduct_Rule] FOREIGN KEY([Ruleid])
REFERENCES [dbo].[Rule] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleProduct] CHECK CONSTRAINT [FK_RuleProduct_Rule]
GO
/****** Object:  ForeignKey [FK_RuleTask_TaskDifficulty]    Script Date: 06/05/2007 11:34:45 ******/
ALTER TABLE [dbo].[RuleTask]  WITH CHECK ADD  CONSTRAINT [FK_RuleTask_TaskDifficulty] FOREIGN KEY([TaskDifficultyId])
REFERENCES [dbo].[TaskDifficulty] ([ID])
GO
ALTER TABLE [dbo].[RuleTask] CHECK CONSTRAINT [FK_RuleTask_TaskDifficulty]
GO
/****** Object:  ForeignKey [FK_RuleTask_TaskInfoSource]    Script Date: 06/05/2007 11:34:45 ******/
ALTER TABLE [dbo].[RuleTask]  WITH CHECK ADD  CONSTRAINT [FK_RuleTask_TaskInfoSource] FOREIGN KEY([TaskInfoSourceId])
REFERENCES [dbo].[TaskInfoSource] ([ID])
GO
ALTER TABLE [dbo].[RuleTask] CHECK CONSTRAINT [FK_RuleTask_TaskInfoSource]
GO
/****** Object:  ForeignKey [FK_RuleTask_TaskType]    Script Date: 06/05/2007 11:34:46 ******/
ALTER TABLE [dbo].[RuleTask]  WITH CHECK ADD  CONSTRAINT [FK_RuleTask_TaskType] FOREIGN KEY([TaskTypeId])
REFERENCES [dbo].[TaskType] ([ID])
GO
ALTER TABLE [dbo].[RuleTask] CHECK CONSTRAINT [FK_RuleTask_TaskType]
GO
/****** Object:  ForeignKey [FK_RuleUnit_CompareOperation]    Script Date: 06/05/2007 11:35:02 ******/
ALTER TABLE [dbo].[RuleUnit]  WITH CHECK ADD  CONSTRAINT [FK_RuleUnit_CompareOperation] FOREIGN KEY([CompareOpId])
REFERENCES [dbo].[CompareOperation] ([Id])
GO
ALTER TABLE [dbo].[RuleUnit] CHECK CONSTRAINT [FK_RuleUnit_CompareOperation]
GO
/****** Object:  ForeignKey [FK_RuleUnit_LogicalOperation]    Script Date: 06/05/2007 11:35:03 ******/
ALTER TABLE [dbo].[RuleUnit]  WITH CHECK ADD  CONSTRAINT [FK_RuleUnit_LogicalOperation] FOREIGN KEY([LogicalOpId])
REFERENCES [dbo].[LogicalOperation] ([id])
GO
ALTER TABLE [dbo].[RuleUnit] CHECK CONSTRAINT [FK_RuleUnit_LogicalOperation]
GO
/****** Object:  ForeignKey [FK_RuleUnit_Rule]    Script Date: 06/05/2007 11:35:03 ******/
ALTER TABLE [dbo].[RuleUnit]  WITH NOCHECK ADD  CONSTRAINT [FK_RuleUnit_Rule] FOREIGN KEY([RuleId])
REFERENCES [dbo].[Rule] ([id])
GO
ALTER TABLE [dbo].[RuleUnit] CHECK CONSTRAINT [FK_RuleUnit_Rule]
GO
/****** Object:  ForeignKey [FK_Task_TaskDifficulty]    Script Date: 06/05/2007 11:35:56 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskDifficulty] FOREIGN KEY([DifficultyID])
REFERENCES [dbo].[TaskDifficulty] ([ID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskDifficulty]
GO
/****** Object:  ForeignKey [FK_Task_TaskInfoSource]    Script Date: 06/05/2007 11:35:56 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskInfoSource] FOREIGN KEY([InfoSourceID])
REFERENCES [dbo].[TaskInfoSource] ([ID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskInfoSource]
GO
/****** Object:  ForeignKey [FK_Task_TaskRecurrence]    Script Date: 06/05/2007 11:35:57 ******/
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_TaskRecurrence] FOREIGN KEY([RecurrenceID])
REFERENCES [dbo].[TaskRecurrence] ([ID])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_TaskRecurrence]
GO
/****** Object:  ForeignKey [FK_TaskNote_Task]    Script Date: 06/05/2007 11:36:18 ******/
ALTER TABLE [dbo].[TaskNote]  WITH CHECK ADD  CONSTRAINT [FK_TaskNote_Task] FOREIGN KEY([TaskID])
REFERENCES [dbo].[Task] ([id])
GO
ALTER TABLE [dbo].[TaskNote] CHECK CONSTRAINT [FK_TaskNote_Task]
GO
/****** Object:  ForeignKey [FK_TaskNote_User]    Script Date: 06/05/2007 11:36:19 ******/
ALTER TABLE [dbo].[TaskNote]  WITH CHECK ADD  CONSTRAINT [FK_TaskNote_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[TaskNote] CHECK CONSTRAINT [FK_TaskNote_User]
GO
/****** Object:  ForeignKey [FK_TaskType_TaskType]    Script Date: 06/05/2007 11:36:38 ******/
ALTER TABLE [dbo].[TaskType]  WITH CHECK ADD  CONSTRAINT [FK_TaskType_TaskType] FOREIGN KEY([ParentID])
REFERENCES [dbo].[TaskType] ([ID])
GO
ALTER TABLE [dbo].[TaskType] CHECK CONSTRAINT [FK_TaskType_TaskType]
GO
/****** Object:  ForeignKey [FK_User_Company]    Script Date: 06/05/2007 11:37:09 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Company] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Company]
GO
/****** Object:  ForeignKey [FK_User_UserStatus]    Script Date: 06/05/2007 11:37:09 ******/
ALTER TABLE [dbo].[User]  WITH NOCHECK ADD  CONSTRAINT [FK_User_UserStatus] FOREIGN KEY([UserStatusId])
REFERENCES [dbo].[Status] ([id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_UserStatus]
GO
/****** Object:  ForeignKey [FK_UserRole_RoleTemplate]    Script Date: 06/05/2007 11:37:16 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_RoleTemplate] FOREIGN KEY([roleId])
REFERENCES [dbo].[RoleTemplate] ([id])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_RoleTemplate]
GO
/****** Object:  ForeignKey [FK_UserRole_User]    Script Date: 06/05/2007 11:37:17 ******/
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([userId])
REFERENCES [dbo].[User] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
