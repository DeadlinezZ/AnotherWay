-- phpMyAdmin SQL Dump
-- version 4.0.4.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 30. Jul 2014 um 14:40
-- Server Version: 5.6.11
-- PHP-Version: 5.5.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `anotherway`
--
CREATE DATABASE IF NOT EXISTS `anotherway` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `anotherway`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `friends`
--

CREATE TABLE IF NOT EXISTS `friends` (
  `id_f` int(13) NOT NULL AUTO_INCREMENT,
  `u1_id` int(11) NOT NULL,
  `u2_id` int(11) NOT NULL,
  PRIMARY KEY (`id_f`),
  KEY `u1_id` (`u1_id`),
  KEY `u2_id` (`u2_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id_u` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(20) NOT NULL,
  `password` varchar(32) NOT NULL,
  `email` varchar(100) NOT NULL,
  `exp` int(11) NOT NULL DEFAULT '0',
  `ap` int(11) NOT NULL DEFAULT '0',
  `coins` int(11) NOT NULL DEFAULT '0',
  `level` int(3) NOT NULL DEFAULT '0',
  `admin` int(1) NOT NULL DEFAULT '0',
  `gm` int(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id_u`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;

--
-- Daten für Tabelle `users`
--



--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `friends`
--
ALTER TABLE `friends`
  ADD CONSTRAINT `friends_ibfk_1` FOREIGN KEY (`u1_id`) REFERENCES `users` (`id_u`),
  ADD CONSTRAINT `friends_ibfk_2` FOREIGN KEY (`u2_id`) REFERENCES `users` (`id_u`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
