USE [master]
GO
/****** Object:  Database [AuctionDb]    Script Date: 03.04.2019 0:30:57 ******/
CREATE DATABASE [AuctionDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AuctionDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\AuctionDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AuctionDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\AuctionDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [AuctionDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AuctionDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AuctionDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AuctionDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AuctionDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AuctionDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AuctionDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [AuctionDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AuctionDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AuctionDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AuctionDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AuctionDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AuctionDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AuctionDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AuctionDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AuctionDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AuctionDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AuctionDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AuctionDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AuctionDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AuctionDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AuctionDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AuctionDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AuctionDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AuctionDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AuctionDb] SET  MULTI_USER 
GO
ALTER DATABASE [AuctionDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AuctionDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AuctionDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AuctionDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AuctionDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AuctionDb] SET QUERY_STORE = OFF
GO
USE [AuctionDb]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 03.04.2019 0:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[DoB] [datetime] NULL,
	[OrganizationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Employees] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LotItemAttachments]    Script Date: 03.04.2019 0:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LotItemAttachments](
	[AttachmentId] [uniqueidentifier] NOT NULL,
	[AttachmentName] [nvarchar](max) NULL,
	[AttachmentExtension] [nvarchar](max) NULL,
	[AttachmentBody] [varbinary](max) NULL,
	[LotItemId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.LotItemAttachments] PRIMARY KEY CLUSTERED 
(
	[AttachmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LotItems]    Script Date: 03.04.2019 0:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LotItems](
	[LotId] [uniqueidentifier] NOT NULL,
	[LotName] [nvarchar](max) NULL,
	[LotDescription] [nvarchar](max) NULL,
	[PublishedDate] [datetime] NOT NULL,
	[InitialCost] [decimal](18, 2) NOT NULL,
	[CreatedByEmployeeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.LotItems] PRIMARY KEY CLUSTERED 
(
	[LotId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 03.04.2019 0:30:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[OrganizationId] [uniqueidentifier] NOT NULL,
	[OrganizationName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Organizations] PRIMARY KEY CLUSTERED 
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Employees_dbo.Organizations_OrganizationId] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([OrganizationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_dbo.Employees_dbo.Organizations_OrganizationId]
GO
ALTER TABLE [dbo].[LotItemAttachments]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LotItemAttachments_dbo.LotItems_LotItemId] FOREIGN KEY([LotItemId])
REFERENCES [dbo].[LotItems] ([LotId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LotItemAttachments] CHECK CONSTRAINT [FK_dbo.LotItemAttachments_dbo.LotItems_LotItemId]
GO
ALTER TABLE [dbo].[LotItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.LotItems_dbo.Employees_CreatedByEmployeeId] FOREIGN KEY([CreatedByEmployeeId])
REFERENCES [dbo].[Employees] ([EmployeeId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LotItems] CHECK CONSTRAINT [FK_dbo.LotItems_dbo.Employees_CreatedByEmployeeId]
GO
USE [master]
GO
ALTER DATABASE [AuctionDb] SET  READ_WRITE 
GO
