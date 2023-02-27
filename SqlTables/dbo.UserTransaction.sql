CREATE TABLE [dbo].[UserTransaction] (
    [RecordId]          INT          IDENTITY (100, 1) NOT NULL,
    [UserId]            INT          NOT NULL,
    [CardNumber]        BIGINT       NOT NULL,
    [TransactionType]   VARCHAR (50) NOT NULL,
    [TransactionTime]   DATETIME     NOT NULL,
    [TransactionAmount] FLOAT (53)   NOT NULL,
    PRIMARY KEY CLUSTERED ([RecordId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserInfo] ([UserId])
);

