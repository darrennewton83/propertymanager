IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'property')

BEGIN
EXEC sp_executesql N'CREATE SCHEMA property'
END

if NOT EXISTS (select 1 from sys.tables 
INNER JOIN sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id
	where sys.tables.name ='address' and sys.schemas.name = 'property')
BEGIN
	CREATE TABLE [property].[address](
		[id] [int] NOT NULL,
		[line1] [nvarchar](250) NOT NULL,
		[line2] [nvarchar](250) NULL,
		[city] [nvarchar](250) NOT NULL,
		[region] [nvarchar](250) NULL,
		[postcode] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_address] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END

if NOT EXISTS (select 1 from sys.tables 
INNER JOIN sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id
	where sys.tables.name ='overview' and sys.schemas.name = 'property')
BEGIN
	CREATE TABLE [property].[overview](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[type] [int] NOT NULL,
		[purchase_price] [money] NULL,
		[purchase_date] [date] NULL,
		[garage] [bit] NOT NULL,
		[parking_spaces] [tinyint] NULL,
		[notes] [nvarchar](max) NULL,
	 CONSTRAINT [PK_properties] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

if NOT EXISTS (select 1 from sys.tables 
INNER JOIN sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id
	where sys.tables.name ='type' and sys.schemas.name = 'property')
BEGIN
	CREATE TABLE [property].[type](
		[id] [int] IDENTITY(1,1) NOT NULL,
		[name] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_type] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
END

if NOT EXISTS (select 1 from sys.indexes WHERE name = 'IX_overview')
BEGIN
	CREATE NONCLUSTERED INDEX [IX_overview] ON [property].[overview]
	(
		[type] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END

if NOT EXISTS (select 1 from sys.indexes WHERE name = 'IX_type')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [IX_type] ON [property].[type]
	(
		[name] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
END

ALTER TABLE [property].[overview] ADD  CONSTRAINT [DF_properties_garage]  DEFAULT ((0)) FOR [garage]
GO
ALTER TABLE [property].[address]  WITH CHECK ADD  CONSTRAINT [FK_address_overview] FOREIGN KEY([id])
REFERENCES [property].[overview] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [property].[address] CHECK CONSTRAINT [FK_address_overview]
GO
ALTER TABLE [property].[overview]  WITH CHECK ADD  CONSTRAINT [FK_overview_type] FOREIGN KEY([type])
REFERENCES [property].[type] ([id])
GO
ALTER TABLE [property].[overview] CHECK CONSTRAINT [FK_overview_type]
GO
ALTER TABLE [property].[overview]  WITH CHECK ADD  CONSTRAINT [CK_properties] CHECK  (([purchase_price]>(0)))
GO
ALTER TABLE [property].[overview] CHECK CONSTRAINT [CK_properties]
GO