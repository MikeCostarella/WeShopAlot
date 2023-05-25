-------------------------------------------------------------------------------------------
-- MunicipalIncomeTaxManagementAgencies
-------------------------------------------------------------------------------------------

DROP TABLE IF EXISTS #T1

SELECT
	municipalIncomeTaxManagementAgency.Id
    ,municipalIncomeTaxManagementAgency.InternalId
	,(CASE WHEN municipalIncomeTaxManagementAgency.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,municipalIncomeTaxManagementAgency.Abbreviation AS TaxAgency_Abbreviation
    ,municipalIncomeTaxManagementAgency.Name AS TaxAgency_Name
INTO #T1
FROM
	WSA.MunicipalIncomeTaxManagementAgency municipalIncomeTaxManagementAgency

SELECT * FROM #T1
WHERE
	1 = 1
	--AND CollectedByRITA = 'Yes'
ORDER BY
	TaxAgency_Name