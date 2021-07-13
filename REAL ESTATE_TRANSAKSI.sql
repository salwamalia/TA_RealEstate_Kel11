USE REALESTATE

-----------------------TRANSAKSI----------------------------------------------------------
--===============================-
--TRANSAKSI PEMBELIAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertPembelian]
	@idTbeli varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@idClient varchar(5),
	@total	int,
	@status varchar(5) = 0
AS
BEGIN
INSERT INTO tPembelian
VALUES(@idTbeli,@tanggal,@idProperty,@idClient,@total,@status)
END

------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdatePembelian]
	@idTbeli varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@idClient varchar(5),
	@total	int,
	@status varchar(5) = 0
AS
BEGIN
UPDATE tPembelian SET
	tanggal = @tanggal,
	idProperty = @idProperty,
	idClient = @idClient,
	total = @total,
	status = @status
WHERE idTBeli = @idTbeli
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_DeletePembelian
(
@idTBeli varchar(5)
)
AS
DELETE FROM tPembelian
FROM tPembelian INNER JOIN tDetailPembelian AS Y
ON tPembelian.idTBeli = Y.idTBeli AND Y.idTBeli = @idTBeli
DELETE FROM tDetailPembelian
WHERE tDetailPembelian.idTBeli = @idTBeli
--------------------------------------------------------------------------------------

--===============================-
--DETAIL PEMBELIAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[sp_InsertDetailPembelian]
	@idTBeli varchar(5),
	@idProperty varchar(5),
	@pembayaran varchar(50),
	@idCicilan varchar(5),
	@perBulan int,
	@dp int,
	@total int
AS
BEGIN
INSERT INTO tDetailPembelian
VALUES(@idTbeli,@idProperty,@pembayaran,@idCicilan,@perBulan,@dp,@total)
END
------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_UpdateDetailPembelian]
	@idTBeli varchar(5),
	@idProperty varchar(5),
	@pembayaran varchar(50),
	@idCicilan varchar(5),
	@perBulan int,
	@dp int,
	@total int
AS
BEGIN
UPDATE tDetailPembelian SET
	idProperty = @idProperty,
	pembayaran = @pembayaran,
	idCicilan = @idCicilan,
	perBulan = @perBulan,
	dp = @dp,
	total = @total
WHERE idTBeli = @idTbeli
END
-----------------------------------------------------------------------------------


--/////////////////////////////////////////////////////////////////

--===============================-
--TRANSAKSI PENYEWAAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertPenyewaan]
	@idTSewa varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@idClient varchar(5),
	@total	int,
	@status varchar(5) = 0
AS
BEGIN
INSERT INTO tPenyewaan
VALUES(@idTSewa,@tanggal,@idProperty,@idClient,@total,@status)
END

------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdatePenyewaan]
	@idTSewa varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@idClient varchar(5),
	@total	int,
	@status varchar(5) = 0
AS
BEGIN
UPDATE tPenyewaan SET
	tanggal = @tanggal,
	idProperty = @idProperty,
	idClient = @idClient,
	total = @total,
	status = @status
WHERE idTSewa = @idTSewa
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_DeletePenyewaan
(
@idTSewa varchar(5)
)
AS
DELETE FROM tPenyewaan
FROM tPenyewaan INNER JOIN tDetailPenyewaan AS Y
ON tPenyewaan.idTSewa = Y.idTSewa AND Y.idTSewa = @idTSewa
DELETE FROM tDetailPenyewaan
WHERE tDetailPenyewaan.idTSewa = @idTSewa
--------------------------------------------------------------------------------------
--===============================-
--DETAIL PENYEWAAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertDetailPenyewaan]
	@idTSewa varchar(5),
	@idProperty varchar(5),
	@mulai date,
	@sampai date,
	@pembayaran varchar(50),
	@idCicilan varchar(5),
	@perBulan int,
	@dp int,
	@total int
AS
BEGIN
INSERT INTO tDetailPenyewaan
VALUES(@idTSewa,@idProperty,@mulai,@sampai,@pembayaran,@idCicilan,@perBulan,@dp,@total)
END
------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateDetailPenyewaan]
	@idTSewa varchar(5),
	@idProperty varchar(5),
	@mulai date,
	@sampai date,
	@pembayaran varchar(50),
	@idCicilan varchar(5),
	@perBulan int,
	@dp int,
	@total int
AS
BEGIN
UPDATE tDetailPenyewaan SET
	idProperty = @idProperty,
	mulaiSewa = @mulai,
	sampai = @sampai,
	pembayaran = @pembayaran,
	idCicilan = @idCicilan,
	perBulan = @perBulan,
	dp = @dp,
	total = @total
WHERE idTSewa = @idTSewa
END
-----------------------------------------------------------------------------------

--////////////////////////

SELECT*FROM tPenyewaan
SELECT*FROM tDetailPenyewaan
SELECT*FROM tPembelian
SELECT*FROM tDetailPembelian
SELECT*FROM client
SELECT * FROM property p INNER JOIN pemilik b ON p.idPemilik = b.idPemilik WHERE p.idProperty = 'PT001'
SELECT * FROM tPembelian
UNION
SELECT * FROM tDetailPembelian