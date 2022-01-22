SET IDENTITY_INSERT [dbo].[News] ON
GO
DELETE FROM [dbo].[News];
GO
INSERT INTO
	[dbo].[News]
(
	[Id],
	[Title],
	[CreationDate],
	[Author_Id],
	[Body]
)
SELECT
	news_id Id,
	REPLACE(
		REPLACE(
			REPLACE(
				REPLACE(
					REPLACE(
						nw.news_title,
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
	DATEADD(second, nw.news_datestamp,{d '1970-01-01'}) DateCreated,
	us.Id as Author_Id,
	REPLACE(
		REPLACE(
			REPLACE(
				REPLACE(
					REPLACE(
						REPLACE(
							CASE 
								WHEN LEN(nw.news_extended)=0 THEN nw.news_body
								WHEN nw.news_extended = nw.news_body then nw.news_body
								ELSE nw.news_extended
							END,
							'[/link]','[/url]'
						),
						'[link=','[url='
					),
					'\n',char(13)+char(10)
				),
				'\r',''
			),
			'&#092;','\'
		),
		'&quot;','"'
	) Body
	
FROM [ak-old].[e107_news] nw
left join [ak-old].e107_user ou on ou.user_id = nw.news_author
left join [dbo].WeUsers us on us.Login = ou.user_name

SET IDENTITY_INSERT [dbo].[News] OFF
GO	

/*
SELECT '1'+char(10)+char(13)+'2' FROM [dbo].[News] WHERE Body like '%&%'

SELECT
	nw.news_title Title,
	convert(varchar,nw.news_id) PublicId
FROM [ak-old].[e107_news] nw
	WHERE LEN(nw.news_extended)<>0
		  AND nw.news_extended <> nw.news_body
*/