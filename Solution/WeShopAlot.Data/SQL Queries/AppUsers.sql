-------------------------------------------------------------------------------------------
-- AppUsers
-------------------------------------------------------------------------------------------
--DELETE FROM WSA.AppUser;

DROP TABLE IF EXISTS #T1

SELECT
	--TOP (1000)
	appUser.Id
	,(CASE WHEN appUser.IsDeleted = 1 THEN 'Yes' ELSE 'No' END) AS Deleted
    ,appUser.DisplayName
    ,appUser.EmailAddress
    ,appUser.AddressId
	,address.AddressLine1
	,address.AddressLine2
	,address.City
	,address.State
	,address.ZipCode
    ,appUser.PasswordHash
INTO #T1
FROM
	WSA.AppUser appUser
	LEFT JOIN WSA.Address address ON appUser.AddressId = address.Id

SELECT * FROM #T1
WHERE
	1 = 1
ORDER BY
	Id