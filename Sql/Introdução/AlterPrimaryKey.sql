INSERT INTO [dbo].[TABELA DE PRODUTOS]
([CODIGO DO PRODUTO],[NOME DO PRODUTO],[EMBALAGEM],[TAMANHO],[SABOR],[PRE�O DE LISTA])
VALUES
('788975', 'Peda�os de Frutas - 1,5 Litros', 'PET', '1,5 Litros','Ma�a',18.01)

DELETE FROM [dbo].[TABELA DE PRODUTOS] WHERE [CODIGO DO PRODUTO] = '788975';

ALTER TABLE [dbo].[TABELA DE PRODUTOS] ADD CONSTRAINT PK_PRODUTOS
PRIMARY KEY CLUSTERED ([CODIGO DO PRODUTO]);

ALTER TABLE [dbo].[TABELA DE PRODUTOS]
ALTER COLUMN [CODIGO DO PRODUTO] VARCHAR(10) NOT NULL

ALTER TABLE [dbo].[TABELA DE PRODUTOS] ADD CONSTRAINT PK_PRODUTOS
PRIMARY KEY CLUSTERED ([CODIGO DO PRODUTO]);
