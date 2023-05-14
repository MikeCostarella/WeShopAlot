-------------------------------------------------------------------------------------------
-- Counties
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	county.Id
	,(CASE WHEN county.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
	,county.Name AS CountyName
	--,CONCAT(county.Id + '-' + township.Id) AS SomeNumberAsString
	--,(SELECT FORMAT (county.DateFounded, 'yyyy-MM-dd hh:mm:ss')) AS DateFounded
	,(SELECT COUNT(DISTINCT township.Id)
		FROM MSA.Township
		WHERE
			township.CountyId = county.Id
	) AS TownshipCount
	,(STUFF((
		SELECT ',' + township.Name
		FROM
			MSA.Township township
		WHERE
			township.CountyId = county.Id
		GROUP BY township.Name
		FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 2, '')
		) AS Townships			
INTO #T1
FROM MSA.County county

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	CountyName
