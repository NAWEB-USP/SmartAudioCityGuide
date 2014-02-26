USE [master]
GO
/****** Object:  Database [smartAudioCityGuide_db]    Script Date: 26/02/2014 10:14:59 ******/
CREATE DATABASE [smartAudioCityGuide_db]
GO
ALTER DATABASE [smartAudioCityGuide_db] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [smartAudioCityGuide_db].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ARITHABORT OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET RECOVERY FULL 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET  MULTI_USER 
GO
ALTER DATABASE [smartAudioCityGuide_db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [smartAudioCityGuide_db] SET DB_CHAINING OFF 
GO
USE [smartAudioCityGuide_db]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 26/02/2014 10:14:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](255) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Codes]    Script Date: 26/02/2014 10:15:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Codes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.Codes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Comments]    Script Date: 26/02/2014 10:15:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[locationsId] [int] NOT NULL,
	[typeOfCommentsId] [int] NOT NULL,
	[description] [nvarchar](300) NULL,
	[archiveDescription] [nvarchar](300) NULL,
	[isText] [bit] NOT NULL,
	[sound] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 26/02/2014 10:15:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NULL,
	[eMail] [nvarchar](300) NULL,
	[country] [nvarchar](300) NULL,
	[city] [nvarchar](300) NULL,
	[phone] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Locations]    Script Date: 26/02/2014 10:15:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[longitude] [float] NOT NULL,
	[latitude] [float] NOT NULL,
 CONSTRAINT [PK_dbo.Locations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[TypeOfComments]    Script Date: 26/02/2014 10:15:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeOfComments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NULL,
	[description] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.TypeOfComments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[UserLocations]    Script Date: 26/02/2014 10:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLocations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[windowsPhoneId] [nvarchar](300) NULL,
	[latitude] [float] NOT NULL,
	[longitude] [float] NOT NULL,
	[requestTime] [datetime] NOT NULL,
	[hash] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.UserLocations] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Users]    Script Date: 26/02/2014 10:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NOT NULL,
	[userName] [nvarchar](300) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[authenticate] [int] NOT NULL,
	[hash] [nvarchar](300) NULL,
	[typeOfBlindness] [int] NOT NULL,
	[idFacebook] [nvarchar](300) NULL,
	[acessTokenFacebook] [nvarchar](300) NULL,
	[phoneId] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
