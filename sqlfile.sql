-- -- Load category

-- -- INSERT INTO category("CategoryName") VALUES ('Electronics');
-- -- INSERT INTO category("CategoryName") VALUES ('Mobile Phones');
-- -- INSERT INTO category("CategoryName") VALUES ('Laptops and Tablets');
-- -- INSERT INTO category("CategoryName") VALUES ('Shoes');
-- -- INSERT INTO category("CategoryName") VALUES ('Men Cloth');
-- -- INSERT INTO category("CategoryName") VALUES ('Construction Material');
-- -- INSERT INTO category("CategoryName") VALUES ('Women Cloth');
-- -- INSERT INTO category("CategoryName") VALUES ('Cosmetics');
-- -- INSERT INTO category("CategoryName") VALUES ('Traditional Cloths');
-- -- INSERT INTO category("CategoryName") VALUES ('Furniture');
-- -- INSERT INTO category("CategoryName") VALUES ('Boutique');

-- --users
-- --oroginal password
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Niko','Niko@gmail.com','Niko123','+25112435667','admin');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Nati','Nati@gmail.com','Nati123','+25134435667','user');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Sura','Sura@gmail.com','Sura123','+25112655667','user');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Kidcore','Kid@gmail.com','Kidus123','+25112435623','user');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Shemsu','Shem@gmail.com','Shems123','+25156435667','user');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Chachse','Chacha@gmail.com','Chacho123','+25112890667','user');
-- -- INSERT INTO users("Username","Email","Password","Phone","Role") VALUES ('Linge','Linge@gmail.com','Linge123','+25112456312','user');

--  --hashed password
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Niko','Niko@gmail.com','$2y$12$MHjsInAWVs6e9srMIe8.berYATsit0AMg6svyV9jp34kkR3eDmRUu','+25112435667','admin');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Nati','Nati@gmail.com','$2y$12$Lon4fjpFVKy.6nJ8l7ZE.ObkngyG46Aj7drKg7m5A41D/haSaOfaC','+25134435667','user');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Sura','Sura@gmail.com','$2y$12$iOo/h81pnUFPf41bxqgCXu4J1Y8VZpIf1NSL8d343lnAgTrksc9MS','+25112655667','user');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Kidcore','Kid@gmail.com','$2y$12$p54nuOFAfTftrJv4Kal8zOUgw/bdVDuSNTnYQvZAwkx68BVa2W6mK','+25112435623','user');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Shemsu','Shem@gmail.com','$2y$12$Bsfx621CbAx9VIzYGIoZnujP9tSjIQGHy6reumXciqk/ah/.D85Zm','+25156435667','user');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Chachse','Chacha@gmail.com','$2y$12$ICZ8Y72ODlSptfY1rgsyze7ajrwLRPpA7kWUlRT6j9XTrwANtPT6S','+25112890667','user');
-- -- INSERT INTO users("DateAccountCreated","Username","Email","Password","Phone","Role") VALUES ('2021-05-30','Linge','Linge@gmail.com','$2y$12$vsZuaBf1y8tU5mnaFkq/ouSEjdVZQxmPkXHivbtM6rr0xhVH3i9sq','+25112456312','user');

-- -- location

-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (8.994868002864457,38.78508564085951,'URAEL, around Saro-Maria hotel');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (8.980571601619761,38.76097945731203,'KERA, 22 mazoriya around 22 mazoria speciality clinic');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (9.033778206262228,38.76094726784849,'4 Kilo, around Birhanna Selam Printing Press');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (8.996809996711518,38.78638116784843,'Bole, next to Edna Mall');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (9.016457187391918,38.782980981429155,'Kazanchis, around roha apartment');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (9.005255593544991,38.78851679668357,'22 Addis Ababa , New stadium cross , on the way to Zerihun building, road');
-- -- INSERT INTO location("Longitude","Latitude","LocationDescription") VALUES (9.033904302023592,38.76147516784841,'4 kilo, Awash international bank 4 kilo branch');


-- --shops
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (true,'2','Sammy Handmade','Lucky Building',+251115539776,9,1,'We deliver traditional handmade products');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (false,'2','Legend Electrical Excellence','Unknown',+251911221932,1,2,'We deliver brand and original electronics products with affordable price');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (true,'3','YAKAMOZ Men''s Wear','Lucky Building',+251936516125,5,3,'We deliver comfortable, long live and brand original men''s cloths.');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (true,'4','Naty Computer and Laptop store','Morning Star Mall',+251115639776,3,4,'We deliver brand and original laptops and tablets with affordable price, there is a discount for campus students.');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (false,'5','HM Furniture','Unknown',+251911244483,10,5,'We deliver original , comfortable dubai made furniture');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (true,'5','J Cosmetics','Zerihun',+251930071231,8,6,'We offer the perfect natural Himalayan cosmetic products at an affordable price.');
-- -- INSERT INTO shops("IsActive","OwnerId","ShopName","BuildingName","PhoneNumber","CategoryId","ShopLocationId","Description") VALUES (true,'4','Lili Brand Shoe','Lucky Building',+251115539776,7,1,'We deliver Rubber sole,Platform measures ,approximately Textile and leather shoe.');

-- --products
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Lacoste','2021-04-29T19:53:58.888Z',1,'Vans Men''s Low-Top Sneakers','Rubber sole,Platform measures ,approximately Textile',4,100,'NEW','users/product.img.png','true',5,15);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Ambesa','2021-03-29T19:53:58.888Z',1,'Free People Gold Rush Mini','100% Polyester,Imported,Self Tie closure,Hand Wash Only',7,150,'NEW','users/gol.img.png','true',5,150);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Apple','2021-02-29T19:53:58.888Z',2,'New Apple iPhone 12 Pro Max','128GB, Graphite',2,100,'Slightly Used','users/phone.img.png','false',0,0);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Adidas','2021-02-29T19:53:58.888Z',4,'Adidas Men''s Low-Top Sneakers','Rubber sole,Platform measures ,approximately Textile',4,80,'NEW','users/product.img.png','true',0,15);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Microsoft','2021-05-29T19:53:58.888Z',1,'New Windows Tablet','1GB, Graphite',3,300,'Used','users/product.img.png','false',5,0);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('HP','2021-05-29T19:13:58.888Z',3,'HP ProBook Lap Top','1GB storage, 8Gb RAM, 14inch screen',1,400,'slightly used','users/product.img.png','true',15,15);
-- -- INSERT INTO products("Brand","DateInserted","ShopId","ProductName","Description","CategoryId","Price","Condition","ImageUrl","DeliveryAvailable","Discount","ProductCount") VALUES ('Levis','2021-10-29T19:43:58.888Z',5,'Ejetebab','100% cotton, white, wash by hands',9,700,'NEW','users/product.img.png','true',0,15);

-- -- Insert into reviews

-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (1,3,5,'Amazing and modern shoe','2021-05-29T19:53:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (2,3,4,'A dress that fits my wife','2020-05-29T19:53:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (3,4,2,'Not bad','2021-02-29T19:53:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (5,6,0,'I do not like such brands they smell fast','2021-10-29T19:53:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (4,5,5,'I like those phones but no money no funny. I can not buy it','2021-05-29T19:51:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (6,5,3,'Pc''s of the time. Nice product','2020-07-29T19:53:58.888Z');
-- -- INSERT INTO reviews("UserId","ProductId","Rating","Comment","ReviewDate") VALUES (3,1,1,'I like it. If I were get married, I would buy it','2021-05-29T19:22:58.888Z');


