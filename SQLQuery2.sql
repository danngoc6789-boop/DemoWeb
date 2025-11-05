UPDATE [DemoWebDb].[dbo].[Products]
SET Image = '~/Content/Images/BongTai/DTJ1/1.png'
WHERE Id = 1027

UPDATE [DemoWebDb].[dbo].[Products]
SET Image = '~/Content/Images/BongTai/DTJ2/1.png'
WHERE Id = 1028
SELECT Id, Name, Image 
FROM [DemoWebDb].[dbo].[Products]
WHERE Id IN (1027, 1028)
SELECT Id, Name, Image, Images 
FROM [DemoWebDb].[dbo].[Products]
WHERE Id = 1027
SELECT Id, Name, Image, 
       CAST(Images AS NVARCHAR(MAX)) AS Images
FROM [DemoWebDb].[dbo].[Products]
WHERE Id = 1027
-- Kiểm tra xem DTJ2 có những file ảnh nào
-- Sau đó update vào database

UPDATE [DemoWebDb].[dbo].[Products]
SET Images = '~/Content/Images/BongTai/DTJ1/2.png;~/Content/Images/BongTai/DTJ1/3.jpg;~/Content/Images/BongTai/DTJ1/4.jpg;~/Content/Images/BongTai/DTJ1/5.jpg;'
WHERE Id = 1027
SELECT Id, Name, Image, 
       CAST(Images AS NVARCHAR(MAX)) AS Images
FROM [DemoWebDb].[dbo].[Products]
WHERE Id = 1028
----thêm cột mới để hover
ALTER TABLE [dbo].[Products]
ADD HoverImage NVARCHAR(500) NULL
-- Chọn ảnh nào làm hover image
UPDATE [DemoWebDb].[dbo].[Products]
SET HoverImage = '~/Content/Images/BongTai/DTJ2/2.jpg'
WHERE Id = 1027
UPDATE [DemoWebDb].[dbo].[Products]
SET Image = '~/Content/Images/BongTai/DTJ1/1.png',
    Images = '~/Content/Images/BongTai/DTJ1/1.png;~/Content/Images/BongTai/DTJ1/2.jpg;~/Content/Images/BongTai/DTJ1/3.jpg;~/Content/Images/BongTai/DTJ1/4.jpg;~/Content/Images/BongTai/DTJ1/5.png;'
WHERE Id = 1027
------thêm sản phẩm
INSERT INTO [DemoWebDb].[dbo].[Products] 
    (Name, Description, Price, Image, Images, Type, Category, CreatedDate)
