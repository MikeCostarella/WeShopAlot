-------------------------------------------------------------------------------------------
-- MunicipalityTypes
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipalityType.Id
	,(CASE WHEN municipalityType.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,municipalityType.Name
    ,municipalityType.Description
INTO #T1
FROM
	WSA.MunicipalityType municipalityType

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Name