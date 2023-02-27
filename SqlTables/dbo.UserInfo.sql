CREATE TABLE [dbo].[UserInfo] (
    [UserId]           INT          IDENTITY (1000, 1) NOT NULL,
    [Password]         VARCHAR (50) NOT NULL,
    [F_Name]           VARCHAR (50) NOT NULL,
    [L_Name]           VARCHAR (50) NOT NULL,
    [Date_Of_Birth]    DATE         NOT NULL,
    [Email]            VARCHAR (50) NOT NULL,
    [Mobile]           VARCHAR (50) NOT NULL,
    [Occupation]       VARCHAR (50) NULL,
    [Address_Street]   VARCHAR (50) NULL,
    [Address_City]     VARCHAR (50) NULL,
    [Address_Province] VARCHAR (50) NULL,
    [Address_Country]  VARCHAR (50) NULL,
    [Postcode]         VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [CHK_AGE] CHECK (datediff(year,[Date_Of_Birth],getdate())>=(18))
);