VALUES 
    (N'Bông tai vàng trắng đính đá Saphia ', N'Bông tai thiết kế sang trọng', 39000000, 
     '~/Content/Images/BongTai/DTJ3/1.png', 
     '~/Content/Images/BongTai/DTJ3/2.png;~/Content/Images/BongTai/DTJ3/3.jpg;~/Content/Images/BongTai/DTJ3/4.jpg;~/Content/Images/BongTai/DTJ3/5.jpg;', 
     N'Vàng trắng ', N'Bông Tai', GETDATE()),
     
    (N'Bông tai vàng trắng 24K', N'Bông tai vàng trắng 24K', 8500000, 
     '~/Content/Images/BongTai/DTJ4/1.png', 
     '~/Content/Images/BongTai/DTJ4/2.png;~/Content/Images/BongTai/DTJ4/3.jpg;', 
     N'Vàng trắng', N'Bông tai', GETDATE()),
     
    (N'Bông tai vàng đính ngọc trai tự nhiên 24K', N'Bông tai vàng 24K cao cấp', 23000000, 
     '~/Content/Images/BongTai/DTJ5/1.png', 
     '~/Content/Images/BongTai/DTJ5/2.png;~/Content/Images/BongTai/DTJ5/3.jpg;~/Content/Images/BongTai/DTJ5/4.jpg;~/Content/Images/BongTai/DTJ5/5.jpg;', 
     N'Vàng 24K', N'Bông tai', GETDATE()),

	 (N'Bông tai vàng trắng đính kim cương tự nhiên', N'Bông tai đính kim cương tự nhiên', 129000000, 
     '~/Content/Images/BongTai/DTJ6/1.png', 
     '~/Content/Images/BongTai/DTJ6/2.png;~/Content/Images/BongTai/DTJ6/3.jpg;~/Content/Images/BongTai/DTJ6/4.jpg;~/Content/Images/BongTai/DTJ6/5.jpg;', 
     N'Vàng trắng', N'Bông tai', GETDATE()),

	 (N'Bông tai đính kim cương vàng trắng', N'Bông tai vàng trắng cao cấp', 163000000, 
     '~/Content/Images/BongTai/DTJ7/1.png', 
     '~/Content/Images/BongTai/DTJ7/2.png;~/Content/Images/BongTai/DTJ7/3.jpg', 
     N'Vàng 24K', N'Bông tai', GETDATE()),

	 (N'Bông tai kim cương vàng trắng 14K', N'Bông tai vàng 14K cao cấp', 99000000, 
     '~/Content/Images/BongTai/DTJ8/1.png', 
     '~/Content/Images/BongTai/DTJ8/2.png;~/Content/Images/BongTai/DTJ8/3.jpg;~/Content/Images/BongTai/DTJ8/4.jpg;~/Content/Images/BongTai/DTJ8/5.jpg;', 
     N'Vàng 14K', N'Bông tai', GETDATE()),

	 (N'Bông tai bạc đính đá ', N'Bông tai bạc cao cấp', 9000000, 
     '~/Content/Images/BongTai/DTJ9/1.png', 
     '~/Content/Images/BongTai/DTJ9/2.png;~/Content/Images/BongTai/DTJ9/3.jpg;~/Content/Images/BongTai/DTJ9/4.jpg;~/Content/Images/BongTai/DTJ9/5.jpg;', 
     N'Bạc', N'Bông tai', GETDATE()),

	 (N'Bông tai Kim Cương Vàng trắng 24K', N'Bông tai vàng 24K cao cấp', 116000000, 
     '~/Content/Images/BongTai/DTJ10/1.png', 
     '~/Content/Images/BongTai/DTJ10/2.png;~/Content/Images/BongTai/DTJ10/3.jpg;~/Content/Images/BongTai/DTJ10/4.jpg;~/Content/Images/BongTai/DTJ10/5.jpg;', 
     N'Vàng 24K', N'Bông tai', GETDATE())
INSERT INTO [DemoWebDb].[dbo].[Products] 
    (Name, Description, Price, Image, Images, Type, Category, CreatedDate)
