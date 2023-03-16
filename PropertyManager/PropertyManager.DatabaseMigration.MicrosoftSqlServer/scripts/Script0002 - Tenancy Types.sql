IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = 'tenancy')

BEGIN
EXEC sp_executesql N'CREATE SCHEMA tenancy'
END

if NOT EXISTS (select 1 from sys.tables 
INNER JOIN sys.schemas ON sys.schemas.schema_id = sys.tables.schema_id
	where sys.tables.name ='type' and sys.schemas.name = 'tenancy')
CREATE TABLE [tenancy].[type](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_type_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_type_1] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO