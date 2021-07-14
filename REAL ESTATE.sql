CREATE DATABASE REALESTATE
USE REALESTATE

CREATE TABLE jabatan
(	idJabatan	varchar(5)	NOT NULL	PRIMARY KEY,
	jabatan		varchar(50)	NOT NULL
);

CREATE TABLE pegawai
(	idPegawai		varchar(5)	NOT NULL	PRIMARY KEY,
	nama			varchar(50)	NOT NULL,
	jeniskelamin	varchar(50) NOT NULL,
	username		varchar(15)	NOT NULL,
	password		varchar(15)	NOT NULL,
	idJabatan		varchar(5)	NOT NULL,
);

CREATE TABLE client
(	idClient		varchar(5)		NOT NULL	PRIMARY KEY,
	nama			varchar(50)		NOT NULL,
	jeniskelamin	varchar(50)		NOT NULL,
	telepon			varchar(13)		NOT NULL,
	email			varchar(50)		NOT NULL,
	alamat			varchar(100)	NOT NULL,
);

CREATE TABLE pemilik
(	idPemilik		varchar(5)		NOT NULL	PRIMARY KEY,
	nama			varchar(50)		NOT NULL,
	jeniskelamin	varchar(50)		NOT NULL,
	telepon			varchar(13)		NOT NULL,
	email			varchar(50)		NOT NULL,
	alamat			varchar(100)	NOT NULL,
);

CREATE TABLE propertyTipe
(	idTipe		varchar(5)		NOT NULL	PRIMARY KEY,
	nama		varchar(50)		NOT NULL
);

CREATE TABLE property
(	idProperty		varchar(5)		NOT NULL	PRIMARY KEY,
	namaProperty	varchar(50)		NOT NULL,
	idTipe			varchar(5)		NOT NULL,
	idPemilik		varchar(5)		NOT NULL,
	ukuran			varchar(50)		NOT NULL,
	fasilitas		varchar(100)	NULL,
	harga			int				NOT NULL,
	gambar			image			NULL,
	idInterior		varchar(5)		NOT NULL,
	statusProperty	varchar(5)		NULL,
);

CREATE TABLE desainInterior
(	idInterior	varchar(5)		NOT NULL	PRIMARY KEY,
	interior	varchar(50)		NOT NULL
);

CREATE TABLE kategoriCicilan
(	idCicilan		varchar(5)		NOT NULL	PRIMARY KEY,
	cicilan			varchar(50)		NOT NULL,
);

-----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------

CREATE TABLE TSewaProperty
(	idTSewaProperty		varchar(5)		NOT NULL	PRIMARY KEY,
	tanggal				date			NOT NULL,
	idClient			varchar(5)		NOT NULL,
	idProperty			varchar(5)		NOT NULL,
	mulaiSewa			date			NOT NULL,
	sampai				date			NOT NULL,
	harga				int				NOT NULL
);

-----------------------------------------------------------------------------------------
CREATE TABLE TJualPropertyLunas
(	idJPLunas		varchar(5)		NOT NULL	PRIMARY KEY,
	tanggal			date			NOT NULL,
	idProperty		varchar(5)		NOT NULL,
	harga			int				NOT NULL,
	idClient		varchar(5)		NOT NULL
);
-----------------------------------------------------------------------------------------
CREATE TABLE TJualPropertyCicil
(	idTJualPropertyCicil	varchar(5)		NOT NULL	PRIMARY KEY,
	tanggal					date			NOT NULL,
	idProperty				varchar(5)		NOT NULL,
	idClient				varchar(5)		NOT NULL,
	hargaProperty			int				NOT NULL,
	perBulan				int				NOT NULL,
	jumlahCicilan			int				NOT NULL,
	total					int				NOT NULL,
	statusBayar				varchar(5)		NOT NULL

);
-----------------------------------------------------------------------------------------
CREATE TABLE TJualPropertyCicilDet
(	idDetailCicil			varchar(5)		NOT NULL	PRIMARY KEY,
	idTJualPropertyCicil	varchar(5)		NOT NULL,
	tanggalDetail			date			NOT NULL,
	harga					int				NOT NULL
);
-----------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------


SELECT*FROM jabatan
SELECT*FROM pegawai
SELECT*FROM pemilik
SELECT*FROM client
SELECT*FROM property
SELECT*FROM propertyTipe
SELECT*FROM kategoriCicilan
SELECT*FROM desainInterior

SELECT*FROM TSewaProperty
SELECT*FROM TJualPropertyLunas
SELECT*FROM TJualPropertyCicil
SELECT*FROM TJualPropertyCicilDet

SELECT * FROM TJualPropertyCicil p INNER JOIN TJualPropertyCicilDet b ON p.idTJualPropertyCicil = b.idTJualPropertyCicil

SELECT * FROM property p INNER JOIN pemilik b ON p.idPemilik = b.idPemilik WHERE p.idProperty 
SELECT * FROM TBeliProperty p INNER JOIN property b ON p.idProperty = b.idProperty WHERE p.idTBeliProperty

-------------------------------------------------------------------------------------
----------------------------------------------------------------------------------

--//////////////MASTER///////////////////////////////////////
--===============================-
--JABATAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InputJabatan]
	@idJabatan varchar(5),
	@jabatan varchar(50)
AS
BEGIN
	INSERT INTO Jabatan
	Values (@idJabatan, @jabatan)
END
--------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateJabatan]
	@idJabatan varchar(5),
	@jabatan varchar(50)
AS
BEGIN
      UPDATE jabatan
      SET jabatan = @jabatan
      WHERE idJabatan = @idJabatan
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteJabatan]
	@idJabatan varchar(5)
AS
DELETE FROM [dbo].[jabatan]
WHERE idJabatan = @idJabatan
GO
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--PEGAWAI
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertPegawai]
	@idPegawai varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@username varchar(15),
	@password varchar(15),
	@idJabatan varchar(5)
AS
BEGIN
INSERT INTO pegawai
VALUES(@idPegawai,@nama,@jeniskelamin,@username,@password,@idJabatan)
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdatePegawai]
	@idPegawai varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@username varchar(15),
	@password varchar(15),
	@idjabatan varchar(5)
AS
BEGIN
UPDATE pegawai SET
	nama = @nama,
	jeniskelamin = @jeniskelamin,
	username = @username,
	password = @password,
	idJabatan = @idjabatan
WHERE idPegawai = @idPegawai
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeletePegawai]
@idPegawai varchar(5)
AS
BEGIN
DELETE FROM pegawai WHERE idPegawai = @idPegawai
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--PROPERTY TIPE
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertType]
	@idTipe varchar(5),
	@nama varchar(50)
AS
BEGIN
INSERT INTO propertyTipe
VALUES(@idTipe,@nama)
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateType]
	@idTipe varchar(5),
	@nama varchar(50)
AS
BEGIN
UPDATE propertyTipe SET
	nama = @nama
WHERE idTipe = @idTipe
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteType]
@idTipe varchar(5)
AS
BEGIN
DELETE FROM propertyTipe WHERE idTipe = @idTipe
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--PEMILIK
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertPemilik]
	@idPemilik varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@telepon varchar(13),
	@email varchar(50),
	@alamat varchar(100)
AS
BEGIN
INSERT INTO pemilik
VALUES(@idPemilik,@nama,@jeniskelamin,@telepon,@email,@alamat)
END
--------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdatePemilik]
	@idPemilik varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@telepon varchar(13),
	@email varchar(50),
	@alamat varchar(100)
AS
BEGIN
UPDATE pemilik SET
	nama = @nama,
	jenisKelamin = @jeniskelamin,
	telepon = @telepon,
	email = @telepon,
	alamat = @alamat
WHERE idPemilik = @idPemilik
END
------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeletePemilik]
@idPemilik varchar(5)
AS
BEGIN
DELETE FROM pemilik WHERE idPemilik = @idPemilik
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--CLIENT
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertClient]
	@idClient varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@telepon varchar(13),
	@email varchar(50),
	@alamat varchar(100)
AS
BEGIN
INSERT INTO client
VALUES(@idClient,@nama,@jeniskelamin,@telepon,@email,@alamat)
END
--------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateClient]
	@idClient varchar(5),
	@nama varchar(50),
	@jeniskelamin varchar(50),
	@telepon varchar(13),
	@email varchar(50),
	@alamat varchar(100)
AS
BEGIN
UPDATE client SET
	nama = @nama,
	jenisKelamin = @jeniskelamin,
	telepon = @telepon,
	email = @telepon,
	alamat = @alamat
WHERE idClient = @idClient
END
------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteClient]
@idClient varchar(5)
AS
BEGIN
DELETE FROM client WHERE idClient = @idClient
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--DESAIN INTERIOR
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertInterior]
	@idInterior varchar(5),
	@interior varchar(50)
AS
BEGIN
INSERT INTO desainInterior
VALUES(@idInterior,@interior)
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateInterior]
	@idInterior varchar(5),
	@interior varchar(50)
AS
BEGIN
      UPDATE desainInterior SET 
		interior = @interior
      WHERE idInterior = @idInterior
END
--------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteInterior]
@idInterior varchar(5)
AS
BEGIN
DELETE FROM desainInterior WHERE idInterior = @idInterior
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--KATEGORI CICILAN
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_InsertCicilan]
	@idCicilan varchar(5),
	@cicilan varchar(50)
AS
BEGIN
INSERT INTO kategoriCicilan
VALUES(@idCicilan,@cicilan)
END
-----------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateCicilan]
	@idCicilan varchar(5),
	@cicilan varchar(50)
AS
BEGIN
      UPDATE kategoriCicilan SET 
		cicilan = @cicilan
      WHERE idCicilan = @idCicilan
