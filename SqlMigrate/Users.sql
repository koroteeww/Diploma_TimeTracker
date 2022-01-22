SET IDENTITY_INSERT [dbo].[WeUsers] ON
GO
DELETE FROM [dbo].[WeUsers];
GO
INSERT
 INTO [dbo].[WeUsers]
 (
 Id,
 Login,
 Email,
 Password,
 Salt,
 CreationDate,
 LastLoginDate,
 IsLocked,
 IsApproved
 )
SELECT 
	User_id,
	user_name login,
	user_email email,
	user_password password,
	'md5' salt,
	DATEADD(second, user_join,{d '1970-01-01'}) DateCreated,
	DATEADD(second, user_lastvisit,{d '1970-01-01'}) DateLastLogIn,
	user_ban IsLocked,
	1 IsApproved
FROM [ak-old].[e107_user]

SET IDENTITY_INSERT [dbo].[WeUsers] OFF
GO