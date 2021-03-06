USE [master]
GO
/****** Object:  Database [GardenationDb]    Script Date: 11/28/2016 5:42:55 PM ******/
CREATE DATABASE [GardenationDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GardenationDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\GardenationDb.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'GardenationDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\GardenationDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [GardenationDb] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GardenationDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GardenationDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GardenationDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GardenationDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GardenationDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GardenationDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [GardenationDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GardenationDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GardenationDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GardenationDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GardenationDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GardenationDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GardenationDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GardenationDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GardenationDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GardenationDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GardenationDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GardenationDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GardenationDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GardenationDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GardenationDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GardenationDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GardenationDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GardenationDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [GardenationDb] SET  MULTI_USER 
GO
ALTER DATABASE [GardenationDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GardenationDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GardenationDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GardenationDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [GardenationDb] SET DELAYED_DURABILITY = DISABLED 
GO
USE [GardenationDb]
GO
/****** Object:  Table [dbo].[City]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[SpringFrostDate] [datetime] NULL,
	[FallFrostDate] [datetime] NULL,
	[WeatherType] [nvarchar](50) NULL,
	[SpecialMessage] [nvarchar](500) NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Garden]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Garden](
	[GardenID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[CreatedDate] [date] NULL,
	[LastVisitedDate] [date] NULL,
	[SqFeet] [int] NULL,
	[CityID] [int] NOT NULL,
 CONSTRAINT [PK_Garden] PRIMARY KEY CLUSTERED 
(
	[GardenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PromptListItem]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromptListItem](
	[PromptListItemID] [int] IDENTITY(1,1) NOT NULL,
	[TriggerDate] [date] NOT NULL,
	[GardenID] [int] NOT NULL,
	[PromptListTypeID] [int] NOT NULL,
	[VegetableReference] [nvarchar](50) NULL,
	[Message] [nvarchar](300) NULL,
	[Complete] [bit] NULL,
 CONSTRAINT [PK_PromptListItem] PRIMARY KEY CLUSTERED 
(
	[PromptListItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PromptListType]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PromptListType](
	[PromptListTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ColorClass] [nvarchar](50) NULL,
	[IconClass] [nvarchar](50) NULL,
 CONSTRAINT [PK_PromptListType] PRIMARY KEY CLUSTERED 
(
	[PromptListTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vegetable]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vegetable](
	[VegetableID] [int] IDENTITY(1,1) NOT NULL,
	[WaterCountdown] [int] NULL,
	[HarvestedDate] [datetime] NULL,
	[SowDate] [datetime] NULL,
	[GardenID] [int] NOT NULL,
	[VegetableTypeID] [int] NOT NULL,
	[HarvestSuggestionDate] [datetime] NULL,
	[WaterReminderActive] [bit] NULL,
 CONSTRAINT [PK_Vegetable] PRIMARY KEY CLUSTERED 
(
	[VegetableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VegetableType]    Script Date: 11/28/2016 5:42:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VegetableType](
	[VegetableTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[WaterFrequency] [int] NULL,
	[EquipmentNeeded] [nvarchar](100) NULL,
	[SowDateMod] [int] NULL,
	[HarvestDateMod] [int] NULL,
	[ContinualHarvest] [bit] NULL,
	[HarvestInstructions] [nvarchar](500) NULL,
 CONSTRAINT [PK_VegetableType] PRIMARY KEY CLUSTERED 
(
	[VegetableTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Garden]  WITH CHECK ADD  CONSTRAINT [FK_Garden_City] FOREIGN KEY([CityID])
REFERENCES [dbo].[City] ([CityID])
GO
ALTER TABLE [dbo].[Garden] CHECK CONSTRAINT [FK_Garden_City]
GO
ALTER TABLE [dbo].[PromptListItem]  WITH CHECK ADD  CONSTRAINT [FK_PromptListItem_Garden] FOREIGN KEY([GardenID])
REFERENCES [dbo].[Garden] ([GardenID])
GO
ALTER TABLE [dbo].[PromptListItem] CHECK CONSTRAINT [FK_PromptListItem_Garden]
GO
ALTER TABLE [dbo].[PromptListItem]  WITH CHECK ADD  CONSTRAINT [FK_PromptListItem_PromptListType] FOREIGN KEY([PromptListTypeID])
REFERENCES [dbo].[PromptListType] ([PromptListTypeID])
GO
ALTER TABLE [dbo].[PromptListItem] CHECK CONSTRAINT [FK_PromptListItem_PromptListType]
GO
ALTER TABLE [dbo].[Vegetable]  WITH CHECK ADD  CONSTRAINT [FK_Vegetable_Garden] FOREIGN KEY([GardenID])
REFERENCES [dbo].[Garden] ([GardenID])
GO
ALTER TABLE [dbo].[Vegetable] CHECK CONSTRAINT [FK_Vegetable_Garden]
GO
ALTER TABLE [dbo].[Vegetable]  WITH CHECK ADD  CONSTRAINT [FK_Vegetable_VegetableType] FOREIGN KEY([VegetableTypeID])
REFERENCES [dbo].[VegetableType] ([VegetableTypeID])
GO
ALTER TABLE [dbo].[Vegetable] CHECK CONSTRAINT [FK_Vegetable_VegetableType]
GO
USE [master]
GO
ALTER DATABASE [GardenationDb] SET  READ_WRITE 
GO
