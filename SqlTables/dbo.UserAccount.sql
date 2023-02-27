CREATE TABLE [dbo].[UserAccount] (
    [UserId]     INT        NOT NULL,
    [CardNumber] BIGINT     NOT NULL,
    [Balance]    FLOAT (53) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserInfo] ([UserId])
);

