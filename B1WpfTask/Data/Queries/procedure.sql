USE B1Database;

GO
ALTER PROCEDURE CalculateSumAndMedian AS
BEGIN
    DECLARE @SumInt BIGINT;
    DECLARE @Median FLOAT;

    SELECT @SumInt = SUM(CAST(PositiveEvenNumber AS BIGINT))
    FROM Rows;

    WITH OrderedNumbers AS (
    SELECT
        PositiveNumber,
        ROW_NUMBER() OVER (ORDER BY PositiveNumber) AS RowAsc,
        COUNT(*) OVER () AS TotalRows
    FROM
        Rows
	)

	SELECT TOP 1
		@Median = PositiveNumber
	FROM
		OrderedNumbers
	WHERE
		RowAsc IN ((TotalRows + 1) / 2, (TotalRows + 2) / 2);

    SELECT
        SumInt = @SumInt,
        MedianDecimal = @Median;
END;