VALUES 

	 (N'Charm Mặt Yêu Thích', N'Charm vàng 24K cao cấp', 1600000, 
     '~/Content/Images/Charm/1/1.png', 
     '~/Content/Images/Charm/1/2.png;~/Content/Images/Charm/1/3.png;~/Content/Images/Charm/1/4.jpg;~/Content/Images/Charm/1/5.jpg;', 
     N'Vàng 24K', N'Charm', GETDATE()),

	 (N'Hạt Charm đính đá', N'Charm bạc cao cấp', 460000, 
     '~/Content/Images/Charm/2/1.png', 
     '~/Content/Images/Charm/2/2.png;~/Content/Images/Charm/2/3.png;~/Content/Images/Charm/2/4.jpg;~/Content/Images/Charm/2/5.jpg;', 
     N'Bạc', N'Charm', GETDATE()),

	 (N'Hạt Charm Đính Đá Vàng', N'Charm bạc cao cấp', 565000, 
     '~/Content/Images/Charm/3/1.png', 
     '~/Content/Images/Charm/3/2.png;~/Content/Images/Charm/3/3.png;~/Content/Images/Charm/3/4.jpg;~/Content/Images/Charm/3/5.jpg;~/Content/Images/Charm/3/6.jpg;', 
     N'Bạc', N'Charm', GETDATE()),

	 (N'Hạt Charm Đính Đá Saphia Xanh Lá', N'Charm vàng cao cấp', 699000, 
     '~/Content/Images/Charm/4/1.png', 
     '~/Content/Images/Charm/4/2.png;~/Content/Images/Charm/4/3.png;~/Content/Images/Charm/4/4.jpg;~/Content/Images/Charm/4/5.jpg;~/Content/Images/Charm/4/6.jpg;', 
     N'Vàng 18k', N'Charm', GETDATE()),

	 (N'Hạt Charm Đính Đá Cỏ Bốn Lá', N'Charm bạc cao cấp', 499000, 
     '~/Content/Images/Charm/5/1.png', 
     '~/Content/Images/Charm/5/2.png;~/Content/Images/Charm/5/3.png;~/Content/Images/Charm/5/4.jpg;', 
     N'Bạc', N'Charm', GETDATE()),

	 (N'Dây Chuyền Vàng Trắng Mắt Xích  Nam 24K', N'Dây Chuyền Vàng Trắng 24K', 2990000, 
     '~/Content/Images/DayChuyen/N1/1.png', 
     '~/Content/Images/DayChuyen/N1/2.png;~/Content/Images/DayChuyen/N1/3.png;~/Content/Images/DayChuyen/N1/4.jpg;~/Content/Images/DayChuyen/N1/5.jpg;~/Content/Images/DayChuyen/N1/6.jpg;', 
     N'Vàng Trắng 24K', N'Dây Chuyền', GETDATE()),
	 
	 (N'Dây Chuyền Vàng Nam 24K', N'Dây Chuyền Vàng 24K', 2399000, 
     '~/Content/Images/DayChuyen/N2/1.png', 
     '~/Content/Images/DayChuyen/N2/2.png;~/Content/Images/DayChuyen/N2/3.png;~/Content/Images/DayChuyen/N2/4.jpg;~/Content/Images/DayChuyen/N2/5.jpg;~/Content/Images/DayChuyen/N2/6.jpg;', 
     N'Vàng 24K', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Bạc Chơn Nam', N'Dây Chuyền Bạc Cao Cấp', 1399000, 
     '~/Content/Images/DayChuyen/N3/1.png', 
     '~/Content/Images/DayChuyen/N3/2.png;~/Content/Images/DayChuyen/N3/3.png;~/Content/Images/DayChuyen/N3/4.jpg;', 
     N'Bạc', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Bạc Chơn Nữ', N'Dây Chuyền Bạc Cao Cấp', 1399000, 
     '~/Content/Images/DayChuyen/Nu1/1.png', 
     '~/Content/Images/DayChuyen/Nu1/2.png;~/Content/Images/DayChuyen/Nu1/3.png;~/Content/Images/DayChuyen/Nu1/4.jpg;', 
     N'Bạc', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Vàng Trắng Mắt Xích Nữ 14K', N'Dây Chuyền Vàng Trắng 14K', 2199000, 
     '~/Content/Images/DayChuyen/Nu2/1.png', 
     '~/Content/Images/DayChuyen/Nu2/2.png;~/Content/Images/DayChuyen/Nu2/3.png;~/Content/Images/DayChuyen/Nu2/4.jpg;~/Content/Images/DayChuyen/Nu2/5.jpg;~/Content/Images/DayChuyen/Nu2/6.jpg;', 
     N'Vàng Trắng 14K', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Vàng Trắng Nữ 18K', N'Dây Chuyền Vàng Trắng 18K', 2299000, 
     '~/Content/Images/DayChuyen/Nu3/1.png', 
     '~/Content/Images/DayChuyen/Nu3/2.png;~/Content/Images/DayChuyen/Nu3/3.png;~/Content/Images/DayChuyen/Nu3/4.jpg;~/Content/Images/DayChuyen/Nu3/5.jpg;~/Content/Images/DayChuyen/Nu3/6.jpg;', 
     N'Vàng Trắng 18K', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Bạc Xoáy Nữ', N'Dây Chuyền Bạc Cao Cấp', 1199000, 
     '~/Content/Images/DayChuyen/Nu4/1.png', 
     '~/Content/Images/DayChuyen/Nu4/2.png;~/Content/Images/DayChuyen/Nu4/3.png;~/Content/Images/DayChuyen/Nu4/4.jpg;~/Content/Images/DayChuyen/Nu4/5.jpg;~/Content/Images/DayChuyen/Nu4/6.jpg;', 
     N'Bạc', N'Dây Chuyền', GETDATE()),

	 
	 (N'Dây Chuyền Vàng Trắng Xoáy Nữ 14K', N'Dây Chuyền Vàng Trắng 14K', 2199000, 
     '~/Content/Images/DayChuyen/Nu5/1.png', 
     '~/Content/Images/DayChuyen/Nu5/2.png;~/Content/Images/DayChuyen/Nu5/3.png;~/Content/Images/DayChuyen/Nu5/4.jpg;', 
     N'Vàng Trắng 14K', N'Dây Chuyền', GETDATE()),

	 (N'Dây Chuyền Vàng Trắng Chơn Unisex 14K', N'Dây Chuyền Vàng Trắng 14K', 2199000, 
     '~/Content/Images/DayChuyen/Unisex/1.png', 
     '~/Content/Images/DayChuyen/Unisex/2.png;~/Content/Images/DayChuyen/Unisex/3.png;~/Content/Images/DayChuyen/Unisex/4.jpg;', 
     N'Vàng Trắng 14K', N'Dây Chuyền', GETDATE()),

	 (N'Kiềng Vàng Trắng 24K', N'Kiềng Vàng Trắng 24K', 2499000, 
     '~/Content/Images/Kieng/1/1.png', 
     '~/Content/Images/Kieng/1/2.png;~/Content/Images/Kieng/1/3.png;~/Content/Images/Kieng/1/4.jpg;', 
     N'Vàng Trắng 24K', N'Kiềng', GETDATE()),

	 (N'Kiềng Cưới Vàng Trắng Lá Ngọc Cành Vàng 24K', N'Kiềng Vàng Trắng 24K', 12499000, 
     '~/Content/Images/Kieng/2/1.png', 
     '~/Content/Images/Kieng/2/2.png;~/Content/Images/Kieng/2/3.jpg;~/Content/Images/Kieng/2/4.jpg;~/Content/Images/Kieng/2/5.jpg;~/Content/Images/Kieng/2/6.jpg;', 
     N'Vàng Trắng 24K', N'Kiềng', GETDATE()),

	 (N'Kiềng Cưới Vàng  24K', N'Kiềng Vàng  24K', 8999000, 
     '~/Content/Images/Kieng/3/1.png', 
     '~/Content/Images/Kieng/3/2.png;~/Content/Images/Kieng/3/3.png;~/Content/Images/Kieng/3/4.jpg;', 
     N'Vàng 24K', N'Kiềng', GETDATE()),

	 (N'Kiềng Vàng Trắng 14K', N'Kiềng Vàng Trắng 14K', 1899000, 
     '~/Content/Images/Kieng/4/1.png', 
     '~/Content/Images/Kieng/4/2.png;~/Content/Images/Kieng/4/3.jpg;', 
     N'Vàng Trắng 14K', N'Kiềng', GETDATE()),

	 (N'Kiềng Cưới Vàng Lá Ngọc Cành Vàng 24K', N'Kiềng Trắng 24K', 13499000, 
     '~/Content/Images/Kieng/5/1.png', 
     '~/Content/Images/Kieng/5/2.png;~/Content/Images/Kieng/5/3.npg;~/Content/Images/Kieng/5/4.jpg;', 
     N'Vàng 24K', N'Kiềng', GETDATE())

UPDATE Products 
SET Name = N'Dây Chuyền Vàng Mắt Xích 24K Nam',
    Description = N'Dây Chuyền Vàng 24K',
    Price = 2990000
WHERE Id = 1042;

UPDATE Products 
SET Name = N'Kiềng Cưới Vàng Lá Ngọc Cành Vàng 24k',
    Description = N'Kiềng Vàng 24K',
    Price = 13499000
WHERE Id = 1055;
SELECT * FROM PRODUCTS

	 