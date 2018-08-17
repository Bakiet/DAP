-- phpMyAdmin SQL Dump
-- version 3.5.1
-- http://www.phpmyadmin.net
--
-- Servidor: localhost
-- Tiempo de generación: 04-08-2018 a las 21:01:40
-- Versión del servidor: 5.5.24-log
-- Versión de PHP: 5.4.3

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de datos: `dap`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `events`
--

CREATE TABLE IF NOT EXISTS `events` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Subject` varchar(100) NOT NULL,
  `Description` varchar(200) NOT NULL,
  `Start` datetime NOT NULL,
  `End` datetime DEFAULT NULL,
  `ThemeColor` varchar(10) DEFAULT NULL,
  `IsFullDay` bit(64) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=7 ;

--
-- Volcado de datos para la tabla `events`
--

INSERT INTO `events` (`Id`, `Subject`, `Description`, `Start`, `End`, `ThemeColor`, `IsFullDay`) VALUES
(1, 'Reunión', 'Reunión con bidyut adak', '2018-08-04 00:00:00', NULL, 'red', b'0011000000000000000000000000000000000000000000000000000000000000'),
(2, 'Instalación de ascensor ', 'Cliente Google', '2018-08-08 00:00:00', NULL, 'green', b'0011000100000000000000000000000000000000000000000000000000000000'),
(4, 'fallas 7888', 'prueba', '2018-08-02 00:00:00', '2018-08-03 06:00:00', 'blue', b'0011000000000000000000000000000000000000000000000000000000000000');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
