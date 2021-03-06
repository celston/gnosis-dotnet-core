/****** Object:  Table [dbo].[PREFIX_FieldUniqueidentifier]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldUniqueidentifier](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PREFIX_FieldUniqueidentifier] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_FieldText]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldText](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PREFIX_FieldText] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_FieldInteger]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldInteger](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [int] NULL,
 CONSTRAINT [PK_PREFIX_FieldInteger] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_FieldDecimal]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldDecimal](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [decimal](28, 13) NULL,
 CONSTRAINT [PK_PREFIX_FieldDecimal] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_FieldDateTime]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldDateTime](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [datetime] NULL,
 CONSTRAINT [PK_PREFIX_FieldDateTime] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_FieldBit]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_FieldBit](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[fieldName] [varchar](256) NOT NULL,
	[delta] [int] NOT NULL,
	[value] [bit] NULL,
 CONSTRAINT [PK_PREFIX_FieldBit] PRIMARY KEY CLUSTERED 
(
	[revision] ASC,
	[fieldName] ASC,
	[delta] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_EntityRevision]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_EntityRevision](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[author] [uniqueidentifier] NULL,
	[label] [varchar](256) NULL,
	[created] [datetime] NOT NULL,
 CONSTRAINT [PK_PREFIX_Entity_Revision] PRIMARY KEY CLUSTERED 
(
	[revision] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_Entity]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_Entity](
	[id] [uniqueidentifier] NOT NULL,
	[revision] [uniqueidentifier] NOT NULL,
	[type] [varchar](256) NOT NULL,
	[label] [varchar](256) NULL,
	[author] [uniqueidentifier] NULL,
	[created] [datetime] NOT NULL,
	[updated] [datetime] NOT NULL,
	[status] [bit] NOT NULL,
	[protected] [bit] NULL,
 CONSTRAINT [PK_PREFIX_Entity] PRIMARY KEY CLUSTERED 
(
	[status] ASC,
	[type] ASC,
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[revision] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_DataVarbinary]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PREFIX_DataVarbinary](
	[md5] [uniqueidentifier] NOT NULL,
	[data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_PREFIX_DataVarbinary] PRIMARY KEY CLUSTERED 
(
	[md5] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PREFIX_DataText]    Script Date: 04/01/2015 07:52:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PREFIX_DataText](
	[md5] [uniqueidentifier] NOT NULL,
	[data] [text] NOT NULL,
 CONSTRAINT [PK_PREFIX_DataText] PRIMARY KEY CLUSTERED 
(
	[md5] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Default [DF_Entity_status]    Script Date: 04/01/2015 07:52:10 ******/
ALTER TABLE [dbo].[PREFIX_Entity] ADD  CONSTRAINT [DF_PREFIX_Entity_status]  DEFAULT ((1)) FOR [status]
GO
