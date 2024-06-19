USE [PlantifyDb]
GO

/****** Object:  Table [dbo].[Plants]    Script Date: 6/19/2024 3:55:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Plants](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[common_name] [nvarchar](500) NULL,
	[scientific_name] [nvarchar](max) NULL,
	[other_name] [nvarchar](max) NULL,
	[medium_url] [nvarchar](max) NULL,
	[small_url] [nvarchar](max) NULL,
	[user_id] [nvarchar](450) NULL,
 CONSTRAINT [PK_Plants] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Plants]  WITH CHECK ADD  CONSTRAINT [FK_Plants_AspNetUsers] FOREIGN KEY([user_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[Plants] CHECK CONSTRAINT [FK_Plants_AspNetUsers]
GO