INSERT [dbo].[__MigrationHistory] ([MigrationId], [Model], [ProductVersion]) VALUES (N'201210261349088_InitialCreate', 0x1F8B0800000000000400ED5CCD6EDB3810BE2FB0EF20E8B47BA8E5A440D116768BD4698A609BA4A8D3DE6989B6894AA42A5269FC6C3DEC23ED2BECE89F94284BB225DB0172296AFE7C33246746C3E187FCF7FBDFC9FB47CF351E70C009A353F36C34360D4C6DE610BA9A9AA158BE786DBE7FF7E71F938F8EF7687CCFC69D47E36026E553732D84FFD6B2B8BDC61EE2238FD801E36C294636F32CE430EB7C3C7E6D9D8D2D0C10266019C6E46B4805F170FC037ECE18B5B12F42E4DE3007BB3C6D879E798C6ADC220F731FD9786ACE3D14888BD0216C46C4E653481C3C4A6699C6854B106834C7EEB2A37AE337917A662E18447F0415C5E67EE3E358FCD4FCC661F9F21018F40FDE280DD0F425603E0EC4E62B5EA61389631A963ACF2A4FCCA7497322D953F39A8A97E7A6711BBA2E5AB8D0B0442EC7A6E1BF7A3B172CC09F30C50112D8F98284C0019CCEB58363DDD33D78EBBF6AB70D6FACF179B40D16A2940924E0A82B8A97D4A4F06FA6E85C046035A671451EB1F319D39558E7CADEA0C7ACE5E5186CE71B25606430490421D62C6EBBD4104EE2F628927DC4F92F16381D259FED2F1985621D1DAB0D47DD641ADB91D688AF7BDDB9E4F776A102C4DD2D3FB8843A1473BEDF0A887305B160C1D88F83AF03E4727ECF7E607A3415FC35A3F8BAAB0976953BB18A00B8352C7E66761C2A9E43A3AAA6CBE88A88D0C9BDF59285A064676377415A6798D6A737639E075BF27C78D52FCC75A3AA4D06903AC6BE4049ECCC4E6A5F3407733B207EB20B878E9E81BD260FF8F2882A107E8F1F4526F603632E46B4F3267216D2AE1118FE3B4C049E01E0B303AB6A469B7C2A5FC8E8E2F0FC95D4ABD947A0FD054925FBC5BF1C242FEAE9033D5CBE10E09F21E6E29E14F7A34B38DDE4F7E95D153A44392A90FD9CA90C7D036F63F2F80611F7E0526DF8E88A607378B9F1491EE396772ABE77AF64A0CF1E787C0F3C5C16BFDD48E2A84C60F753BD34E5D97820A964A8C9FC39165226C04DA3109614724749D9B5BC1DE5D9F9654B8350E45E4D287666E15590DCF89B31A26C5C0710B737CC0EA55CB16633BA2C27FD64EAB4C9BA1A30D4CBA706A9141BB4B6935B4951D9B792D27EF60460D5BC014C6E90EF83594B6F02698B314F1E04662FE6DD8BFD5E8261D95C53F3CFB5CD25418C412B5CEA4D2E5E5724E002122CB44091A3CD1CAF326C079FC824CBAE510EB2C54964A3A3FFD7BA61FA4A32D23C63C8DB7A052B8D4E325E34CE15D23E7FA433E7367251A009DE33E6861EADFB006C9B9DC454797ED2D21EA1782390518AD6F64845CD5F462A5ADB23A9357C194DED698F9824E83252D2D21EA1529897C12A9DED71E51ABD6A07457B879DD394DD95FDD3F47738E1ECE6A81C70D658C59958256729BBA655F1CD52EA5376F45661C0D5D710760F0535458918B2291C6C993B4C4890EEC73288D4DC012BBFB22B5079EBC91CB9AD4DB9773F717D0ADFEAC0EBA70E73DE5965A81CBEF52E596F3552115EB51BA9A36BB8946BF1D57829F7B64756327A1954E9E8103235B57625646AFA3B84F7B48CAE9C6FDAD61E252DA3CB2069D3097960A5B8BE8FFB69C05AF99E76DE308E9794CEE5F949CBC91C897247EA312BDEE773D830FF944364B97C2EA395FB86FCC41EE2C3AF14C76534A56388FCFB68E14B5735DF2782E9F15A05B1BAA9A77A874CCBDC3244DAD4259AA6456B35A0A68D1D70E262A60212B774BCE568EE382764ADA562534F36BBAD7A1DE336596E13C0A9DAEFAE29E5A0A75FA9099687E4D2F3DA60A9063849EB71CD64E14A812E19621AB0510F602151716EC305F646D180D1FCA73B7309ACB71870832859461F86A8B63035A3FAE1EE3CE3FC718173C73D49B23189D6DEF8A4D2F115597E20A10FD1BD0305D527893D39C0BD019729BE5AE0B31D80750CDE78BFF778946FB3EA3DE8B9BBA85725E7F6A5643DF7B62F09256AED6EB04F81393B889F57182C4B97A1EE0654E6D3B44179027CD741B65CA54FEDE2AE1A96EA2E30751CD55DB0346FDBBD85905A026A5F12547EE982745FBEC22DCDD4FACB438F7F3F6DBEE8200E20F33C070ED747A6719E68FCD0932FFBF2A65DBE05837D9834CC4A078E41ECCBAC1CD86E8F479C3C89DB44673E635FA025BA626FB0121BB1D7BCF740E6786C2EE19331CADED2A06E0CBE2AA9A9A674A7542A6A797B49C50556B360B08A44C57694BECFED287D3A01ED29725B886D45974E446B3EE06C3B1F508FDD862A58E202367105EB0EA2CB5EB5A013EAD7D3926C58E61236920D75D27A2224568B8C134BFEBB0513B84490550111FD15038AED682B0BD06CCC355DB2CCBD61B9B246D99092F7DF608120BB401781204BD83CE88ECA1F31C3F73B724318F2D15B60E79ADE85C20FC505E7D85BB8CA8BC0C4DA2E3F665DAA3A4FEEE280C3FB5802A849A204E98E7E0889EBE47A5F69EEEF35109127A4611AB49A8B285CAF3639D26DE5F9A20E28DDBE4BEC631A05F97BECF9905C627E47E7E801D7EBD6BC87EA8E4D2E095A05C8E32946311F7E82F939DEE3BBFF0159CAF3BD79430000, N'5.0.0.net40')

SET IDENTITY_INSERT [dbo].[Codes] ON 
INSERT [dbo].[Codes] ([id], [code]) VALUES (1, N'wonders')
SET IDENTITY_INSERT [dbo].[Codes] OFF

USE [master]
GO
ALTER DATABASE [smartAudioCityGuide_db] SET  READ_WRITE 
GO
