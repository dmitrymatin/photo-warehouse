-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: photowarehousedb
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20210623120316_Initial','5.0.7'),('20210627100118_UpdateCategoriesDeleteBehaviour','5.0.7');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `applicationuserphotoitem`
--

LOCK TABLES `applicationuserphotoitem` WRITE;
/*!40000 ALTER TABLE `applicationuserphotoitem` DISABLE KEYS */;
INSERT INTO `applicationuserphotoitem` VALUES ('c7d7c9d3-ccd9-4b3d-9936-19a9b50b862e',6),('c7d7c9d3-ccd9-4b3d-9936-19a9b50b862e',12);
/*!40000 ALTER TABLE `applicationuserphotoitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('416dc9da-855a-420d-9d32-1055226efca2','Client','CLIENT','591ee98e-ee45-4e36-a84f-e78b560c10a1'),('4d85f4f4-22f0-42c0-a7ff-f26baa72616c','Administrator','ADMINISTRATOR','182cc4ae-9166-4546-87af-dbf9956de97a');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('598c26aa-8fc7-4f0a-893f-8d0db3bb5855','416dc9da-855a-420d-9d32-1055226efca2'),('c7d7c9d3-ccd9-4b3d-9936-19a9b50b862e','416dc9da-855a-420d-9d32-1055226efca2'),('387519da-f975-4ad6-a1bc-fc66e9482902','4d85f4f4-22f0-42c0-a7ff-f26baa72616c');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('387519da-f975-4ad6-a1bc-fc66e9482902','2021-06-27 23:27:56',NULL,'administrator','ADMINISTRATOR','admin@pw.com','ADMIN@PW.COM',1,'AQAAAAEAACcQAAAAED+uyJl8bOw0zfjO2yoIil79sUBPBO9vJLMJeYHadUA2BTpMFrgoz6ZmJmyUDT5WSA==','5ZINI2XZWPQEQ3SKDSZLPXBYNT72CDLT','81a0725e-e626-46e0-aacd-33dffe88ae85',NULL,0,0,NULL,1,0),('598c26aa-8fc7-4f0a-893f-8d0db3bb5855','2021-06-28 00:20:24',NULL,'clientUser1','CLIENTUSER1','123@example.com','123@EXAMPLE.COM',0,'AQAAAAEAACcQAAAAEPUCtkudWhuckNsvzGY41JDydSrCK90IfNO0xarVsghgO8EsNKPfO/Ci+NuGClDXrw==','O2CTSS624IBWMAMV54N2LZR4Z4APTHOS','1a310b7c-bdea-4e6b-9695-d79052a6564d',NULL,0,0,NULL,1,0),('c7d7c9d3-ccd9-4b3d-9936-19a9b50b862e','2021-06-28 00:22:52',NULL,'clientUser11','CLIENTUSER11','123@example.com','123@EXAMPLE.COM',0,'AQAAAAEAACcQAAAAELRhTIJ7BN0RYjiLl0cjRwiFosBDa5IJO0iG29Q1AuLUOu7I0Q1eUX+QvlKW2ZY1hg==','GALIDGZRKRIVIQUT7XSSAJI7SHN6OUKS','888b3f3d-33bc-46ee-b303-e69082fe1c36',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `feedbacks`
--

LOCK TABLES `feedbacks` WRITE;
/*!40000 ALTER TABLE `feedbacks` DISABLE KEYS */;
/*!40000 ALTER TABLE `feedbacks` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `fileformats`
--

LOCK TABLES `fileformats` WRITE;
/*!40000 ALTER TABLE `fileformats` DISABLE KEYS */;
INSERT INTO `fileformats` VALUES (1,'.jpg');
/*!40000 ALTER TABLE `fileformats` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `orderphotoitem`
--

LOCK TABLES `orderphotoitem` WRITE;
/*!40000 ALTER TABLE `orderphotoitem` DISABLE KEYS */;
INSERT INTO `orderphotoitem` VALUES (6,1),(12,1);
/*!40000 ALTER TABLE `orderphotoitem` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (1,'2021-06-28 00:46:39',NULL,'c7d7c9d3-ccd9-4b3d-9936-19a9b50b862e');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `orderstatuses`
--

LOCK TABLES `orderstatuses` WRITE;
/*!40000 ALTER TABLE `orderstatuses` DISABLE KEYS */;
/*!40000 ALTER TABLE `orderstatuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `photocategories`
--

LOCK TABLES `photocategories` WRITE;
/*!40000 ALTER TABLE `photocategories` DISABLE KEYS */;
INSERT INTO `photocategories` VALUES (2,'Города'),(3,'Природа');
/*!40000 ALTER TABLE `photocategories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `photoitems`
--

LOCK TABLES `photoitems` WRITE;
/*!40000 ALTER TABLE `photoitems` DISABLE KEYS */;
INSERT INTO `photoitems` VALUES (1,'2021-06-27 23:50:50','83231cb9-3411-403c-8dc6-7a5a57ddd682.jpg',1,1,1),(2,'2021-06-27 23:51:48','ecba7943-5d77-441b-b5e0-79c1b3b500bb.jpg',2,2,1),(3,'2021-06-27 23:53:17','d17515f6-ab4d-40a1-97ef-a1fa5fe4ed6b.jpg',3,3,1),(4,'2021-06-27 23:56:17','b446a0cc-c722-44f0-a313-06bea9b56e37.jpg',4,4,1),(5,'2021-06-27 23:56:17','c71947fc-8522-4b1d-8018-f3206d42374e.jpg',5,5,1),(6,'2021-06-28 00:06:44','b446a0cc-c722-44f0-a313-06bea9b56e37(1).jpg',4,6,1),(7,'2021-06-28 00:08:10','3f7be27e-bf9c-4af8-b403-b260adb9f030.jpg',6,7,1),(8,'2021-06-28 00:08:11','9f2ffaf7-8bc0-4052-942b-c01f206cd717.jpg',7,8,1),(9,'2021-06-28 00:08:11','e2b281a0-2321-4584-9e78-8857d430b764.jpg',8,9,1),(10,'2021-06-28 00:10:44','a8c9c4a4-e84a-404d-a4a7-0ddd230dae86.jpg',9,10,1),(11,'2021-06-28 00:10:45','15e332b4-6302-40c7-b35f-712b06963111.jpg',10,11,1),(12,'2021-06-28 00:10:45','a3515518-d959-40c5-9106-08f236a4a4aa.jpg',11,12,1),(13,'2021-06-28 00:10:46','8b2334e0-6b9b-404e-b6b3-e5bda0f887c7.jpg',12,13,1);
/*!40000 ALTER TABLE `photoitems` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `photos`
--

LOCK TABLES `photos` WRITE;
/*!40000 ALTER TABLE `photos` DISABLE KEYS */;
INSERT INTO `photos` VALUES (1,'Лондон','Вид на Биг-Бен',0,1,'2021-06-27 23:50:49','2021-05-31 21:00:00',2),(2,'Лондон','Вид на мост и Биг-Бен',0,0,'2021-06-27 23:51:48','2021-05-31 21:51:00',2),(3,'Пейзаж в Швейцарии',NULL,0,0,'2021-06-27 23:53:16','2021-06-02 20:53:00',3),(4,'Нью-Йорк','Улицы Нью-Йорка',0,2,'2021-06-27 23:56:17','2021-06-03 18:56:00',2),(5,'Нью-Йорк','Небоскрёбы Нью-Йорка',0,0,'2021-06-27 23:56:17','2021-06-03 18:56:00',2),(6,'Санкт-Петербург','Развод мостов ночью',0,0,'2021-06-28 00:08:10','2021-06-10 00:07:00',2),(7,'Санкт-Петербург','Дворцовая площадь',0,0,'2021-06-28 00:08:11','2021-06-10 00:07:00',2),(8,'Париж',NULL,0,0,'2021-06-28 00:08:11','2021-06-10 00:07:00',2),(9,'Пейзаж в Новой Зеландии',NULL,0,0,'2021-06-28 00:10:44','2021-06-05 00:10:00',3),(10,'Озеро Байкал в России',NULL,0,0,'2021-06-28 00:10:45','2021-06-05 00:10:00',3),(11,'Озеро Морейн в Канаде',NULL,0,3,'2021-06-28 00:10:45','2021-06-05 00:10:00',3),(12,'Дом в горах',NULL,0,0,'2021-06-28 00:10:46','2021-06-05 00:10:00',3);
/*!40000 ALTER TABLE `photos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `photosizes`
--

LOCK TABLES `photosizes` WRITE;
/*!40000 ALTER TABLE `photosizes` DISABLE KEYS */;
INSERT INTO `photosizes` VALUES (1,2848.00,4272.00),(2,1999.00,1376.00),(3,5378.00,3585.00),(4,2522.00,3363.00),(5,3425.00,5184.00),(6,2399.00,2234.00),(7,4892.00,2348.00),(8,4038.00,6057.00),(9,3708.00,5562.00),(10,5184.00,3456.00),(11,3328.00,5916.00),(12,5407.00,3605.00),(13,3456.00,5184.00);
/*!40000 ALTER TABLE `photosizes` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-28  4:18:11