END
----------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteCicilan]
@idCicilan varchar(5)
AS
BEGIN
DELETE FROM kategoriCicilan WHERE idCicilan = @idCicilan
END
-----------------------------------------------------------------------------------
-----------------------------------------------------------------------------------
--===============================-
--PROPERTY
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InputProperty]
	@idProperty varchar(5),
	@namaProperty varchar(50),
	@idTipe varchar(5),
	@idPemilik varchar(5),
	@ukuran varchar(50),
	@fasilitas varchar(100),
	@harga int,
	@gambar image,
	@idInterior varchar(5),
	@statusProperty varchar(5) = 0
AS
BEGIN
	INSERT INTO property(idProperty, namaProperty, idTipe, idPemilik, ukuran, fasilitas, harga, gambar, idInterior, statusProperty)
	Values ( @idProperty, @namaProperty, @idTipe, @idPemilik, @ukuran, @fasilitas, @harga, @gambar, @idInterior, @statusProperty)
END
--------------------------------------------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateProperty]
	@idProperty varchar(5),
	@namaProperty varchar(50),
	@idTipe varchar(5),
	@ukuran varchar(50),
	@fasilitas varchar(100),
	@harga int,
	@gambar image,
	@idInterior varchar(5),
	@statusProperty varchar(5) = 0
AS
if(CONVERT(Varbinary,@gambar) = 0x00)
BEGIN
UPDATE property SET namaProperty = @namaProperty, idTipe = @idTipe,	idPemilik = @idPemilik, ukuran = @ukuran,
	fasilitas = @fasilitas, harga = @harga, gambar = gambar, idInterior = @idInterior, statusProperty = @statusProperty WHERE idProperty = @idProperty
END
ELSE
BEGIN
UPDATE property SET namaProperty = @namaProperty, idTipe = @idTipe,	idPemilik = @idPemilik, ukuran = @ukuran,
					fasilitas = @fasilitas, harga = @harga, gambar = @gambar, idInterior = @idInterior, statusProperty = @statusProperty WHERE idProperty = @idProperty
END
GO
-----------------------------------------------------------------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteProperty]
@idProperty varchar(5)
AS
BEGIN
DELETE FROM property WHERE idProperty = @idProperty
END
-----------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

--//////////////TRANSAKASI///////////////////////////////////////
--===============================-
--SEWA PROPERTY 
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTSewaProperty]
	@idTSewaProperty varchar(5),
	@tanggal  date,
	@idClient varchar(5),
	@idProperty varchar(5),
	@mulai date,
	@sampai date,
	@harga int,
	@statusProperty varchar(5) = 1
AS
BEGIN
	INSERT INTO TSewaProperty Values (@idTSewaProperty, @tanggal, @idClient, @idProperty, @mulai, @sampai, @harga)
	UPDATE property SET statusProperty = @statusProperty WHERE idProperty = @idProperty
END
-----------------------------------------------------------------------------------
--===============================-
--JUAL PROPERTY LUNAS 
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertTJualPropertyLunas]
	@idJPLunas varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@harga int,
	@idClient varchar(5),
	@statusProperty varchar(5) = 1
AS
BEGIN
	INSERT INTO TJualPropertyLunas Values (@idJPLunas, @tanggal, @idProperty, @harga, @idClient)
	UPDATE property SET statusProperty = @statusProperty WHERE idProperty = @idProperty
END
-----------------------------------------------------------------------------------------
--===============================-
--JUAL PROPERTY CICIL 
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_InsertTJualPropertyCicil]
	@idTJualPropertyCicil varchar(5),
	@tanggal date,
	@idProperty varchar(5),
	@idClient varchar(5),
	@hargaProperty int,
	@perBulan int,
	@jumlahCicilan int,
	@total int,
	@statusProperty varchar(5) = 1
AS
BEGIN
	INSERT INTO TJualPropertyCicil Values (@idTJualPropertyCicil, @tanggal, @idProperty, @idClient, @hargaProperty, @perBulan, @jumlahCicilan, @total,0)
	UPDATE property SET statusProperty = @statusProperty WHERE idProperty = @idProperty
END
-----------------------------------------------------------------------------------
--===============================-
--JUAL DETAIL PROPERTY CICIL 
--===============================-
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_InsertTJualPropertyCicilDet]
	@idDetailCicil varchar(5),
	@idTJualPropertyCicil varchar(5),
	@tanggalDetail date,
	@harga int,
	@jumlah int null
AS
BEGIN
	INSERT INTO TJualPropertyCicilDet Values (@idDetailCicil, @idTJualPropertyCicil, @tanggalDetail, @harga)
	UPDATE TJualPropertyCicil SET total = total- @harga, jumlahCicilan = jumlahCicilan-1 WHERE idTJualPropertyCicil = @idTJualPropertyCicil
	SELECT @jumlah = sum(jumlahCicilan) FROM TJualPropertyCicil WHERE idTJualPropertyCicil = @idTJualPropertyCicil 
	IF(@jumlah = 0)
	BEGIN
	UPDATE TJualPropertyCicil SET statusBayar = 1 WHERE idTJualPropertyCicil = @idTJualPropertyCicil 
	END
END
-----------------------------------------------------------------------------------
select*FROM TJualPropertyCicilDet