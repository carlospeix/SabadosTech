SELECT [c0].[Name] AS [CountryName], [c1].[Name] AS [CategoryName], 
       COALESCE(SUM([o].[Quantity]), 0) AS [Quantity],
       COALESCE(SUM([o].[Total]), 0.0) AS [Amount]
FROM [OrderItems] AS [o]
INNER JOIN [Orders] AS [o0] ON [o].[OrderId] = [o0].[Id]
INNER JOIN [Customers] AS [c] ON [o0].[CustomerId] = [c].[Id]
INNER JOIN [Countries] AS [c0] ON [c].[CountryId] = [c0].[Id]
INNER JOIN [Products] AS [p] ON [o].[ProductId] = [p].[Id]
INNER JOIN [Categories] AS [c1] ON [p].[CategoryId] = [c1].[Id]
GROUP BY [c0].[Name], [c1].[Name]
