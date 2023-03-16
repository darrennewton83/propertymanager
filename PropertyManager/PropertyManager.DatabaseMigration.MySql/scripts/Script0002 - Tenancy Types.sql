CREATE TABLE IF NOT EXISTS `propertymanager`.`tenancytype` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` NVARCHAR(250) NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC) VISIBLE);