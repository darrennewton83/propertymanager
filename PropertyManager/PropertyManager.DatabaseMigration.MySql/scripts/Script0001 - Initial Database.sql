-- -----------------------------------------------------
-- Table `propertymanager`.`propertytype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `propertymanager`.`propertytype` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(250) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 29
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

CREATE UNIQUE INDEX `name_UNIQUE` ON `propertymanager`.`propertytype` (`name` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `propertymanager`.`propertyoverview`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `propertymanager`.`propertyoverview` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `type` INT NULL DEFAULT NULL,
  `purchase_price` DECIMAL(10,2) NULL DEFAULT NULL,
  `purchase_date` DATE NULL DEFAULT NULL,
  `garage` TINYINT(1) NULL DEFAULT '0',
  `parking_spaces` TINYINT UNSIGNED NULL DEFAULT NULL,
  `notes` LONGTEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_property_propertytype`
    FOREIGN KEY (`type`)
    REFERENCES `propertymanager`.`propertytype` (`id`))
ENGINE = InnoDB
AUTO_INCREMENT = 30
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

CREATE INDEX `id_idx` ON `propertymanager`.`propertyoverview` (`type` ASC) VISIBLE;


-- -----------------------------------------------------
-- Table `propertymanager`.`propertyaddress`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `propertymanager`.`propertyaddress` (
  `id` INT NOT NULL,
  `line1` VARCHAR(250) CHARACTER SET 'utf8mb3' NOT NULL,
  `line2` VARCHAR(25) CHARACTER SET 'utf8mb3' NULL DEFAULT NULL,
  `city` VARCHAR(250) CHARACTER SET 'utf8mb3' NOT NULL,
  `region` VARCHAR(250) CHARACTER SET 'utf8mb3' NULL DEFAULT NULL,
  `postcode` VARCHAR(50) CHARACTER SET 'utf8mb3' NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_propertyaddress_property`
    FOREIGN KEY (`id`)
    REFERENCES `propertymanager`.`propertyoverview` (`id`)
    ON DELETE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;