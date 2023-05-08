declare @constraint [nvarchar](1000)
declare DROPCONSTRAINT cursor for

SELECT 'ALTER TABLE[OG].[' + OBJECT_NAME(parent_object_id) + '] DROP CONSTRAINT' + name AS DROPCONSTRAINT
FROM sys.foreign_keys
open DROPCONSTRAINT
Fetch next from DROPCONSTRAINT into @CONSTRAINT
	while (@@FETCH_Status = 0)
	begin
	exec Sp_executesql @CONSTRAINT
	Fetch next from DROPCONSTRAINT into @CONSTRAINT
	end
	close DROPCONSTRAINT
	deallocate DROPCONSTRAINT

declare @Table [nvarchar](1000)
declare DROPTABLE cursor for
SELECT 'Drop table' + QUOTENAME(s.NAME) + '.' + QUOTENAME(t.NAME) + ';' AS DROPTABLE
	FROM sys.tables t
	JOIN sys.schemas s
	  ON t.[schema_id] = s.[schema_id]
	WHERE is_external = 0
open DROPTABLE
Fetch next from DROPTABLE into @Table
	while (@@FETCH_Status = 0)
	begin
	exec Sp_executesql @Table
	Fetch next from DROPTABLE into @Table
	END
	close DROPTABLE
	deallocate DROPTABLE