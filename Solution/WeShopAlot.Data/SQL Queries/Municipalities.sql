-------------------------------------------------------------------------------------------
-- Municipalities
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipality.Id
	,(CASE WHEN municipality.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    --,municipality.StateId
	,stateProvince.Name AS State_Name
    ,municipality.Name
    --,municipality.MunicipalityTypeId
	,municipalityType.Name AS Type
INTO #T1
FROM
	WSA.Municipality municipality
	LEFT JOIN WSA.StateProvince stateProvince ON municipality.StateId = stateProvince.Id
	LEFT JOIN WSA.MunicipalityType municipalityType ON municipality.MunicipalityTypeId = municipalityType.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	State_Name, Name