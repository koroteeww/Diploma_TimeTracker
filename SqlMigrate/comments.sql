SET IDENTITY_INSERT [dbo].[NewsComments] ON
GO
DELETE FROM [dbo].[NewsComments];
GO

INSERT INTO
	[dbo].[NewsComments]
(	
	Id,
	Body,
	CreationDate,
	Author_Id,
	News_Id
)
SELECT
	nc.comment_id Id,
	REPLACE(
		REPLACE(
			REPLACE(
				REPLACE(
					REPLACE(
						nc.comment_comment,
						'\;','\'
					),
					'\n',char(13)+char(10)
				),
				'\r',''
			),
			'&#092','\'
		),
		'&quot;','"'
	) Title,
	DATEADD(second, nc.comment_datestamp,{d '1970-01-01'}) DateCreated,
	us.Id as Author_Id,
	un.Id AS News_Id
	
FROM [ak-old].e107_comments nc
INNER join [ak-old].e107_news nw on nw.news_id = nc.comment_item_id
INNER join [ak-old].e107_user nu on nu.user_id = CONVERT(int,SUBSTRING(nc.comment_author,0,CHARINDEX('.',nc.comment_author)))
left join [dbo].WeUsers us on us.Login = nu.user_name
LEFT join [dbo].News un on un.Id = nw.news_id

SET IDENTITY_INSERT [dbo].[NewsComments] OFF
GO	